using System.Collections.Generic;
using TeamNotification_Library.Models;
using System.Linq;
using TeamNotification_Library.Service.Content;

namespace TeamNotification_Library.Service.Mappers
{
    public class TemplateDataToCollectionDataListMapper : IMapEntities<object, IEnumerable<CollectionData>>
    {
        public IEnumerable<CollectionData> MapFrom(object source)
        {
//            return ((IEnumerable<CollectionData>) source).Select(x => GetGetterFor(x.type).GetValue());
            return ((IEnumerable<CollectionData>) source);
//            foreach (CollectionData item in (IEnumerable<CollectionData>)source)
//            {
//                collection.Add(item);
//            }
//            throw new System.NotImplementedException();

        }

        private IGetFieldValue GetGetterFor(CollectionData collectionData)
        {
            return null;
        }

        //            var collection = new List<CollectionData>();
        //            foreach (CollectionData item in (IEnumerable<CollectionData>)Resources["templateData"])
        //            {
        //                collection.Add(item);
        //            }
        //
        //            var password = formHelper.Find<PasswordBox>(templateContainer, "password");
        //            int a = 0;
    }
}