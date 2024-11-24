using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DiagramDesigner.Bayesian;

namespace DiagramDesigner
{
    public partial class frmConnectionProperties : Form
    {
        public frmConnectionProperties()
        {
            InitializeComponent();
        }

        private void frmConnectionProperties_Load(object sender, EventArgs e)
        {
 
        }

        public void ShowConnectionPropertiesDialog(Bayesian.Connection curConn)
        {
            txtID.Text = curConn.ID.ToString();
            txtSourceAndSink.Text = "From " + curConn.SourceNode.Name + " to " + curConn.SinkNode.Name;

            ShowDialog();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
