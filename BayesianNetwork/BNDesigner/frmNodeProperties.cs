using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using IBAyes.Bayesian;


namespace DiagramDesigner
{
    public partial class frmNodeProperties : Form
    {
        Node bnNode;
        StringCollection deletedStates;
        bool bNodeNameChanged;

        const int COL_E=0;
        const int COL_State = 1;
        const int COL_P = 2;


        public frmNodeProperties()
        {
            InitializeComponent();
        }

        private void frmNodeProperties_Load(object sender, EventArgs e)
        {
            if (txtNodeName.Text.CompareTo(bnNode.Name) != 0)
                bNodeNameChanged = true;

            txtNodeID.Text = bnNode.NodeID.ToString();
            txtNodeName.Text = bnNode.Name.ToString();
            chkOverriden.Checked = bnNode.Overriden;
            grdStates.Columns[COL_P].ReadOnly = true;
            grdStates.Columns[COL_E].Visible = false;

            if (bnNode.NodeType == enmNodeType.General)
            {
                grdCAST.Visible = false;
                grdNMax.Visible = false;
                grdCPT.Height = grdCPT.Height + grdCAST.Height + 10;
                grdCPT.Top = grdCAST.Top;
                chkOverriden.Visible = false;
                chkOverriden.Checked = false;
                tabPage1.Text = "General";
            }

            if (bnNode.NodeType == enmNodeType.CAST ^ bnNode.NodeType == enmNodeType.NoisyOR)
            {
                grdCAST.Visible = true;
                grdNMax.Visible = false;
                grdCPT.ReadOnly = !(chkOverriden.Checked);
                tabPage1.Text = bnNode.NodeType == enmNodeType.CAST ? "CAST" : "NoisyOR";
            }

            if (bnNode.NodeType == enmNodeType.NoisyMax)
            {
                grdCAST.Visible = false;
                grdNMax.Visible = true;
                grdCPT.ReadOnly = !(chkOverriden.Checked);
                tabPage1.Text = "NoisyMax";
                grdNMax.Top = grdCAST.Top;
            }

            switch (bnNode.NodeType)
            {
                case enmNodeType.General:
                    cboNodeType.SelectedIndex = 0; break;
                case enmNodeType.NoisyMax:
                    cboNodeType.SelectedIndex = 1; break;
                case enmNodeType.CAST:
                    cboNodeType.SelectedIndex = 2; break;
                case enmNodeType.NoisyOR:
                    cboNodeType.SelectedIndex = 3; break;
            }

            deletedStates = new StringCollection();
            
            String str;
            for (int i = 0; i<=bnNode.States.Count-1;i++)
            {
                str = bnNode.States[i];
                AddState(str, bnNode.EvidenceOn == i,bnNode.GetPosteriorProbab(i));
            }

            //Generate CPT grid structure

            GenerateCPTStructure(bnNode);

            if (bnNode.Parents.Count <= 9)
            {
                //Display CPT probabilities in grid

                for (int i = 0; i < bnNode.CPT.Rows; i++)
                {
                    for (int j = 0; j < bnNode.CPT.Columns; j++)
                    {
                        grdCPT[j, i + bnNode.Parents.Count].Value = bnNode.CPT.GetValue(i, j);
                    }
                }
                //chkOverriden.Enabled = false;
            }

            if (bnNode.NodeType == enmNodeType.NoisyMax)
            {
                //Display NMax probabilities in grid
                for (int i = 0; i < bnNode.NOPT.Rows; i++)
                {
                    for (int j = 0; j < bnNode.NOPT.Columns; j++)
                    {
                        grdNMax[j, i + 2].Value = bnNode.NOPT.GetValue(i, j);
                        if ((i  == bnNode.NoOfStates - 1) & (grdNMax.Columns[j].ReadOnly == true))
                            grdNMax[j, i + 2].Value = 1;
                    }
                }
            }

            if (bnNode.NodeType == enmNodeType.CAST ^ bnNode.NodeType == enmNodeType.NoisyOR)
            {
                //Display CAST probabilities in grid
                for (int i = 0; i < bnNode.CASTPT.Rows; i++)
                {
                    grdCAST[bnNode.CASTPT.Columns - 1, i + 2].Value = bnNode.CASTPT.GetValue(i, bnNode.CASTPT.Columns - 1);
                    for (int j = 0; j < bnNode.CASTPT.Columns-1; j++)
                    {
                        if (bnNode.NodeType == enmNodeType.CAST)
                        {
                            //grdCAST[j, i + 2].Value = bnNode.CASTPT.GetValue(i, j);
                            if (bnNode.CASTPT.GetValue(i, j) == 0.9)
                                grdCAST[j, i + 2].Value = "High +";
                            else if (bnNode.CASTPT.GetValue(i, j) == 0.6)
                                grdCAST[j, i + 2].Value = "Medium +";
                            else if (bnNode.CASTPT.GetValue(i, j) == 0.3)
                                grdCAST[j, i + 2].Value = "Low +";
                            else if (bnNode.CASTPT.GetValue(i, j) == 0)
                                grdCAST[j, i + 2].Value = "None";
                            if (bnNode.CASTPT.GetValue(i, j) == -0.3)
                                grdCAST[j, i + 2].Value = "Low -";
                            if (bnNode.CASTPT.GetValue(i, j) == -0.6)
                                grdCAST[j, i + 2].Value = "Medium -";
                            if (bnNode.CASTPT.GetValue(i, j) == -0.9)
                                grdCAST[j, i + 2].Value = "High -";
                        }
                        else if (bnNode.NodeType == enmNodeType.NoisyOR)
                        {
                            grdCAST[j, i + 2].Value = bnNode.CASTPT.GetValue(i, j);
                        }
                    }

                }
            }

        }

