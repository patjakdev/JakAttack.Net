using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JakAttack.ViewModels
{
    public class Helpers
    {
        public static IHtmlString TextileToHtml(string textileInput)
        {
            return MvcHtmlString.Create(Textile.TextileFormatter.FormatString(textileInput));
        }
    }
}