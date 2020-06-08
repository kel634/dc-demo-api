using System;
using System.Collections.Generic;

namespace dc_demo_api.Models
{
    public partial class AssetVariant
    {
        public int AssetVariantId { get; set; }
        public int AssetId { get; set; }
        public int VariantTypeId { get; set; }
        public string Url { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual VariantType VariantType { get; set; }
    }
}
