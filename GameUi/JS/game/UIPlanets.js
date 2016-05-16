/* scripts for planets interfaces */

/* on this variable is SvgPlanet.js asking, when is clicked on the planet */
var openedPlanetContext = '';

$('#contextPanel').on('click', '.contextBtn', function () {
	$('#contextPanel .contextBtn').removeClass('active');
	$(this).addClass('active');
	openedPlanetContext = $(this).attr('id');
});