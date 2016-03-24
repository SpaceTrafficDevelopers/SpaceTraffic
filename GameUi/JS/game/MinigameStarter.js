$(document).ready(function () {
    ajax.send({
        requestId: 'MinigameStarter',
        relatedObject: 'MinigameStarter',
        data: {},
        repeatEvery: 2,
        callback: function (minigames) {

            if (minigames) {
                var dialogElement = $('#dialog');

                if (dialogElement.is(':empty'))
                    dialogElement.append(prepareDialogElement(minigames));

                prepareMinigameStarterDialog(dialogElement, minigames);

                if (dialogElement.dialog('isOpen') === false)
                    dialogElement.dialog('open');
            }
        }
    });
});

function prepareMinigameStarterDialog(dialogElement, minigames) {
    $(dialogElement).dialog({
        autoOpen: false,
        title: 'Minihry',
        modal: true,
        closeOnEscape: false, //not work in any browsers
        buttons: {
            'Ok': function () {
                var selectedGame = 0;

                if (Array.isArray(minigames))
                    selectedGame = $('select[name="minigames"]').val();
                else
                    selectedGame = minigames.MinigameId;

                closeMinigameStarterDialog(this);

                createGame(minigames, selectedGame);

            },
            'Storno': function () {
                closeDialog(this);
            }
        }
    });
}

function prepareMinigameStarterDialogElement(minigames) {
    var dialogElement = '';

    if (!Array.isArray(minigames)) {
        dialogElement += 'Chcete si zahrat minihru ' + minigames.Name + '?';
    }
    else{
        dialogElement += 'Kterou minihru si chcete zahrát?';
        dialogElement += '<select class="dropdown-select dropdown-dark" name="minigames">';
        
        for (var i = 0; i < minigames.length; i++) {
            dialogElement += '<option value="' + minigames[i].MinigameId +'">' + minigames[i].Name + '</option>'    
        }

        dialogElement += '</select>';
    }

    return dialogElement;
}

function closeMinigameStarterDialog(dialog) {
    $(dialog).dialog('close');
    $(dialog).empty();

    //send close dialog message
    sendAjaxMessage('MinigameStarterCloseDialog', 'MinigameStarter', { close: true }, null);
}

function sendAjaxMessage(id, object, data, callbackFunction) {
    ajax.send({
        requestId: id,
        relatedObject: object,
        data: data,
        callback: callbackFunction
    });
}

function getGameId(selectedGame) {
    var minigameId = 1;
    
    var callbackFunction = function(minigame){
        alert(minigame);
        minigameId = minigame;
    }

    /*ajax.send({
        requestId: 'MinigameStarterCreateGame',
        relatedObject: 'MinigameStarter',
        data: { selectedGameId: 1 },
        callback: function (gameId) {

            alert(gameId);
        }
    });*/

    sendAjaxMessage('MinigameStarterCreateGame', 'MinigameStarter', { selectedGameId: selectedGame }, callbackFunction);

    return minigameId;
}

function getDescriptorById(selectedId, minigames) {
    if (Array.isArray(minigames)) {
        for (var i = 0; i < minigames.length; i++) {
            if (minigames[i].MinigameId == selectedId)
                return minigames[i]
        }

        return null;
    }
    else
        return minigames;
}

function createGame(minigames, selectedGame) {
    var gameId = getGameId(selectedGame);

    if (gameId !== -1) {
        var minigameDescriptor = getDescriptorById(selectedGame, minigames);

        if (minigameDescriptor !== null) {

            if (minigameDescriptor.ExternalClient)
                showInfoDialog(minigameDescriptor, gameId);
            else
                showWindow(minigameDescriptor);

            return;
        }
    } 

    alert("Hru se nepodařilo vytvořit.");
}

function showInfoDialog(minigameDescriptor, gameId) {
    var info = '<div>Tuto hru si můžete zahrát pouze v externím klientovi. ID vaší hry je : ' + gameId;
    info += ' Klienta stáhnete <a target="_blank" href="' + minigameDescriptor.ClientURL + '">zde</a>';
    info += '</div>';

    $(info).dialog({
        title: 'Externí minihra',
        modal: true,
        closeOnEscape: false, //not work in any browsers
        buttons: {
            'Ok': function () {
                $(this).dialog('close');
            }
        }
    });
}

function showWindow(minigameDescriptor) {

    var myWin = window.open(minigameDescriptor.ClientURL, minigameDescriptor.Name, 'height=500,width=550,menubar=no,location=no,status=no,scrollbars=no,directories=no');
        //myWin.location //open(url, 'name', 'height=500,width=550,menubar=no,location=no,status=no,scrollbars=no,directories=no');

    return myWin;
}