using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JakAttack.ViewModels.Blog
{
    public class DisplayPostViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DatePosted { get; set; }

        public DateTime? DateLastModified { get; set; }

        public string AuthorFullName { get; set; }
    }

    public class CreateOrModifyPostViewModel
    {
        // Only used by Edit view - since it's not actually passed back by the Create view, the RequiredAttribute doesn't apply
        [Required]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}