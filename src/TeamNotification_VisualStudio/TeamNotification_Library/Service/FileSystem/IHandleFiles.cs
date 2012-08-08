using System.Collections.Generic;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.FileSystem
{
    public interface IHandleFiles
    {
        void Write(LoginResponse response);
        
        LoginResponse Read();
    }
}