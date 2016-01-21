// Provides control over viewport area. Responsible for scrolling and zoom of viewport.
var SvgViewportManager = new Class({
	$svgViewport: null,
	offsetX: 0,
	offsetY: 0,
	zoom: 1,
	mouseMove: false,
	$debugOutput: null,
	viewportTX: 0,
	viewportTY: 0,
	rendrer: null,
	lastMouseMoveT: 0,
	initialize: function($svgViewport, renderer){
		//console.group('SvgViewportManager:init');
		//bind functions
		this.buildViewport = this.buildViewport.bind(this);
		this.getViewportTransform = this.getViewportTransform.bind(this);
		this.updateDebugInfo = this.updateDebugInfo.bind(this);
		
		this.$svgViewport = $svgViewport;
		this.viewportTX = $svgViewport.width() / 2.0;
		this.viewportTY = $svgViewport.height() / 2.0;
		
		this.renderer = renderer;
		
		this.$svgViewport.mousedown((function (e) {
			this.$svgViewport.css('cursor', 'pointer');
			this.offsetX = this.viewportTX - e.pageX;
			this.offsetY = this.viewportTY - e.pageY;
			this.lastMouseMoveT = TimeUtils.getCurrentTimeInS();
			if (e.which == 1) this.mouseMove = true;
		}).bind(this));
		
		this.$svgViewport.mousemove((function (e) {
			if (this.mouseMove) {
				var t = TimeUtils.getCurrentTimeInS();
				var diffT = TimeUtils.getTimeDiff(t, this.lastMouseMoveT);
				if(diffT > 0.02)
				{
					this.renderer.debugInfo.mouseX=e.pageX;
					this.renderer.debugInfo.mouseY=e.pageY;
					
					var newX = e.pageX + this.offsetX
					var newY = e.pageY + this.offsetY;
					
					if((Math.abs(newX/this.zoom) > 1) || (Math.abs(newY/this.zoom) > 1))
					{
						this.viewportTX = newX;
						this.viewportTY = newY;
						
						
						if(this.renderer.forceRedraw())
						{
							this.lastMouseMoveT = t;
							this.updateDebugInfo();
						}
				   }
				}
			}
		}).bind(this));
		
		// If map is dragged, it will unset mouseMove flag and finalize dragging.
		// registered for mouseup and mouseleave events.
		var endDrag = (function (e) {
			if (this.mouseMove)
			{
				this.mouseMove = false;
				this.viewportTX = e.pageX + this.offsetX;
				this.viewportTY = e.pageY + this.offsetY;
				this.offsetX = 0;
				this.offsetY = 0;
				this.$svgViewport.css('cursor', 'auto');
				this.updateDebugInfo();
				this.renderer.forceRedraw();
			}
		}).bind(this);
		
		this.$svgViewport.mouseup(endDrag);
		this.$svgViewport.mouseleave(endDrag);
		
		//console.groupEnd();
		/* zooming with mousewheel */
		var parent = this;
		this.$svgViewport.mousewheel(function (e) {
			if (e.wheelDelta > 0) {/* zoom in */
				parent.zoom *= Math.abs(e.wheelDelta * MOUSEWHEEL_ZOOM_MODIFIER);
			} else {/* zoom out */
				parent.zoom /= Math.abs(e.wheelDelta * MOUSEWHEEL_ZOOM_MODIFIER);
			}
			console.log(parent.zoom);
			parent.$svgViewport.find('#svgViewportGroup').attr('transform', parent.getViewportTransform());
			parent.updateDebugInfo();
		});

	},
	
	updateDebugInfo: function(){
		this.renderer.debugInfo.viewportTX = this.viewportTX;
		this.renderer.debugInfo.viewportTY = this.viewportTY;
		this.renderer.debugInfo.zoom = this.zoom;
	},
	
	buildViewport: function(svgContent){
		var viewportbuffer = '<svg id="svgCanvas" xmlns="http://www.w3.org/2000/svg" version="1.1">'+svgContent+'</svg>';
		if(SPACE_TRAFFIC_DEBUG)
		{
			viewportbuffer += '<div id="debugOverlay" class="debugOverlay"><pre id="debugOutput">Debug output...</pre></div>';
			//console.debug("Debug overlay added");
			
		}
		
		this.$svgViewport.html(viewportbuffer);
		
		// Zakázání tažení obrázku        
		document.getElementById('svgCanvas').ondragstart = function() { return false; };
		
		viewportbuffer = null;
		this.$debugOutput = $('#debugOutput');
	},
	
	getViewportTransform: function() {
		return 'scale('+this.zoom+'),translate(' + this.viewportTX/this.zoom + ',' + this.viewportTY/this.zoom + ')';
	}
	
})