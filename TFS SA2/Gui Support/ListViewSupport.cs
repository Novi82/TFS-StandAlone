using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.IO;

namespace TFS_SA.Gui_Support
{
    public class ListViewSupport
    {
        protected ListView listView;
        public  ListViewSupport(ListView _listView)
        {
            this.listView = _listView;
        }
        public void Load(ItemSet _Items,Func<string,String> getLastCommiter)
        {
            // clear listview
            this.Clear();
            var DisplayItems = new ListViewItem();
            // inogre fist element , it i folder name.
            foreach(var item in _Items.Items.Skip(1))
            {
                var array = new String[4];
                switch (item.ItemType)
                {
                    case ItemType.Any:
                        break;
                    case ItemType.File:
                        array[0] = Path.GetFileName(item.ServerItem);
                        array[1] = getLastCommiter(item.ServerItem);
                        // todo mapping roi moi lam dc
                        array[2] = item.ServerItem;
                        array[3] = item.CheckinDate.ToString();
                        DisplayItems = new ListViewItem(array,"File");
                        DisplayItems.Tag = item;
                        break;
                    case ItemType.Folder:
                        array[0] = Path.GetFileName(item.ServerItem);
                        array[1] = getLastCommiter(item.ServerItem);
                        // todo mapping roi moi lam dc
                        array[2] = item.ServerItem;
                        array[3] = item.CheckinDate.ToString();
                        DisplayItems = new ListViewItem(array, "Folder");
                        DisplayItems.Tag = item;
                        break;
                    default:
                        break;
                }
                // xu ly mau hien thi
                listView.Items.Add(DisplayItems);
            }
        }

        public void Clear()
        {
            this.listView.Items.Clear();
        }

        public Item getSelectedItem(ref String _fileName)
        {
            if( listView.SelectedItems.Count >0)
            {
                var selectedItem = listView.SelectedItems[0];
                _fileName = selectedItem.Text;
                return (Item)selectedItem.Tag;
            }
            else
            {
                String msg = @"No item have been selected";
                throw new Exception(msg);
            }
        }
    }
}
