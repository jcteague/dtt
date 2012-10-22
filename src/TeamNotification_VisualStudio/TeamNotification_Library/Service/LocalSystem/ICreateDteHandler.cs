using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Service.LocalSystem
{
    public interface ICreateDteHandler
    {
        IHandleDte Get(IStoreDTE dteStore);
    }
}
