using System.Windows;

namespace TeamNotification_Library.Service.Factories.UI
{
    public interface ICreateUIElements<T> where T : UIElement
    {
        T Get(string name);
    }
}