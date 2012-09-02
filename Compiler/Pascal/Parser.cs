using System.Collections.Generic;
using Text = System.Text;
using System;
using System.Text;

namespace Pascal
{
    public class Parser
    {
        public Expression _treeExpressions;
        public Int32 _index;
        public IList<Object> _tokens;
        
        public LogHandler _LogHandler;

        public Parser(IList<Object> tokens, ref LogHandler rLogHandler)
        {
            _LogHandler = rLogHandler;
            _tokens = tokens;
            _index = 0;

            _treeExpressions = ParseExpression();
        }

        private Expression ParseExpression()
        {
            Expression curExpression;
                        
            if (GetCurToken().Equals("print"))
            {
                Print print = new Print();
                
                NextToken();                
                print._Expr = GetCurTokenAsLiteral();                
                
                curExpression = print;                              
            }
            else if (GetCurToken().Equals("var"))
            {
                DeclareVar declareVar = new DeclareVar();
                
                NextToken();
                declareVar.Identifier = GetCurTokenAsString();

                NextToken();
                if (!Arithmetic.Colon.Equals(GetCurToken()))
                    throw new System.Exception("expected : after 'var ident'");

                NextToken();
                if (!(GetCurToken() is String))
                    throw new System.Exception("expected var type (integer, float)");
                else
                {
                    String curToken = GetCurTokenAsString();
                    if (curToken.Equals("array", StringComparison.InvariantCultureIgnoreCase))
                    {
                        NextToken();                        
                        if (!Arithmetic.OpenSquareBracket.Equals(GetCurToken()) )
                            throw new System.Exception("Invalid Array Declaration");

                        NextToken();
                        if (!(GetCurToken() is Int32))
                            throw new System.Exception("Invalid Array Declaration");

                        NextToken();
                        if (!Arithmetic.DoubleDots.Equals(GetCurToken()))
                            throw new System.Exception("Invalid Array Declaration");

                        NextToken();
                        if (!(GetCurToken() is Int32))
                            throw new System.Exception("Invalid Array Declaration");

                        NextToken();
                        if (!Arithmetic.CloseSquareBracket.Equals(GetCurToken()))
                            throw new System.Exception("Invalid Array Declaration");

                        NextToken();
                        if (!GetCurToken().Equals("of"))
                            throw new System.Exception("Invalid Array Declaration");
                        /*
                            Added Token array, Type String
                            Added Token OpenSquareBracket, Type Arithmetic
                            Added Token 1, Type Int32
                            Added Token DoubleDots, Type Arithmetic
                            Added Token 5, Type Int32
                            Added Token CloseSquareBracket, Type Arithmetic
                            Added Token of, Type String
                            Added Token integer, Type String                          
                        */
                        NextToken();
                        declareVar.IdentifierType = GetCurTokenAsString();
                        declareVar.IsArray = true;
                    }
                    else
                    {
                        declareVar.IdentifierType = curToken;
                    }
                }

                curExpression = declareVar;                              
            }
            else if (GetCurToken().Equals("read_int"))
            {
                ReadInt readInt = new ReadInt();
                
                NextToken();
                if (!(GetCurToken() is String))
                    throw new System.Exception("expected var name after read_int");
                else
                    readInt._Identifier = GetCurTokenAsString();                                

                curExpression = readInt;                
            }
            else if (GetCurToken().Equals("for"))
            {
                ForLoop forLoop = new ForLoop();
                
                //Get For Identifier followed by :-
                NextToken();
                if (!(GetCurToken() is String))                    
                    throw new System.Exception("expected identifier after 'for'");
                else
                    forLoop._Identifier = (String)GetCurToken();
                
                NextToken();
                if (!Arithmetic.Colon.Equals(GetCurToken()))
                    throw new System.Exception("for missing ': after for'");                

                NextToken();
                if (!Arithmetic.Equal.Equals(GetCurToken()))
                    throw new System.Exception("for missing '=' after for");
                
                //Get x to y
                NextToken();
                forLoop._From = GetCurTokenAsLiteral();

                NextToken();
                if (!GetCurToken().Equals("to"))
                    throw new System.Exception("expected 'to' after for");

                NextToken();
                forLoop._To = GetCurTokenAsLiteral();

                //Begin do, begin
                NextToken();
                if (!GetCurToken().Equals("do"))
                    throw new System.Exception("expected 'do' after from expression in for loop");

                NextToken();
                if (!GetCurToken().Equals("begin"))
                    throw new System.Exception("expected 'begin' in for loop");

                //Get For Loop Body
                NextToken();
                forLoop._Body = ParseExpression();
                
                //Todo: Parse STatement probly increements Token
                //Get For Loop end
                if (_index == _tokens.Count || !GetCurToken().Equals("end"))
                    throw new System.Exception("unterminated 'for' loop body");

                curExpression = forLoop;                
            }
            else if (GetCurToken() is String)
            {
                Assign assign = new Assign();
                assign._Identifier = GetCurTokenAsString();

                NextToken();
                if (!Arithmetic.Equal.Equals(GetCurToken()))
                    throw new System.Exception("Invalid Array Assignment");

                assign._Expression = GetCurTokenAsLiteral();

                curExpression = assign;                
            }
            else
            {
                throw new System.Exception("parse error at token " + _index + ": " + GetCurToken());
            }
            
            NextToken();
            //Check for Graceful end of Line
            if (!Arithmetic.Semi.Equals(GetCurToken()))
                throw new Exception("Unterminated Statement ");

            //Check for End of Program, If program has not ended yet, Recurse....            
            NextToken();
            if (_index == _tokens.Count || GetCurToken().Equals("end"))
                return curExpression;
            else             
                return new LinkedList(curExpression, ParseExpression());
        }

        private String GetCurTokenAsString()
        {
            Object curToken;

            curToken = GetCurToken();

            if (curToken is StringBuilder || curToken is String)
                return GetCurToken().ToString();
            else
                throw new System.Exception("Exxpected a String, received a " + curToken.GetType().Name);
        }

        private Int32 GetCurTokenAsInteger()
        {
            Object curToken;

            curToken = GetCurToken();

            if (curToken is Int32)
                return Convert.ToInt32(GetCurToken());
            else
                throw new System.Exception("Exxpected a String, received a " + curToken.GetType().Name);
        }

        private Expression GetCurTokenAsLiteral()
        {
            Object curToken;

            curToken = GetCurToken();

            if (_index == _tokens.Count)
                throw new System.Exception("expected expression, got EOF");
                            
            else if (curToken is StringBuilder)            
                return new StringLiteral(GetCurTokenAsString());                                            
            else if (curToken is String)            
                return new Variable(GetCurTokenAsString());            
            else if (curToken is Int32)            
                return new IntLiteral(GetCurTokenAsInteger());            
            else            
                throw new System.Exception("expected String literal, int literal, or variable");
        }
        
        private void NextToken()
        {
            _index++;
        }

        private Object GetCurToken()
        {
            if (_index < _tokens.Count)
                return _tokens[_index];
            else
                throw new Exception("Out of Tokens");
        }


        public void Add(System.String pMsg)
        {
            if (_LogHandler != null)
                _LogHandler(pMsg);
        }
    }

}
