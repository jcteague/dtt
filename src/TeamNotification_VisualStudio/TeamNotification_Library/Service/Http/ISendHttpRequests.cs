using System;
using System.Net.Http;
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

        Task<T> Post<T>(string uri, HttpContent content) where T : class;

        void Post(IEnumerable<Tuple<string, HttpContent>> values);

        HttpResponseMessage PostSync(string uri, HttpContent content);
    }
}