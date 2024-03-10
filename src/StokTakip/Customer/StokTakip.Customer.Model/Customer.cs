using NetCore.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace StokTakip.Customer.Model
{
    public class Customer : TracingEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public required string No { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Code { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
