using System;
using AurelienRibon.Ui.SyntaxHighlightBox;
using ICSharpCode.AvalonEdit.Highlighting;
using StructureMap.Configuration.DSL;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Factories.UI.Highlighters;
using TeamNotification_Library.Service.Highlighters;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.LocalSystem;

namespace TeamNotification_Library.Application
{
    public class TeamNotificationLibraryRegistry : Registry
    {
        public TeamNotificationLibraryRegistry()
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.RegisterConcreteTypesAgainstTheFirstInterface().OnAddedPluginTypes(x => x.Singleton());
            });

            For<IProvideConfiguration<RedisConfiguration>>().Singleton().Use<RedisConfigurationProvider>();
            For<IStoreGlobalState>().Singleton();
            For<IStoreDataLocally>().Singleton();
            For<IStoreClipboardData>().Singleton();
            For<IHandleSystemClipboard>().Singleton();
            For<ICreateSyntaxBlockUIInstances>().Singleton();
            For<IProvideSyntaxHighlighter<IHighlightingDefinition>>().Singleton();
			For<IStoreDTE>().Singleton();
            For<IHandleVisualStudioClipboard>().Singleton();
            For<IListenToMessages<Action<string, string>>>().Singleton();
        }
    }
}