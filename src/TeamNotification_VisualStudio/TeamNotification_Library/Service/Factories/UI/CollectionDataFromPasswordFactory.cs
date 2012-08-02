using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Controls;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class CollectionDataFromPasswordFactory : IBuildCollectionDataFromElement<PasswordBox>
    {
        private IHelpControls controlFormHelper;

        public CollectionDataFromPasswordFactory(IHelpControls controlFormHelper)
        {
            this.controlFormHelper = controlFormHelper;
        }

        public CollectionData Get(DependencyObject container, CollectionData item)
        {
            var element = controlFormHelper.Find<PasswordBox>(container, item.name);
            return new CollectionData
                       {
                           label = item.label,
                           maxlength = item.maxlength,
                           name = item.name,
                           type = item.type,
                           value = element.Password
                       };
        }
    }
}