using System;
using System.Windows.Controls;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Renderer;

namespace AvenidaSoftware.TeamNotification_Package
{
    public class DynamicControlBuilder : IBuildDynamicControls
    {
        private readonly ISendHttpRequests httpClient;
        private readonly IRenderContent contentRenderer;

        public DynamicControlBuilder(ISendHttpRequests httpClient, IRenderContent contentRenderer)
        {
            this.httpClient = httpClient;
            this.contentRenderer = contentRenderer;
        }

        public StackPanel GetContentFrom(string href)
        {
            return contentRenderer.Render(httpClient.Get<Collection>(href).Result);
        }
    }
}