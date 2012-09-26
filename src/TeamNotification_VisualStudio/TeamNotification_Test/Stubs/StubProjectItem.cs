using EnvDTE;

namespace TeamNotification_Test.Stubs
{
    public class StubProjectItem : ProjectItem
    {
        public bool SaveAs(string NewFileName)
        {
            return false;
        }

        public Window Open(string ViewKind = "{00000000-0000-0000-0000-000000000000}")
        {
            return new StubWindow();
        }

        public void Remove()
        {
            return;
        }

        public void ExpandView()
        {
            return;
        }

        public void Save(string FileName = "")
        {
            return;
        }

        public void Delete()
        {
            return;
        }

        public bool IsDirty { get; set; }

        public string get_FileNames(short index)
        {
            return "Blah ProjectItem FileNames";
        }

        public short FileCount
        {
            get { return 1; }
        }

        public string Name
        {
            get { return "Blah ProjectItem Name"; }
            set { throw new System.NotImplementedException(); }
        }

        public ProjectItems Collection
        {
            get { return null; }
        }

        public Properties Properties
        {
            get { return null; }
        }

        public DTE DTE
        {
            get { return null; }
        }

        public string Kind
        {
            get { return "Blah ProjectItem Kind"; }
        }

        public ProjectItems ProjectItems
        {
            get { return null; }
        }

        public bool get_IsOpen(string ViewKind = "{FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF}")
        {
            throw new System.NotImplementedException();
        }

        public object Object
        {
            get { throw new System.NotImplementedException(); }
        }

        public object get_Extender(string ExtenderName)
        {
            throw new System.NotImplementedException();
        }

        public object ExtenderNames
        {
            get { throw new System.NotImplementedException(); }
        }

        public string ExtenderCATID
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Saved { get; set; }

        public ConfigurationManager ConfigurationManager
        {
            get { throw new System.NotImplementedException(); }
        }

        public FileCodeModel FileCodeModel
        {
            get { return null; }
        }

        public Document Document
        {
            get { return new StubDocument(); }
        }

        public Project SubProject
        {
            get { return null; }
        }

        public Project ContainingProject
        {
            get { return null; }
        }
    }
}