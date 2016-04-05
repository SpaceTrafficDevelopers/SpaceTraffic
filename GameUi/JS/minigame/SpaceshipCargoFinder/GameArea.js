function GameArea(width, height, cellSize) {
    this.width = width;
    this.height = height;
    this.cellSize = cellSize;
    this.xCellCount = width / cellSize; //maximalni index
    this.yCellCount = height / cellSize;
    this.background;

    this.setImage = function (backgroundImg) {
        this.background = backgroundImg;
    }

    this.paint = function (context) {
        context.drawImage(this.background, 0, 0, this.width, this.height);
        context.strokeStyle = "white";
        context.strokeRect(0, 0, this.width, this.height);
    }

    this.checkCollisionWithSnake = function (nextPosition) {
        if (nextPosition.x < 0 || nextPosition.y < 0
            || nextPosition.x >= this.xCellCount || nextPosition.y >= this.yCellCount) {

            return true;
        }

        return false;
    }
};