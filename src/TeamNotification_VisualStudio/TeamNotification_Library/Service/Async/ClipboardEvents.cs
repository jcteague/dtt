using EnvDTE;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class ClipboardEvents : AbstractEventHandler, IHandleClipboardEvents
    {
        public event CustomEventHandler<ClipboardHasChanged> ClipboardHasChanged;

//        public void Raise(DTE dte)
//        {
////            var currentWindow = GetForegroundWindow();
//
//            
//            var txt = dte.ActiveDocument.Object() as TextDocument;
//            if (txt.IsNotNull())
//            {
//                var selection = txt.Selection;
//
//                var clipboardArgs = new ClipboardHasChanged
//                {
//                    Solution = dte.Solution.FullName,
//                    Document = dte.ActiveDocument.FullName,
//                    Text = selection.Text,
//                    Line = selection.CurrentLine
//                };
//                OnClipboardChanged(this, clipboardArgs);
//            }
//        }

        public void OnClipboardChanged(object source, ClipboardHasChanged args)
        {
            Handle(source, ClipboardHasChanged, args);
        }
    }
}