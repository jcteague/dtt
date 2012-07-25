using System.Windows.Controls;

namespace AvenidaSoftware.TeamNotification_Package
{
    public interface IDynamicControlBuilder
    {
        Panel GetContentFrom();
    }
}