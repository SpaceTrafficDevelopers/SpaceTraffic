using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaceTraffic.GameUi.Models.Ui
{
    /// <summary>
    /// TabItem represents one tab in tab control.
    /// </summary>
    public class TabItem
    {
        /// <summary>
        /// Gets or sets the text displayed on tab.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets the action on selecting this tab.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Gets or sets the title attribute. Used for tooltip.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the PartialView name for this tab.
        /// </summary>
        /// <value>
        /// The partialview name.
        /// </value>
        public string PartialViewName { get; set; }

        /// <summary>
        /// Gets or sets the PartialView model.
        /// </summary>
        /// <value>
        /// The PartialView model.
        /// </value>
        public object PartialViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="text">The text.</param>
        /// <param name="title">The title.</param>
        /// <param name="partialViewName">PartialView name.</param>
        /// <param name="partialViewModel">PartialView model.</param>
        public TabItem(string action, string text= "", string title = "", string partialViewName = "", object partialViewModel = null)
        {
            if (String.IsNullOrEmpty(action))
                throw new ArgumentNullException("Cannot be null or empty: action");

            this.Action = action;

            this.Text = String.IsNullOrWhiteSpace(text)? action : text;
            this.Title = title;
            this.PartialViewName = partialViewName;
            this.PartialViewModel = partialViewModel;
        }
    }
}