/**
http://10consulting.com/2011/08/29/asp-dot-net-mvc-flash-messages-with-razor-view-engine/
https://gist.github.com/13orno/d44d0117f17bcd9d0cb7

*/

; (function ($) {

	$.fn.showalert = function (options, alerttype) {
		var target = this;
		target.append('<div class="alert alert-' + alerttype.toString().toLowerCase() + ' fade in removealert"><a class="close" data-dismiss="alert">&times;</a><span>' + options.message + '</span></div>')
		if (options.timeout >= 0) {
			$(".removealert").delay(options.timeout).fadeOut(options.fadeout);
		}
		
	}


	$.fn.flashMessage = function (options) {
		var target = this;
		options = $.extend({ timeout: 3000, fadeout: 400, alert: 'info' }, options);

		if (!options.message) {
			setFlashMessageFromCookie(options);
		}

		//adding function to closing buttons
		target.click(function (e) {
			var clicktarget = $(e.target);
			if (clicktarget.hasClass('close')) {
				clicktarget.parent('.alert').fadeOut(options.fadeout);
			}
		});


		// Get the first alert message read from the cookie
		function setFlashMessageFromCookie() {
			$.each(new Array('Success', 'Error', 'Warning', 'Info'), function (i, alert) {

				var cookie = $.cookie("Flash." + alert);

				if (!jQuery.isEmptyObject(cookie)) {
					options.message = cookie;
					options.alert = alert;
					target.showalert(options, options.alert);
					deleteFlashMessageCookie(alert);
					return;
				}
			});
		}

		// Delete the named flash cookie
		function deleteFlashMessageCookie(alert) {
			$.cookie("Flash." + alert, null, { path: '/' });
		}
	};


}(jQuery));