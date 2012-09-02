<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebMSIL._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center">
            <tr>
                <td>
                    <asp:TextBox 
                        runat="server"
                        ID="txtCSharp"
                        Text=
                        "
using System; 
using System.Collections.Generic; 
using System.Text;  
namespace TestMSIL 
{         
class Program     
{         
    static void Main(string[] args)         
    {            
        int x;              
        int a, b;              
        a = 3;             
        b = 5;                          
        x = a * b;                         
        Console.WriteLine(x);         
    }     
} 
}                         
                        "
                        Rows="50"
                        Columns="50"      
                        TextMode ="MultiLine"                  
                        />
                </td>
                <td>
                    <asp:TextBox 
                        runat="server"
                        ID="txtOutputMsil"
                        Text=""
                        Rows="50"
                        Columns="50"
                        TextMode ="MultiLine"                  
                        />
                </td>     
                <td>
                    <asp:Button
                        runat="server"
                        ID="BtnCtoMsil"
                        OnClick="BtnCtoMsil_Click"
                        Text="Convert"
                        />
                </td>                           
            </tr>
            
            
            <!--<tr>
                <td>
                    <asp:TextBox 
                        runat="server"
                        ID="txtInputMSIL"
                        Text=
                        "     
                        "
                        Rows="25"
                        Columns="50"      
                        TextMode ="MultiLine"                  
                        />
                </td>
                <td>
                    <asp:TextBox 
                        runat="server"
                        ID="txtOutputCSharp"
                        Text=""
                        Rows="25"
                        Columns="50"
                        TextMode ="MultiLine"                  
                        />
                </td>     
                <td>
                    <asp:Button
                        runat="server"
                        ID="btnMsiltoExe"
                        OnClick="btnMsiltoExe_Click"
                        Text="Convert"
                        />
                </td>                           
            </tr>            -->
        
        </table>
    </div>
    </form>
</body>
</html>
