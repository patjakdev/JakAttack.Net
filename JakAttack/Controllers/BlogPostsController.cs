using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JakAttack.Models;

namespace JakAttack.Controllers
{   
    public class BlogPostsController : Controller
    {
        private JakAttackContext _context = new JakAttackContext();

        //
        // GET: /BlogPosts/

        public ViewResult Index()
        {
            return View(_context.BlogPosts.OrderByDescending(item => item.DateLastModified).ToList());
        }

        //
        // GET: /BlogPosts/Details/5

        public ViewResult Details(int id)
        {
            BlogPost blogpost = _context.BlogPosts.Single(x => x.BlogPostId == id);
            return View(blogpost);
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
        public ActionResult Create(BlogPost blogpost)
        {
            blogpost.DatePosted = DateTime.Now.ToUniversalTime();
            blogpost.DateLastModified = blogpost.DatePosted;

            if (ModelState.IsValid)
            {
                _context.BlogPosts.Add(blogpost);
                _context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(blogpost);
        }
        
        //
        // GET: /BlogPosts/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            BlogPost blogpost = _context.BlogPosts.Single(x => x.BlogPostId == id);
            return View(blogpost);
        }

        //
        // POST: /BlogPosts/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(BlogPost blogpost)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(blogpost).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogpost);
        }

        //
        // GET: /BlogPosts/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            BlogPost blogpost = _context.BlogPosts.Single(x => x.BlogPostId == id);
            return View(blogpost);
        }

        //
        // POST: /BlogPosts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogpost = _context.BlogPosts.Single(x => x.BlogPostId == id);
            _context.BlogPosts.Remove(blogpost);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}