using System;
using System.Collections.Generic;

namespace dc_demo_api.Models
{
    public partial class Folder
    {
        public Folder()
        {
            Assets = new HashSet<Asset>();
            SubFolders = new HashSet<Folder>();
        }

        public int FolderId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }

        public virtual Folder ParentFolder { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Folder> SubFolders { get; set; }
    }
}
