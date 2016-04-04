function Position(x, y) {
    this.x = x;
    this.y = y;

    this.equal = function (position) {
        if (this.x == position.x && this.y == position.y)
            return true;

        return false;
    }
};