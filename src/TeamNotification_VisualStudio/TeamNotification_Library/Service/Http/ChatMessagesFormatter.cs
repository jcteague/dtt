using System.Collections.Generic;
using System.Windows.Documents;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Http
{
//    public class ChatMessagesFormatter : IFormatChatMessages
//    {
//        private List<object> buffer = new List<object>();
//
//        public void AddMessage(Block block)
//        {
//            if (block.GetType() == typeof(BlockUIContainer))
//            {
//                var resources = block.Resources;
//                buffer.Add(new CodeClipboardData
//                               {
//                                   message = resources["message"].Cast<string>(),
//                                   solution = resources["solution"].Cast<string>(),
//                                   document = resources["document"].Cast<string>(),
//                                   line = resources["line"].Cast<int>()
//                               });
//            }
//            else
//            {
//                buffer.Add(new PlainClipboardData { message = ((Paragraph)block).GetText() });
//            }
//        }
//    }
}