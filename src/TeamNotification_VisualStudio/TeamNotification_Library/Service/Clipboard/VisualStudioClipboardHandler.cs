using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using EnvDTE;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.LocalSystem;

namespace TeamNotification_Library.Service.Clipboard
{
    public class VisualStudioClipboardHandler : IHandleVisualStudioClipboard
    {
        private IStoreDTE dteStore;
        readonly IStoreGlobalState applicationGlobalState;
        private IHandleSystemClipboard systemClipboardHandler;
        private IStoreClipboardData clipboardStorage;

        private bool isRegistered;

        public VisualStudioClipboardHandler(IStoreDTE dteStore, IStoreGlobalState applicationGlobalState, IHandleSystemClipboard systemClipboardHandler, IStoreClipboardData clipboardStorage)
        {
            this.dteStore = dteStore;
            this.applicationGlobalState = applicationGlobalState;
            this.systemClipboardHandler = systemClipboardHandler;
            this.clipboardStorage = clipboardStorage;
        }

        public void SetUpFor(HwndSource hwndSource)
        {
            if (!isRegistered && hwndSource.IsNotNull())
            {
                installedHandle = hwndSource.Handle;
                viewerHandle = SetClipboardViewer(installedHandle);
                hwndSource.AddHook(hwndSourceHook);
                isRegistered = true;
            }
        }

        IntPtr hwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_CHANGECBCHAIN:
                    viewerHandle = lParam;
                    if (viewerHandle != IntPtr.Zero)
                    {
                        SendMessage(viewerHandle, msg, wParam, lParam);
                    }
                    break;

                case WM_DRAWCLIPBOARD:
                    UpdateClipboard();
                    if (viewerHandle != IntPtr.Zero)
                    {
                        SendMessage(viewerHandle, msg, wParam, lParam);
                    }

                    break;
            }
            return IntPtr.Zero;
        }

        void UpdateClipboard()
        {
            var dte = dteStore.dte;
            if (applicationGlobalState.Active && dte.ActiveWindow.Document.IsNotNull())
            {
                var activeDocument = dte.ActiveDocument;
                var txt = activeDocument.Object() as TextDocument;
                if (txt.IsNull()) return;
                var selection = txt.Selection;
                var activeProjects = dte.ActiveDocument.ProjectItem.ContainingProject;
                var message = systemClipboardHandler.GetText(true);
                var clipboard = new ChatMessageModel
                {
                    chatMessageBody = new ChatMessageBody
                    {
                        project = activeProjects.UniqueName,
                        solution = dte.Solution.FullName,
                        document = activeDocument.FullName,
                        message = message,
                        line = selection.CurrentLine,
                        column = selection.CurrentColumn,
                        programminglanguage = activeDocument.GetProgrammingLanguage()
                    }
                };

                clipboardStorage.Store(clipboard);
            }
            else
            {
                var clipboard = new ChatMessageModel { chatMessageBody = new ChatMessageBody { message = systemClipboardHandler.GetText(true) } };
                clipboardStorage.Store(clipboard);
            }
        }

        IntPtr viewerHandle = IntPtr.Zero;
        IntPtr installedHandle = IntPtr.Zero;

        const int WM_DRAWCLIPBOARD = 0x308;
        const int WM_CHANGECBCHAIN = 0x30D;

        [DllImport("user32.dll")]
        private extern static IntPtr SetClipboardViewer(IntPtr hWnd);
        [DllImport("user32.dll")]
        private extern static int ChangeClipboardChain(IntPtr hWnd, IntPtr hWndNext);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
    }
}