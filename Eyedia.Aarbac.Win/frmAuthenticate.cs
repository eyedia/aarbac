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
    public partial class frmAuthenticate : Form
    {
        public frmAuthenticate()
        {
            InitializeComponent();
            cbUsers.DataSource = Rbac.GetUsers();
            cbUsers.DisplayMember = "UserName";
        }
        public RbacUser User
        {
            get
            {
                if (cbUsers.SelectedItem != null)
                {
                    RbacUser user = (RbacUser)cbUsers.SelectedItem;
                    user = user.PopulateRole();
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
        private void frmAuthenticate_KeyUp(object sender, KeyEventArgs e)
        {
            Application.Exit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
            Cursor = Cursors.WaitCursor;
            frmMain mainWindow = new frmMain(this);
            mainWindow.Show();            
        }
    }
}
