//start game dialog class
function StartGameDialog(dialogElement, startGameFce, gameId, gameName, startDescription) {
    //dialog element
    this.dialogElement = dialogElement;
    this.startGameFce = startGameFce;
    this.gameId = gameId;
    this.gameName = gameName;
    this.startDescription = startDescription;

    //this for private methods
    var that = this;

    //method for show dialog
    this.showDialog = function () {
        this.dialogElement.empty();
        this.dialogElement.append(this.startDescription);

        prepareDialog();
    };

    //method for prepare jQuery dialog
    function prepareDialog() {
        that.dialogElement.dialog({
            autoOpen: true,
            title: that.gameName,
            modal: true,
            closeOnEscape: false,
            buttons: {
                'Start': function () {
                    startGame();
                }
            }
        });
    };

    function startGame() {
        that.dialogElement.empty();
        that.dialogElement.dialog('close');

        sendAjaxMessage('StartSpaceshipCargoFinder', 'StartSpaceshipCargoFinder', { minigameId: that.gameId }, startCallback);
    }

    function startCallback(result) {
        if (result.State == 0) {
            alert(result.Message);
            window.close();
        }
        else
            that.startGameFce();
    }
};