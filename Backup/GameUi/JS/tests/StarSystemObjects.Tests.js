/// <reference path="../jquery-1.6.4-vsdoc.js" />
/// <reference path="../jquery-ui-1.8.16.js" />
/// <reference path="../qunit.js" />
/// <reference path="SvgStarSystemMap.js" />
/// <reference path="StarSystemObjects.js" />
/// <reference path="SvgStarSystemMap.js" />
/**
 * @author Azaroth
 */
$(document).ready(function(){

	module('SvgStarSystemObjects.js: EllipticOrbit');
	
	test('EllipticOrbit:construction', function() {
		
		var a = 10;
		var b = 9;
		var velocity = 50;
		var direction = 'clockwise';
		var rotationAngleInDeg = 90; //90°
		var initialAngleInDeg = 90;
		var ellipticOrbit = new EllipticOrbit(a,b,velocity, direction, rotationAngleInDeg, initialAngleInDeg);
		
		strictEqual(ellipticOrbit.a, a, 'Expecting value of semimajor axis');
		strictEqual(ellipticOrbit.b, b, 'Expecting value of semiminor axis');
		strictEqual(ellipticOrbit.directionModifier, CLOCKWISE, 'Expecting value of directionModifier as CLOCKWISE constant');
		ok(ellipticOrbit.orbitalEccentricity > 0, 'Eccentricity should be greater than 0');
		ok(ellipticOrbit.orbitalEccentricity < 1, 'Eccentricity should be less than 1');
		ok(ellipticOrbit.period > 0, 'Period should be greater than 0');
		ok(ellipticOrbit.cx > 0, 'Period should be greater than 0');
	});
	
	test('EllipticOrbit:calculatePosition', function() {
		
		var a = 10;
		var b = 10;
		var velocity = 50;
		var direction = 'clockwise';
		var rotationAngleInDeg = 0; //90°
		var initialAngleInDeg = 0;
		var ellipticOrbit = new EllipticOrbit(a,b,velocity, direction, rotationAngleInDeg, initialAngleInDeg);
		var result = ellipticOrbit.calculatePosition(1000);
		ok((result instanceof Point), 'Should be Point instance');
		ok((!isNaN(result.x)), 'x should be number');
		ok((!isNaN(result.y)), 'y should be number');
		console.debug('RESULTX:', result.x);
		console.debug('RESULTY:', result.y);
	});	
});