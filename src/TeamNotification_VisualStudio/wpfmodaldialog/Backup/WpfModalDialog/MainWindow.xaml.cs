using System.Threading;
using System.Windows;

namespace S2Snext.GUI.Dialogs
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private DialogWindowControls _controls = DialogWindowControls.OkCancel;

		public static readonly DependencyProperty AutoCloseProperty =
			DependencyProperty.Register("AutoClose", typeof (bool), typeof (MainWindow), new PropertyMetadata(default(bool)));

		public bool AutoClose
		{
			get { return (bool) GetValue(AutoCloseProperty); }
			set { SetValue(AutoCloseProperty, value); }
		}

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			ModalDialog.LockElement = ModalDialogParent;
			ModalMode.SelectedItem = _controls;
		}

		private void ShowModalDialog_Click(object sender, RoutedEventArgs e)
		{
			ModalDialog.LoadingFunction = () =>
			{
				Thread.Sleep(5000);
				return true;
			};
			ModalDialog.SucessMessage = "Success";
			ModalDialog.FailedMessage = "Failed";
			ModalDialog.AutoCloseLoader = AutoClose;
			var res = ModalDialog.Show(MessageTextBox.Text, _controls);
			var resultMessagePrefix = "Result: ";
			ResultText.Text = resultMessagePrefix + res.ToString();
		}

		private void ComboBox_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			
		}

		private void ModalMode_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			_controls = (DialogWindowControls)ModalMode.SelectedItem;
		}
	}
}
