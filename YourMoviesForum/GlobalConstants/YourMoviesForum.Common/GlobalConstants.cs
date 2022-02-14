namespace YourMoviesForum.Common
{
    public static class GlobalConstants
    {
        //Global
        public const string SystemName = "Your Movies Forum";
        public const string SystemEmail = "yourmoviesforum@gmail.com";
        public const int PostPerPage = 5;
        public const int TagsPerPage = 8;
        public const string IFrameTag = "iframe";

        public static class Post
        {
            public const int PostTitleMinLength = 3;
            public const int PostTitleMaxLength = 70;
            public const int PostContentMinLength = 10;
            public const int PostContentMaxLength = 500;
        }

        public static class Tag
        {
            public const int TagNameMaxLength = 20;
            public const int TagNameMinLength = 5;
            public const string TagDisplayName = "Tags";
        }

        public static class Category
        {
            public const string CategoryDisplayName = "Categories";
        }

        public static class User
        {
            public const int UserUsernameMaxLength = 30;
            public const int UserUsernameMinLength = 4;
            public const int UserPasswordMaxLength = 50;
            public const int UserPasswordMinLength = 6;
        }

        public static class Administrator
        {
            public const string AdministratorRoleName = "Administrator";
            public const string AdministratorUsername = "Admin";
            public const string AdministratorEmail = "admin@yourmoviesforum.net";
            public const string AdministratorPassword = "admin567";
        }

        public static class TestUser
        {
            public const string UserUserName = "testuser";
            public const string UserEmail = "testuser@forum.net";
            public const string UserPassword = "user567";
        }

    }
}
