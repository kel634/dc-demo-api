using System;
using System.Collections.Generic;

namespace dc_demo_api.Models
{
    public partial class AssetMetadata
    {
        public int AssetMetadataId { get; set; }
        public int AssetId { get; set; }

        public virtual Asset Asset { get; set; }
    }
}
