using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories
{
    public class CallbackFactory : ICreateCallback
    {
//        public Action<Collection> BuildFor(StackPanel panel)
//        {
//            return BuildContentOn(panel);
//        }
        public Func<Collection, StackPanel> BuildFor(StackPanel panel)
        {
            return BuildContentOn(panel);
        }

//        public Func<Collection, StackPanel> Build(Task<string> responseTask)
        public Func<Collection, StackPanel> Build()
        {
            return BuildContent();
        }

        private Func<Collection, StackPanel> BuildContent()
        {
            return (collection) =>
            {
                // Create a StackPanel and set its properties
                StackPanel panel = new StackPanel();
                panel.Width = 300;
                panel.Height = 200;
                panel.Background = new SolidColorBrush(Colors.LightBlue);
                panel.Orientation = Orientation.Horizontal;

                // Create Ellipses and add to StackPanel
                Ellipse redCircle = new Ellipse();
                redCircle.Width = 100;
                redCircle.Height = 100;
                redCircle.Fill = new SolidColorBrush(Colors.Red);
                panel.Children.Add(redCircle);

                Ellipse yellowCircle = new Ellipse();
                yellowCircle.Width = 60;
                yellowCircle.Height = 60;
                yellowCircle.Fill = new SolidColorBrush(Colors.Yellow);
                panel.Children.Add(yellowCircle);

                Ellipse greenCircle = new Ellipse();
                greenCircle.Width = 40;
                greenCircle.Height = 40;
                greenCircle.Fill = new SolidColorBrush(Colors.Green);
                panel.Children.Add(greenCircle);

                Ellipse blueCircle = new Ellipse();
                blueCircle.Width = 20;
                blueCircle.Height = 20;
                blueCircle.Fill = new SolidColorBrush(Colors.Blue);
                panel.Children.Add(blueCircle);

                return panel;
            };
        }

//        private Action<Collection> BuildContentOn(StackPanel panel)
        private Func<Collection, StackPanel> BuildContentOn(StackPanel panel)
        {
            return (collection) =>
                {
                    // Create a StackPanel and set its properties
                    StackPanel dynamicStackPanel = new StackPanel();
                    panel.Width = 300;
                    panel.Height = 200;
                    panel.Background = new SolidColorBrush(Colors.LightBlue);
                    panel.Orientation = Orientation.Horizontal;

                    // Create Ellipses and add to StackPanel
                    Ellipse redCircle = new Ellipse();
                    redCircle.Width = 100;
                    redCircle.Height = 100;
                    redCircle.Fill = new SolidColorBrush(Colors.Red);
                    panel.Children.Add(redCircle);

                    Ellipse yellowCircle = new Ellipse();
                    yellowCircle.Width = 60;
                    yellowCircle.Height = 60;
                    yellowCircle.Fill = new SolidColorBrush(Colors.Yellow);
                    panel.Children.Add(yellowCircle);

                    Ellipse greenCircle = new Ellipse();
                    greenCircle.Width = 40;
                    greenCircle.Height = 40;
                    greenCircle.Fill = new SolidColorBrush(Colors.Green);
                    panel.Children.Add(greenCircle);

                    Ellipse blueCircle = new Ellipse();
                    blueCircle.Width = 20;
                    blueCircle.Height = 20;
                    blueCircle.Fill = new SolidColorBrush(Colors.Blue);
                    panel.Children.Add(blueCircle);

                    return panel;
                };
        }
    }
}