"use strict";

/// <reference path="../jquery-1.6.4-vsdoc.js" />
/// <reference path="../jquery-ui-1.8.16.js" />

var menuWidthBugCorrection = 30;
var contextPanelWidthBugCorrection = 40;
var topMarginBugCorrection = 30;


var ViewportManager = {
	$topPanel: {},
	$contextPanel: {},
	$mainPanel: {},
	$menuPanel: {},
	$viewport: {},
	
	init: function() {
		console.debug('ViewportManager.init()');
		// selector initialization
		this.$viewport = $('#content');
		this.$contextPanel = $('#contextPanel');
		this.$mainPanel = $('#mainPanel');
		this.$menuPanel = $('#menuPanel');
		this.$topPanel = $('#topPanel');
		
		// method binding
		this.doLayout = this.doLayout.bind(this);
		
		this.doLayout();
		
		// event registration
		$(window).resize(this.doLayout.bind(ViewportManager));
		
		console.debug(this.$contextPanel);
		
		// selector function overwriting
		this.$contextPanel.hide = (function(){
			this.$contextPanel._hide();
			this.doLayout();
		}).bind(ViewportManager);
		
		this.$contextPanel.hide = (function(){
			this.$contextPanel._hide();
			this.doLayout();
		}).bind(ViewportManager);
	},
	
	doLayout: function(){
		console.debug('ViewportManager.doLayout(): this=',this);
		//var $content = $('#content');
		this.$viewport.height($('#envelope').outerHeight() - $('#header').outerHeight() - $('#footer').outerHeight());

		var menuPanelWidth = this.$menuPanel.outerWidth(true) + menuWidthBugCorrection;
		//TODO: better way of getting margin from contextPanel.
		var contextPanelEffectiveWidth = this.$contextPanel.is(':hidden') ? parseInt(ViewportManager.$contextPanel.css('margin-right').replace('px', '')) : this.$contextPanel.outerWidth(true) + contextPanelWidthBugCorrection;
		this.$mainPanel.css('left', menuPanelWidth);
		this.$mainPanel.css('top', this.$topPanel.outerHeight(true) + topMarginBugCorrection);
		this.$mainPanel.outerWidth(this.$viewport.width() - menuPanelWidth - contextPanelEffectiveWidth);
		console.debug('mainPanel.left:', this.$mainPanel.position().left);
		console.debug('mainPanel.width:', this.$mainPanel.width());
	}
}


// Initialization of ViewportManager
/*
$(document).ready(function () {
	ViewportManager.init();
	setTimeout(function(){
		//ViewportManager.$contextPanel.hide();
		//ViewportManager.$menuPanel.hide();
	},2000);
});*/

