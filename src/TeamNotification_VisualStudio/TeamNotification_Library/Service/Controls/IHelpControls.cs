using System;
using System.Windows;

namespace TeamNotification_Library.Service.Controls
{
    public interface IHelpControls
    {
        T Find<T>(DependencyObject logicalTreeNode, string name) where T : class;
        
        object Find(DependencyObject logicalTreeNode, string name);
    }
}