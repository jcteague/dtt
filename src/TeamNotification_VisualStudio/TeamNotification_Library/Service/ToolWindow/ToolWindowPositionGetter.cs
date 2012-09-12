using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EnvDTE;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service.LocalSystem;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class ToolWindowPositionGetter : IGetToolWindowPosition
    {
        private IStoreDTE dteStore;

        public ToolWindowPositionGetter(IStoreDTE dteStore)
        {
            this.dteStore = dteStore;
        }

        private bool IsWindowToUseForDocking(IWrapWindow window)
        {
            return window.IsStartPageWindow() || window.IsDocumentWindow();
        }

        public int Get()
        {
            foreach (Window win in dteStore.dte.Windows)
            {
                Debug.WriteLine(win.Caption + ": " + win.ObjectKind);
            }

//            Debug.WriteLine(dteStore.dte.ActiveDocument.Windows);
//            Debug.WriteLine(dteStore.dte.ActiveDocument.ActiveWindow.Height);

            int pluginWidth = 0;
            int pluginHeight = 0;
            int documentWindowWidth = 0;
            int documentWindowHeight = 0;

            foreach (var win in dteStore.Windows)
            {
                if (IsWindowToUseForDocking(win))
                {
                    documentWindowWidth = win.Width;
                    documentWindowHeight = win.Height;
                }

                if (win.IsPluginWindow())
                {
                    if (win.IsFloating)
                        return GlobalConstants.DockPositions.NotDocked;

                    pluginWidth = win.Width;
                    pluginHeight = win.Height;

//                    var xc = dteStore.MainWindow.Width * 0.5;
//                    var yc = dteStore.MainWindow.Height * 0.5;
//
//                    var mainWindowWidth = dteStore.MainWindow.Width.ToString();
//                    var mainWindowHeight = dteStore.MainWindow.Height.ToString();
//
//                    var dockWidth = win.Width.ToString();
//                    var dockHeight = win.Height.ToString();
//
//                    var dockHalfWidth = win.Width*0.5;
//                    var dockHalfHeight = win.Height*0.5;
//
//                    var top = win.Top < yc && win.Width > xc;
//                    var bottom = win.Top > yc && win.Width > xc;
//                    var left = win.Left < xc && win.Height > yc;
//                    var right = win.Left > xc && win.Height > yc;
//
//                    if (top)
//                        return GlobalConstants.DockPositions.Top;
//
//                    if (bottom)
//                        return GlobalConstants.DockPositions.Bottom;
//
//                    if (left)
//                        return GlobalConstants.DockPositions.Left;
//
//                    if (right)
//                        return GlobalConstants.DockPositions.Right;



//                    var marginLeft = (dteStore.MainWindow.Width - win.Width)/2;
//                    var left = win.Left;
//                    var touchingLeftAndRight = marginLeft == win.Left;
//                    
//                    var marginTop = (dteStore.MainWindow.Height - win.Height)/2;
//                    var top = win.Top;
//                    var touchingTopAndBottom = marginTop == win.Top;
//                    
//                    if (touchingLeftAndRight)
//                        return GlobalConstants.DockPositions.Left;
//
//                    if (touchingTopAndBottom)
//                        return GlobalConstants.DockPositions.Top;

//                    var xc = dteStore.MainWindow.Width * 0.5;
//                    var yc = dteStore.MainWindow.Height * 0.5;
//
//                    var tendsToLeft = win.Left < xc;
//                    var tendsToTop = win.Top < yc;
//                    var tendsToRight = (win.Left + win.Width) > xc;
//                    var tendsToBottom = (win.Top) > yc;
//
//                    if (tendsToLeft && tendsToTop && tendsToRight)
//                        return GlobalConstants.DockPositions.Top;
//
//                    if (tendsToLeft && tendsToTop && tendsToBottom)
//                        return GlobalConstants.DockPositions.Left;
//
//                    if (tendsToLeft && tendsToRight && tendsToBottom)
//                        return GlobalConstants.DockPositions.Bottom;
//
//                    if (tendsToTop && tendsToRight && tendsToBottom)
//                        return GlobalConstants.DockPositions.Right;
                }
            }

            var isTop = pluginWidth >= documentWindowWidth;
            var isLeft = pluginHeight >= documentWindowHeight;

            if (isTop)
                return GlobalConstants.DockPositions.Top;

            if (isLeft)
                return GlobalConstants.DockPositions.Left;

            return GlobalConstants.DockPositions.NotDocked;
        }
    }
}