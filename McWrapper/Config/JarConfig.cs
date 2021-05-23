using System.Collections.Generic;
using McWrapper.Models;

namespace McWrapper.Config
{
    public class JarConfig : IConfig
    {
        public List<Jar> Jars { get; set; }
    }
}