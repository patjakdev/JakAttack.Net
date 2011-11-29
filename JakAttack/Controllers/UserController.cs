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
using DotNetOpenAuth.OpenId.Extensions.ProviderAuthenticationPolicy;
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

                    // Collect some information on the user with the authentication response
                    var fetch = new FetchRequest();
                    fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, true));
                    fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.First, true));
                    fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Last, true));
                    req.AddExtension(fetch);

                    // Force the user to enter their password by setting the maximum authentication age to 0
                    var pape = new PolicyRequest();
                    pape.MaximumAuthenticationAge = new TimeSpan(0);
                    req.AddExtension(pape);

                    // Add a callback argument indicating where the login request came from. This is used to redirect the user when
                    // the authentication response is received.
                    req.AddCallbackArguments("ReturnUrl", ReturnUrl);

                    return req.RedirectingResponse.AsActionResult();
                }
            }
            else
            {
                switch (authResponse.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        FormsAuthentication.SetAuthCookie(authResponse.ClaimedIdentifier, false);

                        var fetch = authResponse.GetExtension<FetchResponse>();
                        var user = new User 
                            { 
                                ClaimedId   = authResponse.ClaimedIdentifier, 
                                Email       = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email),
                                FirstName   = fetch.GetAttributeValue(WellKnownAttributes.Name.First),
                                LastName    = fetch.GetAttributeValue(WellKnownAttributes.Name.Last)
                            };
                        if (_context.Users.Select(u => u.ClaimedId == user.ClaimedId).Count() == 0)
                        {
                            _context.Users.Add(user);
                            _context.SaveChanges();
                        }
                        return Redirect(authResponse.GetCallbackArgument("ReturnUrl"));
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
