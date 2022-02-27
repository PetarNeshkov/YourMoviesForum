using Microsoft.AspNetCore.Mvc;
using YourMoviesForum.Web.InputModels.Replies;

namespace YourMovies.Web.Components
{
    [ViewComponent(Name = "CreateReply")]
    public class ReplyCreateViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(int? parentId, int postId)
        {
            var viewModel = new ReplyCreateInputModel
            {
                ParentId = parentId,
                PostId = postId
            };

            return this.View(viewModel);
        }
    }
}
