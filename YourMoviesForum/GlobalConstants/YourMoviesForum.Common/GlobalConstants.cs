namespace YourMoviesForum.Common
{
    public static class GlobalConstants
    {
        //Global
        public const string SystemName = "Your Movies";
        public const int PostPerPage = 3;
        public const int TagsPerPage = 8;

        //Posts
        public static class Post
        {
            public const int PostTitleMinLength = 3;
            public const int PostTitleMaxLength = 50;
            public const int PostContentMinLength = 10;
            public const int PostContentMaxLength = 500;
        }

        //Tags
        public static class Tags
        {
            public const int TagNameMaxLength = 20;
            public const int TagNameMinLength = 5;
        }

        public static class User
        {
            public const int UserUsernameMaxLength = 30;
            public const int UserUsernameMinLength = 4;
            public const int UserPasswordMaxLength = 50;
            public const int UserPasswordMinLength = 6;
        }

    }
}
