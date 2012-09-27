using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Machine.Specifications;
using TeamNotification_Library.Functional;
using TeamNotification_Library.Service.LocalSystem;
using TeamNotification_Test.Stubs;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;
using DocumentWrapper = TeamNotification_Library.Service.LocalSystem.DocumentWrapper;

namespace TeamNotification_Test.Library.Service.LocalSystem
{
    [Subject(typeof(DteHandler))]
    public class DteHandlerSpecs
    {
        public abstract class Concern : Observes<IHandleDte, DteHandler>
        {
            Establish context = () =>
            {
                ValidProjectName = "ImAsValidASPossible";
                ValidDocumentName = "ThisIsIndeedAValidDocument";

                ProjectItem = fake.an<IWrapProjectItem>();
                solution = fake.an<IWrapSolution>();
                Projects = new IWrapProject[2] { fake.an<IWrapProject>(), fake.an<IWrapProject>() };

                dteStore = depends.on<IStoreDTE>();
                dteStore.Stub(x => x.Solution).Return(solution);

                visualStudioItemsFinder = depends.on<IFindVisualStudioItems>();

                solution.Stub(x => x.Projects).Return(Projects);
            };

            protected static IWrapSolution solution;
            protected static IWrapProjectItem ProjectItem;
            protected static IWrapProject[] Projects;
            protected static IStoreDTE dteStore;
            protected static IFindVisualStudioItems visualStudioItemsFinder;
            protected static string ValidProjectName;
            protected static string ValidDocumentName;
        }

        public abstract class when_opening_a_solution_file : Concern
        {
            private Establish context = () =>
            {
                SolutionName = "foobar.sln";
            };

            protected static string SolutionName;
        }

        public class and_the_file_is_invalid : when_opening_a_solution_file
        {
            Because of = () =>
                _doc = sut.OpenFile("", "");

            It should_return_an_invalid_document_instance = () =>
                _doc.ShouldBeNull();

            private static IWrapDocument _doc;
        }

        public class and_the_file_is_valid : when_opening_a_solution_file
        {
            Establish context = () =>
            {
                var projectItem = new StubProjectItem();
                document = new Just<ProjectItem>(projectItem);
                visualStudioItemsFinder.Stub(x => x.FindDocument(ValidProjectName, ValidDocumentName)).Return(document);
            };

            Because of = () =>
                Result = sut.OpenFile(ValidProjectName, ValidDocumentName);

            private It should_return_a_valid_document_instance = () =>
            {
                Result.ShouldNotBeNull();
                Result.ShouldBeOfType<DocumentWrapper>();
            };

            private static IWrapDocument Result;
            private static Maybe<ProjectItem> document;
        }

        public class when_asking_for_the_editpoint : Concern
        {
            Establish context = () =>
            {
                Line = 1;
                fakeTextDoc = fake.an<IWrapTextDocument>();
                fakeDoc = fake.an<IWrapDocument>();
                fakeEditPoint = fake.an<EditPoint>();
                ProjectItem.Stub(x => x.Document).Return(fakeDoc);
                fakeTextDoc.Stub(x => x.CreateEditPoint()).Return(fakeEditPoint);
                fakeDoc.Stub(x => x.TextDocument).Return(fakeTextDoc);
            };
            Because of = () =>
                ep = sut.GetEditPoint(fakeDoc, Line);

            It should_return_a_valid_edit_point = () =>
                ep.ShouldEqual(fakeEditPoint);
            
            private static EditPoint ep;
            private static EditPoint fakeEditPoint;
            private static int Line;
            private static IWrapDocument fakeDoc;
            private static IWrapTextDocument fakeTextDoc;
        }

        public abstract class when_pasting_some_code : Concern
        {
            Establish Context = () =>
            {
                Code = "This is quite a wonderfulish message indeed.";
                EditPoint = fake.an<EditPoint>(); 
            };

            protected static string Code;
            protected static EditPoint EditPoint;
            protected static PasteOptions PasteOption;
        }

        public class and_appending_it : when_pasting_some_code
        {
            Establish Context = () =>
            {
                PasteOption = PasteOptions.Append;
            };
            Because of = () =>
                sut.PasteCode(EditPoint, Code, PasteOption);

            It should_append_the_code_to_the_end_of_the_document = () =>
            {
                EditPoint.AssertWasCalled(x => x.EndOfDocument());
                EditPoint.AssertWasCalled(x => x.Insert(Code));
            };
        }

        public class and_overwrite_it : when_pasting_some_code
        {
            Establish Context = () =>
            {
                PasteOption = PasteOptions.Overwrite;
            };
            Because of = () =>
                sut.PasteCode(EditPoint, Code, PasteOption);

            It should_delete_the_overlapped_code_and_insert_the_new_one_to_the_document = () =>
            {
                EditPoint.AssertWasCalled(x => x.Delete(Code.Length));
                EditPoint.AssertWasCalled(x => x.Insert(Code));
            };
        }
        public class and_insert_it : when_pasting_some_code
        {
            Establish Context = () =>
            {
                PasteOption = PasteOptions.Insert;
            };
            Because of = () =>
                sut.PasteCode(EditPoint, Code, PasteOption);

            It should_insert_the_code_in_the_current_position_to_the_document = () => 
                EditPoint.AssertWasCalled(x => x.Insert(Code));
        }
    }
}
