using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Mappers
{
    public class ObjectToFormUrlEncodedContentMapper : IMapPropertiesToFormUrlEncodedContent
    {
        public FormUrlEncodedContent MapFrom<T>(T source)
        {
            var data =
                GetType()
                .GetProperties()
                .Select(
                    x => new KeyValuePair<string, string>(x.Name, x.GetValue(this, null).ToString())
                );
            
            return new FormUrlEncodedContent(data);
        }
    }
}