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
using SpaceTraffic.GameUi.Models.Ui;
using SpaceTraffic.Entities;
using System.Collections;
using SpaceTraffic.GameUi.Extensions;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class ProfileController : TabsControllerBase
    {
        protected override void BuildTabs()
        {
            this.Tabs.AddTab("Overview", text: "Profil", title: "Profil", partialViewName: "_Overview");
            this.Tabs.AddTab("Settings", text: "Nastavení" ,title: "Nastavení", partialViewName: "_Settings", partialViewModel: new Models.ProfileSettingsModel());
            this.Tabs.AddTab("Achievements", text: "Úspěchy", title: "Úspěchy", partialViewName: "_AchievementsList");
        }

        public ActionResult Index()
        {
			return PartialView(INDEX_VIEW);
        }

        /// <summary>
        /// Gets user data from database and displays them to user.
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Overview()
        {
			Player player = GSClient.PlayerService.GetPlayer(getCurrentPlayerId());
			ExperienceLevels globalExpiriencesLevels = GSClient.AchievementsService.GetExperienceLevels();
			TLevel currentLevel = globalExpiriencesLevels.GetLevel(player.ExperienceLevel);
			TLevel nextLevel = globalExpiriencesLevels.GetLevel(currentLevel.LevelID + 1);
			
			var tabView = GetTabView("Overview");
            //player entity
			tabView.ViewBag.player = player;

            //player current level
			tabView.ViewBag.currentLevel = currentLevel;

            //exp to percent for exp_ring
            float requiredExp = (currentLevel.LevelID > 0) ? currentLevel.RequiredXP : nextLevel.RequiredXP;
            tabView.ViewBag.expInPercent = (nextLevel == null) ? 100 : (int)(((player.Experiences - currentLevel.RequiredXP) / requiredExp) * 100);

            //exp to next level
            tabView.ViewBag.expToNextL = (nextLevel == null) ? 0 : (nextLevel.RequiredXP - player.Experiences);

            //avatar image
            tabView.ViewBag.avatarID = currentLevel.LevelID;

            //player game age 
            tabView.ViewBag.playerGameAge = FormatInGameAge(DateTime.Now - player.AddedDate);

            return tabView;
        }

        /// <summary>
        /// Gets user settings from database and displays them to user.
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Settings()
        {
            Models.ProfileSettingsModel model = new Models.ProfileSettingsModel();
            Player player = GSClient.PlayerService.GetPlayer(getCurrentPlayerId());

            model.Email = player.Email;
            model.RememberMe = player.StayLogedIn;
            model.SendGameInfo = player.SendInGameInfo;
            model.SendNews = player.SendNewsletter;

            return GetTabView("Settings", model);
        }

        /// <summary>
        /// Saves user settings to database.
        /// If successful, displays success message to user, otherwise displays error message to user.
        /// </summary>
        /// <param name="model">ProfileSettingsModel model</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(Models.ProfileSettingsModel model)
        {
            if(ModelState.IsValid)
            {
                //SpaceTraffic.Utils.Debugging.DebugEx.WriteLineF("PlayerUpdate begin: rem:" + model.RememberMe + ", GInfo: " + model.SendGameInfo + ", SNews: " + model.SendNews);
                Player player = GSClient.PlayerService.GetPlayer(getCurrentPlayerId());

                player.StayLogedIn = model.RememberMe;
                player.SendInGameInfo = model.SendGameInfo;
                player.SendNewsletter = model.SendNews;

                bool state = GSClient.AccountService.UpdatePlayer(player);

                if(state)
                {
                    return JavaScript("$('a[title=\"Nastavení\"]').click();").Success("Nastavení bylo uloženo.");
                }
                else
                {
                    return JavaScript("$('a[title=\"Nastavení\"]').click();").Error("Při ukládání nastala chyba.");
                }
            }

            return GetTabView("Settings", model);
        }

        public PartialViewResult Achievements()
        {
			Player player = GSClient.PlayerService.GetPlayer(getCurrentPlayerId());
			Achievements allAchievements = GSClient.AchievementsService.GetAchievements();
			List<EarnedAchievement> earnedAchievements = GSClient.AchievementsService.GetEarnedAchievements(player.PlayerId);
			List<TAchievement> earnedDefinition = new List<TAchievement>();//earned achievemenets from xml
			foreach (EarnedAchievement earnedAchv in earnedAchievements)
			{
				earnedDefinition.Add(allAchievements.GetAchievement(earnedAchv.AchievementId));
			}
			List<TAchievement> lockedAchievements = allAchievements.Items.Except(earnedDefinition).ToList();

			var tabView = GetTabView("Achievements");
			tabView.ViewBag.player = player;
			tabView.ViewBag.earnedAchievements = earnedAchievements;
			tabView.ViewBag.allAchievements = allAchievements;
			tabView.ViewBag.lockedAchievements = lockedAchievements;
			return tabView;

        }

        /// <summary>
        /// Formats player age to days and hours, which is more user friendly.
        /// </summary>
        /// <param name="age">Player age</param>
        /// <returns></returns>
        private string FormatInGameAge(TimeSpan age)
        {
            string result = "";
            int days = age.Days;
            int hours = age.Hours;

            if (days > 0)
            {
                if(days == 1)
                {
                    result = days + " den a ";
                }
                else if(days <= 4)
                {
                    result = days + " dny a ";
                }
                else
                {
                    result = days + " dní a ";
                }
            }

            if (hours == 0)
            {
                result += "O hodin";
            }
            else if (hours > 0)
            {
                if (hours == 1)
                {
                    result += hours + " hodina";
                }
                else if (hours <= 4)
                {
                    result += hours + " hodiny";
                }
                else
                {
                    result += hours + " hodin";
                }
            }

            return result;
        }
    }
}
