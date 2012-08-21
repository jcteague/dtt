using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Machine.Specifications;
using TeamNotification_Library.Service.LocalSystem;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.LocalSystem
{
    [Subject(typeof(DteHandler))]
    public class DteHandlerSpecs
    {
        public class Concern : Observes<IHandleDte, DteHandler>
        {
            Establish context = () =>
            {
                dteHandler = depends.on<IHandleDte>();
                currSolution = depends.on<Solution>();
            };
            
            protected static string _fileName;
            protected static Solution currSolution;
            protected static IHandleDte dteHandler;
        }

        public abstract class when_opening_a_solution_file : Concern
        {
            private Establish context = () =>
            {
                _solutionName = "foobar.sln";
                _line = 1;
                currSolution.Stub(x => x.FileName).Return(_solutionName);
                currSolution.Stub(x => x.FullName).Return(_solutionName);
            };
            protected static int _line;
            protected static string _solutionName;
        }

        public class and_the_file_is_valid : when_opening_a_solution_file
        {

            Because of = () =>
                _ep = sut.GetEditPoint(_projectName, _fileName, _line);

            It should_return_a_new_EditPoint_instance = () =>
                _ep.GetType().ShouldBe(typeof(EditPoint));

            private static EditPoint _ep;
            private static string _projectName;
        }
    }
}
