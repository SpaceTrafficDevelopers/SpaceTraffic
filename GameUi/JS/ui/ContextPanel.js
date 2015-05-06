/**
* Functions for updating ContextPanel.
* @author dyrczyk
*/

//cookies: currentStarSystem, currentPlanet, selectShipPath
// $.cookie("currentStarSystem");
// $.cookie("currentStarSystem", starSystem.name, { path: '/' });

// TODO: popsat funkce, jejich atributy, datove typy
// info v contextPanelu vypisovat podle toho jestli je nastavene jmeno planety v cookie

/*
* Returns base information
* String SvgStarSystemMap, String planetName, String planetAltName, IList<Entities.Base> bases
*/
function addBase(SvgStarSystemMap, planetName, planetAltName, base, loggedPlayerId) {
    var buffer = "";
    if (base != null) {
        buffer += '<div id="ships">' +
                      addShips(planetName, planetAltName, base, loggedPlayerId)
                      + "</div>" +
                      "<div id='traders'>" +
                      //addTraders(SvgStarSystemMap, planetAltName, base) +
                      "</div>" +
                      "<div id='lands'>" +
                      //addLands(SvgStarSystemMap, planetName, planetAltName, base, loggedPlayerId)
                      +"</div>" +
                      "<div id='buildings'>" +
                      //addBuildings(SvgStarSystemMap, planetName, planetAltName, base, base.Planet, loggedPlayerId)
                      + "</div>" +
                      "<div id='deposit'>" +
                      //addDeposit()
                      + "</div>";
        } else {
            buffer = "<h4>There isn't any base</h4>";
        }
    return buffer;
}

/*
* Returns list of ships with cargos 
* String planetName, String planetAltName, Entities.Base base, int logget player id
*/
function addShips(planetName, planetAltName, base, loggedPlayerId) {
    var buffer = '<h3>Ships</h3>';
    $.each(base.SpaceShips, function (index, ship) {
        //pouze zadokovane lode
        if (!ship.IsFlying) {
            if (ship.PlayerId == loggedPlayerId) {
                buffer += "<div>" +
                        '<a class="tiny button" href="" onclick="shipDetail(' + "'" + ship.SpaceShipId + "','" + planetName + "','" + planetAltName + "'" + '); return false;">' + ship.SpaceShipName + '</a>' +
                      "</div>";
                if (ship.SpaceShipsCargos != null) {
                    buffer += '<div id="shipsCargos">' + addShipCargos(ship) + "</div>";
                }
            }
        }
    });
    buffer += "<div><a class='alert tiny button' href='" + $("#appRoot").attr("href") + "Game/Ships#Buy_new_ship' onclick=addInfoToCookies()>Buy a new ship</a></div>";
    return buffer;
}

/* 
* Shows ship detail information 
* int shipId, String dockedPlanetName, String dockedPlanetAltName
*/
function shipDetail(shipId, dockedPlanetName, dockedPlanetAltName) {
    $.getJSON($("#appRoot").attr("href") + 'Player/GetPlayer/', function (player) {
        var buffer = "";
        $.each(player.SpaceShips, function (index, ship) {
            if (shipId == ship.SpaceShipId) {
                buffer += "<h3>" + ship.SpaceShipName + "</h3>";
                if (ship.SpaceShipsCargos != null) {
                    buffer += '<div id="shipsCargos">' + addShipCargos(ship) + "</div>";
                }
                buffer += '<a class="tiny button" href="" onclick="shipSelectPath(' + "'" + ship.SpaceShipId + "','" + dockedPlanetName + "','" + dockedPlanetAltName + "'" + '); return false;">selectPath</a>';
            }
        });
        $("#contextPanel").html(buffer);
        buffer = null;
    });
}

/* 
* Returns ship cargos 
* Entities.SpaceShip ship
*/
function addShipCargos(ship) {
    var buffer = "";
    $.each(ship.SpaceShipsCargos, function (index, cargos) {
        if (cargos.Cargo.Type == 'voda')
            buffer += "<div class='cargo tiny button'></div>";
    });
    return buffer;
}

