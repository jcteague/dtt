using System;
using System.Windows;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Highlighters.Avalon;

namespace AvenidaSoftware.TeamNotification_Package.Controls.Avalon
{
    public class MixedTextEditor : TextEditor
    {
        private IHandleMixedEditorEvents mixedEditorEvents;

        public MixedTextEditor() : base()
        {
            mixedEditorEvents = Container.GetInstance<IHandleMixedEditorEvents>();
            mixedEditorEvents.CodeWasAppended += AppendCode;
            TextArea.ActiveInputHandler = new MixedEditorTextAreaInputHandler(TextArea);
        }

        private void AppendCode(object sender, CodeWasAppended e)
        {
            int a = 0;
        }

        protected override IVisualLineTransformer CreateColorizer(IHighlightingDefinition highlightingDefinition)
        {
            if (highlightingDefinition.IsNull())
                throw new ArgumentNullException("highlightingDefinition");
            return new MixedHighlightingColorizer(highlightingDefinition.MainRuleSet);
        }

    }
}