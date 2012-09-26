using EnvDTE;

namespace TeamNotification_Test.Stubs
{
    public class StubDocument : Document
    {
        public void Activate()
        {
            return;
        }

        public void Close(vsSaveChanges Save = vsSaveChanges.vsSaveChangesPrompt)
        {
            return;
        }

        public Window NewWindow()
        {
            return new StubWindow();
        }

        public bool Redo()
        {
            return true;
        }

        public bool Undo()
        {
            return true;
        }

        public vsSaveStatus Save(string FileName = "")
        {
            return 0;
        }

        public object Object(string ModelKind = "")
        {
            return null;
        }

        public void PrintOut()
        {
            return;
        }

        public void ClearBookmarks()
        {
            return;
        }

        public bool MarkText(string Pattern, int Flags = 0)
        {
            return true;
        }

        public bool ReplaceText(string FindText, string ReplaceText, int Flags = 0)
        {
            return true;
        }

        public DTE DTE
        {
            get { return null; }
        }

        public string Kind
        {
            get { return "Blah Kind"; }
        }

        public Documents Collection
        {
            get { return null; }
        }

        public Window ActiveWindow
        {
            get { return new StubWindow(); }
        }

        public string FullName 
        { 
            get { return "Blah FullName"; } 
        }

        public string Name
        {
            get { return "Blah Name"; }
        }

        public string Path
        {
            get { return "Blah Path"; }
        }

        public bool ReadOnly { get; set; }

        public bool Saved { get; set; }

        public Windows Windows
        {
            get { return null; }
        }

        public ProjectItem ProjectItem
        {
            get { return new StubProjectItem(); }
        }

        public object Selection
        {
            get { return null; }
        }

        public object get_Extender(string ExtenderName)
        {
            return null;
        }

        public object ExtenderNames
        {
            get { return null; }
        }

        public string ExtenderCATID
        {
            get { return ""; }
        }

        public int IndentSize
        {
            get { return 0; }
        }

        public string Language { get; set; }

        public int TabSize
        {
            get { return 0; }
        }

        public string Type
        {
            get { return "Blah Type"; }
        }
    }
}