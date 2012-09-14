using Microsoft.VisualStudio.Shell.Interop;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;

namespace AvenidaSoftware.TeamNotification_Package
{
    public sealed class ToolWindowEvents: IVsWindowFrameNotify3
    {
        private readonly TeamNotificationPackage package;
        private readonly IHandleToolWindowEvents toolWindowEvents;

        public ToolWindowEvents(TeamNotificationPackage package, IHandleToolWindowEvents toolWindowEvents)
        {
            this.package = package;
            this.toolWindowEvents = toolWindowEvents;
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
            toolWindowEvents.OnDockableChange(this, new ToolWindowWasDocked
                                                        {
                                                            isDocked = fDockable == 1,
                                                            x = x,
                                                            y = y,
                                                            w = w,
                                                            h = h
                                                        });

            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnClose(ref uint pgrfSaveOptions)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }
    }
}