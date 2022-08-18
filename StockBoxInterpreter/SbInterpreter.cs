using System;
using StockBox.Data.SbFrames;
using StockBox.Interpreter.Expressions;
using StockBox.Associations.Tokens;
using StockBox.Validation;
using static StockBox.Associations.Tokens.TokenType;


namespace StockBox.Interpreter
{

    /// <summary>
    /// Recursively calculate the provided expression
    /// </summary>
    public class SbInterpreter : IVisitor, IValidationResultsListProvider
    {

        private readonly SbFrameList _frames;

        public ValidationResultList Results { get { return _results; } }
        private readonly ValidationResultList _results = new ValidationResultList();

        public SbInterpreter(SbFrameList frames) { _frames = frames; }

        public SbInterpreter() { }

        /// <summary>
        /// Begin the recursive evaluation
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object Interpret(Expr expression)
        {
            return evaluate(expression);
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

                case eGreat:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left > (double)right;

                case eGreatEqual:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left >= (double)right;

                case eLess:
                    _results.AddRange(checkNumberOperands(expr.Operator, left, right));
                    if (_results.HasFailures)
                        throw new Exception(_results.GetFailureMessages());
                    return (double)left < (double)right;

                case eLessEqual:
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

    }
}
