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
	bgZoom: 1,
	bgViewportTX: 0,
	bgViewportTY: 0,
	bgOffsetX: 0,
	bgOffsetY: 0,
	bg2Zoom: 1,
	bg2ViewportTX: 0,
	bg2ViewportTY: 0,
	bg2OffsetX: 0,
	bg2OffsetY: 0,
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
		
		this.$svgViewport.mousedown((function (e, touchPageXY) {
			if (typeof e.pageX === 'undefined' && typeof touchPageXY !== 'undefined') {
				e.pageX = touchPageXY.pageX;
				e.pageY = touchPageXY.pageY;
				e.which = 1;
			}
			this.$svgViewport.css('cursor', 'pointer');
			this.offsetX = this.viewportTX - e.pageX;
			this.offsetY = this.viewportTY - e.pageY;
			this.bgOffsetX = this.bgViewportTX - (e.pageX * BG_MOVING_CORRECTION);
			this.bgOffsetY = this.bgViewportTY - (e.pageY * BG_MOVING_CORRECTION);
			this.bg2OffsetX = this.bg2ViewportTX - (e.pageX * BG_MOVING_CORRECTION2);
			this.bg2OffsetY = this.bg2ViewportTY - (e.pageY * BG_MOVING_CORRECTION2);
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
					var newBgX = e.pageX * BG_MOVING_CORRECTION + this.bgOffsetX;
					var newBgY = e.pageY * BG_MOVING_CORRECTION + this.bgOffsetY;
					var newBg2X = e.pageX * BG_MOVING_CORRECTION2 + this.bg2OffsetX;
					var newBg2Y = e.pageY * BG_MOVING_CORRECTION2 + this.bg2OffsetY;
					var redraw = false;
					if ((Math.abs(newX / this.zoom) > 1) || (Math.abs(newY / this.zoom) > 1)) {
						this.viewportTX = newX;
						this.viewportTY = newY;
						redraw = true;
					}
					if ((Math.abs(newX / this.bgZoom) > 1) || (Math.abs(newY / this.bgZoom) > 1)) {
						this.bgViewportTX = newBgX;
						this.bgViewportTY = newBgY;
						redraw = true;
					}
					if ((Math.abs(newX / this.bgZoom) > 1) || (Math.abs(newY / this.bgZoom) > 1)) {
						this.bg2ViewportTX = newBg2X;
						this.bg2ViewportTY = newBg2Y;
						redraw = true;
					}
					if (redraw && this.renderer.forceRedraw()) {
						this.lastMouseMoveT = t;
						this.updateDebugInfo();
					}
				}
			}
		}).bind(this));
		
		// If map is dragged, it will unset mouseMove flag and finalize dragging.
		// registered for mouseup and mouseleave events.
		var endDrag = (function (e, touchPageXY) {
			if (typeof e.pageX === 'undefined' && typeof touchPageXY !== 'undefined') {
				e.pageX = touchPageXY.pageX;
				e.pageY = touchPageXY.pageY;
			}
			if (this.mouseMove)
			{
				this.mouseMove = false;
				this.viewportTX = e.pageX + this.offsetX;
				this.viewportTY = e.pageY + this.offsetY;
				this.bgViewportTX = e.pageX * BG_MOVING_CORRECTION + this.bgOffsetX;
				this.bgViewportTY = e.pageY * BG_MOVING_CORRECTION + this.bgOffsetY;
				this.bg2ViewportTX = e.pageX * BG_MOVING_CORRECTION2 + this.bg2OffsetX;
				this.bg2ViewportTY = e.pageY * BG_MOVING_CORRECTION2 + this.bg2OffsetY;
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
		this.$svgViewport.mousewheel(function (e, delta) {
			if (typeof delta === 'undefined' && typeof e.delta !== 'undefined') {
				delta = e.delta;
			}
			var oldZoom = parent.zoom;
			var oldBgZoom = parent.bgZoom;
			var oldBg2Zoom = parent.bg2Zoom;
			if (delta > 0) {/* zoom in */
				parent.zoom *= MOUSEWHEEL_ZOOM_SPEED;
				parent.bgZoom *= BG_MOUSEWHEEL_ZOOM_SPEED;

			} else {/* zoom out */
				parent.zoom /= MOUSEWHEEL_ZOOM_SPEED;
				parent.bgZoom /= BG_MOUSEWHEEL_ZOOM_SPEED;
			}
	
			parent.viewportTX = parent.getCoordinateCorrection(parent.zoom, oldZoom, e.offsetX, parent.viewportTX, delta);
			parent.viewportTY = parent.getCoordinateCorrection(parent.zoom, oldZoom, e.offsetY, parent.viewportTY, delta);

			parent.bgViewportTX = parent.getCoordinateCorrection(parent.bgZoom, oldBgZoom, e.offsetX, parent.bgViewportTX, delta);
			parent.bgViewportTY = parent.getCoordinateCorrection(parent.bgZoom, oldBgZoom, e.offsetY, parent.bgViewportTY, delta);

			parent.bg2ViewportTX = parent.getCoordinateCorrection(parent.bg2Zoom, oldBg2Zoom, e.offsetX, parent.bg2ViewportTX, delta);
			parent.bg2ViewportTY = parent.getCoordinateCorrection(parent.bg2Zoom, oldBg2Zoom, e.offsetY, parent.bg2ViewportTY, delta);
			
			parent.updateDebugInfo();
		});

	},
	/* calculates translate correction when is zoomed */
	getCoordinateCorrection: function (zoom, oldZoom, eOffset, viewportT, delta) {
		var zoomDiff = Math.abs(zoom - oldZoom);
		/* cursor position in svg coordinates */
		var svgCursor = (eOffset - viewportT) / oldZoom;
		var correction = svgCursor - (svgCursor * (1 + zoomDiff));
		if (delta > 0) {/* zoom in */
			return viewportT + correction;
		} else {/* zoom out */
			return viewportT - correction;
		}
	},	
	updateDebugInfo: function(){
		this.renderer.debugInfo.viewportTX = this.viewportTX;
		this.renderer.debugInfo.viewportTY = this.viewportTY;
		this.renderer.debugInfo.zoom = this.zoom;
	},
	
	buildViewport: function(svgContent){
		var viewportbuffer = '<svg id="svgCanvas" xmlns="http://www.w3.org/2000/svg" version="1.1">' + svgContent + '</svg>';
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
	
	getViewportTransform: function () {
		return 'scale(' + this.zoom + '),translate(' + this.viewportTX / this.zoom + ',' + this.viewportTY / this.zoom + ')';
	},
	/* similar as getViewportTransform but it is counted for background of closerStars (background moving while viewport moving)*/
	getDynamicBackgroundTransform: function () {
		return 'scale(' + this.bgZoom + '),translate(' + this.bgViewportTX / this.bgZoom + ',' + this.bgViewportTY / this.bgZoom + ') rotate(32)';
	},
	getSecondDynamicBackgroundTransform: function () {
		return 'scale(' + this.bg2Zoom + '),translate(' + this.bg2ViewportTX / this.bg2Zoom + ',' + this.bg2ViewportTY / this.bg2Zoom + ') rotate(45)';
	}
	
})