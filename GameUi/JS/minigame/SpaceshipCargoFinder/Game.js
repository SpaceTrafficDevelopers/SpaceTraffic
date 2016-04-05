function Game(canvas, gameId, gameName, pauseDialog) {
    this.gameArea;
    this.snake;
    this.food;
    this.score;
    this.gameLoop;
    this.canvas = canvas;
    this.contex = canvas.getContext("2d");
    this.gameId = gameId;
    this.gameName = gameName;
    this.pauseDialog = pauseDialog;
    this.status;

    var interval = 70;
    var that = this;

    this.init = function (pieceSize, snakeLength) {
        this.gameArea = new GameArea(this.canvas.width, this.canvas.height, pieceSize);
        this.snake = new Snake(snakeLength);
        this.food = new Food();

        this.score = 0;
        this.snake.createSnake();
        this.food.generateFood(this.gameArea, this.snake);

        var imgLoader = new ImageLoader();
        imgLoader.createImages(initialPaint);

        this.gameArea.setImage(imgLoader.background);
        this.food.setImage(imgLoader.cargo);
        this.snake.setImages(imgLoader.snakeUp, imgLoader.snakeRight, imgLoader.snakeDown, imgLoader.snakeLeft, imgLoader.cargo);

        imgLoader.loadImages();
        keyBinding();

        this.status = State.PREPARED;
    }

    function initialPaint() {
        that.gameArea.paint(that.contex);
        that.food.paint(that.contex, that.gameArea.cellSize);
        that.snake.paint(that.contex, that.gameArea.cellSize);
    }

    this.update = function () {

        var nextPosition = that.snake.getNextPosition();

        if (that.gameArea.checkCollisionWithSnake(nextPosition) || that.snake.checkCollisoinWithSelf(nextPosition)) {
            stopGame();

            var winDialog = new GameDialog($('#gameDialog'), "Prohrál jsi! Zkus to někdy příště.", false, that.gameId, that.gameName);
            winDialog.showDialog();

            return;
        }

        that.move(nextPosition);

        that.gameArea.paint(that.contex);
        that.snake.paint(that.contex, that.gameArea.cellSize);
        that.food.paint(that.contex, that.gameArea.cellSize);

        that.snake.updated = true;
        paintScore();
    }

    function stopGame() {
        clearInterval(that.gameLoop);
        that.status = State.STOP;
    }

    function paintScore() {
        var scoreElement = $("#score");
        scoreElement.text(that.score);
    }

    function keyBinding() {

        $(document).keydown(function (e) {
            that.snake.changeDirection(e.which);
        });
    }

    this.move = function (nextPostion) {
        if (this.food.position.equal(nextPostion)) {
            addScoreMessage();

            this.score++;
            this.snake.move(true, nextPostion);
            this.food.generateFood(this.gameArea, this.snake);
        }
        else
            this.snake.move(false, nextPostion);
    }

    this.start = function () {
        preparePauseDialog();

        that.gameLoop = setInterval(that.update, interval);
        that.status = State.RUN;

        initPause()

        updateRequestMessage();
        checkCollisionMessage();
    }

    function updateRequestCallback(result) {
        if (result.ReturnValue == null || result.ReturnValue == false) {
            alertAndClose(result.Message);
        }
    }

    function checkCollisionCallback(result) {
        if (result.State == 0) {
            alertAndClose(result.Message);
        }
        else if (result.ReturnValue == true) {
            alertAndClose('Nepodváděj! Hra bude ukončena.')
        }
    }

    function alertAndClose(message) {
        $(window).unbind('onbeforeunload');
        alert(message);
        window.close();
    }

    function addScoreCallback(result) {
        if (result.State == 0) {
            alertAndClose(result.Message);
        }
        else if (result.ReturnValue == true) {
            stopGame();
            var winDialog = new GameDialog($('#gameDialog'), "Vyhrál jsi! Tvá odměna byla připsána.", true, that.gameId, that.gameName);
            winDialog.showDialog();
        }
    }

    function updateRequestMessage() {
        sendAjaxMessageRepeatedly('PerformActionSpaceshipCargoFinderUpdate', 'PerformActionSpaceshipCargoFinder',
            { minigameId: that.gameId, action: 'updateRequest' }, 15, updateRequestCallback)
    }

    function checkCollisionMessage() {
        sendAjaxMessageRepeatedly('PerformActionSpaceshipCargoFinderCheckCollision', 'PerformActionSpaceshipCargoFinder',
            { minigameId: that.gameId, action: 'checkCollision', body: that.snake.body }, 10, checkCollisionCallback)
    }

    function addScoreMessage() {
        sendAjaxMessage('PerformActionSpaceshipCargoFinderAddScore', 'PerformActionSpaceshipCargoFinder',
            { minigameId: that.gameId, action: 'addScore' }, addScoreCallback)
    }

    function initPause() {
        window.onblur = function () {
            if (that.status == State.RUN && typeof (that.gameLoop) != 'undefined') {
                clearInterval(that.gameLoop);
                that.status = State.PAUSE;

                if (that.pauseDialog.dialog('isOpen') == false)
                    that.pauseDialog.dialog('open');
            }
        }
    }

    function preparePauseDialog() {
        that.pauseDialog.dialog({
            autoOpen: false,
            title: that.gameName,
            modal: true,
            closeOnEscape: false,
            buttons: {
                'Pokračovat': function () {

                    $(this).dialog('close');
                    setTimeout(function () {

                        if (that.status == State.PAUSE && typeof (that.gameLoop) != 'undefined') {
                            that.gameLoop = setInterval(that.update, interval);
                            that.status = State.RUN;
                        }

                    }, 500)
                }
            }
        });
    }

    var State = {
        PREPARED : 0,
        RUN: 1,
        PAUSE: 2,
        STOP: 3,
    }

};