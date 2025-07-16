using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.ActionsResults;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Common.Models;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Web.Website.Models;
using Umbraco.Extensions;


namespace MahakaleswarumUjjayini.Controller
{
    public class LoginController : SurfaceController
    {
        private readonly IMemberSignInManager _signInManager;
        [ActivatorUtilitiesConstructor]
        public LoginController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        IMemberSignInManager signInManager,
        IMemberManager memberManager,
        ITwoFactorLoginService twoFactorLoginService)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _signInManager = signInManager;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateUmbracoFormRouteString]
        public async Task<IActionResult> HandleLogin([Bind(Prefix = "loginModel")] LoginModel model)
        {
            if (ModelState.IsValid == false)
            {
                return Redirect("/login");
            }



            // Sign the user in with username/password, this also gives a chance for developers to
            // custom verify the credentials and auto-link user accounts with a custom IBackOfficePasswordChecker
            var result = await _signInManager.PasswordSignInAsync(
                model.Username, model.Password, model.RememberMe, true);

            if (result.Succeeded)
            {
                return Redirect("/");
                //TempData["LoginSuccess"] = true;

            }

            return CurrentUmbracoPage();
        }

        [HttpGet]
        public IActionResult RenderLoginForm()
        {
            return PartialView("LoginForm", new LoginModel());
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
        /// <summary>
        ///     We pass in values via encrypted route values so they cannot be tampered with and merge them into the model for use
        /// </summary>
        /// <param name="model"></param>
        private void MergeRouteValuesToModel(LoginModel model)
        {
            if (RouteData.Values.TryGetValue(nameof(LoginModel.RedirectUrl), out var redirectUrl) && redirectUrl != null)
            {
                model.RedirectUrl = redirectUrl.ToString();
            }
        }
    }
}