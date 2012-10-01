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
            return FindProject(projectName).Select(x => FindDocumentIn(x.ProjectItems, fileName));
        }

        private Maybe<Project> FindProject(string projectName)
        {
            return visualStudioProjectsList.GetAllProjects().First(x => RemoveUnnecessaryPath(x) == projectName).ToMaybe();
        }

        private string RemoveUnnecessaryPath(Project project)
        {
            return project.UniqueName.Remove(0, project.UniqueName.LastIndexOf('\\') + 1);
        }

        private ProjectItem FindDocumentIn(ProjectItems projectItems, string fileName)
        {
            foreach (ProjectItem projectItem in projectItems)
            {
                if (projectItem.IsNull())
                    continue;

                var name = projectItem.Name;
                if (name == fileName)
                {
                    return projectItem;
                }
                
                if (projectItem.ProjectItems.Count > 0)
                {
                    var document = FindDocumentIn(projectItem.ProjectItems, fileName);
                    if (document.IsNotNull())
                        return document;
                }
            }

            return null;
        }
    }
}