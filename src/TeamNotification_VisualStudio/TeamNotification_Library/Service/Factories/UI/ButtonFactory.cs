using System.Windows.Controls;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class ButtonFactory : ICreateButtons
    {
        public Button Get()
        {
            return new Button();
        }
    }
}