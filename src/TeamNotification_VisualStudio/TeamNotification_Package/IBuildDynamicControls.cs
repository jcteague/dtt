using System.Windows.Controls;

namespace AvenidaSoftware.TeamNotification_Package
{
    public interface IBuildDynamicControls
    {
        StackPanel GetContentFrom(string href);
    }
}