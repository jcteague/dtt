using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using BookSleeve;
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
        readonly ISendChatMessages send_chat_messages;

        public MyControl(ISendChatMessages send_chat_messages)
        {
            InitializeComponent();

            this.send_chat_messages = send_chat_messages;
            SubscribeToRedis("chat1");
        }

        void SubscribeToRedis(string channel)
        {
            var conn = new RedisConnection("10.0.0.32");
            conn.Open();
            var sub = conn.GetOpenSubscriberChannel();
            sub.Subscribe(channel, ChatMessageArrived);
        }

        public void ChatMessageArrived(string arg1, byte[] arg2)
        {
            var message = new UTF8Encoding().GetString(arg2).Split(new []{'-'});
            var username = message[0];
            var payload = message[1];
            messageList.Dispatcher.Invoke((MethodInvoker)(()=>messageList.Children.Add(new Label {Content = username + " : " + payload})));
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
            send_chat_messages.SendMessage(message);
        }
    }
}