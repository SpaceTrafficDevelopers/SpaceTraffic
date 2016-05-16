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

namespace SpaceTraffic.GameUi.Extensions
{
	internal static class FlashMessageExtensions
	{
		public static ActionResult Error(this ActionResult result, string message)
		{
			CreateCookieWithFlashMessage(Notification.Error, message);
			return result;
		}

		public static ActionResult Warning(this ActionResult result, string message)
		{
			CreateCookieWithFlashMessage(Notification.Warning, message);
			return result;
		}

		public static ActionResult Success(this ActionResult result, string message)
		{
			CreateCookieWithFlashMessage(Notification.Success, message);
			return result;
		}

		public static ActionResult Information(this ActionResult result, string message)
		{
			CreateCookieWithFlashMessage(Notification.Info, message);
			return result;
		}

		public static PartialViewResult Error(this PartialViewResult result, string message)
		{
			CreateCookieWithFlashMessage(Notification.Error, message);
			return result;
		}

		public static PartialViewResult Warning(this PartialViewResult result, string message)
		{
			CreateCookieWithFlashMessage(Notification.Warning, message);
			return result;
		}

		public static PartialViewResult Success(this PartialViewResult result, string message)
		{
			CreateCookieWithFlashMessage(Notification.Success, message);
			return result;
		}

		public static PartialViewResult Information(this PartialViewResult result, string message)
		{
			CreateCookieWithFlashMessage(Notification.Info, message);
			return result;
		}

        public static JavaScriptResult Error(this JavaScriptResult result, string message)
        {
            CreateCookieWithFlashMessage(Notification.Error, message);
            return result;
        }

        public static JavaScriptResult Warning(this JavaScriptResult result, string message)
        {
            CreateCookieWithFlashMessage(Notification.Warning, message);
            return result;
        }

        public static JavaScriptResult Success(this JavaScriptResult result, string message)
        {
            CreateCookieWithFlashMessage(Notification.Success, message);
            return result;
        }

        public static JavaScriptResult Information(this JavaScriptResult result, string message)
        {
            CreateCookieWithFlashMessage(Notification.Info, message);
            return result;
        }

        private static void CreateCookieWithFlashMessage(Notification notification, string message)
		{
			HttpContext.Current.Response.Cookies.Add(new HttpCookie(string.Format("Flash.{0}", notification), message) { Path = "/" });
		}

		private enum Notification
		{
			Error,
			Warning,
			Success,
			Info
		}
	}
	
}