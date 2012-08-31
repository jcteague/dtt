using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TeamNotification_Library.Service.Content
{
    public class ControlHighLightAnimator : IAnimateControl<TextElement>
    {
        protected ColorAnimation Animation;
        protected Color HighlightStartColor;
        protected Color HighlightEndColor;
        public ControlHighLightAnimator()
        {
            HighlightStartColor = Color.FromRgb(255, 200, 0);
            HighlightEndColor = Color.FromRgb(255, 255, 255);
            Animation = new ColorAnimation(HighlightStartColor,HighlightEndColor, TimeSpan.FromSeconds(0.5));
        }

        public void Animate(TextElement element)
        {
            //element.Background = new SolidColorBrush(HighlightColor);
            element.BeginAnimation(GetDependencyProperty(typeof(TextElement), "(Background).(SolidColorBrush.Color)"), Animation);
        }

        private DependencyProperty GetDependencyProperty(Type type, string name)
        {
            FieldInfo fieldInfo = type.GetField(name, BindingFlags.Public | BindingFlags.Static);
            return (fieldInfo != null) ? (DependencyProperty)fieldInfo.GetValue(null) : null;
        }
    }
}
