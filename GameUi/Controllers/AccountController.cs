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
using System.Web.Routing;
using System.Web.Security;
using SpaceTraffic.GameUi.Models;
using SpaceTraffic.Utils.Debugging;
using SpaceTraffic.GameUi.Security;
using SpaceTraffic.GameUi.Extensions;

namespace SpaceTraffic.GameUi.Controllers
{
    //RequireHttps]
    public class AccountController : AbstractController
    {

        //
        // GET: /Account/LogOn
        [AllowAnonymous]
        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    string userData = Membership.GetUser(model.UserName).ProviderUserKey.ToString();

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), model.RememberMe, userData, FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);

                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    faCookie.HttpOnly = true;
                    faCookie.Secure = FormsAuthentication.RequireSSL;
                    faCookie.Path = FormsAuthentication.FormsCookiePath;
                    faCookie.Domain = FormsAuthentication.CookieDomain;

                    if (authTicket.IsPersistent)
                        faCookie.Expires = authTicket.Expiration;

                    Response.Cookies.Add(faCookie);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    string userLower = model.UserName.ToLower();
                    if (GSClient.AccountService.AccountUsernameExists(userLower))
                    {
                        int id = GSClient.AccountService.GetAccountInfoByUserName(userLower).PlayerId;
                        if (GSClient.PlayerService.GetPlayer(id).IsEmailConfirmed)
                        {
                            ModelState.AddModelError("", "Špatné jméno nebo heslo.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Účet není aktivován.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Špatné jméno nebo heslo.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clears session cookie
            HttpCookie sessionCookie = new HttpCookie("ASP.NET_SessionId", string.Empty);
            sessionCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(sessionCookie);

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            DebugEx.WriteLineF("Registration begin");
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {

            if (ModelState.IsValid)
            {

                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    return RedirectToAction("RegistrationSuccessful", "Account");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            return View(model);
        }

        //
        // GET: /Account/RegistrationSuccessful
        [AllowAnonymous]
        public ActionResult RegistrationSuccessful()
        {
            return View();
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        //
        // POST: /Account/ChangePassword
        /// <summary>
        /// Changes password for currently signed in player after password verification.
        /// If successful, redirects user to Settings screen, otherwise displays error message to user.
        /// </summary>
        /// <param name="model">ChangePasswordModel model</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = Membership.Provider.ChangePassword(getCurrentPlayerName(), model.OldPassword, model.NewPassword);

                if (status)
                {
                    if (Request.IsAjaxRequest())
                    {
                        //return JavaScript("document.location.replace('" + Url.Action("Settings", "Profile", new { Area = "Game" }) + "');").Success("Heslo bylo změněno.");
                        //return JavaScript("document.location.replace('" + Url.Action("Index", "Default", new { Area = "Game" }) + "');").Success("Heslo bylo změněno.");
                        return JavaScript("$('#cancelbutton').click();").Success("Heslo bylo změněno.");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Default", new { Area = "Game" });
                    }
                }
                else
                {
                    return ModelErrorUsingJS("Špatně zadané heslo.");
                }
            }

            return PartialView(model);
        }

        //
        // GET: /Account/DeleteAccount

        [Authorize]
        public ActionResult DeleteAccount()
        {
            return PartialView();
        }

        //
        // POST: /Account/DeleteAccount
        /// <summary>
        /// Deletes currently signed on player after password verification.
        /// If successful, redirects user to LogOn screen, otherwise displays error message to user.
        /// </summary>
        /// <param name="model">DeleteAccountModel model</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccount(DeleteAccountModel model)
        {
            //DebugEx.WriteLineF("Delete Account clicked");

            if (ModelState.IsValid)
            {
                string playerName = getCurrentPlayerName();

                if (Membership.ValidateUser(playerName, model.Password))
                {
                    bool status = Membership.DeleteUser(playerName, true);

                    if (status)
                    {
                        return RedirectToAction("LogOff").Success("Účet byl smazán.");
                    }
                    else
                    {
                        return ModelErrorUsingJS("Nastala neznámá chyba. Prosím zkuste to znovu, nebo kontaktujte tým Space Traffic.");
                    }
                }
                else
                {
                    return ModelErrorUsingJS("Špatně zadané heslo.");
                }
                
            }

            return PartialView(model);
        }

        //
        // GET: /Account/LostPassword
        [AllowAnonymous]
        public ActionResult LostPassword()
        {
            return View();
        }

        /// <summary>
        /// Sends reset email to user after username and email verification.
        /// If successful, redirects user to LogOn screen, otherwise displays error message to user.
        /// </summary>
        /// <param name="model">LostPasswordModel model</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(LostPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                string playerName = model.UserName.ToLower();
                bool status = GSClient.AccountService.AccountUsernameExists(playerName);

                if(status)
                {
                    Entities.Player player = GSClient.PlayerService.GetPlayer(GSClient.AccountService.GetAccountInfoByUserName(playerName).PlayerId);

                    if (player.Email == model.Email.ToLower())
                    {
                        if(Membership.Provider.ResetPassword(playerName, string.Empty) == null)
                        {
                            ModelState.AddModelError("", "Nastala neznámá chyba.");
                        }
                        else
                        {
                            return RedirectToAction("LogOn", "Account").Success("Email pro obnovu hesla byl odeslán.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Špatné jméno nebo email.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Špatné jméno nebo email.");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Activates user with token.
        /// If invalid token si given, than shows error message
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ActivationToken()
        {
            string[] tokens = Request.QueryString.GetValues("Token");
            if (tokens != null && tokens.Length == 1)
            {
                string token = tokens[0];

                if (GSClient.AccountService.AccountTokenExists(token))
                {
                    int id = GSClient.AccountService.GetAccountInfoByToken(token).PlayerId;
                    Entities.Player player = GSClient.PlayerService.GetPlayer(id);

                    if (player.IsEmailConfirmed)
                    {
                        return RedirectToAction("LogOn", "Account").Warning("Účet je již aktivován");
                    }

                    if ((DateTime.Now - player.AddedDate) > new TimeSpan(48, 0, 0))
                    {
                        Membership.DeleteUser(player.PlayerName);

                        return RedirectToAction("LogOn", "Account").Error("Účet byl odstraněn. Propásli jste aktivační lhůtu (48 hodin).");
                    }

                    player.IsEmailConfirmed = true;

                    if (GSClient.AccountService.UpdatePlayer(player))
                    {
                        return RedirectToAction("LogOn", "Account").Success("Účet byl úspěšně aktivován.");
                    }
                    else
                    {
                        return RedirectToAction("LogOn", "Account").Error("Nastala neznámá chyba.");
                    }
                }
                else
                {
                    return RedirectToAction("LogOn", "Account").Error("Nesprávný aktivační odkaz");
                }
            }
            else
            {
                return RedirectToAction("LogOn", "Account").Error("Nesprávný aktivační odkaz");
            }
        }


        /// <summary>
        /// This is javascript workaround for ModelState.AddModelError.
        /// There must be <div></div> with validation-summary-errors class for proper function inside of view.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <returns></returns>
        private JavaScriptResult ModelErrorUsingJS(string message)
        {
            return JavaScript("$(\".validation-summary-errors\").html(\"<ul><li>" + message +"</li></ul>\");");
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Toto jméno je již obsazené.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Účet s tímto e-mailem již existuje.";

                case MembershipCreateStatus.ProviderError:
                    return "Došlo k chybě při registraci. Prosím zkuste to znovu.";

                default:
                    return "Nastala neznámá chyba. Prosím zkuste to znovu, nebo kontaktujte tým Space Traffic.";
            }
        }
        #endregion
    }
}
