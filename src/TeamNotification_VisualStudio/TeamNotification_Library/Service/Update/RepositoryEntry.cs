using Microsoft.VisualStudio.ExtensionManager;

namespace TeamNotification_Library.Service.Update
{
    public class RepositoryEntry : IRepositoryEntry
    {
        public string Name { get; set; }
        public string DownloadUrl { get; set; }
        public string VsixReferences { get; set; }
    }
}