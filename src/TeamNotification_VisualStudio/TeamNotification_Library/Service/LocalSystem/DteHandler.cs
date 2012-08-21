using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class DteHandler : IHandleDte
    {
        private const string vsViewKindCode = "{7651A701-06E5-11D1-8EBD-00A0C90F26EA}";
        public FileInfo Solution { get; private set; }
        public Projects Projects { get; private set; }
        public bool HasTextOnLine { get; private set; }

        public EnvDTE.Solution CurrentSolution { get; private set; }

        public DteHandler(EnvDTE.Solution solution)
        {
            CurrentSolution = solution;
            this.Solution = new FileInfo(solution.FileName);
            this.Projects = solution.Projects;
            this.HasTextOnLine = false;
        }
        public Document OpenFile(string projectName, string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            foreach(Project p in this.Projects)
            {
                if (p.UniqueName != projectName) continue;
                foreach (ProjectItem d in p.ProjectItems)
                {
                    if (d.Name == fileInfo.Name)
                    {
                        Window w = d.Open(vsViewKindCode);
                        w.Visible = true;
                        w.Activate();
                        return d.Document;
                    }
                }
                break;
            }
            return null;
        }

        public EditPoint GetEditPoint(string projectName, string fileName, int line)
        {
            var document = OpenFile(projectName, fileName);
            if (document != null)
            {
                var editDoc = (TextDocument) document.Object("TextDocument");
                var objEditPt = editDoc.CreateEditPoint();
                objEditPt.StartOfDocument();
                document.ReadOnly = false;

                var i = 1;
                while (!objEditPt.AtEndOfDocument && i < line)
                {
                    objEditPt.LineDown();
                    ++i;
                }
                HasTextOnLine = !(objEditPt.AtEndOfDocument);
                return objEditPt;
            }
            return null;
        }

        public void PasteCode(EditPoint objEditPt, string code, PasteOptions pasteOption)
        {
            var newCode = new StringBuilder("//Begining pasted code from conversation\r\n");
            newCode.AppendLine(code);
            newCode.AppendLine("//Ending pasted code from conversation");
            var c = newCode.ToString();

            switch (pasteOption)
            {
                case PasteOptions.Overwrite:
                    objEditPt.Delete(code.Length);
                    break;
                case PasteOptions.Append:
                    objEditPt.EndOfDocument();// code.Length);
                    break;
            }
            objEditPt.Insert(c);
        }
    }
}
