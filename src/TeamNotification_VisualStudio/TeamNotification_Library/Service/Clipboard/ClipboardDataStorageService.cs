using System;
using System.Windows;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using WindowsClipboard = System.Windows.Clipboard;

using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Clipboard
{
    public class ClipboardDataStorageService : IStoreClipboardData
    {
        public bool IsCode { get; private set; }
        
        private readonly ISerializeJSON serializer;
        private IHandleSystemClipboard systemClipboardHandler;

        public ClipboardDataStorageService(ISerializeJSON serializer, IHandleSystemClipboard systemClipboardHandler)
        {
            this.serializer = serializer;
            this.systemClipboardHandler = systemClipboardHandler;
            CopyFlag = false;
        }
        
        private bool CopyFlag { get; set; }
        
        public void Store<T>(T clipboardArgs) where T : ChatMessageData
        {
//            if (CopyFlag)
//            {
//                CopyFlag = false;
//                return;
//            }

            IsCode = typeof (T) == typeof(CodeClipboardData);
            var data = serializer.Serialize(clipboardArgs);
            systemClipboardHandler.SetText(data);
//            CopyFlag = true;
        }

        public T Get<T>() where T : ChatMessageData
        {
            return serializer.Deserialize<T>(systemClipboardHandler.GetText());
        }
    }
}