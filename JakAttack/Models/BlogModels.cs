using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace JakAttack.Models.Blog
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        // This field contains Textile-formatted content. Eventually, this should be allowed to contain HTML as well, but there are
        // sanitization issues that I don't want to deal with yet.
        public string Content { get; set; }

        public DateTime DatePosted { get; set; }

        public DateTime? DateLastModified { get; set; }

        public virtual User Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class Comment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Post")]
        public string Post { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Posted On")]
        public string DatePosted { get; set; }

        public virtual Post BlogPost { get; set; }
    }
}