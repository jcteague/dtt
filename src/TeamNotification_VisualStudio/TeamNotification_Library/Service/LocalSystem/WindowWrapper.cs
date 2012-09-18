using EnvDTE;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class WindowWrapper : IWrapWindow
    {
        private Window window;

        public WindowWrapper(Window window)
        {
            this.window = window;
        }

        public int Width
        {
            get { return window.Width; }
        }

        public int Height
        {
            get { return window.Height; }
        }

        public bool IsPluginWindow()
        {
            return window.IsPluginWindow();
        }

        public bool IsStartPageWindow()
        {
            return window.IsStartPageWindow();
        }

        public bool IsDocumentWindow()
        {
            return window.Type == vsWindowType.vsWindowTypeDocument;
        }

        public bool IsFloating
        {
            get { return window.IsFloating; }
        }

        public int Left
        {
            get { return window.Left; }
        }

        public int Top
        {
            get { return window.Top; }
        }
    }
}