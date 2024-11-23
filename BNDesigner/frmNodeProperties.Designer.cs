namespace DiagramDesigner
{
    partial class frmNodeProperties
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPT = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdNMax = new System.Windows.Forms.DataGridView();
            this.chkOverriden = new System.Windows.Forms.CheckBox();
            this.grdCAST = new System.Windows.Forms.DataGridView();
            this.grdCPT = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdStates = new System.Windows.Forms.DataGridView();
            this.txtNewState = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.NodeType = new System.Windows.Forms.Label();
            this.cboNodeType = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtNodeName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNodeID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.E = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.tabPT.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdNMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCAST)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCPT)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStates)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabPT);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.NodeType);
            this.groupBox1.Controls.Add(this.cboNodeType);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.txtNodeName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtNodeID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(660, 590);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // tabPT
            // 
            this.tabPT.Controls.Add(this.tabPage1);
            this.tabPT.Location = new System.Drawing.Point(7, 183);
            this.tabPT.Name = "tabPT";
            this.tabPT.SelectedIndex = 0;
            this.tabPT.Size = new System.Drawing.Size(645, 370);
            this.tabPT.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grdNMax);
            this.tabPage1.Controls.Add(this.chkOverriden);
            this.tabPage1.Controls.Add(this.grdCAST);
            this.tabPage1.Controls.Add(this.grdCPT);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(637, 344);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grdNMax
            // 
            this.grdNMax.AllowUserToAddRows = false;
            this.grdNMax.AllowUserToDeleteRows = false;
            this.grdNMax.AllowUserToResizeRows = false;
            this.grdNMax.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grdNMax.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.grdNMax.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdNMax.Location = new System.Drawing.Point(5, 32);
            this.grdNMax.Name = "grdNMax";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdNMax.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grdNMax.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.grdNMax.Size = new System.Drawing.Size(624, 70);
            this.grdNMax.TabIndex = 13;
            // 
            // chkOverriden
            // 
            this.chkOverriden.AutoSize = true;
            this.chkOverriden.Location = new System.Drawing.Point(558, 125);
            this.chkOverriden.Name = "chkOverriden";
            this.chkOverriden.Size = new System.Drawing.Size(72, 17);
            this.chkOverriden.TabIndex = 12;
            this.chkOverriden.Text = "Overriden";
            this.chkOverriden.UseVisualStyleBackColor = true;
            this.chkOverriden.CheckedChanged += new System.EventHandler(this.chkOverriden_CheckedChanged);
            // 
            // grdCAST
            // 
            this.grdCAST.AllowUserToAddRows = false;
            this.grdCAST.AllowUserToDeleteRows = false;
            this.grdCAST.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCAST.Location = new System.Drawing.Point(6, 6);
            this.grdCAST.Name = "grdCAST";
            this.grdCAST.Size = new System.Drawing.Size(624, 113);
            this.grdCAST.TabIndex = 11;
            // 
            // grdCPT
            // 
            this.grdCPT.AllowUserToAddRows = false;
            this.grdCPT.AllowUserToDeleteRows = false;
            this.grdCPT.AllowUserToResizeRows = false;
            this.grdCPT.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grdCPT.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.grdCPT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCPT.Location = new System.Drawing.Point(4, 148);
            this.grdCPT.Name = "grdCPT";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdCPT.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grdCPT.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.grdCPT.Size = new System.Drawing.Size(625, 190);
            this.grdCPT.TabIndex = 10;
            this.grdCPT.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdCPT_CellContentClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdStates);
            this.groupBox2.Controls.Add(this.txtNewState);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.btnRemove);
            this.groupBox2.Location = new System.Drawing.Point(9, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(292, 119);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "States";
            // 
            // grdStates
            // 
            this.grdStates.AllowUserToAddRows = false;
            this.grdStates.AllowUserToDeleteRows = false;
            this.grdStates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdStates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.E,
            this.State,
            this.P});
            this.grdStates.Location = new System.Drawing.Point(7, 41);
            this.grdStates.Name = "grdStates";
            this.grdStates.RowHeadersVisible = false;
            this.grdStates.Size = new System.Drawing.Size(279, 72);
            this.grdStates.TabIndex = 13;
            this.grdStates.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdStates_CellComitEdit);
            // 
            // txtNewState
            // 
            this.txtNewState.AcceptsReturn = true;
            this.txtNewState.Enabled = false;
            this.txtNewState.Location = new System.Drawing.Point(6, 15);
            this.txtNewState.Name = "txtNewState";
            this.txtNewState.Size = new System.Drawing.Size(224, 20);
            this.txtNewState.TabIndex = 6;
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(236, 13);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(265, 13);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(23, 23);
            this.btnRemove.TabIndex = 14;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.button2_Click);
            // 
            // NodeType
            // 
            this.NodeType.AutoSize = true;
            this.NodeType.Location = new System.Drawing.Point(404, 28);
            this.NodeType.Name = "NodeType";
            this.NodeType.Size = new System.Drawing.Size(60, 13);
            this.NodeType.TabIndex = 16;
            this.NodeType.Text = "Node Type";
            this.NodeType.Click += new System.EventHandler(this.label4_Click);
            // 
            // cboNodeType
            // 
            this.cboNodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNodeType.Enabled = false;
            this.cboNodeType.FormattingEnabled = true;
            this.cboNodeType.Items.AddRange(new object[] {
            "CPT",
            "NoisyMax",
            "CAST",
            "NoisyOR"});
            this.cboNodeType.Location = new System.Drawing.Point(470, 24);
            this.cboNodeType.Name = "cboNodeType";
            this.cboNodeType.Size = new System.Drawing.Size(121, 21);
            this.cboNodeType.TabIndex = 15;
            this.cboNodeType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.button1.Location = new System.Drawing.Point(9, 557);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "1- S";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(577, 559);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(496, 559);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtNodeName
            // 
            this.txtNodeName.BackColor = System.Drawing.SystemColors.Window;
            this.txtNodeName.Location = new System.Drawing.Point(189, 25);
            this.txtNodeName.Name = "txtNodeName";
            this.txtNodeName.Size = new System.Drawing.Size(197, 20);
            this.txtNodeName.TabIndex = 3;
            this.txtNodeName.TextChanged += new System.EventHandler(this.txtNodeName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name:";
            // 
            // txtNodeID
            // 
            this.txtNodeID.Location = new System.Drawing.Point(62, 25);
            this.txtNodeID.Name = "txtNodeID";
            this.txtNodeID.ReadOnly = true;
            this.txtNodeID.Size = new System.Drawing.Size(73, 20);
            this.txtNodeID.TabIndex = 1;
            this.txtNodeID.TextChanged += new System.EventHandler(this.txtNodeID_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Node ID;";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(637, 344);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "NoisyMax";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // E
            // 
            this.E.HeaderText = "E";
            this.E.Name = "E";
            this.E.ReadOnly = true;
            this.E.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.E.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.E.Width = 25;
            // 
            // State
            // 
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.Width = 140;
            // 
            // P
            // 
            this.P.HeaderText = "P";
            this.P.Name = "P";
            this.P.ReadOnly = true;
            this.P.Width = 90;
            // 
            // frmNodeProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(684, 607);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Name = "frmNodeProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Node Properties";
            this.Load += new System.EventHandler(this.frmNodeProperties_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPT.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdNMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCAST)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCPT)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStates)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNodeID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNodeName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtNewState;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridView grdCPT;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView grdStates;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label NodeType;
        private System.Windows.Forms.ComboBox cboNodeType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabPT;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView grdCAST;
        private System.Windows.Forms.CheckBox chkOverriden;
        private System.Windows.Forms.DataGridView grdNMax;
        private System.Windows.Forms.DataGridViewCheckBoxColumn E;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn P;
    }
}