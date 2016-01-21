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
		return '(w)&#x2192;'+this.body.destination;
	},
	
    buildObject : function(t) {
		this.setT(t);
		var point = this.body.trajectory.calculatePosition(this.getT());
		
		var transform = SvgOrbitFactory.getTransformationForPointOnOrbit(point, this.body.trajectory)
		
        return '<g id='+this.getId(ID_OBJECT_SUFFIX)+' class="'+this.cssClassPrefix+'" transform="'+transform+'">\
        	<g class="outer-rings"> 								\
        	<circle cx="0" cy="0" r="26"/>							\
        	<circle cx="0" cy="0" r="20"/>							\
        	</g>													\
        	<circle class="center" cx="0" cy="0" r="11" />	\
        	<g class="curves" >										\
        		<path d="M -20 -28 q 20 30 0 56" />					\
        		<path d="M 20 -28 q -20 30 0 56" />					\
        	</g>													\
        </g>';
		//TODO: Parametrize
		//return '<circle id="'+this.getId('Obj')+'" cx="0" cy="0" r="' + this.iconSize + '" class="planet" transform="' + SvgOrbitFactory.getTransformationForPointOnOrbit(point, this.body.trajectory) + '"/>';
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