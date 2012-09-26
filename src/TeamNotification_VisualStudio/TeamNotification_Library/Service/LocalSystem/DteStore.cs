using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EnvDTE;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class DTEStore : IStoreDTE
    {
        public DTE dte { get; set; }
        public IWrapSolution Solution { get { return new SolutionWrapper(dte.Solution); } }
        public IWrapWindow MainWindow { get { return new WindowWrapper(dte.MainWindow);} }
        public IEnumerable<IWrapWindow> Windows
        {
            get
            {
                foreach (Window window in dte.Windows)
                {
                    yield return new WindowWrapper(window);
                }
            }
        }
    }

    public class ProjectWrapper : IWrapProject
    {
        private EnvDTE.Project Project { get; set; }
        public ProjectItemWrapper[] ProjectItems { get; set; }
        public string UniqueName { get { return Project.UniqueName.Remove(0, Project.UniqueName.LastIndexOf('\\') + 1); } }
        
        public Project Value 
        { 
            get { return Project; }
        }

        public ProjectWrapper(EnvDTE.Project project)
        {
            Project = project;
            var projectItems = new ProjectItemWrapper[project.ProjectItems.Count];
            var i = 0;
            foreach (ProjectItem pi in project.ProjectItems)
                projectItems[i++] = new ProjectItemWrapper(pi);
            ProjectItems = projectItems;
        }
    }
    
    public class ProjectItemWrapper : IWrapProjectItem
    {
        private readonly EnvDTE.ProjectItem _projectItem;
        public IWrapProject SubProject
        {
            get { return new ProjectWrapper(_projectItem.SubProject); }
        }
        public string Name { get { return _projectItem.Name; } }
        public IWrapDocument Document { get { return new DocumentWrapper(_projectItem.Document); } }

        public ProjectItemWrapper(EnvDTE.ProjectItem projectItem) { _projectItem = projectItem; }
        public Window Open(string viewKind = "{7651A701-06E5-11D1-8EBD-00A0C90F26EA}") { return _projectItem.Open(viewKind); }
    }
}
