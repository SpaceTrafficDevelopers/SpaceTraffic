//Snake class -- representing ship
function Snake(length) {
    //snake head
    this.head;

    //snake body (with head) - array
    this.body;

    //snake length
    this.length = length;

    //snake directions
    this.direction;

    //indication if direction was updated or not (this is very fast key pressing)
    this.updated = true;

    //ship up image
    this.shipUpImage;

    //ship right image
    this.shipRightImage;

    //ship left image
    this.shipLeftImage;

    //ship down image
    this.shipDownImage;

    //cargo image
    this.cargoImage;

    //this for private
    var that = this;

    //method for crating snake (starting position is top left corner with right direction)
    this.createSnake = function () {
        this.body = [];
        for (var i = this.length - 1; i >= 0; i--)
            this.body.push(new Position(i, 0));

        this.head = this.body[0];
        this.direction = Direction.RIGHT;
    }

    //method for setting images
    this.setImages = function (snakeUpImg, snakeRightImg, snakeDownImg, snakeLeftImg, cargoImg) {
        this.shipUpImage = snakeUpImg;
        this.shipRightImage = snakeRightImg;
        this.shipLeftImage = snakeLeftImg;
        this.shipDownImage = snakeDownImg;

        this.cargoImage = cargoImg;
    }

    //method for painting snake
    this.paint = function (context, cellSize) {
        var ship = getShipImageByDirection();
        context.drawImage(ship, this.head.x * cellSize, this.head.y * cellSize, cellSize, cellSize);

        for (var i = 1; i < this.body.length; i++)
            context.drawImage(this.cargoImage, this.body[i].x * cellSize, this.body[i].y * cellSize, cellSize, cellSize);
    }

    //method for return right head image by direction
    function getShipImageByDirection() {
        switch (that.direction) {
            case Direction.UP:
                return that.shipUpImage;

            case Direction.LEFT:
                return that.shipLeftImage;

            case Direction.RIGHT:
                return that.shipRightImage;

            case Direction.DOWN:
                return that.shipDownImage;

            default:
                return that.shipUpImage;
        }
    }

    //method for checking collision with self
    //return true if collision is detected
    this.checkCollisoinWithSelf = function (nextPosition) {
        for (var i = 0; i < this.body.length; i++) {
            if (this.body[i].equal(nextPosition))
                return true;
        }
        return false;
    }

    //method ofr move snake, if eat == true, snake is extended
    this.move = function (eat, nextPostion) {
        if (!eat)
            this.body.pop();

        this.body.unshift(nextPostion);
        this.head = this.body[0];
    }

    //method for getting next postion by direction
    this.getNextPosition = function () {
        var nextPosition = new Position(this.head.x, this.head.y);

        switch (this.direction) {
            case Direction.UP:
                nextPosition.y--;
                break;
            case Direction.DOWN:
                nextPosition.y++;
                break;
            case Direction.LEFT:
                nextPosition.x--;
                break;
            case Direction.RIGHT:
                nextPosition.x++;
                break;
        }

        return nextPosition;
    }

    //method for change direction on key press (key event handler)
    this.changeDirection = function (key) {
        switch(key) {
            case Direction.UP:
                setDirection(Direction.UP, Direction.DOWN);
                break;
            case Direction.DOWN:
                setDirection(Direction.DOWN, Direction.UP);
                break;
            case Direction.LEFT:
                setDirection(Direction.LEFT, Direction.RIGHT);
                break;
            case Direction.RIGHT:
                setDirection(Direction.RIGHT, Direction.LEFT);
                break;
        }
    }

    //method for set snake direction on direction if current direction is not false direction for next direction
    //updated is for eliminating fast key pressing error
    function setDirection(direction, falseDirection){
        if(that.direction != direction && that.direction != falseDirection && that.updated){
            that.direction = direction;
            that.updated = false;
        }
    }

    //direction enum
    //values are key codes
    var Direction = {
        UP: 38,
        DOWN: 40,
        LEFT: 37,
        RIGHT: 39
    }
};