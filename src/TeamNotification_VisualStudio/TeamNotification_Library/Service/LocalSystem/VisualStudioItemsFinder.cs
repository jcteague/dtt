using System.Linq;
using EnvDTE;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Functional;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class VisualStudioItemsFinder : IFindVisualStudioItems
    {
        private readonly IListVisualStudioProjects visualStudioProjectsList;

        public VisualStudioItemsFinder(IListVisualStudioProjects visualStudioProjectsList)
        {
            this.visualStudioProjectsList = visualStudioProjectsList;
        }

        public Maybe<ProjectItem> FindDocument(string projectName, string fileName)
        {
            var project = FindProject(projectName);
            var document = project.Select(x => DocumentFilter(x.ProjectItems, fileName));
            return document;
        }

        public Maybe<Project> FindProject(string projectName)
        {
            var projects = visualStudioProjectsList.GetAllProjects();
            return projects.First(x => RemoveUnnecessaryPath(x) == projectName).ToMaybe();
        }

        private string RemoveUnnecessaryPath(Project project)
        {
            return project.UniqueName.Remove(0, project.UniqueName.LastIndexOf('\\') + 1);
        }

        private ProjectItem DocumentFilter(ProjectItems projectItems, string fileName)
        {
            foreach (ProjectItem item in projectItems)
            {
                if (item.Name == fileName)
                    return item;
            }

            return null;
        }
    }
}