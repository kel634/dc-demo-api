using System;
using System.Collections.Generic;

namespace dc_demo_api.Models
{
    public partial class VariantType
    {
        public VariantType()
        {
            AssetVariants = new HashSet<AssetVariant>();
        }

        public int VariantTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AssetVariant> AssetVariants { get; set; }
    }
}
