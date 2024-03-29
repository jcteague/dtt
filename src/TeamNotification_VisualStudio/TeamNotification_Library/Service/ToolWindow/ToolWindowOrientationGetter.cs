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
    public class ToolWindowOrientationGetter : IGetToolWindowOrientation
    {
        private IStoreDTE dteStore;

        public ToolWindowOrientationGetter(IStoreDTE dteStore)
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
                        return GlobalConstants.DockOrientations.InputAtRight;

                    var mainWidth = dteStore.MainWindow.Width;
                    var mainHeight = dteStore.MainWindow.Height;

                    var pluginWidth = win.Width;
                    var pluginHeight = win.Height;

                    if ((pluginHeight / (mainHeight * 1.0)) * 100 > 60 && (pluginWidth / (mainWidth * 1.0)) * 100 < 60)
                        return GlobalConstants.DockOrientations.InputAtBottom;
                }
            }

            return GlobalConstants.DockOrientations.InputAtRight;
        }
    }
}