
//init of flash messages
;
$(document).ready(function () {
	$("#alert_placeholder").flashMessage({
		timeout: -1,//it never fades out automaticly
		fadeout: 300 //speed of removing even with clicking on cross
	});
});