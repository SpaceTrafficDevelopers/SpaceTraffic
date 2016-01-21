// Prevents errors in MSIE and Firefox and console logging.

// Nastavení debug režimu. True zapnuto, false vypnuto.
SPACE_TRAFFIC_DEBUG = true;

// Nastavení mapy

// Maximum fps pro překreslování. 0 pro vypnutí automatického překreslování mapy
SPACE_TRAFFIC_MAX_FPS = 40;

// Minimální interval mezi snímky.

// Vytvoření konzole, pokud nenní nadefinována. Pokud není DEBUG, přepíše se prázdnou funkcí.
if ((typeof (console) === 'undefined') || !(SPACE_TRAFFIC_DEBUG)) {
	var console = {};
	console.log = console.error = console.info = console.debug = 
		console.warn = console.trace = console.dir = console.dirxml = 
		console.group = console.groupEnd = console.time = console.timeEnd = 
		console.assert = console.profile = function () { };
}

// Určení prohlížeče.
var isIE  = (navigator.appVersion.indexOf("MSIE") != -1) ? true : false;
var isWin = (navigator.appVersion.toLowerCase().indexOf("win") != -1) ? true : false;
var isOpera = (navigator.userAgent.indexOf("Opera") != -1) ? true : false;

SpaceTraffic = {};

// Kompatibilita s requestAnimFrame funkcí.
// http://paulirish.com/2011/requestanimationframe-for-smart-animating/

window.requestAnimFrame = (function(){
  return  window.requestAnimationFrame       || 
          window.webkitRequestAnimationFrame || 
          window.mozRequestAnimationFrame    || 
          window.oRequestAnimationFrame      || 
          window.msRequestAnimationFrame     || 
          function(/* function */ callback, /* DOMElement */ element){
            window.setTimeout(callback, 1000 / 40);
          };
})();


$(document).ready(function () {
		
	ViewportManager.init();
	
    //console.debug("Loading star system");
    var storedStarSystem = $.cookie("currentStarSystem");
    
    //console.debug("storedStarSystem:", storedStarSystem);
    if(storedStarSystem == null)
    {
    	storedStarSystem = 'Solar System'
    }
    StarSystemLoader.loadStarSystem(storedStarSystem,function(starSystem){
        SvgStarSystemMap.currentStarSystem = starSystem;
        SvgStarSystemMap.init($("#viewport"));
        
        //console.debug("currentStarSystem: ", SvgStarSystemMap.currentStarSystem);
        SvgStarSystemMap.draw();
        SvgStarSystemMap.startUpdateTimer();
    });
});