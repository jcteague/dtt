using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class DTEStore : IStoreDTE
    {
        public DTE dte { get; set; }
        public IWrapSolution Solution { get { return new SolutionWrapper(dte.Solution); } }
    }

    public class DocumentWrapper : IWrapDocument
    {
        private Document Document { get; set; }
        public bool ReadOnly { get { return Document.ReadOnly; } set { Document.ReadOnly = value; } }
        public TextDocument GetTextDocument()
        {
            if (Document == null) return null;
            return (TextDocument)Document.Object("TextDocument");
        }
        public DocumentWrapper(Document document){ Document = document; }
    }

    public class SolutionWrapper : IWrapSolution
    {
        private EnvDTE.Solution Solution { get; set; }
        //public FileInfo File { get { return new FileInfo(Solution.FileName); } }
        public IWrapProject[] Projects { get; private set; }
        public string FileName { get { return Solution.FileName.Remove(0,Solution.FileName.LastIndexOf('\\')+1); }}
        public bool IsOpen { get { return Solution.IsOpen; } }
        public IWrapProject FindProject(string projectName)
        {
            var project = this.Projects.Where(x=>x.UniqueName == projectName);
            var count = project.Count();
            return count > 0 ? project.ElementAt(0) : null;
        }

        public SolutionWrapper(EnvDTE.Solution solution)
        {
            Solution = solution;
            var projects = new ProjectWrapper[solution.Projects.Count];
            var i=0;
            foreach(Project p in solution.Projects)
                projects[i++] = new ProjectWrapper(p);
            Projects = projects;
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
