using StructureMap;

namespace TeamNotification_Library.Service
{
    public class Container
    {
        public static T GetInstance<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }
    }
}