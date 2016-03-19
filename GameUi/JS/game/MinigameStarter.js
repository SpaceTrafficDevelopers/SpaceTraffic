$(document).ready(function () {
    ajax.send({
        requestId: 'MinigameStarter',
        relatedObject: 'MinigameStarter',
        data: {},
        repeatEvery: 2,
        callback: function (minigames) {

            if (minigames) {
                var dial = $('#dialog');
                //var dialogElement = prepareDialogElement(minigames);
                if (dial.is(':empty'))
                    dial.append(prepareDialogElement(minigames));

                //var $dialog = $(dialogElement).dialog({
                var $dialog = $(dial).dialog({
                    autoOpen: false,
                    title: 'Minihry',
                    modal: true,
                    closeOnEscape: false, //not work in any browsers
                    buttons: {
                        'Ok': function () {
                            if (Array.isArray(minigames))
                                alert($('select[name="minigames"]').val());

                            $(this).dialog('close');

                            var myWin = window.open("about:blank", 'name', 'height=500,width=550,menubar=no,location=no,status=no,scrollbars=no,directories=no');
                            alert(window.location.host + minigames.ClientURL);
                            showWindow(myWin, window.location.host + minigames.ClientURL);
                            $(this).empty();
                        },
                        'Storno': function () {
                            $(this).dialog('close');
                            $(this).empty();
                        }
                    }

                });

                if (dial.dialog('isOpen') === false)
                    dial.dialog('open');

                //var result = confirm("Blabolik?");
                //var myWin = window.open("about:blank", 'name', 'height=500,width=550');
                //parameters toolbar=no,directories=no,status=no,linemenubar=no,scrollbars=no,resizable=no ,modal=yes
                //showWindow(myWin, "http://google.com");
            }
        }
    });
});

function showWindow(win, url) {
    win.open(url, 'name', 'height=500,width=550,menubar=no,location=no,status=no,scrollbars=no,directories=no');
}

function prepareDialogElement(minigames) {
    //var dialogElement = '<div>'
    var dialogElement = ''

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