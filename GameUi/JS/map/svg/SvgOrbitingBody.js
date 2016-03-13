/**
 * @author Azaroth
 */

const ID_ORBIT_SUFFIX = "-orbit";

var SvgOrbitingBody = new Class({
	Extends: SvgMapItem,
	initialize: function(idPrefix, body){
		this.parent(idPrefix);
		this.body = body;
	},
	body : {},
	iconSize: 10,
	$svgOrbit : {}, //#planet1T
	$svgIcon : {}, //#planet1Obj
	svgNameplate : {},//#planet1N
	_updateBuffer: {},
	// True, if this object requires updates. Only checked on building.
	isStatic : function() {
		return false;
	},
	getName : function() {
		return this.idPrefix;
	},
	
	// Event handlers
	onclickHandler: function(sender) {
		
	},
	
	// Builds background for this object, like trajectory.
	buildBackground : function(t) {
		this.setT(t);
		if(this.body.trajectory instanceof CircularOrbit)
		{
			return SvgOrbitFactory.buildCurcularOrbit(this.body.trajectory, this.idPrefix+ID_ORBIT_SUFFIX);		
		}
		else if(this.body.trajectory instanceof EllipticOrbit)
		{
			return SvgOrbitFactory.buildEllipticOrbit(this.body.trajectory, this.idPrefix+ID_ORBIT_SUFFIX);
		}
		else
		{
			return '';
		}
	},
	// Builds this object graphics.
	buildObject : function(t) {
		this.setT(t);
		var point = this.body.trajectory.calculatePosition(this.getT());
		
		var transform = SvgOrbitFactory.getTransformationForPointOnOrbit(point, this.body.trajectory);
		
		return '<circle id="'+this.getId(ID_OBJECT_SUFFIX)+'" cx="0" cy="0" r="' + this.iconSize + 
			'"class="'+this.cssClassPrefix+'" transform="' + transform + '"/>';
	},
	// Builds nameplate and overlay icons.
	buildOverlay : function(t) {
		this.setT(t);
		var point = this.body.trajectory.calculatePosition(this.getT());
		this.svgNameplate = new SvgNameplate(this.getId(ID_NAMEPLATE_SUFFIX), this.getName(), 0, this.iconSize, '', this.body);
		var pointTansformation = SvgOrbitFactory.getTransformationForPointOnOrbit(point, this.body.trajectory);
		return this.svgNameplate.buildOverlayAt(pointTansformation);
	},
	// Makes preparations for updating elements related to this object.
	// All calculations must be done here.
	// Returns false, if there is currently nothing to update.
	prepareUpdate : function(t) {
		this.setT(t);
		var point = this.body.trajectory.calculatePosition(this.getT());
		this._updateBuffer.transformation = SvgOrbitFactory.getTransformationForPointOnOrbit(point, this.body.trajectory);
	},
	// Performs update of all elements.
	performUpdate : function() {
		this.$svgIcon.attr('transform', this._updateBuffer.transformation);
		this.svgNameplate.updatePosition(this._updateBuffer.transformation);
	},
	// Used for making this element interactive.
	revive : function() {
		var orbitId = '#'+this.getId(ID_ORBIT_SUFFIX);
		var objectId = '#'+this.getId(ID_OBJECT_SUFFIX);
		var nameplateId = '#'+this.getId(ID_NAMEPLATE_SUFFIX)
		
		var $svgBody = $(orbitId+','+objectId+','+nameplateId);
		var $svgOrbit = this.$svgOrbit = $(orbitId);
		var $svgIcon = this.$svgIcon = $(objectId);//+','+nameplateId);
		this.svgNameplate.revive();
		
		var $svgNameplate = this.svgNameplate.$svgNameplate;
		
		var mouseoverHandler = function(){
			//console.debug("SvgPlanet.mouseoverHandler");
			if(!$svgIcon.hasClass('hover'))
			{
				//console.debug("Reordering svg");
				$svgBody.addClass('hover');
				SvgStarSystemMap.$svgTopLayer.append($svgNameplate);
			}
		};
		var mouseleaveHandler = function(){
			$svgBody.removeClass('hover');
			SvgStarSystemMap.$svgOverlayLayer.append($svgNameplate);
		};
		
		$svgIcon.mouseenter(mouseoverHandler).mouseleave(mouseleaveHandler);
		$svgNameplate.mouseenter(mouseoverHandler).mouseleave(mouseleaveHandler);
		
		var boundCallbackHandler = this.onclickHandler.bind(this);
		
		$svgIcon.click(boundCallbackHandler);
		$svgNameplate.click(boundCallbackHandler);
	}
});