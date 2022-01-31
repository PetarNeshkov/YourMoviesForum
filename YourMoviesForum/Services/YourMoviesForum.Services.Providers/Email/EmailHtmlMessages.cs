namespace YourMoviesForum.Services.Providers.Email
{
    public static class EmailHtmlMessages
    {
        public static string GetResetPasswordHtml(string username, string ressetPasswordLink)
        {
            return $"<div style=\"font-size:20px\">" +
                    $"<div>Hi {username},</div>" +
                    $"<br>" +
                    $"<div>looks like you having problems logging in.</div>" +
                    $"<div>Please reset your password by <a href='{ressetPasswordLink}'>clicking here</a>.</div>" +
                    $"<br>" +
                    $"<div>Sincerely Yours,</div>" +
                    $"<div>YourMovies Forum Team</div>" +
                    $"</div>";
        }

        public static string GetEmailConfirmationHtml(string username, string confirmationLink)
        {
            return $"<div style=\"font-size:20px\">" +
                    $"<div>Hi {username},</div>" +
                    $"<br>" +
                    $"<div>thank you for becoming a member of our forum comunity.</div>" +
                    $"<div>Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.</div>" +
                    $"<br>" +
                    $"<div>Sincerely Yours,</div>" +
                    $"<div>YourMovies Team</div>" +
                    $"</div>";
        }
    }
}
