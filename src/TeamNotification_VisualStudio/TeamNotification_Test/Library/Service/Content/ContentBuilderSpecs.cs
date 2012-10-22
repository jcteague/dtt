using System.Collections.Generic;
using System.Windows.Controls;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Content;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Service.Content
{
    [Subject(typeof(ContentBuilder))]
    public class ContentBuilderSpecs
    {
        public abstract class Concern : Observes<IBuildContent, ContentBuilder>
        {
             
        }

        public class when_getting_the_content_from_a_collection : Concern
        {
            Establish context = () =>
            {
                var data1 = new CollectionData {label = "foo label", name = "foo name", type = "string"};
                var data2 = new CollectionData {label = "bar label", name = "bar name", type = "password"};
                var collectionDataList = new List<CollectionData> {data1, data2};
                collection = new Collection {template = new Collection.Template {data = collectionDataList}};

                inputFactory = depends.on<IBuildStackPanels>();
                
                panel1 = new StackPanel {Name = "foo"};
                inputFactory.Stub(x => x.GetFor(data1)).Return(panel1);
                panel2 = new StackPanel {Name = "bar"};
                inputFactory.Stub(x => x.GetFor(data2)).Return(panel2);
            };

            Because of = () =>
                result = sut.GetContentFor(collection);

            It should_create_an_input_for_each_field_type_in_the_collection_data = () =>
                result.ShouldContainOnly(panel1, panel2);

            private static IEnumerable<StackPanel> result;
            private static Collection collection;
            private static IBuildStackPanels inputFactory;
            private static StackPanel panel1;
            private static StackPanel panel2;
        }
    }
}