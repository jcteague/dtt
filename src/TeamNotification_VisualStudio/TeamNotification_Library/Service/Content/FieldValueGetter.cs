using System;
using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Factories.UI;

namespace TeamNotification_Library.Service.Content
{
    public class FieldValueGetter : IGetFieldValue
    {
        private IBuildCollectionDataFromElement<PasswordBox> collectionDataFromPasswordFactory;
        private IBuildCollectionDataFromElement<TextBox> collectionDataFromTextBoxFactory;

        public FieldValueGetter(IBuildCollectionDataFromElement<PasswordBox> collectionDataFromPasswordFactory, IBuildCollectionDataFromElement<TextBox> collectionDataFromTextBoxFactory)
        {
            this.collectionDataFromPasswordFactory = collectionDataFromPasswordFactory;
            this.collectionDataFromTextBoxFactory = collectionDataFromTextBoxFactory;
        }

        public CollectionData GetValue(CollectionData item, DependencyObject container)
        {
            switch (item.type)
            {
                case "password":
                    return collectionDataFromPasswordFactory.Get(container, item);
                default:
                    return collectionDataFromTextBoxFactory.Get(container, item);
            }
        }
    }
}