/// <reference path="../../Scripts/jquery-1.6.4-vsdoc.js" />
/// <reference path="../../Scripts/mootools-core-1.4.1.js" />
/// <reference path="../../Scripts/jquery-ui-1.8.16.js" />
/// <reference path="StarSystemObjects.js" />

var SvgRenderer = new Class({
	maxFps: 1,
	debugOutputId: "debugOutput",
	lastT: 0,
    fps: 0,
    frameCount: 0,
    // Number of frames canceled due 
    droppedFrameCount: 0,
    isRenderingFrame: false, 
    renderQueue: new Queue(),
    bufferQueue: new Queue(),
    timer: null,
    
    staticRenderCallback: null,
    svgViewportManager: null,
    $debugOutput: null,
    debugInfo: {},
    /**
    * Initializes rendrer
    */
    initialize: function(staticRenderCallbackFnc, maxFps, debugOutputId) {
    	
    	console.debug("staticRenderCallbackFnc: ",staticRenderCallbackFnc);
    	//console.debug("viewportManager: ",viewportManager);
        // bind function to this instance
        
        this.maxFps = maxFps;
        this.redraw = this.redraw.bind(this);
        this.forceRedraw = this.forceRedraw.bind(this);
        this.debugPrint = this.debugPrint.bind(this);
        /*this.startUpdateTimer = this.startUpdateTimer.bind(this);
        this.stopUpdateTimer = this.stopUpdateTimer.bind(this);
        this.getLastUpdateTime = this.getLastUpdateTime.bind(this);
        this.addRenderCallback = this.addRenderCallback.bind(this);*/
       	this.staticRenderCallback = staticRenderCallbackFnc;
       	this.debugOutputId = debugOutputId;
        this.lastT = (new Date().getTime())/1000;
    },    

	// Animation update.
    redraw: function () {
    	if(this.isRenderingFrame)
    	{
    		this.droppedFrameCount++;
    		return false;
    	}
    	this.isRenderingFrame = true;
    	
    	// Switch render queue
    	var pomQueue = this.renderQueue;
    	this.renderQueue = this.bufferQueue;
    	this.bufferQueue = pomQueue;
    	
    	requestAnimFrame((function(){
	    	var t = TimeUtils.getCurrentTimeInS();    	
			
			this.staticRenderCallback(this, t);
			
			var fnc;
			while(!this.renderQueue.isEmpty())
			{
				fnc = this.renderQueue.dequeue();
				fnc.call(this,this, t);
			}
			
	        var frameTime = TimeUtils.getCurrentTimeInS()-this.lastT;
	        this.fps = this.fps*0.9+0.1*1/frameTime;
	        this.lastT = t;
			this.frameCount++;
			
			if(SPACE_TRAFFIC_DEBUG)
			{
		        this.debugPrint(
		        	"FPS: "+Math.round(this.fps)+
		        	"\nFrames: "+this.frameCount+
		        	"\nDropped frames: "+this.droppedFrameCount+
		        	"\nviewportTX: "+this.debugInfo.viewportTX+
		        	"\nviewportTY: "+this.debugInfo.viewportTY+
		        	"\nzoom: "+this.debugInfo.zoom+
		        	"\nmouseX: "+this.debugInfo.mouseX+
		        	"\nmouseY: "+this.debugInfo.mouseY
		        	);
	        }
	        /*
	        if(this.svgViewportManager.mouseMove)
	        	this.startUpdateTimer();
	        */
	        this.isRenderingFrame = false;
    	}).bind(this));
    	return true;
    },
    
    forceRedraw: function(){
    	return this.redraw();
    },
    
    
    startUpdateTimer: function(){
    	console.debug("Renderer start");
    	if(typeof this.timer != "undefined") clearInterval(this.timer);
    	if(this.maxFps > 0)
    	{
    		this.timer = setInterval(this.redraw, 1000/this.maxFps );
    	}
    },    
    
    stopUpdateTimer: function(){
    	clearInterval(this.timer); 
    },
    
    getLastUpdateTime: function () {
    	return this.lastT;
    },
    
    addRenderCallback: function(fnc){
    	this.bufferQueue.enqueue(fnc);
    },
    
    debugPrint: function(text){
		if(SPACE_TRAFFIC_DEBUG)
		{
			if(this.$debugOutput==null)
				this.$debugOutput = $('#'+this.debugOutputId);
			this.$debugOutput.html(text);
		}
	}
});

