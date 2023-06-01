namespace YourMoviesForum.Services.Providers.Email
{
    public static class EmailHtmlMessages
    {
        public static string GetResetPasswordHtml(string username, string resetPasswordLink)
        {
            return $"<div style=\"font-size:20px\">" +
                    $"<div>Hi {username},</div>" +
                    $"<br>" +
                    $"<div>Looks like you having problems logging in.</div>" +
                    $"<div>Please reset your password by <a href='{resetPasswordLink}'>clicking here</a>.</div>" +
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
                    $"<div>Thank you for becoming a member of our forum community.</div>" +
                    $"<div>Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.</div>" +
                    $"<br>" +
                    $"<div>Sincerely Yours,</div>" +
                    $"<div>YourMovies Team</div>" +
                    $"</div>";
        }

        public static string RemoveAccountConfirmationLink(string email, string confirmationLink)
        {
            return $"<div style=\"font-size:20px\">" +
                   $"<div>We are sorry to let you go!</div>" +
                   $"<div>The following account of {email} will be deleted. Please confirm <a href='{confirmationLink}'>clicking here</a>.</div>" +
                   $"<br>" +
                   $"<div>Sincerely Yours,</div>" +
                   $"<div>YourMovies Team</div>" +
                   $"</div>";
        }
    }
}
