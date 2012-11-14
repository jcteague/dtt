using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Async.Models
{
    public class DataWasPasted
    {
        public string Text { get; private set; }
        public object ExtraData { get; private set; }

        public DataWasPasted(string text, object extraData)
        {
            Text = text;
            ExtraData = extraData;
        }
    }
}