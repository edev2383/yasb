using System;
using System.Collections.Generic;
using StockBox.Associations.Tokens;


namespace StockBox.Interpreter.Expressions
{

    /// <summary>
    /// Class <c>Expr</c> is the base Expression. All Rule.Statements are
    /// scanned and then parsed down into appropriate Expressions
    ///
    /// Binary
    /// Unary
    /// Literal
    /// Grouping
    /// DomainLiteral
    /// DomainExpr
    /// </summary>
    public abstract class Expr
    {
        public Expr Left { get { return _left; } }
        public Token Operator { get { return _operator; } }
        public Expr Right { get { return _right; } }
        public object Value { get { return _value; } }
        public Token Name { get { return _name; } }

        public string Statement { get; set; }

        public object Column { get { return _column; } }
        public int[] Indices { get { return _indices; } }

        private readonly Expr _left;
        private readonly Token _operator;
        private readonly Expr _right;
        private readonly object _value;
        private readonly object _column;
        private readonly int[] _indices;
        private readonly Token _name;

        public Expr(Expr left, Token op, Expr right)
        {
            _left = left;
            _operator = op;
            _right = right;
        }

        public Expr(Literal index, Token op, DomainLiteral literal)
        {
            _left = index;
            _operator = op;
            _right = literal;
        }

        public Expr(object column, params int[] indices)
        {
            _column = column;
            _indices = indices;
        }

        public Expr(object value)
        {
            _value = value;
        }

        /// <summary>
        /// A generic constructor for expressions that consist only of Tokens.
        /// Properties are set by their location in the constructor
        /// </summary>
        /// <param name="indicator"></param>
        /// <param name="indices"></param>
        /// <param name="domainToken"></param>
        /// <param name="variableName"></param>
        public Expr(Token indicator = null, Token indices = null, Token domainToken = null, Token variableName = null)
        {
            if (domainToken != null)
            {
                _operator = domainToken;
            }
            else if (variableName != null)
            {
                _name = variableName;
            }
            else
            {
                _column = indicator.Lexeme;
                if (indices != null)
                {
                    List<int> tmp = new List<int>();
                    foreach (var idx in indices.Lexeme.Split(','))
                        tmp.Add(Convert.ToInt32(idx.Trim()));
                    _indices = tmp.ToArray();
                }
            }
        }

        public abstract object Accept(IVisitor visitor);

        public abstract Expr Clone();
    }
}
