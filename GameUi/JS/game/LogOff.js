$(document).ready(function () {
    $.ajaxSetup({
        cache: false
    });

    $(document).ajaxError(
        function (e, request) {
            if (request.status == 401) {
                window.location.replace(baseUrl + "Account/LogOn");
            }
        }
    );
})