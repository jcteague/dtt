using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;

namespace TeamNotification_Library.Service.Renderer
{
    public class ContentRenderer : IRenderContent
    {
        private readonly ICreateInstances<StackPanel> panelFactory;

        public ContentRenderer(ICreateInstances<StackPanel> panelFactory)
        {
            this.panelFactory = panelFactory;
        }

        public StackPanel Render(Collection collection)
        {
            var panel = panelFactory.GetInstance();
//            panel.Children.Add(templateRenderer.RenderFor(collection));



            return panel;
        }

//        private Panel BuildContentFrom(Collection collection)
//        {
//            // Create a StackPanel and set its properties
//            
//
//            //            // Create Ellipses and add to StackPanel
//            //            Ellipse redCircle = new Ellipse();
//            //            redCircle.Width = 100;
//            //            redCircle.Height = 100;
//            //            redCircle.Fill = new SolidColorBrush(Colors.Red);
//            //            panel.Children.Add(redCircle);
//            //
//            //            Ellipse yellowCircle = new Ellipse();
//            //            yellowCircle.Width = 60;
//            //            yellowCircle.Height = 60;
//            //            yellowCircle.Fill = new SolidColorBrush(Colors.Yellow);
//            //            panel.Children.Add(yellowCircle);
//            //
//            //            Ellipse greenCircle = new Ellipse();
//            //            greenCircle.Width = 40;
//            //            greenCircle.Height = 40;
//            //            greenCircle.Fill = new SolidColorBrush(Colors.Green);
//            //            panel.Children.Add(greenCircle);
//            //
//            //            Ellipse blueCircle = new Ellipse();
//            //            blueCircle.Width = 20;
//            //            blueCircle.Height = 20;
//            //            blueCircle.Fill = new SolidColorBrush(Colors.Blue);
//            //            panel.Children.Add(blueCircle);
//
////            return panel;
//        }
    }
}