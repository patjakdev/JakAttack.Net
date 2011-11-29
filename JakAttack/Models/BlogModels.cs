using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace JakAttack.Models
{
    public class BlogPost
    {
        [Required]
        public int BlogPostId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Posted On")]
        public DateTime DatePosted { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Last Modified On")]
        public DateTime DateLastModified { get; set; }

        public virtual User Author { get; set; }

        public virtual ICollection<BlogPostComment> Comments { get; set; }
    }

    public class BlogPostComment
    {
        [Required]
        public int BlogPostCommentId { get; set; }

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

        public virtual BlogPost BlogPost { get; set; }
    }
}