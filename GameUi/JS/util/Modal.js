// Simple modal widget, uses jQuery
$(document).ready(function () {
    var id;
    
    // Opens modal window if link with specified data-tog="modal" was clicked
    $('a[data-tog=modal]').click(function(e) {
        e.preventDefault();
        id = $(this).attr('href');
         $(id).css('display', 'block');
    });
    
    // Closes modal window on close button click
    $('.modal .close').click(function(e) {
        e.preventDefault();
        $(id).fadeOut(350);
    });
    
    //Closes modal window if clicked out of modal 
    $(window).click(function(e) {
        if($(e.target).is(id)) {
            $(id).fadeOut(350);
        }
    });
});

