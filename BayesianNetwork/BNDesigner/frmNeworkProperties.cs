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
    public partial class frmNeworkProperties : Form
    {
        Network bnNetwork;
        public frmNeworkProperties(Network network)
        {
            InitializeComponent();
            bnNetwork = network;
        }

        private void NeworkProperties_Load(object sender, EventArgs e)
        {
            cboAlgorithm.Items.Add("Lauritzean");
            cboAlgorithm.Items.Add("Likelihood Sampling");
            cboAlgorithm.Items.Add("Backward Sampling");
            cboAlgorithm.Items.Add("EPIS Sampling");
            cboAlgorithm.Items.Add("Henrion");
            cboAlgorithm.Items.Add("Self Importance");
            cboAlgorithm.SelectedIndex = Convert.ToInt32(bnNetwork.Algorithm);
            txtSampleSize.Text = bnNetwork.SampleSize.ToString();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bnNetwork.Algorithm = ((enmBayesianAlgorithm)cboAlgorithm.SelectedIndex);
            bnNetwork.SampleSize = Convert.ToInt32(txtSampleSize.Text);
            this.Close();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
