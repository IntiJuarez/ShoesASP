using System.ComponentModel.DataAnnotations;

namespace ShoesASP.Models.ViewModels
{
    public class ShoesViewModel
    {
        [Required]
        [Display(Name = "Nombre")]
        [StringLength(15, ErrorMessage = "Máximo de caracteres superados")]
        public string Name { get; set; }

        [Required]
        [Display(Name= "Marca")]
        public int BrandId { get; set; }
    }
}
