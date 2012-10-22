using System.Collections.Generic;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{
    public interface IListVisualStudioProjects
    {
        IEnumerable<Project> GetAllProjects();
    }
}