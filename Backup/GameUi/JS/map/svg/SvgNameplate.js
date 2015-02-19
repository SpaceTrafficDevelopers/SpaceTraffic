/**
 * @author Azaroth
 */
const ID_NAMEPLATE_SUFFIX = "-nameplate";
const ID_NAMEPLATE_BACKGROUNG_SUFFIX = "-background";
const ID_NAMEPLATE_TEXT_SUFFIX = "-text";

var SvgNameplate = new Class({
	Extends: SvgMapItem,
	initialize: function(idPrefix, text, dx, dy, cssClass){
		this.parent(idPrefix);
		this.text = text;
		this.dx = dx;
		this.dy = dy;
		this.cssClass = cssClass;
		this.cssClassPrefix = "nameplate";
	},
	
	text: '',
	textWidth: 90,
	cssClass: '',
	$svgNameplate: {},
	$svgText: {},
	$svgBackground: {},
	buildOverlayAt: function(groupTransform){
		var background_x = (this.dx-(this.textWidth/2));
		var background_y = (this.dy + 3);
		var text_x = this.dx;
		var text_y = (this.dy + 18) ;
		
		return '<g id="'+this.getId()+'" class="nameplate" transform="' + groupTransform +'">'
		+ '<rect id="'+this.getId(ID_NAMEPLATE_BACKGROUNG_SUFFIX)+'" x="'+background_x+'" y="'+background_y+'" ry="10" width="'+this.textWidth+'" height="20" ></rect>'
		+ '<text id="'+this.getId(ID_NAMEPLATE_TEXT_SUFFIX)+'" x="'+text_x+'" y="' + text_y + '">' + this.text + '</text></g>';
	},
	updatePosition: function(groupTransform){
		this.$svgNameplate.attr('transform',groupTransform); 
	},
	revive: function(){
		this.$svgNameplate = $('#'+this.getId());
		this.$svgBackground = $('#'+this.getId(ID_NAMEPLATE_BACKGROUNG_SUFFIX));
		this.$svgText = $('#'+this.getId(ID_NAMEPLATE_TEXT_SUFFIX));
		console.debug('$svgNameplate', this.$svgNameplate);
		console.debug('$svgBackground', this.$svgBackground);
		console.debug('$svgText', this.$svgText);
		console.debug('element:', this.$svgText.get(0));
		
		// Sets textWidth from rendered element. 
		this.textWidth=this.$svgText.get(0).getComputedTextLength()+20;
		
		// Set
		var background_x = (this.dx-(this.textWidth/2));
		
		this.$svgBackground.attr("x", background_x);
		this.$svgBackground.attr("width", this.textWidth);
	}
});