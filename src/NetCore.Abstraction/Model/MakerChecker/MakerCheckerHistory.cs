using System.ComponentModel.DataAnnotations;

namespace NetCore.Abstraction.Model
{
    public class MakerCheckerHistory
    {
        public int MakerCheckerHistoryId { get; set; }
        [Required]
        [StringLength(32)]
        public string Reference { get; set; }
        public int MakerCheckerId { get; set; }
        public MakerChecker MakerChecker { get; set; }
        [Required]
        [StringLength(150)]
        public string MakerCheckerName { get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
    }
}
