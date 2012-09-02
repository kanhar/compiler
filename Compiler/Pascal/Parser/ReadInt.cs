using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pascal
{



    // read_int <ident>
    public class ReadInt : Expression
    {
        public String _Identifier;

        public ReadInt() : this(null) { }

        public ReadInt(String pIdentifier)
        {
            _Identifier = pIdentifier;
        }

    }
}
