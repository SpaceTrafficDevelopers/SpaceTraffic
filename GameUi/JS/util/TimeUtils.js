/**
 * @author Azaroth
 */

// Used for calculations with time in consistent manner.
// https://developer.mozilla.org/en/JavaScript/Reference/Global_Objects/Date/now
var TimeUtils = {
	
	// Gets current time in seconds.
	getCurrentTimeInS: function(){
		return Date.now() / 1000;
	},
	
	// Gets difference of given time (in seconds) and current time.
	getDiffTimeToCurrentInS: function(pastTime){
		return (Date.now() / 1000)-pastTime;
	},
	
	// Gets difference in given two times.
	getTimeDiff: function(laterTime, formerTime){
		return laterTime-formerTime;
	}
}