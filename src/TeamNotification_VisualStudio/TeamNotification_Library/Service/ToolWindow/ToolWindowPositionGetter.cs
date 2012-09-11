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

        public int Get()
        {
            foreach (var win in dteStore.Windows)
            {
                if (win.IsPluginWindow())
                {
                    if (win.IsFloating)
                        return GlobalConstants.DockPositions.NotDocked;

                    var xc = dteStore.MainWindow.Width * 0.5;
                    var yc = dteStore.MainWindow.Height * 0.5;

                    if (win.Top < yc && win.Width > xc)
                        return GlobalConstants.DockPositions.Top;

                    if (win.Top > yc && win.Width > xc)
                        return GlobalConstants.DockPositions.Bottom;

                    if (win.Left < xc && win.Height > yc)
                        return GlobalConstants.DockPositions.Left;

                    if (win.Left > xc && win.Height > yc)
                        return GlobalConstants.DockPositions.Right;
                    
                }
            }
            return GlobalConstants.DockPositions.NotDocked;
        }
    }
}