using System;
using System.ComponentModel.DataAnnotations;

namespace McWrapper.Models
{
    public class Server
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid JarFile { get; set; }
        public string JavaArguments { get; set; }
        [Required]
        public bool EnablePlugins { get; set; }
        public Plugin[] EnabledPlugins { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
    }

    public class ServerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid JarFile { get; set; }
        public string JavaArguments { get; set; }
        public bool EnablePlugins { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
    }
}