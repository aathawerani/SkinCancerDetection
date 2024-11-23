namespace DiagramDesigner
{
    partial class frmSensitivityAnalysis
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSTA = new System.Windows.Forms.TabPage();
            this.grdSA = new System.Windows.Forms.DataGridView();
            this.tabSTI = new System.Windows.Forms.TabPage();
            this.grdSAInfluence = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Child = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAnalyse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboNodes = new System.Windows.Forms.ComboBox();
            this.Node = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.P1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.P0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Diff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.E1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.E0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiffE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabSTA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSA)).BeginInit();
            this.tabSTI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSAInfluence)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnAnalyse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboNodes);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(673, 610);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSTA);
            this.tabControl1.Controls.Add(this.tabSTI);
            this.tabControl1.Location = new System.Drawing.Point(9, 69);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(658, 502);
            this.tabControl1.TabIndex = 4;
            // 
            // tabSTA
            // 
            this.tabSTA.Controls.Add(this.grdSA);
            this.tabSTA.Location = new System.Drawing.Point(4, 22);
            this.tabSTA.Name = "tabSTA";
            this.tabSTA.Padding = new System.Windows.Forms.Padding(3);
            this.tabSTA.Size = new System.Drawing.Size(650, 476);
            this.tabSTA.TabIndex = 0;
            this.tabSTA.Text = "Sensitivity to Action";
            this.tabSTA.UseVisualStyleBackColor = true;
            // 
            // grdSA
            // 
            this.grdSA.AllowUserToAddRows = false;
            this.grdSA.AllowUserToDeleteRows = false;
            this.grdSA.AllowUserToOrderColumns = true;
            this.grdSA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Node,
            this.P1,
            this.P0,
            this.Diff,
            this.E1,
            this.E0,
            this.DiffE});
            this.grdSA.Location = new System.Drawing.Point(6, 6);
            this.grdSA.Name = "grdSA";
            this.grdSA.ReadOnly = true;
            this.grdSA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdSA.Size = new System.Drawing.Size(638, 464);
            this.grdSA.TabIndex = 1;
            // 
            // tabSTI
            // 
            this.tabSTI.Controls.Add(this.grdSAInfluence);
            this.tabSTI.Location = new System.Drawing.Point(4, 22);
            this.tabSTI.Name = "tabSTI";
            this.tabSTI.Padding = new System.Windows.Forms.Padding(3);
            this.tabSTI.Size = new System.Drawing.Size(650, 476);
            this.tabSTI.TabIndex = 1;
            this.tabSTI.Text = "Sensitivity to Influence";
            this.tabSTI.UseVisualStyleBackColor = true;
            // 
            // grdSAInfluence
            // 
            this.grdSAInfluence.AllowUserToAddRows = false;
            this.grdSAInfluence.AllowUserToDeleteRows = false;
            this.grdSAInfluence.AllowUserToOrderColumns = true;
            this.grdSAInfluence.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSAInfluence.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Child,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.Column1,
            this.Column2,
            this.Column3});
            this.grdSAInfluence.Location = new System.Drawing.Point(6, 6);
            this.grdSAInfluence.Name = "grdSAInfluence";
            this.grdSAInfluence.ReadOnly = true;
            this.grdSAInfluence.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdSAInfluence.Size = new System.Drawing.Size(638, 464);
            this.grdSAInfluence.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "From Node";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // Child
            // 
            this.Child.HeaderText = "To Node";
            this.Child.Name = "Child";
            this.Child.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "g Min";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 65;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "g Max";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 65;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "g Diff";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 65;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "h Min";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "h Max";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "h Diff";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(592, 577);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAnalyse
            // 
            this.btnAnalyse.Location = new System.Drawing.Point(347, 31);
            this.btnAnalyse.Name = "btnAnalyse";
            this.btnAnalyse.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyse.TabIndex = 2;
            this.btnAnalyse.Text = "&Analyse";
            this.btnAnalyse.UseVisualStyleBackColor = true;
            this.btnAnalyse.Click += new System.EventHandler(this.btnAnalyse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Node:";
            // 
            // cboNodes
            // 
            this.cboNodes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNodes.FormattingEnabled = true;
            this.cboNodes.Location = new System.Drawing.Point(60, 33);
            this.cboNodes.Name = "cboNodes";
            this.cboNodes.Size = new System.Drawing.Size(281, 21);
            this.cboNodes.TabIndex = 0;
            // 
            // Node
            // 
            this.Node.HeaderText = "Node";
            this.Node.Name = "Node";
            this.Node.ReadOnly = true;
            this.Node.Width = 250;
            // 
            // P1
            // 
            this.P1.HeaderText = "T- Prob";
            this.P1.Name = "P1";
            this.P1.ReadOnly = true;
            this.P1.Width = 70;
            // 
            // P0
            // 
            this.P0.HeaderText = "F- Prob";
            this.P0.Name = "P0";
            this.P0.ReadOnly = true;
            this.P0.Width = 70;
            // 
            // Diff
            // 
            this.Diff.HeaderText = "Diff";
            this.Diff.Name = "Diff";
            this.Diff.ReadOnly = true;
            this.Diff.Width = 70;
            // 
            // E1
            // 
            this.E1.HeaderText = "T- Entropy";
            this.E1.Name = "E1";
            this.E1.ReadOnly = true;
            this.E1.Width = 55;
            // 
            // E0
            // 
            this.E0.HeaderText = "F-Entropy";
            this.E0.Name = "E0";
            this.E0.ReadOnly = true;
            this.E0.Width = 55;
            // 
            // DiffE
            // 
            this.DiffE.HeaderText = "Diff";
            this.DiffE.Name = "DiffE";
            this.DiffE.ReadOnly = true;
            this.DiffE.Width = 65;
            // 
            // frmSensitivityAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 615);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Name = "frmSensitivityAnalysis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sensitivity Analysis";
            this.Load += new System.EventHandler(this.frmSensitivityAnalysis_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabSTA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSA)).EndInit();
            this.tabSTI.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSAInfluence)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdSA;
        private System.Windows.Forms.ComboBox cboNodes;
        private System.Windows.Forms.Button btnAnalyse;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSTA;
        private System.Windows.Forms.TabPage tabSTI;
        private System.Windows.Forms.DataGridView grdSAInfluence;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Child;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Node;
        private System.Windows.Forms.DataGridViewTextBoxColumn P1;
        private System.Windows.Forms.DataGridViewTextBoxColumn P0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Diff;
        private System.Windows.Forms.DataGridViewTextBoxColumn E1;
        private System.Windows.Forms.DataGridViewTextBoxColumn E0;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiffE;
    }
}