var playerIsPlayingDialog = new PlayerIsPlayingMinigameDialog();

$(document).ready(function () {
    ajax.send({
        requestId: 'PlayerIsPlayingMinigame',
        relatedObject: 'PlayerIsPlayingMinigame',
        data: {},
        repeatEvery: 2,
        callback: function (isPlaying) {
            var dialogEle = $('#isPlayingDialog');

            if(isPlaying === true){
                playerIsPlayingDialog.dialogElement = dialogEle;
                playerIsPlayingDialog.prepareDialog();
                playerIsPlayingDialog.open();
            }
            else if(isPlaying === false){
                playerIsPlayingDialog.dialogElement = dialogEle;
                playerIsPlayingDialog.closeOpenedDialog();
            }
            
        }
    });
});

function PlayerIsPlayingMinigameDialog() {
    this.dialogElement;
    
    var that = this;

    this.prepareDialog = function () {
        if (this.dialogElement.is(':empty'))
            this.dialogElement.append(prepareDialogElement());

        preparePlayerIsPlayingMinigameDialog();
    };

    this.open = function () {
        if (this.dialogElement.dialog('isOpen') === false)
            this.dialogElement.dialog('open');
    };

    this.closeOpenedDialog = function () {

        if(this.dialogElement.dialog('isOpen') === true){
            closeDialog(false);
        }
    };

    function closeDialog(sendMessage){
        that.dialogElement.empty();
        that.dialogElement.dialog('close');

        if(sendMessage)
            sendAjaxMessage('PlayerIsPlayingMinigameCloseDialog', 'PlayerIsPlayingMinigame', { close: true }, function () { });
    };

    function sendAjaxMessage(id, object, data, callbackFunction) {
        ajax.send({
            requestId: id,
            relatedObject: object,
            data: data,
            callback: callbackFunction
        });
    };

    function preparePlayerIsPlayingMinigameDialog() {
        that.dialogElement.dialog({
            autoOpen: false,
            title: 'Máte rozehranou minihru',
            modal: true,
            closeOnEscape: false, 
            buttons: {
                'Pokračovat do hry': function () {
                    closeDialog(true);
                }
            }
        });
    };

    function prepareDialogElement() {
        var dialogElement = 'Během hraní miniher není možné ovládat hru. ';
        dialogElement += 'Dialog se automaticky zavře po ukončení aktuálně rozehrané minihry. ';
        dialogElement += 'Pokud chcete hru ovládat hned, stačí kiknout na tlačítko "Pokračovat do hry". '
        dialogElement += 'VAROVÁNÍ: V případě kliknutí na tlačítko "Pokračovat do hry" bude aktuálně rozehraná hra ukončena!'

        return dialogElement;
    };
};



