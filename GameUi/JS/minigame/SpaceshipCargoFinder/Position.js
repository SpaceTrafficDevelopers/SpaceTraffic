//Position class.
function Position(x, y) {
    //x coordinate
    this.x = x;

    //y coordinate
    this.y = y;

    //equeal method
    this.equal = function (position) {
        if (this.x == position.x && this.y == position.y)
            return true;

        return false;
    }
};