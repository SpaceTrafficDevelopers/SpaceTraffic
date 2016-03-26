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


function MinigameStarterDialog() {
    this.dialogElement;
    this.minigames;

    //this for private method
    var that = this;

    this.prepareDialog = function () {
        if (this.dialogElement.attr('canbedisplayed') == 'true') {
            if (this.dialogElement.is(':empty'))
                this.dialogElement.append(prepareDialogElement());

            prepareMinigameStarterDialog();
        }
    };

    this.open = function () {

        if (this.dialogElement.dialog('isOpen') === false && this.dialogElement.attr('canbedisplayed') == 'true')
            this.dialogElement.dialog('open');
    };

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

    function sendAjaxMessage(id, object, data, callbackFunction) {
        ajax.send({
            requestId: id,
            relatedObject: object,
            data: data,
            callback: callbackFunction
        });
    };

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

    function createGame(selectedGame) {
        var startGameCallback = function (gameId) {

            if (gameId !== -1) {
                var minigameDescriptor = getDescriptorById(selectedGame);

                if (minigameDescriptor !== null) {

                    if (minigameDescriptor.ExternalClient)
                        showInfoDialog(minigameDescriptor, gameId);
                    else
                        showWindow(minigameDescriptor, gameId);

                    return;
                }
            }

            alert("Hru se nepodařilo vytvořit.");
        };

        sendAjaxMessage('MinigameStarterCreateGame', 'MinigameStarter', { selectedGameId: selectedGame }, startGameCallback);
    };

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

    function showWindow(minigameDescriptor, gameId) {
        var myWin = window.open(minigameDescriptor.ClientURL + '?gameId=' + gameId, minigameDescriptor.Name, 'height=500,width=550,menubar=no,location=no,status=no,scrollbars=no,directories=no');

        //var myWin = window.open(window.location, minigameDescriptor.Name, 'height=500,width=550,menubar=no,location=no,status=no,scrollbars=no,directories=no');
        //'about:blank'
        //myWin.location = minigameDescriptor.ClientURL;
        //myWin.location //open(url, 'name', 'height=500,width=550,menubar=no,location=no,status=no,scrollbars=no,directories=no');

        //return myWin;
    };
};

