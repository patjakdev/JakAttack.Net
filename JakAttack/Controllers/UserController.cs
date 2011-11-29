using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JakAttack.Models;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using System.Web.Security;
using DotNetOpenAuth.OpenId.Extensions.OAuth;

namespace JakAttack.Controllers
{
    public class UserController : Controller
    {
        private static readonly OpenIdRelyingParty s_relyingParty = new OpenIdRelyingParty();
        private JakAttackContext _context = new JakAttackContext();

        static UserController()
        {
            s_relyingParty.DiscoveryServices.Insert(0, new HostMetaDiscoveryService { UseGoogleHostedHostMeta = true });
        }

        public ActionResult LogIn(string ReturnUrl)
        {
            var authResponse = s_relyingParty.GetResponse();
            if (authResponse == null)
            {
                Identifier id;
                if (Identifier.TryParse("jakattack.net", out id))
                {
                    var req = s_relyingParty.CreateRequest(id);
                    req.Mode = AuthenticationRequestMode.Setup;
                    var fetch = new FetchRequest();
                    fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, true));
                    fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.First, true));
                    fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Last, true));
                    req.AddExtension(fetch);

                    return req.RedirectingResponse.AsActionResult();
                }
            }
            else
            {
                switch (authResponse.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var fetch = authResponse.GetExtension<FetchResponse>();
                        FormsAuthentication.SetAuthCookie(authResponse.ClaimedIdentifier, false);
                        var user = new User 
                            { 
                                ClaimedId   = authResponse.ClaimedIdentifier, 
                                Email       = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email),
                                FirstName   = fetch.GetAttributeValue(WellKnownAttributes.Name.First),
                                LastName    = fetch.GetAttributeValue(WellKnownAttributes.Name.Last)
                            };
                        if (_context.Users.Find(user.ClaimedId) == null)
                        {
                            _context.Users.Add(user);
                            _context.SaveChanges();
                        }
                        return RedirectToAction("Index", "Home");
                    case AuthenticationStatus.Canceled:
                        break;
                    case AuthenticationStatus.Failed:
                        break;
                }
            }
            return new EmptyResult();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
