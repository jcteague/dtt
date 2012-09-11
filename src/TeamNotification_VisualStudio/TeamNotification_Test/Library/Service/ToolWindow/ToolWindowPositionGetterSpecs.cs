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
                pluginWindow = fake.an<IWrapWindow>();
                var otherWindow = fake.an<IWrapWindow>();
                var windows = new List<IWrapWindow> { pluginWindow, otherWindow };

                pluginWindow.Stub(x => x.IsPluginWindow()).Return(true);
                otherWindow.Stub(x => x.IsPluginWindow()).Return(false);

                dteStore.Stub(x => x.Windows).Return(windows);
            };

            protected static IWrapWindow pluginWindow;
        }

        public class when_getting_the_position_of_the_tool_window_and_the_plugin_window_is_floating : when_getting_the_position_of_the_tool_window
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.IsFloating).Return(true);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_a_dock_position_of_not_docked = () =>
                result.ShouldEqual(GlobalConstants.DockPositions.NotDocked);

            private static int result;
        }

        public abstract class when_getting_the_position_of_the_tool_window_and_the_plugin_window_is_not_floating : when_getting_the_position_of_the_tool_window
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

        public class when_getting_the_position_of_the_tool_window_and_the_plugin_window_Top_is_below_the_center_and_its_width_above_the_center : when_getting_the_position_of_the_tool_window_and_the_plugin_window_is_not_floating
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.Top).Return(30);
                pluginWindow.Stub(x => x.Width).Return(600);

                pluginWindow.Stub(x => x.Left).Return(600);
                pluginWindow.Stub(x => x.Height).Return(30);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_a_dock_position_of_Top = () =>
                result.ShouldEqual(GlobalConstants.DockPositions.Top);

            private static int result;
        }

        public class when_getting_the_position_of_the_tool_window_and_the_plugin_window_Top_is_above_the_center_and_its_width_above_the_center : when_getting_the_position_of_the_tool_window_and_the_plugin_window_is_not_floating
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.Top).Return(60);
                pluginWindow.Stub(x => x.Width).Return(600);

                pluginWindow.Stub(x => x.Left).Return(600);
                pluginWindow.Stub(x => x.Height).Return(30);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_a_dock_position_of_Bottom = () =>
                result.ShouldEqual(GlobalConstants.DockPositions.Bottom);

            private static int result;
        }

        public class when_getting_the_position_of_the_tool_window_and_the_plugin_window_Left_is_below_the_center_and_its_height_above_the_center : when_getting_the_position_of_the_tool_window_and_the_plugin_window_is_not_floating
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.Left).Return(50);
                pluginWindow.Stub(x => x.Height).Return(600);

                pluginWindow.Stub(x => x.Top).Return(600);
                pluginWindow.Stub(x => x.Width).Return(60);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_a_dock_position_of_Left = () =>
                result.ShouldEqual(GlobalConstants.DockPositions.Left);

            private static int result;
        }

        public class when_getting_the_position_of_the_tool_window_and_the_plugin_window_Left_is_above_the_center_and_its_height_above_the_center : when_getting_the_position_of_the_tool_window_and_the_plugin_window_is_not_floating
        {
            Establish context = () =>
            {
                pluginWindow.Stub(x => x.Left).Return(600);
                pluginWindow.Stub(x => x.Height).Return(60);

                pluginWindow.Stub(x => x.Top).Return(600);
                pluginWindow.Stub(x => x.Width).Return(60);
            };

            Because of = () =>
                result = sut.Get();

            It should_return_a_dock_position_of_Right = () =>
                result.ShouldEqual(GlobalConstants.DockPositions.Right);

            private static int result;
        }
    }
}
