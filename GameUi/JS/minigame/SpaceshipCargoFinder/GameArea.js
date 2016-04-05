//Game area class
function GameArea(width, height, cellSize) {
    //Game area width
    this.width = width;

    //Game area height
    this.height = height;

    //Game area cell size
    this.cellSize = cellSize;

    //Maximum x coordinate of cell in width.
    this.xCellCount = width / cellSize;

    //Maximum y coordinate of cell in height.
    this.yCellCount = height / cellSize;

    //background image
    this.backgroundImage;

    //method for setting background image
    this.setImage = function (backgroundImg) {
        this.backgroundImage = backgroundImg;
    }

    //method for drawing game area
    this.paint = function (context) {
        context.drawImage(this.backgroundImage, 0, 0, this.width, this.height);
        context.strokeStyle = "white";
        context.strokeRect(0, 0, this.width, this.height);
    }

    //method for check collision between game area and snakes
    //if collision is detected, it returns true
    this.checkCollisionWithSnake = function (nextPosition) {
        if (nextPosition.x < 0 || nextPosition.y < 0
            || nextPosition.x >= this.xCellCount || nextPosition.y >= this.yCellCount) {

            return true;
        }

        return false;
    }
};