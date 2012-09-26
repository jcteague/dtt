 using System.Collections;
 using System.Collections.Generic;
 using System.Windows;
 using EnvDTE;
 using Machine.Specifications;
 using TeamNotification_Library.Functional;
 using TeamNotification_Library.Service.LocalSystem;
 using TeamNotification_Test.Stubs;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using Rhino.Mocks;
 using TeamNotification_Library.Extensions;

namespace TeamNotification_Test.Library.Service.LocalSystem
{  
    [Subject(typeof(VisualStudioItemsFinder))]  
    public class VisualStudioItemsFinderSpecs
    {
        public abstract class Concern : Observes<IFindVisualStudioItems,
                                            VisualStudioItemsFinder>
        {
            Establish context = () =>
            {
                visualStudioProjectsList = depends.on<IListVisualStudioProjects>();
            };

            protected static IListVisualStudioProjects visualStudioProjectsList;
        }

        public abstract class when_finding_a_document : Concern
        {
            Establish context = () =>
            {
                projectName = "blah_project_name";

                var project1 = fake.an<Project>();
                var uniqueName1 = "project1\\anything";
                project1.Stub(x => x.UniqueName).Return(uniqueName1);

                project2 = fake.an<Project>();
                var uniqueName2 = "any_base_path\\{0}".FormatUsing(projectName);
                project2.Stub(x => x.UniqueName).Return(uniqueName2);

                var project3 = fake.an<Project>();
                var uniqueName3 = "project3\\foo";
                project3.Stub(x => x.UniqueName).Return(uniqueName3);

                var projects = new List<Project> { project1, project2, project3 };
                visualStudioProjectsList.Stub(x => x.GetAllProjects()).Return(projects);
            };

            protected static Project project2;
            protected static string projectName;
        }
   
        public class when_finding_a_document_and_there_is_a_project_item_matching_the_file_name : when_finding_a_document
        {
            Establish context = () =>
            {
                fileName = "blah_file_name";

                ProjectItem project2Item1 = fake.an<ProjectItem>();
                project2Item1.Stub(x => x.Name).Return("not_important_file_name");

                ProjectItem project2Item2 = fake.an<ProjectItem>();
                project2Item2.Stub(x => x.Name).Return(fileName);

                ProjectItems project2Items = fake.an<ProjectItems>();
                project2.Stub(x => x.ProjectItems).Return(project2Items);
                
                IEnumerator project2ItemsEnumerator = new List<ProjectItem>{project2Item1, project2Item2}.GetEnumerator();
                project2Items.Stub(x => x.GetEnumerator()).Return(project2ItemsEnumerator);
                
                expectedResult = project2Item2;
            };

            Because of = () =>
                result = sut.FindDocument(projectName, fileName);

            It should_return_the_document_in_the_project_that_matches_the_name = () =>
                result.Value.ShouldEqual(expectedResult);

            private static string fileName;
            private static Maybe<ProjectItem> result;
            private static ProjectItem expectedResult;
        }

        public class when_finding_a_document_and_there_is_not_a_project_item_matching_the_file_name : when_finding_a_document
        {
            Establish context = () =>
            {
                nonMatchingFileName = "blah_file_name_that_does_not_match";

                ProjectItem project2Item1 = fake.an<ProjectItem>();
                project2Item1.Stub(x => x.Name).Return("not_important_file_name");

                ProjectItem project2Item2 = fake.an<ProjectItem>();
                project2Item2.Stub(x => x.Name).Return("another_non_matching_file_name");

                ProjectItems project2Items = fake.an<ProjectItems>();
                project2.Stub(x => x.ProjectItems).Return(project2Items);

                IEnumerator project2ItemsEnumerator = new List<ProjectItem> { project2Item1, project2Item2 }.GetEnumerator();
                project2Items.Stub(x => x.GetEnumerator()).Return(project2ItemsEnumerator);
            };

            Because of = () =>
                result = sut.FindDocument(projectName, nonMatchingFileName);

            It should_return_nothing = () =>
                result.IsEmpty.ShouldBeTrue();

            private static string nonMatchingFileName;
            private static Maybe<ProjectItem> result;
        }
    }
}
