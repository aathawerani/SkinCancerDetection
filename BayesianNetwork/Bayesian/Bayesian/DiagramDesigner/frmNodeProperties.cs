using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DiagramDesigner.Bayesian;


namespace DiagramDesigner
{
    public partial class frmNodeProperties : Form
    {
        Bayesian.Node bnNode;
        StringCollection deletedStates;

        public frmNodeProperties()
        {
            InitializeComponent();
        }

        private void frmNodeProperties_Load(object sender, EventArgs e)
        {
            txtNodeID.Text = bnNode.NodeID.ToString();
            txtNodeName.Text = bnNode.Name.ToString();
            deletedStates = new StringCollection();

            lstStates.Items.Clear();

            //foreach (String str in bnNode.States)
            String str;
            for (int i = 0; i<=bnNode.States.Count-1;i++)
            {
                str = bnNode.States[i];
                lstStates.Items.Add(str);
            }

            //Generate CPT grid structure
            GenerateCPTStructure(bnNode);

            //Display CPT probabilities in grid
            for (int i = 0; i < bnNode.CPT.Rows ; i++)
            {
                for (int j = 0; j < bnNode.CPT.Columns; j++)
                {
                    grdCPT[j, i + bnNode.Parents.Count].Value = bnNode.CPT.GetValue(i, j);
                }
            }
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
            if (txtNewState.Text != "" )
            {
                lstStates.Items.Add(txtNewState.Text.Trim());
                int rowIndex = grdCPT.Rows.Add();
                grdCPT.Rows[rowIndex].HeaderCell.Value=txtNewState.Text.Trim();
                txtNewState.Text = "";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bnNode.Name = txtNodeName.Text;

            List<int> lst;
            //lst = bnNode.GetColumnIndex(0, 1);
            //bnNode.CPT.initialize(bnNode.NoOfStates, grdCPT.Columns.Count);

            //Save Node states

            //Remove deleted states
            foreach (String str in deletedStates)
            {
                bnNode.RemoveState(str);
            }

            //Add newly added states
            foreach (String str in lstStates.Items)
            {
                if (!bnNode.States.Contains(str))
                {
                    bnNode.AddState(str);
                }
            }

            //Save CPT values in node

            for (int i = bnNode.Parents.Count; i < grdCPT.Rows.Count; i++)
            {
                for (int j = 0; j < grdCPT.Columns.Count; j++)
                { 
                    double value = Convert.ToDouble(grdCPT[j, i].Value);
                    bnNode.CPT.SetValue(i - bnNode.Parents.Count, j, value);
                }
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
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

        private void lstStates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
            {
                int i = lstStates.SelectedIndex;
                if (i >= 0)
                {
                    if (lstStates.Items.Count <= 2)
                    {
                        MessageBox.Show("State cannot be deleted. There must be atleast 2 states in a Node.");
                        return;
                    }
                    //If the node was just added and then deleted then no need to put it on deleted list
                    if (bnNode.States.Contains(lstStates.SelectedItem.ToString()))
                        deletedStates.Add(lstStates.SelectedItem.ToString());

                    lstStates.Items.RemoveAt(i);
                    grdCPT.Rows.RemoveAt(bnNode.Parents.Count + i);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lstStates.Items.Count < 2)
            {
                MessageBox.Show("Please define states of this node.");// ,,, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GenerateCPTStructure(bnNode);
        }

        private void GenerateCPTStructure(Node CurNode)
        {
            int i = 1;
            int statesCount;
            grdCPT.Rows.Clear();
            grdCPT.Columns.Clear();

            int totalcols;
            totalcols =1;

            statesCount = lstStates.Items.Count;

            //CurNode.GenerateCPTStructure(CurNode);
            //if (statesCount == 0)
            //    return;

            //Total columns in grid = product of NoofStates of all parents.
            for (i = 0; i < CurNode.Parents.Count; i++)
            {
                totalcols = totalcols * ((Node)CurNode.Parents[i]).NoOfStates;
            }

            //Add columns in grid
            for (i = 1; i <= totalcols; i++)
            {
                grdCPT.Columns.Add("col" + i.ToString(),"");
            }

            //Add one row for each parent of this node
            if (CurNode.Parents.Count > 0)
                grdCPT.Rows.Add(CurNode.Parents.Count);

            //Add one row for each state of this node
            for (i = 0; i < statesCount; i++)
            {
                grdCPT.Rows.Add();
                grdCPT.Rows[CurNode.Parents.Count + i].HeaderCell.Value = lstStates.Items[i].ToString();
            }
                        

            //Display parent nodes states in column header.
            int colspan,j,k;
            Node curParent;
            colspan = totalcols;

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
                Font newFont = new Font(FontFamily.GenericSansSerif.Name, size, FontStyle.Bold);
                grdCPT.Rows[i].HeaderCell.Style.Font = newFont; 

                //for each column, display column  heading as state/outcome of node's parents.
                for( k = 0,j = 0; j < totalcols;j++)
                {
                    k = j / colspan;

                    if (k >= curParent.NoOfStates)
                        k = k % curParent.NoOfStates;

                    grdCPT[j, i].Value = curParent.States[k];
                }
            }
        }

        private void grdCPT_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //bnNetwork.
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int i;
            double sum;

            for (i = 0; i < grdCPT.SelectedCells.Count; i++)
            {
                sum = 0;
                for (int j = bnNode.Parents.Count; j< grdCPT.Rows.Count; j++)
                {
                    if (j != grdCPT.SelectedCells[i].RowIndex)
                    {
                        sum = sum + Convert.ToDouble(grdCPT[grdCPT.SelectedCells[i].ColumnIndex, j].Value);
                    }

                }
                grdCPT.SelectedCells[i].Value = 1.0 - sum;
            }

        }
    }
}
