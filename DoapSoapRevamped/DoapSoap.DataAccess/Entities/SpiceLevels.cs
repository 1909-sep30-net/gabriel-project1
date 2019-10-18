using System;
using System.Collections.Generic;

namespace DoapSoap.DataAccess.Entities
{
    public partial class SpiceLevels
    {
        public SpiceLevels()
        {
            Products = new HashSet<Products>();
        }

        public int SpiceId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
