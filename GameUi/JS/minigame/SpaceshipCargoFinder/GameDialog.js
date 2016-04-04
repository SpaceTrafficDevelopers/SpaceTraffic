function GameDialog(dialogElement, message, winDialog, gameId, gameName) {
    //dialog element
    this.dialogElement = dialogElement;
    this.message = message;
    this.winDialog = winDialog;
    this.gameId = gameId;
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