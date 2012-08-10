 using System.Collections.Generic;
 using System.Net.Http;
 using Machine.Specifications;
 using TeamNotification_Library.Service.Factories;
 using TeamNotification_Library.Service.Mappers;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Mappers
{  
    [Subject(typeof(ObjectToFormUrlEncodedContentMapper))]  
    public class ObjectToFormUrlEncodedContentMapperSpecs
    {
        public abstract class Concern : Observes<IMapPropertiesToFormUrlEncodedContent, ObjectToFormUrlEncodedContentMapper>
        {
            Establish context = () =>
            {
                formFactory = depends.on<ICreateFormUrlEncodedContent>();

            };

            protected static ICreateFormUrlEncodedContent formFactory;
        }

   
        public class when_mapping_an_object : Concern
        {
            Establish context = () =>
            {
                stubObject = new StubClass
                                 {
                                     FirstProperty = "foo",
                                     SecondProperty = "bar",
                                     IntegerProperty = 10
                                 };

                var value1 = new KeyValuePair<string, string>("FirstProperty", "foo");
                var value2 = new KeyValuePair<string, string>("SecondProperty", "bar");
                var value3 = new KeyValuePair<string, string>("IntegerProperty", "10");
                var nameValueCollection = new List<KeyValuePair<string, string>> {value1, value2, value3};

                form = new FormUrlEncodedContent(nameValueCollection);
                formFactory.Stub(x => x.GetInstance(Arg<List<KeyValuePair<string, string>>>.List.ContainsAll(nameValueCollection))).Return(form);
            };

            Because of = () =>
                result = sut.MapFrom(stubObject);

            It should_return_a_form_with_all_the_object_properties_as_name_values = () =>
                result.ShouldEqual(form);
                
            private static FormUrlEncodedContent result;
            private static StubClass stubObject;
            private static FormUrlEncodedContent form;

            public class StubClass
            {
                public string FirstProperty { get; set; }
                public string SecondProperty { get; set; }
                public int IntegerProperty { get; set; }
            }
        }
    }
}
