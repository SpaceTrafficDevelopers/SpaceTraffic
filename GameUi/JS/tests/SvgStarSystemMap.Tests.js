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

	module('SvgStarSystemMap.js: SvgOrbitFactory');
	
	test('SvgOrbitFactory:buildCurcularOrbit', function() {
		var circularOrbit = new CircularOrbit(30,0,'clockwise',0);
		circularOrbit.cx = 10;
		circularOrbit.cy = 20;
		var result = SvgOrbitFactory.buildCurcularOrbit(circularOrbit, 'testId');
		var expected = '<circle id="testId" cx="10" cy="20" r="30" class="orbit"/>';
		strictEqual(result,expected, 'Expecting properly formed svg circle tag.');
	});
	
	test('SvgOrbitFactory:buildEllipticOrbit', function() {
		var ellipticOrbit = new EllipticOrbit(30,40,0,'clockwise',50,0);
		ellipticOrbit.cx = 10;
		ellipticOrbit.cy = 20;
		var result = SvgOrbitFactory.buildEllipticOrbit(ellipticOrbit, 'testId');
		var expected = '<ellipse id="testId" cx="10" cy="20" rx="30" ry="40" class="orbit" transform="rotate(50)"/>';
		strictEqual(result,expected, 'Expecting properly formed svg ellipse tag with rotation.');
	});
	
	module('SvgStarSystemMap.js: TimeUtils');
	
	test('TimeUtils:getCurrentTimeInS', function() {
		var currentTime =  (new Date()).getTime() / 1000;
		var result = TimeUtils.getCurrentTimeInS();
		strictEqual(Math.floor(result), Math.floor(currentTime), 'Expecting current time (may not work on slow machine).');
	});
	
	test('TimeUtils:getDiffTimeToCurrentInS', function() {
		var currentTime =  (new Date()).getTime() / 1000;
		var result = TimeUtils.getDiffTimeToCurrentInS(currentTime);
		strictEqual(result, 0, 'Expecting no difference.');
		
		var oldTime =  ((new Date()).getTime()-1000) / 1000;
		var result = TimeUtils.getDiffTimeToCurrentInS(oldTime);
		strictEqual(Math.floor(result), 1, 'Expecting difference of 1s.');
	});
	
});