using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Renderer;

namespace AvenidaSoftware.TeamNotification_Package
{
    public class DynamicControlBuilder : IBuildDynamicControls
    {
        private readonly ISendHttpRequests httpClient;
        private ICreateCallback callbackFactory;
        private ISerializeJSON serializer;

        public DynamicControlBuilder(ISendHttpRequests httpClient, ICreateCallback callbackFactory, ISerializeJSON serializer)
        {
            this.callbackFactory = callbackFactory;
            this.httpClient = httpClient;
            this.serializer = serializer;
        }

        public Panel GetContentFrom(string href)
        {
            var collection = httpClient.Get<string, Collection>(href, Callback).Result;
            return BuildContent();
        }

        private Collection Callback(string response)
        {
            return serializer.Deserialize<Collection>(response);
        }

        private Panel BuildContent()
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
        }
    }
}