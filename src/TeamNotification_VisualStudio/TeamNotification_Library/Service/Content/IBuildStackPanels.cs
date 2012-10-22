using System.Windows.Controls;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Content
{
    public interface IBuildStackPanels
    {
        StackPanel GetFor(CollectionData collectionData);
    }
}