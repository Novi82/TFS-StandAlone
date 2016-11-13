
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TFS_SA.Gui_Support
{
    public class FolderTreeNode:TreeNode
    {
        public Item Item
        {
            get;
            private set;
        }
        public FolderTreeNode( )
        {

        }
        public FolderTreeNode(Item _Item)
        {
            //todo get name ntn?
            this.Text = _Item.ServerItem;
            
            this.Item = _Item;
            this.Tag = _Item;
        }
        public FolderTreeNode(String Value)
        {
            this.Text = Value;
           //todo
            //this.Item = new Item();
        }

    }
}
