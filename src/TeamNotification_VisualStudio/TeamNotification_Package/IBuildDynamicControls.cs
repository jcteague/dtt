using System.Windows.Controls;

namespace AvenidaSoftware.TeamNotification_Package
{
    public interface IBuildDynamicControls
    {
        Panel GetContentFrom(string href);
    }
}