using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using YourMoviesForum.Data.Models;

namespace YourMoviesForum.Services.Data.Posts
{
    public class PostService : IPostService
    {
        private readonly YourMoviesDbContext data;

        public PostService(YourMoviesDbContext data)
        {
            this.data = data;
        }
        public async Task<int> CreateAsync(string title, string content, string ImageUrl, int categoryId,IEnumerable<int>tagIds)
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
    }
}
