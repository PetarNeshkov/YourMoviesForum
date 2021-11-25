using System.Collections.Generic;

namespace YourMoviesForum.Web.InputModels.Tags
{
    public class AllTagsQueryModel
    {
        public int PostsCount { get; init; }
        public string SearchTerm { get; init; }

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
                if (CurrentPage == 1)
                {
                    return this.TotalPages;
                }

                return CurrentPage - 1;
            }
        }

        public IEnumerable<TagsListingViewModel> Tags { get; set; }
    }
}
