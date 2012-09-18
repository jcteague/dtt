using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class MessageInputAtRight : IActOnChatElements
    {
        public void ExecuteOn(ChatUIElements chatUIElements)
        {
            chatUIElements.OuterGridRowDefinition3.Height = new GridLength(0);
            chatUIElements.MessageContainerBorder.SetValue(Grid.ColumnSpanProperty, 2);
            
            chatUIElements.MessageTextBoxGrid.SetValue(Grid.RowProperty, 1);
            chatUIElements.MessageTextBoxGrid.SetValue(Grid.ColumnProperty, 2);
            chatUIElements.MessageTextBoxGrid.SetValue(Grid.ColumnSpanProperty, 1);

            chatUIElements.MessageGridRowDefinition2.Height = new GridLength(34);
            chatUIElements.MessageGridColumnDefinition2.Width = new GridLength(0);
            
            chatUIElements.SendMessageButton.SetValue(Grid.RowProperty, 1);
            chatUIElements.SendMessageButton.SetValue(Grid.ColumnProperty, 0);

            chatUIElements.SendMessageButton.Margin = new Thickness(0, 0, 0, 0);

            chatUIElements.MessageTextBoxGridSplitter.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Center);
            chatUIElements.MessageTextBoxGridSplitter.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        }
    }
}