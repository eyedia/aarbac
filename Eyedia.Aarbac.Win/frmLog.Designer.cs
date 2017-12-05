namespace Eyedia.Aarbac.Win
{
    partial class frmLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLog));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lvwLogs = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.txtQuery = new FastColoredTextBoxNS.FastColoredTextBox();
            this.txtParsedQuery = new FastColoredTextBoxNS.FastColoredTextBox();
            this.txtErrors = new FastColoredTextBoxNS.FastColoredTextBox();
            this.propLog = new System.Windows.Forms.PropertyGrid();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParsedQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 776);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1258, 37);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(79, 32);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 683);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1258, 93);
            this.pnlBottom.TabIndex = 1;
            this.pnlBottom.Visible = false;
            // 
            // lvwLogs
            // 
            this.lvwLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
            this.lvwLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwLogs.FullRowSelect = true;
            this.lvwLogs.Location = new System.Drawing.Point(0, 0);
            this.lvwLogs.Name = "lvwLogs";
            this.lvwLogs.Size = new System.Drawing.Size(821, 683);
            this.lvwLogs.TabIndex = 2;
            this.lvwLogs.UseCompatibleStateImageBehavior = false;
            this.lvwLogs.View = System.Windows.Forms.View.Details;
            this.lvwLogs.SelectedIndexChanged += new System.EventHandler(this.lvwLogs_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Date & Time";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Query";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Parsed Query";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Errors";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Role";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "User";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Total Elapsed Time";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Parsing";
            this.columnHeader8.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Conditions";
            this.columnHeader9.Width = 100;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Permission";
            this.columnHeader10.Width = 100;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Parameters";
            this.columnHeader11.Width = 100;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvwLogs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1258, 683);
            this.splitContainer1.SplitterDistance = 821;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtQuery);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(433, 683);
            this.splitContainer2.SplitterDistance = 171;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.txtParsedQuery);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(433, 508);
            this.splitContainer3.SplitterDistance = 172;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.txtErrors);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.propLog);
            this.splitContainer4.Size = new System.Drawing.Size(433, 332);
            this.splitContainer4.SplitterDistance = 107;
            this.splitContainer4.TabIndex = 0;
            // 
            // txtQuery
            // 
            this.txtQuery.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.txtQuery.AutoIndentCharsPatterns = "";
            this.txtQuery.AutoScrollMinSize = new System.Drawing.Size(0, 29);
            this.txtQuery.BackBrush = null;
            this.txtQuery.CharHeight = 29;
            this.txtQuery.CharWidth = 16;
            this.txtQuery.CommentPrefix = "--";
            this.txtQuery.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtQuery.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.IsReplaceMode = false;
            this.txtQuery.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtQuery.LeftBracket = '(';
            this.txtQuery.Location = new System.Drawing.Point(0, 0);
            this.txtQuery.Margin = new System.Windows.Forms.Padding(4);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Paddings = new System.Windows.Forms.Padding(0);
            this.txtQuery.RightBracket = ')';
            this.txtQuery.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtQuery.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtQuery.ServiceColors")));
            this.txtQuery.ShowLineNumbers = false;
            this.txtQuery.Size = new System.Drawing.Size(433, 171);
            this.txtQuery.TabIndex = 18;
            this.txtQuery.WordWrap = true;
            this.txtQuery.Zoom = 100;
            // 
            // txtParsedQuery
            // 
            this.txtParsedQuery.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.txtParsedQuery.AutoIndentCharsPatterns = "";
            this.txtParsedQuery.AutoScrollMinSize = new System.Drawing.Size(0, 29);
            this.txtParsedQuery.BackBrush = null;
            this.txtParsedQuery.CharHeight = 29;
            this.txtParsedQuery.CharWidth = 16;
            this.txtParsedQuery.CommentPrefix = "--";
            this.txtParsedQuery.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtParsedQuery.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtParsedQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtParsedQuery.IsReplaceMode = false;
            this.txtParsedQuery.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtParsedQuery.LeftBracket = '(';
            this.txtParsedQuery.Location = new System.Drawing.Point(0, 0);
            this.txtParsedQuery.Margin = new System.Windows.Forms.Padding(4);
            this.txtParsedQuery.Name = "txtParsedQuery";
            this.txtParsedQuery.Paddings = new System.Windows.Forms.Padding(0);
            this.txtParsedQuery.RightBracket = ')';
            this.txtParsedQuery.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtParsedQuery.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtParsedQuery.ServiceColors")));
            this.txtParsedQuery.ShowLineNumbers = false;
            this.txtParsedQuery.Size = new System.Drawing.Size(433, 172);
            this.txtParsedQuery.TabIndex = 18;
            this.txtParsedQuery.WordWrap = true;
            this.txtParsedQuery.Zoom = 100;
            // 
            // txtErrors
            // 
            this.txtErrors.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.txtErrors.AutoIndentCharsPatterns = "";
            this.txtErrors.AutoScrollMinSize = new System.Drawing.Size(0, 29);
            this.txtErrors.BackBrush = null;
            this.txtErrors.CharHeight = 29;
            this.txtErrors.CharWidth = 16;
            this.txtErrors.CommentPrefix = "--";
            this.txtErrors.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtErrors.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErrors.IsReplaceMode = false;
            this.txtErrors.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtErrors.LeftBracket = '(';
            this.txtErrors.Location = new System.Drawing.Point(0, 0);
            this.txtErrors.Margin = new System.Windows.Forms.Padding(4);
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.Paddings = new System.Windows.Forms.Padding(0);
            this.txtErrors.RightBracket = ')';
            this.txtErrors.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtErrors.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtErrors.ServiceColors")));
            this.txtErrors.ShowLineNumbers = false;
            this.txtErrors.Size = new System.Drawing.Size(433, 107);
            this.txtErrors.TabIndex = 18;
            this.txtErrors.WordWrap = true;
            this.txtErrors.Zoom = 100;
            // 
            // propLog
            // 
            this.propLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propLog.HelpVisible = false;
            this.propLog.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propLog.Location = new System.Drawing.Point(0, 0);
            this.propLog.Name = "propLog";
            this.propLog.Size = new System.Drawing.Size(433, 221);
            this.propLog.TabIndex = 0;
            // 
            // frmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 813);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLog";
            this.Text = "aarbac - Logs";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParsedQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtErrors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ListView lvwLogs;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private FastColoredTextBoxNS.FastColoredTextBox txtQuery;
        private FastColoredTextBoxNS.FastColoredTextBox txtParsedQuery;
        private FastColoredTextBoxNS.FastColoredTextBox txtErrors;
        private System.Windows.Forms.PropertyGrid propLog;
    }
}