using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace TeamNotification_Library.Service.Controls
{
    public interface IAnimateControl<T> where T : IAnimatable
    {
        void Animate(T element);
    }
}
