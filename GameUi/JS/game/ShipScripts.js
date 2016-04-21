//this file contains additional functionality for ship detail

$('#cargoBuyForm, #cargoSellForm, #repairingForm, #refuelingForm').on('successSend', function () {
	setTimeout(function () {
		$('.refreshButton').click();
	}, 800);
});
//send max amount button
$('.sendMax').click(function (e) {
	e.preventDefault();
	var inp = $(this).parent().find('.buyAmount, .sellAmount, .actionAmount');
	var send = $(this).parent().find('.sendBtn');
	inp.val(inp.attr('max'));
	send.click();
});
$('#cargoBuyForm form, #cargoSellForm form').submit(function () {
	setTimeout(function () {
		$('#mainPanel .refreshButton').click();
	}, 800);
	
});