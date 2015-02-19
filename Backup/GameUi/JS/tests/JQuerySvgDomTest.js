$(document).ready(function(){

	module('JQuery SVG DOM');
	
	test('outherWidthBugTest_IncludeMargin', function() {
		
		$('#qunit-fixture').html('<div id="outherWidthBugTest" style="width: 20px; margin: 10px; padding: 0px; position: relative;">&nbsp;</div>');
		var resultWidth = $('#outherWidthBugTest').outerWidth(true);
		var expected = 40;	
		strictEqual(resultWidth,expected, 'Expecting JQuery.outherWidth from created div element.');
	});
	
	test('outherWidthBugTest_ExcludeMargin', function() {
		
		$('#qunit-fixture').html('<div id="outherWidthBugTest" style="width: 20px; margin: 10px; padding: 0px; position: relative;">&nbsp;</div>');
		var resultWidth = $('#outherWidthBugTest').outerWidth(false);
		var expected = 20;	
		strictEqual(resultWidth,expected, 'Expecting JQuery.outherWidth from created div element.');
	});
	
	test('outherHeightBugTest_IncludeMargin', function() {
		
		$('#qunit-fixture').html('<div id="outherHeightBugTest" style="width: 20px; margin: 10px; padding: 0px; position: relative;">&nbsp;</div>');
		var resultWidth = $('#outherHeightBugTest').outerHeight(true);
		var expected = 40;	
		strictEqual(resultWidth,expected, 'Expecting JQuery.outerHeight from created div element.');
	});
	
	test('outherHeightBugTest_ExcludeMargin', function() {
		
		$('#qunit-fixture').html('<div id="outherHeightBugTest" style="width: 20px; margin: 10px; padding: 0px; position: relative;">&nbsp;</div>');
		var resultWidth = $('#outherHeightBugTest').outerHeight(true);
		var expected = 20;	
		strictEqual(resultWidth,expected, 'Expecting JQuery.outerHeight from created div element.');
	});
	
	
});