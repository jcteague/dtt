using System.Windows.Controls;

namespace TeamNotification_Library.Configuration
{
    public interface IProvideConfiguration<T> where T : IStoreConfiguration
    {
        IStoreConfiguration Get();
    }
}