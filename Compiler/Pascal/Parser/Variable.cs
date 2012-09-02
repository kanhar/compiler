using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pascal
{



    // <ident> := <char> <ident_rest>*
    // <ident_rest> := <char> | <digit>
    public class Variable : Expression
    {
        public String _Identifier;

        public Variable() : this(String.Empty) { }

        public Variable(String pIdentifier)
        {
            _Identifier = pIdentifier;
        }
    }

   
}
