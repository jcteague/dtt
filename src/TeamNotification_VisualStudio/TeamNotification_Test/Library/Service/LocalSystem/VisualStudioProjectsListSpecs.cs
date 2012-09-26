 using System.Collections;
 using System.Collections.Generic;
 using EnvDTE;
 using EnvDTE80;
 using Machine.Specifications;
 using TeamNotification_Library.Service.LocalSystem;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.LocalSystem
{  
    [Subject(typeof(VisualStudioProjectsList))]  
    public class VisualStudioProjectsListSpecs
    {
        public abstract class Concern : Observes<IListVisualStudioProjects,
                                            VisualStudioProjectsList>
        {
            Establish context = () =>
            {
                dteStore = depends.on<IStoreDTE>();
            };

            protected static IStoreDTE dteStore;
        }

   
        public class when_getting_all_the_projects : Concern
        {
            Establish context = () =>
            {
                IWrapSolution solution = fake.an<IWrapSolution>();
                dteStore.Stub(x => x.Solution).Return(solution);

                var project1 = fake.an<IWrapProject>();
                var project2 = fake.an<IWrapProject>();

                concreteProject1 = fake.an<Project>();
                project1.Stub(x => x.Value).Return(concreteProject1);

                concreteProject2 = fake.an<Project>();
                project2.Stub(x => x.Value).Return(concreteProject2);

                concreteProject1.Stub(x => x.Kind).Return(ProjectKinds.vsProjectKindSolutionFolder);
                concreteProject2.Stub(x => x.Kind).Return("");

                ProjectItems project1Items = fake.an<ProjectItems>();
                concreteProject1.Stub(x => x.ProjectItems).Return(project1Items);

                ProjectItem project1Item1 = fake.an<ProjectItem>();
                IEnumerator project1ItemsEnumerator = new List<ProjectItem>{project1Item1}.GetEnumerator();
                project1Items.Stub(x => x.GetEnumerator()).Return(project1ItemsEnumerator);

                project1SubProject = fake.an<Project>();
                project1Item1.Stub(x => x.SubProject).Return(project1SubProject);

                project1SubProject.Stub(x => x.Kind).Return("");

                var projects = new List<IWrapProject> {project1, project2}.ToArray();
                solution.Stub(x => x.Projects).Return(projects);
            };

            Because of = () =>
                result = sut.GetAllProjects();

            It should_return_all_the_projects_in_the_solution = () =>
                result.ShouldContain(project1SubProject, concreteProject2);
            
                
            private static IEnumerable<Project> result;
            private static Project concreteProject1;
            private static Project concreteProject2;
            private static Project project1SubProject;
        }
    }
}
