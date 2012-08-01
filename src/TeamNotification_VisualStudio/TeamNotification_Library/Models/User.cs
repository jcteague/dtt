namespace TeamNotification_Library.Models
{
    public class User
    {
        public User()
        {
            Id = 1;
            FirstName = "Jhon";
            LastName = "Doe";
            Email = "foo@bar.com";
        }

        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
    }
}