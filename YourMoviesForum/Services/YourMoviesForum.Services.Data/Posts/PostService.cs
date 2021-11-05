using Microsoft.EntityFrameworkCore;

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
        public async Task<string> CreateAsync(string title, string content, string ImageUrl, string categoryId)
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

            return post.Id;
        }

        private async Task<Post> GetByIdAsync(string id)
            => await data.Posts.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }
}
