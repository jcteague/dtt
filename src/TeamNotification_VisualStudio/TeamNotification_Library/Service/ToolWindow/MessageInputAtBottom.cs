using System.Windows.Controls;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class MessageInputAtBottom : IActOnChatElements
    {
        public void ExecuteOn(ChatUIElements chatUIElements)
        {
            chatUIElements.MessageInput.SetValue(Grid.RowProperty, 1);
            chatUIElements.MessageInput.SetValue(Grid.ColumnProperty, 2);
        }
    }
}