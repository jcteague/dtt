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

        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
    }
}