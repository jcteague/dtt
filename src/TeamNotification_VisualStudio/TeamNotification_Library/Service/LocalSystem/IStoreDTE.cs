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
        IWrapWindow MainWindow { get; }
        IEnumerable<IWrapWindow> Windows { get; }
    }
    
    public interface IWrapWindow
    {
        int Width { get; }
        int Height { get; }
        bool IsPluginWindow();
        bool IsStartPageWindow();
        bool IsDocumentWindow();
        bool IsFloating { get; }
        int Left { get; }
        int Top { get; }
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
        IWrapTextDocument TextDocument { get; }
        IWrapSelection Selection { get; }
        bool ReadOnly { get; set; }
        int Length { get; }
        int Lines { get; }
        void Paste(int line, string text, PasteOptions option);
        void Paste(int line, PasteOptions option);
        void Undo();
    }

    public interface IWrapSelection
    {
        void Copy();
        void Cut();
        void Select(int line, int lines = 1);
        string Text { get; }
        int Lines { get; }
    }

    public interface IWrapTextDocument
    {
        TextSelection Selection { get; }
        EditPoint CreateEditPoint();
        TextPoint EndPoint { get; }
        TextPoint StartPoint { get; }
    }
}
