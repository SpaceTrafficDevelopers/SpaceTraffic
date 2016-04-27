$(document).ready(function () {

	module('AJAX');

	test('ajax lib variable exists', function () {
		strictEqual(typeof ajax, 'object', 'ajax variable does not exists!');
	});

	test('ajax clock runs', function (assert) {
		assert.expect(2);
		var done = assert.async();
		ok($.fn.ajaxRequester.clock >= 0);
		var clockBefore = $.fn.ajaxRequester.clock;
		setTimeout(function () {
			strictEqual($.fn.ajaxRequester.clock, clockBefore + 1);
			done();
		}, 1200);
	});

	test('all ajax links ready', function (assert) {
		$('a.ajax').each(function () {
			ok(jQuery._data(this, "events").click.length > 0);
		});

	});

	test('all ajax forms ready', function (assert) {
		$('.ajax form, form.ajax').each(function () {
			ok(jQuery._data(this, "events").submit.length > 0);
		});
		ok(true);
	});

	test('repeatitive credits amount update works', function (assert) {
		assert.expect(2);
		var done1 = assert.async();
		var done2 = assert.async();
		var callbackCount = 0;
		ajax.send({
			requestId: 'CreditsAmountTest',
			relatedObject: 'CreditsAmount',
			data: {},
			repeatEvery: 1,
			callback: function (credits) {
				callbackCount++;
				if (callbackCount > 2) return;
				setTimeout(function () {
					strictEqual(credits, $('.creditsAmount').text());
					if (callbackCount == 1) done1();
					if (callbackCount == 2) done2();
				}, 500);/*let component have time to change */
			}
		});
	});

	test('single credits amount update works', function (assert) {
		assert.expect(1);
		var done1 = assert.async();
		ajax.send({
			requestId: 'CreditsAmountTest2',
			relatedObject: 'CreditsAmount',
			data: {},
			callback: function (credits) {
				setTimeout(function () {
					strictEqual(credits, $('.creditsAmount').text());
					done1();
				}, 500);/*let component have time to change */
			}
		});
	});

});