using System;
using System.Globalization;

namespace TeamNotification_Library.Service.LocalSystem.Messages
{
    public interface IShowAlertMessages
    {
        void Print(string message);
    }

    public class MessagesAlert : IShowAlertMessages
    {
        public void Print(string message)
        {
//            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
//            Guid clsid = Guid.Empty;
//            int result;
//
//            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
//                       0,
//                       ref clsid,
//                       "TeamNotification",
//                       string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.ToString()),
//                       string.Empty,
//                       0,
//                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
//                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
//                       OLEMSGICON.OLEMSGICON_INFO,
//                       0,        // false
//                       out result));
        }
    }
}