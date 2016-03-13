/// <reference path="../../Scripts/jquery-1.6.4-vsdoc.js" />
/// <reference path="../../Scripts/mootools-core-1.4.1.js" />
/// <reference path="../../Scripts/jquery-ui-1.8.16.js" />
/// <reference path="StarSystemObjects.js" />

var SvgStarSystemMap = {
	svgItems: [],
	svgDynamicItems: [],
		
	$svgViewport: null,
	
	// JQuery selector for svgViewportGroup.
	$svgViewportGroup: null,
	
	// JQuery selector for svgBackgroundLayer.
	$svgBackgroundLayer: null,
	
	// JQuery selector for svgObjectLayer
	$svgObjectLayer: null,
	
	// JQuery selector for svgOverlayLayer
	$svgOverlayLayer: null,
	
	// JQuery selector for svgTopLayer
	$svgTopLayer: null,
		
	currentStarSystem: null,
	svgViewportManager: null,
	
	renderer: null,
	/**
	* Initializes rendrer
	*/
	init: function ($svgViewport) {
		//console.debug("StarSystemRenderer.init()", $svgViewport);
		this.$svgViewport = $svgViewport;
				
		// bind function to this instance
		this.update = this.update.bind(this);
		this.startUpdateTimer = this.startUpdateTimer.bind(this);
		this.stopUpdateTimer = this.stopUpdateTimer.bind(this);
		this.setFade = this.setFade.bind(this);
		this.unsetFade = this.unsetFade.bind(this);
		this.updateObjectList = this.updateObjectList.bind(this);
		
		
		this.renderer = new SvgRenderer(this.update, SPACE_TRAFFIC_MAX_FPS, 'debugOutput');
		this.svgViewportManager = new SvgViewportManager(this.$svgViewport, this.renderer);
	},

	/**
	* Draw scene.
	*/
	draw: function () {
		//console.group('SvgStarSystemMap.draw');
		//get time in seconds
		if(this.timer != null)
		{
			clearInterval(this.timer);
			this.timer = null;
		}
		
		var backgroundLayerBuff = '<g id="svgBackgroundLayer">';
		var objectLayerBuff ='<g id="svgObjectLayer">';
		var overlayLayerBuff = '<g id="svgOverlayLayer">';
		var defsBuff = '';
		
		var t = TimeUtils.getCurrentTimeInS();
		
		var svgStar = new SvgStar(0, this.currentStarSystem.star);
		
		backgroundLayerBuff += svgStar.buildBackground(t);
		objectLayerBuff += svgStar.buildObject();
		defsBuff += svgStar.buildDefs();
		overlayLayerBuff += svgStar.buildOverlay();
		
		this.svgItems.length=0;
		this.svgDynamicItems.length=0;
		
		this.svgItems.push(svgStar);
		if(svgStar.isStatic() == false){
			this.svgDynamicItems.push(svgStar);
		} 
		
		var svgPlanet;
		for (var i = 0; i < this.currentStarSystem.planets.length; i++) {
			svgPlanet = new SvgPlanet(i, this.currentStarSystem.planets[i]);
			backgroundLayerBuff += svgPlanet.buildBackground(t);
			objectLayerBuff += svgPlanet.buildObject();
			defsBuff += svgPlanet.buildDefs();
			overlayLayerBuff += svgPlanet.buildOverlay();
			
			this.svgItems.push(svgPlanet);
			if(svgPlanet.isStatic() == false){
				this.svgDynamicItems.push(svgPlanet);
			}
		}
		
		var svgWormholeEndpoint;
		for (var i = 0; i < this.currentStarSystem.wormholeEndpoints.length; i++) {
			if(this.currentStarSystem.wormholeEndpoints[i].destination != null)
			{
				svgWormholeEndpoint = new SvgWormholeEndpoint(this.currentStarSystem.wormholeEndpoints[i]);
				backgroundLayerBuff += svgWormholeEndpoint.buildBackground(t);
				objectLayerBuff += svgWormholeEndpoint.buildObject();
				defsBuff += svgWormholeEndpoint.buildDefs();
				overlayLayerBuff += svgWormholeEndpoint.buildOverlay();
				
				this.svgItems.push(svgWormholeEndpoint);
				if(svgWormholeEndpoint.isStatic() == false){
					this.svgDynamicItems.push(svgWormholeEndpoint);
				}        		
			}
		}
		
		//preparations
		var buffer = '<defs>' + defsBuff + this.createBackgroundDefs() + '</defs>' + this.createBackgrounds()
			+ '<g id="svgViewportGroup" transform="' + this.svgViewportManager.getViewportTransform() + '">'
					+ backgroundLayerBuff+'</g>'+ objectLayerBuff+'</g>'+ overlayLayerBuff+'</g>'+ '<g id="svgTopLayer"></g>';
		//console.debug('buffer', buffer);
		
		//console.debug('Building viewport, Writing svg');
		this.svgViewportManager.buildViewport(buffer);
		
		buffer = null;
		
		$('#svgStarSystemMap').bind('dragstart', function(event) { event.preventDefault(); });
		
		this.$svgViewportGroup = $('#svgViewportGroup');
		this.$svgViewportGroupDefs = $('#svgViewportGroupDefs');
		this.$svgBackgroundLayer = $('#svgBackgroundLayer');
		this.$svgObjectLayer = $('#svgObjectLayer');
		this.$svgOverlayLayer = $('#svgOverlayLayer');
		this.$svgTopLayer = $('#svgTopLayer');
		
		//console.debug('svgItems:',this.svgItems);
		
		//console.debug('svgDynamicItems:',this.svgDynamicItems);
		
		for (var i = 0; i < this.svgItems.length; i++) {
			//console.debug('Revive on: ', this.svgItems[i])
			this.svgItems[i].revive();
		}
		//console.groupEnd();
	},

	createBackgroundDefs: function () {
		var patternSize = 200;
		var distantStars = '<pattern id="bgDistantStars" x="0" y="0" width="' + patternSize + '" height="' + patternSize + '" patternTransform="rotate(-32)" patternUnits="userSpaceOnUse"><g id="distantStars">';
		distantStars = distantStars + this.createStarsObjects(50, patternSize, BG_FAR_SEED, 1.0, 1.2, 'distantStar');
		distantStars = distantStars + '</g></pattern>';

		var closerStars = '<pattern id="bgCloserStars" x="0" y="0" width="' + patternSize + '" height="' + patternSize + '" patternTransform="" patternUnits="userSpaceOnUse"><g id="closerStars">';
		closerStars = closerStars + this.createStarsObjects(15, patternSize, BG_MIDDLE_SEED, 1.2, 2.2, 'closerStar');
		closerStars = closerStars + '</g></pattern>';

		var middleDistanceStars = '<pattern id="bgMiddleDistanceStars" x="0" y="0" width="' + patternSize + '" height="' + patternSize + '" patternTransform="" patternUnits="userSpaceOnUse"><g id="middleDistanceStars">';
		middleDistanceStars = middleDistanceStars + this.createStarsObjects(20, patternSize, BG_CLOSE_SEED, 1.0, 2.0, 'middleDistanceStar');
		middleDistanceStars = middleDistanceStars + '</g></pattern>';

		return distantStars + closerStars + middleDistanceStars;
	},
	/* creates objects (stars) for background pattern */
	createStarsObjects: function (numberOfStars, patternSize, seed, minSize, maxSize, className) {
		var buff = '';
		Math.seedrandom(seed);
		var cx = 0;
		var cy = 0;
		for(var stars = 0; stars < numberOfStars; stars++){
			curCx = Math.random() * patternSize;
			curCy = Math.random() * patternSize;
			buff = buff + '<circle r="' + (minSize + (Math.random() * (minSize - maxSize))) + '" cy="' + curCy + '" cx="' + curCx + '" class="' + className + '" />';
		}
		Math.seedrandom();
		return buff;
	},
	createBackgrounds: function () {
		return '<rect x="0" y="0" width="100%" height="100%" id="distantStarsRect" style="fill: url(#bgDistantStars);" />'
			+ '<rect x="0" y="0" width="100%" height="100%" id="middleDistanceStarsRect" style="fill: url(#bgMiddleDistanceStars);" />'
			+ '<rect x="0" y="0" width="100%" height="100%" id="closerStarsRect" style="fill: url(#bgCloserStars);" />';
	},

	// Animation update. Used as staticRenderCallback in SvgRenderer.
	update: function (renderer, t) {
		for (var i = 0; i < this.svgDynamicItems.length; i++) {
			this.svgDynamicItems[i].prepareUpdate(t);
		};	
			
		this.$svgViewportGroup.attr('transform', this.svgViewportManager.getViewportTransform());
		this.$svgViewportGroupDefs.attr('transform', this.svgViewportManager.getViewportTransform());
		this.$svgViewport.find('#bgCloserStars').attr('patternTransform', this.svgViewportManager.getDynamicBackgroundTransform());
		this.$svgViewport.find('#bgMiddleDistanceStars').attr('patternTransform', this.svgViewportManager.getSecondDynamicBackgroundTransform());
		
		for (var i = 0; i < this.svgDynamicItems.length; i++) {
			this.svgDynamicItems[i].performUpdate(t);
		};
	},
	
	startUpdateTimer: function(){
		this.renderer.startUpdateTimer();
	},    
	
	stopUpdateTimer: function(){
		this.renderer.stopUpdateTimer();
	},
	
	getLastUpdateTime: function () {
		return this.lastT;
	},
	
	setFade: function() {
		this.renderer.addRenderCallback((function(renderer, t){
			this.$svgBackgroundLayer.addClass('fade');
			this.$svgObjectLayer.addClass('fade');
			this.$svgOverlayLayer.addClass('fade');
		}).bind(this));
	},
	
	unsetFade: function() {
		this.renderer.addRenderCallback((function(renderer, t){
			this.$svgBackgroundLayer.removeClass('fade');
			this.$svgObjectLayer.removeClass('fade');
			this.$svgOverlayLayer.removeClass('fade');
		}).bind(this));
	},

	//updating star object list (changed for cargo controller tests)
	updateObjectList: function() {

		//links for trading, test link fo planner, test links for buy ship
		buffer += '<ul>' + 
			'<li><a href="/Game/Cargo/BuyCargo?starSystemName=Proxima Centauri&planetName=Proxima Centauri 1&cargoLoadEntityId=1&count=1&buyingPlace=TraderCargoDAO&buyerShipId=1&traderId=1">BuyCargo</a></li>' +
		   '<li><a href="/Game/Cargo/SellCargo?starSystemName=Proxima Centauri&planetName=Proxima Centauri 1&cargoLoadEntityId=1&count=1&loadingPlace=TraderCargoDAO&buyerId=1&sellerShipId=1">SellCargo</a></li>' +

			'<li><a href="/Game/Ships?baseId=1&starSystemName=Solar System#Buy_new_ship">BuyShip</a></li>' +
			'<li><a href="/Game/Ships?baseId=2&starSystemName=Proxima Centauri#Buy_new_ship">BuyShipOnProxima</a></li>' +
			'<li><a href="/Game/Planner/TestPlanner">TestPlanner</a></li>' + 
			'</ul>';
		$("#contextPanelContent").html(buffer);
		buffer = null;
	}
};

