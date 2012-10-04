using EnvDTE;

namespace TeamNotification_Test.Stubs
{
    public class StubWindow : Window
    {
        public void SetFocus()
        {
            throw new System.NotImplementedException();
        }

        public void SetKind(vsWindowType eKind)
        {
            throw new System.NotImplementedException();
        }

        public void Detach()
        {
            throw new System.NotImplementedException();
        }

        public void Attach(int lWindowHandle)
        {
            throw new System.NotImplementedException();
        }

        public void Activate()
        {
            return;
        }

        public void Close(vsSaveChanges SaveChanges = vsSaveChanges.vsSaveChangesNo)
        {
            throw new System.NotImplementedException();
        }

        public void SetSelectionContainer(ref object[] Objects)
        {
            throw new System.NotImplementedException();
        }

        public void SetTabPicture(object Picture)
        {
            throw new System.NotImplementedException();
        }

        public Windows Collection
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Visible { get; set; }

        public int Left
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int Top
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int Width
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int Height
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public vsWindowState WindowState
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public vsWindowType Type
        {
            get { throw new System.NotImplementedException(); }
        }

        public LinkedWindows LinkedWindows
        {
            get { throw new System.NotImplementedException(); }
        }

        public Window LinkedWindowFrame
        {
            get { throw new System.NotImplementedException(); }
        }

        public int HWnd
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Kind
        {
            get { throw new System.NotImplementedException(); }
        }

        public string ObjectKind
        {
            get { throw new System.NotImplementedException(); }
        }

        public object Object
        {
            get { throw new System.NotImplementedException(); }
        }

        public object get_DocumentData(string bstrWhichData = "")
        {
            throw new System.NotImplementedException();
        }

        public ProjectItem ProjectItem
        {
            get { throw new System.NotImplementedException(); }
        }

        public Project Project
        {
            get { throw new System.NotImplementedException(); }
        }

        public DTE DTE
        {
            get { throw new System.NotImplementedException(); }
        }

        public Document Document
        {
            get { throw new System.NotImplementedException(); }
        }

        public object Selection
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Linkable
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Caption
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsFloating
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool AutoHides
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public ContextAttributes ContextAttributes
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}