
//init of flash messages
;

var renderFlashMessage = function () {
	$("#alert_placeholder").flashMessage({
		timeout: 8000,//it never fades out automaticly
		fadeout: 300 //speed of removing even with clicking on cross
	});
};
$(document).ready(function () {
	renderFlashMessage();
});