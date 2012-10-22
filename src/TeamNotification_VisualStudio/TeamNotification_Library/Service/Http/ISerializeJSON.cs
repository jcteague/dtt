namespace TeamNotification_Library.Service.Http
{
    public interface ISerializeJSON
    {
        T Deserialize<T>(string json);
        string Serialize<T>(T obj);
    }
}