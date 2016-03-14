$(document).ready(function () {
    ajax.send({
        requestId: 'MinigameStarter',
        relatedObject: 'MinigameStarter',
        data: {},
        repeatEvery: 2,
        callback: function (minigameId) {

            if (minigameId != 0) {
               // alert(minigameId[0] + " " + minigameId[1] + " " + minigameId[2]);

                var $dialog = $('<div><select name="pokus"><option value="item1">item1</option></select></div>').dialog({
                    title: '',
                    modal: true,   //dims screen to bring dialog to the front
                    closeOnEscape: false,
                    buttons: {
                        'Ok': function () {
                            alert($('select[name="pokus"]').val());
                            $(this).dialog('close');
                            var myWin = window.open("about:blank", 'name', 'height=500,width=550,menubar=no,location=no,status=no,scrollbars=no,directories=no');
                            showWindow(myWin, "http://localhost:2457/Game");
                            
                        },
                        'Storno': function () {
                            $(this).dialog('close');
                        }
                    }
                });

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

function prepareDialogElement(minigameNames) {
    var dialogElement = '<div>'

    if (!Array.isArray(minigameNames)) {
        dialogElement += 'Chcete si zahrat minihru ' + minigameName + '?';
    }
    else{
        dialogElement += 'Kterou minihru si chcete zahrát?';
        dialogElement += '<select name="minigames">';
        
        for (var i = 0; i < minigameNames.lenght; i++) {
            dialogElement += '<option value="' + minigameNames[i] +'">' + minigameNames[i] + '</option>'    
        }

        dialogElement += '</select>';
    }

    dialogElement += '</div>';

    return dialogElement;
}