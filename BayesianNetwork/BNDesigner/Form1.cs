using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IBAyes.Bayesian;

namespace DiagramDesigner
{
    public partial class frmResult : Form
    {
        public frmResult()
        {
            InitializeComponent();
        }

        public frmResult(Network bnNetwork)
        {
            string result;
            int i;
            result = "";
            InitializeComponent();
            //Print updated belifes on console
            foreach (Node node in bnNetwork.Nodes)
            {
                result = result + "\r\n" + node.Name + " : \r\n";
                if (node.EvidenceOn >= 0)
                {
                    //TODO: temporary fix for a smile problem     
                    for (i = 0; i < node.NoOfStates; i++)
                    {
                        if (i == node.EvidenceOn)
                            result = result + "\t   " + node.States[i] + " : 1\r\n";
                        else
                            result = result + "\t   " + node.States[i] + " : 0\r\n";
                    }
                }
                else
                {
                    double[] arr = new double[node.NoOfStates];
                    //arr = bnNetwork.SmileNetwork.GetNodeValue(node.NodeHandle);
                    for (i = 0; i < node.NoOfStates; i++)
                    {
                        result = result  +"\t" + node.States[i] + " : " + node.GetPosteriorProbab(i) + "\r\n";
                    }
                }
            }
            textBox1.Text = result;
 
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
