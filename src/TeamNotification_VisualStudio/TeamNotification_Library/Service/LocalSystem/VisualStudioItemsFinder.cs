using EnvDTE;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class VisualStudioItemsFinder : IFindVisualStudioItems
    {
        private readonly IStoreDTE dteStore;

        public VisualStudioItemsFinder(IStoreDTE dteStore)
        {
            this.dteStore = dteStore;
        }

        public Maybe<ProjectItem> FindDocument(string projectName, string fileName)
        {
            return dteStore.
        }
    }
}