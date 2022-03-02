using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Replies;

namespace YourMovies.Web.Components
{
    [ViewComponent(Name = "NestedReply")]
    public class NestedReplyViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(int? parentId,
            IEnumerable<PostRepliesDetailsViewModel> replies)
        {
            var viewModel = new NestedReplies
            {
                ParentId = parentId,
                Replies = replies
            };

            return this.View(viewModel);
        }
    }
}
