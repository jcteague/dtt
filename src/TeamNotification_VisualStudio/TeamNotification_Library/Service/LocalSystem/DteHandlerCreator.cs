﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class DteHandlerCreator : ICreateDteHandler
    {
        public IHandleDte Get(IStoreDTE dteStore)
        {
            return new DteHandler(dteStore, Container.GetInstance<IFindVisualStudioItems>());
        }
    }
}
