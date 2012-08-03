namespace TeamNotification_Library.Models
{
    public class User
    {
        public User()
        {
            id = 1;
            first_name = "eddy";
            last_name = "espinal";
            email = "eespinal@intellisys.com.do";
            password = "123456";
        }

        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}