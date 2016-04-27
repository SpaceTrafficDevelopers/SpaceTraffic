$(document).ready(function () {

	module('StarSystemMap UI');

	setTimeout(function () {

		module('StarSystemMap UI');

		test('defs are there', function (assert) {
			var $element = $('#svgCanvas defs');
			strictEqual($element.length, 1);
		});

		test('star is there', function (assert) {
			var $element = $('#svgCanvas .star');
			strictEqual($element.length, 1);
		});

		test('planets are there', function (assert) {
			var $element = $('#svgCanvas .planet');
			ok($element.length > 0);
		});

		test('wormholes are there', function (assert) {
			var $element = $('#svgCanvas .wormholeEndpoint');
			ok($element.length > 0);
		});

		test('nameplates are there', function (assert) {
			var $element = $('#svgCanvas .nameplate');
			var $planets = $('#svgCanvas .planet');
			var $wormholes = $('#svgCanvas .wormholeEndpoint');
			ok($element.length > 0);
			strictEqual($element.length, $planets.length + $wormholes.length);
		});

		test('bases are there', function (assert) {
			var $element = $('#svgCanvas g.baseIconSVG');
			ok($element.length > 0);
		});

		test('background stars are there', function (assert) {
			var $elements = $('#bgDistantStars, #bgCloserStars, #bgMiddleDistanceStars');
			strictEqual($elements.length, 3);
		});

		test('patterns are there', function (assert) {
			var $star = $('#svgCanvas .star');			
			var $planets = $('#svgCanvas .planet');
			var $starsBG = $('#bgDistantStars, #bgCloserStars, #bgMiddleDistanceStars');
			var $elements = $('#svgCanvas defs pattern');
			strictEqual($elements.length, $star.length + $planets.length + $starsBG.length);
		});

	}, 1500);/*time to render map*/

});