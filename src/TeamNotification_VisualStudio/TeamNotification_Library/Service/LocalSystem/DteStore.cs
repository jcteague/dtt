using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public class WindowWrapper : IWrapWindow
    {
        private Window window;

        public WindowWrapper(Window window)
        {
            this.window = window;
        }

        public int Width
        {
            get { return window.Width; }
        }

        public int Height
        {
            get { return window.Height; }
        }

        public bool IsPluginWindow()
        {
            return window.IsPluginWindow();
        }

        public bool IsFloating
        {
            get { return window.IsFloating; }
        }

        public int Left
        {
            get { return window.Left; }
        }

        public int Top
        {
            get { return window.Top; }
        }
    }

    public class ProjectWrapper : IWrapProject
    {
        private EnvDTE.Project Project { get; set; }
        public ProjectItemWrapper[] ProjectItems { get; set; }
        public string UniqueName { get { return Project.UniqueName.Remove(0, Project.UniqueName.LastIndexOf('\\') + 1); } }
        public IWrapProjectItem FindDocument(string fileName)
        {
            var doc = this.ProjectItems.Where(x => x.Name == fileName);
            var count = doc.Count();
            return count > 0 ? doc.ElementAt(0) : null;
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
        public string Name { get { return _projectItem.Name; } }
        public IWrapDocument Document { get { return new DocumentWrapper(_projectItem.Document); } }

        public ProjectItemWrapper(EnvDTE.ProjectItem projectItem) { _projectItem = projectItem; }
        public Window Open(string viewKind = "{7651A701-06E5-11D1-8EBD-00A0C90F26EA}") { return _projectItem.Open(viewKind); }
    }
}
