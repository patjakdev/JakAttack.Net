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
        private JakAttackContext context = new JakAttackContext();

        //
        // GET: /BlogPosts/

        public ViewResult Index()
        {
            return View(context.BlogPosts.ToList());
        }

        //
        // GET: /BlogPosts/Details/5

        public ViewResult Details(int id)
        {
            BlogPost blogpost = context.BlogPosts.Single(x => x.BlogPostId == id);
            return View(blogpost);
        }

        //
        // GET: /BlogPosts/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /BlogPosts/Create

        [HttpPost]
        public ActionResult Create(BlogPost blogpost)
        {
            if (ModelState.IsValid)
            {
                context.BlogPosts.Add(blogpost);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(blogpost);
        }
        
        //
        // GET: /BlogPosts/Edit/5
 
        public ActionResult Edit(int id)
        {
            BlogPost blogpost = context.BlogPosts.Single(x => x.BlogPostId == id);
            return View(blogpost);
        }

        //
        // POST: /BlogPosts/Edit/5

        [HttpPost]
        public ActionResult Edit(BlogPost blogpost)
        {
            if (ModelState.IsValid)
            {
                context.Entry(blogpost).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogpost);
        }

        //
        // GET: /BlogPosts/Delete/5
 
        public ActionResult Delete(int id)
        {
            BlogPost blogpost = context.BlogPosts.Single(x => x.BlogPostId == id);
            return View(blogpost);
        }

        //
        // POST: /BlogPosts/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogpost = context.BlogPosts.Single(x => x.BlogPostId == id);
            context.BlogPosts.Remove(blogpost);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}