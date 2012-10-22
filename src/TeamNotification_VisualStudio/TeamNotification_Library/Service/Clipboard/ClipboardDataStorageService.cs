using System;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Clipboard
{
    public class ClipboardDataStorageService : IStoreClipboardData
    {
        private readonly ISerializeJSON serializer;
        private IHandleSystemClipboard systemClipboardHandler;

        public ClipboardDataStorageService(ISerializeJSON serializer, IHandleSystemClipboard systemClipboardHandler)
        {
            this.serializer = serializer;
            this.systemClipboardHandler = systemClipboardHandler;
        }

        public void Store<T>(T clipboardArgs) where T : ChatMessageModel
        {
            var data = serializer.Serialize(clipboardArgs);
            systemClipboardHandler.SetText(data);
        }

        public T Get<T>() where T : ChatMessageModel, new()
        {
            var text = systemClipboardHandler.GetText();
            if (text.IsNullOrWhiteSpace())
                return new T { body = serializer.Serialize(new ChatMessageBody { message = text } )};

            T value;
            try
            {
                value = serializer.Deserialize<T>(text);
            }
            catch (Exception)
            {
                value = new T { chatMessageBody = new ChatMessageBody { message = text } };
            }
            return value;
        }
    }
}