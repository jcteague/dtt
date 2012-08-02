using System.Windows.Controls;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class PasswordBoxFactory : ICreateUIElements<PasswordBox>
    {
        public PasswordBox Get(string name)
        {
            return new PasswordBox {Name = name, Width = 100};
        }
    }
}