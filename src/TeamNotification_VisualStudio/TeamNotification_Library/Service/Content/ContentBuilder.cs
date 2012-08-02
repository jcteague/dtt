using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Content
{
    public class ContentBuilder : IBuildContent
    {
        private IBuildStackPanels stackPanelFactory;

        public ContentBuilder(IBuildStackPanels stackPanelFactory)
        {
            this.stackPanelFactory = stackPanelFactory;
        }
        
        public IEnumerable<StackPanel> GetContentFor(Collection collection)
        {
            return collection.template.data.Select(x => stackPanelFactory.GetFor(x));
        }
    }
}