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
		//TODO: Parametrize
		return '<circle id="' + this.getId(ID_OBJECT_SUFFIX) + '" cx="0" cy="0" r="' + this.body.size +
			'"class="' + this.cssClassPrefix + '" transform="' + transform + '"/>';
	},
});