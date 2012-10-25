using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    public class CustomMessageBoxResult<T>
    {
        public string Label { get; set; }
        public T Value { get; set; }
    }

    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public static class CustomMessageBoxValues
        {
            public const int Ok = 1;
            public const int Cancel = 2;
        }

        public object LastResult;

        public static CustomMessageBoxResult<string> Show(String caption)
        {
            var cmb = new CustomMessageBox {tbxMessageCaption = {Text = caption}, Visibility = Visibility.Visible};
            cmb.gButtons.Children.Add(new Button { Content = "Ok", IsDefault = true, IsCancel = true, Width = 75});
            cmb.ShowDialog();
            if (cmb.DialogResult.HasValue && cmb.DialogResult.Value)
                return new CustomMessageBoxResult<string> { Value="Ok"};
            return new CustomMessageBoxResult<string> { Value = "cancel" };
        }

        public static void ShowOkCancel(String caption, Action okAction, Action cancelAction)
        {
            var cmb = new CustomMessageBox {tbxMessageCaption = {Text = caption}, Visibility = Visibility.Visible};
            cmb.gButtons.Children.Add(GetButtonFor("Ok", okAction, cmb));
            cmb.gButtons.Children.Add(GetButtonFor("Cancel", cancelAction, cmb));

            cmb.ShowDialog();
        }

        private static Button GetButtonFor(string content, Action action, CustomMessageBox cmb)
        {
            var button = new Button {Content = content, IsDefault = false, IsCancel = false, Width = 75};
            button.Click += (s, e) =>
                                {
                                    action();
                                    cmb.Close();
                                };
            return button;
        }

        public static CustomMessageBoxResult<T> Show<T>(String caption, CustomMessageBoxResult<T>[] customMessageBoxResults)
        {
            var cmb = new CustomMessageBox();
            var buttonsCount = customMessageBoxResults.Length;
            var buttonsWidth = 75;

            for (var i = 0; i < buttonsCount; ++i)
            {
                var result = customMessageBoxResults[i];
                var b = new Button { Content = result.Label, IsDefault = (i == 0), CommandParameter = result.Value, Width = buttonsWidth, Margin=new Thickness(10,0,0,0)};
                b.Click += cmb.HandleClick<T>;
                cmb.gButtons.Children.Add(b);
            }
            cmb.tbxMessageCaption.Text = caption;
            cmb.Visibility = Visibility.Visible;
            cmb.ShowDialog();
            if (cmb.DialogResult.HasValue && cmb.DialogResult.Value)
                return (CustomMessageBoxResult<T>)cmb.LastResult;
            return new CustomMessageBoxResult<T> { Value = default(T)};
        }

        public void HandleClick<T>(object sender, EventArgs args)
        {
            DialogResult = true;
            LastResult = new CustomMessageBoxResult<T>{Value=(T)((Button)sender).CommandParameter};
        }

        public CustomMessageBox()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }
    }
}
