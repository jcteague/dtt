using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class VisualStudioProjectsList : IListVisualStudioProjects
    {
        private readonly IStoreDTE dteStore;

        public VisualStudioProjectsList(IStoreDTE dteStore)
        {
            this.dteStore = dteStore;
        }

        public IEnumerable<Project> GetAllProjects()
        {
            IWrapProject[] projects = dteStore.Solution.Projects;
            var flattenedProjects = new List<Project>();
            foreach (IWrapProject wrappedProject in projects)
            {
                var project = wrappedProject.Value;
                if (project.IsNull())
                    continue;
                if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    flattenedProjects.AddRange(GetSubProjectsFrom(project));
                }
                else
                {
                    flattenedProjects.Add(project);
                }
            }

            return flattenedProjects;
        }

        private IEnumerable<Project> GetSubProjectsFrom(Project project)
        {
            var flattenedSubProjects = new List<Project>();
            foreach (ProjectItem projectItem in project.ProjectItems)
            {
                var subProject = projectItem.SubProject;
                if (subProject.IsNull())
                    continue;
                
                if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    flattenedSubProjects.AddRange(GetSubProjectsFrom(subProject));
                }
                else
                {
                    flattenedSubProjects.Add(subProject);
                }
            }

            return flattenedSubProjects;
        }
    }
}