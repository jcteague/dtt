using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class MessageInputAtBottom : IActOnChatElements
    {
        public void ExecuteOn(ChatUIElements chatUIElements)
        {
            chatUIElements.OuterGridRow3.Height = new GridLength(70);
            chatUIElements.MessageContainerBorder.SetValue(Grid.ColumnSpanProperty, 3);
            
            chatUIElements.MessageTextBoxGrid.SetValue(Grid.RowProperty, 2);
            chatUIElements.MessageTextBoxGrid.SetValue(Grid.ColumnProperty, 0);
            chatUIElements.MessageTextBoxGrid.SetValue(Grid.ColumnSpanProperty, 3);

            chatUIElements.MessageRow2.Height = new GridLength(0);
            chatUIElements.MessageColumn2.Width = new GridLength(100);
            
            chatUIElements.SendMessageButton.SetValue(Grid.RowProperty, 0);
            chatUIElements.SendMessageButton.SetValue(Grid.ColumnProperty, 1);

            chatUIElements.SendMessageButton.Margin = new Thickness(0, 0, 15, 18);

        }
    }
}