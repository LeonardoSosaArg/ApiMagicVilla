using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto
{
    public class VillaUpdateDto
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        public string Detail { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Capacity { get; set; }
        public string Province { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public DateTime DateUpdated { get; set; } = DateTime.Now;
    }
}
