using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pascal
{
    
    // <ident> = <expr>
    public class Assign : Expression
    {
        public String _Identifier;
        public Int32 _Index;
        public Expression _Expression;

        public Assign(): this(String.Empty, null, -1){}

        public Assign(System.String pIdent, Expression pExpr, Int32 pIndex)
        {
            _Identifier = pIdent;
            _Expression = pExpr;
            _Index = pIndex;
        }
    }
   
}
