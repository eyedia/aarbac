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
    public partial class frmLog : Form
    {
        public frmLog()
        {
            InitializeComponent();
            splitContainer2.SplitterDistance = 80;
            splitContainer3.SplitterDistance = 80;
            splitContainer4.SplitterDistance = 50;
            Bind();
        }

        private void Bind()
        {
            List<Framework.DataManager.RbacLog> logs = Rbac.GetLogs(false);
            foreach(Framework.DataManager.RbacLog log in logs)
            {
                ListViewItem item = new ListViewItem(log.DateTime.ToString());
                item.SubItems.Add(log.Query);
                item.SubItems.Add(log.ParsedQuery);
                item.SubItems.Add(log.Errors);
                item.SubItems.Add(log.RbacRole.Name);
                item.SubItems.Add(log.RbacUser.UserName);
                item.SubItems.Add(GetTotalTime(log).ToString());
                if (log.ElapsedTimeParseQuery != null)
                    item.SubItems.Add(TimeSpan.FromTicks((long)log.ElapsedTimeParseQuery).ToString());
                else
                    item.SubItems.Add(string.Empty);

                if (log.ElapsedTimeConditionsNRelations != null)
                    item.SubItems.Add(TimeSpan.FromTicks((long)log.ElapsedTimeConditionsNRelations).ToString());
                else
                    item.SubItems.Add(string.Empty);

                if (log.ElapsedTimeApplyPermission != null)
                    item.SubItems.Add(TimeSpan.FromTicks((long)log.ElapsedTimeApplyPermission).ToString());
                else
                    item.SubItems.Add(string.Empty);

                if (log.ElapsedTimeApplyParameters != null)
                    item.SubItems.Add(TimeSpan.FromTicks((long)log.ElapsedTimeApplyParameters).ToString());
                else
                    item.SubItems.Add(string.Empty);

                lvwLogs.Items.Add(item);
            }
        }

        private TimeSpan GetTotalTime(Framework.DataManager.RbacLog log)
        {
            long totaltime = 0;

            if (log.ElapsedTimeParseQuery != null)
                totaltime += (long)log.ElapsedTimeParseQuery;

            if (log.ElapsedTimeConditionsNRelations != null)
                totaltime += (long)log.ElapsedTimeConditionsNRelations;

            if (log.ElapsedTimeApplyPermission != null)
                totaltime += (long)log.ElapsedTimeApplyPermission;

            if (log.ElapsedTimeApplyParameters != null)
                totaltime += (long)log.ElapsedTimeApplyParameters;

            return TimeSpan.FromTicks(totaltime);
        }

        private void lvwLogs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwLogs.SelectedItems.Count > 0)
            {
                txtQuery.Text = lvwLogs.SelectedItems[0].SubItems[1].Text;
                txtParsedQuery.Text = lvwLogs.SelectedItems[0].SubItems[2].Text;
                txtErrors.Text = lvwLogs.SelectedItems[0].SubItems[3].Text;

                LogProperty logProperty = new LogProperty();

                logProperty.DateTime = lvwLogs.SelectedItems[0].Text;
                logProperty.UserName = lvwLogs.SelectedItems[0].SubItems[4].Text;
                logProperty.RoleName = lvwLogs.SelectedItems[0].SubItems[5].Text;
                logProperty.TotalTime = lvwLogs.SelectedItems[0].SubItems[6].Text;
                logProperty.ParseTime = lvwLogs.SelectedItems[0].SubItems[7].Text;
                logProperty.ConditionTime = lvwLogs.SelectedItems[0].SubItems[8].Text;
                logProperty.PermissionTime = lvwLogs.SelectedItems[0].SubItems[9].Text;
                logProperty.ParameterTime = lvwLogs.SelectedItems[0].SubItems[10].Text;

                propLog.SelectedObject = logProperty;
            }
        }
    }

    public class LogProperty
    {
        public string DateTime { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string TotalTime { get; set; }
        public string ParseTime { get; set; }
        public string ConditionTime { get; set; }
        public string PermissionTime { get; set; }
        public string ParameterTime { get; set; }
    }
}
