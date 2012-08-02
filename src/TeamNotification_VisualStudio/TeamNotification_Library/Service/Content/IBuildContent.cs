using System.Collections.Generic;
using System.Windows.Controls;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Content
{
    public interface IBuildContent
    {
        IEnumerable<StackPanel> GetContentFor(Collection collection);
    }
}