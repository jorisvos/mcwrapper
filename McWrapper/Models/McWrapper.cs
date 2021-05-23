using System;
using System.ComponentModel.DataAnnotations;

namespace McWrapper.Models
{
    public class McWrapper
    {
        [Required]
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
    }

    public class McWrapperDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
    }
}