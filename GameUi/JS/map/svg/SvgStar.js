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
	buildDefs: function (t) {/* definition of pattern used for animation */
		this.setT(t);
		var detail = 2;/* detail * detail = number of squares of pattern */
		var squareMiddle = this.star.size / detail;
		var squareSize = squareMiddle * 2;
		var animationName = 'anim' + this.getId(ID_OBJECT_SUFFIX);
		this.addAnimation(animationName, '{ 100% { fill-opacity: 0; } }');
		var animationNameR1 = 'animR1' + this.getId(ID_OBJECT_SUFFIX);
		this.addAnimation(animationNameR1, '{ 0% { r: 0; fill-opacity: 0.4; }, 100% { r: ' + squareMiddle * 0.6 + '; fill-opacity: 0; } }');
		var animationNameR2 = 'animR2' + this.getId(ID_OBJECT_SUFFIX);
		this.addAnimation(animationNameR2, '{ 0% { r: 0; fill-opacity: 0.25; }, 100% { r: ' + squareMiddle + '; fill-opacity: 0; } }');
		var animationNameR3 = 'animR3' + this.getId(ID_OBJECT_SUFFIX);
		this.addAnimation(animationNameR3, '{ 0% { r: 0; fill-opacity: 0.6; }, 100% { r: ' + squareMiddle * 0.75 + '; fill-opacity: 0; } }');
		var animationNameE1 = 'animE1' + this.getId(ID_OBJECT_SUFFIX);
		this.addAnimation(animationNameE1, '{ 0% { rx: 0; ry: 0; fill-opacity: 0.5; }, 100% { rx: ' + squareMiddle * 0.8 + '; ry: ' + squareMiddle * 0.6 + '; fill-opacity: 0; } }');
		var animationNameE2 = 'animE2' + this.getId(ID_OBJECT_SUFFIX);
		this.addAnimation(animationNameE2, '{ 0% { rx: 0; ry: 0; fill-opacity: 1; }, 100% { rx: ' + squareMiddle * 0.25 + '; ry: ' + squareMiddle * 0.2 + '; fill-opacity: 0; } }');
		var animationNameE3 = 'animE3' + this.getId(ID_OBJECT_SUFFIX);
		this.addAnimation(animationNameE3, '{ 0% { rx: 0; ry: 0; fill-opacity: 0.45; }, 100% { rx: ' + squareMiddle * 0.15 + '; ry: ' + squareMiddle * 0.1 + '; fill-opacity: 0; } }');
		
		Math.seedrandom(this.star.randomSeed);
		var seededRandom = Math.random();
		Math.seedrandom();
		return '<pattern patternTransform="translate(-60, 15) rotate(' + 150 * seededRandom + ')" id="pattern' + this.getId(ID_OBJECT_SUFFIX) + '" x="' + squareMiddle + '" y="' + squareMiddle + '" width="' + squareSize + '" height="' + squareSize + '" patternUnits="userSpaceOnUse" >'
			+ ' <rect x="0" y="0" width="' + squareSize + '" height="' + squareSize + '" style="fill: ' + this.star.colorPrimary + '" />'

			+ ' <circle cx="' + squareMiddle + '" cy="' + 1.5 * squareMiddle + '" r="' + squareMiddle * 0.6 + '" style="stroke: none; fill: ' + this.star.colorSecondary + '; fill-opacity: 0;'
			+ 'animation: ' + animationNameR1 + ' 17s 3s linear infinite";  -moz-animation: ' + animationNameR1 + ' 17s 3s linear infinite; -webkit-animation: ' + animationNameR1 + ' 17s 3s linear infinite;" > '
			+'<animate attributeName="r" from="0" to="' + squareMiddle * 0.6 + '" dur="17s" begin="3s" repeatCount="indefinite" />'
			+ '</circle>'

			+ ' <circle cx="0" cy="' + squareMiddle + '" r="' + squareMiddle + '" style="stroke: none; fill: ' + this.star.colorOptional + '; fill-opacity: 0;' +
			+ 'animation: ' + animationNameR2 + ' 12s 2s linear infinite; -moz-animation: ' + animationNameR2 + ' 12s 2s linear infinite; -webkit-animation: ' + animationNameR2 + ' 12s 2s linear infinite;">'
			+'<animate attributeName="r" from="0" to="' + squareMiddle + '" dur="12s" begin="2s" repeatCount="indefinite" />'
			+ '</circle>'

			+ ' <circle cx="' + squareSize + '" cy="' + squareMiddle + '" r="' + squareMiddle + '" style="stroke: none; fill: ' + this.star.colorOptional + '; fill-opacity: 0;'
			+ 'animation: ' + animationNameR2 + ' 12s 2s linear infinite; -moz-animation: ' + animationNameR2 + ' 12s 2s linear infinite; -webkit-animation: ' + animationNameR2 + ' 12s 2s linear infinite;">'
			+'<animate attributeName="r" from="0" to="' + squareMiddle + '" dur="12s" begin="2s" repeatCount="indefinite" />'
			+ '</circle>'

			+ ' <circle cx="' + squareMiddle + '" cy="' + 0.75 * squareMiddle + '" r="' + squareMiddle * 0.6 + '" style="stroke: none; fill: ' + this.star.colorOptional + '; fill-opacity: 0;'
			+ 'animation: ' + animationNameR1 + ' 5s -3s linear infinite; -moz-animation: ' + animationNameR1 + ' 5s -3s linear infinite; -webkit-animation: ' + animationNameR1 + ' 5s -3s linear infinite;">'
			+'<animate attributeName="r" from="0" to="' + squareMiddle * 0.6 + '" dur="5s" begin="-3s" repeatCount="indefinite" />'
			+ '</circle>'

			+ ' <ellipse  cx="' + squareMiddle + '" cy="' + 0.75 * squareMiddle + '" rx="' + squareMiddle * 0.8 + '" ry="' + squareMiddle * 0.6 + '" style="stroke: none; fill: ' + this.star.colorSecondary + '; fill-opacity: 0;'
			+ 'animation: ' + animationNameE1 + ' 11s -1s linear infinite; -moz-animation: ' + animationNameE1 + ' 11s -1s linear infinite; -webkit-animation: ' + animationNameE1 + ' 11s -1s linear infinite;">'
			+'<animate attributeName="rx" from="0" to="' + squareMiddle * 0.8 + '" dur="11s" begin="-1s" repeatCount="indefinite" /><animate attributeName="ry" from="0" to="' + squareMiddle * 0.6 + '" dur="11s" begin="-1s" repeatCount="indefinite" />'
			+ '</ellipse>'

			+ ' <ellipse  cx="' + 0.4 * squareMiddle + '" cy="' + 0.27 * squareMiddle + '" rx="' + squareMiddle * 0.25 + '" ry="' + squareMiddle * 0.2 + '" style="stroke: none; fill: ' + this.star.colorSecondary + '; fill-opacity: 0;'
			+ 'animation: ' + animationNameE2 + ' 4s 0s linear infinite; -moz-animation: ' + animationNameE2 + ' 4s 0s linear infinite; -webkit-animation: ' + animationNameE2 + ' 4s 0s linear infinite;">'
			+'<animate attributeName="rx" from="0" to="' + squareMiddle * 0.25 + '" dur="4s" begin="0s" repeatCount="indefinite" /><animate attributeName="ry" from="0" to="' + squareMiddle * 0.2 + '" dur="4s" begin="0s" repeatCount="indefinite" />'
			+ '</ellipse>'

			+ ' <ellipse  cx="' + 0.7 * squareMiddle + '" cy="' + 0.44 * squareMiddle + '" rx="' + squareMiddle * 0.15 + '" ry="' + squareMiddle * 0.1 + '" style="stroke: none; fill: ' + this.star.colorSecondary + '; fill-opacity: 0;'
			+ 'animation: ' + animationNameE3 + ' 4s 0s linear infinite; -moz-animation: ' + animationNameE3 + ' 4s 0s linear infinite; -webkit-animation: ' + animationNameE3 + ' 4s 0s linear infinite;">'
			+'<animate attributeName="rx" from="0" to="' + squareMiddle * 0.15 + '" dur="4s" begin="0s" repeatCount="indefinite" /><animate attributeName="ry" from="0" to="' + squareMiddle * 0.1 + '" dur="4s" begin="0s" repeatCount="indefinite" />'
			+ '</ellipse>'

			+ ' <circle cx="' + squareMiddle + '" cy="0" r="' + squareMiddle * 0.75 + '" style="stroke: none; fill: ' + this.star.colorSecondary + '; fill-opacity: 0;'
			+ 'animation: ' + animationNameR3 + ' 30s ease infinite; -moz-animation: ' + animationNameR3 + ' 30s ease infinite; -webkit-animation: ' + animationNameR3 + ' 30s ease infinite;">'
			+'<animate attributeName="r" from="0" to="' + squareMiddle * 0.75 + '" dur="30s" begin="0" repeatCount="indefinite" />'
			+ '</circle>'

			+ ' <circle cx="' + squareMiddle + '" cy="' + squareSize + '" r="' + squareMiddle * 0.75 + '" style="stroke: none; fill: ' + this.star.colorSecondary + '; fill-opacity: 0;'
			+ 'animation: ' + animationNameR3 + ' 30s ease infinite; -moz-animation: ' + animationNameR3 + ' 30s ease infinite; -webkit-animation: ' + animationNameR3 + ' 30s ease infinite;">'
			+'<animate attributeName="r" from="0" to="' + squareMiddle * 0.75 + '" dur="30s" begin="0" repeatCount="indefinite" />'
			+ '</circle>'
			+ '</pattern>';
	},
	buildObject : function(t) {
		this.setT(t);
		return ''
			+ '<circle id="' + this.getId(ID_OBJECT_SUFFIX) + '" cx="0" cy="0" r="' + this.star.size + '" class="star" style="stroke-width: ' + this.star.size * 2 + ';stroke-opacity: 0.1;stroke: ' + this.star.colorPrimary + ';fill: url(#pattern' + this.getId(ID_OBJECT_SUFFIX) + ');"/>';
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