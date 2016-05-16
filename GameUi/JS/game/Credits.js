/* loading of amout of players credits */
$(document).ready(function () {
		ajax.send({
			requestId: 'CreditsAmount',
			relatedObject: 'CreditsAmount',
			data: {},
			repeatEvery: 5,
			callback: function (credits) {
				$('.creditsAmount').each(function () {
					var before = $(this).text();
					$(this).text(credits);
					if (before != '-' && before != credits) {
						if (before > credits) {
							$(this).css('color', 'red');
						} else {
							$(this).css('color', 'green');
						}
						$(this).stop().animate({
							color: "white"
						}, 2500);
					}					
				});
			}
		});
});