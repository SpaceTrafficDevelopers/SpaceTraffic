/* loading of number of players ships etc. */
$(document).ready(function () {
	ajax.send({
		requestId: 'ShipsInfo',
		relatedObject: 'ShipsInfo',
		data: {},
		repeatEvery: 5,
		callback: function (ships) {
			$('.shipsAmount').each(function () {
				$(this).text(ships.amount);
			});
		}
	});
});