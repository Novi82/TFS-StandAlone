using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFS_SA.TfsWapper;
using Microsoft.TeamFoundation.VersionControl.Client;
namespace TFS_SA
{
    public partial class frmWorkspace : Form
    {

        protected Boolean isConnected;
        protected TfsWrapper tfs;
        public frmWorkspace()
        {
            InitializeComponent();
        }
        public frmWorkspace(TfsWrapper _tfs)
        {
            InitializeComponent();
            this.tfs = _tfs;
        }

        private void frmWorkspace_Load(object sender, EventArgs e)
        {
            try
            {
                this.AddWorkSapces();
                if (cbxWorkspace.Items.Count > 0)
                {
                    cbxWorkspace.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected void AddWorkSapces()
        {
            Workspace[] wss = tfs.GetWorkspaces();
            cbxWorkspace.Items.Clear();
            var sorted = wss.OrderBy(x => x.Name);
            foreach (Workspace ws in sorted)
            {
                tfsWorkspaceItem item = new tfsWorkspaceItem(ws.Name, ws);
                cbxWorkspace.Items.Add(item);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbxWorkspace.SelectedItem != null)
                {
                    var item = (tfsWorkspaceItem)cbxWorkspace.SelectedItem;
                    var name  = item.Name;
                    if (item.WorkSpace.Delete())
                    {
                        MessageBox.Show(name + @" was deteted!");
                    }
                    else
                    {
                        MessageBox.Show( @"Can't delete " + name);
                    }
                    AddWorkSapces();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var ws = tfs.VCS.CreateWorkspace(txtNewWS.Text);
                MessageBox.Show(ws.Name + @"was create success!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                AddWorkSapces();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
