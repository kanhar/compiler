using System.Text;
using System;
using System.IO;
using System.Collections.Generic;

using Reflect = System.Reflection;
using Emit = System.Reflection.Emit;

namespace Pascal
{
    public sealed class CodeGenerator
    {
        Reflect.AssemblyName _assemblyName;
        Emit.AssemblyBuilder _assemblyBuilder;
        Emit.ModuleBuilder _moduleBuilder;
        Emit.TypeBuilder _typeBuilder;
        Emit.MethodBuilder _methodBuilder;
        Emit.ILGenerator _ilGenerator;

        Pascal.Expression _statement;

        Dictionary<
            String,
            Emit.LocalBuilder
                  > _symbolTable;

        public CodeGenerator(Expression pExpression, String pModuleName, ref LogHandler rLogHandler)
        {
            _symbolTable = new Dictionary<String, Emit.LocalBuilder>();
            _assemblyName = new Reflect.AssemblyName(Path.GetFileNameWithoutExtension(pModuleName));

            _statement = pExpression;

            //Init Assembly
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_assemblyName, Emit.AssemblyBuilderAccess.Save);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(pModuleName);
            _typeBuilder = _moduleBuilder.DefineType("PascalCompilerType");
            _methodBuilder = _typeBuilder.DefineMethod
                                            (
                                                "Main",
                                                Reflect.MethodAttributes.Static,
                                                typeof(void),
                                                Type.EmptyTypes
                                            );
            _ilGenerator = _methodBuilder.GetILGenerator();

            //Actual Work
            GenerateStatement(_statement, null);
            _ilGenerator.Emit(Emit.OpCodes.Ret);

            //Finalizing Work
            _typeBuilder.CreateType();
            _moduleBuilder.CreateGlobalFunctions();
            _assemblyBuilder.SetEntryPoint(_methodBuilder);
            _assemblyBuilder.Save(pModuleName);
        }


        private void GenerateStatement(Expression pStatement, Type pExpectedType)
        {
            if (pStatement is DeclareVar)
            {
                DeclareVar curStmnt = (DeclareVar)pStatement;

                DeclareSymbolInTable(curStmnt.Identifier, curStmnt.GetSystemType());
            }
            else if (pStatement is ReadInt)
            {
                ReadInt curStmnt = (ReadInt)pStatement;

                _ilGenerator.Emit(Emit.OpCodes.Call, GetMSILMethod("ReadLine", typeof(String)));
                _ilGenerator.Emit(Emit.OpCodes.Call, GetMSILMethod("Parse", typeof(Int32)));

                ValidateSymbolType(curStmnt._Identifier, typeof(Int32));
                _ilGenerator.Emit(Emit.OpCodes.Stloc, GetValueFromSymbolTable(curStmnt._Identifier, -1));
            }
            else if (pStatement is StringLiteral)
            {
                StringLiteral curStmnt = (StringLiteral)pStatement;

                _ilGenerator.Emit(Emit.OpCodes.Ldstr, curStmnt._Value);

                if (pExpectedType != typeof(String))
                    throw new Exception("can't coerce a " + typeof(Int32).Name + " to a " + pExpectedType.Name);

            }
            else if (pStatement is IntLiteral)
            {
                IntLiteral curStmnt = (IntLiteral)pStatement;

                //Sample ASM:   IL_0001:  ldc.i4.3 (pushes 3 onto stack)
                _ilGenerator.Emit(Emit.OpCodes.Ldc_I4, curStmnt._Value);

                //Casting Possibility
                if (pExpectedType != typeof(Int32))
                {
                    if (pExpectedType == typeof(String))
                    {
                        _ilGenerator.Emit(Emit.OpCodes.Box, typeof(Int32));
                        _ilGenerator.Emit(Emit.OpCodes.Callvirt, GetMSILMethod("toString", null));
                    }
                    else
                    {
                        throw new Exception("can't coerce a " + typeof(Int32).Name + " to a " + pExpectedType.Name);
                    }
                }
            }
            else if (pStatement is Variable)
            {
                Variable curStmnt = (Variable)pStatement;

                _ilGenerator.Emit(Emit.OpCodes.Ldloc, GetValueFromSymbolTable(curStmnt._Identifier, -1));
                if (pExpectedType != TypeOfExpression(pStatement))
                {
                    if (TypeOfExpression(pStatement) == typeof(Int32) && pExpectedType == typeof(String))
                    {
                        _ilGenerator.Emit(Emit.OpCodes.Box, typeof(Int32));
                        _ilGenerator.Emit(Emit.OpCodes.Callvirt, GetMSILMethod("toString", null));
                    }
                    else
                    {
                        throw new Exception("can't coerce a " + TypeOfExpression(pStatement).Name +
                            " to a " + pExpectedType.Name);
                    }
                }
            }
            else if (pStatement is LinkedList)
            {
                LinkedList seq = (LinkedList)pStatement;

                GenerateStatement(seq.First, null);
                GenerateStatement(seq.Second, null);
            }
            else if (pStatement is Assign)
            {
                //retrieve info about variable, including address, type
                //LHS addrs <--addr;
                //LHS type <-- type;
                //getToken();
                //match(TK_ASSIGN);

                //a = b+c;
                //push b
                //push c
                //add
                //pop a


                Assign curStmnt = (Assign)pStatement;

                GenerateStatement(curStmnt._Expression, TypeOfExpression(curStmnt._Expression));

                //ValidateSymbolType(curStmnt._Identifier, TypeOfExpression(curStmnt._Expression));
                //Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 0.
                _ilGenerator.Emit(Emit.OpCodes.Stloc, GetValueFromSymbolTable(curStmnt._Identifier, -1));
            }
            else if (pStatement is Print)
            {
                Print curStmnt = (Print)pStatement;

                GenerateStatement(curStmnt._Expr, TypeOfExpression(curStmnt._Expr));
                _ilGenerator.Emit(Emit.OpCodes.Call, GetMSILMethod("Print", TypeOfExpression(curStmnt._Expr)));
            }
            else if (pStatement is ForLoop)
            {
                ForLoop curStmnt = (ForLoop)pStatement;

                Assign assign = new Assign(curStmnt._Identifier, curStmnt._From, -1);
                GenerateStatement(assign, null);

                // jump to the test
                Emit.Label test = _ilGenerator.DefineLabel();
                Emit.Label body = _ilGenerator.DefineLabel();

                _ilGenerator.Emit(Emit.OpCodes.Br, test);
                _ilGenerator.MarkLabel(body);

                GenerateStatement(curStmnt._Body, null);

                // to (increment the value of x)
                _ilGenerator.Emit(Emit.OpCodes.Ldloc, GetValueFromSymbolTable(curStmnt._Identifier, -1));
                _ilGenerator.Emit(Emit.OpCodes.Ldc_I4, 1);
                _ilGenerator.Emit(Emit.OpCodes.Add);

                ValidateSymbolType(curStmnt._Identifier, typeof(Int32));
                _ilGenerator.Emit(Emit.OpCodes.Stloc, GetValueFromSymbolTable(curStmnt._Identifier, -1));

                _ilGenerator.MarkLabel(test);
                _ilGenerator.Emit(Emit.OpCodes.Ldloc, GetValueFromSymbolTable(curStmnt._Identifier, -1));
                GenerateStatement(curStmnt._To, typeof(Int32));

                _ilGenerator.Emit(Emit.OpCodes.Blt, body);
            }
            else
            {
                throw new Exception("don't know how to generate a " + pStatement.GetType().Name);
            }
        }

