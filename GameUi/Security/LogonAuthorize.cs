/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpaceTraffic.GameUi.Security
{
    /// <summary>
    /// LogonAuthorize is filter that derives from AuthorizeAttribute, which runs Authorize filter on every controller except controllers with [AllowAnonymous] attribute.
    /// This filter also handles Unauthorized requests -> redirects to login page.
    /// This filter must be registered in global.asax file as global filter to work globaly.
    /// </summary>
    public sealed class LogonAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">AuthorizationContext</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                base.OnAuthorization(filterContext);
            }
        }

        /// <summary>
        /// HTTP 401 result for unauthorized request
        /// </summary>
        private class Http401Result : ActionResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.HttpContext.Response.Write("Vaše přihlášení vypršelo.");
                context.HttpContext.Response.End();
            }
        }

        /// <summary>
        /// Handles unauthorized requests
        /// </summary>
        /// <param name="filterContext">Authorization context</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new Http401Result();
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }

    /// <summary>
    /// Attribute, which is used for action methods or controllers that needs to opt out of authorization (e.g. LogOn, Register, LostPassword)
    /// Warning: Don't use this attribute on whole AbstractController, because it's base controller for all other controllers in application.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowAnonymousAttribute : Attribute { }
}