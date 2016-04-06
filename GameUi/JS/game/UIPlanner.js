
var wormholes = [];
var toPlanet = null;
var toStarSystem = null;
var planPlace = $('#shipPlan');

var updateShipPlan = function () {
	var buff = '';
	if (typeof toPlanet !== 'undefined' && toPlanet != null) {
		buff = buff + '<span>Leť na:' + toPlanet.altName + '</span>';
	}
	if (wormholes.length > 0) {
		buff = buff + '<p>Přes: ';
		console.log(wormholes);
		buff = buff + wormholes.map(function (hole) { return hole.id; }).join(', ');
		buff = buff + '</p>';
	}
		planPlace.html(buff);
}

$('#planningUI .closebutton').click(function (e) {
	$('#planningUI').remove();
});
$('body').off('planetClick wormholeClick');
$('body').on('planetClick', function (e, originalEvent, planet) {
	var hasBase = (typeof (planet.hasBase) != "undefined" && planet.hasBase !== null && planet.hasBase);
	if (hasBase && planet.name != currentPlanetName) {
		toPlanet = planet;
		toStarSystem = $.cookie('currentStarSystem');
		updateShipPlan();
	}
});
$('body').on('wormholeClick', function (e, originalEvent, wormhole) {
	wormholes.push(wormhole);
	updateShipPlan();
});
$('#cancelPlan').click(function (e) {
	wormholes = [];
	toPlanet = null;
	toStarSystem = null;
	updateShipPlan();
});

$('#runPlan').click(function (e) {
	if (toPlanet != null) {
		ajax.send({
			requestId: 'PlanSingleFly',
			relatedObject: 'PlanSingleFly',
			data: {
				shipId: currShipId,
				fromStarSystem: currentShipStarSystem,
				fromPlanet: currentPlanetName,
				toStarSystem: toStarSystem,
				toPlanet: toPlanet.name,
				wormholes: wormholes.map(function (hole) { return hole.id; })
			},
			callback: function () {
				$('#planningUI .closebutton').click();
				$('#contextPanelContent .refreshButton').click();
			}
		});
	}	
});
