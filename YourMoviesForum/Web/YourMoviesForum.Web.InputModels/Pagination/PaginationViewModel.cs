namespace YourMovies.Web.Views.Pagination
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
        public int NextPage
        {
            get
            {
                if (CurrentPage == TotalPages)
                {
                    return 1;
                }

                return CurrentPage + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (CurrentPage == 0)
                {
                    return this.TotalPages;
                }

                return CurrentPage - 1;
            }
        }

        public string SearchTerm { get; set; }
    }
}

