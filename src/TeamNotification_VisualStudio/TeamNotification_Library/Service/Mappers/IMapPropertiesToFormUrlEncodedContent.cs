using System.Net.Http;

namespace TeamNotification_Library.Service.Mappers
{
    public interface IMapPropertiesToFormUrlEncodedContent
    {
        FormUrlEncodedContent MapFrom<T>(T source);
    }
}