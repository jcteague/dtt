using System.Linq;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Editing;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Controls;

namespace TeamNotification_Library.Service.Highlighters.Avalon
{
    public class MixedEditorTextAreaInputHandler : TextAreaDefaultInputHandler
    {
        public MixedEditorTextAreaInputHandler(TextArea textArea) : base(textArea)
        {
            // Just to be able to execute our paste event
            NestedInputHandlers.Remove(Editing);
            NestedInputHandlers.Add(GetHandler(textArea));
            NestedInputHandlers.Add(Editing);
        }

        private TextAreaInputHandler GetHandler(TextArea textArea)
        {
			TextAreaInputHandler handler = new TextAreaInputHandler(textArea);
            handler.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnMixedEditorPaste, CanPaste));
			return handler;
        }

        private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnMixedEditorPaste(object sender, ExecutedRoutedEventArgs e)
        {
            var clipboardStorage = Container.GetInstance<IStoreClipboardData>();
            var chatMessageModel = clipboardStorage.Get<ChatMessageModel>();
            var pasteData = new DataWasPasted(chatMessageModel.chatMessageBody.message, chatMessageModel);
            Container.GetInstance<IHandleMixedEditorEvents>().OnDataWasPasted(sender, pasteData);
        }
    }
}