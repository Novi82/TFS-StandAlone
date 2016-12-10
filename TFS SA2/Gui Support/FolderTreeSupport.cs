using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TFS_SA.Gui_Support
{ 
    public enum BuildMode
        {
            AddNew,
            Update
        }
    public class FolderTreeSupport 
    {
        public TreeView treeView { get; set; }
         public FolderTreeSupport(TreeView _treeView)
        {
            this.treeView = _treeView;

        }
        public void Clear()
        {
            treeView.Nodes.Clear();
        }
        public void AddRootNode(Item _root)
        {
            this.Clear();
            var RootNode = new FolderTreeNode(_root);
            //RootNode.Item;
            treeView.Nodes.Add(RootNode);
            treeView.SelectedNode = RootNode;
        }
       
        public void addNode(ref FolderTreeNode _Parent, List<FolderTreeNode> _Items, BuildMode _Mode)
        {
             if(_Mode == BuildMode.AddNew)
                {
                    _Parent.Nodes.Clear();
                }
            foreach (var item in _Items)
            {
                _Parent.Nodes.Add(item);
            }
        }

        internal void initTree(ItemSet folder)
        {
            throw new NotImplementedException();
        }

        public void SelectNode(Item _Item)
        {
            var currentNode = treeView.SelectedNode;
            foreach(TreeNode node in currentNode.Nodes)
            {
                var noteItem = (Item)node.Tag;
                if( noteItem.ServerItem.Equals( _Item.ServerItem))
                {
                    treeView.SelectedNode = node;
                    return;
                }
            }
        }
        public String getSelectedPath()
        {
            if(treeView.SelectedNode != null)
            {
                var item = (Item) (treeView.SelectedNode.Tag);
                return item.ServerItem;
            }
            else
            {
                return String.Empty;
            }
        }
      
    }
}
