﻿using System.Collections.Generic;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public interface IFormatPlainMessages
    {
        Block GetFormattedElement(ChatMessageModel chatMessage);
    }
}