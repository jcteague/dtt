using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TeamNotification_Library.Service.Controls;

namespace S2Snext.GUI.Dialogs
{
	/// <summary>
	/// Interaction logic for ModalDialog.xaml
	/// </summary>
    public partial class ModalDialog : IShowCode
	{
		private BackgroundWorker _worker = new BackgroundWorker();
		private bool _hideRequest;
		private DialogWindowResult _result;

		public ModalDialog()
		{
			InitializeComponent();
			Visibility = Visibility.Hidden;
		}

		public Panel LockElement { get; set; }
		public bool AutoCloseLoader { get; set; }


		#region Message

		// Using a DependencyProperty as the backing store for Message.
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MessageProperty =
			DependencyProperty.Register(
				"Message", typeof(string), typeof(ModalDialog), new UIPropertyMetadata(string.Empty));

		public string Message
		{
			get { return (string)GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		#endregion

		#region LoadingFunction

		public static readonly DependencyProperty LoadingFunctionProperty =
			DependencyProperty.Register("LoadingFunction", typeof(Func<bool>), typeof(ModalDialog),
										new PropertyMetadata(default(Func<bool>)));

		public Func<bool> LoadingFunction
		{
			get { return (Func<bool>)GetValue(LoadingFunctionProperty); }
			set { SetValue(LoadingFunctionProperty, value); }
		}

		#endregion

		#region FailedMessage

		public static readonly DependencyProperty FailedMessageProperty =
			DependencyProperty.Register("FailedMessage", typeof(string), typeof(ModalDialog),
										new PropertyMetadata(default(string)));

		public string FailedMessage
		{
			get { return (string)GetValue(FailedMessageProperty); }
			set { SetValue(FailedMessageProperty, value); }
		}

		#endregion

		#region SuccessMessage

		public static readonly DependencyProperty SucessMessageProperty =
			DependencyProperty.Register("SucessMessage", typeof(string), typeof(ModalDialog),
										new PropertyMetadata(default(string)));

		public string SucessMessage
		{
			get { return (string)GetValue(SucessMessageProperty); }
			set { SetValue(SucessMessageProperty, value); }
		}

		#endregion

        public string Show(string code)
        {
            Message = "";
            Visibility = Visibility.Visible;
            ClearControlsContainer();
            //SetDialogControls(DialogWindowControls.Prompt);
            var fta = new FlyingTextArea(code);
            fta.btnFinish.Click += HideHandlerDialog;
            ControlsContainer.Children.Add(fta);
            EnableControls();

            LockChildren();
            LockElement.IsEnabled = false;

            _hideRequest = false;
            while (!_hideRequest)
            {
                // HACK: Stop the thread if the application is about to close
                if (Dispatcher.HasShutdownStarted || Dispatcher.HasShutdownFinished)
                    break;
                
                // HACK: Simulate "DoEvents"
                Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    new ThreadStart(delegate { }));
                Thread.Sleep(20);
            }

            return fta.tbxInsertedText.Text;
        }
	    public DialogWindowResult Show(string message, DialogWindowControls controls = DialogWindowControls.Ok)
		{
			Message = message;
			Visibility = Visibility.Visible;
			SetDialogControls(controls);
			LockChildren();
			LockElement.IsEnabled = false;

			_hideRequest = false;
			while (!_hideRequest)
			{
				// HACK: Stop the thread if the application is about to close
				if (Dispatcher.HasShutdownStarted ||
					Dispatcher.HasShutdownFinished)
				{
					break;
				}

				// HACK: Simulate "DoEvents"
				Dispatcher.Invoke(
					DispatcherPriority.Background,
					new ThreadStart(delegate { }));
				Thread.Sleep(20);
			}

			return _result;
		}

		private void LockChildren()
		{
			LockElement.IsEnabled = false;
			//foreach (UIElement child in LockElement.Children)
			//{
			//	if (child != this)
			//		child.IsEnabled = false;
			//}
		}

		private void UnlockChilren()
		{
			LockElement.IsEnabled = true;
			foreach (UIElement child in LockElement.Children)
			{
				child.IsEnabled = true;
			}
		}

		private void EnableControls()
		{
			foreach (UIElement child in ControlsContainer.Children)
			{
				child.IsEnabled = true;
			}
		}

		private void SetDialogControls(DialogWindowControls controls)
		{
			ClearControlsContainer();
			switch (controls)
			{
				case (DialogWindowControls.LoadIndicator):
					SetLoadingIndicator();
					break;
				case (DialogWindowControls.Ok):
					SetOkButton();
					break;
				case (DialogWindowControls.OkCancel):
					SetOkCancelButtons();
					break;
				case (DialogWindowControls.YesNo):
					SetYesNoButtons();
					break;
                case (DialogWindowControls.Prompt):
			        SetPromt("");
                    break;
			}
			EnableControls();
		}

	    private void SetPromt(string text)
	    {
            ControlsContainer.Children.Add((new FlyingTextArea(text)));
	    }

	    private void SetYesNoButtons()
		{
			ControlsContainer.Children.Add(CreateButton("Yes", YesButtonClick));
			ControlsContainer.Children.Add(CreateButton("No", NoButtonClick));

		}

		private void SetOkCancelButtons()
		{
			ControlsContainer.Children.Add(CreateButton("Ok", OkButtonClick));
			ControlsContainer.Children.Add(CreateButton("Cancel", CancelButtonClick));
		}

		private void ClearControlsContainer()
		{
			ControlsContainer.Children.Clear();
			loader.Visibility = Visibility.Collapsed;
		}

		private void SetOkButton()
		{
			ControlsContainer.Children.Add(CreateButton("Ok", OkButtonClick));
		}

		private Button CreateButton(string label, RoutedEventHandler handler)
		{
			var okButton = new Button { Content = label, Margin = new Thickness(4), IsEnabled = true, Width = 75 };
			okButton.Click += handler;
			return okButton;
		}


		private void SetLoadingIndicator()
		{
			loader.Visibility = Visibility.Visible;
			if (LoadingFunction == null)
				LoadingFunction = () => false;

			_worker.DoWork += WorkerDoWork;
			_worker.RunWorkerCompleted += WorkerCompleted;
			_worker.RunWorkerAsync();
		}

		void WorkerDoWork(object sender, DoWorkEventArgs e)
		{
			var res = (Func<bool>)Dispatcher.Invoke(new Func<Func<bool>>(() => LoadingFunction));
			e.Result = res();
		}

		void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			_result = ((bool)e.Result)
						? DialogWindowResult.Success
						: DialogWindowResult.Failed;
			if (_result == DialogWindowResult.Success)
			{
				Message = SucessMessage;
			}
			else
			{
				Message = FailedMessage;
			}

			if (!AutoCloseLoader)
			{
				ClearControlsContainer();
				ControlsContainer.Children.Add(CreateButton("Ok", (sender1, args1) => HideHandlerDialog()));
			}
			else
				HideHandlerDialog();
		}
        
		private void HideHandlerDialog(object sender, EventArgs e)
		{
            HideHandlerDialog();
		}

	    private void HideHandlerDialog()
		{
			_hideRequest = true;
			Visibility = Visibility.Hidden;
			UnlockChilren();
		}

		private void OkButtonClick(object sender, RoutedEventArgs e)
		{
			_result = DialogWindowResult.Ok;
			HideHandlerDialog();
		}

		private void CancelButtonClick(object sender, RoutedEventArgs e)
		{
			_result = DialogWindowResult.Cancel;
			HideHandlerDialog();
		}

		private void YesButtonClick(object sender, RoutedEventArgs e)
		{
			_result = DialogWindowResult.Yes;
			HideHandlerDialog();
		}

		private void NoButtonClick(object sender, RoutedEventArgs e)
		{
			_result = DialogWindowResult.No;
			HideHandlerDialog();
		}

	}

    
    public enum DialogWindowResult
	{
		Success,
		Failed,
		Ok,
		Cancel,
		Yes,
		No
	}

	public enum DialogWindowControls
	{
		LoadIndicator,
		OkCancel,
		YesNo,
		Ok,
        Prompt
	}
}