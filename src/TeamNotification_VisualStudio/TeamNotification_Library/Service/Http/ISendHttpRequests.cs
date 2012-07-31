using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TeamNotification_Library.Service.Http
{
    public interface ISendHttpRequests
    {
        void Get(string uri);

        void Get(string uri, Action<Task<string>> action);

        Task<T> Get<T>(string uri) where T : class;

        Task<T> Post<T>(string uri, HttpContent content) where T : class;
//        Task<HttpResponseMessage> Post<T>(string uri, HttpContent content) where T : class;

    }
}