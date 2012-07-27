using System.Windows.Controls;

namespace TeamNotification_Library.Service.Factories.UI
{
    public interface ICreateTextBox
    {
        TextBox Get(string text);
    }
}