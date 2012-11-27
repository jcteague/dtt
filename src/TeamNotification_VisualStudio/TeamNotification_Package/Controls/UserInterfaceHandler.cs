using System;
using StructureMap;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Extensions;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    public class UserInterfaceHandler : IHandleUI
    {
        private IHandleUIEvents userInterfaceEvents;

        public UserInterfaceHandler(IHandleUIEvents userInterfaceEvents)
        {
            this.userInterfaceEvents = userInterfaceEvents;
        }

        public void Initialize()
        {
            userInterfaceEvents.ControlRenderWasRequested += ShowControl;
        }

        private void ShowControl(object sender, ControlRenderWasRequested e)
        {
            var instance = Activator.CreateInstance(e.Control);
            if (e.Action.IsNotNull())
                e.Action(instance);
        }
    }
}