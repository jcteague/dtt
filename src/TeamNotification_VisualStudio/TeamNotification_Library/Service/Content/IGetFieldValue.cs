using System.Windows;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Content
{
    public interface IGetFieldValue
    {
        CollectionData GetValue(CollectionData item, DependencyObject container);
    }
}