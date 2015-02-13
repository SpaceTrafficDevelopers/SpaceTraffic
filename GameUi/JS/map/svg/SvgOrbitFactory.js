/// Provides build methods for orbit trajectory visualization.
var SvgOrbitFactory = {
	buildCurcularOrbit : function(circularOrbit, id) {
		//TODO: test circle trajectory.
		return '<circle id="' + id + '" cx="' + circularOrbit.cx + '" cy="' + circularOrbit.cy + '" r="' + circularOrbit.radius + '" class="orbit"/>';
	},
	buildEllipticOrbit : function(ellipticOrbit, id) {
		return '<ellipse id="'+id+'" cx="' + ellipticOrbit.cx + '" cy="' + ellipticOrbit.cy + '" rx="' + ellipticOrbit.a + '" ry="' + ellipticOrbit.b + '" class="orbit" transform="rotate(' + ellipticOrbit.rotationAngleInDeg + ')"/>';
	},
	
	getTransformationForPointOnOrbit: function(point, orbit){
		if(orbit instanceof CircularOrbit) {
			return 'translate(' + (orbit.cx + point.x) + ',' + (orbit.cy + point.y) + ')';
		}
		else
		{
			return 'rotate(' + orbit.rotationAngleInDeg + ') '+
			   'translate(' + (orbit.cx + point.x) + ',' + (orbit.cy + point.y)  + ') '+
			   'rotate(' +  (-orbit.rotationAngleInDeg) + ')';
		}
	}
};