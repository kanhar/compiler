using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pascal
{
    // print <expr>
    public class Print : Expression
    {
        public Expression _Expr;

        public Print() : this(null) { }

        public Print(Expression pExpr)
        {
            _Expr = pExpr;
        }

    }
   
}
