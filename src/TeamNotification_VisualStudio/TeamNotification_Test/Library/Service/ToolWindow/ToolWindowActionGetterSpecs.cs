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
                toolWindowPositionGetter = depends.on<IGetToolWindowPosition>();
                                        
            };

            protected static IGetToolWindowPosition toolWindowPositionGetter;
        }

        public abstract class when_gettting_the_action_for_a_position : Concern
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

   
        public class when_gettting_the_action_for_a_position_and_the_position_is_top : when_gettting_the_action_for_a_position
        {
            Establish context = () =>
            {
                var t = toolWindowDockedArgs;
                toolWindowPositionGetter.Stub(getter => getter.Get()).Return(GlobalConstants.DockPositions.Top);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_the_action_for_that_position = () =>
                result.ShouldBeAn<MessageInputAtRight>();
                
            private static IActOnChatElements result;
        }

        public class when_gettting_the_action_for_a_position_and_the_position_is_bottom : when_gettting_the_action_for_a_position
        {
            Establish context = () =>
            {
                var t = toolWindowDockedArgs;
                toolWindowPositionGetter.Stub(getter => getter.Get()).Return(GlobalConstants.DockPositions.Bottom);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_the_action_for_that_position = () =>
                result.ShouldBeAn<MessageInputAtRight>();

            private static IActOnChatElements result;
        }

        public class when_gettting_the_action_for_a_position_and_the_position_is_undocked : when_gettting_the_action_for_a_position
        {
            Establish context = () =>
            {
                var t = toolWindowDockedArgs;
                toolWindowPositionGetter.Stub(getter => getter.Get()).Return(GlobalConstants.DockPositions.NotDocked);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_the_action_for_that_position = () =>
                result.ShouldBeAn<MessageInputAtRight>();

            private static IActOnChatElements result;
        }

        public class when_gettting_the_action_for_a_position_and_the_position_is_left : when_gettting_the_action_for_a_position
        {
            Establish context = () =>
            {
                var t = toolWindowDockedArgs;
                toolWindowPositionGetter.Stub(getter => getter.Get()).Return(GlobalConstants.DockPositions.Left);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_the_action_for_that_position = () =>
                result.ShouldBeAn<MessageInputAtBottom>();

            private static IActOnChatElements result;
        }

        public class when_gettting_the_action_for_a_position_and_the_position_is_right : when_gettting_the_action_for_a_position
        {
            Establish context = () =>
            {
                var t = toolWindowDockedArgs;
                toolWindowPositionGetter.Stub(getter => getter.Get()).Return(GlobalConstants.DockPositions.Right);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_the_action_for_that_position = () =>
                result.ShouldBeAn<MessageInputAtBottom>();

            private static IActOnChatElements result;
        }
    }
}
