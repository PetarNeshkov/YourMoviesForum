using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Web.InputModels.Posts;

using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace YourMoviesForum.Services.Data.Posts
{
    public class PostService : IPostService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;

        public PostService(YourMoviesDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }
        public async Task<int> CreatePostAsync(string title, string content,int categoryId, IEnumerable<int> tagIds)
        {
            var post = new Post
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
            };

            await data.Posts.AddAsync(post);
            await data.SaveChangesAsync();
            await AddTagsAsync(post.Id, tagIds);

            return post.Id;
        }

        private async Task<Post> GetByIdAsync(int id)
            => await data.Posts.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        private async Task AddTagsAsync(int id, IEnumerable<int> tagIds)
        {
            var post = await GetByIdAsync(id);

            foreach (var tagId in tagIds)
            {
                var currentName = data.Tags
                    .Where(x => x.Id == tagId)
                    .Select(t => t.Name)
                    .FirstOrDefault();
                post.Tags.Add(new Tag
                {
                    Id = tagId,
                    Name = currentName
                });
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
                PostSorting.TagsCount => queryablePosts.OrderByDescending(t => t.Tags.Count()),
                PostSorting.DateCreated or _ => queryablePosts.OrderByDescending(c => c.Id)
            };
            return queryablePosts;
        }

        public async Task<IEnumerable<TModel>> GetThreeRandomPosts<TModel>()
        {

            var totalPosts = await data.Posts
                            .Where(x => !x.IsDeleted)
                            .OrderBy(x => Guid.NewGuid())
                            .Take(3)
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
    }
}
