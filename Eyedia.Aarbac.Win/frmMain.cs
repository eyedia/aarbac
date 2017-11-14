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

            propertyGrid.SelectedObject = response;
        }
    }
}
