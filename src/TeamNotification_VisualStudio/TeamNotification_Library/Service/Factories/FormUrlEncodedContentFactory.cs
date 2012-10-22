using System.Collections.Generic;
using System.Net.Http;

namespace TeamNotification_Library.Service.Factories
{
    public class FormUrlEncodedContentFactory : ICreateFormUrlEncodedContent
    {
        public FormUrlEncodedContent GetInstance(IEnumerable<KeyValuePair<string, string>> data)
        {
            return new FormUrlEncodedContent(data);
        }
    }
}