/**
 * @author Azaroth
 */

var SvgPlanet = new Class({
	Extends: SvgOrbitingBody,
	initialize: function(id, planet){
		this.parent('planet'+id, planet);
		this.cssClassPrefix = "planet";
	},
	getName: function(){
		return (this.body.altName ? this.body.altName : this.body.name);
	},
	onclickHandler: function (sender) {
		this.showPlanetDetails($('#contextPanel'));
		this.showPlanetInfo($('#infoPanel'));
	},

	showPlanetInfo: function ($element) {/* info about planet on the left*/
		$element.html('<h2>' + this.body.altName + '</h2><p>' + this.body.description + '</p>');
		$element.css('display', 'block');
	},
	showPlanetDetails: function ($element) {/* detailed UI of planet on the right */

	},
	/* override */
	buildObject: function (t) {
		this.setT(t);
		var point = this.body.trajectory.calculatePosition(this.getT());
		var transform = SvgOrbitFactory.getTransformationForPointOnOrbit(point, this.body.trajectory);
		switch(this.body.type.trim().toLowerCase()){
			case 'terrastrial':
				return this.buildTerrastrialObject(point, transform);
			case 'terrastrialT3':
				return this.buildTerrastrialT3Object(point, transform);
			case 'gas':
				return this.buildGasObject(point, transform);
			default:
				return this.buildTerrastrialObject(point, transform);
		}
	},
	buildDefs: function (t) {/* definition of pattern used for animation */
		this.setT(t);
		Math.seedrandom(this.body.randomSeed);

		switch(this.body.type.trim().toLowerCase()){
			case 'terrastrial':
				var buff = this.buildTerrastrialDefs();
				break;
			case 'terrastrialT3':
				var buff = this.buildTerrastrialT3Defs();
				break;
			case 'gas':
				var buff = this.buildGasDefs();
				break;
			default:
				var buff = this.buildTerrastrialDefs();
				break;
		}

		Math.seedrandom();
		return buff;
	},
	buildTerrastrialObject: function(point, transform) {
		return '<circle id="' + this.getId(ID_OBJECT_SUFFIX) + '" cx="0" cy="0" r="' + this.body.size +
			'"class="' + this.cssClassPrefix + '" transform="' + transform + '" style="fill: url(#pattern' + this.getId(ID_OBJECT_SUFFIX) + ')"/>';
	},
	buildTerrastrialT3Object: function(point, transform) {
		return '<circle id="' + this.getId(ID_OBJECT_SUFFIX) + '" cx="0" cy="0" r="' + this.body.size +
			'"class="' + this.cssClassPrefix + '" transform="' + transform + '" style="fill: url(#pattern' + this.getId(ID_OBJECT_SUFFIX) + ')"/>';
	},
	buildGasObject: function(point, transform){
		var buff = '<g id="' + this.getId(ID_OBJECT_SUFFIX) + '" transform="' + transform + '">';
		if (this.body.hasRing) {
			buff = buff + '<path transform="translate(-' + this.body.size * 2 + ', ' + this.body.size * 1.3 + ') scale(' + this.body.size + ')" style="fill: ' + this.body.ringColorSecondary + '; fill-opacity: 0.7;" d="M -0.16009811,0.19932816 0.07232384,0.02550001 C -0.10889376,-0.25130299 0.53221239,-1.0553369 1.5352144,-1.7987187 2.5369605,-2.5411214 3.5067505,-2.9293772 3.7207613,-2.6776249 L 3.9609957,-2.8553594 C 3.7345213,-3.1615431 2.628702,-2.7244501 1.4902925,-1.8807501 0.35173179,-1.0369897 -0.38741741,-0.10628204 -0.16009811,0.19932816 Z m 0.6679688,-0.4941407 0.00586,-0.00391 c -0.00586,0.00391 0,-0.00195 0,-0.00195 0,0 -0.025154,-0.054395 -0.023438,-0.089844 -0.00415,0.038257 0.00172,0.07001 0.017578,0.095704 z m -0.017578,-0.095703 c 0.00158,-0.032723 0.01294,-0.071895 0.029297,-0.1132812 -0.018035,0.042048 -0.025636,0.07957 -0.029297,0.1132812 z" />'
			buff = buff + '<path transform="translate(-' + this.body.size * 2 + ', ' + this.body.size * 1.3 + ') scale(' + this.body.size + ')" style="fill: ' + this.body.ringColorPrimary + '; fill-opacity: 0.7;" d="m 3.7328484,-2.6993176 c -0.1995095,-0.268719 -1.1820812,0.1218089 -2.194773,0.8723237 -1.01254536,0.7504549 -1.67184226,1.5766903 -1.47276476,1.845679 m 0.4468418,-0.3310263 c -0.1419352,-0.1906532 0.3641802,-0.8056425 1.13024196,-1.373376 0.7659965,-0.5676518 1.5015898,-0.8729229 1.6427944,-0.6817595" />'
		}
		buff = buff + '<circle cx="0" cy="0" r="' + this.body.size + '"class="' + this.cssClassPrefix + '" style="fill: url(#pattern' + this.getId(ID_OBJECT_SUFFIX) + ')"/>'
		if (this.body.hasRing) {
			buff = buff + '<path transform="translate(-' + this.body.size * 2 + ', ' + this.body.size * 1.3 + ') scale(' + this.body.size + ')" style="fill: ' + this.body.ringColorSecondary + '; fill-opacity: 0.7;" d="M 3.9589844,-2.8554688 3.7382812,-2.6933594 C 3.9194988,-2.4165564 3.2666739,-1.6008037 2.2636719,-0.85742188 1.2619258,-0.11501917 0.29213577,0.27323657 0.078125,0.02148438 L -0.16210938,0.19921875 C 0.06436499,0.50540245 1.1701843,0.06830947 2.3085938,-0.77539062 3.4471545,-1.6191509 4.1863037,-2.5498586 3.9589844,-2.8554688 Z m -0.6679688,0.4941407 -0.00586,0.00391 c 0.00586,-0.00391 0,0.00195 0,0.00195 0,0 0.025154,0.054395 0.023438,0.089844 0.00415,-0.038257 -0.00172,-0.07001 -0.017578,-0.095703 z m 0.017578,0.095703 c -0.00158,0.032723 -0.01294,0.071895 -0.029297,0.1132812 0.018035,-0.042048 0.025636,-0.07957 0.029297,-0.1132812 z" />'
			buff = buff + '<path transform="translate(-' + this.body.size * 2 + ', ' + this.body.size * 1.3 + ') scale(' + this.body.size + ')" style="fill: ' + this.body.ringColorPrimary + '; fill-opacity: 0.7;" d="M 0.06891405,0.0158384 C 0.26842363,0.2845575 1.2509953,-0.10597057 2.2636871,-0.85648527 3.2762325,-1.6069402 3.9355294,-2.4331756 3.7364519,-2.7021643 M 3.2896101,-2.371138 c 0.1419352,0.1906532 -0.3641802,0.8056425 -1.130242,1.37337603 -0.7659965,0.5676517 -1.5015898,0.8729228 -1.64279436,0.6817595" />'
		}
		buff = buff + '</g>';
		return buff;
	},
	buildTerrastrialDefs: function(point, transform) {
		var planetSize = this.body.size * 2;
		var animationName = 'animPlanet' + this.getId(ID_OBJECT_SUFFIX);
		var animateDur = this.body.rotationPeriod;
		this.addAnimation(animationName, '{ 0%{ cx: ' + (planetSize * 1.5) + ';}, 100% { cx:' + -(planetSize * 1.5) + '; } }');
		var buff = '<pattern id="pattern' + this.getId(ID_OBJECT_SUFFIX) + '" x="' + this.body.size + '" y="' + this.body.size + '" width="' + planetSize + '" height="' + planetSize + '" patternUnits="userSpaceOnUse" >'
			+ ' <rect x="0" y="0" width="' + planetSize + '" height="' + planetSize + '" style="fill: ' + this.body.colorPrimary + '" />';
		for (var i = 0; i < ((Math.random() * 100 % 10) + 2) ; i++) {/* 2 - 12 times*/
			var animateBegin = -(Math.random() * this.body.rotationPeriod);
			buff = buff + ' <circle cx="' + -(this.body.size) + '" cy="' + (0.1 + Math.random()) * planetSize + '" r="' + Math.random() * 0.25 * planetSize + '" style="stroke: none; fill: ' + this.body.colorSecondary + '; fill-opacity: ' + Math.min(1, Math.random() + 0.5) + '; animation: ' + animationName + ' ' + animateDur + 's ' + animateBegin + 's linear infinite;-webkit-animation: ' + animationName + ' ' + animateDur + 's ' + animateBegin + 's linear infinite;-moz-animation: ' + animationName + ' ' + animateDur + 's ' + animateBegin + 's linear infinite;" >';
			if (!$.browser.webkit) {
				buff = buff + '<animate attributeName="cx" from="' + (planetSize * 1.5) + '" to="' + -(planetSize * 1.5) + '" dur="' + animateDur + '" begin="' + animateBegin + '" repeatCount="indefinite" />';
			}
			buff = buff + '</circle>';
		}
		buff = buff
			//+ '<path transform="scale(' + this.body.size + ', 1) translate(0, ' + this.body.size / 2 + ')" style="fill: ' + this.body.colorOptional + ';fill-opacity: 0.8;" d="m 0.38476562,0.0099925 a 0.27027029,0.15624849 0 0 0 -0.26953124,0.1563 0.27027029,0.15624849 0 0 0 0.23046874,0.1562 0.3466005,0.1400649 0 0 0 -0.0800781,0.09 0.3466005,0.1400649 0 0 0 0,0.014 A 0.33108109,0.20089091 0 0 0 0,0.6232925 a 0.33108109,0.20089091 0 0 0 0.33203125,0.1992 0.33108109,0.20089091 0 0 0 0.33007813,-0.1992 0.33108109,0.20089091 0 0 0 -0.0195313,-0.07 0.3466005,0.1400649 0 0 0 0.31640626,-0.1406 0.3466005,0.1400649 0 0 0 -0.34765626,-0.1407 0.3466005,0.1400649 0 0 0 -0.0292969,0 0.27027029,0.15624849 0 0 0 0.0742188,-0.1074 0.27027029,0.15624849 0 0 0 -0.27148438,-0.1563 z m 0.91601558,0 a 0.32284629,0.18303396 0 0 0 -0.32226558,0.1836 0.32284629,0.18303396 0 0 0 0.32226558,0.1836 0.32284629,0.18303396 0 0 0 0.3222657,-0.1836 0.32284629,0.18303396 0 0 0 -0.3222657,-0.1836 z m 0.4902344,0.4297 a 0.17905407,0.07142788 0 0 0 -0.1484375,0.07 0.17905407,0.07142788 0 0 0 0.1777344,0.072 A 0.17905407,0.07142788 0 0 0 2,0.5099925 a 0.17905407,0.07142788 0 0 0 -0.1796875,-0.07 0.17905407,0.07142788 0 0 0 -0.029297,0 z m -0.5546875,0.1074 a 0.37837839,0.16406038 0 0 0 -0.37890622,0.1641 0.37837839,0.16406038 0 0 0 0.37890622,0.1641 0.37837839,0.16406038 0 0 0 0.3789063,-0.1641 0.37837839,0.16406038 0 0 0 -0.3789063,-0.1641 z m -0.5722656,0.3125 a 0.125,0.07589213 0 0 0 -0.10351562,0.076 0.125,0.07589213 0 0 0 0.125,0.074 0.125,0.07589213 0 0 0 0.125,-0.074 0.125,0.07589213 0 0 0 -0.125,-0.076 0.125,0.07589213 0 0 0 -0.0214844,0 z">'
			//+ ' <animateTransform attributeName="transform" begin="0s" dur="10s" type="translate" from="10 0" to="-10 0" repeatCount="indefinite"/></path>'
			+ '</pattern>';
		return buff;
	},
	buildTerrastrialT3Defs: function(point, transform) {
		return this.buildTerrastrialDefs(point, transform);
	},
	buildGasDefs: function(point, transform){
		var planetSize = this.body.size * 2;
		var animationName = 'animPlanet' + this.getId(ID_OBJECT_SUFFIX);
		var animateDur = this.body.rotationPeriod;
		this.addAnimation(animationName, '{ 0%{ cx: ' + (planetSize * 2) + ';}, 100% { cx:' + -(planetSize * 2) + '; } }');
		var buff = '<pattern id="pattern' + this.getId(ID_OBJECT_SUFFIX) + '" x="' + this.body.size + '" y="' + this.body.size + '" width="' + planetSize + '" height="' + planetSize + '" patternUnits="userSpaceOnUse" >'
			+ ' <rect x="0" y="0" width="' + planetSize + '" height="' + planetSize + '" style="fill: ' + this.body.colorPrimary + '" />';
		for (var i = 0; i < ((Math.random() * 100 % 10) + 5) ; i++) {/* 5 - 15 times*/
			var rx = Math.random() * planetSize;
			var animateBegin = -(Math.random() * this.body.rotationPeriod);
			buff = buff + '<ellipse cx="' + -(this.body.size) + '" cy="' + ((0.1 + Math.random()) * planetSize) + '" rx="' + rx + '" ry="' + (rx * 0.2) + '" style="stroke: none; fill: ' + ((Math.random() < 0.7) ? this.body.colorSecondary : this.body.colorOptional) + '; fill-opacity: ' + Math.random() + '; animation: ' + animationName + ' ' + animateDur + 's ' + animateBegin + 's linear infinite;-webkit-animation: ' + animationName + ' ' + animateDur + 's ' + animateBegin + 's linear infinite;-moz-animation: ' + animationName + ' ' + animateDur + 's ' + animateBegin + 's linear infinite;" >';
			if (!$.browser.webkit) {
				buff = buff + '<animate attributeName="cx" from="' + (planetSize * 2) + '" to="' + -(planetSize * 2) + '" dur="' + animateDur + '" begin="' + animateBegin + '" repeatCount="indefinite" />';
			}
			buff = buff + '</ellipse>';
		}
		buff = buff
			+ '</pattern>';
		return buff;
	}
});