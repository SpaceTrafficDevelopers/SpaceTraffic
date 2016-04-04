function Game(canvas, gameId, gameName) {
    this.gameArea;
    this.snake;
    this.food;
    this.score;
    this.gameLoop;
    this.canvas = canvas;
    this.contex = canvas.getContext("2d");
    this.gameId = gameId;
    this.gameName = gameName;

    var interval = 70;
    var that = this;

    this.init = function (pieceSize, snakeLength) {
        this.gameArea = new GameArea(this.canvas.width, this.canvas.height, pieceSize);
        this.gameArea.init();

        this.snake = new Snake(snakeLength);
        this.food = new Food();

        this.score = 0;
        this.snake.createSnake();

        this.food.init();
        this.food.generateFood(this.gameArea, this.snake);

        keyBinding();

        this.gameArea.paint(that.contex);
        this.food.paint(that.contex, that.gameArea.cellSize);
        this.snake.paint(that.contex, that.gameArea.cellSize);
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
        that.gameLoop = setInterval(that.update, interval);

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
};