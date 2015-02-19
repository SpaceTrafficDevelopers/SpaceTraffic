using System.Web.Mvc;

namespace SpaceTraffic.GameUi.Areas.Game
{

    /// <summary>
    /// Initializes Game Area
    /// </summary>
    public class GameAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the name of the area to register.
        /// </summary>
        /// <returns>The name of the area to register.</returns>
        public override string AreaName
        {
            get
            {
                return "Game";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Game_default",
                "Game/{controller}/{action}/{id}",
                new { controller="Default", action = "Index", id = UrlParameter.Optional },
                new string[] { "SpaceTraffic.GameUi.Areas.Game.Controllers" }
            );
        }
    }
}
