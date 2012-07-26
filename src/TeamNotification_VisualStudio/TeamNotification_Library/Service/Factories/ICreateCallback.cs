using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories
{
    public interface ICreateCallback
    {
//        Action<Collection> BuildFor(StackPanel panel);
        
        Func<Collection, StackPanel> BuildFor(StackPanel panel);

//        Func<Collection, StackPanel> Build(Task<string> responseTask);

        Func<Collection, StackPanel> Build();

    }
}