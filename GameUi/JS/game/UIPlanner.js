﻿
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
		buff = buff + wormholes.map(function (hole) { return "→" + hole.destination; }).join('');
		buff = buff + '</p>';
	}
		planPlace.html(buff);
}

/* switching to star system where the ship is */
StarSystemLoader.switchToStarSystem(currentShipStarSystem);
/* closing planning */
$('#planningUI .closebutton').click(function (e) {
	$('#planningUI').remove();
});
/* binding reactions on planets and wormholes clicking*/
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
/* cancel button*/
$('#cancelPlan').click(function (e) {
	wormholes = [];
	toPlanet = null;
	toStarSystem = null;
	updateShipPlan();
});

/* start button */
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
				wormholes: wormholes.map(function (hole) { return {index: hole.id, starsystem: hole.destination} })
			},
			callback: function () {
				$('#planningUI .closebutton').click();
				$('#contextPanelContent .refreshButton').click();
			}
		});
	}	
});