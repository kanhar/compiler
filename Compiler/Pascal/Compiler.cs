using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace Pascal
{
    public delegate void LogHandler(String message);
    
    public class Compiler
    {
        public Scanner          _scanner;
        public Parser           _parser;
        public Expression       _expressionTree;
        public IList<Object>    _tokenList;        
        public CodeGenerator    _codeGen;
                
        public LogHandler       _logHandler;
        public String           _processOutput, _ildasmProcessOutput, _tokens;
        
        public String           EXECUTABLE_NAME = "loop.exe";

        public void Scanner(String pFileContent)
        {
            TextReader reader;

            reader = new StringReader(pFileContent);
            _scanner = new Scanner(reader, ref _logHandler);
            _tokenList = _scanner._tokenList;
            _tokens = _scanner.Print();
        }

        public void Parser()
        {
            //Parse File, Get AST
            _parser = new Parser(_tokenList, ref _logHandler);
            _expressionTree = _parser._treeExpressions;
        }
        
        public void Compile()
        {            
            Process coreProcess, ildasmProcess;
            ProcessStartInfo processStartInfo;                                    
                           
            //Generate Code 
            _codeGen = new CodeGenerator(_expressionTree, EXECUTABLE_NAME, ref _logHandler);

            //Execute Process, read process output and close
            processStartInfo = new ProcessStartInfo(EXECUTABLE_NAME);
            coreProcess = new Process();

            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardInput = false;
            coreProcess.StartInfo = processStartInfo;
            coreProcess.Start();            
            _processOutput = coreProcess.StandardOutput.ReadToEnd();             
            coreProcess.WaitForExit();
            coreProcess.Close();

            // Save the Assembly and generate the MSIL code with ILDASM.EXE                        
            ildasmProcess = new Process();
            ildasmProcess.StartInfo.FileName = @"C:\Users\kmunshi\Desktop\CEX\CompilerExtract\ildasm.exe";
            ildasmProcess.StartInfo.Arguments = "/text /nobar \"" + @"C:\Users\kmunshi\Desktop\CEX\CompilerExtract\loop.exe";
            ildasmProcess.StartInfo.UseShellExecute = false;
            ildasmProcess.StartInfo.CreateNoWindow = true;
            ildasmProcess.StartInfo.RedirectStandardOutput = true;
            ildasmProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ildasmProcess.Start();

            _ildasmProcessOutput = ildasmProcess.StandardOutput.ReadToEnd();
            ildasmProcess.WaitForExit();
            ildasmProcess.Close();

            if (_logHandler != null)
                _logHandler(_ildasmProcessOutput);            
        }

    }
}
