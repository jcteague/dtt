using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Models;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace AvenidaSoftware.TeamNotification_Package
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        readonly ISendChatMessages messageSender;
        readonly IServiceChatRoomsControl chatRoomControlService;
        readonly IListenToMessages messageListener;
        readonly ISerializeJSON serializeJson;
        private string roomId { get; set; }
        private string currentChannel { get; set; }
        private List<string> subscribedChannels; 

        public Chat(ISendChatMessages messageSender, IListenToMessages messageListener, IRedisConnection connection, IServiceChatRoomsControl chatRoomControlService, ISerializeJSON serializeJson)
        {
            this.chatRoomControlService = chatRoomControlService;
            this.messageSender = messageSender;
            this.messageListener = messageListener;
            this.serializeJson = serializeJson;
            this.subscribedChannels = new List<string>();

            InitializeComponent();
            connection.Open();
            var collection = chatRoomControlService.GetCollection();
            var roomLinks = this.formatRooms(collection.rooms);

            Resources.Add("rooms", roomLinks);
            if(roomLinks.Count > 0)
                lstRooms.SelectedIndex = 0;
        }

        #region Events
        public void ChatMessageArrived(string channel, string payload)
        {
            if (channel == currentChannel)
            {
                var m = serializeJson.Deserialize<MessageData>(payload);
                var messageBody = serializeJson.Deserialize<MessageBody>(m.body);
                AppendMessage(m.name, messageBody.message);
            }
        }
        void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            this.SendMessage();
        }

        private void CheckKeyboard(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.SendMessage();
        }
        #endregion

        private void SendMessage()
        {
            var message = messageTextBox.Text;
            messageTextBox.Text = "";
            messageSender.SendMessage(message, this.roomId);
        }

        private void AppendMessage(string username, string message)
        {
            messageList.Dispatcher.Invoke((MethodInvoker)(() => messageList.Children.Add(new Label { Content = username + ": " + message, Margin = new Thickness(5.0,0.0,0.0,5.0)})));
            messageList.Dispatcher.Invoke((MethodInvoker)(() => scrollViewer1.ScrollToBottom() ));
        }

        private void ChangeRoom(string newRoomId)
        {
            currentChannel = "chat " + newRoomId;
            
            if (!subscribedChannels.Contains(currentChannel))
            {
                messageListener.ListenOnChannel(currentChannel, ChatMessageArrived);
                subscribedChannels.Add(currentChannel);
            }
            messageList.Dispatcher.Invoke((MethodInvoker) (() => messageList.Children.Clear()));
            AddMessages(newRoomId);
        }
        private void AddMessages(string currentRoomId)
        {
            this.roomId = currentRoomId;
            var collection = chatRoomControlService.GetMessagesCollection(this.roomId);
            foreach (var message in collection.messages)
            {
                var newMessage = Collection.getField(message.data, "body");
                var username = Collection.getField(message.data, "user");
                AppendMessage(username,newMessage);
            }
        }
        private List<Collection.Link> formatRooms(IEnumerable<Collection.Room> unformattedRoom)
        {
            return unformattedRoom.Select(room => new Collection.Link {name = Collection.getField(room.data, "name"), rel = Collection.getField(room.data,"id")}).ToList();
        }
        private void OnRoomSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var roomData = (Collection.Link) e.AddedItems[0];
            this.ChangeRoom(roomData.rel);
        }
    }
}