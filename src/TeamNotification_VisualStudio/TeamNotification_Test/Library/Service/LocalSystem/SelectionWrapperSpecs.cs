using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Service.LocalSystem;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Service.LocalSystem
{
    [Subject(typeof(SelectionWrapper))]
    public class SelectionWrapperSpecs
    {
        public abstract class Concern : Observes<IWrapSelection, SelectionWrapper>
        {
            private Establish context = () =>
            {
                TestDocFullName = "testdir\\testdoc.cs";
                TestDirName = "testdir\\";

                TextDocumentMock = fake.an<IWrapTextDocument>();
                DocumentMock = fake.an<Document>();
                EditPointMock = fake.an<EditPoint>();
                EndPointMock = fake.an<TextPoint>();
                TextSelectionMock = fake.an<TextSelection>();

                TextDocumentMock.Stub(x => x.EndPoint).Return(EndPointMock);
                TextDocumentMock.Stub(x => x.CreateEditPoint()).Return(EditPointMock);
                
                DocumentMock.Stub(x => x.Object("TextDocument")).Return(TextDocumentMock);
            };
            protected static string TestDocFullName;
            protected static string TestDirName;
            protected static string TestDocName;
            protected static IWrapTextDocument TextDocumentMock;
            protected static TextSelection TextSelectionMock;
            protected static IWrapDocument DocumentWrapper;
            protected static EditPoint EditPointMock;
            protected static TextPoint EndPointMock;
            protected static IHandleDte DteHandler;
            protected static Document DocumentMock;
        }
        public class when_selected_text : Concern
        {
            private Establish context = () =>
            {
                Line = 1;
                selectionText = "Some awesomely selected text\nAnd the other line my fellow gentleman ;D\n";
                TextDocumentMock = depends.on<IWrapTextDocument>();
                TextSelectionMock.Stub(x => x.MoveTo(Line, 1));
                TextSelectionMock.Stub(x => x.Text).Return(selectionText);
                TextDocumentMock.Stub(x => x.Selection).Return(TextSelectionMock);
            };
            Because of = () =>
            {
                sut.Select(1, 10);
                resultText = sut.Text;
                resultLines = sut.Lines;
            };

            It should_have_some_text_selected = () =>
                resultText.ShouldEqual(selectionText);

            It should_have_some_lines = () =>
                resultLines.ShouldBeGreaterThan(0);

            private static string resultText;
            private static string selectionText;
            private static int Line;
            private static int resultLines;
        }

        public class and_copying_it : when_selected_text
        {
            private Because of = () =>
                sut.Copy();
            private It should_call_the_textselection_copy_method = () =>
                TextSelectionMock.AssertWasCalled(x => x.Copy());
        }
        public class and_cuting_it : when_selected_text
        {
            private Because of = () =>
                sut.Cut();
            private It should_call_the_textselection_copy_method = () =>
                TextSelectionMock.AssertWasCalled(x => x.Cut());
        }
    }
}
