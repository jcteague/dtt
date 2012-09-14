 using Machine.Specifications;
 using TeamNotification_Library.Configuration;
 using TeamNotification_Library.Service.Async;
 using TeamNotification_Library.Service.Async.Models;
 using TeamNotification_Library.Service.ToolWindow;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Async
{  
//    [Subject(typeof(ToolWindowEvents))]  
//    public class ToolWindowEventsSpecs
//    {
//        public abstract class Concern : Observes<IHandleToolWindowEvents,
//                                            ToolWindowEvents>
//        {
//            Establish context = () =>
//            {
//                toolWindowPositionGetter = depends.on<IGetToolWindowPosition>();
//            };
//
//            protected static IGetToolWindowPosition toolWindowPositionGetter;
//        }
//
//        public class when_the_docking_is_changed : Concern
//        {
//            Establish context = () =>
//            {
//                x = 0;
//                y = 0;
//                w = 100;
//                h = 100;
//
//                eventArgs = new ToolWindowWasDocked
//                                {
//                                    isDocked = true,
//                                    x = x,
//                                    y = y,
//                                    w = w,
//                                    h = h
//                                };
//
//                toolWindowPositionGetter.Stub(positionGetter => positionGetter.GetPosition(x, y, w, h)).Return(GlobalConstants.DockPositions.Top);
//            };
//
//            Because of = () =>
//                sut.OnDockableChange(null, eventArgs);
//        
//            It should_update_set_the_correct_position_of_the_message_input_box = () =>
//            
//                
//            private static int x;
//            private static int y;
//            private static int w;
//            private static int h;
//            private static ToolWindowWasDocked eventArgs;
//        }
//    }
}
