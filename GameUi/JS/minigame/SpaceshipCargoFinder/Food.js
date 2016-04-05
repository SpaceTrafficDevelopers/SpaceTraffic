function Food() {
    this.position = new Position(-1, -1);
    this.cargo;

    var that = this;

    this.setImage = function (cargoImg) {
        this.cargo = cargoImg;
    }

    this.generateFood = function (gameArea, snake) {
        do {
            this.position.x = generateCoordinate(gameArea.xCellCount - 1);
            this.position.y = generateCoordinate(gameArea.yCellCount - 1);
        } while (checkCollisionWithSnake(snake))
    }

    function generateCoordinate(maxCellIndex){
        return Math.round(maxCellIndex * Math.random());
    }

    function checkCollisionWithSnake(snake) {
        for (var i = 0; i < snake.body.length; i++) {
            if (that.position.equal(snake.body[i]))
                return true;
        }

        return false;
    }

    this.paint = function (context, cellSize) {
        context.drawImage(this.cargo, this.position.x * cellSize, this.position.y * cellSize, cellSize, cellSize);
    }
}