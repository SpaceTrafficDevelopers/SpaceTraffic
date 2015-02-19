
// Orbit of space object defined by orbital eccentricity, period and direction of movement.
// Subclass of Trajectory.

const CLOCKWISE = 1;
const COUNTERCLOCKWISE = -1;

var Orbit = new Class({
	Extends: Trajectory,
	// Orbital period in seconds.
	period: 0.0,
	
	// Direction of movement on orbit as 1 for clockwise and -1 for counterclockwise.
	directionModifier: CLOCKWISE,
	
	// Orbital eccentricity e.
	orbitalEccentricity: 0.0,
	
	cx: 0,
	
	cy: 0,
	
	// Constructor
	initialize: function(direction){
		this.directionModifier = (direction == 'clockwise') ? CLOCKWISE : COUNTERCLOCKWISE;
	}
})