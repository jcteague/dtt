using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AvenidaSoftware.TeamNotification_Package.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Controls;
using Container = TeamNotification_Library.Service.Container;

namespace AvenidaSoftware.TeamNotification_Package
{
    [Guid(GuidList.guidLoginWindowPersistanceString)]
    public class LoginWindow : ToolWindowPane
    {
        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public LoginWindow() :
            base(null)
        {           
            // Set the window title reading it from the resources.
            this.Caption = Resources.ToolWindowTitle;
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.
            if(Container.GetInstance<IServiceLoginControl>().IsUserLogged())
            {
                base.Content = Container.GetInstance<Chat>();
            }
            else
            {
                base.Content = Container.GetInstance<LoginControl>();
            }
        }

        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();
            var handler = new ToolWindowEvents((TeamNotificationPackage)this.Package, Container.GetInstance<IHandleToolWindowEvents>());
            ((IVsWindowFrame)this.Frame).SetProperty((int)__VSFPROPID.VSFPROPID_ViewHelper, handler);
//
//            var pdwSFP = new VSSETFRAMEPOS[1];
//            Guid guid;
//            int px;
//            int py;
//            int pcx;
//            int pcy;
//            ((IVsWindowFrame)this.Frame).GetFramePos(pdwSFP, out guid, out px, out py, out pcx, out pcy);
//
//            object res0;
//            ((IVsWindowFrame) this.Frame).GetProperty((int) __VSFPROPID.VSFPROPID_FrameMode, out res0);
//
//            object res2;
//            ((IVsWindowFrame)this.Frame).GetProperty((int)__VSFPROPID.VSFPROPID_SPFrame, out res2);
//            
//            object res3;
//            ((IVsWindowFrame)this.Frame).GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out res3);
//            
//
//            object res;
//            ((IVsWindowFrame) this.Frame).GetProperty((int) __VSFPROPID.VSFPROPID_WindowState, out res);
//
//            object res1;
//            ((IVsWindowFrame)this.Frame).GetProperty((int)__VSFPROPID.VSFPROPID_IsWindowTabbed, out res1);
//
//            int a = 0;
        }
    }
}