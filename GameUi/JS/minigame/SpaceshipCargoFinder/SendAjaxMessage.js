//method for send ajax message
function sendAjaxMessage(id, object, data, callbackFunction) {
    ajax.send({
        requestId: id,
        relatedObject: object,
        data: data,
        callback: callbackFunction
    });
};

//method for send ajax message with repetition
function sendAjaxMessageRepeatedly(id, object, data, repeat, callbackFunction) {
    ajax.send({
        requestId: id,
        relatedObject: object,
        data: data,
        repeatEvery: repeat,
        callback: callbackFunction
    });
};