using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFS_SA.TfsWapper
{
    class tfsWorkspaceItem
    {
        public string Name { get; set; }
    public Workspace WorkSpace { get; set; }

    public tfsWorkspaceItem(string name, Workspace workspace)
    {
        this.Name = name;
        this.WorkSpace = workspace;
    }
    public override string ToString()
    {
        return this.Name;
    }
    }
}
