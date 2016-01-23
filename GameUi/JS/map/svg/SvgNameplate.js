/**
 * @author Azaroth
 */
const ID_NAMEPLATE_SUFFIX = "-nameplate";
const ID_NAMEPLATE_BACKGROUNG_SUFFIX = "-background";
const ID_NAMEPLATE_TEXT_SUFFIX = "-text";

var SvgNameplate = new Class({
	Extends: SvgMapItem,
	initialize: function(idPrefix, text, dx, dy, cssClass, relatedObject){
		this.parent(idPrefix);
		this.text = text;
		this.dx = dx;
		this.dy = dy;
		this.cssClass = cssClass;
		this.cssClassPrefix = "nameplate";
		this.relatedObject = relatedObject;
	},
	
	text: '',
	minWidth: 50,
	textWidth: 90,
	textPadding: 15,
	cssClass: '',
	$svgNameplate: {},
	$svgText: {},
	$svgBackground: {},
	buildOverlayAt: function (groupTransform) {
		$('body').append('<span id="testText">' + this.text + '</span>');/* finding out text length*/
		var lengthElement = $('#testText');
		this.textWidth = lengthElement.width();
		lengthElement.remove();
		var startCorrection = (typeof (this.relatedObject.size) != "undefined" && this.relatedObject.size !== null)? this.relatedObject.size : 0;
		
		return '<g x="20" id="' + this.getId() + '" class="nameplate '+ this.cssClass + '" transform="' + groupTransform + '">'
			+ '<path transform="translate(' + startCorrection / 2 + ', ' + -startCorrection + ')" d="m 0,0 14,-52 ' + Math.max(this.minWidth, this.textWidth + this.textPadding) + ',0 -5.04054,18.81159 -' + Math.max(this.minWidth, this.textWidth + this.textPadding) + ',0" id="' + this.getId(ID_NAMEPLATE_BACKGROUNG_SUFFIX) + '" class="nameplateBg" />'
		//+ '<rect id="'+this.getId(ID_NAMEPLATE_BACKGROUNG_SUFFIX)+'" x="'+background_x+'" y="'+background_y+'" ry="10" width="'+this.textWidth+'" height="20" ></rect>'
		+ '<text transform="translate(' + startCorrection / 2 + ', ' + -startCorrection + ')" id="' + this.getId(ID_NAMEPLATE_TEXT_SUFFIX) + '" x="' + (this.textPadding + 3) + '" y="-38">' + this.text + '</text></g>';
	},
	updatePosition: function(groupTransform){
		this.$svgNameplate.attr('transform',groupTransform); 
	},
	revive: function(){
		this.$svgNameplate = $('#'+this.getId());
		this.$svgBackground = $('#'+this.getId(ID_NAMEPLATE_BACKGROUNG_SUFFIX));
		this.$svgText = $('#'+this.getId(ID_NAMEPLATE_TEXT_SUFFIX));
		//console.debug('$svgNameplate', this.$svgNameplate);
		//console.debug('$svgBackground', this.$svgBackground);
		//console.debug('$svgText', this.$svgText);
		//console.debug('element:', this.$svgText.get(0));
		
		// Sets textWidth from rendered element. 
		this.textWidth=this.$svgText.get(0).getComputedTextLength()+20;
		
		// Set
		var background_x = (this.dx-(this.textWidth/2));
		
		this.$svgBackground.attr("x", background_x);
		this.$svgBackground.attr("width", this.textWidth);
	}
});