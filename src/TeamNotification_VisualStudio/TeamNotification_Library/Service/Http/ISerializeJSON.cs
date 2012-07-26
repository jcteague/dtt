namespace TeamNotification_Library.Service.Http
{
    public interface ISerializeJSON
    {
        T Deserialize<T>(string json);
    }
}