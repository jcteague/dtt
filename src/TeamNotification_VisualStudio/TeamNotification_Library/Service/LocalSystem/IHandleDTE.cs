
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{
    public enum PasteOptions { Insert = 0, Append, Overwrite }
    public interface IHandleDte
    {
        bool HasTextOnLine { get; }
        FileInfo Solution { get; }
        Solution CurrentSolution { get; }
        Projects Projects{ get;  }
        Document OpenFile(string projectName, string fileName);
        EditPoint GetEditPoint(string projectName, string fileName, int line);
        void PasteCode(EditPoint objEditPt, string code, PasteOptions pasteOption);
    }
}
