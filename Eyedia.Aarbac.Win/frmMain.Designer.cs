namespace Eyedia.Aarbac.Win
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerBase = new System.Windows.Forms.SplitContainer();
            this.lvwQueries = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExecuteAll = new System.Windows.Forms.Button();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.cbRoles = new System.Windows.Forms.ComboBox();
            this.cbUsers = new System.Windows.Forms.ComboBox();
            this.cbInstances = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtQuery = new FastColoredTextBoxNS.FastColoredTextBox();
            this.txtParsedQuerys1 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.txtParsedQuery = new FastColoredTextBoxNS.FastColoredTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.engineInput = new System.Windows.Forms.PropertyGrid();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propInstance = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSaveInstance = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.propUser = new System.Windows.Forms.PropertyGrid();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSaveUser = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtRole = new FastColoredTextBoxNS.FastColoredTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtEntitlements = new FastColoredTextBoxNS.FastColoredTextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnSaveRole = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.propRole = new System.Windows.Forms.PropertyGrid();
            this.label12 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.batchTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadQueriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLoadedQueriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.usertabPage1 = new System.Windows.Forms.TabPage();
            this.usertabPage2 = new System.Windows.Forms.TabPage();
            this.lvwUserParameters = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBase)).BeginInit();
            this.splitContainerBase.Panel1.SuspendLayout();
            this.splitContainerBase.Panel2.SuspendLayout();
            this.splitContainerBase.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParsedQuerys1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParsedQuery)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEntitlements)).BeginInit();
            this.panel5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.usertabPage1.SuspendLayout();
            this.usertabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 509);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(857, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainerBase
            // 
            this.splitContainerBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerBase.Location = new System.Drawing.Point(0, 33);
            this.splitContainerBase.Name = "splitContainerBase";
            // 
            // splitContainerBase.Panel1
            // 
            this.splitContainerBase.Panel1.Controls.Add(this.lvwQueries);
            this.splitContainerBase.Panel1.Controls.Add(this.panel3);
            this.splitContainerBase.Panel1Collapsed = true;
            // 
            // splitContainerBase.Panel2
            // 
            this.splitContainerBase.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainerBase.Size = new System.Drawing.Size(857, 476);
            this.splitContainerBase.SplitterDistance = 153;
            this.splitContainerBase.TabIndex = 1;
            // 
            // lvwQueries
            // 
            this.lvwQueries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvwQueries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwQueries.Location = new System.Drawing.Point(0, 0);
            this.lvwQueries.Name = "lvwQueries";
            this.lvwQueries.Size = new System.Drawing.Size(153, 57);
            this.lvwQueries.TabIndex = 1;
            this.lvwQueries.UseCompatibleStateImageBehavior = false;
            this.lvwQueries.View = System.Windows.Forms.View.Details;
            this.lvwQueries.SelectedIndexChanged += new System.EventHandler(this.lvwQueries_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Query";
            this.columnHeader1.Width = 200;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExecuteAll);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 57);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(153, 43);
            this.panel3.TabIndex = 2;
            // 
            // btnExecuteAll
            // 
            this.btnExecuteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecuteAll.Location = new System.Drawing.Point(47, 3);
            this.btnExecuteAll.Name = "btnExecuteAll";
            this.btnExecuteAll.Size = new System.Drawing.Size(103, 37);
            this.btnExecuteAll.TabIndex = 0;
            this.btnExecuteAll.Text = "Execute All";
            this.btnExecuteAll.UseVisualStyleBackColor = true;
            this.btnExecuteAll.Click += new System.EventHandler(this.btnExecuteAll_Click);
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRight.Name = "splitContainerRight";
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.tabControl1);
            this.splitContainerRight.Size = new System.Drawing.Size(857, 476);
            this.splitContainerRight.SplitterDistance = 325;
            this.splitContainerRight.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.cbRoles, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.cbUsers, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.cbInstances, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.btnExecute, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.txtQuery, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtParsedQuerys1, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.txtParsedQuery, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(325, 476);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(3, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Role";
            // 
            // cbRoles
            // 
            this.cbRoles.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbRoles.FormattingEnabled = true;
            this.cbRoles.Location = new System.Drawing.Point(68, 149);
            this.cbRoles.Name = "cbRoles";
            this.cbRoles.Size = new System.Drawing.Size(254, 28);
            this.cbRoles.TabIndex = 5;
            this.cbRoles.SelectedIndexChanged += new System.EventHandler(this.cbRoles_SelectedIndexChanged);
            // 
            // cbUsers
            // 
            this.cbUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbUsers.FormattingEnabled = true;
            this.cbUsers.Location = new System.Drawing.Point(68, 115);
            this.cbUsers.Name = "cbUsers";
            this.cbUsers.Size = new System.Drawing.Size(254, 28);
            this.cbUsers.TabIndex = 4;
            this.cbUsers.SelectedIndexChanged += new System.EventHandler(this.cbUsers_SelectedIndexChanged);
            // 
            // cbInstances
            // 
            this.cbInstances.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbInstances.FormattingEnabled = true;
            this.cbInstances.Location = new System.Drawing.Point(68, 75);
            this.cbInstances.Name = "cbInstances";
            this.cbInstances.Size = new System.Drawing.Size(254, 28);
            this.cbInstances.TabIndex = 1;
            this.cbInstances.SelectedIndexChanged += new System.EventHandler(this.cbInstances_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(3, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(3, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "Instance";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(3, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 20);
            this.label6.TabIndex = 7;
            this.label6.Text = "Query";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 308);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 60);
            this.label7.TabIndex = 9;
            this.label7.Text = "Parsed Query(s1)";
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.Location = new System.Drawing.Point(224, 266);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(98, 39);
            this.btnExecute.TabIndex = 11;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 391);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 40);
            this.label1.TabIndex = 12;
            this.label1.Text = "Parsed Query";
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
            this.txtQuery.AutoScrollMinSize = new System.Drawing.Size(2, 22);
            this.txtQuery.BackBrush = null;
            this.txtQuery.CharHeight = 22;
            this.txtQuery.CharWidth = 12;
            this.txtQuery.CommentPrefix = "--";
            this.txtQuery.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtQuery.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.IsReplaceMode = false;
            this.txtQuery.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtQuery.LeftBracket = '(';
            this.txtQuery.Location = new System.Drawing.Point(68, 183);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Paddings = new System.Windows.Forms.Padding(0);
            this.txtQuery.RightBracket = ')';
            this.txtQuery.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtQuery.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtQuery.ServiceColors")));
            this.txtQuery.ShowLineNumbers = false;
            this.txtQuery.Size = new System.Drawing.Size(254, 77);
            this.txtQuery.TabIndex = 17;
            this.txtQuery.Zoom = 100;
            // 
            // txtParsedQuerys1
            // 
            this.txtParsedQuerys1.AutoCompleteBracketsList = new char[] {
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
            this.txtParsedQuerys1.AutoIndentCharsPatterns = "";
            this.txtParsedQuerys1.AutoScrollMinSize = new System.Drawing.Size(2, 22);
            this.txtParsedQuerys1.BackBrush = null;
            this.txtParsedQuerys1.CharHeight = 22;
            this.txtParsedQuerys1.CharWidth = 12;
            this.txtParsedQuerys1.CommentPrefix = "--";
            this.txtParsedQuerys1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtParsedQuerys1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtParsedQuerys1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtParsedQuerys1.IsReplaceMode = false;
            this.txtParsedQuerys1.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtParsedQuerys1.LeftBracket = '(';
            this.txtParsedQuerys1.Location = new System.Drawing.Point(68, 311);
            this.txtParsedQuerys1.Name = "txtParsedQuerys1";
            this.txtParsedQuerys1.Paddings = new System.Windows.Forms.Padding(0);
            this.txtParsedQuerys1.RightBracket = ')';
            this.txtParsedQuerys1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtParsedQuerys1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtParsedQuerys1.ServiceColors")));
            this.txtParsedQuerys1.ShowLineNumbers = false;
            this.txtParsedQuerys1.Size = new System.Drawing.Size(254, 77);
            this.txtParsedQuerys1.TabIndex = 18;
            this.txtParsedQuerys1.Zoom = 100;
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
            this.txtParsedQuery.AutoScrollMinSize = new System.Drawing.Size(2, 22);
            this.txtParsedQuery.BackBrush = null;
            this.txtParsedQuery.CharHeight = 22;
            this.txtParsedQuery.CharWidth = 12;
            this.txtParsedQuery.CommentPrefix = "--";
            this.txtParsedQuery.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtParsedQuery.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtParsedQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtParsedQuery.IsReplaceMode = false;
            this.txtParsedQuery.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtParsedQuery.LeftBracket = '(';
            this.txtParsedQuery.Location = new System.Drawing.Point(68, 394);
            this.txtParsedQuery.Name = "txtParsedQuery";
            this.txtParsedQuery.Paddings = new System.Windows.Forms.Padding(0);
            this.txtParsedQuery.RightBracket = ')';
            this.txtParsedQuery.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtParsedQuery.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtParsedQuery.ServiceColors")));
            this.txtParsedQuery.ShowLineNumbers = false;
            this.txtParsedQuery.Size = new System.Drawing.Size(254, 79);
            this.txtParsedQuery.TabIndex = 19;
            this.txtParsedQuery.Zoom = 100;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(68, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(254, 66);
            this.panel1.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(59, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(195, 66);
            this.label5.TabIndex = 17;
            this.label5.Text = "This is only test bed, in real world no way we can change role of a user during r" +
    "un time";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(59, 66);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(528, 476);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Controls.Add(this.txtErrors);
            this.tabPage1.Controls.Add(this.engineInput);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(520, 443);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Engine Properties";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Location = new System.Drawing.Point(3, 191);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(514, 20);
            this.label11.TabIndex = 4;
            this.label11.Text = "Engine Output";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 191);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(514, 180);
            this.treeView1.TabIndex = 0;
            // 
            // txtErrors
            // 
            this.txtErrors.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtErrors.ForeColor = System.Drawing.Color.Red;
            this.txtErrors.Location = new System.Drawing.Point(3, 371);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.Size = new System.Drawing.Size(514, 69);
            this.txtErrors.TabIndex = 3;
            this.txtErrors.Visible = false;
            // 
            // engineInput
            // 
            this.engineInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.engineInput.HelpVisible = false;
            this.engineInput.Location = new System.Drawing.Point(3, 23);
            this.engineInput.Name = "engineInput";
            this.engineInput.Size = new System.Drawing.Size(514, 168);
            this.engineInput.TabIndex = 1;
            this.engineInput.ToolbarVisible = false;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Location = new System.Drawing.Point(3, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(514, 20);
            this.label9.TabIndex = 2;
            this.label9.Text = "Engine Intput";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propInstance);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(520, 443);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Instance";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // propInstance
            // 
            this.propInstance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propInstance.Location = new System.Drawing.Point(3, 3);
            this.propInstance.Name = "propInstance";
            this.propInstance.Size = new System.Drawing.Size(514, 394);
            this.propInstance.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSaveInstance);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 397);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(514, 43);
            this.panel2.TabIndex = 1;
            // 
            // btnSaveInstance
            // 
            this.btnSaveInstance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveInstance.Location = new System.Drawing.Point(425, 3);
            this.btnSaveInstance.Name = "btnSaveInstance";
            this.btnSaveInstance.Size = new System.Drawing.Size(84, 37);
            this.btnSaveInstance.TabIndex = 0;
            this.btnSaveInstance.Text = "Save";
            this.btnSaveInstance.UseVisualStyleBackColor = true;
            this.btnSaveInstance.Click += new System.EventHandler(this.btnSaveInstance_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tabControl2);
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(520, 443);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "User";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // propUser
            // 
            this.propUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propUser.Location = new System.Drawing.Point(3, 3);
            this.propUser.Name = "propUser";
            this.propUser.Size = new System.Drawing.Size(506, 361);
            this.propUser.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnSaveUser);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 400);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(520, 43);
            this.panel4.TabIndex = 2;
            // 
            // btnSaveUser
            // 
            this.btnSaveUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveUser.Location = new System.Drawing.Point(431, 3);
            this.btnSaveUser.Name = "btnSaveUser";
            this.btnSaveUser.Size = new System.Drawing.Size(84, 37);
            this.btnSaveUser.TabIndex = 0;
            this.btnSaveUser.Text = "Save";
            this.btnSaveUser.UseVisualStyleBackColor = true;
            this.btnSaveUser.Click += new System.EventHandler(this.btnSaveUser_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtRole);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.txtEntitlements);
            this.tabPage4.Controls.Add(this.panel5);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.propRole);
            this.tabPage4.Controls.Add(this.label12);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(520, 443);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Role";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtRole
            // 
            this.txtRole.AutoCompleteBracketsList = new char[] {
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
            this.txtRole.AutoIndentCharsPatterns = "";
            this.txtRole.AutoScrollMinSize = new System.Drawing.Size(2, 22);
            this.txtRole.BackBrush = null;
            this.txtRole.CharHeight = 22;
            this.txtRole.CharWidth = 12;
            this.txtRole.CommentPrefix = null;
            this.txtRole.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtRole.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRole.IsReplaceMode = false;
            this.txtRole.Language = FastColoredTextBoxNS.Language.XML;
            this.txtRole.LeftBracket = '<';
            this.txtRole.LeftBracket2 = '(';
            this.txtRole.Location = new System.Drawing.Point(0, 159);
            this.txtRole.Name = "txtRole";
            this.txtRole.Paddings = new System.Windows.Forms.Padding(0);
            this.txtRole.RightBracket = '>';
            this.txtRole.RightBracket2 = ')';
            this.txtRole.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtRole.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtRole.ServiceColors")));
            this.txtRole.ShowLineNumbers = false;
            this.txtRole.Size = new System.Drawing.Size(520, 122);
            this.txtRole.TabIndex = 6;
            this.txtRole.Zoom = 100;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label10.Location = new System.Drawing.Point(0, 281);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(520, 20);
            this.label10.TabIndex = 8;
            this.label10.Text = "Entitlements";
            // 
            // txtEntitlements
            // 
            this.txtEntitlements.AutoCompleteBracketsList = new char[] {
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
            this.txtEntitlements.AutoIndentCharsPatterns = "";
            this.txtEntitlements.AutoScrollMinSize = new System.Drawing.Size(2, 22);
            this.txtEntitlements.BackBrush = null;
            this.txtEntitlements.CharHeight = 22;
            this.txtEntitlements.CharWidth = 12;
            this.txtEntitlements.CommentPrefix = null;
            this.txtEntitlements.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEntitlements.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtEntitlements.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtEntitlements.IsReplaceMode = false;
            this.txtEntitlements.Language = FastColoredTextBoxNS.Language.XML;
            this.txtEntitlements.LeftBracket = '<';
            this.txtEntitlements.LeftBracket2 = '(';
            this.txtEntitlements.Location = new System.Drawing.Point(0, 301);
            this.txtEntitlements.Name = "txtEntitlements";
            this.txtEntitlements.Paddings = new System.Windows.Forms.Padding(0);
            this.txtEntitlements.RightBracket = '>';
            this.txtEntitlements.RightBracket2 = ')';
            this.txtEntitlements.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtEntitlements.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtEntitlements.ServiceColors")));
            this.txtEntitlements.ShowLineNumbers = false;
            this.txtEntitlements.Size = new System.Drawing.Size(520, 99);
            this.txtEntitlements.TabIndex = 9;
            this.txtEntitlements.Zoom = 100;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnSaveRole);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 400);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(520, 43);
            this.panel5.TabIndex = 12;
            // 
            // btnSaveRole
            // 
            this.btnSaveRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveRole.Location = new System.Drawing.Point(431, 3);
            this.btnSaveRole.Name = "btnSaveRole";
            this.btnSaveRole.Size = new System.Drawing.Size(84, 37);
            this.btnSaveRole.TabIndex = 0;
            this.btnSaveRole.Text = "Save";
            this.btnSaveRole.UseVisualStyleBackColor = true;
            this.btnSaveRole.Click += new System.EventHandler(this.btnSaveRole_Click);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Location = new System.Drawing.Point(0, 139);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(520, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Row && Column Permissions";
            // 
            // propRole
            // 
            this.propRole.Dock = System.Windows.Forms.DockStyle.Top;
            this.propRole.HelpVisible = false;
            this.propRole.Location = new System.Drawing.Point(0, 20);
            this.propRole.Name = "propRole";
            this.propRole.Size = new System.Drawing.Size(520, 119);
            this.propRole.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(520, 20);
            this.label12.TabIndex = 10;
            this.label12.Text = "Basic Properties";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.batchTestToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(857, 33);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // batchTestToolStripMenuItem
            // 
            this.batchTestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadQueriesToolStripMenuItem,
            this.showLoadedQueriesToolStripMenuItem});
            this.batchTestToolStripMenuItem.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.batchTestToolStripMenuItem.Name = "batchTestToolStripMenuItem";
            this.batchTestToolStripMenuItem.Size = new System.Drawing.Size(104, 29);
            this.batchTestToolStripMenuItem.Text = "Batch Test";
            // 
            // loadQueriesToolStripMenuItem
            // 
            this.loadQueriesToolStripMenuItem.Name = "loadQueriesToolStripMenuItem";
            this.loadQueriesToolStripMenuItem.Size = new System.Drawing.Size(270, 30);
            this.loadQueriesToolStripMenuItem.Text = "Load Queries";
            this.loadQueriesToolStripMenuItem.Click += new System.EventHandler(this.loadQueriesToolStripMenuItem_Click);
            // 
            // showLoadedQueriesToolStripMenuItem
            // 
            this.showLoadedQueriesToolStripMenuItem.CheckOnClick = true;
            this.showLoadedQueriesToolStripMenuItem.Name = "showLoadedQueriesToolStripMenuItem";
            this.showLoadedQueriesToolStripMenuItem.Size = new System.Drawing.Size(270, 30);
            this.showLoadedQueriesToolStripMenuItem.Text = "Show Loaded Queries";
            this.showLoadedQueriesToolStripMenuItem.Click += new System.EventHandler(this.showLoadedQueriesToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "test.csv";
            this.openFileDialog1.Filter = "\"Csv files|*.csv|All files|*.*\"";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.usertabPage1);
            this.tabControl2.Controls.Add(this.usertabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(520, 400);
            this.tabControl2.TabIndex = 3;
            // 
            // usertabPage1
            // 
            this.usertabPage1.Controls.Add(this.propUser);
            this.usertabPage1.Location = new System.Drawing.Point(4, 29);
            this.usertabPage1.Name = "usertabPage1";
            this.usertabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.usertabPage1.Size = new System.Drawing.Size(512, 367);
            this.usertabPage1.TabIndex = 0;
            this.usertabPage1.Text = "Basic Properties";
            this.usertabPage1.UseVisualStyleBackColor = true;
            // 
            // usertabPage2
            // 
            this.usertabPage2.Controls.Add(this.lvwUserParameters);
            this.usertabPage2.Location = new System.Drawing.Point(4, 29);
            this.usertabPage2.Name = "usertabPage2";
            this.usertabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.usertabPage2.Size = new System.Drawing.Size(512, 367);
            this.usertabPage2.TabIndex = 1;
            this.usertabPage2.Text = "Parameters";
            this.usertabPage2.UseVisualStyleBackColor = true;
            // 
            // lvwUserParameters
            // 
            this.lvwUserParameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.lvwUserParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwUserParameters.FullRowSelect = true;
            this.lvwUserParameters.Location = new System.Drawing.Point(3, 3);
            this.lvwUserParameters.Name = "lvwUserParameters";
            this.lvwUserParameters.Size = new System.Drawing.Size(506, 361);
            this.lvwUserParameters.TabIndex = 0;
            this.lvwUserParameters.UseCompatibleStateImageBehavior = false;
            this.lvwUserParameters.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            this.columnHeader3.Width = 300;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 531);
            this.Controls.Add(this.splitContainerBase);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AARBAC - An Automated Role Based Access Control";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainerBase.Panel1.ResumeLayout(false);
            this.splitContainerBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBase)).EndInit();
            this.splitContainerBase.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
            this.splitContainerRight.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParsedQuerys1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParsedQuery)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEntitlements)).EndInit();
            this.panel5.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.usertabPage1.ResumeLayout(false);
            this.usertabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitContainerBase;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private System.Windows.Forms.ListView lvwQueries;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbInstances;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbUsers;
        private System.Windows.Forms.ComboBox cbRoles;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem batchTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadQueriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLoadedQueriesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private FastColoredTextBoxNS.FastColoredTextBox txtQuery;
        private FastColoredTextBoxNS.FastColoredTextBox txtParsedQuerys1;
        private FastColoredTextBoxNS.FastColoredTextBox txtParsedQuery;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PropertyGrid engineInput;
        private System.Windows.Forms.TextBox txtErrors;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSaveInstance;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExecuteAll;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private FastColoredTextBoxNS.FastColoredTextBox txtRole;
        private System.Windows.Forms.Label label10;
        private FastColoredTextBoxNS.FastColoredTextBox txtEntitlements;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PropertyGrid propInstance;
        private System.Windows.Forms.PropertyGrid propUser;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSaveUser;
        private System.Windows.Forms.PropertyGrid propRole;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnSaveRole;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage usertabPage1;
        private System.Windows.Forms.TabPage usertabPage2;
        private System.Windows.Forms.ListView lvwUserParameters;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}

