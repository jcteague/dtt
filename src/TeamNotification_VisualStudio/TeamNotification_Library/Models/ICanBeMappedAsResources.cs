using System.Windows;

namespace TeamNotification_Library.Models
{
    public interface ICanBeMappedAsResources
    {
        ResourceDictionary AsResources();
    }
}