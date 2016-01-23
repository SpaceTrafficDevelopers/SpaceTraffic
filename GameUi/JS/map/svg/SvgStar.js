/**
 * @author Azaroth
 */


var SvgStar = new Class({
	Extends: SvgMapItem,
	initialize: function(id, star){
		this.parent('star'+id);
		this.star = star;
		this.cssClassPrefix = "star";
	},
	star : {},
	$svgIcon : {},
	$svgNameplate : {},
	isStatic : function() {
		return true;
	},
	buildBackground : function(t) {
		this.setT(t);
		return "";
	},
	buildObject : function(t) {
		this.setT(t);
		return '<circle id="'+this.getId(ID_OBJECT_SUFFIX)+'" cx="0" cy="0" r="' + this.star.size + '" class="star" />';
	},
	buildOverlay : function(t) {
		this.setT(t);
		return '<text id="'+this.idPrefix+'N" x="0" y="' + (20 + 18) + '" class="starName">' + this.star.name + '</text>';
	},
	// Prepares for updating related elements. All calculations must be done here. Returns false, if there is currently nothing to update.
	prepareUpdate : function(t) {
		this.setT(t);
		return false;
	},
	performUpdate : function() {},
	revive : function() {
		var $svgIcon = this.$svgIcon = $('#'+this.getId(ID_OBJECT_SUFFIX));
		var $svgNameplate = this.$svgNameplate = $('#'+this.getId('N'));
		var $svgStar = $('#'+this.getId(ID_OBJECT_SUFFIX)+', #'+this.getId('N'));
		
		var mouseoverHandler = function(){
			//console.debug("mouseoverHandler");
			//console.debug("$svgIcon:",$svgIcon);
			//console.debug("$svgNameplate:",$svgNameplate);
			$svgStar.addClass('starHover');
		
		};
		var mouseleaveHandler = function(){
			$svgStar.removeClass('starHover');
		};
		
		$svgIcon.mouseover(mouseoverHandler).mouseleave(mouseleaveHandler);
		var boundCallbackHandler = this.onclickHandler.bind(this);

		$svgIcon.click(boundCallbackHandler);
		$svgNameplate.mouseover(mouseoverHandler).mouseleave(mouseleaveHandler);
	},
	
	getName: function() {
		return this.star.name;
	},
	onclickHandler: function (sender) {
		this.showStarInfo($('#infoPanel'));
	},

	showStarInfo: function ($element) {/* info about star on the left*/
		$element.html('<h2>' + this.getName() + '</h2><p>' + this.star.description + '</p>');
		$element.css('display', 'block');
	}
});