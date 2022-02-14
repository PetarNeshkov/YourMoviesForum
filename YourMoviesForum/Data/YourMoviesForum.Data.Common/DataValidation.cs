namespace YourMoviesForum.Data.Common
{
    public static class DataValidation
    {
        public const int IdMaxLength = 40;

        public static class Post
        {
            public const int TitleMaxLength = 70;
            //public const int ContentMaxLength = 1000;
        }

        public static class Reply
        {
            public const int ContentMaxLength = 400;
        }

        public static class User
        {
            public const int UsernameMaxLength = 20;
        }

        public static class Tag
        {
            public const int NameMaxLength = 30;
        }

        public static class Category
        {
            public const int NameMaxLength = 20;
        }

    }
}
