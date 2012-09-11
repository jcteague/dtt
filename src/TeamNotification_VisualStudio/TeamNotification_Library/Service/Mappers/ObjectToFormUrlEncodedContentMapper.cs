using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;

namespace TeamNotification_Library.Service.Mappers
{
    public class ObjectToFormUrlEncodedContentMapper : IMapPropertiesToFormUrlEncodedContent
    {
        private ICreateFormUrlEncodedContent formFactory;

        public ObjectToFormUrlEncodedContentMapper(ICreateFormUrlEncodedContent formFactory)
        {
            this.formFactory = formFactory;
        }

        public FormUrlEncodedContent MapFrom<T>(T source)
        {
            var data =
                source.GetType()
                .GetProperties().Where(x => x.GetValue(source,null).ToString()!= "" )
                .Select(
                    x => new KeyValuePair<string, string>(x.Name, x.GetValue(source, null).ToString())
                );
            
            return formFactory.GetInstance(data);
        }
    }
}