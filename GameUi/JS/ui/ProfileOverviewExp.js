/*
 Exp ring script - using HTML Canvas and Mootools FX for animation
 Created from:
    http://www.w3schools.com/tags/canvas_arc.asp
    http://mootools.net/core/docs/1.5.2/Fx/Fx
    http://jsfiddle.net/oskar/Aapn8/
*/
var canvas = document.getElementById('exp_ring');
var context = context = canvas.getContext('2d');
var imgdata = null;
var circ = Math.PI * 2;
var quart = Math.PI / 2;

context.beginPath();
context.strokeStyle = '#373535'; //background circle color
context.lineCap = 'square';
context.closePath();
context.fill();
context.lineWidth = 20.0;

context.beginPath();
context.arc(169, 169, 155, 0, 2 * Math.PI); //position and radius of background circle
context.stroke();

imgdata = context.getImageData(0, 0, 338, 338); //size of canvas

var draw = function (current) {
    context.putImageData(imgdata, 0, 0);
    context.beginPath();
    context.arc(169, 169, 155, -(quart), ((circ) * current) - quart, false);
    context.stroke();
}

var myFx = new Fx({
    duration: 3000,
    onStep: function (step) {
        draw(step / 100);
    }
});

myFx.set = function (now) {
    var ret = Fx.prototype.set.call(this, now);
    this.fireEvent('step', now);
    return ret;
};

function ringAnimationStart(percents, isLastLevel) {
    if (isLastLevel == true) {
        context.strokeStyle = '#e5c100'; //front circle color last level
    } else {
        context.strokeStyle = '#48a2f0'; //front circle color
    }

    myFx.start(0, percents);
}