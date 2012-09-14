 using System.Collections.Generic;
 using Machine.Specifications;
 using TeamNotification_Library.Configuration;
 using TeamNotification_Library.Service.LocalSystem;
 using TeamNotification_Library.Service.ToolWindow;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.ToolWindow
{  
    [Subject(typeof(ToolWindowOrientationGetter))]  
    public class ToolWindowOrientationGetterSpecs
    {
        public abstract class Concern : Observes<IGetToolWindowOrientation,
                                            ToolWindowOrientationGetter>
        {
            Establish context = () =>
            {
                dteStore = depends.on<IStoreDTE>();
            };

            protected static IStoreDTE dteStore;
        }

   
        public abstract class when_getting_the_orientation_for_the_tool_window : Concern
        {
            Establish context = () =>
            {
                pluginWindow = fake.an<IWrapWindow>();
                var otherWindow = fake.an<IWrapWindow>();
                var windows = new List<IWrapWindow> { pluginWindow, otherWindow };

                pluginWindow.Stub(x => x.IsPluginWindow()).Return(true);
                otherWindow.Stub(x => x.IsPluginWindow()).Return(false);

                dteStore.Stub(x => x.Windows).Return(windows);
            };

            protected static IWrapWindow pluginWindow;
        }

        public class when_getting_the_orientation_of_the_tool_window_and_the_plugin_window_is_floating : when_getting_the_orientation_for_the_tool_window
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.IsFloating).Return(true);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_a_dock_orientation_of_input_at_right = () =>
                result.ShouldEqual(GlobalConstants.DockOrientations.InputAtRight);

            private static int result;
        }

        public abstract class when_getting_the_orientation_of_the_tool_window_and_the_plugin_window_is_not_floating : when_getting_the_orientation_for_the_tool_window
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.IsFloating).Return(false);

                mainWindow = fake.an<IWrapWindow>();
                dteStore.Stub(x => x.MainWindow).Return(mainWindow);

                mainWindow.Stub(x => x.Height).Return(100);
                mainWindow.Stub(x => x.Width).Return(1000);
            };

            protected static IWrapWindow mainWindow;
        }

        public class when_getting_the_orientation_of_the_tool_window_and_the_plugin_window_height_percent_is_greater_than_the_plugin_window_width_percent_and_it_is_not_above_the_60_percent_of_the_main_window_height : when_getting_the_orientation_of_the_tool_window_and_the_plugin_window_is_not_floating
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.Height).Return(70);
                pluginWindow.Stub(x => x.Width).Return(400);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_a_dock_position_of_Top = () =>
                result.ShouldEqual(GlobalConstants.DockOrientations.InputAtBottom);

            private static int result;
        }

        public class when_getting_the_orientation_of_the_tool_window_and_the_plugin_window_height_percent_is_not_greater_than_the_plugin_window_width_percent : when_getting_the_orientation_of_the_tool_window_and_the_plugin_window_is_not_floating
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.Height).Return(70);
                pluginWindow.Stub(x => x.Width).Return(800);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_a_dock_orientation_of_input_at_right = () =>
                result.ShouldEqual(GlobalConstants.DockOrientations.InputAtRight);

            private static int result;
        }

        public class when_getting_the_orientation_of_the_tool_window_and_the_plugin_window_height_percent_is_not_greater_than_60_percent_height_of_the_main_window_height : when_getting_the_orientation_of_the_tool_window_and_the_plugin_window_is_not_floating
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.Height).Return(40);
                pluginWindow.Stub(x => x.Width).Return(400);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_a_dock_orientation_of_input_at_right = () =>
                result.ShouldEqual(GlobalConstants.DockOrientations.InputAtRight);

            private static int result;
        }
    }
}
