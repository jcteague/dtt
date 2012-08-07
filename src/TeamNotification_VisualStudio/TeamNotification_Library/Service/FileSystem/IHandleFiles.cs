using System.Collections.Generic;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.FileSystem
{
    public interface IHandleFiles
    {
        void Write(User user);
        
        User Read();
    }
}