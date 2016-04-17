//this file contains additional functionality for buy and sell forms

$('#cargoBuyForm, #cargoSellForm').on('successSend', function () {
	setTimeout(function () {
		$('.refreshButton').click();
	}, 800);
});
//send max amount button
$('.sendMax').click(function (e) {
	e.preventDefault();
	var inp = $(this).parent().find('.buyAmount, .sellAmount');
	var send = $(this).parent().find('.sendBtn');
	inp.val(inp.attr('max'));
	send.click();
});