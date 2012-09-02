using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pascal
{
    // <stmt> ; <stmt>
    public class LinkedList : Expression
    {
        public Expression First;
        public Expression Second;

        public LinkedList() : this(null, null) { }
        
        public LinkedList(Expression pFirst, Expression pSecond)
        {
            First = pFirst;
            Second = pSecond;
        }
    }
}
