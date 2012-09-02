using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pascal
{
    public sealed class Scanner
    {
        public List<Object> _tokenList;
        public LogHandler _logHandler;

        public Scanner(TextReader input, ref LogHandler rLogHandler)
        {
            _logHandler = rLogHandler;
            _tokenList = new List<Object>();

            Scan(input);
        }

        public void Scan(TextReader pInput)
        {
            while (pInput.Peek() != -1)
            {
                char ch = (char)pInput.Peek();

                if (char.IsWhiteSpace(ch))
                {
                    pInput.Read();
                    continue;
                }

                // keyword or identifier
                if (char.IsLetter(ch) || ch == '_')
                {
                    StringBuilder accum = new StringBuilder();

                    while (char.IsLetter(ch) || ch == '_')
                    {
                        accum.Append(ch);
                        pInput.Read();

                        if (pInput.Peek() == -1)
                            break;
                        else
                            ch = (char)pInput.Peek();
                    }

                    AddToken(accum.ToString());
                }
                // String literal
                else if (ch == '"')
                {
                    StringBuilder accum = new StringBuilder();

                    // skip the '"'
                    pInput.Read();

                    if (pInput.Peek() == -1)
                        throw new System.Exception("Unterminated String literal");

                    while ((ch = (char)pInput.Peek()) != '"')
                    {
                        accum.Append(ch);
                        pInput.Read();

                        if (pInput.Peek() == -1)
                            throw new System.Exception("Unterminated String literal");
                    }

                    // skip the terminating "
                    pInput.Read();
                    AddToken(accum);
                }
                // numeric literal
                else if (char.IsDigit(ch))
                {
                    StringBuilder accum = new StringBuilder();

                    while (char.IsDigit(ch))
                    {
                        accum.Append(ch);
                        pInput.Read();

                        if (pInput.Peek() == -1)
                            break;
                        else
                            ch = (char)pInput.Peek();
                    }

                    AddToken(Int32.Parse(accum.ToString()));
                }
                else
                {
                    pInput.Read();
                    switch (ch)
                    {
                        case '+':                            
                            AddToken(Arithmetic.Add);
                            break;
                        case '-':                            
                            AddToken(Arithmetic.Sub);
                            break;
                        case '*':                            
                            AddToken(Arithmetic.Mul);
                            break;
                        case '/':                            
                            AddToken(Arithmetic.Div);
                            break;
                        case '=':                            
                            AddToken(Arithmetic.Equal);
                            break;
                        case ':':                            
                            AddToken(Arithmetic.Colon);
                            break;
                        case ';':                            
                            AddToken(Arithmetic.Semi);
                            break;
                        case '[':                            
                            AddToken(Arithmetic.OpenSquareBracket);
                            break;
                        case ']':                            
                            AddToken(Arithmetic.CloseSquareBracket);
                            break;
                        case '.':
                            if ((char)pInput.Peek() == '.')
                                AddToken(Arithmetic.DoubleDots);
                            else
                                throw new Exception("Invalid Period Sequence");
                            pInput.Read();
                            break;
                        default:
                            throw new Exception("Unrecognized character '" + ch + "'");
                    }
                }

            }
        }

        //Print Token List
        public String Print()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Object obj in _tokenList)
            {
                if (Arithmetic.Semi.Equals(obj))
                    sb.Append("\n");
                else
                    sb.Append(obj.ToString() + "\t");
            }
            sb.Append("\n");
            return sb.ToString();
        }

        public void AddToken(Object pObj)
        {
            _tokenList.Add(pObj);
            Log(String.Format("Added Token {0}, Type {1}", pObj, pObj.GetType().Name));
        }
        
        public void Log(String pMsg)
        {
            if (_logHandler != null)
                _logHandler(pMsg);
        }

    }



}
