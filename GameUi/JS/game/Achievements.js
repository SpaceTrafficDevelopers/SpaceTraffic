$(document).ready(function () {
	setTimeout(function () {
		$.getJSON('/' + 'Achievement/GetEarnedAchievements', function (unviewedAchievements) {
			if (jQuery.isEmptyObject(unviewedAchievements)) {
				return;
			}

			$.each(unviewedAchievements, function (index, achievement) {
				var achvPopup = $("#achievements");
				achvPopup.queue(function () {
					$(".name").text(achievement.Name);
					$(".icon img").attr("src", achievement.Image);
					$(this).dequeue();
				}).slideDown(400).delay(2000).slideUp(400);
			});
		});
	}, 1500);	
});