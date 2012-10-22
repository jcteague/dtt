using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class MessageInputAtBottom : IActOnChatElements
    {
        public void ExecuteOn(ChatUIElements chatUIElements)
        {
            chatUIElements.OuterGridRowDefinition3.Height = new GridLength(70);
            chatUIElements.MessageContainerBorder.SetValue(Grid.ColumnSpanProperty, 3);
            
            chatUIElements.MessageTextBoxGrid.SetValue(Grid.RowProperty, 3);
            chatUIElements.MessageTextBoxGrid.SetValue(Grid.ColumnProperty, 0);
            chatUIElements.MessageTextBoxGrid.SetValue(Grid.ColumnSpanProperty, 3);

            chatUIElements.MessageGridRowDefinition2.Height = new GridLength(0);
            chatUIElements.MessageGridColumnDefinition2.Width = new GridLength(100);
            
            chatUIElements.SendMessageButton.SetValue(Grid.RowProperty, 0);
            chatUIElements.SendMessageButton.SetValue(Grid.ColumnProperty, 1);

            chatUIElements.SendMessageButton.Margin = new Thickness(0, 0, 0, 0);

            chatUIElements.MessageTextBoxGridSplitter.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            chatUIElements.MessageTextBoxGridSplitter.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);

        }
    }
}