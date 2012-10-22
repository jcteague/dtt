using System.Windows.Controls;

namespace TeamNotification_Library.Service.Factories.UI
{
    public interface ICreateLabels
    {
        Label Get(string label);
    }
}