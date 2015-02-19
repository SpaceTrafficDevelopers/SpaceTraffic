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
	}
});