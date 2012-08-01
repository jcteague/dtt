using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;

namespace TeamNotification_Library.Service.Mappers
{
    public class CollectionDataListToFormUrlEncodedContentMapper : IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent>
    {
        private readonly ICreateFormUrlEncodedContent formFactory;

        public CollectionDataListToFormUrlEncodedContentMapper(ICreateFormUrlEncodedContent formFactory)
        {
            this.formFactory = formFactory;
        }

        public FormUrlEncodedContent MapFrom(IEnumerable<CollectionData> source)
        {
            var data = source.Select(x => new KeyValuePair<string, string>(x.name, x.value));
            return formFactory.GetInstance(data);
        }
    }
}