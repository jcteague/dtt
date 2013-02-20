using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
    public class MixedTextEditor : TextEditor, IAppendCode
    {
        private IHandleMixedEditorEvents mixedEditorEvents;
        private IHandleChatEvents chatEvents;
        static DependencyProperty ContextMenu;
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

            this.TextChanged += UpdateContentResources;
            TextArea.ActiveInputHandler = new MixedEditorTextAreaInputHandler(TextArea);
        }

        private void UpdateContentResources(object sender, EventArgs e)
        {
            GetMergedContentResource(ContentResource, Text);
        }

        public IEnumerable<ChatMessageBody> GetTextEditorMessages()
        {
            var messages = new List<ChatMessageBody>();
            var content = GetMergedContentResource(ContentResource, Text);

            var newTextMessageContent = "";
            var limit = content.Values.Count;
            for (var i = 0; i < limit;)
            {
                while (i < limit && content.Values.ElementAt(i) is string)
                {
                    newTextMessageContent += content.Values.ElementAt(i); ++i;
                    if (i < limit && (content.Values.ElementAt(i) is string)) newTextMessageContent += "\n";
                }
                newTextMessageContent = newTextMessageContent.Trim();
                if(!newTextMessageContent.IsNullOrWhiteSpace())
                {
                    messages.Add(new ChatMessageBody {message=newTextMessageContent, source = "vs plugin"} );
                    newTextMessageContent = string.Empty;
                }
                var codestart = i;
                while (i < limit && content.Values.ElementAt(i) is MixedEditorLineData)
                {
                    newTextMessageContent += (content.Values.ElementAt(i) as MixedEditorLineData).Message; ++i;
                    if(i<limit && (content.Values.ElementAt(i) is MixedEditorLineData)) newTextMessageContent += "\n";
                }
                if (!string.IsNullOrEmpty(newTextMessageContent))
                {
                    messages.Add( new ChatMessageBody
                    {
                        message = newTextMessageContent,
                        programminglanguage = (content.Values.ElementAt(codestart) as MixedEditorLineData).ProgrammingLanguage,
                        solution = (content.Values.ElementAt(codestart) as MixedEditorLineData).Solution,
                        project = (content.Values.ElementAt(codestart) as MixedEditorLineData).Project,
                        document = (content.Values.ElementAt(codestart) as MixedEditorLineData).Document,
                        line = (content.Values.ElementAt(codestart) as MixedEditorLineData).Line,
                        column = (content.Values.ElementAt(codestart) as MixedEditorLineData).Column,
                        source = "vs plugin"
                    });
                    newTextMessageContent = string.Empty;
                }
            }
            return messages;
        }

        private SortedList<int, object> GetMergedContentResource(SortedList<int, object> contentResource, string text)
        {
            var result = CloneSortedList(contentResource);
            var textLines = text.Split('\n');
            if(result.Count > 0)
            {
                var counter = result.Last().Key - textLines.Length;
                while (counter > 0) { text += '\n'; --counter; }
            }

            text.Split('\n').ZipWithIndex().Each(x => {
                if (!x.Item1.IsNull())
                    result.AddOrUpdate(x.Item2 + 1, x.Item1);
            });
            result = ClearResult(result);
            return result;
        }

        private SortedList<int, object> ClearResult(SortedList<int, object> contentResource)
        {
            while (contentResource.Count > 0 && contentResource.Last().Value is MixedEditorLineData)
            {
                if ((contentResource.Last().Value as MixedEditorLineData).Message.IsNullOrWhiteSpace())
                {
                    ContentResource.Remove(contentResource.Last().Key);
                    contentResource.Remove(contentResource.Last().Key);
                }else break;
            }

            while (contentResource.Count > 0 && contentResource.First().Value is MixedEditorLineData)
            {
                if ((contentResource.First().Value as MixedEditorLineData).Message.IsNullOrWhiteSpace())
                {
                    ContentResource.Remove(contentResource.First().Key);
                    contentResource.Remove(contentResource.First().Key);
                }else break;
            }

            return contentResource;
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

        public void AppendCode(ChatMessageModel chatMessageModel)
        {
            var programmingLanguage = chatMessageModel.chatMessageBody.programminglanguage;

            if (!Resources.Contains("content"))
            {
                Resources.Add("content", new SortedList<int, object>());
            }
            var content = ContentResource;
            chatMessageModel.chatMessageBody.message.Split('\n').ZipWithIndex().Each(x =>
            {
                var position = LineCount + x.Item2;
                var valueAtPosition = new MixedEditorLineData(
                    x.Item1,
                    programmingLanguage,
                    chatMessageModel.chatMessageBody.solution,
                    chatMessageModel.chatMessageBody.project,
                    chatMessageModel.chatMessageBody.document,
                    x.Item2 + chatMessageModel.chatMessageBody.line,
                    chatMessageModel.chatMessageBody.column);
                if (content.ContainsKey(position))
                    content[position] = valueAtPosition;
                else
                    content.Add(position, valueAtPosition);
            });

            Text += chatMessageModel.chatMessageBody.message;
        }
        private void AppendCode(object sender, CodeWasAppended e)
        {
            AppendCode(e.ChatMessageModel);
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
        public string Message { get; set; }

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