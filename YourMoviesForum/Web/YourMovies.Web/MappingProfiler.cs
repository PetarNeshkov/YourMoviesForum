using AutoMapper;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Tags;

namespace YourMovies.Web
{
    public class MappingProfiler:Profile
    {
        public MappingProfiler()
        {
            //Categories
            CreateMap<Category, PostCategoryViewModel>();


            //Tags
            CreateMap<Tag, PostsTagViewModel>();
        }
    }
}
