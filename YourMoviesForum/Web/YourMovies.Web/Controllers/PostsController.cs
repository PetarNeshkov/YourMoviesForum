﻿using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using YourMoviesForum;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Categories;
using YourMoviesForum.Services.Data.Tags;
using YourMoviesForum.Web.InputModels;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Tags;

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

        public async Task<IActionResult> Add() => View(new AddPostFormModel
        {
            Tags = await tagService.GetAllTagsAsync<PostTagViewModel>(),
            Categories = await categoryService.GetAllCategoriesAsync<PostCategoryViewModel>()
        });
       

        [HttpPost]
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
                input.Content,
                input.CategoryId,
                input.TagIds);


            return RedirectToAction("Index", "Home");
        }
    }
}
