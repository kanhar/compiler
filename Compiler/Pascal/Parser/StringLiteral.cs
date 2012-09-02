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

    // <string> := " <string_elem>* "
    public class StringLiteral : Expression
    {
        public String _Value;

        public StringLiteral():this(String.Empty) {}        

        public StringLiteral(String pValue)
        {
            _Value = pValue;
        }
    }
}
