; (function ($) {


	/* component that can send single and repeatitive ajax requests. theese requests are parsed on server side and related object is created and called */
	$.fn.ajaxRequester = function (opts) {


		return {
			/* parameters necessary for every request */
			necessaryParameters: ['requestId', 'relatedObject', 'data', 'callback'],
			/** sends request to parse on server into object */
			send: function (requestObject) {
				if (validateRequest(requestObject, this.necessaryParameters)) {
					if ('repeatEvery' in requestObject) {
						requestObject.lastSentAt = -1;
						$.fn.ajaxRequester.registeredRepeatedRequests.push(requestObject);
					} else {
						$.fn.ajaxRequester.registeredSingleRequests.push(requestObject);
						sendRequest(getCombinedRequestObjects(requestObject));
					}
				}
			},
			/** returns request with given id that is repeatedly called */
			getRequest: function (requestId) {
				return getRegisteredRequest(requestId);
			},
			/** stops repeatitive sending by request with given id */
			stopSending: function (requestId) {
				removeRegisteredRequest(requestId);
			},

		}

	}

	/**
		Validates given object for necessary parameters
	*/
	function validateRequest(requestObject, necessaryParameters) {
		if('requestId' in requestObject){
			var isOk = true;
			isOk = !checkDuplicity(requestObject.requestId);
			$.each(necessaryParameters, function(index, parameter){
				if (!(parameter in requestObject)) {
					alert('Missing parameter "' + parameter + '" in ajax object with id: "' + requestObject.requestId + '" See documentation about Ajax at SpaceTraffic wiki.');
					isOk = false;
				}
			});
			return isOk;
		}else{
			alert("RequestId parameter is missing in ajax object! See documentation about Ajax at SpaceTraffic wiki.");
		}
		return false;
	}

	/**
		returns repeatitive request registered in variable registeredRepeatedRequests
	*/
	function getRegisteredRequest(requestId) {
		var returnObject = false;
		$.each($.fn.ajaxRequester.registeredRepeatedRequests, function (index, requestObject) {
			if (requestObject.requestId == requestId) {
				returnObject = requestObject;
				return;
			}
		});
		return returnObject;
	}

	/**
		returns repeatitive request registered in variable registeredRepeatedRequests
	*/
	function removeRegisteredRequest(requestId) {
		$.each($.fn.ajaxRequester.registeredRepeatedRequests, function (index, object) {
			if (requestObject.requestId == requestId) {
				delete $.fn.ajaxRequester.registeredRepeatedRequests[index];
				return true;
			}
		});
		return false;
	}

	/* checks duplicity of given object and alerts if is already in requests array. also returns if there is duplicity.*/
	function checkDuplicity(requestId) {
		var requestInArray = getRegisteredRequest(requestId);
		
		if (!requestInArray || requestInArray == 'undefined') {
			return false;
		} else {
			alert('Duplicate key ' + requestId +' : requestId must be unique for each repeatitive request. Try another requestId.');
			return true;
		}
	}

	/* Sends complete request on server. Do not use without validating request first.*/
	function sendRequest(data) {
		$.fn.ajaxRequester.ajaxLock = true;
		$.post(baseUrl + 'Ajax', JSON.stringify(data), function (data, textStatus) {
			$.each(data, function (key, objectData) {
				handleSingleResponse(objectData);
			});
		}).always(function () {
			$.fn.ajaxRequester.ajaxLock = false;
		});
	}

	/* called when data arrives for each response object */
	function handleSingleResponse(responseObject) {
		if (responseObject.error && responseObject.error !== 'undefined') {
			alert('Object ' + responseObject.requestId + ' ' + responseObject.error);
		} else {
			doCallback($.fn.ajaxRequester.registeredRepeatedRequests, responseObject, false);
			doCallback($.fn.ajaxRequester.registeredSingleRequests, responseObject, true);
		}
	}

	/** searches for callback in given register and call it. returns false if callback does not exist. Callback can be removed after call if removeAfter is set	*/
	function doCallback(registerArray, responseObject, removeAfter) {
		var registeredObjects = $.grep(registerArray, function (e) { return e.requestId == responseObject.requestId; });
		if (registeredObjects.length > 0) {
			registeredObjects[0].callback(responseObject.data);
			if (removeAfter) {
				var index = registerArray.indexOf(registeredObjects[0]);
				registerArray.splice(index, 1);
			}
			return true;
		}
		return false;
	}

	/* builds data for request - it combines every repeatitive request that should be send now and if there is some it sends additional request also */
	function getCombinedRequestObjects(additionalObject) {
		var outputArray = [];
		if (additionalObject && additionalObject != 'undefined') {
			outputArray.push(additionalObject);
		}
		$.each($.fn.ajaxRequester.registeredRepeatedRequests, function (index, requestObject) {
			if (requestObject.lastSentAt == -1 || ($.fn.ajaxRequester.clock - requestObject.lastSentAt) > requestObject.repeatEvery - 1) {/* when is time to send repeatitive request*/
				outputArray.push(requestObject);
				requestObject.lastSentAt = $.fn.ajaxRequester.clock;
			}
		});
		return outputArray;
	}

	/* called every second. sends request if needed */
	function timeStep() {
		if (!$.fn.ajaxRequester.ajaxLock) {
			var data = getCombinedRequestObjects();
			if (data.length > 0) {
				sendRequest(data);
			}
		}
		$.fn.ajaxRequester.clock++;
	}

	$(document).ready(function () {
		setInterval(timeStep, 1000);
	});

	/* every request registered by register function (every request what needs repeatitive calls)*/
	$.fn.ajaxRequester.registeredRepeatedRequests = [];

	/* every single request registered. request is deleted right after calling its callback */
	$.fn.ajaxRequester.registeredSingleRequests = [];

	/* clock for requests, is incrased every second by 1 */
	$.fn.ajaxRequester.clock = 0;

	/* lock for prevent sending multiple request at the time */
	$.fn.ajaxRequester.ajaxLock = false;
	
	
})(jQuery);

var ajax = $('body').ajaxRequester();

/* AJAX links*/

var ajaxClick = function (e, $linkElement) {
	e.preventDefault();
	$.ajax({
		url: $linkElement.attr('href'),
		success: function (data) {
			renderFlashMessage();
			var $relatedElement = $('#' + $linkElement.data('related-element-id'));
			if ($relatedElement.length > 0) {
				$relatedElement.html(data);
				$relatedElement.parent().addClass('open');
				$relatedElement.addClass('open');
				ViewportManager.doLayout();
				$relatedElement.find('a.ajax').click(function (e) {
					ajaxClick(e, $(this));
				});
				$relatedElement.trigger('load');
			}
		},
		error: function (err) {
			console.log(err);
		}
	});
};

$(document).ready(function () {
	$('a.ajax').click(function (e) {
		ajaxClick(e, $(this));
	});
});