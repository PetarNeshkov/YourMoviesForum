using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Tags;
using YourMoviesForum.Data.Models;

using AutoMapper;
using System.Globalization;

namespace YourMovies.Web
{
    public class MappingProfiler:Profile
    {
        public MappingProfiler()
        {
            //Posts
            CreateMap<Post, PostListingViewModel>();
            CreateMap<Post, AllPostsQueryModel>();
            CreateMap<Post, PostTagViewModel>();
            CreateMap<Post, PostDetailsViewModel>();
     

            //Categories
            CreateMap<Category, PostCategoryViewModel>();


            //Tags
            CreateMap<Tag, PostTagViewModel>();
            CreateMap<Tag, TagsListingViewModel>();


            //User
            CreateMap<ApplicationUser,PostAuthorDetailsViewModel>();
        }
    }
}
