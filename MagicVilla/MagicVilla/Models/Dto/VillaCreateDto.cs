using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto
{
    public class VillaCreateDto
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        public string Detail { get; set; }
        [Required]
        public double Price { get; set; }
        public int Capacity { get; set; }
        public string Province { get; set; }
        public string ImageUrl { get; set; }
    }
}
