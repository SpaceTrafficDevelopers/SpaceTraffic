var svgns = "http://www.w3.org/2000/svg";

function makeCircle(evt) {
    if (window.svgDocument == null)
        svgDocument = evt.target.ownerDocument;

    var shape = svgDocument.createElementNS(svgns, "circle");
    shape.setAttributeNS(null, "cx", 25);
    shape.setAttributeNS(null, "cy", 25);
    shape.setAttributeNS(null, "r", 20);
    shape.setAttributeNS(null, "fill", "green");

    var click = svgDocument.createElementNS(svgns, "click");
    click.
    set.setAttributeNS(null, "to", "blue");
    set.setAttributeNS(null, "begin", "click")

    shape.appendChild(click)
    svgDocument.documentElement.appendChild(shape);
}

function makeLine(evt) {
    if (window.svgDocument == null)
        svgDocument = evt.target.ownerDocument;

    var shape = svgDocument.createElementNS(svgns, "line");
    shape.setAttributeNS(null, "x1", 5);
    shape.setAttributeNS(null, "y1", 5);
    shape.setAttributeNS(null, "x2", 45);
    shape.setAttributeNS(null, "y2", 45);
    shape.setAttributeNS(null, "stroke", "green");

    svgDocument.documentElement.appendChild(shape);
}

function makeShape(e) {
    if (window.svgDocument == null)
        svgDocument = e.target.ownerDocument;

    var data = svgDocument.createTextNode("Text");

    var text = svgDocument.createElementNS(svgns, "text");
    text.setAttributeNS(null, "x", "25");
    text.setAttributeNS(null, "y", "13");
    text.setAttributeNS(null, "fill", "green");
    text.setAttributeNS(null, "text-anchor", "middle");

    text.appendChild(data);
    svgDocument.documentElement.appendChild(text);
}

