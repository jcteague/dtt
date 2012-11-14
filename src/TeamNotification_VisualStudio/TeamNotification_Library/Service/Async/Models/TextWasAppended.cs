namespace TeamNotification_Library.Service.Async.Models
{
    public class TextWasAppended
    {
        public string Text { get; private set; }

        public TextWasAppended(string text)
        {
            Text = text;
        }
    }
}