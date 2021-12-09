﻿namespace YourMoviesForum.Common
{
    public static class ErrorMessages
    {
        public static class Post
        {
            public const string TitleLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
            public const string ContentLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
        }

        public static class Tags
        {
            public const string TagNameLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
            public const string TagExistingNameErrorMessage = "This tag name is already used";
            public const string TagNonExistingIdErrorMessage = "Invalid tag";
        }
    }
}
