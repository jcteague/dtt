using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    public class CustomMessageBoxResult
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBoxResult LastResult { get; private set; }

        public static CustomMessageBoxResult Show(String caption)
        {
            var cmb = new CustomMessageBox {tbxMessageCaption = {Text = caption}, Visibility = Visibility.Visible};
            cmb.gButtons.Children.Add(new Button { Content = "Ok", IsDefault = true, IsCancel = true, Width = 75});
            cmb.ShowDialog();
            if (cmb.DialogResult.HasValue && cmb.DialogResult.Value)
                return new CustomMessageBoxResult { Value="Ok"};
            return new CustomMessageBoxResult { Value = "cancel" };
        }

        public static CustomMessageBoxResult Show(String caption, CustomMessageBoxResult[] customMessageBoxResults)
        {
            var cmb = new CustomMessageBox();
            var buttonsCount = customMessageBoxResults.Length;
            var buttonsWidth = 75;

            for (var i = 0; i < buttonsCount; ++i)
            {
                var result = customMessageBoxResults[i];
                var b = new Button { Content = result.Label, IsDefault = (i == 0), CommandParameter = result.Value, Width = buttonsWidth, Margin=new Thickness(10,0,0,0)};
                b.Click += cmb.HandleClick;
                cmb.gButtons.Children.Add(b);
            }
            cmb.tbxMessageCaption.Text = caption;
            cmb.Visibility = Visibility.Visible;
            cmb.ShowDialog();
            if (cmb.DialogResult.HasValue && cmb.DialogResult.Value)
                return cmb.LastResult;
            return new CustomMessageBoxResult { Value = "cancel" };
        }

        public void HandleClick(object sender, EventArgs args)
        {
            DialogResult = true;
            LastResult = new CustomMessageBoxResult{Value=(string)((Button)sender).CommandParameter};
        }

        public CustomMessageBox()
        {
            InitializeComponent();
        }
    }
}
