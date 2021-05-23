using System;

namespace McWrapper.Models
{
    public class PluginServer
    {
        public Guid Id { get; set; }
        public Guid PluginId { get; set; }
        public Guid ServerId { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
    }
    
    public class PluginServerDto
    {
        public Guid Id { get; set; }
        public Guid PluginId { get; set; }
        public Guid ServerId { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
    }
}