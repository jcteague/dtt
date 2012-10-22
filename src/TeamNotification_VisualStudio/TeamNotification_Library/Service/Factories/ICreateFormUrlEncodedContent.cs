using System.Collections.Generic;
using System.Net.Http;

namespace TeamNotification_Library.Service.Factories
{
    public interface ICreateFormUrlEncodedContent
    {
        FormUrlEncodedContent GetInstance(IEnumerable<KeyValuePair<string, string>> data);
    }
}