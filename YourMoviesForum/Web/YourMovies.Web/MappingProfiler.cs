using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Tags;
using YourMoviesForum.Data.Models;

using AutoMapper;

using System.Linq;
using YourMoviesForum.Web.InputModels.Categories;

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
            CreateMap<Post, EditPostFormModel>()
              .ForMember(
                  x=>x.TagIds,
                  x=>x.MapFrom(src=>src.Tags.Select(t=>t.Id)));
            CreateMap<Post,PostDeleteViewModel>();
            CreateMap<Post,PostDeleteAuthorViewModel>();

            //Categories
            CreateMap<Category, PostCategoryViewModel>();
            CreateMap<Category, CategoryListingViewModel>()
                 .ForMember(
                    dest => dest.PostsCount,
                    dest => dest.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)));


            //Tags
            CreateMap<Tag, PostTagViewModel>();
            CreateMap<Tag, TagsListingViewModel>()
                 .ForMember(
                    dest => dest.PostsCount,
                    dest => dest.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)));


            //Replies
            CreateMap<Reply, PostRepliesDetailsViewModel>();

            //User
            CreateMap<ApplicationUser,PostAuthorDetailsViewModel>();

        }
    }
}
