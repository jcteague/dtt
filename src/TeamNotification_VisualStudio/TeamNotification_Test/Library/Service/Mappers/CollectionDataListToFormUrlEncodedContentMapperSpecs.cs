using System.Collections.Generic;
using System.Net.Http;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Mappers;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Service.Mappers
{
    [Subject(typeof(CollectionDataListToFormUrlEncodedContentMapper))]
    public class CollectionDataListToFormUrlEncodedContentMapperSpecs
    {
        public abstract class Concern : Observes<IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent>, CollectionDataListToFormUrlEncodedContentMapper>
        {
            
        }

        public class when_mapping_a_list_of_collection_data : Concern
        {
            Establish context = () =>
            {
                formFactory = depends.on<ICreateFormUrlEncodedContent>();

                var collectionData1 = new CollectionData { name = "foo", value = "foo value" };
                var collectionData2 = new CollectionData { name = "bar", value = "bar value" };
                collectionDataList = new List<CollectionData> { collectionData1, collectionData2 };
                
                var data = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("foo", "foo value"), new KeyValuePair<string, string>("bar", "bar value") };
                form = new FormUrlEncodedContent(data);
                formFactory.Stub(x => x.GetInstance(Arg<IEnumerable<KeyValuePair<string, string>>>.List.ContainsAll(data))).Return(form);
            };

            Because of = () =>
                result = sut.MapFrom(collectionDataList);

            It should_return_a_form_filled_with_each_input_name_and_value = () =>
                result.ShouldEqual(form);
            
            private static IEnumerable<CollectionData> collectionDataList;
            private static FormUrlEncodedContent result;
            private static FormUrlEncodedContent form;
            private static ICreateFormUrlEncodedContent formFactory;
        }
         
    }
}