$(document).ready(function () {

	module('UI on the page');

	test('logo is shown', function () {
		var $logo = $('#logo:visible');
		strictEqual($logo.length, 1);
		strictEqual($logo.find('h1').text(), "Space Traffic");
	});

	test('menu is there', function () {
		var $menu = $('.mainmenu');
		strictEqual($menu.length, 1);
	});

	test('menu button is shown', function () {
		var $button = $('#menuButton:visible');
		strictEqual($button.length, 1);
		strictEqual($button.text(), "MENU");
	});

	test('menuPanel is there', function () {
		var $panel = $('#menuPanel');
		strictEqual($panel.length, 1);
	});

	test('mainPanel is there', function () {
		var $panel = $('#mainPanel');
		strictEqual($panel.length, 1);
	});

	test('contextPanel is there', function () {
		var $panel = $('#contextPanel');
		strictEqual($panel.length, 1);
	});

	test('infoStream is shown', function () {
		var $stream = $('#infoStream:visible');
		strictEqual($stream.length, 1);
	});

	test('viewport is shown', function () {
		var $element = $('#viewport:visible');
		strictEqual($element.length, 1);
	});

	test('map is shown', function (assert) {
		assert.expect(1);
		var done = assert.async();
		setTimeout(function () {
			var $element = $('#svgCanvas:visible');
			strictEqual($element.length, 1);
			done();
		}, 2000);/*time to render map*/
	});

	test('reference button is shown', function () {
		var $element = $('a:contains("Reference"):visible');
		ok($element.length >= 1);
	});

	test('logout button is shown', function () {
		var $element = $('a:contains("Odhlásit se"):visible');
		ok($element.length >= 1);
	});

});