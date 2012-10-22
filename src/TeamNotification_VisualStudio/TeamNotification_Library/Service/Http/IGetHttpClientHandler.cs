using System.Net.Http;

namespace TeamNotification_Library.Service.Http
{
    public interface IGetHttpClientHandler
    {
        HttpClientHandler GetHandler();
    }
}