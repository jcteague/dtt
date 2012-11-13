using System.Windows.Input;
using ICSharpCode.AvalonEdit.Editing;

namespace TeamNotification_Library.Service.Highlighters.Avalon
{
    public class MixedEditorTextAreaInputHandler : TextAreaInputHandler
    {
        public MixedEditorTextAreaInputHandler(TextArea textArea) : base(textArea)
        {
            NestedInputHandlers.Add(GetHandler(textArea));
        }

        private TextAreaInputHandler GetHandler(TextArea textArea)
        {
			TextAreaInputHandler handler = new TextAreaInputHandler(textArea);
//            AddBinding(new CommandBinding(ApplicationCommands.Paste, SetResourcesForPaste));
            handler.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, SetResourcesForPaste, CanPaste));
			return handler;
        }

        private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SetResourcesForPaste(object sender, ExecutedRoutedEventArgs e)
        {
            int a = 0;
        }
    }
}