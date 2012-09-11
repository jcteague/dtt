 using Machine.Specifications;
 using TeamNotification_Library.Configuration;
 using TeamNotification_Library.Service.LocalSystem;
 using TeamNotification_Library.Service.ToolWindow;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;

namespace TeamNotification_Test.Library.Service.ToolWindow
{  
    [Subject(typeof(ToolWindowPositionGetter))]  
    public class ToolWindowPositionGetterSpecs
    {
        public abstract class Concern : Observes<IGetToolWindowPosition,
                                            ToolWindowPositionGetter>
        {
            Establish context = () =>
            {
                dteStore = depends.on<IStoreDTE>();
            };

            protected static IStoreDTE dteStore;
        }

   
        public abstract class when_getting_the_position_of_the_tool_window : Concern
        {
            Establish context = () =>
            {

            };

            protected static int x;
            protected static int y;
            protected static int w;
            protected static int h;
        }

        public class when_getting_the_position_of_the_tool_window_and_the_x_coordinate_of : when_getting_the_position_of_the_tool_window
        {
            Establish context = () =>
            {
                x = 0;
                y = 0;
                w = 100;
                y = 100;
            };

            Because of = () =>
                result = sut.Get();

            It should_return_LEFT = () =>
                result.ShouldEqual(GlobalConstants.DockPositions.Left);

            private static int result;
        }
    }
}
