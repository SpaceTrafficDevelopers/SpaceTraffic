using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Models.Ui;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    /// <summary>
    /// Controller for page with tabs.
    /// </summary>
    public abstract class TabsControllerBase : Controller
    {
        /// <summary>
        /// Page which should be displayed for Index action.
        /// </summary>
        protected const string INDEX_VIEW = "~/Areas/Game/Views/Shared/_GameWindowLayout.cshtml";

        /// <summary>
        /// Gets the tabs.
        /// </summary>
        protected Tabs Tabs { get; private set; }

        /// <summary>
        /// Builds the tabs.
        /// Use this method to add tabs to the collection.
        /// </summary>
        protected abstract void BuildTabs();

        /// <summary>
        /// Initializes data that might not be available when the constructor is called.
        /// Used for building tabs and putting them into ViewBag.
        /// </summary>
        /// <param name="requestContext">The HTTP context and route data.</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.Tabs = new Tabs();
            this.BuildTabs();
            ViewBag.Tabs = this.Tabs;
        }

        /// <summary>
        /// Gets the view for given tab.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        /// <returns></returns>
        protected PartialViewResult GetTabView(string tabName)
        {
            this.Tabs.CurrentTab = this.Tabs.Items[tabName];
            return PartialView(this.Tabs.Items[tabName].PartialViewName);
        }
    }
}
