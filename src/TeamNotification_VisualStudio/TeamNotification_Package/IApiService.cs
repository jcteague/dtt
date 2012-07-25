namespace AvenidaSoftware.TeamNotification_Package
{
    public interface IApiService
    {
        string GetString();
    }

    public class ApiService : IApiService
    {
        public string GetString()
        {
            return "Hello From Service";
        }
    }
}