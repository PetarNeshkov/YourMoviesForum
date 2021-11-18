using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Tags;
using YourMoviesForum.Data.Models;

using AutoMapper;


namespace YourMovies.Web
{
    public class MappingProfiler:Profile
    {
        public MappingProfiler()
        {
            //Posts
            CreateMap<Post, PostListingViewModel>();
            CreateMap<Post, AllPostsQueryModel>();

            //Categories
            CreateMap<Category, PostCategoryViewModel>();


            //Tags
            CreateMap<Tag, PostsTagViewModel>();
        }
    }
}
