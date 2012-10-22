namespace TeamNotification_Library.Configuration
{
    public class GlobalConstants
    {
        public static class Fields
        {
            public const string Password = "password";
            public const string TextBox = "string";
        }

        public static class Paths
        {
            public const string UserResource = @"yackety.user";
            public const string LogFile = @"yackety.log";
        }

        public static class ProgrammingLanguages
        {
            public const int Unknown = -1;
            public const int CSharp = 1;
            public const int JavaScript = 2;
            public const int VisualBasic = 3;
        }

        public static class DockPositions
        {
            public const int NotDocked = 0;
            public const int Top = 1;
            public const int Right = 2;
            public const int Bottom = 3;
            public const int Left = 4;
        }

        public static class DockOrientations
        {
            public const int InputAtRight = 0;
            public const int InputAtBottom = 1;
        }

        public static class Guids
        {
            public const string LoginWindowPersistanceString = "052b3fa5-4a40-4588-9614-5b99dce7120d";
        }

        public static class Logentries
        {
            public const string Token = "6824292a-62fa-4a82-98b3-457cf38d555a";
        }
    }
}