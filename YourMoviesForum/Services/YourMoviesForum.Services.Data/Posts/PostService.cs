using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using YourMoviesForum.Data.Models;

using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace YourMoviesForum.Services.Data.Posts
{
    public class PostService : IPostService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;

        public PostService(YourMoviesDbContext data,IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }
        public async Task<int> CreatePostAsync(string title, string content, string ImageUrl, int categoryId,IEnumerable<int>tagIds)
        {
            var post = new Post
            {
                Title = title,
                Content = content,
                ImageUrl = ImageUrl,
                CategoryId = categoryId,
            };

            await data.Posts.AddAsync(post);
            await data.SaveChangesAsync();
            await AddTagsAsync(post.Id,tagIds);

            return post.Id;
        }

        private async Task<Post> GetByIdAsync(int id)
            => await data.Posts.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        private async Task AddTagsAsync(int id,IEnumerable<int>tagIds)
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

        public async Task<IEnumerable<TModel>> GetAllPostsAsync<TModel>()
        {
            var queryablePosts = data.Posts
                 .AsNoTracking()
                 .OrderByDescending(d => d.CreatedOn)
                 .Where(p => !p.IsDeleted);

            var posts = await queryablePosts
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            return posts;
        }

        public IEnumerable<TModel> GetThreeRandomPosts<TModel>()
        {

            var totalPosts = data.Posts
                            .Where(x => !x.IsDeleted)
                            .OrderBy(x=> Guid.NewGuid())
                            .Take(3)
                            .ProjectTo<TModel>(mapper.ConfigurationProvider)
                            .ToList();

            return totalPosts;
        }
    }
}
