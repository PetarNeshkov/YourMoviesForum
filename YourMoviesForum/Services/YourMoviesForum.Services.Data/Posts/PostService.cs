using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Services.Providers.DateTime;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using YourMoviesForum.Services.Data.Users;

namespace YourMoviesForum.Services.Data.Posts
{
    public class PostService : IPostService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IUserService userService;

        public PostService(
            YourMoviesDbContext data, 
            IMapper mapper,
            IDateTimeProvider dateTimeProvider,
            IUserService userService)
        {
            this.data = data;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
            this.userService = userService;
        }
        public async Task<int> CreatePostAsync(
            string title, 
            string content, 
            int categoryId,
            IEnumerable<int> tagIds,
            string authorId)
        {
            var post = new Post
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                AuthorId = authorId
            };

            await userService.AddRatingToUserAsync(authorId);

            await data.Posts.AddAsync(post);
            await data.SaveChangesAsync();
            await AddTagsAsync(post, tagIds);

            return post.Id;
        }

        private async Task<Post> GetByIdAsync(int id)
            => await data.Posts.Include(t=>t.Tags).FirstOrDefaultAsync(p => p.Id == id);

        private async Task AddTagsAsync(Post post,IEnumerable<int> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                var tagToFind = data.Tags.FirstOrDefault(x=>x.Id==tagId);

                post.Tags.Add(tagToFind);   
            }
               await data.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllPostsAsync<TModel>(PostSorting query,
            string searchFilter = null, int skip = 0, int take = 0)
        {
            var queryablePosts = data.Posts
                 .AsNoTracking()
                 .OrderByDescending(d => d.CreatedOn)
                 .Where(p => !p.IsDeleted);

            queryablePosts = SortingBySearch(searchFilter, queryablePosts);

            queryablePosts = SortingByFilter(query, queryablePosts);

            var posts = await queryablePosts
                .Skip(skip).Take(take)
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            return posts;
        }

        private static IQueryable<Post> SortingBySearch(string searchFilter, IQueryable<Post> queryablePosts)
        {
            if (!string.IsNullOrWhiteSpace(searchFilter))
            {
                queryablePosts = queryablePosts
                     .Where(p => p.Title.ToLower().Contains(searchFilter.ToLower())
                     || p.Category.Name.ToLower().Contains(searchFilter.ToLower()));
            }

            return queryablePosts;
        }

        private IQueryable<Post> SortingByFilter(PostSorting query, IQueryable<Post> queryablePosts)
        {
            queryablePosts = query switch
            {
                PostSorting.TagsCount => queryablePosts.OrderByDescending(t => t.Tags.Count),
                PostSorting.RatingCount => queryablePosts.OrderByDescending(r => r.Rating),
                PostSorting.ReplyCount=>queryablePosts.OrderByDescending(r=>r.Replies.Count),
                PostSorting.DateCreated or _ => queryablePosts.OrderByDescending(d => d.Id)
            };
            return queryablePosts;
        }
        private string LatestActivity(string currentTime, string postTime)
        {
            var currentTimeParsed=DateTime.Parse(currentTime);
            var postTimeParsed=DateTime.Parse(postTime);

            var activity = currentTimeParsed - postTimeParsed;
            var daysFromNow = activity.Days;
            var hoursFromNow = activity.Hours;
            var minutesFromNow = activity.Minutes;

            var result=String.Empty;

            if (daysFromNow != 0)
            {
                result = $"{daysFromNow}d";
            }
            else
            {
                result = hoursFromNow == 0 ? $"{minutesFromNow}min" : $"{hoursFromNow}h";
            }

            return result;
        }

        public async Task<IEnumerable<TModel>> GetFourRandomPosts<TModel>()
        {

            var totalPosts = await data.Posts
                            .AsNoTracking()
                            .Where(x => !x.IsDeleted)
                            .OrderBy(x => Guid.NewGuid())
                            .Take(4)
                            .ProjectTo<TModel>(mapper.ConfigurationProvider)
                            .ToListAsync();

            return totalPosts;
        }

        public async Task<int> GetPostsSearchCountAsync(string searchFilter = null)
        {
            var queryablePosts = data.Posts.Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchFilter))
            {
                queryablePosts = queryablePosts
                        .Where(p => p.Title.ToLower().Contains(searchFilter.ToLower())
                        || p.Category.Name.ToLower().Contains(searchFilter.ToLower()));

            }
            var countOfPosts = await queryablePosts.CountAsync();

            return countOfPosts;
        }

        public async Task<IList<TModel>> GetAllPostsByTagIdAsync<TModel>(int tagId, int skip = 0, int take = 0)
            => await data.Posts
                .AsNoTracking()
                .Where(p => !p.IsDeleted &&
                    p.Tags.Select(t => t.Id).Contains(tagId))
                .Skip(skip).Take(take)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<TModel> GetByIdAsync<TModel>(int id)
            => await data.Posts
                .AsNoTracking()
                .Where(p => p.Id == id && !p.IsDeleted)
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<string> GetPostAuthorIdAsync<TModel>(int id)
            => await data.Posts
                 .AsNoTracking()
                 .Where(p => p.Id == id && !p.IsDeleted)
                 .Select(p => p.AuthorId)
                 .FirstOrDefaultAsync();

        public async Task EditPostAsync(
            int id, 
            string title,
            string content,
            int categoryId, 
            IEnumerable<int> tagIds)
        {
            var post=await GetByIdAsync(id);

            post.Tags.Clear();

            post.Title = title;
            post.Content = content;
            post.CategoryId = categoryId;

            await AddTagsAsync(post,tagIds);
            await data.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            var post= await GetByIdAsync(id);

            post.IsDeleted= true;
            post.DeletedOn = dateTimeProvider.Now();

            await data.SaveChangesAsync();

        }

        public async Task<IList<TModel>> GetAllPostsByCategoryIdAsync<TModel>(int categoryId, int skip = 0, int take = 0)
          => await data.Posts
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.CategoryId==categoryId)
                .Skip(skip).Take(take)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task ViewAsync(int id)
        {
            var post = await GetByIdAsync(id);

            post.Rating++;

            await data.SaveChangesAsync();
        }

        public async Task<string> GetLatestPostActivityAsync(int id)
        {
            var latestReplyOfPost=await data.Posts
                .Where(p=> p.Id==id && !p.IsDeleted)
                .SelectMany(p=>p.Replies)
                .Where(r=>!r.IsDeleted)
                .OrderByDescending(r=>r.CreatedOn)
                .FirstOrDefaultAsync(r=>r.PostId==id);

            if (latestReplyOfPost!=null)
            {
                return LatestActivity(dateTimeProvider.Now(), latestReplyOfPost.CreatedOn);
            }

            var post=await GetByIdAsync(id);

            return LatestActivity(dateTimeProvider.Now(), post.CreatedOn);
        }
    }
}
