//PlayerIsPlayingDialog object
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

//PlayerIsPlayingMinigame dialog class
function PlayerIsPlayingMinigameDialog() {
    //dialog element
    this.dialogElement;
    
    //this for private methods
    var that = this;

    //method for prepare dialog
    this.prepareDialog = function () {
        if (this.dialogElement.is(':empty'))
            this.dialogElement.append(prepareDialogElement());

        preparePlayerIsPlayingMinigameDialog();
    };

    //method for open dialog
    this.open = function () {
        if (this.dialogElement.dialog('isOpen') === false)
            this.dialogElement.dialog('open');
    };

    //method for close dialog on ending playing minigame
    this.closeOpenedDialog = function () {

        if (this.dialogElement.hasClass("ui-dialog-content") && this.dialogElement.dialog('isOpen') === true) {
            closeDialog(false);
        }
    };

    //method for close dialog
    //sendMessage - true for send ajax message to handler for forced finish minigame
    function closeDialog(sendMessage){
        that.dialogElement.empty();
        that.dialogElement.dialog('close');

        if(sendMessage)
            sendAjaxMessage('PlayerIsPlayingMinigameCloseDialog', 'PlayerIsPlayingMinigame', { close: true }, function () { });
    };

    //method for send ajax message
    function sendAjaxMessage(id, object, data, callbackFunction) {
        ajax.send({
            requestId: id,
            relatedObject: object,
            data: data,
            callback: callbackFunction
        });
    };

    //method for prepare jQuery dialog
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

    //method for prepare dialog element
    function prepareDialogElement() {
        var dialogElement = 'Během hraní miniher není možné ovládat hru. ';
        dialogElement += 'Dialog se automaticky zavře po ukončení aktuálně rozehrané minihry. ';
        dialogElement += 'Pokud chcete hru ovládat hned, stačí kiknout na tlačítko "Pokračovat do hry". '
        dialogElement += 'VAROVÁNÍ: V případě kliknutí na tlačítko "Pokračovat do hry" bude aktuálně rozehraná hra ukončena!'

        return dialogElement;
    };
};



