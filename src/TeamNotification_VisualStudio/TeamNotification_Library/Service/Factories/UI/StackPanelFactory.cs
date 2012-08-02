using System.Windows.Controls;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class StackPanelFactory : ICreateUIElements<StackPanel>
    {
        public StackPanel Get(string name)
        {
            return new StackPanel {Name = name};
        }
    }
}