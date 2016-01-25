/// <reference path="../../Scripts/jquery-1.6.4-vsdoc.js" />
/// <reference path="../../Scripts/mootools-core-1.4.1.js" />
/// <reference path="../../Scripts/jquery-ui-1.8.16.js" />
/// <reference path="StarSystemObjects.js" />


/// Object for loading galaxy map into an object.
/// Use galaxy map async function.
var GalaxyMapLoader = {

    /// Loads galaxy of given namefrom xml file on server.
    /// Loaded StarSystem object instance will be passed as parameter to the callback function.
    loadGalaxyMap: function (galaxyMapName, callback) {
        //console.group("GalaxyMapLoader.loadGalaxyMap");
        //console.log("Loading GalaxyMap: " + galaxyMapName);
        $.ajax({
            type: "GET",
            url: '/GalaxyMap/Get/' + galaxyMapName,
            dataType: "xml",
            success: function (xml) {
                var $galaxyMap = $(xml).find('galaxymap');
                var galaxyMap = GalaxyMapLoader.parseGalaxyMap($galaxyMap);
                //console.log("GalaxyMap loaded: ", galaxyMap);
                callback(galaxyMap);
            }
        });
        //console.groupEnd();
    },

    parseGalaxyMap: function ($galaxyMap) {
        //console.debug("parseGalaxyMap()", $galaxyMap);
        var name = $galaxyMap.attr('name');
        //console.log('GalaxyMap:' + name);
        var starSystems = new Array();
        $galaxyMap.find('starsystems').find('starsystem').each(function () {
            starSystems.push(GalaxyMapLoader.parseStarSystem($(this)));
        });
        var wormholeEndpoints = new Array();
        $galaxyMap.find('wormholes').find('wormhole').each(function () {
            var wormhole = GalaxyMapLoader.parseWormhole($(this));
            wormholeEndpoints.push(wormhole);
            wormholeEndpoints[wormhole.id] = wormhole;
        });
       
        return new GalaxyMap(name, starSystems, wormholeEndpoints);
    },

    parseStarSystem: function ($starsystem) {
        //console.debug("ENTRY parseStarSystem()", $starsystem);
        var starsystemName = $starsystem.attr('name');
        return new StarSystem(starsystemName, null, null, null);
    },

    parseWormhole: function ($wormhole) {
        //console.debug("ENTRY parseWormholes()", $wormholeEndpoint);
        var id = parseInt($wormhole.attr('id'));
        var endpoints = new Array();
        $galaxyMap.find('wormholes').find('wormhole').find('endpoint').each(function () {
            var endpoint = GalaxyMapLoader.parseEndpoint($(this));
            endpoints.push(endpoint);
            endpoints[endpoint.id] = endpoint;
        });
        return new GalaxyWormholeEndpoint(id, null, endpoint);
    },

    parseEndpoint: function ($endpoint) {
        //console.debug("ENTRY parseEndpoint()", $planet);
        var endpointSystem = $endpoint.attr('system');
        var id = $endpoint.attr('id');
        return new GalaxyEndpoint(starsystemName, null, id);
    },
}
//#endregion