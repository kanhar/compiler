<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CompilerGUI._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <table width="600" align="center" border="1">
    <tr>
        <td>
            <asp:Button ID="btnLoad"    Text="Load Default Program" runat="server"  OnClick="btnLoad_Click" />&nbsp;|&nbsp;        
        </td>
        <td>
            <asp:Button ID="btnCompile" Text="Compile Program" runat="server"  OnClick="btnCompile_Click" />                    
        </td>
    </tr>
    
        <tr>
            <td>
                <h3>Input Code</h3>
                <asp:TextBox 
                    ID="txtInput" 
                    runat="server" 
                    TextMode="MultiLine"
                    Rows="21"
                    Columns="40"            
                     />        
          
            </td>
            
            <td>
                <h3>Tokens</h3>
                <asp:TextBox 
                    ID="txtTokens" 
                    runat="server" 
                    TextMode="MultiLine"
                    Rows="21"
                    Columns="100"            
                     />                            
            </td>
        </tr>
            

        <tr>
            <td>
                <h3>Output</h3>
                <asp:TextBox 
                    ID="txtResult" 
                    runat="server" 
                    TextMode="MultiLine"
                    Rows="25"
                    Columns="40"            
                     />          
            </td>
            <td>
                 <h3>Assembly (MSIL) + Log</h3>
                <asp:TextBox 
                    ID="txtLog" 
                    runat="server" 
                    TextMode="MultiLine"
                    Rows="25"
                    Columns="100"            
                     /> 
            </td>
        </tr>
        
        <tr>
            <td colspan="2">
                <h3>AST</h3>
                <asp:TreeView 
                    runat="server"
                    ID="treeview"
                    SkipLinkText=""
                    ImageSet="BulletedList"
                    
                    />                  
            </td>
        </tr>
    </table>             
    </div>
    </form>
</body>
</html>
