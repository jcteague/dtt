
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
        bool IsValidSolution { get; }
        IWrapSolution CurrentSolution { get; }
        IWrapDocument OpenFile(string projectName, string fileName);
        EditPoint GetEditPoint(IWrapDocument document, int line);
        void PasteCode(EditPoint objEditPt, string code, PasteOptions pasteOption);
    }
}
