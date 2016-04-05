//Game dialog class (win dialog or loose dialog)
function GameDialog(dialogElement, message, winDialog, gameId, gameName) {
    //dialog element
    this.dialogElement = dialogElement;

    //message
    this.message = message;

    //indication if dialog is for win (true)
    this.winDialog = winDialog;

    //minigame id
    this.gameId = gameId;

    //game name
    this.gameName = gameName;

    //this for private methods
    var that = this;

    //method for prepare jQuery dialog
    this.showDialog = function () {
        this.dialogElement.empty();
        this.dialogElement.append(this.message);

        this.dialogElement.dialog({
            autoOpen: true,
            title: that.gameName,
            modal: true,
            closeOnEscape: false,
            buttons: {
                'Zavřít': function () {

                    //if dialog is winner dialog unbind end game request on close and send remove game request
                    if (winDialog == true) {
                        $(window).unbind('beforeunload');
                        sendAjaxMessage('PerformActionSpaceshipCargoFinderRemoveGame', 'PerformActionSpaceshipCargoFinder',
                            { minigameId: that.gameId, action: 'removeGame' }, function () { });
                    }

                    window.close();
                }
            }
        });
    };
};