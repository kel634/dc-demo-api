using System;
using System.Collections.Generic;

namespace dc_demo_api.Models
{
    public partial class Asset
    {
        public Asset()
        {
            AssetVariants = new HashSet<AssetVariant>();
        }

        public int AssetId { get; set; }
        public string FileName { get; set; }
        public string DisplayName { get; set; }
        public string PreviewUrl { get; set; }
        public int FolderId { get; set; }

        public virtual Folder Folder { get; set; }
        public virtual AssetMetadata AssetMetadata { get; set; }
        public virtual ICollection<AssetVariant> AssetVariants { get; set; }
    }
}
