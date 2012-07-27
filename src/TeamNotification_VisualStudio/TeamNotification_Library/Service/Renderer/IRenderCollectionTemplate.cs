using System.Windows.Controls;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Renderer
{
    public interface IRenderCollectionTemplate
    {
        StackPanel RenderFor(Collection collection);
    }
}