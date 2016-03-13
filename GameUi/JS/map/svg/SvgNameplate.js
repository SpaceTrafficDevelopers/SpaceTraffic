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
		var buffer = '';
		this.textWidth = lengthElement.width();
		lengthElement.remove();
		var startCorrection = (typeof (this.relatedObject.size) != "undefined" && this.relatedObject.size !== null)? this.relatedObject.size : 0;
		var hasBase = (typeof (this.relatedObject.hasBase) != "undefined" && this.relatedObject.hasBase !== null && this.relatedObject.hasBase);
		buffer = buffer + '<g x="20" id="' + this.getId() + '" class="nameplate ' + this.cssClass + '" transform="' + groupTransform + '">'
			+ '<path transform="translate(' + startCorrection / 2 + ', ' + -startCorrection + ')" d="m 0,0 14,-52 ' + Math.max(this.minWidth, this.textWidth + this.textPadding) + ',0 -5.04054,18.81159 -' + Math.max(this.minWidth, this.textWidth + this.textPadding) + ',0" id="' + this.getId(ID_NAMEPLATE_BACKGROUNG_SUFFIX) + '" class="nameplateBg" />'
		//+ '<rect id="'+this.getId(ID_NAMEPLATE_BACKGROUNG_SUFFIX)+'" x="'+background_x+'" y="'+background_y+'" ry="10" width="'+this.textWidth+'" height="20" ></rect>'
		+ '<text transform="translate(' + startCorrection / 2 + ', ' + -startCorrection + ')" id="' + this.getId(ID_NAMEPLATE_TEXT_SUFFIX) + '" x="' + (this.textPadding + 3) + '" y="-38">' + this.text + '</text>';
		if (hasBase) {
			buffer = buffer + this.buildNameplateInterface(startCorrection);
		}
		buffer = buffer + '</g>';
		return buffer;
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
	},
	buildNameplateInterface: function (startCorrection) {
		return '<g transform="translate(' + ((startCorrection / 2) + 17) + ', ' + (-startCorrection - 26) + ')" >'
				+ '<g class="baseIconSVG">'
					 +'<circle class="iconBg" r="5" cy="0" cx="5" />'
				   + '<path transform="translate(0, -5)" d="M 5,0.00390625 C 3.9293312,1.0743063 2.8595899,2.1442437 1.7890625,3.2148438 L 2.3613281,3.3515625 5.0019531,0.7109375 9.2910156,5 5.0019531,9.2890625 1.46875,6.4648438 5,9.9960938 9.9980469,5 5,0.00390625 Z M 4.8574219,1.1191406 C 4.081584,1.8950406 3.326619,2.6519344 2.5507812,3.4277344 L 2.8847656,3.5429688 5.0839844,1.34375 4.8574219,1.1191406 Z M 8.09375,4.359375 4.3046875,8.1484375 5.0234375,8.8671875 8.8125,5.078125 8.09375,4.359375 Z M 4.8574219,5.8300781 3.4316406,7.2539062 4.1503906,7.9726562 5.5742188,6.546875 4.8574219,5.8300781 Z"  />'
				   + '<path	transform="translate(0, -5)" d="M 3.9335938 3.6386719 L 3.8535156 3.7597656 L 3.9335938 3.7597656 L 4.0390625 3.6386719 L 3.9335938 3.6386719 z M 3.1230469 3.8164062 L 3.1230469 3.9277344 L 3.4121094 3.9277344 L 3.1230469 4.3378906 L 3.1230469 4.4511719 L 3.5585938 4.4511719 L 3.5585938 4.3378906 L 3.2832031 4.3378906 L 3.5585938 3.9277344 L 3.5585938 3.8164062 L 3.1230469 3.8164062 z M 3.8242188 3.8164062 L 3.6113281 4.4511719 L 3.7402344 4.4511719 L 3.7871094 4.3105469 L 3.9941406 4.3105469 L 4.0390625 4.4511719 L 4.1699219 4.4511719 L 3.9628906 3.8164062 L 3.8242188 3.8164062 z M 4.234375 3.8164062 L 4.234375 4.4511719 L 4.3515625 4.4511719 L 4.3515625 4.1894531 L 4.4003906 4.1894531 L 4.5625 4.4511719 L 4.703125 4.4511719 L 4.5097656 4.1347656 L 4.703125 3.8164062 L 4.5625 3.8164062 L 4.4003906 4.0820312 L 4.3496094 4.0820312 L 4.3496094 3.8164062 L 4.234375 3.8164062 z M 4.7792969 3.8164062 L 4.7792969 4.328125 L 4.7792969 4.3320312 C 4.7792969 4.4140979 4.8223073 4.4550781 4.9101562 4.4550781 L 5.171875 4.4550781 L 5.171875 4.34375 L 4.953125 4.34375 C 4.9380983 4.34375 4.9266381 4.340651 4.9179688 4.3339844 C 4.9058321 4.3293177 4.9003906 4.3149687 4.9003906 4.2929688 L 4.9003906 3.8164062 L 4.7792969 3.8164062 z M 5.4375 3.8164062 L 5.2226562 4.4511719 L 5.3535156 4.4511719 L 5.3984375 4.3105469 L 5.6074219 4.3105469 L 5.6523438 4.4511719 L 5.78125 4.4511719 L 5.5742188 3.8164062 L 5.4375 3.8164062 z M 5.8457031 3.8164062 L 5.8457031 4.4511719 L 6.1152344 4.4511719 C 6.1655164 4.4511719 6.206713 4.4374896 6.2402344 4.4101562 C 6.303231 4.3592896 6.3359375 4.2685187 6.3359375 4.1367188 C 6.3359375 3.9235188 6.2620346 3.8164062 6.1152344 3.8164062 L 5.8457031 3.8164062 z M 6.4199219 3.8164062 L 6.4199219 4.4511719 L 6.5410156 4.4511719 L 6.5410156 4.0449219 L 6.7949219 4.4511719 L 6.9160156 4.4511719 L 6.9160156 3.8164062 L 6.7949219 3.8164062 L 6.7949219 4.2265625 L 6.5429688 3.8164062 L 6.4199219 3.8164062 z M 7.1972656 3.8164062 L 6.984375 4.4511719 L 7.1132812 4.4511719 L 7.1601562 4.3105469 L 7.3671875 4.3105469 L 7.4121094 4.4511719 L 7.5429688 4.4511719 L 7.3359375 3.8164062 L 7.1972656 3.8164062 z M 5.9667969 3.9238281 L 6.1054688 3.9238281 C 6.1291648 3.9238281 6.1490358 3.9287865 6.1640625 3.9394531 C 6.1975838 3.9643198 6.2148438 4.0295656 6.2148438 4.1347656 C 6.2148438 4.272899 6.177713 4.3417969 6.1054688 4.3417969 L 5.9667969 4.3417969 L 5.9667969 3.9238281 z M 3.8945312 3.9765625 L 3.9667969 4.203125 L 3.8183594 4.203125 L 3.8945312 3.9765625 z M 5.5078125 3.9765625 L 5.578125 4.203125 L 5.4316406 4.203125 L 5.5078125 3.9765625 z M 7.2675781 3.9765625 L 7.3398438 4.203125 L 7.1914062 4.203125 L 7.2675781 3.9765625 z " />'
				+ '</g>'
			+ '</g>';
	}
});