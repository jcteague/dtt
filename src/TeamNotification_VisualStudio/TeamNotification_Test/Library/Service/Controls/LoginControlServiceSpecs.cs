using System.Threading.Tasks;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Service.Controls
{
    [Subject(typeof(LoginControlService))]
    public class LoginControlServiceSpecs
    {
        public abstract class Concern : Observes<IServiceLoginControl, LoginControlService>
        {
            Establish context = () =>
            {
                httpClient = depends.on<ISendHttpRequests>();
                configurationProvider = depends.on<IProvideConfiguration<LoginConfiguration>>();                        
            };

            protected static ISendHttpRequests httpClient;
            protected static IProvideConfiguration<LoginConfiguration> configurationProvider;
        }

        public class when_getting_the_collection : Concern
        {
            Establish context = () =>
            {
                collection = fake.an<Collection>();

                var configuration = fake.an<IStoreConfiguration>();
                configurationProvider.Stub(x => x.Get()).Return(configuration);

                var collectionTask = Task.Factory.StartNew(() => collection);
                httpClient.Stub(x => x.Get<Collection>(configuration.HREF)).Return(collectionTask);
            };

            Because of = () =>
                result = sut.GetCollection();

            It should_return_a_collection_from_the_href = () =>
                result.ShouldEqual(collection);
            
            private static Collection result;
            private static Collection collection;
        }

        public class when_the_submit_button_is_clicked : Concern
        {
//            Because of = () =>
//
//            It should_post_the_input_values_to_the_service_href = () =>
//                


        }
    }
}