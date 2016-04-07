//Game class
function Game(canvas, gameId, gameName, pauseDialog) {
    //game area
    this.gameArea;

    //snake
    this.snake;

    //food
    this.food;

    //score
    this.score;
    
    //game loop - interval handler
    this.gameLoop;

    //canvas
    this.canvas = canvas;

    //context
    this.contex = canvas.getContext("2d");

    //minigame id
    this.gameId = gameId;

    //game name
    this.gameName = gameName;

    //pause dialog (element)
    this.pauseDialog = pauseDialog;

    //game status
    this.status;

    //game interval (speed regulation)
    var interval = 70;

    //this for private method
    var that = this;

    //initialization method
    this.init = function (pieceSize, snakeLength) {
        this.gameArea = new GameArea(this.canvas.width, this.canvas.height, pieceSize);
        this.snake = new Snake(snakeLength);
        this.food = new Food();

        this.score = 0;
        this.snake.createSnake();
        this.food.generateFood(this.gameArea, this.snake);

        var imgLoader = new ImageLoader();
        imgLoader.createImages(drawElements);

        this.gameArea.setImage(imgLoader.background);
        this.food.setImage(imgLoader.cargo);
        this.snake.setImages(imgLoader.snakeUp, imgLoader.snakeRight, imgLoader.snakeDown, imgLoader.snakeLeft, imgLoader.cargo);

        imgLoader.loadImages();
        keyBinding();

        this.status = State.PREPARED;
    }

    //method for draw game elements
    function drawElements() {
        that.gameArea.paint(that.contex);
        that.food.paint(that.contex, that.gameArea.cellSize);
        that.snake.paint(that.contex, that.gameArea.cellSize);
    }

    //update method for game loop. it generates new snake position, check collision, move snake and paint elements
    this.update = function () {

        var nextPosition = that.snake.getNextPosition();

        if (that.gameArea.checkCollisionWithSnake(nextPosition) || that.snake.checkCollisoinWithSelf(nextPosition)) {
            stopGame();

            var winDialog = new GameDialog($('#gameDialog'), "Prohrál jsi! Zkus to někdy příště.", false, that.gameId, that.gameName);
            winDialog.showDialog();

            return;
        }

        that.move(nextPosition);
        drawElements();

        that.snake.updated = true;
        paintScore();
    }

    //method for stopping game
    function stopGame() {
        clearInterval(that.gameLoop);
        that.status = State.STOP;
    }

    //mathod for "drawing"´score
    function paintScore() {
        var scoreElement = $("#score");
        scoreElement.text(that.score);
    }

    //method for bind key event
    function keyBinding() {

        $(document).keydown(function (e) {
            that.snake.changeDirection(e.which);
        });
    }

    //method for move snake
    this.move = function (nextPostion) {
        //if next postion is food add score and generate new food
        if (this.food.position.equal(nextPostion)) {
            addScoreMessage();

            this.score++;
            this.snake.move(true, nextPostion);
            this.food.generateFood(this.gameArea, this.snake);
        }
        else
            this.snake.move(false, nextPostion);
    }

    //method for starting game
    this.start = function () {
        preparePauseDialog();

        that.gameLoop = setInterval(that.update, interval);
        that.status = State.RUN;

        initPause()

        //start sending update request and check collision request
        updateRequestMessage();
        checkCollisionMessage();
    }

    //update request callback method
    function updateRequestCallback(result) {
        //if minigame does not exist alert player and close window
        if (result.ReturnValue == null || result.ReturnValue == false) {
            alertAndClose(result.Message);
        }
    }

    //check collision callback method
    function checkCollisionCallback(result) {
        //if result is failure or collision was detected, alert player and close window
        if (result.State == 0) {
            alertAndClose(result.Message);
        }
        else if (result.ReturnValue == true) {
            alertAndClose('Nepodváděj! Hra bude ukončena.')
        }
    }

    //method for alert player and close window.
    function alertAndClose(message) {
        //unbinding is here because server end and remove game and it is not necessary sending end request
        $(window).unbind('onbeforeunload');
        alert(message);
        window.close();
    }

    //add score callback method
    function addScoreCallback(result) {
        //if minigame does not exist alert player and close window
        if (result.State == 0) {
            alertAndClose(result.Message);
        }
        //if player wins, stop game and show win dialog
        else if (result.ReturnValue == true) {
            stopGame();
            var winDialog = new GameDialog($('#gameDialog'), "Vyhrál jsi! Tvá odměna byla připsána.", true, that.gameId, that.gameName);
            winDialog.showDialog();
        }
    }

    //method for start sending update request each 15 seconds
    function updateRequestMessage() {
        sendAjaxMessageRepeatedly('PerformActionSpaceshipCargoFinderUpdate', 'PerformActionSpaceshipCargoFinder',
            { minigameId: that.gameId, action: 'updateRequest' }, 15, updateRequestCallback)
    }

    //method for start sending check collision request each 10 seconds
    function checkCollisionMessage() {
        sendAjaxMessageRepeatedly('PerformActionSpaceshipCargoFinderCheckCollision', 'PerformActionSpaceshipCargoFinder',
            { minigameId: that.gameId, action: 'checkCollision', body: that.snake.body }, 10, checkCollisionCallback)
    }

    //method for send add score request
    function addScoreMessage() {
        sendAjaxMessage('PerformActionSpaceshipCargoFinderAddScore', 'PerformActionSpaceshipCargoFinder',
            { minigameId: that.gameId, action: 'addScore' }, addScoreCallback)
    }

    //method for initializing pause on blur event
    function initPause() {
        window.onblur = function () {
            //if game is in run status and gameloop was initialized pause game and open pause dialog
            if (that.status == State.RUN && typeof (that.gameLoop) != 'undefined') {
                clearInterval(that.gameLoop);
                that.status = State.PAUSE;

                if (that.pauseDialog.dialog('isOpen') == false)
                    that.pauseDialog.dialog('open');
            }
        }
    }

    //method for preparing pause dialog
    function preparePauseDialog() {
        that.pauseDialog.dialog({
            autoOpen: false,
            title: that.gameName,
            modal: true,
            closeOnEscape: false,
            buttons: {
                'Pokračovat': function () {
                    //on click, close dialog and after 500ms resume game again
                    $(this).dialog('close');
                    setTimeout(function () {

                        //resume game if game is in pause state
                        if (that.status == State.PAUSE && typeof (that.gameLoop) != 'undefined') {
                            that.gameLoop = setInterval(that.update, interval);
                            that.status = State.RUN;
                        }

                    }, 500)
                }
            }
        });
    }

    //Game state enum
    var State = {
        PREPARED : 0,
        RUN: 1,
        PAUSE: 2,
        STOP: 3,
    }

};