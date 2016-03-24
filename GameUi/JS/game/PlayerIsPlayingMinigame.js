$(document).ready(function () {
    ajax.send({
        requestId: 'PlayerIsPlayingMinigame',
        relatedObject: 'PlayerIsPlayingMinigame',
        data: {},
        repeatEvery: 2,
        callback: function (isPlaying) {

            if (isPlaying && isPlaying === true) {
                var dialogElement = $('#dialog');

                if (dialogElement.is(':empty'))
                    dialogElement.append(preparePlayerIsPlayingMinigameDialogElement(minigames));

                prepareMinigameStartDialog(dialogElement, minigames);

                if (dialogElement.dialog('isOpen') === false)
                    dialogElement.dialog('open');
            }
        }
    });
});

function preparePlayerIsPlayingMinigameDialog(dialogElement, minigames) {
    $(dialogElement).dialog({
        autoOpen: false,
        title: 'Minihry',
        modal: true,
        closeOnEscape: false, //not work in any browsers
        buttons: {
            'Pokračovat do hry': function () {
                
                //send message to end minigame
                closeDialog(this);

            }
        }
    });
}

function preparePlayerIsPlayingMinigameDialogElement() {
    var dialogElement = 'Během hraní miniher není možné ovládat ovládat hru. ';
    dialogElement += 'Dialog se automaticky zavře po ukončení aktuálně rozehrané minihry. ';
    dialogElement += 'Pokud chcete hru ovládat hned, stačí kiknout na tlačítko "Pokračovat do hry". '
    dialogElement += 'VAROVÁNÍ: V případě kliknutí na tlačítko "Pokračovat do hry" bude aktuálně rozehraná hra ukončena!'

    return dialogElement;
}