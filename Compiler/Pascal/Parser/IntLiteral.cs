using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//DEF: A Statement is something that can exist independently and product a value
/* <stmt> := var <ident> = <expr>
    | <ident> = <expr>
    | for <ident> = <expr> to <expr> do <stmt> end
    | read_int <ident>
    | print <expr>
    | <stmt> ; <stmt>     
  */
/* <expr> := <string>
     *  | <int>
     *  | <arith_expr>
     *  | <ident>
     */

namespace Pascal
{


    // <int> := <digit>+
    public class IntLiteral : Expression
    {
        public Int32 _Value;

        public IntLiteral() : this(0) { }
        
        public IntLiteral(Int32 pValue)
        {
            _Value = pValue;
        }
    }
}
