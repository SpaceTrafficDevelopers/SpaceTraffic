/**
 * @author Azaroth
 */

var SvgWormholeEndpoint = new Class({
	Extends: SvgOrbitingBody,
	initialize: function(wormholeEndpoint){
		this.parent('wormholeEndpoint'+wormholeEndpoint.id, wormholeEndpoint);
		this.iconSize = 30;
		
		this.cssClassPrefix = 'wormholeEndpoint';
	},
	getName: function(){
		return '&#x2192;'+this.body.destination;
	},
	buildDefs: function (t) {/* definition of pattern used for animation */
		this.setT(t);
		return '<filter height="1.0253561" y="-0.012678025" width="1.0227816" x="-0.011390815" id="filter' + this.getId(ID_OBJECT_SUFFIX) + '" style="color-interpolation-filters:sRGB"><feGaussianBlur id="feGaussianBlur4604" stdDeviation="1"/></filter>';
	},
	
	buildObject : function(t) {
		this.setT(t);
		var point = this.body.trajectory.calculatePosition(this.getT());
		
		var transform = SvgOrbitFactory.getTransformationForPointOnOrbit(point, this.body.trajectory)
		
		return '<g id=' + this.getId(ID_OBJECT_SUFFIX) + ' class="' + this.cssClassPrefix + '" transform="' + transform + '">'
			+ '<g transform="scale(-0.4,0.4)" >'
			+ '<circle cx="0" cy="0" r="62" style="fill:#12a8c2;fill-opacity:0.3;filter:url(#filter' + this.getId(ID_OBJECT_SUFFIX) + ')"/>'
			+ '<g id="whgroup' + this.getId(ID_OBJECT_SUFFIX) + '" class="wormholeGroup" style="filter:url(#filter' + this.getId(ID_OBJECT_SUFFIX) + ')">'
				+ '<path  class="hovering" style="animation: counterspin 20s linear infinite; fill:#15c6e5;fill-opacity:0.3;" d="m -25,25 c -11.27065,-13.93988 -12.9137,-32.86596 2.32892,-43.94787 11.9193,-7.47345 29.24045,-9.37629 36.01619,1.70693 1.98994,3.3996 2.76128,7.93923 1.93724,11.99052 -0.82403,4.05131 -3.31349,7.64425 -7.79791,8.88937 -1.82201,0.50589 -4.53226,-0.0553 -6.4738,-1.2897 -0.97078,-0.61716 -1.76284,-1.42833 -2.03652,-2.45337 -0.27368,-1.02505 0.0436,-2.19762 1.05943,-3.32583 l 0.74302,0.66655 c -0.86554,0.96125 -1.01644,1.72456 -0.83572,2.40139 4.86528,3.68204 10.81686,0.59863 11.64888,-2.97524 3.27018,-17.93453 -15.3457,-21.49515 -29.40218,-12.72104 -14.05647,8.77412 -12.45166,28.07211 -0.15749,39.25629 11.92236,10.84594 33.3612,15.3001 51.43511,1.05408 21.42858,-18.45959 21.23262,-49.64344 4.09128,-68.68848 -17.14134,-19.04504 -78.95731,-16.34175 -96.940356,20.79504 -3.75674,10.08148 -1.293672,14.06334 -3.75674,10.08148 -2.462047,-6.23901 -1.204176,-32.59651 4.572151,-37.07402 71.7370474,-55.60684 158.74932,18.89378 97.558675,81.04992 -15.99042,14.24476 -41.25507,14.61824 -63.99018,-5.41602 z" />'
				+ '<path  class="hovering" style="animation: counterspin 20s linear infinite; fill:#15c6e5;fill-opacity:0.3;" d="m 25,-25 c 13.17207,12.15913 17.5409,30.64722 4.06542,43.82136 -10.71024,9.1221 -27.57271,13.51535 -35.88329,3.53123 -2.46166,-3.07528 -3.88282,-7.45518 -3.65467,-11.58313 0.22814,-4.12795 2.17056,-8.04378 6.42715,-9.92572 1.72946,-0.76462 4.49242,-0.60218 6.59238,0.33778 1.04998,0.46994 1.95125,1.15774 2.37061,2.13229 0.41936,0.97456 0.27538,2.18073 -0.56621,3.44428 l -0.83178,-0.55182 c 0.71708,-1.07656 0.75575,-1.85368 0.47883,-2.49717 -5.34756,-2.938 -10.7894,0.97547 -11.09464,4.6322 -0.63625,18.21912 18.29913,19.04398 30.93548,8.3252 12.63633,-10.7188 8.25145,-29.58042 -5.53394,-38.8646 -13.36845,-9.00339 -35.22649,-10.30322 -51.04475,6.41198 -18.526796,21.3705 -13.813166,52.19667 5.90753,68.55616 19.7207,16.3595 84.05045,7.06261 96.46107,-32.28849 5.336916,-27.53573 -4.022712,37.02714 -25.003855,46.75113 -60.214499,26.77202 -127.927696,-27.96065 -83.724805,-88.31686 13.75695,-16.41199 38.7007,-20.44334 64.09947,-3.91582 z" />'
			+ '</g>'

			+ '<path transform="rotate(-10)" style="animation: counterspin 16s -10s linear infinite; fill:#18c9e8;fill-opacity:0.2;filter:url(#filter' + this.getId(ID_OBJECT_SUFFIX) + ');" d="m -25,25 c -11.27065,-13.9399 -12.9137,-32.86595 2.32892,-43.94786 11.9193,-7.47345 29.24045,-9.37629 36.01619,1.70693 1.98994,3.3996 2.76128,7.93923 1.93724,11.99052 -0.82403,4.05131 -3.31349,7.64425 -7.79791,8.88937 -1.82201,0.50589 -4.53226,-0.0553 -6.4738,-1.2897 -0.97078,-0.61716 -1.76284,-1.42833 -2.03652,-2.45337 -0.27368,-1.02505 0.0436,-2.19762 1.05943,-3.32583 l 0.74302,0.66655 c -0.86554,0.96125 -1.01644,1.72456 -0.83572,2.40139 4.86528,3.68204 10.81686,0.59863 11.64888,-2.97524 3.27018,-17.93453 -15.3457,-21.49515 -29.40218,-12.72104 -14.05647,8.77412 -12.45166,28.07208 -0.15749,39.25628 11.92236,10.846 33.3612,15.3001 51.435111,1.0541 21.42858,-18.4596 21.23262,-49.64345 4.09128,-68.68849 -17.141341,-19.04504 -79.134087,-18.10952 -97.117133,19.02727 l -4.964996,23.64027 c -6.770714,-25.99579 -8.119621,-43.84283 3.408826,-52.37005 72.009905,-53.26342 160.906343,22.7963 100.107033,84.5549 -15.990421,14.2448 -41.255071,14.6183 -63.990181,-5.416 z"/>'
			+ '<path transform="rotate(270)" style="animation: counterspin 16s -10s linear infinite; fill:#18c9e8;fill-opacity:0.2;filter:url(#filter' + this.getId(ID_OBJECT_SUFFIX) + ');" d="m 25,-25 c 13.172071,12.15913 17.540901,30.64722 4.06542,43.82136 -10.71024,9.12209 -27.57271,13.51539 -35.88329,3.53119 -2.46166,-3.07524 -3.88282,-7.45514 -3.65467,-11.58309 0.22814,-4.12795 2.17056,-8.04378 6.42715,-9.92572 1.72946,-0.76462 4.49242,-0.60218 6.59238,0.33778 1.04998,0.46994 1.95125,1.15774 2.37061,2.13229 0.41936,0.97456 0.27538,2.18073 -0.56621,3.44428 l -0.83178,-0.55182 c 0.71708,-1.07656 0.75575,-1.85368 0.47883,-2.49717 -5.34756,-2.938 -10.7894,0.97547 -11.09464,4.6322 -0.63625,18.21915 18.29913,19.04395 30.93548,8.3252 12.636331,-10.7188 8.25145,-29.58042 -5.53394,-38.8646 -13.36845,-9.00339 -35.22649,-10.30322 -51.04475,6.41198 -18.526796,21.3705 -13.813166,52.19667 5.90753,68.55617 19.7207,16.3595 84.050451,7.0626 96.461071,-32.2885 l 1.85124,-21.12201 c 6.4076,6.9353 1.06115,16.32714 -2.29629,33.40581 -1.0957,5.5736 -4.61762,14.4037 -13.97737,21.8229 -61.75483,48.9519 -142.991962,-9.1952 -94.306241,-75.67243 13.75695,-16.41199 38.7007,-20.44334 64.09947,-3.91582 z" />'
			+ '<circle style="fill:#000000;" cx="0" cy="0" r="4.28125" />'
			+ '</g>'
		+ '</g>';
	},

	onclickHandler: function(){
		//console.debug("this",this);
		
		StarSystemLoader.loadStarSystem(this.body.destination, function(starSystem){
			SvgStarSystemMap.currentStarSystem = starSystem;
			// Sets currentStarSystem cookie
			$.cookie("currentStarSystem", starSystem.name, { path: '/' });
			//console.debug("currentStarSystem: ", SvgStarSystemMap.currentStarSystem);
			SvgStarSystemMap.draw();
			SvgStarSystemMap.startUpdateTimer();
		});
	}
});