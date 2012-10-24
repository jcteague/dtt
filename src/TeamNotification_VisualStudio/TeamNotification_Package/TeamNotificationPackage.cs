using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows;
using AvenidaSoftware.TeamNotification_Package.Controls;
using EnvDTE;
using Microsoft.VisualStudio.ExtensionManager;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using NLog;
using NLog.Config;
using NLog.Targets;
using StructureMap;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.LocalSystem;
using TeamNotification_Library.Service.Logging;
using TeamNotification_Library.Service.Logging.Providers;
using TeamNotification_Library.Service.Update;

namespace AvenidaSoftware.TeamNotification_Package
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    //[ProvideToolWindow(typeof(MyToolWindow))]
    [ProvideToolWindow(typeof (LoginWindow))]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    [Guid(GuidList.guidTeamNotificationPkgString)]
    public sealed class TeamNotificationPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public TeamNotificationPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));

        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            //            // Get the instance number 0 of this tool window. This window is single instance so this instance6
            //            // is actually the only one.
            //            // The last flag is set to true so that if the tool window does not exists it will be created.
            //            ToolWindowPane window = this.FindToolWindow(typeof(MyToolWindow), 0, true);
            //            if ((null == window) || (null == window.Frame))
            //            {
            //                throw new NotSupportedException(Resources.CanNotCreateWindow);
            //            }
            //            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            //            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());

            //            IVsSolution solution = (IVsSolution) Package.GetGlobalService(typeof(SVsSolution));
            //            string solutionDir;
            //            string solutionFile;
            //            string optsFile;
            //            var b = solution.GetSolutionInfo(out solutionDir, out solutionFile, out optsFile);

            //            var b = VsShellUtilities.GetProject(new SolutionServiceProvider(), "cs");

            //            DTE dte = (DTE) GetService(typeof (DTE));
            //            Debug.WriteLine(dte.Solution.FileName);
            //            Debug.WriteLine(dte.ActiveDocument);
            //            int a = 0;

        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowLoginWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = this.FindToolWindow(typeof (LoginWindow), 0, true);

            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame) window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }


        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            var pdm = GetService(typeof (SVsProfileDataManager)) as IVsProfileDataManager;
            string settingsLocation;
            pdm.GetDefaultSettingsLocation(out settingsLocation);

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof (IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidTeamNotificationCmdSet,
                                                        (int) PkgCmdIDList.teamNotificationToolCommand);
                MenuCommand menuItem = new MenuCommand(ShowLoginWindow, menuCommandID);

                mcs.AddCommand(menuItem);
                // Create the command for the tool window
                //CommandID toolwndCommandID = new CommandID(GuidList.guidTeamNotificationCmdSet, (int)PkgCmdIDList.openTeamNotificationWindow);
                //MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
                //mcs.AddCommand( menuToolWin );
            }


            Bootstrapper.Initialize();

            var dialogMessagesEvents = ObjectFactory.GetInstance<IHandleDialogMessages>();
            dialogMessagesEvents.AlertMessageWasRequested += (s, e) => Alert(e.Message);
            dialogMessagesEvents.DialogMessageWasRequested += (s, e) => ShowOkCancelDialog(e.Message, e.OkAction, e.CancelAction);
            

            ObjectFactory.GetInstance<IConfigureLogging>().Initialize();

            ObjectFactory.GetInstance<ILog>().TryOrLog(() =>
            {
                var updateManager = Package.GetGlobalService(typeof(SVsExtensionManager)) as IVsExtensionManager;
                var repoManager = Package.GetGlobalService(typeof(SVsExtensionRepository)) as IVsExtensionRepository;

                ObjectFactory.GetInstance<ICheckForUpdates>().CheckForUpdates(updateManager, repoManager);
            });
        }

        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            Alert(string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.ToString()));
        }

        private void Alert(string message)
        {
            // Show a Message Box to prove we were here
            IVsUIShell uiShell = (IVsUIShell) GetService(typeof (SVsUIShell));
            Guid clsid = Guid.Empty;
            int result;

            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
                0,
                ref clsid,
                "YacketyApp",
                message,
                string.Empty,
                0,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                OLEMSGICON.OLEMSGICON_INFO,
                0, // false
                out result));
        }

        private void ShowOkCancelDialog(string message, Action okAction, Action cancelAction)
        {
            CustomMessageBox.ShowOkCancel(message, okAction, cancelAction);
        }
    }
}
