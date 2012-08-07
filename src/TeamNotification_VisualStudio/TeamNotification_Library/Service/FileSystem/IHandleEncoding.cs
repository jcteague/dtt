namespace TeamNotification_Library.Service.FileSystem
{
    public interface IHandleEncoding
    {
        string Encode(string value);
        string Decode(string encodedData);
    }
}