"use strict";

/// <reference path="../jquery-1.6.4-vsdoc.js" />
/// <reference path="../jquery-ui-1.8.16.js" />

var menuWidthBugCorrection = 10;
var contextPanelWidthBugCorrection = 40;
var topMarginBugCorrection = 30;
var sideMargin = 15;


var ViewportManager = {
	$topPanel: {},
	$contextPanel: {},
	$mainPanel: {},
	$menuPanel: {},
	$viewport: {},

	init: function () {
		// selector initialization
		this.$viewport = $('#content');
		this.$contextPanel = $('#contextPanel');
		this.$mainPanel = $('#mainPanel');
		this.$menuPanel = $('#menuPanel');
		this.$topPanel = $('#applicationPanel');
		this.$infoPanel = $('#infoPanel');

		// method binding
		this.doLayout = this.doLayout.bind(this);

		this.doLayout();

		// event registration
		$(window).resize(this.doLayout.bind(ViewportManager));
		this.$viewport.bind('changed', this.doLayout.bind(ViewportManager));

		// selector function overwriting
		this.$contextPanel.hide = (function () {
			this.$contextPanel._hide();
			this.doLayout();
		}).bind(ViewportManager);

		this.$contextPanel.hide = (function () {
			this.$contextPanel._hide();
			this.doLayout();
		}).bind(ViewportManager);

		var parent = this;
		this.$contextPanel.on('click', '.closebutton', function () {
			$('#gameTopPanel .rightPart').removeClass('contextOpen');
			parent.$contextPanel.removeClass('open');
			parent.$infoPanel.removeClass('open');
			parent.doLayout();
		});
		this.$mainPanel.on('click', '.closebutton', function () {
			parent.$mainPanel.removeClass('open');
			parent.doLayout();
		});

	},
	closeMainPanel: function () {
		this.$mainPanel.find('.closebutton').trigger('click');
	},
	closeContextPanel: function () {
		this.$contextPanel.find('.closebutton').trigger('click');
	},

	doLayout: function () {
		this.$viewport.height($('#envelope').outerHeight() - this.$topPanel.outerHeight() - $('#gameBottomPanel').outerHeight() - 10);
		var menuPanelWidth = (this.$menuPanel.is(':visible')) ? this.$menuPanel.outerWidth(true) + menuWidthBugCorrection : 0;
		menuPanelWidth += sideMargin;
		var contextPanelEffectiveWidth = this.$contextPanel.is(':hidden') ? parseInt(ViewportManager.$contextPanel.css('margin-right').replace('px', '')) : this.$contextPanel.outerWidth(true) + contextPanelWidthBugCorrection;
		this.$mainPanel.css('left', menuPanelWidth);
		this.$mainPanel.css('top', this.$topPanel.outerHeight(true) + topMarginBugCorrection);
		this.$mainPanel.outerWidth(this.$viewport.width() - menuPanelWidth - contextPanelEffectiveWidth - sideMargin);

		if (this.$contextPanel.is(':visible')) {
			$('#gameTopPanel .rightPart').addClass('contextOpen');
		}
	}
}
