using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Pascal;

using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;

/*
 * References:-
    http://geciauskas.com/dynamically-creating-net-assembly-with-system-reflections/
    http://www.codeproject.com/KB/msil/MsilParser.aspx
 * TodO:    for x:= 10 downto 1 do, while loop
 *          Infix Expression        
 *          arrays
 *          scope?
*/
namespace CompilerGUI
{
    public partial class _Default : System.Web.UI.Page
    {
        String filePath = @"C:\Users\kmunshi\Desktop\CEX\CompilerExtract\CompilerGUI\loop.gfn";

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {                
                ClearTextBoxes();
                txtInput.Text = Helper.ReadFileToEnd(filePath);                
            }
            catch (Exception ex)
            {
                Add(ex.Message);
            }
        }

        protected void btnCompile_Click(object sender, EventArgs e)
        {
            try
            {
                String pascalCode;
                Compiler compiler;

                //Init GUI                
                txtResult.Text = "";
                txtTokens.Text = "";                

                //Show Code
                pascalCode = txtInput.Text;
                ClearTextBoxes();
                txtInput.Text = pascalCode;

                //Show Tokens
                compiler = new Compiler();
                compiler._logHandler += new LogHandler(Add);
                
                compiler.Scanner(pascalCode);
                txtTokens.Text = compiler._tokens;

                compiler.Parser();
                compiler.Compile();

                
                

                //PopulateTreeView
                //TreeNode node = new TreeNode("AST");
                //treeview.Nodes.Add(node);
                //PopulateTreeView(compiler._expressionTree, node);
                //treeview.CollapseAll();

                //Add Output
                txtResult.Text = compiler._processOutput;
            }
            catch (Exception ex)
            {
                Add(ex.Message);
            }
        }

        public void PopulateTreeView(Expression AST, TreeNode curNode)
        {
            /*
            String res = "";

            if (AST is LinkedList)
            {
                LinkedList s = (LinkedList)AST;

                TreeNode firstNode = new TreeNode("-");
                TreeNode secondNode = new TreeNode("-");

                PopulateTreeView(s.First, firstNode);
                PopulateTreeView(s.Second, secondNode);

                curNode.ChildNodes.Add(firstNode);
                curNode.ChildNodes.Add(secondNode);
            }
            else if (AST is DeclareVar)
            {
                DeclareVar dv = (DeclareVar)AST;
                res = "ident = " + dv.Identifier + ", type = " + dv.IdentifierType;
                curNode.ChildNodes.Add(new TreeNode("DeclareVar", dv.Identifier));
                //txtAST.Text = txtAST.Text + res + "\n";                    
            }
            else if (AST is Assign)
            {
                Assign a = (Assign)AST;

            }
            else if (AST is ForLoop)
            {
                ForLoop fl = (ForLoop)AST;
                res = "for loop = " + fl._Identifier;
                //txtAST.Text = txtAST.Text + res + "\n";
                curNode.ChildNodes.Add(new TreeNode("ForLoop", fl._Identifier));
                PopulateTreeView(fl._Body, curNode);
            }
            else
            {
                return;
            }
*/
        }


        public void ClearTextBoxes()
        {
            txtResult.Text = "";
            txtTokens.Text = "";
            txtInput.Text = "";
            txtLog.Text = "";
        }

        public void Add(String pMsg)
        {            
            txtLog.Text = txtLog.Text + "\n" + pMsg;
        }
    }
}
