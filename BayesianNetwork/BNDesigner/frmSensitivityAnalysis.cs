using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using IBAyes.Bayesian;

namespace DiagramDesigner
{
    public partial class frmSensitivityAnalysis : Form
    {
        private Network _network;
        public frmSensitivityAnalysis(Network network)
        {
            InitializeComponent();
            _network = network;
        }

        private void frmSensitivityAnalysis_Load(object sender, EventArgs e)
        {
            foreach (Node node in _network.Nodes)
            {
                cboNodes.Items.Add(node);
                cboNodes.DisplayMember = "Name";
                cboNodes.ValueMember = "NodeID";
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            int i=0;
            SAResult result;

            if (cboNodes.SelectedIndex < 0)
            {
                return; 
            }
            Hashtable results = _network.PerformSensitivityAnalysis((Node)cboNodes.SelectedItem);

            grdSA.Rows.Clear();
            foreach (string key in results.Keys)
            {
                result = (SAResult)results[key];
                grdSA.Rows.Add();
                grdSA[0, i].Value = result.NodeName;
                grdSA[1, i].Value = result.ProbT;
                grdSA[2, i].Value = result.ProbF;
                grdSA[3, i].Value = Math.Round(result.ProbT - result.ProbF,4);
                grdSA[4, i].Value = result.EntT;
                grdSA[5, i].Value = result.EntF;
                grdSA[6, i].Value = Math.Round(result.EntT - result.EntF,4);
                i++;
            }
            grdSA.Sort(grdSA.Columns[3], ListSortDirection.Descending);

            i = 0;
            Hashtable resultsSI = _network.PerformSensitivityToInfluence((Node)cboNodes.SelectedItem);
            grdSAInfluence.Rows.Clear();
            foreach (string key in resultsSI.Keys)
            {
                result = (SAResult)resultsSI[key];
                grdSAInfluence.Rows.Add();
                grdSAInfluence[0, i].Value = result.NodeName;
                grdSAInfluence[1, i].Value = result.ToNodeName;
                grdSAInfluence[2, i].Value = result.ProbF;
                grdSAInfluence[3, i].Value = result.ProbT;
                grdSAInfluence[4, i].Value = Math.Round(result.ProbT - result.ProbF, 4);
                grdSAInfluence[5, i].Value = result.ProbH_F;
                grdSAInfluence[6, i].Value = result.ProbH_T;
                grdSAInfluence[7, i].Value = Math.Round(result.ProbH_T - result.ProbH_F, 4);
                i++;
            }
            grdSAInfluence.Sort(grdSAInfluence.Columns[4], ListSortDirection.Descending);
        }
    }
}
