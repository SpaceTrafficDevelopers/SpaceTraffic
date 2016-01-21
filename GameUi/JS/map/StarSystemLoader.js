/// <reference path="../../Scripts/jquery-1.6.4-vsdoc.js" />
/// <reference path="../../Scripts/mootools-core-1.4.1.js" />
/// <reference path="../../Scripts/jquery-ui-1.8.16.js" />
/// <reference path="StarSystemObjects.js" />


/// Object for loading star systems into an object.
/// Use loadStarSystem async function.
var StarSystemLoader = {

	/// Loads star system of given name from xml file on server.
	/// Loaded StarSystem object instance will be passed as parameter to the callback function.
	loadStarSystem: function (starSystemName, callback) {
		//console.group("StarSystemLoader.loadStarSystem");
		//console.log("Loading StarSystem: "+starSystemName);
		$.ajax({
			type: "GET",
			url: '/StarSystem/Get/' + starSystemName,
			dataType: "xml",
			success: function (xml) {
				var $starSystem = $(xml).find('starsystem');
				var starSystem = StarSystemLoader.parseStarSystem($starSystem);
				//console.log("StarSystem loaded: ", starSystem);
				//console.log("Loading connections of "+starSystemName);
				$.getJSON('/StarSystem/GetConnections/'+ starSystemName, function (connections){
					for (var i = 0; i < connections.length; i++) {
						starSystem.wormholeEndpoints[connections[i].EndpointId].destination = connections[i].DestinationStarSystemName
					}
					callback(starSystem);
				});
			}
		}); 
		//console.groupEnd();
	},

	parseStarSystem: function ($starSystem) {
		//console.debug("parseStarSystem()", $starSystem);
		var name = $starSystem.attr('name');
		//console.log('StarSystem:' + name);
		var star = StarSystemLoader.parseStar($starSystem.find('star'));
		var planets = new Array();
		$starSystem.find('planets').find('planet').each(function () {
			planets.push(StarSystemLoader.parsePlanet($(this)));
		});
		var wormholeEndpoints = new Array();
		$starSystem.find('wormholeEndpoints').find('wormholeEndpoint').each(function () {
			var wormholeEndpoint = StarSystemLoader.parseWormholeEndpoint($(this));
			wormholeEndpoints[wormholeEndpoint.id] = wormholeEndpoint;
		});
		return new StarSystem(name, star, planets, wormholeEndpoints);
	},

	parseStar: function ($star) {
		//console.debug("ENTRY parseStar()", $star);
		var name = $star.attr('name');
		var trajectory = StarSystemLoader.parseTrajectory($star.find('trajectory').first());
		return new Star(name, trajectory, null);
	},

	parsePlanet: function ($planet) {
		//console.debug("ENTRY parsePlanet()", $planet);
		var planetName = $planet.attr('name');
		var planetAltName = $planet.attr('altName');
		var trajectory = StarSystemLoader.parseTrajectory($planet.find('trajectory').first());
		var description = $planet.find('details').find('description').text();
		return new Planet(planetName, planetAltName, trajectory, description, null);
	},

	parseWormholeEndpoint: function ($wormholeEndpoint) {
		//console.debug("ENTRY parseWormholeEndpoint()", $wormholeEndpoint);
		var id = parseInt($wormholeEndpoint.attr('id'));
		var trajectory = StarSystemLoader.parseTrajectory($wormholeEndpoint.find('trajectory').first());
		return new WormholeEndpoint(id, trajectory, null);
	},

	parseTrajectory: function ($trajectory) {
		//console.group("StarSystemLoader.parseTrajectory()");
		//var velocity = parseFloat($trajectory.attr('velocity'));
		//var direction = $trajectory.attr('direction');
		$shape = $trajectory.children();
		//console.debug($shape);
		var shape;
		switch ($shape[0].tagName) {
			case 'circularOrbit':
				//console.debug('circularOrbit');
				shape = new CircularOrbit(
					parseFloat($shape.attr('radius')),
					parseInt($shape.attr('period')),
					$shape.attr('direction'),
					parseFloat($shape.attr('initialAngle'))
				 );
				break;
			case 'stacionary':
				//console.debug('stacionary');
				var p = new Point(
					parseInt($shape.attr('x')),
					parseInt($shape.attr('y'))
				);
				shape = p;
				break;
			case 'ellipticOrbit':
				//console.debug('ellipticOrbit');
				shape = new EllipticOrbit(
					parseFloat($shape.attr('a')),
					parseFloat($shape.attr('b')),
					parseInt($shape.attr('period')),
					$shape.attr('direction'),
					parseFloat($shape.attr('angle')),
					parseFloat($shape.attr('initialAngle'))
				);
				break;
			default:
				throw "Unknown element: " + $shape.tagName;
				break;
		}
		//console.groupEnd();
		return shape;
	}
}
//#endregion