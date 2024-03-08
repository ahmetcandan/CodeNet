using NetCore.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace StokTakip.Customer.Model
{
    public class Customer : TracingEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name
        {
            get { return GetValue(_name); }
            set { _name = SetValue(value); }
        }
        private string _name;

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
