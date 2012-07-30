using System.Windows.Controls;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class TextBoxFactory : ICreateTextBox
    {
         public TextBox Get(string text)
         {
             return new TextBox
                        {
                            Text = text
                        };
         }
    }
}