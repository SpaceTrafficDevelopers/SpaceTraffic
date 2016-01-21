/* that thing which is rolling text messages down there
	http://www.givainc.com/labs/marquee_jquery_plugin.cfm
*/

; $(function () {
	$(document).ready(function () {
		console.log($("#marquee"));
		$("#infoStream").marquee({
			yScroll: "bottom",
			showSpeed: 850,
			scrollSpeed: 12,
			pauseSpeed: 5000
		});
	});
});