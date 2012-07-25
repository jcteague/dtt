using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AvenidaSoftware.TeamNotification_Package
{
    public class DynamicControlBuilder : IDynamicControlBuilder
    {
        public Panel GetContentFrom()
        {
            // Create a StackPanel and set its properties
            StackPanel dynamicStackPanel = new StackPanel();
            dynamicStackPanel.Width = 300;
            dynamicStackPanel.Height = 200;
            dynamicStackPanel.Background = new SolidColorBrush(Colors.LightBlue);
            dynamicStackPanel.Orientation = Orientation.Horizontal;

            // Create Ellipses and add to StackPanel
            Ellipse redCircle = new Ellipse();
            redCircle.Width = 100;
            redCircle.Height = 100;
            redCircle.Fill = new SolidColorBrush(Colors.Red);
            dynamicStackPanel.Children.Add(redCircle);

            /*
            Ellipse orangeCircle = new Ellipse();
            orangeCircle.Width = stackpanel-in-wpf;
            orangeCircle.Height = stackpanel-in-wpf;
            orangeCircle.Fill = new SolidColorBrush(Colors.Orange);
            dynamicStackPanel.Children.Add(orangeCircle);
                */

            Ellipse yellowCircle = new Ellipse();
            yellowCircle.Width = 60;
            yellowCircle.Height = 60;
            yellowCircle.Fill = new SolidColorBrush(Colors.Yellow);
            dynamicStackPanel.Children.Add(yellowCircle);

            Ellipse greenCircle = new Ellipse();
            greenCircle.Width = 40;
            greenCircle.Height = 40;
            greenCircle.Fill = new SolidColorBrush(Colors.Green);
            dynamicStackPanel.Children.Add(greenCircle);

            Ellipse blueCircle = new Ellipse();
            blueCircle.Width = 20;
            blueCircle.Height = 20;
            blueCircle.Fill = new SolidColorBrush(Colors.Blue);
            dynamicStackPanel.Children.Add(blueCircle);

            return dynamicStackPanel;
        }
    }
}