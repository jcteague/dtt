namespace TeamNotification_Library.Service.Factories
{
    public interface ICreateInstances<T>
    {
        T GetInstance();
    }
}