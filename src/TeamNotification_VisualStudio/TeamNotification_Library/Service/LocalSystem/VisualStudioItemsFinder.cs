using System.Linq;
using EnvDTE;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Functional;
using TeamNotification_Library.Service.Logging;

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
            return FindProject(projectName).Select(x => DocumentFilter(x.ProjectItems, fileName));
        }

        private Maybe<Project> FindProject(string projectName)
        {
            Container.GetInstance<ILog>().Write("Looking for this project in the solution: {0}".FormatUsing(projectName));
            return visualStudioProjectsList.GetAllProjects().First(x => RemoveUnnecessaryPath(x) == projectName).ToMaybe();
        }

        private string RemoveUnnecessaryPath(Project project)
        {
            return project.UniqueName.Remove(0, project.UniqueName.LastIndexOf('\\') + 1);
        }

        private ProjectItem DocumentFilter(ProjectItems projectItems, string fileName)
        {
            Container.GetInstance<ILog>().Write("Looking for the file in the {1}: {0}".FormatUsing(fileName, projectItems.ContainingProject.Name));
            foreach (ProjectItem item in projectItems)
            {
                if (item.Name == fileName)
                    return item;
            }

            return null;
        }
    }
}