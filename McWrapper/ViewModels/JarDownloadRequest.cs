namespace McWrapper.ViewModels
{
    public class JarDownloadRequest
    {
        public string DownloadUrl { get; set; }
        public string FileName { get; set; }
        public JarKind JarKind { get; set; }
        public string MinecraftVersion { get; set; }
    }
}