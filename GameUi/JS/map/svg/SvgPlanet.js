/**
 * @author dyrczyk
 */

var SvgPlanet = new Class({
    Extends: SvgOrbitingBody,
    initialize: function (id, planet) {
        this.parent('planet' + id, planet);
        this.cssClassPrefix = "planet";
    },
    getName: function () {
        return (this.body.altName ? this.body.altName : this.body.name);
    },

    onclickHandler: function () {

        var buffer = "";
        var planetName = this.getName();
        var planetAltName = this.body.name;
        var base;

        $.cookie("currentPlanet", planetAltName, { path: '/' });

        // get me the base
        // getJSON is asynchronous!
        $.getJSON('http://localhost:2457/' + 'StarSystem/GetBase/' + SvgStarSystemMap.currentStarSystem.name + ";" + planetAltName, function (base) {

            // vybírám cestu
            if (document.getElementById("selectedPath")) {

                // mùžu jen na planetu s base
                if (base == null) {
                    alert(planetName + " has no dock!");
                } else {

                    // nemá smysl letìt zpìt na startovní base
                    if (planetAltName == $.cookie("selectShipPath").split(';')[2]) {
                        alert("This planet is same like starting planet " + planetAltName);
                    } else {

                        buffer += SvgStarSystemMap.currentStarSystem.name + " - " + planetName + "<br />";
                        $("#selectedPath").append(buffer);

                        $.cookie("selectShipPath", $.cookie("selectShipPath") + "p;" + SvgStarSystemMap.currentStarSystem.name + ";" + planetAltName + ";", { path: '/' });

                        buffer = null;
                    }
                }

                // chci info o planete
            } else {
                // get player id
                var cookieName = "loggedPlayerId=";
                var loggedPlayerId = -1;
                if (document.cookie.length > 0) { // if there are any cookies
                    var offset = document.cookie.indexOf(cookieName);

                    offset += cookieName.length;
                    var end = document.cookie.indexOf(";", offset);
                    if (end == -1) document.cookie.length + 1;
                    var idFromCookie = document.cookie.substring(offset, end);
                    if (idFromCookie != null && idFromCookie != "") loggedPlayerId = idFromCookie;
                }

                buffer += "<span id='starSystem'>" + SvgStarSystemMap.currentStarSystem.name + "</span>" +
                          "<h2 id='planetName'>" + planetName + "</h2>" +
                          "<div id='Base'>" + addBase(SvgStarSystemMap, planetName, planetAltName, base, loggedPlayerId) + "</div>";

                // vypise az dostane succes odpoved od serveru
                $("#contextPanel").html(buffer);
                buffer = null;
            }

        });
    }

});