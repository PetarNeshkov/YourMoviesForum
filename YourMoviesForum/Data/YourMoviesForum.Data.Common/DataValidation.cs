namespace YourMoviesForum.Data.Common
{
    public static class DataValidation
    {
        public static class Post
        {
            public const int IdMaxLength = 40;
            public const int TitleMaxLength = 20;
            public const int ContentMaxLength = 400;
        }

        public static class Reply
        {
            public const int IdMaxLength = 40;
            public const int ContentMaxLength = 400;
        }

        public static class User
        {
            public const int IdMaxLength = 40;
            public const int FirstNameMaxLength = 20;
            public const int LastNameMaxLength = 20;
        }

        public static class Tag
        {
            public const int IdMaxLength = 40;
            public const int NameMaxLength = 30;
        }

        public static class Category
        {
            public const int IdMaxLength = 40;

        }
    }
}
