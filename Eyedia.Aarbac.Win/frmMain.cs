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
using GenericParsing;
using System.IO;
using System.Drawing.Design;

namespace Eyedia.Aarbac.Win
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Bind();
        }

        RbacEngineWebRequest _Request;
        private void Bind()
        {
            cbInstances.DataSource = Rbac.GetRbacs();
            cbInstances.DisplayMember = "Name";

            cbUsers.DataSource = Rbac.GetUsers();
            cbUsers.DisplayMember = "UserName";

            cbRoles.DataSource = Rbac.GetRoles();
            cbRoles.DisplayMember = "Name";

            _Request = new RbacEngineWebRequest();
            engineInput.SelectedObject = _Request;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            txtErrors.Text = string.Empty;
            txtErrors.Visible = false;
            RbacEngineWebResponse response = new RbacEngineWebResponse();
            try
            {
                _Request.RbacName = ((Rbac)cbInstances.SelectedItem).Name;
                _Request.UserName = ((RbacUser)cbUsers.SelectedItem).UserName;
                _Request.RoleName = ((RbacRole)cbRoles.SelectedItem).Name;
                _Request.Query = txtQuery.Text;
                engineInput.SelectedObject = _Request;

                using (Rbac ctx = new Rbac(_Request.UserName, _Request.RbacName, _Request.RoleName))
                {
                    SqlQueryParser parser = new SqlQueryParser(ctx, _Request.SkipParsing);
                    parser.Parse(_Request.Query);

                    using (RbacSqlQueryEngine eng = new RbacSqlQueryEngine(parser, _Request.DebugMode))
                    {
                        eng.SkipExecution = _Request.SkipExecution;
                        eng.Execute();
                        response.SetResult(eng);
                    }
                }

            }
            catch (RbacException ex)
            {
                txtErrors.Text = ex.Message;
                txtErrors.Visible = true;
            }

            BindResult(response);

        }

        private void BindResult(RbacEngineWebResponse response)
        {
            treeView1.Nodes.Clear();
            response.RbacName = _Request.RbacName;
            response.UserName = _Request.UserName;
            response.RoleName = _Request.RoleName;
            if (string.IsNullOrEmpty(response.Errors))
            {
                txtParsedQuerys1.Text = response.ParsedQueryStage1;
                txtParsedQuery.Text = response.ParsedQuery;
                TreeNode root = treeView1.Nodes.Add("Root");
                JToken token = JToken.Parse(JsonConvert.SerializeObject(response));
                Traverse(token, root);
                root.Expand();
            }
            else
            {
                txtErrors.Visible = true;
                txtErrors.Text = response.Errors;
            }
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

        private void loadQueriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    GenericParserAdapter parser = new GenericParserAdapter(openFileDialog1.FileName);
                    parser.FirstRowHasHeader = true;
                    DataTable table = parser.GetDataTable();
                    table.TableName = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                    lvwQueries.Tag = table;
                    foreach (DataRow row in table.Rows)
                    {
                        ListViewItem lvi = new ListViewItem(row["Query"].ToString());
                        lvi.SubItems.Add(row["User"].ToString());
                        lvi.SubItems.Add(row["Role"].ToString());
                        lvi.Tag = row;
                        lvwQueries.Items.Add(lvi);
                    }
                    splitContainerBase.Panel1Collapsed = false;
                }
                catch
                {
                    MessageBox.Show("Error! This operation accepts a csv file with 3 columns 'Instance', 'Query', 'User', 'Role'. The first row expected is header.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lvwQueries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwQueries.SelectedItems.Count == 1)
            {
                DataRow row = (DataRow)lvwQueries.SelectedItems[0].Tag;
                cbInstances.Text = row["Instance"].ToString();
                cbUsers.Text = row["User"].ToString();
                cbRoles.Text = row["Role"].ToString();
                txtQuery.Text = row["Query"].ToString();
                btnExecute_Click(sender, e);
            }
        }

      
        private void btnExecuteAll_Click(object sender, EventArgs e)
        {
            if (lvwQueries.Tag != null)
            {

                DataTable table = lvwQueries.Tag as DataTable;
                if (table.Columns["ParsedQueryStage1"] == null)
                {
                    table.Columns.Add("ParsedQueryStage1");
                    table.Columns.Add("ParsedQuery");
                    table.Columns.Add("Errors");
                }
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        Rbac rbac = new Rbac(row[2].ToString(), row[0].ToString(), row[3].ToString());

                        SqlQueryParser parser = new SqlQueryParser(rbac);
                        parser.Parse(row[1].ToString());
                        RbacSqlQueryEngine engine = new RbacSqlQueryEngine(parser, true);
                        engine.Execute();
                        row["ParsedQueryStage1"] = parser.ParsedQueryStage1;
                        row["ParsedQuery"] = parser.ParsedQuery;
                        row["Errors"] = parser.AllErrors + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        row["Errors"] = ex.Message;
                    }
                }

                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, table.TableName + "_out.csv");
                try
                {
                    table.ToCsv(fileName);
                    MessageBox.Show("Test results are saved on " + fileName + "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cbInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbInstances.SelectedIndex > -1)
            {
                propInstance.SelectedObject = new RbacEngineWeb(Rbac.GetRbac(((Rbac)cbInstances.SelectedItem).Name));
                tabPage2.Text = ((RbacEngineWeb)propInstance.SelectedObject).Name;
            }
                
        }

        private void cbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbUsers.SelectedIndex > -1)
            {
                var user = (RbacUser)cbUsers.SelectedItem;
                int roleId = 0;
                if (cbRoles.SelectedItem != null)
                    roleId = ((RbacRole)cbRoles.SelectedItem).RoleId;

                propUser.SelectedObject = new RbacRegisterUser(roleId, user.UserName, user.FullName, user.Email, string.Empty);
                tabPage3.Text = user.UserName;

                LoadUserParameters();
            }
        }

        private void LoadUserParameters()
        {
            lvwUserParameters.Items.Clear();
            if (cbUsers.SelectedIndex > -1)
            {
                var selUser = (RbacUser)cbUsers.SelectedItem;
                RbacUser user = new RbacUser(selUser.UserName);
                if(user.Parameters != null)
                {
                    foreach(var paramter in user.Parameters)
                    {
                        ListViewItem item = new ListViewItem(paramter.Key);
                        item.SubItems.Add(paramter.Value);
                        lvwUserParameters.Items.Add(item);
                    }
                }
            }
        }

        private void cbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRoles.SelectedItem != null)
            {
                RbacRole dbRole = Rbac.GetRole(((RbacRole)cbRoles.SelectedItem).Name);
                RbacRoleWeb role = new RbacRoleWeb(dbRole);

                tabPage4.Text = role.Name;
                txtRole.Text = role.MetaDataRbac;
                txtEntitlements.Text = role.MetaDataEntitlements;

                role.MetaDataRbac = string.Empty;
                role.MetaDataEntitlements = string.Empty;
                propRole.SelectedObject = role;
            }
            else
            {
                tabPage4.Tag = null;
                tabPage4.Text = "Role";
            }
        }

        private void btnSaveInstance_Click(object sender, EventArgs e)
        {
             
            if (propInstance != null)
            {
                RbacEngineWeb rbac = propInstance.SelectedObject as RbacEngineWeb;          
                Rbac.Save(rbac);
            }
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {            
            if (propUser.SelectedObject != null)
            {
                //RbacUser user = propUser.SelectedObject as RbacUser;            
                //Rbac.Save(user);
            }
        }

        private void btnSaveRole_Click(object sender, EventArgs e)
        {
            if (propRole.SelectedObject != null)
            {
                RbacRoleWeb wRole = propRole.SelectedObject as RbacRoleWeb;              
                wRole.MetaDataRbac = txtRole.Text;
                wRole.MetaDataEntitlements = txtEntitlements.Text;
                Rbac.Save(wRole);
            }
        }

    }
}
