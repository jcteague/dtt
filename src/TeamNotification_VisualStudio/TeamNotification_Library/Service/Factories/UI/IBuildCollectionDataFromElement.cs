using System.Windows;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories.UI
{
    public interface IBuildCollectionDataFromElement<T> where T : UIElement
    {
        CollectionData Get(DependencyObject container, CollectionData item);
    }
}