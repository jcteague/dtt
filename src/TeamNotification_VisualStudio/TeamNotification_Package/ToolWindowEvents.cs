using Microsoft.VisualStudio.Shell.Interop;

namespace AvenidaSoftware.TeamNotification_Package
{
    public sealed class ToolWindowEvents: IVsWindowFrameNotify3
    {
        private readonly TeamNotificationPackage package;

        public ToolWindowEvents(TeamNotificationPackage package)
        {
            this.package = package;
        }

        public int OnShow(int fShow)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnMove(int x, int y, int w, int h)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnSize(int x, int y, int w, int h)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnDockableChange(int fDockable, int x, int y, int w, int h)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnClose(ref uint pgrfSaveOptions)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }
    }
}