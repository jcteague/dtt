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

        public MixedTextEditor() : this(Container.GetInstance<IHandleMixedEditorEvents>())
        {
        }

        public MixedTextEditor(IHandleMixedEditorEvents mixedEditorEvents) : base()
        {
            this.mixedEditorEvents = mixedEditorEvents;
            mixedEditorEvents.CodeWasAppended += AppendCode;
            mixedEditorEvents.TextWasAppended += AppendText;
            TextArea.ActiveInputHandler = new MixedEditorTextAreaInputHandler(TextArea);
        }

        private void AppendCode(object sender, CodeWasAppended e)
        {
            var programmingLanguage = e.ChatMessageModel.chatMessageBody.programminglanguage;
            UpdateContentResource(e, x => x.ChatMessageModel.chatMessageBody.message, x => new MixedEditorMessageContentAndProgrammingLanguage(x, programmingLanguage));

            Text += e.ChatMessageModel.chatMessageBody.message;
        }

        private void AppendText(object sender, TextWasAppended e)
        {
            UpdateContentResource(e, x => x.Text, x => x);
            Text += e.Text;
        }

        private void UpdateContentResource<T, R>(T value, Func<T, string> messageGetter, Func<string, R> valueAtPositionGetter)
        {
            if (!Resources.Contains("content"))
            {
                Resources.Add("content", new SortedList<int, object>());
            }
            var content = (SortedList<int, object>) Resources["content"];
            messageGetter(value).Split('\n').ZipWithIndex().Each(x =>
                                                                       {
                                                                           var position = LineCount + x.Item2;
                                                                           var valueAtPosition = valueAtPositionGetter(x.Item1);
                                                                           if (content.ContainsKey(position))
                                                                               content[position] = valueAtPosition;
                                                                           else
                                                                               content.Add(position, valueAtPosition);
                                                                       });
            Resources["content"] = content;
        }

        protected override IVisualLineTransformer CreateColorizer(IHighlightingDefinition highlightingDefinition)
        {
            if (highlightingDefinition.IsNull())
                throw new ArgumentNullException("highlightingDefinition");
            return new MixedHighlightingColorizer(highlightingDefinition.MainRuleSet, this);
        }
    }

    public class MixedEditorMessageContentAndProgrammingLanguage
    {
        public int ProgrammingLanguage { get; private set; }
        public string Message { get; private set; }

        public MixedEditorMessageContentAndProgrammingLanguage(string message, int programmingLanguage)
        {
            ProgrammingLanguage = programmingLanguage;
            Message = message;
        }
    }

}