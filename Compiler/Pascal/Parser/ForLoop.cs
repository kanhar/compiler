using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pascal
{
    // for <ident> = <expr> to <expr> do <stmt> end
    public class ForLoop : Expression
    {
        public String _Identifier;
        public Expression _From;
        public Expression _To;
        public Expression _Body;

        public ForLoop() :this(String.Empty, null, null, null) { }

        public ForLoop(String pIdentifier, Expression pFrom, Expression pTo, Expression pBody)
        {
            _Identifier = pIdentifier;
            _From = pFrom;
            _To = pTo;
            _Body = pBody;
        }
    }

   
}
