using System;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;

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
        }
        
        public void Store<T>(T clipboardArgs) where T : ChatMessageData
        {
            IsCode = typeof (T) == typeof(CodeClipboardData);
            var data = serializer.Serialize(clipboardArgs);
            systemClipboardHandler.SetText(data);
        }

        public T Get<T>() where T : ChatMessageData, new()
        {
            var text = systemClipboardHandler.GetText();
            T value;
            try
            {
                value = serializer.Deserialize<T>(text);
            }
            catch (Exception)
            {
                value = new T {message = text};
            }
            return value;
        }
    }
}