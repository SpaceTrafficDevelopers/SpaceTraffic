/**
 * @author Azaroth
 */


// Circular orbit, defined by radius and initialAngle of the orbiting object.
// Subclass of Orbit
var CircularOrbit = new Class({
	Extends: Orbit,
	// Circle radius.
	radius: 1,
	
	// Angular velocity - omega - in radians per second.
	period: 1,
	
	// Angle to initial position of the orbiting object in radians.
	initialAngleInRad: 0,
	
	// Constructor
	initialize: function(radius, period, direction, initialAngleDeg){
		this.cx = 0;
		this.cy = 0;
		this.parent(direction);
		this.orbitalEccentricity = 0.0;
		this.radius = radius;
		this.initialAngleInRad = initialAngleDeg*Math.PI/180;
		this.period = period;
		this.angularVelocity = PIx2 /this.period;
	},
	
	calculatePosition: function(timeInSec){
		var alpha = (this.directionModifier*(this.angularVelocity)*timeInSec+this.initialAngleInRad)%PIx2;
		return new Point(this.radius * Math.cos(alpha), this.radius * Math.sin(alpha));
	}	
})