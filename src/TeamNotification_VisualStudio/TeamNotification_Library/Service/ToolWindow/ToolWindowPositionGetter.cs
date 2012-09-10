using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EnvDTE;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service.LocalSystem;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class ToolWindowPositionGetter : IGetToolWindowPosition
    {
        private IStoreDTE dteStore;

        public ToolWindowPositionGetter(IStoreDTE dteStore)
        {
            this.dteStore = dteStore;
        }

        public int Get(int x, int y, int w, int h, bool isDocked)
        {
            if (!isDocked)
                return GlobalConstants.DockPositions.NotDocked;

            var xc = dteStore.MainWindow.Width * 0.5;
            var yc = dteStore.MainWindow.Height * 0.5;

            foreach (Window win in dteStore.dte.Windows)
            {
                if (win.ObjectKind.ToLower() == "{052b3fa5-4a40-4588-9614-5b99dce7120d}")
                {
                    var left = win.Left < xc && h > yc;
                    var top = win.Top < yc && w > xc;
                    var right = win.Left > xc && h > yc;
                    var bottom = win.Top > yc && w > xc;

                    if (left)
                        return GlobalConstants.DockPositions.Left;

                    if (top)
                        return GlobalConstants.DockPositions.Top;

                    if (right)
                        return GlobalConstants.DockPositions.Right;

                    if (bottom)
                        return GlobalConstants.DockPositions.Bottom;

                }
            }
            return GlobalConstants.DockPositions.NotDocked;
        }
    }
}