using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Renderer;

namespace AvenidaSoftware.TeamNotification_Package
{
    public class DynamicControlBuilder : IBuildDynamicControls
    {
        private readonly ISendHttpRequests httpClient;
        private ICreateCallback callbackFactory;
        private ISerializeJSON serializer;

        public DynamicControlBuilder(ISendHttpRequests httpClient, ICreateCallback callbackFactory, ISerializeJSON serializer)
        {
            this.callbackFactory = callbackFactory;
            this.httpClient = httpClient;
            this.serializer = serializer;
        }

//        public Panel GetContentFrom(string href)
//        {
//            var panel = new StackPanel();
//            return httpClient.Get(href, callbackFactory.BuildFor(panel)).Result;
//            return panel;
//        }
        
        public Panel GetContentFrom(string href)
        {
            return httpClient.Get(href, x => serializer.Deserialize<Collection>(x.Result)).Result;
        }
    }
}