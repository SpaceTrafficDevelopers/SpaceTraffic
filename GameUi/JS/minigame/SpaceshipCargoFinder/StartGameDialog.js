//start game dialog class
function StartGameDialog(dialogElement, startGameFce, gameId, gameName, startDescription, controls) {
    //dialog element
    this.dialogElement = dialogElement;

    //start function
    this.startGameFce = startGameFce;

    //minigame id
    this.gameId = gameId;

    //game name
    this.gameName = gameName;

    //start description
    this.startDescription = startDescription;

    //controls description
    this.controls = controls;

    //this for private methods
    var that = this;

    //method for show dialog
    this.showDialog = function () {
        this.dialogElement.empty();
        this.dialogElement.append(this.startDescription);
        this.dialogElement.append('<br />Ovládání: ' + this.controls);

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

    //method for start game. it close dialog and send start game request
    function startGame() {
        that.dialogElement.empty();
        that.dialogElement.dialog('close');

        sendAjaxMessage('StartSpaceshipCargoFinder', 'StartSpaceshipCargoFinder', { minigameId: that.gameId }, startCallback);
    }

    //start callback method
    function startCallback(result) {
        //if result is failure alert player and close window, otherwise start game
        if (result.State == 0) {
            alert(result.Message);
            window.close();
        }
        else
            that.startGameFce();  
    }
};