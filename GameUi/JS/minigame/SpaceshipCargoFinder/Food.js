//Snake food class - representing cargo
function Food() {
    //food position
    this.position = new Position(-1, -1);

    //food image
    this.foodImage;

    //this for private method
    var that = this;

    //method for setting image
    this.setImage = function (foodImg) {
        this.foodImage = foodImg;
    }

    //method for generating food position
    this.generateFood = function (gameArea, snake) {
        do {
            this.position.x = generateCoordinate(gameArea.xCellCount - 1);
            this.position.y = generateCoordinate(gameArea.yCellCount - 1);
        } while (checkCollisionWithSnake(snake))
    }

    //method for generating coordinates
    //return new random coordinate
    function generateCoordinate(maxCellIndex){
        return Math.round(maxCellIndex * Math.random());
    }

    //method for check collision between food and snake
    //returns true if food collidate with snake
    function checkCollisionWithSnake(snake) {
        for (var i = 0; i < snake.body.length; i++) {
            if (that.position.equal(snake.body[i]))
                return true;
        }

        return false;
    }

    //method for drawing food
    this.paint = function (context, cellSize) {
        context.drawImage(this.foodImage, this.position.x * cellSize, this.position.y * cellSize, cellSize, cellSize);
    }
}