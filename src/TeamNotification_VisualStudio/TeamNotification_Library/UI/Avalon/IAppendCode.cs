using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.UI.Avalon
{
    public interface IAppendCode
    {
        void AppendCode(ChatMessageModel messages);
    }
}
