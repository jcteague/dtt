using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Controls
{
    [Subject(typeof(ChatRoomsControlService))]
    public class ChatRoomsControlServiceSpecs
    {
        public abstract class Concern : Observes<IServiceChatRoomsControl, ChatRoomsControlService>
        {

        }

        public class When_getting_a_collection : Concern
        {
            private Establish Context = () =>
            {
                string siteUrl = "MyUrl/";
                roomId = "1";

                IProvideUser userProvider = depends.on<IProvideUser>();
                IProvideConfiguration<ServerConfiguration> configuration = depends.on<IProvideConfiguration<ServerConfiguration>>();
                IStoreConfiguration serverConfiguration = fake.an<IStoreConfiguration>();
                ISendHttpRequests httpClient = depends.on<ISendHttpRequests>();

                collection = new Collection { href = "asd" };
                messagesCollection = new Collection { href = "4509u245j" };
                User user = new User { first_name = "foo", last_name = "bar", id = 1, email = "foo@bar.com", password = "123456789" };

                Task<Collection> firstTask = Task.Factory.StartNew(() => collection);
                Task<Collection> messageTask = Task.Factory.StartNew(() => messagesCollection);

                string userUrl = siteUrl + "user/" + user.id;
                string messagesUrl = siteUrl + "room/" + roomId + "/messages";

                serverConfiguration.Stub(x => x.Uri).Return(siteUrl);
                userProvider.Stub(x => x.GetUser()).Return(user);
                configuration.Stub(x => x.Get()).Return(serverConfiguration);

                httpClient.Stub(x => x.Get<Collection>(userUrl)).Return(firstTask);
                httpClient.Stub(x => x.Get<Collection>(messagesUrl)).Return(messageTask);
            };

            private Because of = () =>
            {
                result = sut.GetCollection();
                result2 = sut.GetMessagesCollection(roomId);
            }; 

            It should_return_the_collection_from_the_http_client = () =>
                result.ShouldEqual(collection);

            It should_return_the_messages_collection_from_the_http_client = () =>
                result2.ShouldEqual(messagesCollection);

            private static string roomId;
            private static Collection result;
            private static Collection result2;
            private static Collection collection;
            private static Collection messagesCollection;
        }
    }
}
