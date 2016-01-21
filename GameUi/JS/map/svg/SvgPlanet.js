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

	}
});