        public void DeclareSymbolInTable(String pIdentifier, Type pType)
        {
            _symbolTable[pIdentifier] = _ilGenerator.DeclareLocal(pType);
        }

        public Emit.LocalBuilder GetValueFromSymbolTable(String pIdentifier, Int32 pIndex)
        {
            if (!_symbolTable.ContainsKey(pIdentifier))
                throw new Exception("Undeclared variable '" + pIdentifier + "'");

            if (pIndex != -1)
            {
                //Array with Index pIndex
                Emit.LocalBuilder local = _symbolTable[pIdentifier];                
                return _symbolTable[pIdentifier];
            }
            else
            {
                return _symbolTable[pIdentifier];
            }
        }

        private void ValidateSymbolType(String pName, Type pExpectedType)
        {
            Emit.LocalBuilder localBuilder = GetValueFromSymbolTable(pName, -1);

            if (localBuilder.LocalType != pExpectedType)
            {
                String ErrMSg = "Variable {0} is of type {1}, but type {2} cannot be stored in it";
                throw new Exception(String.Format(ErrMSg, pName, localBuilder.LocalType.Name, pExpectedType.Name));
            }
        }

        private Type TypeOfExpression(Expression expr)
        {
            if (expr is StringLiteral)
                return typeof(String);

            if (expr is IntLiteral)
                return typeof(Int32);

            if (expr is Variable)
                if (_symbolTable.ContainsKey(((Variable)expr)._Identifier))
                    return ((Emit.LocalBuilder)_symbolTable[((Variable)expr)._Identifier]).LocalType;
                else
                    throw new Exception("undeclared variable '" + ((Variable)expr)._Identifier + "'");

            throw new Exception("Cannot Calculate the type of " + expr.GetType().Name);
        }

        public Reflect.MethodInfo GetMSILMethod(String pFunction, Type pType)
        {
            Reflect.MethodInfo methodInfo = null;
            switch (pFunction)
            {
                case "Print":
                    methodInfo = typeof(System.Console).GetMethod
                                 (
                                    "WriteLine",
                                    new Type[] { pType }
                                 );
                    break;
                case "ReadLine":
                    methodInfo = typeof(System.Console).GetMethod
                                (
                                    "ReadLine",
                                    Reflect.BindingFlags.Public | Reflect.BindingFlags.Static,
                                    null,
                                    Type.EmptyTypes,
                                    null
                                );
                    break;
                case "Parse":
                    methodInfo = typeof(Int32).GetMethod
                                (
                                    "Parse",
                                    Reflect.BindingFlags.Public | Reflect.BindingFlags.Static,
                                    null,
                                    new Type[] { typeof(String) },
                                    null
                                );
                    break;
                case "toString":
                    methodInfo = typeof(object).GetMethod
                                (
                                    "ToString"
                                );
                    break;
                default:
                    break;
            }

            return methodInfo;
        }

        //Destructor
        ~CodeGenerator()
        {            
            _symbolTable = null;
            _ilGenerator = null;
        }

    }

}
