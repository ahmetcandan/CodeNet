using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Net5Api.EntityFramework.Model
{
    public class ParameterKey
    {
        public ParameterKey()
        {
            ParameterValues = new HashSet<ParameterValue>();
        }

        public int ParameterKeyId { get; set; }
        [Required]
        [StringLength(255)]
        public string KeyName { get; set; }
        [Required]
        [StringLength(100)]
        public string KeyType { get; set; }
        public virtual ICollection<ParameterValue> ParameterValues { get; set; }
    }
}
