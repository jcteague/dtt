namespace TeamNotification_Library.Service.Mappers
{
    public interface IMapEntities<T, R>
    {
        R MapFrom(T source);
    }
}