function ImageLoader() {
    this.cargo;
    this.snakeUp;
    this.snakeRight;
    this.snakeDown;
    this.snakeLeft;
    this.background;

    this.initPaint;

    var loadedImages = 0;
    var LOADED_IMAGES_COUNT = 6;

    var that = this;

    this.createImages = function (initPaint) {
        this.cargo = new Image();
        this.background = new Image();

        this.snakeUp = new Image();
        this.snakeRight = new Image();
        this.snakeDown = new Image();
        this.snakeLeft = new Image();

        this.initPaint = initPaint;

        initOnLoad();
    }

    this.loadImages = function () {
        this.cargo.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/cargo.png';
        this.background.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/background.png';
        
        this.snakeUp.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/shipUp.png';
        this.snakeRight.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/shipRight.png';
        this.snakeDown.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/shipDown.png';
        this.snakeLeft.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/shipLeft.png';
    }

    function initOnLoad() {
        that.cargo.onload = setOnLoad;
        that.background.onload = setOnLoad;

        that.snakeUp.onload = setOnLoad;
        that.snakeRight.onload = setOnLoad;
        that.snakeDown.onload = setOnLoad;
        that.snakeLeft.onload = setOnLoad;
    }

    function setOnLoad() {
        loadedImages++;

        if (loadedImages == LOADED_IMAGES_COUNT)
            that.initPaint();
    }
}