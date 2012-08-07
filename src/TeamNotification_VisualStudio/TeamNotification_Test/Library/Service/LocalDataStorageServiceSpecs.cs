using System.Collections.Generic;
using Machine.Specifications;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.FileSystem;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service
{
    [Subject(typeof(LocalDataStorageService))]
    public class LocalDataStorageServiceSpecs
    {
        public abstract class Concern : Observes<IStoreDataLocally, LocalDataStorageService>
        {
            Establish context = () =>
            {
                resourceHandler = depends.on<IHandleFiles>();
                userFromResponseGetter = depends.on<ICreateUserFromResponse>();
            };
            
            protected static IHandleFiles resourceHandler;
            protected static ICreateUserFromResponse userFromResponseGetter;
        }

        public class when_storing_a_user : Concern
        {
            Establish context = () =>
            {
                userObject = new User {first_name = "blah"};
                userFromResponseGetter.Stub(x => x.Get(user, items)).Return(userObject);
            };

            Because of = () =>
                sut.Store(user, items);

            It should_write_the_user_with_the_resource_handler = () =>
                resourceHandler.AssertWasCalled(x => x.Write(userObject));
                
            private static User user;
            private static IEnumerable<CollectionData> items;
            private static User userObject;
        }
         
    }
}