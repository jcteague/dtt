﻿using System.Windows.Controls;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Renderer
{
    public interface IRenderContent
    {
        StackPanel Render(Collection collection);
    }
}