using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Tags;
using YourMoviesForum.Web.InputModels.Categories;
using YourMoviesForum.Web.InputModels.Replies;
using YourMoviesForum.Data.Models;

using AutoMapper;

using System.Linq;
using YourMoviesForum.Web.InputModels.Reactions.enums;
using YourMoviesForum.Web.InputModels.Reactions;

namespace YourMovies.Web
{
    public class MappingProfiler:Profile
    {
        public MappingProfiler()
        {
            //Posts
            CreateMap<Post, PostListingViewModel>()
                .ForMember(
                    x => x.RepliesCount,
                    x => x.MapFrom(src => src.Replies.Count(r => !r.IsDeleted)));
            CreateMap<Post, AllPostsQueryModel>();
            CreateMap<Post, PostTagViewModel>();
            CreateMap<Post, PostDetailsViewModel>()
                .ForMember(
                  x => x.RepliesCount,
                  x => x.MapFrom(src => src.Replies.Count(r => !r.IsDeleted)))
                .ForMember(
                  x => x.LikesCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Like)))
                .ForMember(
                  x => x.HeartReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Heart)))
                .ForMember(
                  x => x.HahaReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Haha)))
                .ForMember(
                  x => x.WowReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Wow)))
                .ForMember(
                  x => x.SadReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Sad)))
                .ForMember(
                  x => x.AngryReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Angry))); 
            CreateMap<Post, EditPostFormModel>()
              .ForMember(
                  x=>x.TagIds,
                  x=>x.MapFrom(src=>src.Tags.Select(t=>t.Id)));
            CreateMap<Post,PostDeleteViewModel>();
            CreateMap<Post,PostDeleteAuthorViewModel>();
            CreateMap<Post, PostReactionsViewModel>()
                .ForMember(
                  x => x.LikesCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Like)))
                .ForMember(
                  x => x.HeartReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Heart)))
                .ForMember(
                  x => x.HahaReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Haha)))
                .ForMember(
                  x => x.WowReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Wow)))
                .ForMember(
                  x => x.SadReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Sad)))
                .ForMember(
                  x => x.AngryReactionsCount,
                  x => x.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Angry)));

            //Categories
            CreateMap<Category, PostCategoryViewModel>();
            CreateMap<Category, CategoryListingViewModel>()
                 .ForMember(
                    x => x.PostsCount,
                    x => x.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)));


            //Tags
            CreateMap<Tag, PostTagViewModel>();
            CreateMap<Tag, TagsListingViewModel>()
                 .ForMember(
                    x => x.PostsCount,
                    x => x.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)));


            //Replies
            CreateMap<Reply, PostRepliesDetailsViewModel>();
            CreateMap<Reply, EditReplyFormModel>();
            CreateMap<Reply, ReplyDetailsViewModel>();
            CreateMap<Reply, ReplyDeleteViewModel>();
            CreateMap<Reply,ReplyDeleteAuthorViewModel>();

            //User
            CreateMap<ApplicationUser,PostAuthorDetailsViewModel>();
            CreateMap<ApplicationUser,ReplyAuthorDetailsViewModel>();
        }
    }
}
