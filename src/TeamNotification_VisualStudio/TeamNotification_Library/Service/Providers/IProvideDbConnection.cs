using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Service.Providers
{
    public interface IProvideDbConnection<T> where T : IRedisConnection
    {
        T Get();

        void Set(string host, int port);
    }
}