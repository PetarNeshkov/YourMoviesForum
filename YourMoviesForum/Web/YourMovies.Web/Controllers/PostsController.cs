﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourMovies.Web.Infrastructure;
using YourMoviesForum;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Categories;
using YourMoviesForum.Services.Data.Tags;
using YourMoviesForum.Web.InputModels;
using YourMoviesForum.Web.InputModels.Posts;

using static YourMoviesForum.Common.GlobalConstants.Administrator;


namespace YourMovies.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly YourMoviesDbContext data;
        private readonly IPostService postService;
        private readonly ICategoryService categoryService;
        private readonly ITagService tagService;

        public PostsController(
            YourMoviesDbContext data,
            IPostService postService,
            ICategoryService categoryService,
            ITagService tagService)
        {
            this.data = data;
            this.postService = postService;
            this.categoryService = categoryService;
            this.tagService = tagService;
        }

        [Authorize]
        public async Task<IActionResult> Add() => View(new AddPostFormModel
        {
            Tags = await tagService.GetAllTagsAsync<PostTagViewModel>(),
            Categories = await categoryService.GetAllCategoriesAsync<PostCategoryViewModel>()
        });
       

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddPostFormModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Tags = await tagService.GetAllTagsAsync<PostTagViewModel>();
                input.Categories = await categoryService.GetAllCategoriesAsync<PostCategoryViewModel>();

                return View(input);
            }

            await postService.CreatePostAsync(
                input.Title,
                input.SanitizedContent,
                input.CategoryId,
                input.TagIds,
                this.User.Id());
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var post=await postService.GetByIdAsync<PostDetailsViewModel>(id);

            if (post==null)
            {
                return NotFound();
            }

            //if (post.Author.Id != this.User.Id() || User.IsInRole(AdministratorUsername))
            //{
            //    return this.Unauthorized();
            //}

            post.Tags = await tagService.GetAllPostsByIdAsync<PostTagViewModel>(id);

            return View(post);
        }
    }
}
