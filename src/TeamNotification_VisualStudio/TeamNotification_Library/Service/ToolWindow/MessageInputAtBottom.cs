using System.Windows.Controls;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class MessageInputAtBottom : IActOnChatElements
    {
        public void ExecuteOn(ChatUIElements chatUIElements)
        {
            chatUIElements.MessageTextBoxBorder.SetValue(Grid.RowProperty, 2);
            chatUIElements.MessageTextBoxBorder.SetValue(Grid.ColumnProperty, 0);
            chatUIElements.MessageContainerBorder.SetValue(Grid.ColumnSpanProperty, 3);
            chatUIElements.MessageInput.Height = 65;
        }
    }
}