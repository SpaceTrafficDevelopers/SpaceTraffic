/**
 * @author Azaroth
 */


// Elliptic orbit, defined by major (a) and minor (b) axis, rotationAngle from x-axis and initial angle of the orbiting object.
// Subclass of Orbit.
var EllipticOrbit = new Class({
	Extends: Orbit,
	
	// Major axis
	a: 0,
	
	// Minor axis
	b: 0,
	// Rotation angle from x-axis.
	rotationAngleInDeg: 0,
	// Angle to initial position of the orbiting object in radians.
	initialAngleInRad: 0,
	
	// Semi-latus rectum
	_p: 0.0,
	
	// Square root of (1+e)/(1-e) where e is orbitalEccentricity
	_sqrt1pluseSlash1minuse: 0.0,
	
	// Constructor initializes new instance.
	// It also performs initial computations like calculation period, 
	initialize: function(a, b, period, direction, rotationAngleInDeg, initialAngleDeg){
		this.parent(direction);
		this.a = a;
		this.b = b;
		var a2 = a*a;
		var b2 = b*b;
	
		// eccentricity e = sqrt[(a^2 - b^2)/a^2]
		this.orbitalEccentricity = Math.sqrt((a2-b2)/a2);
		this.rotationAngleInDeg = rotationAngleInDeg;
		this.initialAngleInRad = initialAngleDeg*Math.PI/180;
		this.cx = this.orbitalEccentricity*this.a;
		this.cy = 0;
		
		// Perimeter approximation as (PI * {a + b + sqrt[ 2 * (a^2 + b^2)]})/2
		//var perimeter = ((Math.PI*(this.a+this.b+Math.sqrt(2*(a2+b2))))/2);
		this.period = period;
		
		// p = a * (1 - e^2).
		this._p = this.a *(1-this.orbitalEccentricity*this.orbitalEccentricity);
		// saving rsult of sqrt[(1+e)/(1-e)]
		this._sqrt1pluseSlash1minuse = Math.sqrt( (1+this.orbitalEccentricity) / (1-this.orbitalEccentricity) );
		
	},
	
	calculatePosition: function(timeInSec){
		var M = (PIx2 * timeInSec) / this.period;
		var E = this.calculateE(M);
		var theta = this.directionModifier * 2 * Math.atan( this._sqrt1pluseSlash1minuse * Math.tan(E / 2) );
    	var r = this._p /(1 + this.orbitalEccentricity * Math.cos(theta));
    	var x1 = r * Math.cos(theta);
    	var y1 = r * Math.sin(theta);
		return new Point(-x1-this.cx,y1-this.cy);//(this.a*(1-this.e)-x1+1), (this.b+y1+1));
	},
	
	calculateE: function(M){
	    var Enew=1;
	    var Eold=0;
	    var Etemp=0;
	    var E=0;
	    //change the value in the next line for different accuracy
	    //of value of E found from M
	    while(Math.abs(Enew-Eold)>0.00001){
	        Etemp=Enew;
	        Enew=M+this.orbitalEccentricity*Math.sin(Eold);
	        Eold=Etemp;
		}
		
	    var E=Enew;
	    return E;
    }
})