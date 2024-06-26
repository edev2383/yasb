﻿using System;
using StockBox.Data.SbFrames;
using StockBox.Interpreter.Expressions;
using StockBox.Base.Tokens;
using StockBox.Validation;
using static StockBox.Base.Tokens.TokenType;
using StockBox.Associations.Enums;
using StockBox.Associations;
using System.Collections.Generic;
using StockBox.Interpreter.Statements;
using sbenv = StockBox.Interpreter.Environments;

namespace StockBox.Interpreter
{

    /// <summary>
    /// Recursively calculate the provided expression
    /// </summary>
    public class SbInterpreter : IVisitor, IStatementVisitor, IValidationResultsListProvider
    {

        protected sbenv.Environment Environment = new sbenv.Environment();

        private readonly SbFrameList _frames;
        private readonly IPosition _position;

        public ValidationResultList Results { get { return _results; } }
        private readonly ValidationResultList _results = new ValidationResultList();
        private readonly ValidationResultList _exceptions = new ValidationResultList();

        public SbInterpreter(SbFrameList frames, IPosition position = null)
        {
            _frames = frames;
            _position = position;
        }

        public SbInterpreter(SbFrameList frames)
        {
            _frames = frames;
        }

        public SbInterpreter() { }


        public void InterpretStatements(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                    Execute(statement);
            }
            catch (Exception e)
            {
                _exceptions.AddFromBool(false, e.Message);
            }
        }

        protected void Execute(Stmt statement)
        {
            statement.Accept(this);
        }

        /// <summary>
        /// Begin the recursive evaluation
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object Interpret(Expr expression)
        {
            try
            {
                return evaluate(expression);
            }
            catch (Exception e)
            {
                _exceptions.AddFromBool(false, e.Message);
                // this is a bandaid solution. If ANYTHING fails downstream, we
                // catch all exceptions and just return false. This will allow
                // the interpreter to continue evaluating. In the future we need
                // to come up with a more comprehensive, error-logging/handling
                // way to handle this behavior
                return false;
            }
        }

        /// <summary>
        /// Returns the aggregated results list
        /// </summary>
        /// <returns></returns>
        public ValidationResultList GetResults()
        {
            return _results;
        }

        /// <summary>
        /// A Domain Expression is structured as (Index) (Operator) (Column)
        /// Default is (ZERO) (DAYS AGO) (Column) which would return the current
        /// column value of the daily frame
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitDomainExpr(DomainExpr expr)
        {
            object ret = null;

            object indexFomZero = evaluate(expr.Left);
            _results.AddRange(ValidateDomainIndex(indexFomZero));
            _results.AddRange(ValidateDomainOperator(expr.Operator));

            EFrequency frequency = MapFrequencyFromToken(expr.Operator);
            _results.AddRange(ValidateFrequency(frequency));

            var right = evaluate(expr.Right);

            var foundFrame = _frames.FindByFrequency(frequency);
            _results.Add(foundFrame != null, "SbFrame MUST NOT BE null", expr);

            if (_results.HasFailures)
                throw new Exception(_results.GetFailureMessages());

            ret = foundFrame.GetTargetValue((DataColumn)right, Convert.ToInt32(indexFomZero));

            return ret;
        }

        /// <summary>
        /// Domain literal is a DataColumn, i.e., `Close`, `Open`, `SMA(15)`
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitDomainLiteral(DomainLiteral expr)
        {
            return new DataColumn((string)expr.Column, expr.Indices);
        }

