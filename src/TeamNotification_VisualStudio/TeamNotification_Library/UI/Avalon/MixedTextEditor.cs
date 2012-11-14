using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Highlighters.Avalon;

namespace TeamNotification_Library.UI.Avalon
{
    public class MixedTextEditor : TextEditor
    {
        private IHandleMixedEditorEvents mixedEditorEvents;

        public MixedTextEditor() : base()
        {
            mixedEditorEvents = Container.GetInstance<IHandleMixedEditorEvents>();
            mixedEditorEvents.CodeWasAppended += AppendCode;
            mixedEditorEvents.TextWasAppended += AppendText;
            TextArea.ActiveInputHandler = new MixedEditorTextAreaInputHandler(TextArea);
        }

        private void AppendCode(object sender, CodeWasAppended e)
        {
//            if (!Resources.Contains("content"))
//            {
//                Resources.Add("content", new OrderedDictionary<int, TypeAndContent>());
//            }
//
//            var content = (OrderedDictionary) Resources["content"];
//            var lines = e.ChatMessageModel.chatMessageBody.message.Split('\n').ZipWithIndex();
////            content.Add(new KeyValuePair<int, TypeAndContent>(1, new TypeAndContent(e.ChatMessageModel.chatMessageBody.message, e.ChatMessageModel.chatMessageBody.programminglanguage)));
        }

        private void AppendText(object sender, TextWasAppended e)
        {
            int a = 0;
        }

        protected override IVisualLineTransformer CreateColorizer(IHighlightingDefinition highlightingDefinition)
        {
            if (highlightingDefinition.IsNull())
                throw new ArgumentNullException("highlightingDefinition");
            return new MixedHighlightingColorizer(highlightingDefinition.MainRuleSet);
        }

        private class TypeAndContent
        {
            public int ProgrammingLanguage { get; private set; }
            public string Message { get; private set; }

            public TypeAndContent(string message, int programmingLanguage)
            {
                ProgrammingLanguage = programmingLanguage;
                Message = message;
            }
        }
    }

}