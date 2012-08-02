using System.Windows.Controls;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class LabelFactory : ICreateLabels
    {
        public Label Get(string label)
        {
            return new Label { Content = label, Width = 75 };
        }
    }
}