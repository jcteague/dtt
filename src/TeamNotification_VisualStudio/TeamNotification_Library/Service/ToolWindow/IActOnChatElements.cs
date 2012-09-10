using System.Windows.Controls;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.ToolWindow
{
    public interface IActOnChatElements
    {
        void ExecuteOn(ChatUIElements chatUIElements);
    }

    public class MessageInputAtBottom : IActOnChatElements
    {
        public void ExecuteOn(ChatUIElements chatUIElements)
        {
            chatUIElements.MessageInput.SetValue(Grid.RowProperty, 1);
            chatUIElements.MessageInput.SetValue(Grid.ColumnProperty, 2);
        }
    }

    public class MessageInputAtRight : IActOnChatElements
    {
        public void ExecuteOn(ChatUIElements chatUIElements)
        {
            return;
            throw new System.NotImplementedException();
        }
    }
}