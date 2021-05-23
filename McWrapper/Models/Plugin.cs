using System;
using System.ComponentModel.DataAnnotations;

namespace McWrapper.Models
{
    public class Plugin
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Version { get; set; }
        public string FileName { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
    }

    public class PluginDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string FileName { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
    }
}