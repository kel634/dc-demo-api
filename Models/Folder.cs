using System;
using System.Collections.Generic;

namespace dc_demo_api.Models
{
    public partial class Folder
    {
        public Folder()
        {
            Asset = new HashSet<Asset>();
            InverseParent = new HashSet<Folder>();
        }

        public int FolderId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }

        public virtual Folder Parent { get; set; }
        public virtual ICollection<Asset> Asset { get; set; }
        public virtual ICollection<Folder> InverseParent { get; set; }
    }
}
