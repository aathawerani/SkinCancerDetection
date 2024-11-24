using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IBAyes.Bayesian;
using System.Windows.Navigation;

namespace DiagramDesigner
{
    public partial class frmConnectionProperties : Form
    {   
       
        IBAyes.Bayesian.Connection bnConnection;
        public frmConnectionProperties()
        {
            InitializeComponent();
        }

        private void frmConnectionProperties_Load(object sender, EventArgs e)
        {
           
        }

        public void ShowConnectionPropertiesDialog(IBAyes.Bayesian.Connection curConn)
        {
            bnConnection = curConn;
            txtID.Text = curConn.ID.ToString();
            txtSourceAndSink.Text = "\"" + curConn.SourceNode.Name + "\" influences \"" + curConn.SinkNode.Name + "\"";
            txtg.Visible = false;
            txth.Visible = false;
            txtProb.Enabled= false;
            lblProb.Enabled = false;

            if (curConn.SinkNode.NodeType == enmNodeType.CAST)
            {
                int parentIndex = curConn.SinkNode.Parents.IndexOf(curConn.SourceNode);
                txtg.Text = curConn.SinkNode.CASTPT.GetValue(0,parentIndex*2).ToString();
                txth.Text = curConn.SinkNode.CASTPT.GetValue(0, parentIndex * 2+1).ToString();
                grpCausalStrength.Enabled = true;
                //InitializeTrackBar(true);
                DisplayTrackBar(txtg, trackBar1);
                DisplayTrackBar(txth, trackBar2);
            }

            else if (curConn.SinkNode.NodeType == enmNodeType.NoisyOR)
            {
                int parentIndex = curConn.SinkNode.Parents.IndexOf(curConn.SourceNode);
                txtProb.Enabled = true;
                lblProb.Enabled = true;
                txtProb.Text = curConn.SinkNode.CASTPT.GetValue(0, parentIndex * 2).ToString();
                //txth.Text = curConn.SinkNode.CASTPT.GetValue(0, parentIndex * 2 + 1).ToString();
                grpCausalStrength.Enabled = false;
                //InitializeTrackBar(false);
                
            }


            if (bnConnection.SinkNode.NodeType == enmNodeType.General)
            {
                grpCausalStrength.Enabled = false;
                //InitializeTrackBar(false); 
            }

            ShowDialog();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            double gValue, hValue;
            bool isNum = double.TryParse(txtg.Text, out gValue);
            bool isNum2 = double.TryParse(txth.Text, out hValue);

            if (!isNum ^ !isNum2)
            {
                MessageBox.Show("Probabilities must contain numeric values.", "Error", MessageBoxButtons.OK);
                return;
            }

            if (bnConnection.SinkNode.NodeType== enmNodeType.NoisyOR) 
            {
                bool isValid = double.TryParse(txtProb.Text, out gValue);
                if (!isValid)
                {
                    MessageBox.Show("Probabilities must contain numeric values.", "Error", MessageBoxButtons.OK);
                    return;
                }
            }


            if (bnConnection.SinkNode.NodeType == enmNodeType.CAST ^ bnConnection.SinkNode.NodeType == enmNodeType.NoisyOR)
            {
                if (bnConnection.SinkNode.NodeType == enmNodeType.CAST)
                {
                    if ((gValue < -1 ^ gValue > 1) ^ (hValue < -1 & hValue > 1))
                    {
                        MessageBox.Show("CAST parameters must lie between -1 and +1.", "Error", MessageBoxButtons.OK);
                        return;
                    }
                }
                else
                {
                    if ((gValue < 0 ^ gValue > 1))
                    {
                        MessageBox.Show("Probability must lie between 0 and 1.", "Error", MessageBoxButtons.OK);
                        return;
                    }
                }
                
                int parentIndex = bnConnection.SinkNode.Parents.IndexOf(bnConnection.SourceNode);
                bnConnection.SinkNode.CASTPT.SetValue(0, parentIndex * 2, gValue);
                bnConnection.SinkNode.CASTPT.SetValue(0, parentIndex * 2+1, hValue);
                if (bnConnection.SinkNode.NodeType == enmNodeType.NoisyOR)
                {
                    bnConnection.SinkNode.CASTPT.SetValue(1, parentIndex * 2, 1-gValue);
                    bnConnection.SinkNode.CASTPT.SetValue(1, parentIndex * 2 + 1, 1);
                }
                bnConnection.SinkNode.GenerateCPTforCASTNode();
            }

            this.Close();
        }

        public void InitializeTrackBar(bool value)
        {
            trackBar1.Visible = value;
            trackBar2.Visible = value;
            label2.Visible = value;
            label3.Visible = value;
            label4.Visible = value;
            label7.Visible = value;
            label8.Visible = value;
            label9.Visible = value;
            label10.Visible = value;
            label11.Visible = value;
            label12.Visible = value;
            label13.Visible = value;
            label16.Visible = value;
            label17.Visible = value;
            label18.Visible = value;
            label19.Visible = value;
        
        }

        public void DisplayTrackBar(System.Windows.Forms.TextBox tx , TrackBar tBar)
        {
            if (double.Parse(tx.Text.ToString()) == 0)
            {
                tBar.Value = 3;
            }
            if ((double.Parse(tx.Text.ToString())> 0) &&(double.Parse(tx.Text.ToString())<=0.3))
                {
                    tBar.Value = 4;
                }

            if ((double.Parse(tx.Text.ToString()) > 0.3) && (double.Parse(tx.Text.ToString()) <= 0.6))
            {
                tBar.Value = 5;
            }

            if ((double.Parse(tx.Text.ToString()) > 0.6))
            {
                tBar.Value = 6;
            }

            if ((double.Parse(tx.Text.ToString()) < 0) && (double.Parse(tx.Text.ToString()) >= -0.3))
            {
                tBar.Value = 2;
            }
            if ((double.Parse(tx.Text.ToString()) < -0.3) && (double.Parse(tx.Text.ToString()) >= -0.6))
            {
                tBar.Value = 1;
            }
            
            if ((double.Parse(tx.Text.ToString()) < -0.6))
            {
                tBar.Value = 0;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            TrackBar t= (TrackBar)sender;

            switch (t.Value)
            {
                case 0:
                    {
                        txtg.Text = "-0.9";
                        break;
                    }

                case 1:
                    {
                        txtg.Text = "-0.6";
                        break;
                    }
                case 2:
                    {
                        txtg.Text = "-0.3";
                        break;
                    }
                case 3:
                    {
                        txtg.Text = "0";
                        break;
                    }
                case 4:
                    {
                        txtg.Text = "0.3";
                        break;
                    }
                case 5:
                    {
                        txtg.Text = "0.6";
                        break;
                    }
                case 6:
                    {
                        txtg.Text = "0.9";
                        break;
                    }
            }
            //txtg.Text=
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            TrackBar t = (TrackBar)sender;

            switch (t.Value)
            {
                case 0:
                    {
                        txth.Text = "-0.9";
                        break;
                    }

                case 1:
                    {
                        txth.Text = "-0.6";
                        break;
                    }
                case 2:
                    {
                        txth.Text = "-0.3";
                        break;
                    }
                case 3:
                    {
                        txth.Text = "0";
                        break;
                    }
                case 4:
                    {
                        txth.Text = "0.3";
                        break;
                    }
                case 5:
                    {
                        txth.Text = "0.6";
                        break;
                    }
                case 6:
                    {
                        txth.Text = "0.9";
                        break;
                    }
           
            }
            
        }

        
    }
}
