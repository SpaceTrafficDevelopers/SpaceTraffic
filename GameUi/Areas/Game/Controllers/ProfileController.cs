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

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class ProfileController : TabsControllerBase
    {
        protected override void BuildTabs()
        {
            this.Tabs.AddTab("Overview", "Profile", "Profile overview", "_Overview");
            this.Tabs.AddTab("Personal", title: "Personal informations.", partialViewName: "_Personal");
            this.Tabs.AddTab("Settings", title: "Profile settings.", partialViewName: "_Settings");
            this.Tabs.AddTab("Achievements", title: "Achievements", partialViewName: "_AchievementsList");
        }

        public ActionResult Index()
        {
            return View(INDEX_VIEW);
        }

        public PartialViewResult Overview()
        {
			Player player = GSClient.GameService.GetPlayer(getCurrentPlayerId());
			ExperienceLevels globalExpiriencesLevels = GSClient.GameService.GetExperienceLevels();
			TLevel currentLevel = globalExpiriencesLevels.GetLevel(player.ExperienceLevel);
			TLevel nextLevel = globalExpiriencesLevels.GetLevel(currentLevel.LevelID + 1);
			String nextLevelExp = (nextLevel == null) ? "-" : "" + nextLevel.RequiredXP;
			
			
			var tabView = GetTabView("Overview");
			tabView.ViewBag.player = player;
			tabView.ViewBag.currentLevel = currentLevel;
			tabView.ViewBag.nextLevel = nextLevel;
			tabView.ViewBag.nextLevelExp = nextLevelExp;
			return tabView;
        }

        public PartialViewResult Personal()
        {
            return GetTabView("Personal");
        }

        public PartialViewResult Settings()
        {
            return GetTabView("Settings");
        }

        public PartialViewResult Achievements()
        {
			Player player = GSClient.GameService.GetPlayer(getCurrentPlayerId());
			Achievements allAchievements = GSClient.GameService.GetAchievements();
			List<EarnedAchievement> earnedAchievements = GSClient.GameService.GetEarnedAchievements(player.PlayerId);
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
    }
}
