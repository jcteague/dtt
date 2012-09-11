using System.Windows.Controls;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class MessageInputAtRight : IActOnChatElements
    {
        public void ExecuteOn(ChatUIElements chatUIElements)
        {
            chatUIElements.MessageContainerBorder.SetValue(Grid.ColumnSpanProperty, 2);

            chatUIElements.MessageTextBoxBorder.SetValue(Grid.RowProperty, 1);
            chatUIElements.MessageTextBoxBorder.SetValue(Grid.ColumnProperty, 2);

            chatUIElements.MessageInput.Height = double.NaN;
        }
    }
}