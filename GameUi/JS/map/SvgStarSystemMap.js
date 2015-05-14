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
        console.debug("StarSystemRenderer.init()", $svgViewport);
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
		console.group('SvgStarSystemMap.draw');
        //get time in seconds
        if(this.timer != null)
        {
        	clearInterval(this.timer);
        	this.timer = null;
        }
        
        var backgroundLayerBuff = '<g id="svgBackgroundLayer">';
		var objectLayerBuff ='<g id="svgObjectLayer">';
		var overlayLayerBuff = '<g id="svgOverlayLayer">';
        
        var t = TimeUtils.getCurrentTimeInS();
        
        var svgStar = new SvgStar(0, this.currentStarSystem.star);
        
        backgroundLayerBuff += svgStar.buildBackground(t);
        objectLayerBuff += svgStar.buildObject();
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
	        	overlayLayerBuff += svgWormholeEndpoint.buildOverlay();
	            
	            this.svgItems.push(svgWormholeEndpoint);
		        if(svgWormholeEndpoint.isStatic() == false){
		        	this.svgDynamicItems.push(svgWormholeEndpoint);
		        }        		
        	}
        }
		
		//preparations
        var buffer = '<g id="svgViewportGroup" transform="'+this.svgViewportManager.getViewportTransform()+'">'
        			+ backgroundLayerBuff+'</g>'+ objectLayerBuff+'</g>'+ overlayLayerBuff+'</g>'+ '<g id="svgTopLayer"></g>';
		console.debug('buffer', buffer);
		
		console.debug('Building viewport, Writing svg');
		this.svgViewportManager.buildViewport(buffer);
		
		buffer = null;
		
		$('#svgStarSystemMap').bind('dragstart', function(event) { event.preventDefault(); });
		
        this.$svgViewportGroup = $('#svgViewportGroup');
        this.$svgBackgroundLayer = $('#svgBackgroundLayer');
        this.$svgObjectLayer = $('#svgObjectLayer');
        this.$svgOverlayLayer = $('#svgOverlayLayer');
        this.$svgTopLayer = $('#svgTopLayer');
        
        console.debug('svgItems:',this.svgItems);
        
        console.debug('svgDynamicItems:',this.svgDynamicItems);
        
        for (var i = 0; i < this.svgItems.length; i++) {
        	console.debug('Revive on: ', this.svgItems[i])
        	this.svgItems[i].revive();
        }

        $.getJSON($("#appRoot").attr("href") + 'Achievement/GetEarnedAchievements', function (unviewedAchievements) {
        	if (jQuery.isEmptyObject(unviewedAchievements)) {
        		return;
        	}

        	$.each(unviewedAchievements, function (index, achievement) {
        		var achvPopup = $("#achievement");
        		achvPopup.queue(function () {
        			$(".name").text(achievement.Name);
        			$(".icon img").attr("src", achievement.Image);
        			$(this).dequeue();
        		}).slideDown(400).delay(2000).slideUp(400);
        	});
        });
        
        this.updateObjectList();
        console.groupEnd();
    },

	// Animation update. Used as staticRenderCallback in SvgRenderer.
    update: function (renderer, t) {
    	for (var i = 0; i < this.svgDynamicItems.length; i++) {
	    	this.svgDynamicItems[i].prepareUpdate(t);
	    };	
			
        this.$svgViewportGroup.attr('transform', this.svgViewportManager.getViewportTransform());
        
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
    	var buffer = "<ul>";
		for (var i = 0; i < this.svgItems.length; i++) {
		    buffer += '<li>' + this.svgItems[i].getName() + '</li>\n';
	    };
		buffer += "</ul>";

        //tohle je potøeba mìnit, parametry se predavaji jako v LoadCargo
		buffer += '<ul>' + 
        '<li><a href="/Game/Cargo/BuyCargo?starSystemName=Proxima Centauri&planetName=Proxima Centauri 1&cargoLoadEntityId=1&count=1&buyingPlace=TraderCargoDAO&buyerShipId=1&traderId=1">BuyCargo</a></li>' +
            //'<li><a href="/Game/Cargo/LoadCargo?cargoId=1&objectId=2">LoadCargo</a></li>' +
            //'<li><a href="/Game/Cargo/UnloadCargo">UnloadCargo</a></li>' +
            '<li><a href="/Game/Cargo/SellCargo?starSystemName=Proxima Centauri&planetName=Proxima Centauri 1&cargoLoadEntityId=1&count=1&loadingPlace=TraderCargoDAO&buyerId=1&sellerShipId=1">SellCargo</a></li>' +
			'<li><a href="/Game/Ships?baseId=1&starSystemName=Solar System#Buy_new_ship">BuyShip</a></li>' +
            '<li><a href="/Game/Cargo/Planner">TestPlanner</a></li>' + //TODO: delete after test
            '</ul>';
	    $("#contextPanel").html(buffer);
	    buffer = null;
    }
};

