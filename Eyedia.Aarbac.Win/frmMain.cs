using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Eyedia.Aarbac.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eyedia.Aarbac.Win
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Bind();
        }

        private void Bind()
        {
            cbInstances.DataSource = Rbac.GetRbacs();
            cbInstances.DisplayMember = "Name";

            cbUsers.DataSource = Rbac.GetUsers();
            cbUsers.DisplayMember = "UserName";

            cbRoles.DataSource = Rbac.GetRoles();
            cbRoles.DisplayMember = "Name";
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            RbacEngineWebResponse response = new RbacEngineWebResponse();
            //try
            //{
                response.RbacName = ((Rbac)cbInstances.SelectedItem).Name;
                response.UserName = ((RbacUser)cbUsers.SelectedItem).UserName;
                response.RoleName = ((RbacRole)cbRoles.SelectedItem).Name;
                using (Rbac ctx = new Rbac(response.UserName, response.RbacName, response.RoleName))
                {                    
                    SqlQueryParser parser = new SqlQueryParser(ctx);
                    parser.Parse(txtQuery.Text);

                    using (RbacSqlQueryEngine eng = new RbacSqlQueryEngine(parser, true))
                    {                        
                        eng.Execute();
                        response.SetResult(eng);
                    }
                }

            //}
            //catch (Exception ex)
            //{

            //    response.SetResult(ex.Message);
            //}

            TreeNode root = treeView1.Nodes.Add("Root");            
            JToken token = JToken.Parse(JsonConvert.SerializeObject(response));
            Traverse(token, root);
        }

        private void Traverse(JToken token, TreeNode tn)
        {
            if (token is JProperty)
                if (token.First is JValue)
                    tn.Nodes.Add(((JProperty)token).Name + ": " + ((JProperty)token).Value);
                else
                    tn = tn.Nodes.Add(((JProperty)token).Name);

            foreach (JToken token2 in token.Children())
                Traverse(token2, tn);
        }

        private void showLoadedQueriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainerBase.Panel1Collapsed = !showLoadedQueriesToolStripMenuItem.Checked;
        }
    }
}
