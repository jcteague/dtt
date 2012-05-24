using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Forms;
using TeamNotification_Library.Service.Http;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace AvenidaSoftware.TeamNotification_Package
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class MyControl : UserControl
    {
        readonly ISendChatMessages message_sender;

        public MyControl(ISendChatMessages message_sender,IListenToMessages message_listener,IRedisConnection connection)
        {
            InitializeComponent();
            connection.Open();


            this.message_sender = message_sender;
            message_listener.ListenOnChannel("chat1",ChatMessageArrived);
        }

        public void ChatMessageArrived(string channel, string payload)
        {
            var splitted_array = payload.Split(new[] {'-'});
            var userName = splitted_array[0];
            var userMessage = splitted_array[1];
            messageList.Dispatcher.Invoke((MethodInvoker)(()=>messageList.Children.Add(new Label {Content = userName + " : " + userMessage})));
        }


        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format(CultureInfo.CurrentUICulture, "We are inside {0}.button1_Click()", ToString()),
                            "Team Notification");
        }

        void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var message = messageTextBox.Text;
            messageTextBox.Text = "";
            message_sender.SendMessage(message);
        }
    }
}