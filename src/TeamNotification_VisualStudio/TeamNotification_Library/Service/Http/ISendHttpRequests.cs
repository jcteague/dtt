using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TeamNotification_Library.Service.Http
{
    public interface ISendHttpRequests
    {
        void Get(string uri);

        void Post(string uri, params KeyValuePair<string, string>[] values);

        void Get(string uri, Action<Task<string>> action);

        Task<T> Get<T>(string uri) where T : class;
    }
}