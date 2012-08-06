using System.Diagnostics;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace AvenidaSoftware.TeamNotification_Package.BackgroundService
{
    public class DTTClipboardService : SVsUIHierWinClipboardHelper, IVsUIHierWinClipboardHelper
    {
        private IServiceProvider serviceProvider;

        public DTTClipboardService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            Debug.WriteLine("Inside DTT Clipboard Service");
        }

        public int Cut(IDataObject pDataObject)
        {
            Debug.WriteLine("Cutting Inside DTT Clipboard Service");
            Trace.WriteLine("Cutting Inside DTT Clipboard Service");
            return 0;
        }

        public int Copy(IDataObject pDataObject)
        {
            throw new System.NotImplementedException();
        }

        public int Paste(IDataObject pDataObject, uint dwEffects)
        {
            throw new System.NotImplementedException();
        }

        public int AdviseClipboardHelperEvents(IVsUIHierWinClipboardHelperEvents pSink, out uint pdwCookie)
        {
            throw new System.NotImplementedException();
        }

        public int UnadviseClipboardHelperEvents(uint dwCookie)
        {
            throw new System.NotImplementedException();
        }
    }
}