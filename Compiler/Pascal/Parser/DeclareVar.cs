using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pascal
{


    // var <ident> = <expr>
    public class DeclareVar : Expression
    {
        public String Identifier;
        public String IdentifierType;
        public Boolean IsArray;

        public DeclareVar() :this(String.Empty, null, false) { }

        public DeclareVar(String pIdent, String pType, Boolean pIsArray)
        {
            Identifier = pIdent;
            IdentifierType = pType;
            IsArray = pIsArray;
        }

        public Type GetSystemType()
        {
            Type curSystemType = null;

            switch (IdentifierType)
            {
                case "integer":                    
                    if (IsArray)
                        curSystemType = typeof(Int32[]);
                    else
                        curSystemType = typeof(Int32);
                    break;
                case "string":
                    curSystemType = typeof(String);
                    break;
                case "real":
                    curSystemType = typeof(Double);
                    break;
                case "boolean":
                    curSystemType = typeof(Boolean);
                    break;
                case "character":
                    curSystemType = typeof(Char);
                    break;
                default:
                    throw new Exception("Type: " + IdentifierType + " not Supported");
            }
            //Array a = new int a[]{ 1, 2, 3 };
            return curSystemType;
        }
    }
}
