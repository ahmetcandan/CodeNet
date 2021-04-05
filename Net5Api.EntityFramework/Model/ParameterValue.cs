using System;
using System.ComponentModel.DataAnnotations;

namespace Net5Api.EntityFramework.Model
{
    public class ParameterValue
    {
        public int ParameterValueId { get; set; }
        [Required]
        public string Value { get; set; }
        public DateTime? ExpDate { get; set; }
        public int ParameterKeyId { get; set; }
        public ParameterKey ParameterKey { get; set; }
    }
}
