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
        [Display(Name = "Blog Post ID")]
        public int BlogPostId { get; set; }

        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Posted On")]
        public string DatePosted { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Last Modified On")]
        public string DateLastModified { get; set; }

        public virtual ICollection<BlogPostComment> Comments { get; set; }
    }

    public class BlogPostComment
    {
        [Required]
        [Display(Name = "Comment ID")]
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