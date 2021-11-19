using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YourMoviesForum.Web.InputModels.Home;

namespace YourMoviesForum.Web.InputModels.Posts
{
    public class AllPostsQueryModel
    {
        [Display(Name = "Search...")]
        public string SearchTerm { get; init; }

        public PostSorting Sorting { get; init; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
        public int NextPage
        {
            get
            {
                if (CurrentPage >= TotalPages)
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
                if (CurrentPage <= 1)
                {
                    return this.TotalPages;
                }

                return CurrentPage - 1;
            }
        }
        public IEnumerable<PostListingViewModel> Posts { get; set; }
    }
}

