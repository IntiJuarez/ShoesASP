using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoesASP.Models
{
    public partial class Shoe
    {
        public int ShoesId { get; set; }
        public string Name { get; set; } = null!;
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; } = null!;
    }
}
