using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{
    public interface IStoreDTE
    {
        DTE dte { get; set; }
        IWrapSolution Solution { get; }
    }

    public interface IWrapSolution
    {
        IWrapProject[] Projects { get; }
        string FileName { get; }
        bool IsOpen { get; }
        IWrapProject FindProject(string projectName);
    }
    public interface IWrapProject
    {
        ProjectItemWrapper[] ProjectItems { get; }
        string UniqueName { get; }
        IWrapProjectItem FindDocument(string fileName);
    }

    public interface IWrapProjectItem
    {
        string Name { get; }
        IWrapDocument Document { get; }
        Window Open(string viewKind);
    }

    public interface IWrapDocument
    {
        bool ReadOnly { get; set; }
        TextDocument GetTextDocument();
    }
}
