using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Service.Controls
{
    public interface IShowCode
    {
        string Show(string code, int programmingLanguageIdentifier);
    }
}
