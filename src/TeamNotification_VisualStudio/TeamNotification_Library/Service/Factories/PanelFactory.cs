using System.Windows.Controls;
using System.Windows.Media;

namespace TeamNotification_Library.Service.Factories
{
    public class PanelFactory : ICreateInstances<StackPanel>
    {
        public StackPanel GetInstance()
        {
            var panel = new StackPanel
                            {
                                Width = 300,
                                Height = 200,
                                Background = new SolidColorBrush(Colors.LightBlue),
                                Orientation = Orientation.Horizontal
                            };

            return panel;
        }
    }
}