        /// <summary>
        /// Evaluate any found domain tokens prefixed w/ an `@` char.
        ///
        /// Current list includes @Entry, @AllTimeHigh, @AllTimeLow, @52WeekHigh
        /// and @52WeekLow
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitDomainToken(DomainToken expr)
        {
            object ret = null;
            switch (expr.Operator.Type)
            {
                case TokenType.e52WeekHigh:
                    ret = _frames.FiftyTwoWeekHigh;
                    break;
                case TokenType.e52WeekLow:
                    ret = _frames.FiftyTwoWeekLow;
                    break;
                case TokenType.eAllTimeHigh:
                    ret = _frames.AllTimeHigh;
                    break;
                case TokenType.eAllTimeLow:
                    ret = _frames.AllTimeLow;
                    break;
                case TokenType.eEntryPoint:
                    if (_position != null)
                        ret = _position.EntryPrice;
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitBinaryExpr(Binary expr)
        {
            object left = evaluate(expr.Left);
            object right = evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case eBangEqual: return !IsEqual(left, right);
                case eEqualEqual: return IsEqual(left, right);

                case eDaily:
                case eWeekly:
                case eMonthly:
                    _results.AddRange(ValidateDomainExpr(expr.Left, expr.Operator, expr.Right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    // if we've made it this far, we DEFINITELY have a domain
                    // expression, so we create a new DomainExpr object and
                    // visit it
                    return VisitDomainExpr(
                        new DomainExpr(
                            (Literal)expr.Left,
                            expr.Operator,
                            (DomainLiteral)expr.Right));

                case eCrossOver:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());

                    Expr newRight;
                    Expr newLeft;

                    // TODO - there is a lot of potential for failure around
                    // here. We need to type check everything. This is worth
                    // extracting.
                    var current = (double)left > (double)right;

                    if (expr.Left is Literal)
                        newLeft = expr.Left;
                    else
                        newLeft = new Binary(new Literal((int)expr.Left.Left.Value + 1), expr.Left.Operator, expr.Left.Right);

                    if (expr.Right is Literal)
                        newRight = expr.Right;
                    else
                        newRight = new Binary(new Literal((int)expr.Right.Left.Value + 1), expr.Right.Operator, expr.Right.Right);

                    var newLeftValue = evaluate(newLeft);
                    var newRightValue = evaluate(newRight);

                    return current && ((double)newLeftValue < (double)newRightValue);
                case eGreaterThan:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left > (double)right;

                case eGreaterThanOrEqual:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left >= (double)right;

                case eLessThan:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left < (double)right;

                case eLessThenOrEqual:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left <= (double)right;

                case eMinus:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left - (double)right;

                case ePlus:
                    if (left is double && right is double)
                        return (double)left + (double)right;
                    if (left is string && right is string)
                        return $"{left}{right}";
                    throw new Exception("Operands must be two numbers or two strings");

                case eSlash:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left / (double)right;

                case eStar:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left * (double)right;
            }

            return null;
        }

        /// <summary>
        /// Return the evaluation of a Unary expression. A Unary is an exression
        /// that has one operator on the left and a value on the right, i.e.,
        /// -4 or !true
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitUnaryExpr(Unary expr)
        {
            object right = evaluate(expr.Right);
            switch (expr.Operator.Type)
            {
                case eBang: return !IsTruthy(right);
                case eMinus:
                    _results.AddRange(checkNumberOperand(expr.Operator, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return -(double)right;
            }
            return null;
        }

        /// <summary>
        /// A grouping is a parenthesized expression, typically joining two
        /// larger expression, but could also be used to separate operators,
        /// i.e., 15 == (10 + 5)
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitGroupingExpr(Grouping expr)
        {
            return evaluate(expr.Expression);
        }

        /// <summary>
        /// A Literal expression returns just the given value, in most cases,
        /// it will be a number
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        /// <summary>
        /// Primary entry to the Accept/Visit coupling call
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        private object evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        private bool IsTruthy(object operand)
        {
            if (operand == null) return false;
            if (operand is bool) return (bool)operand;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            if (a.GetType() != b.GetType()) return false;
            // at this point, most of the evaluations here will be of numbers,
            // so this if statement should account for 99.9% of calls
            if (a is double) return (double)a == (double)b;
            return a == b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenOperator"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private ValidationResultList checkNumberOperands(Token tokenOperator, object left, object right)
        {
            var ret = new ValidationResultList();
            ret.Add(left != null, "Provided left value is not Null");
            ret.Add(right != null, "Provided right value is not Null");
            if (ret.HasFailures)
                return ret;
            ret.Add(left is double, $"{left.ToString()}: Operand must be a number | {tokenOperator.ToString()}", left);
            ret.Add(right is double, $"{right.ToString()}: Operand must be a number | {tokenOperator.ToString()}", right);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenOperator"></param>
        /// <param name="operand"></param>
        /// <returns></returns>
        private ValidationResultList checkNumberOperand(Token tokenOperator, object operand)
        {
            var ret = new ValidationResultList();
            ret.Add(operand is double, $"{tokenOperator.ToString()}: Operand must be a number.", tokenOperator);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private EFrequency MapFrequencyFromToken(Token token)
        {
            switch (token.Type)
            {
                case eDaily:
                    return EFrequency.eDaily;
                case eWeekly:
                    return EFrequency.eWeekly;
                case eMonthly:
                    return EFrequency.eMonthly;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
        private ValidationResultList ValidateFrequency(EFrequency frequency)
        {
            var ret = new ValidationResultList();
            var found = false;
            switch (frequency)
            {
                case EFrequency.eDaily:
                    found = true;
                    break;
                case EFrequency.eWeekly:
                    found = true;
                    break;
                case EFrequency.eMonthly:
                    found = true;
                    break;
                default:
                    break;
            }
            ret.Add(found, "Frequency must be Daily, Weekly, Monthly", frequency);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="op"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private ValidationResultList ValidateDomainExpr(Expr left, Token op, Expr right)
        {
            var ret = new ValidationResultList();
            ret.Add(left is Literal, "DomainExpr.Left MUST BE of Literal type.", left);
            ret.Add(right is DomainLiteral, "DomainExpr Right MUST be of DomainLiteral type", right);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tryIndex"></param>
        /// <returns></returns>
        private ValidationResultList ValidateDomainIndex(object tryIndex)
        {
            var ret = new ValidationResultList();
            ret.Add(double.TryParse(tryIndex.ToString(), out double result), $"DomainIndex must be a number, {tryIndex} given", tryIndex);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tryToken"></param>
        /// <returns></returns>
        private ValidationResultList ValidateDomainOperator(Token tryToken)
        {
            var ret = new ValidationResultList();
            ret.Add(tryToken.IsOfDomainIndexType(), tryToken.ErrorMessage_IsOfDomainOperatorType(), tryToken);
            return ret;
        }

        #region IStatementVisitor

        public void VisitExpressionStmt(Stmt.Expression stmt)
        {
            evaluate(stmt.Expr);
        }

        public void VisitIfStmt(Stmt.If stmt)
        {
            throw new NotImplementedException();
        }

        public void VisitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = evaluate(stmt.Initializer);
            }
            Environment.Define(stmt.Name.Lexeme, value);
        }

        public void VisitBlockStmt(Stmt.Block block)
        {
            ExecuteBlock(block.Statements, new sbenv.Environment(Environment));
        }

        #endregion

        void ExecuteBlock(List<Stmt> statements, sbenv.Environment environment)
        {
            sbenv.Environment previous = Environment;
            try
            {
                Environment = environment;

                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                Environment = previous;
            }
        }

        public ValidationResultList GetExceptions()
        {
            return _exceptions;
        }

        public sbenv.Environment GetEnvironment()
        {
            return Environment;
        }

        public object VisitAssignExpr(Assign expr)
        {
            object value = evaluate(expr.Right);
            Environment.Assign(expr.Name, value);
            return value;
        }

        public object VisitVariableExpr(Variable expr)
        {
            return Environment.Get(expr.Name);
        }

        public object VisitLogicalExpr(Logical expr)
        {
            object left = evaluate(expr.Left);

            if (expr.Operator.Type == eOr)
            {
                if (IsTruthy(left)) return left;
            }
            else
            {
                if (!IsTruthy(left)) return left;
            }

            return evaluate(expr.Right);
        }
    }
}
