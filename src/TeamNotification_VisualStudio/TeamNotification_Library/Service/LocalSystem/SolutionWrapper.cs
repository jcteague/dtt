using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class SolutionWrapper : IWrapSolution
    {
        private EnvDTE.Solution Solution { get; set; }
        //public FileInfo File { get { return new FileInfo(Solution.FileName); } }
        public IWrapProject[] Projects { get; private set; }
        public string FileName { get { return Solution.FileName.Remove(0, Solution.FileName.LastIndexOf('\\') + 1); } }
        public bool IsOpen { get { return Solution.IsOpen; } }
        public IWrapProject FindProject(string projectName)
        {
            var project = this.Projects.Where(x => x.UniqueName == projectName);
            var count = project.Count();
            return count > 0 ? project.ElementAt(0) : null;
        }

        public SolutionWrapper(EnvDTE.Solution solution)
        {
            Solution = solution;
            var projects = new ProjectWrapper[solution.Projects.Count];
            var i = 0;
            foreach (Project p in solution.Projects)
                projects[i++] = new ProjectWrapper(p);
            Projects = projects;
        }
    }
}
