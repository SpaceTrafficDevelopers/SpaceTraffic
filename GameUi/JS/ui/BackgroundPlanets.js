/*
    Hides planets which don't fit to actual window width, so that sun is always centered.
    Uses jQuery.
*/
function hidePlanets(win) {
    $("#planets_right").children("#planets_spacer").css("margin-left", "0");
    $("#planets_left").children("#planets_spacer").css("margin-left", "0");

    var left = 0;
    var right = 0;
    var half = ($(win).width() - $(".sun").outerWidth(true)) / 2 - 40;

    $($("#planets_left").children(".planet").get().reverse()).each(function () {
        var width = $(this).outerWidth(true);
        if (half > (left + width)) {
            left += width;
            $(this).css("display", "inherit");
        } else {
            $(this).css("display", "none");
        }
    });

    $("#planets_right").children(".planet").each(function () {
        var width = $(this).outerWidth(true);
        if (half > (right + width)) {
            right += width;
            $(this).css("display", "inherit");
        } else {
            $(this).css("display", "none");
        }
    });

    if (left < right) {
        $("#planets_left").children("#planets_spacer").css("margin-left", (right - left));
    } else if (right < left) {
        $("#planets_right").children("#planets_spacer").css("margin-left", (left - right));
    }
}