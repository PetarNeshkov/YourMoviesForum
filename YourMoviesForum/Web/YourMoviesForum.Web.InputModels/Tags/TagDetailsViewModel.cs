using System.Collections.Generic;

using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;

namespace YourMoviesForum.Web.InputModels.Tags
{
    public class TagDetailsViewModel
    {
        public int Id { get; init; }
        public TagsListingViewModel Tag { get; init; }
        public IEnumerable<PostListingViewModel> Posts { get; init; }
        public PostSorting Sorting { get; init; }

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
    }
}
