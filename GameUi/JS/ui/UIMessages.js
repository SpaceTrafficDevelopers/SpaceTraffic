$(document).ready(function () {
    ajax.send({
        requestId: 'UIMessages',
        relatedObject: 'UIMessages',
        data: {},
        repeatEvery: 130,
        callback: function (messages) {
            if (messages && messages.length != 0) {
                var messagesElement = '';

                for (var i = 0; i < messages.length; i++) {
                    messagesElement += '<li>' + messages[i] + '</li>'
                }

                var infoStream = $("#infoStream");
                infoStream.empty();
                infoStream.append(messagesElement);
                infoStream.marquee("update");
            }
        }
    });
});