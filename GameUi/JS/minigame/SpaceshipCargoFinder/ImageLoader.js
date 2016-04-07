//Class for loading images
function ImageLoader() {
    //cargo image (food)
    this.cargo;

    //snake up image
    this.snakeUp;

    //snake right image
    this.snakeRight;

    //snake down image
    this.snakeDown;

    //snake left image
    this.snakeLeft;

    //background image
    this.background;

    //initialization paint method
    this.initPaint;

    //number of loaded images
    var loadedImages = 0;

    //expect number of loaded images
    var LOADED_IMAGES_COUNT = 6;

    //this for private method
    var that = this;

    //method for creating images
    //initPaint is initialization draw method
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

    //metdho for load images
    this.loadImages = function () {
        this.cargo.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/cargo.png';
        this.background.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/background.png';
        
        this.snakeUp.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/shipUp.png';
        this.snakeRight.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/shipRight.png';
        this.snakeDown.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/shipDown.png';
        this.snakeLeft.src = '~/../../Content/images/Minigame/SpaceshipCargoFinder/shipLeft.png';
    }

    //method of init on load event
    function initOnLoad() {
        that.cargo.onload = setOnLoad;
        that.background.onload = setOnLoad;

        that.snakeUp.onload = setOnLoad;
        that.snakeRight.onload = setOnLoad;
        that.snakeDown.onload = setOnLoad;
        that.snakeLeft.onload = setOnLoad;
    }

    ///on load event handler method
    function setOnLoad() {
        loadedImages++;

        //if all images are loaded paint them
        if (loadedImages == LOADED_IMAGES_COUNT)
            that.initPaint();
    }
}