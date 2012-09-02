using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

namespace CompilerGUI
{
    public class Helper
    {
        public static String ReadFileToEnd(String pFilePath)
        {
            TextReader input = File.OpenText(pFilePath);
            String Code = input.ReadToEnd();
            input.Close();
            return Code;
        }
    }
}
