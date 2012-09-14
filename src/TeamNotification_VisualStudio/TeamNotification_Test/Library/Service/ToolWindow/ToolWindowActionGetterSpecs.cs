 using Machine.Specifications;
 using TeamNotification_Library.Configuration;
 using TeamNotification_Library.Service.Async.Models;
 using TeamNotification_Library.Service.ToolWindow;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.ToolWindow
{  
    [Subject(typeof(ToolWindowActionGetter))]  
    public class ToolWindowActionGetterSpecs
    {
        public abstract class Concern : Observes<IGetToolWindowAction,
                                            ToolWindowActionGetter>
        {
            Establish context = () =>
            {
                ToolWindowOrientationGetter = depends.on<IGetToolWindowOrientation>();
                                        
            };

            protected static IGetToolWindowOrientation ToolWindowOrientationGetter;
        }

        public abstract class when_gettting_the_action : Concern
        {
            Establish context = () =>
            {
                var x = 9;
                var y = 10;
                var w = 11;
                var h = 12;

                toolWindowDockedArgs = new ToolWindowWasDocked
                                           {
                                               x = x,
                                               y = y,
                                               w = w,
                                               h = h,
                                               isDocked = false
                                           };
            };

            protected static ToolWindowWasDocked toolWindowDockedArgs;
        }

   
        public class when_gettting_the_action_and_the_orientation_is_input_at_right : when_gettting_the_action
        {
            Establish context = () =>
            {
                var t = toolWindowDockedArgs;
                ToolWindowOrientationGetter.Stub(getter => getter.Get()).Return(GlobalConstants.DockOrientations.InputAtRight);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_the_action_for_that_position = () =>
                result.ShouldBeAn<MessageInputAtRight>();
                
            private static IActOnChatElements result;
        }

        public class when_gettting_the_action_and_the_orientation_is_input_at_bottom : when_gettting_the_action
        {
            Establish context = () =>
            {
                var t = toolWindowDockedArgs;
                ToolWindowOrientationGetter.Stub(getter => getter.Get()).Return(GlobalConstants.DockOrientations.InputAtBottom);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_the_action_for_that_position = () =>
                result.ShouldBeAn<MessageInputAtBottom>();

            private static IActOnChatElements result;
        }
    }
}
