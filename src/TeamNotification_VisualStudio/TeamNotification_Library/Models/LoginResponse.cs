namespace TeamNotification_Library.Models
{
    public class LoginResponse
    {
        public bool success { get; set; }
        public Collection.RedisConfig redis { get; set; }
        public User user { get; set; }
    }
}