using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Highlighters.Avalon;

namespace TeamNotification_Library.UI.Avalon
{
    public class MixedTextEditor : TextEditor
    {
        private IHandleMixedEditorEvents mixedEditorEvents;
        private IHandleChatEvents chatEvents;

        public MixedTextEditor() : this(Container.GetInstance<IHandleMixedEditorEvents>(), Container.GetInstance<IHandleChatEvents>())
        {
        }

        public MixedTextEditor(IHandleMixedEditorEvents mixedEditorEvents, IHandleChatEvents chatEvents) : base()
        {
            this.mixedEditorEvents = mixedEditorEvents;
            this.chatEvents = chatEvents;

            chatEvents.SendMessageRequested += ClearEditor;
            mixedEditorEvents.CodeWasAppended += AppendCode;
            mixedEditorEvents.TextWasAppended += AppendText;

            TextArea.ActiveInputHandler = new MixedEditorTextAreaInputHandler(TextArea);
        }

        public IEnumerable<ChatMessageBody> TextEditorMessages
        {
            get
            {
                var messages = new List<ChatMessageBody>();
                var content = GetMergedContentResource(ContentResource, Text);

                ChatMessageBody message;
                if (content.First().Value is string)
                {
                    message = new ChatMessageBody {message = string.Join("\n", content.Values)};
                }
                else
                {
                    var textMessage = string.Join("\n", content.Select(x => (x.Value as MixedEditorLineData).Message));
                    var representationalLine = content.First().Value as MixedEditorLineData;
                    message = new ChatMessageBody
                                  {
                                      message = textMessage,
                                      programminglanguage = representationalLine.ProgrammingLanguage,
                                      solution = representationalLine.Solution,
                                      project = representationalLine.Project,
                                      document = representationalLine.Document,
                                      line = representationalLine.Line,
                                      column = representationalLine.Column
                                  };
                }

                messages.Add(message);
                
                return messages;
            }
        }

        private SortedList<int, object> GetMergedContentResource(SortedList<int, object> contentResource, string text)
        {
            var result = CloneSortedList(contentResource);
            var r = Resources["content"];
            text.Split('\n').ZipWithIndex().Each(x => result.AddOrUpdate(x.Item2, x.Item1));
            return result;
        }

        private static SortedList<int, object> CloneSortedList(SortedList<int, object> contentResource)
        {
            if (contentResource.IsNull())
                return new SortedList<int, object>();

            var result = new SortedList<int, object>();
            contentResource.Each(x => result.Add(x.Key, x.Value));
            return result;
        }

        private void ClearEditor(object sender, SendMessageWasRequested e)
        {
            Text = "";
            Resources.Remove("content");
        }

        private void AppendCode(object sender, CodeWasAppended e)
        {
            var programmingLanguage = e.ChatMessageModel.chatMessageBody.programminglanguage;

            if (!Resources.Contains("content"))
            {
                Resources.Add("content", new SortedList<int, object>());
            }
            var content = ContentResource;
            e.ChatMessageModel.chatMessageBody.message.Split('\n').ZipWithIndex().Each(x =>
                                                                       {
                                                                           var position = LineCount + x.Item2;
                                                                           var valueAtPosition = new MixedEditorLineData(
                                                                               x.Item1, 
                                                                               programmingLanguage, 
                                                                               e.ChatMessageModel.chatMessageBody.solution,
                                                                               e.ChatMessageModel.chatMessageBody.project,
                                                                               e.ChatMessageModel.chatMessageBody.document,
                                                                               x.Item2 + e.ChatMessageModel.chatMessageBody.line, 
                                                                               e.ChatMessageModel.chatMessageBody.column);
                                                                           if (content.ContainsKey(position))
                                                                               content[position] = valueAtPosition;
                                                                           else
                                                                               content.Add(position, valueAtPosition);
                                                                       });

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
            var content = ContentResource;
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

        private SortedList<int, object> ContentResource
        {
            get { return (SortedList<int, object>) Resources["content"]; }
        }
    }

    public class MixedEditorLineData
    {
        public int ProgrammingLanguage { get; private set; }
        public string Project { get; private set; }
        public string Solution { get; private set; }
        public string Document { get; private set; }
        public int Line { get; private set; }
        public int Column { get; set; }
        public string Message { get; private set; }

        public MixedEditorLineData(string message, int programmingLanguage, string solution, string project, string document, int line, int column)
        {
            ProgrammingLanguage = programmingLanguage;
            Project = project;
            Solution = solution;
            Document = document;
            Line = line;
            Column = column;
            Message = message;
        }
    }

}