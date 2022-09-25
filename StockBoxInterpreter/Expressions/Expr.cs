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

        public string Statement { get; set; }

        public object Column { get { return _column; } }
        public int[] Indices { get { return _indices; } }

        private readonly Expr _left;
        private readonly Token _operator;
        private readonly Expr _right;
        private readonly object _value;
        private readonly object _column;

        private readonly int[] _indices;

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

        public Expr(Token indicator, Token indices = null)
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

        public abstract object Accept(IVisitor visitor);
    }
}
