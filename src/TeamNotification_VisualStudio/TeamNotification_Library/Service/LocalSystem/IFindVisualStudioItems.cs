using EnvDTE;
using TeamNotification_Library.Functional;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.LocalSystem
{
    public interface IFindVisualStudioItems
    {
        Maybe<ProjectItem> FindDocument(string projectName, string fileName);
    }
}