/* 
* Shows ship select path 
* int shipId, String dockedPlanetName, String dockedPlanetAltName
*/
function shipSelectPath(shipId, dockedPlanetName, dockedPlanetAltName) {

    // select first point to ship path
    $.cookie("selectShipPath", "p;" + SvgStarSystemMap.currentStarSystem.name + ";" + dockedPlanetAltName + ";", { path: '/' });

    var buffer = "<h3>" + "Select path for testing ship" + "</h3>" +
                 "<div id=selectedPath>" + SvgStarSystemMap.currentStarSystem.name + " - " + dockedPlanetName + "<br /></div>" +
                 "<div id=shipFlyToButton></div>";
    $("#contextPanel").html(buffer);
    addShipFlyToButton(shipId);
}

/* 
* Shows ship flyTo button 
* int shipId
*/
function addShipFlyToButton(shipId) {
    $("#shipFlyToButton").html(
        '<a class="tiny button" href="" onclick="shipFlyTo(' + "'" + shipId + "'" + '); return false;">done</a>'
    );
}

/* 
* Calls server to perform shipFlyTo action
* int shipId
*/
function shipFlyTo(shipId) {
    var field = $.cookie("selectShipPath").split(';');
    if (field[field.length - 4] == 'w') {
        alert("Ship can NOT docking at wormhole! \nPick some planet with base.");
    } else {

        $.post($("#appRoot").attr("href") + 'Ship/FlyTo/', { id: shipId + ";" + $.cookie("selectShipPath") },
            function (data) {
                // TODO: rozhodnuot jestli se podarilo resolvnout path a vypsat výsledek
                var buffer = "";

                if (data == 0) {
                    data = "Ship took off."
                }
                // vypise az dostane succes odpoved od serveru
                $("#contextPanel").html(data);
                buffer = null;
                $.cookie("selectShipPath", "", { path: '/' });
            }, "json");
    }
}

/* 
* Returns traders and their offers
* String SvgStarSystemMap, String planetAltName, Entities.Base base
*/
function addTraders(SvgStarSystemMap, planetAltName, base) {
    
    // title 
    var buffer = '<br><h3>Traders</h3>';
    // Trade "management" button
    buffer += "<div><a class='alert tiny button' href='" + $("#appRoot").attr("href") + "Game/Trade#BuySell' >Buy, sell and manage cargo</a></div>";
    // return buffer content
    return buffer;
}

/* 
* Returns deposit at this planet
*/
function addDeposit() {

    // title 
    var buffer = '<br><h3>Deposit</h3>';
    // Trade "management" button
    buffer += "<div><a class='alert tiny button' href='" + $("#appRoot").attr("href") + "Game/Auction#Deposit' >Move cargo from deposit</a></div>";
    // return buffer content
    return buffer;
}

// addLands function
// create Land List
// and Land ManageButton
function addLands(SvgStarSystemMap, planetName, planetAltName, base, loggedPlayerId) {
    // init buffer
    var buffer = '<h3>Lands</h3>';
    // if Land count is 0
    if (base.Lands.length == 0)
        buffer += "<div>Your List of Lands is empty !</div>";
    else {
        var isOwnedByPlayer = false;
        var infoToBuffer = "";
        // Land table/list iteration
        for (var i = 0; i < base.Lands.length; i++) {
            if (base.Lands[i].PlayerId == loggedPlayerId) {
                isOwnedByPlayer = true;
                infoToBuffer = "<tr><td>" + (base.Lands[i].LandModel).charAt(0) + "x"
                    + (base.Lands[i].LandModel).charAt(1) + "</td><td></td><td>" + base.Lands[i].Price + "</tr>";
            }
        }
        if (isOwnedByPlayer) {
            // LandList
            buffer += "<table>";
            // table header
            buffer += "<tr><td>Size</td><td width='8px'></td><td>Price</td></tr>";
            buffer += infoToBuffer;
            buffer += "</table>";
        }
        else {
            buffer += "<div>Your List of Lands is empty !</div>";
        }
    }
    // Land "management" button
    buffer += "<div><a class='alert tiny button' href='" + $("#appRoot").attr("href") + "Game/Lands#Edit' >Manage your land</a></div>";
    // return buffer content
    return buffer;
}

// addBuilding function
// create Manage Button
function addBuildings(SvgStarSystemMap, planetName, planetAltName, base, planet, loggedPlayerId) { 
    // title 
    var buffer = '<br><h3>Buildings</h3>';
    // Building "management" button
    buffer += "<div><a class='alert tiny button' href='" + $("#appRoot").attr("href") + "Game/Buildings#New_Building' >Manage your buildings</a></div>";
    // return buffer content
    return buffer;
}

function addInfoToCookies() {
    $.cookie("selectedFromPlanet", "selectedFromPlanet", { path: '/' });
}