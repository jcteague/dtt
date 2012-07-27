using System;
using System.Threading.Tasks;

namespace TeamNotification_Library.Service.Http
{
    public interface ISendHttpRequests
    {
        void Get(string uri);

        void Get(string uri, Action<Task<string>> action);

        Task<T> Get<T>(string uri) where T : class;
    }
}