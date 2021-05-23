using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace McWrapper.Models
{
    public class Jar
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        [Required]
        public JarKind JarKind { get; set; }
        [Required]
        public string MinecraftVersion { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
        
        [Required]
        public IFormFile File { get; set; }
    }

    public class JarDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public JarKind JarKind { get; set; }
        public string MinecraftVersion { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
    }
}