using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Content;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;
using TeamNotification_Library.Service.Providers;
using TeamNotification_Library.Configuration;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private IServiceLoginControl loginControlService;
        private IBuildContent contentBuilder;
        private IGetFieldValue fieldValueGetter;
        private readonly IHandleUserAccountEvents userAccountEvents;
        private IProvideConfiguration<RedisConfiguration> redisConfigurationProvider;
        public LoginControl(IServiceLoginControl loginControlService, IProvideConfiguration<RedisConfiguration> redisConfigurationProvider, IBuildContent contentBuilder, IGetFieldValue fieldValueGetter, IHandleUserAccountEvents userAccountEvents)
        {
            this.loginControlService = loginControlService;
            this.contentBuilder = contentBuilder;
            this.fieldValueGetter = fieldValueGetter;
            this.userAccountEvents = userAccountEvents;
            this.redisConfigurationProvider = redisConfigurationProvider;
            InitializeComponent();
            
            var collection = loginControlService.GetCollection();
            Resources.Add("templateData", collection.template.data);

            foreach (var panel in this.contentBuilder.GetContentFor(collection))
            {
                templateContainer.Children.Add(panel);
            }

            this.userAccountEvents.UserHasLogged += this.OnUserHasLogged;
            this.userAccountEvents.UserCouldNotLogIn += (sender, e) => MessageBox.Show("User and passwords are incorrect");
        }

        private void OnUserHasLogged(object sender, UserHasLogged args  )
        {
            redisConfigurationProvider.Get().Uri =
                args.RedisConfig.host + ":" + args.RedisConfig.port;
            this.Content = Container.GetInstance<Chat>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var collection = new List<CollectionData>();
            foreach (CollectionData item in (IEnumerable<CollectionData>)Resources["templateData"])
            {
                collection.Add(fieldValueGetter.GetValue(item, templateContainer));
            }
            loginControlService.HandleClick(collection);
        }
    }
}
