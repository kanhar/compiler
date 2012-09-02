using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using Microsoft.Build.Utilities;



namespace WebMSIL
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtInputMSIL.Text = @"
//compile with ilasm
.assembly hello {}
.method static public void main() il managed 
{
  .entrypoint
  ldstr ""Hello MS IL!""
  call	void [mscorlib]System.Console::WriteLine(class System.String) 
  ret
}       
";
        }
                
        protected void btnMsiltoExe_Click(object sender, EventArgs e)
        {
            String MSIL = txtInputMSIL.Text;

            //Dissasemble
            Process ildasmProcess;
            String ilOutput;

            //Compile MSIL in Test.il to EXE
            ildasmProcess = new Process();
            ildasmProcess.StartInfo.WorkingDirectory = @"C:\dev\CompilerExtract";
            ildasmProcess.StartInfo.FileName = @"C:\dev\CompilerExtract\ilasm.exe ";
            ildasmProcess.StartInfo.Arguments = @"test.il";
            ildasmProcess.StartInfo.UseShellExecute = false;
            ildasmProcess.StartInfo.CreateNoWindow = true;
            ildasmProcess.StartInfo.RedirectStandardOutput = true;
            ildasmProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ildasmProcess.Start();

            ilOutput = ildasmProcess.StandardOutput.ReadToEnd();
            ildasmProcess.WaitForExit();
            ildasmProcess.Close();

            txtOutputCSharp.Text = ilOutput;

        }


        protected void BtnCtoMsil_Click(object sender, EventArgs e)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            string ExecutableName = "Out.exe";
            Button ButtonObject = (Button)sender;

            txtOutputMsil.Text = "";
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            //Make sure we generate an EXE, not a DLL
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = ExecutableName;
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, txtCSharp.Text);
            

            if (results.Errors.Count > 0)
            {
                txtOutputMsil.ForeColor = System.Drawing.Color.Red;
                foreach (CompilerError CompErr in results.Errors)
                {
                    txtOutputMsil.Text = txtOutputMsil.Text +
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                //Successful Compile
                txtOutputMsil.ForeColor = System.Drawing.Color.Blue;

                //Execute
                Process coreProcess;
                String coreOutput;

                coreProcess = new Process();
                coreProcess.StartInfo = new ProcessStartInfo(ExecutableName);
                coreProcess.StartInfo.UseShellExecute = false;
                coreProcess.StartInfo.RedirectStandardOutput = true;
                coreProcess.StartInfo.RedirectStandardInput = false;
                coreProcess.Start();
                coreOutput = coreProcess.StandardOutput.ReadToEnd();
                coreProcess.WaitForExit();
                coreProcess.Close();


                //Dissasemble
                Process ildasmProcess;
                String ilOutput;

                ildasmProcess = new Process();
                ildasmProcess.StartInfo.FileName = @"c:\dev\CompilerExtract\ildasm.exe";
                ildasmProcess.StartInfo.Arguments = "/text /nobar \"" + @"c:\dev\CompilerExtract\loop.exe";
                ildasmProcess.StartInfo.UseShellExecute = false;
                ildasmProcess.StartInfo.CreateNoWindow = true;
                ildasmProcess.StartInfo.RedirectStandardOutput = true;
                ildasmProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ildasmProcess.Start();

                ilOutput = ildasmProcess.StandardOutput.ReadToEnd();
                ildasmProcess.WaitForExit();
                ildasmProcess.Close();

                String[] arr = ilOutput.Split(new String[] { ".entrypoint" }, StringSplitOptions.RemoveEmptyEntries);
                txtOutputMsil.Text = txtOutputMsil.Text + "Result = " + coreOutput + Environment.NewLine + arr[1];                
            }

        }

        
    }
}
