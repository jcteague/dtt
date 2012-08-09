using System.Windows;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Http;
using WindowsClipboard = System.Windows.Clipboard;

using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Clipboard
{
    public class ClipboardDataStorageService : IStoreClipboardData
    {
        private readonly ISerializeJSON serializer;
        private ClipboardHasChanged _data;

        public ClipboardDataStorageService(ISerializeJSON serializer)
        {
            this.serializer = serializer;
            CopyFlag = false;
        }
        
        public ClipboardHasChanged Data 
        { 
            get { return _data; } 
        }

//        public ClipboardHasChanged ToSend { get; set; }
        private bool CopyFlag { get; set; }
        public void Store(ClipboardHasChanged clipboardArgs)
        {
            if (CopyFlag)
            {
                CopyFlag = false;
                return;
            }
            _data = clipboardArgs;

            //var dataObject = WindowsClipboard.GetText();
            //if (dataObject.IsNotNull()) return;

//            var args = serializer.Deserialize<ClipboardHasChanged>(dataObject);
//            if (args.Text != clipboardArgs.Text)
            var data = serializer.Serialize(clipboardArgs);
            WindowsClipboard.SetData(DataFormats.Text, data);
            CopyFlag = true;

        }
    }
}