//Minigame starter dialog object
var minigameStarterDialog = new MinigameStarterDialog();

$(document).ready(function () {
    ajax.send({
        requestId: 'MinigameStarter',
        relatedObject: 'MinigameStarter',
        data: {},
        repeatEvery: 2,
        callback: function (minigames) {

            if (minigames) {
                minigameStarterDialog.dialogElement = $('#minigameStarterDialog');
                minigameStarterDialog.minigames = minigames;
                minigameStarterDialog.prepareDialog();
                minigameStarterDialog.open();
            }
        }
    });
});

//Minigame starter dialog class
function MinigameStarterDialog() {
    //dialog element
    this.dialogElement;
    //minigames (minigame descriptor or list of minigame descriptors)
    this.minigames;

    //this for private method
    var that = this;

    //method for prepare dialog
    this.prepareDialog = function () {
        if (this.dialogElement.attr('canbedisplayed') == 'true') {
            if (this.dialogElement.is(':empty'))
                this.dialogElement.append(prepareDialogElement());

            prepareMinigameStarterDialog();
        }
    };

    //method for open dialog when it is not opened
    this.open = function () {

        if (this.dialogElement.dialog('isOpen') === false && this.dialogElement.attr('canbedisplayed') == 'true')
            this.dialogElement.dialog('open');
    };

    //method for prepare jQuery dialog
    function prepareMinigameStarterDialog() {
        that.dialogElement.dialog({
            autoOpen: false,
            title: 'Minihry',
            modal: true,
            closeOnEscape: false,
            buttons: {
                'Ok': function () {
                    var selectedGame = 0;

                    if (Array.isArray(that.minigames))
                        selectedGame = $('select[name="minigames"]').val();
                    else
                        selectedGame = that.minigames.MinigameId;

                    closeDialog();
                    createGame(selectedGame);

                },
                'Storno': function () {
                    closeDialog();
                }
            }
        });
    }

    //method for prepare dialog element
    function prepareDialogElement() {
        var dialogElement = '';

        if (!Array.isArray(that.minigames)) {
            dialogElement += 'Chcete si zahrat minihru ' + that.minigames.Name + '?';
        }
        else {
            dialogElement += 'Kterou minihru si chcete zahrát?';
            dialogElement += '<select class="dropdown-select dropdown-dark" name="minigames">';

            for (var i = 0; i < that.minigames.length; i++) {
                dialogElement += '<option value="' + that.minigames[i].MinigameId + '">' + that.minigames[i].Name + '</option>'
            }

            dialogElement += '</select>';
        }

        return dialogElement;
    };

    //method for close dialog
    function closeDialog() {
        var closeCallback = function () {

            that.dialogElement.attr('canbedisplayed', 'true');

            /*if(that.dialogElement.dialog('isOpen') === true)
                that.dialogElement.dialog('close');

            if (!that.dialogElement.is(':empty'))
                that.dialogElement.empty();*/
        };

        that.dialogElement.empty();
        that.dialogElement.attr('canbedisplayed', 'false');
        that.dialogElement.dialog('close');

        sendAjaxMessage('MinigameStarterCloseDialog', 'MinigameStarter', { close: true }, closeCallback);
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

    ///method for get minigame descriptor by selected id
    function getDescriptorById(selectedId) {
        if (Array.isArray(that.minigames)) {
            for (var i = 0; i < that.minigames.length; i++) {
                if (that.minigames[i].MinigameId == selectedId)
                    return that.minigames[i]
            }
            return null;
        }
        else
            return that.minigames;
    };

    //method for create game and open game window or show external minigame dialog
    //window opening is here because browser evaluate new window as user action and  don't block it,
    //in callback method the window is blocked
    function createGame(selectedGame) {
        var win;
        var minigameDescriptor = getDescriptorById(selectedGame);

        if(!minigameDescriptor.ExternalClient)
            win = window.open('', '', 'height=500,width=550,menubar=no,location=no,status=no,scrollbars=no,directories=no');

        var startGameCallback = function (gameId) { 
            if (gameId !== -1) {
                if (minigameDescriptor !== null) {

                    if (minigameDescriptor.ExternalClient)
                        showInfoDialog(minigameDescriptor, gameId);
                    else
                        showWindow(win, minigameDescriptor, gameId);

                    return;
                }
            }

            if(win)
                win.close();

            alert("Hru se nepodařilo vytvořit.");
        };

        sendAjaxMessage('MinigameStarterCreateGame', 'MinigameStarter', { selectedGameId: selectedGame }, startGameCallback);
    };

    //method for show external minigame info dialog
    function showInfoDialog(minigameDescriptor, gameId) {
        var info = '<div>Tuto hru si můžete zahrát pouze v externím klientovi. ID vaší hry je : ' + gameId;
        info += ' Klienta stáhnete <a target="_blank" href="' + minigameDescriptor.ClientURL + '">zde</a>';
        info += '</div>';

        $(info).dialog({
            title: 'Externí minihra',
            modal: true,
            closeOnEscape: false,
            buttons: {
                'Ok': function () {
                    $(this).dialog('close');
                }
            }
        });
    };

    //method for redirect minigame window
    function showWindow(win, minigameDescriptor, gameId) {
        win.location.href = '/Minigame' + minigameDescriptor.ClientURL + '?gameId=' + gameId;
    };
};

