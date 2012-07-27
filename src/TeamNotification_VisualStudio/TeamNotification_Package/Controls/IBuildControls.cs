using System.Windows.Controls;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    public interface IBuildControls<TControl> where TControl : UserControl
    {
        TControl GetContentFrom(string uri);
    }
}