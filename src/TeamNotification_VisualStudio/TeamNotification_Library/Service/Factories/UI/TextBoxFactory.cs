using System.Windows.Controls;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class TextBoxFactory : ICreateUIElements<TextBox>
    {
        public TextBox Get(string name)
        {
            return new TextBox { Name = name, Width = 100 };
        }
    }
}