        public bool NodeNameChanged
        {
            get{return bNodeNameChanged;}
        }

        public void ShowNodePropertiesDialog(Node CurNode)
        {
            bnNode = CurNode;
            ShowDialog();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool hasCASTChild;

            hasCASTChild = false;
            foreach (Node n in bnNode.Chidren)
            {
                if (n.NodeType == enmNodeType.CAST)
                {
                    hasCASTChild = true;
                    break;
                }
            }
            if (bnNode.NodeType == enmNodeType.CAST ^ hasCASTChild)
            {
                MessageBox.Show("New states cannot be added. CAST Node doesn't support more than 2 states.");
                return;
            }
            if (txtNewState.Text != "" )
            {
                AddState(txtNewState.Text.Trim(), false, Double.NaN);
                int rowIndex = grdCPT.Rows.Add();
                grdCPT.Rows[rowIndex].HeaderCell.Value=txtNewState.Text.Trim();
                txtNewState.Text = "";

                if (bnNode.NodeType == enmNodeType.NoisyMax)
                {
                    rowIndex = grdNMax.Rows.Add();
                    grdNMax.Rows[rowIndex].HeaderCell.Value = txtNewState.Text.Trim();
                    txtNewState.Text = "";
                }
            }
            txtNewState.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            try
            {
                
                if (!ProbabilitiesValidated())
                    return;

                // check that spaces are not entered in state names
                
                for (int i = 0; i < grdStates.Rows.Count; i++)
                {
                    string str = grdStates[COL_State, i].Value.ToString();
                    if (str.Contains(" ") || str.Contains("$") || str.Contains("^") || str.Contains("#") | str.Contains("-") || str.Contains("&") || str.Contains("@") || str.Contains("!") || str.Contains("%") || str.Contains("~"))
                    {
                     MessageBox.Show("State names can only contain alphanumeric and underscore", "IBAyes Error", MessageBoxButtons.OK);
                     grdStates[COL_State, 0].Value = bnNode.States[0];
                     grdStates[COL_State, 1].Value = bnNode.States[1];
                     return;
                    }
                }
                
                double value;


                bnNode.Name = txtNodeName.Text;
                if (cboNodeType.SelectedIndex == 0)
                    bnNode.NodeType = enmNodeType.General;
                else if (cboNodeType.SelectedIndex == 1)
                    bnNode.NodeType = enmNodeType.NoisyMax;
                else if (cboNodeType.SelectedIndex == 2)
                    bnNode.NodeType = enmNodeType.CAST;

                bnNode.Overriden = chkOverriden.Checked;

                //Save Node states

                //Remove deleted states
                foreach (String str in deletedStates)
                {
                    bnNode.RemoveState(str);
                }

                //Add newly added states
                for (int i = 0; i < grdStates.Rows.Count; i++)
                {
                    string str = grdStates[COL_State, i].Value.ToString();
                    if (!bnNode.States.Contains(str))
                    {
                        bnNode.AddState(str);
                    }
                }

                //Save CPT values in node
                if ((bnNode.NodeType == enmNodeType.General) ^ chkOverriden.Checked)
                {
                    for (int i = bnNode.Parents.Count; i < grdCPT.Rows.Count; i++)
                    {
                        for (int j = 0; j < grdCPT.Columns.Count; j++)
                        {
                            value = Convert.ToDouble(grdCPT[j, i].Value);
                            bnNode.CPT.SetValue(i - bnNode.Parents.Count, j, value);
                        }
                    }
                }

                //In case of NoisyMax, save NMax probabs too.
                if (bnNode.NodeType == enmNodeType.NoisyMax)
                {
                    for (int i = 2; i < grdNMax.Rows.Count; i++)
                    {
                        for (int j = 0; j < grdNMax.Columns.Count; j++)
                        {
                            value = Convert.ToDouble(grdNMax[j, i].Value);
                            bnNode.NOPT.SetValue(i - 2, j, value);
                        }
                    }
                }

                //In case of CAST, save CAST probabs too.
                if (bnNode.NodeType == enmNodeType.CAST)
                {
                    for (int i = 2; i < grdCAST.Rows.Count; i++)
                    {
                        bnNode.CASTPT.SetValue(0, grdCAST.Columns.Count-1,Convert.ToDouble(grdCAST[grdCAST.Columns.Count-1, 2].Value)) ;
                        for (int j = 0; j < grdCAST.Columns.Count-1; j++)
                        {
                            if (grdCAST[j, i].Value.ToString() == "High +")
                                bnNode.CASTPT.SetValue(i-2, j,0.9);
                            else if (grdCAST[j, i].Value.ToString() == "Medium +")
                                bnNode.CASTPT.SetValue(i-2, j,0.6);
                            else if (grdCAST[j, i].Value.ToString() == "Low +")
                                bnNode.CASTPT.SetValue(i-2, j,0.3);
                            else if (grdCAST[j, i].Value.ToString() == "None")
                                bnNode.CASTPT.SetValue(i-2, j,0);
                            else if (grdCAST[j, i].Value.ToString() == "Low -")
                                bnNode.CASTPT.SetValue(i-2, j,-0.3);
                            else if (grdCAST[j, i].Value.ToString() == "Medium -")
                                bnNode.CASTPT.SetValue(i-2, j,-0.6);
                            else if (grdCAST[j, i].Value.ToString() == "High -")
                                bnNode.CASTPT.SetValue(i-2, j,-0.9);

                            //value = Convert.ToDouble(grdCAST[j, i].Value);
                            //bnNode.CASTPT.SetValue(i - 2, j, value);
                        }
                    }
                }

                if (bnNode.NodeType == enmNodeType.NoisyOR)
                {
                    for (int i = 2; i < grdCAST.Rows.Count; i++)
                    {
                        for (int j = 0; j < grdCAST.Columns.Count; j++)
                        {
                            if (j % 2 == 1)
                                value = 0;

                            value = Convert.ToDouble(grdCAST[j, i].Value);
                            bnNode.CASTPT.SetValue(i - 2, j, value);
                        }
                    }
                }

                if (!(chkOverriden.Checked) & (bnNode.NodeType == enmNodeType.CAST ^ bnNode.NodeType == enmNodeType.NoisyOR))
                    bnNode.GenerateCPTforCASTNode();

                this.Close();
            }
            catch (Exception ex)
            {
                   MessageBox.Show(ex.Message,"IBAyes Error",MessageBoxButtons.OK);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            grdStates[COL_State, 0].Value = bnNode.States[0];
            grdStates[COL_State, 1].Value = bnNode.States[1];
            this.Close();
        }

        private void lstStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void txtNodeID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNodeName_TextChanged(object sender, EventArgs e)
        {

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void GenerateCPTStructure(Node CurNode)
        {
            int i = 1,j, k;
            int statesCount;
            int totalcolsCPT, totalColsNOR, totalColsCAST;

            grdCPT.Rows.Clear();
            grdCPT.Columns.Clear();

            grdNMax.Rows.Clear();
            grdNMax.Columns.Clear();

            
            totalcolsCPT =1;

            totalColsNOR = 0;
            totalColsCAST =0;

            statesCount = grdStates.Rows.Count;

            //Total columns in grid = product of NoofStates of all parents.
            for (i = 0; i < CurNode.Parents.Count; i++)
            {
                totalcolsCPT = totalcolsCPT * ((Node)CurNode.Parents[i]).NoOfStates;
                totalColsNOR = totalColsNOR + ((Node)CurNode.Parents[i]).NoOfStates;
            }

            totalColsCAST = CurNode.Parents.Count * 2;

            //Leak/Baseline COlumn
            totalColsNOR++;
            totalColsCAST++;

            if (bnNode.Parents.Count <= 9)
            {
                //Add columns in grid
                for (i = 1; i <= totalcolsCPT; i++)
                {
                    grdCPT.Columns.Add("col" + i.ToString(), "");
                }

                //Add one row for each parent of this node
                if (CurNode.Parents.Count > 0)
                    grdCPT.Rows.Add(CurNode.Parents.Count);

                //Add one row for each state of this node
                for (i = 0; i < statesCount; i++)
                {
                    grdCPT.Rows.Add();
                    grdCPT.Rows[CurNode.Parents.Count + i].HeaderCell.Value = grdStates[COL_State, i].Value.ToString();
                }


                //Display parent nodes states in column header.
                int colspan;
                Node curParent;
                colspan = totalcolsCPT;

                //for each parent , display states values. Incase of multiple parents, diaplay them in nested manner.
                for (i = 0; i < CurNode.Parents.Count; i++)
                {
                    curParent = (Node)CurNode.Parents[i];
                    colspan = colspan / curParent.NoOfStates;
                    grdCPT.Rows[i].HeaderCell.Value = curParent.Name + " ->";
                    grdCPT.Rows[i].ReadOnly = true;
                    grdCPT.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    grdCPT.Rows[i].DefaultCellStyle.ForeColor = Color.Black;

                    float size;
                    size = 8.25F;
                    Font newFont = new Font(FontFamily.GenericSansSerif.Name, size, System.Drawing.FontStyle.Bold);
                    grdCPT.Rows[i].HeaderCell.Style.Font = newFont;

                    //for each column, display column  heading as state/outcome of node's parents.
                    for (k = 0, j = 0; j < totalcolsCPT; j++)
                    {
                        k = j / colspan;

                        if (k >= curParent.NoOfStates)
                            k = k % curParent.NoOfStates;

                        grdCPT[j, i].Value = curParent.States[k];
                    }
                }
            }
                        
            if (bnNode.NodeType == enmNodeType.NoisyMax)
            {
                //Add columns in grid
                for (i = 1; i <= totalColsNOR; i++)
                {
                    grdNMax.Columns.Add("col" + i.ToString(), "");
                }

                //Add one row for parent name
                grdNMax.Rows.Add();
                
                //Add one row for state name
                grdNMax.Rows.Add();

                for (i = 0; i <= 1; i++)
                {
                    grdNMax.Rows[i].ReadOnly = true;
                    grdNMax.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    grdNMax.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }


                //Add one row for each state of this node
                for (i = 0; i < statesCount; i++)
                {
                    grdNMax.Rows.Add();
                    grdNMax.Rows[i + 2].HeaderCell.Value = grdStates[COL_State, i].Value.ToString();
                }

                int colsVisited = 0;
                for (i = 0; i < bnNode.Parents.Count; i++)
                {
                    for (j = 0; j < ((Node)bnNode.Parents[i]).NoOfStates; j++)
                    {
                        k = colsVisited + j;
                        grdNMax[k,0].Value = ((Node)bnNode.Parents[i]).Name ;
                        grdNMax[k,1].Value = ((Node)bnNode.Parents[i]).States[j];

                        // if this is last state of this node, make it readonly
                        if (j == ((Node)bnNode.Parents[i]).NoOfStates - 1)
                        {
                            grdNMax.Columns[k].DefaultCellStyle.BackColor = Color.Silver;
                            grdNMax.Columns[k].ReadOnly = true;
                        }
                    }
                    colsVisited += ((Node)bnNode.Parents[i]).NoOfStates;
                }
                grdNMax[totalColsNOR - 1, 0].Value = "Leak";
            }
            if (bnNode.NodeType == enmNodeType.CAST ^ bnNode.NodeType == enmNodeType.NoisyOR )
            {
                //Add columns in grid
                for (i = 1; i <= totalColsCAST; i++)
                {
                    if (bnNode.NodeType == enmNodeType.CAST)
                    {
                        grdCAST.Columns.Add("col" + i.ToString(), "");
                    }
                    else if (bnNode.NodeType == enmNodeType.NoisyOR)
                    {
                        grdCAST.Columns.Add("col" + i.ToString(), "");
                        if (i%2 == 0)
                            grdCAST.Columns[i-1].Visible = false;
                    }

                    grdCAST.Columns[i - 1].Width = 85;
                }


                //Add one row for parent name
                grdCAST.Rows.Add();

                //Add one row for h & g
                grdCAST.Rows.Add();

                if (bnNode.NodeType == enmNodeType.NoisyOR)
                    grdCAST.Rows[1].Visible=false;

                for (i = 0; i <= 1; i++)
                {
                    grdCAST.Rows[i].ReadOnly = true;
                    grdCAST.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    grdCAST.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }

                if (bnNode.NodeType == enmNodeType.NoisyOR)
                {
                    //Add one row for each state of this node
                    for (i = 0; i < statesCount; i++)
                    {
                        grdCAST.Rows.Add();
                        grdCAST.Rows[i + 2].HeaderCell.Value = grdStates[COL_State, i].Value.ToString();
                    }
                }
                else if (bnNode.NodeType == enmNodeType.CAST)
                {
                    grdCAST.Rows.Add();
                    for (i = 0; i < grdCAST.Columns.Count - 1; i++)
                    {
                        DataGridViewComboBoxCell comboCol = new DataGridViewComboBoxCell();
                        comboCol.Items.Add("High +");
                        comboCol.Items.Add("Medium +");
                        comboCol.Items.Add("Low +");
                        comboCol.Items.Add("None");
                        comboCol.Items.Add("Low -");
                        comboCol.Items.Add("Medium -");
                        comboCol.Items.Add("High -");
                        comboCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        //comboCol.Style = ComboBoxStyle.DropDownList;
                        grdCAST[i,2]=comboCol;
                    }

                }

                for (i = 0; i < bnNode.Parents.Count; i++)
                {
                    grdCAST[i*2, 0].Value = ((Node)bnNode.Parents[i]).Name;
                    grdCAST[i * 2, 1].Value = "If parent is true";
                    grdCAST[i * 2 + 1, 1].Value = "If parent is false";
                }
                grdCAST[totalColsCAST - 1, 0].Value = bnNode.NodeType == enmNodeType.NoisyOR ? "Leak" : "Baseline";
            }

        }

        private void grdCPT_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int i,startingIndex=0;
            double sum;

            try
            {
                DataGridView grd = null;

                if (bnNode.NodeType == enmNodeType.NoisyOR)
                {
                    if (grdCAST.SelectedCells.Count > 0)
                    {
                        grd = grdCAST;
                        startingIndex = 2;
                    }
                }
                else if (bnNode.NodeType == enmNodeType.NoisyMax)
                {
                    grd = grdNMax;
                    startingIndex = 2;
                }

                else
                {
                    grd = grdCPT;
                    startingIndex = bnNode.Parents.Count;
                }



                for (i = 0; i < grd.SelectedCells.Count; i++)
                {
                    sum = 0;
                    for (int j = startingIndex; j < grd.Rows.Count; j++)
                    {
                        if (j != grd.SelectedCells[i].RowIndex)
                        {
                            sum = sum + Convert.ToDouble(grd[grd.SelectedCells[i].ColumnIndex, j].Value);
                        }

                    }
                    grd.SelectedCells[i].Value = 1.0 - sum;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "IBAyes Error", MessageBoxButtons.OK);

            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddState(string state, bool hasEvidence, double probab)
        {
            int i;
            i=grdStates.Rows.Add();

            if (hasEvidence)
                grdStates[0,i].Value = true;
            else
                grdStates[0, i].Value=false;

            grdStates[1,i].Value = state;
            if ( Double.IsNaN(probab))
                   grdStates[2,i].Value = "";
            else
                grdStates[2,i].Value = probab;
        }

        private void grdStates_CellComitEdit(object sender, DataGridViewCellEventArgs e)
        {
            // if statename is changed and new name doesn't already exist in states then update it in bnNode object
           String stateNewName = grdStates[1, grdStates.CurrentRow.Index].Value.ToString();
           int i = 0;
          
            if (!stateNewName.Contains(" ") && !stateNewName.Contains("#") && !stateNewName.Contains("-") && !stateNewName.Contains("&") && !stateNewName.Contains("@") && !stateNewName.Contains("!") && !stateNewName.Contains("%") &&!stateNewName.Contains("~") && !stateNewName.Contains("$") && !stateNewName.Contains("^"))
           {
               i = bnNode.States.IndexOf(stateNewName);
               if (i < 0)
               {
                   bnNode.States[grdStates.CurrentRow.Index] = stateNewName;
               }
           }
           else {
               grdStates[COL_State, grdStates.CurrentRow.Index].Value = bnNode.States[grdStates.CurrentRow.Index];
               //grdStates[COL_State, 1].Value = bnNode.States[1];
               MessageBox.Show("State names can only contain alphanumeric and underscore.");
           }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = grdStates.CurrentRow.Index;
            if (i >= 0)
            {
                if (grdStates.Rows.Count <= 2)
                {
                    MessageBox.Show("State cannot be deleted. There must be atleast 2 states in a Node.");
                    return;
                }

                //If the node was just added and then deleted then no need to put it on deleted list
                if (bnNode.States.Contains(grdStates[COL_State,i].Value.ToString()))
                    deletedStates.Add(grdStates[COL_State,i].Value.ToString());

                grdStates.Rows.RemoveAt(i);
                grdCPT.Rows.RemoveAt(bnNode.Parents.Count + i);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkOverriden_CheckedChanged(object sender, EventArgs e)
        {
                if (chkOverriden.Checked == false)
                {
                    if (MessageBox.Show("Turning off overriden flag will discard any overriden value in CPT. Are you sure you want to continue?", "IBAyes", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        chkOverriden.Checked = true;
                        return;
                    }
                }
                grdCPT.ReadOnly = !(chkOverriden.Checked);
                grdCAST.ReadOnly = (chkOverriden.Checked);
        }

        private bool ProbabilitiesValidated()
        {
            int i,j;
            double rowSum, cellValue ;
            DataGridView grid;

            grid=grdCPT;
            if (bnNode.NodeType == enmNodeType.NoisyOR ^ bnNode.NodeType == enmNodeType.CAST)
            {
                grid = grdCAST;
            }


            //validate General CPT probabilities
            if (bnNode.NodeType == enmNodeType.General)
            {
                for (i = 0; i < grid.Columns.Count; i++)
                {
                    rowSum = 0;
                    for (j = bnNode.Parents.Count; j < grid.Rows.Count; j++)
                    {
                        bool isNum = double.TryParse(grid[i, j].Value.ToString(), out cellValue);
                        if (!isNum)
                        {
                            MessageBox.Show("Probabilities must contain numeric values.", "Error", MessageBoxButtons.OK);
                            return false;
                        }

                        rowSum += Convert.ToDouble(grid[i, j].Value);
                    }

                    if (rowSum != 1)
                    {
                        MessageBox.Show("Probabilities in each column must be summed up to 1.", "Error", MessageBoxButtons.OK);
                        return false;
                    }
                }
            }

            //Validate NoisyOR parameters
            if (bnNode.NodeType == enmNodeType.NoisyOR)
            {
                for (i = 0; i < grid.Columns.Count; i=i+2)
                {
                    rowSum = 0;
                    for (j = 2; j < grid.Rows.Count; j++)
                    {
                        bool isNum = double.TryParse(grid[i, j].Value.ToString(), out cellValue);
                        if (!isNum)
                        {
                            MessageBox.Show("Probabilities must contain numeric values.", "Error", MessageBoxButtons.OK);
                            return false;
                        }

                        rowSum += Convert.ToDouble(grid[i, j].Value);
                    }
                    if (rowSum != 1)
                    {
                        MessageBox.Show("Probabilities in each column must be summed up to 1.", "Error", MessageBoxButtons.OK);
                        return false;
                    }
                }
            }

            //Validate CAST logic parameters
            if (bnNode.NodeType == enmNodeType.CAST)
            {
                bool isNum = double.TryParse(grid[grid.Columns.Count-1,2].Value.ToString(), out cellValue);
                if (!isNum)
                {
                    MessageBox.Show("Probabilities must contain numeric values.", "Error", MessageBoxButtons.OK);
                    return false;
                }

                if (cellValue < 0 || cellValue > 1)
                {
                    MessageBox.Show("Baseline probability must lie between 0 and 1.", "Error", MessageBoxButtons.OK);
                    return false;
                }

                
                //for (i = 0; i < grid.Columns.Count; i = i + 2)
                //{
                //    bool isNum = double.TryParse(grid[i, 2].Value.ToString(), out cellValue);
                //    if (!isNum)
                //    {
                //        MessageBox.Show("CAST logic parameters must be numeric.", "Error", MessageBoxButtons.OK);
                //        return false;
                //    }
                //    if (cellValue < -1 ^ cellValue > 1)
                //    {
                //        MessageBox.Show("CAST logic parameters must lie within -1 & +1.", "Error", MessageBoxButtons.OK);
                //        return false;
                //    }
                //}
            }
            return true;
        }

    }
}
