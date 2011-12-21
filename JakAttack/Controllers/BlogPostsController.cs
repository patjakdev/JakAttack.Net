using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JakAttack.Models;
using JakAttack.Models.Blog;
using JakAttack.ViewModels.Blog;
using AutoMapper;

namespace JakAttack.Controllers
{   
    public class BlogPostsController : Controller
    {
        private JakAttackContext _context = new JakAttackContext();

        //
        // GET: /BlogPosts/

        public ViewResult Index()
        {
            return View(_context.BlogPosts.OrderByDescending(item => item.DatePosted).ToList().Select(m => Mapper.Map<DisplayPostViewModel>(m)));
        }

        //
        // GET: /BlogPosts/Details/5

        public ViewResult Details(int id)
        {
            Post blogpost = _context.BlogPosts.Single(x => x.Id == id);
            return View(Mapper.Map<DisplayPostViewModel>(blogpost));
        }

        //
        // GET: /BlogPosts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /BlogPosts/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(CreateOrModifyPostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Post post = new Post();
                // It would be better to use ValueInjector or something to get the viewModel fields into the actual database model.
                // Automapper doesn't support unflattening yet.
                post.Content = viewModel.Content;
                post.Title = viewModel.Title;
                post.DatePosted = DateTime.Now.ToUniversalTime();
                post.Author = _context.Users.Single(u => u.ClaimedId == User.Identity.Name);

                _context.BlogPosts.Add(post);
                _context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(viewModel);
        }
        
        //
        // GET: /BlogPosts/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Post blogpost = _context.BlogPosts.Single(x => x.Id == id);
            return View(Mapper.Map<CreateOrModifyPostViewModel>(blogpost));
        }

        //
        // POST: /BlogPosts/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(CreateOrModifyPostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Post post = _context.BlogPosts.Single(x => x.Id == viewModel.Id);
                post.DateLastModified = DateTime.Now;
                // It would be better to use ValueInjector or something to get the viewModel fields into the actual database model. 
                // Automapper doesn't support unflattening yet.
                post.Content = viewModel.Content;
                post.Title = viewModel.Title;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        //
        // GET: /BlogPosts/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Post blogpost = _context.BlogPosts.Single(x => x.Id == id);
            return View(Mapper.Map<DisplayPostViewModel>(blogpost));
        }

        //
        // POST: /BlogPosts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Post blogpost = _context.BlogPosts.Single(x => x.Id == id);
            _context.BlogPosts.Remove(blogpost);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}