/* There is all scripts that define behavior of the menus on game page */


/* opens and closes the main menu */
; $(function () {
	$(document).ready(function () {
		var $menuButton = $('#menuButton');
		var $menuPanel = $('#menuPanel');
		var $toTrigger = $('#content');
		$menuButton.toggle(function () {
			$menuPanel.css('display', 'block');
			$menuButton.addClass('active');
			$.cookie('isMenuOpen', 1, { expires: 2 });
			$toTrigger.trigger('changed');
		}, function () {
			$menuPanel.css('display', 'none');
			$menuButton.removeClass('active');
			$.cookie('isMenuOpen', 0, { expires: 2 });
			$toTrigger.trigger('changed');
		});
		if ($.cookie('isMenuOpen') == 1) {
			$menuButton.trigger('click');
		}
	
	});

});