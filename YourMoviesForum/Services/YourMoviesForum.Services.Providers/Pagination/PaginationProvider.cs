using System;
using YourMovies.Web.Views.Pagination;

namespace YourMoviesForum.Services.Providers.Pagination
{
    public static class PaginationProvider 
    {
       public static PaginationViewModel PaginationHelper(int page, int count,int itemsPerPage)
        {
            return new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(count / (decimal)itemsPerPage)
            };
        }
    }
}
