function Snake(length) {
    this.head;
    this.body;
    this.length = length;
    this.direction;
    this.updated = true;
    this.shipUp;
    this.shipRight;
    this.shipLeft;
    this.shipDown;
    this.cargo;

    var that = this;

    this.createSnake = function () {
        this.body = [];
        for (var i = this.length - 1; i >= 0; i--)
            this.body.push(new Position(i, 0));

        this.head = this.body[0];
        this.direction = Direction.RIGHT;
    }

    this.setImages = function (snakeUp, snakeRight, snakeDown, snakeLeft, cargo) {
        this.shipUp = snakeUp;
        this.shipRight = snakeRight;
        this.shipLeft = snakeLeft;
        this.shipDown = snakeDown;

        this.cargo = cargo;
    }

    this.paint = function (context, cellSize) {
        var ship = getShipImageByDirection();
        context.drawImage(ship, this.head.x * cellSize, this.head.y * cellSize, cellSize, cellSize);

        for (var i = 1; i < this.body.length; i++)
            context.drawImage(this.cargo, this.body[i].x * cellSize, this.body[i].y * cellSize, cellSize, cellSize);
    }

    function getShipImageByDirection() {
        switch (that.direction) {
            case Direction.UP:
                return that.shipUp;

            case Direction.LEFT:
                return that.shipLeft;

            case Direction.RIGHT:
                return that.shipRight;

            case Direction.DOWN:
                return that.shipDown;

            default:
                return that.shipUp;
        }
    }

    this.checkCollisoinWithSelf = function (nextPosition) {
        for (var i = 0; i < this.body.length; i++) {
            if (this.body[i].equal(nextPosition))
                return true;
        }
        return false;
    }

    this.move = function (eat, nextPostion) {
        if (!eat)
            this.body.pop();

        this.body.unshift(nextPostion);
        this.head = this.body[0];
    }

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

    function setDirection(direction, falseDirection){
        if(that.direction != direction && that.direction != falseDirection && that.updated){
            that.direction = direction;
            that.updated = false;
        }
    }

    var Direction = {
        UP: 38,
        DOWN: 40,
        LEFT: 37,
        RIGHT: 39
    }
};