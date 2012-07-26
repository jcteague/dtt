using System;
using System.Threading.Tasks;

namespace TeamNotification_Library.Service.Http
{
    public interface ISendHttpRequests
    {
        void Get(string uri);

        void Get(string uri, Action<Task<string>> action);
        
        Task<R> Get<T, R>(string uri, Func<T, R> action) where T : class;
    }
}