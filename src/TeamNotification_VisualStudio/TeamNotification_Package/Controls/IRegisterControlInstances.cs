using System;
using System.Collections.Generic;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    public interface IRegisterControlInstances
    {
        T GetInstance<T>();
    }

    public class ControlsRegistry : IRegisterControlInstances
    {
        public T GetInstance<T>()
        {
            throw new NotImplementedException();
        }
    }
}