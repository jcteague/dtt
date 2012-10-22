using System;
using System.Windows;
using System.Windows.Controls;

namespace TeamNotification_Library.Service.Controls
{
    public class FormControlHelpers : IHelpControls
    {
        public T Find<T>(DependencyObject logicalTreeNode, string name) where T : class
        {
            return Find(logicalTreeNode, name) as T;
        }

        public object Find(DependencyObject logicalTreeNode, string name)
        {
            return LogicalTreeHelper.FindLogicalNode(logicalTreeNode, name);
        }
    }
}