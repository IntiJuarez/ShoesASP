using System;
using System.Collections.Generic;

namespace ShoesASP.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Shoes = new HashSet<Shoe>();
        }

        public int BrandId { get; set; }
        public string Name { get; set; } = null!;   

        public virtual ICollection<Shoe> Shoes { get; set; }
    }
}
