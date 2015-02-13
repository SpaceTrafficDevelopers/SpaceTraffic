/// <reference path="../jquery-1.5.1-vsdoc.js" />
/// <reference path="../jquery-ui-1.8.11.js" />

/// Creates jQuery UI tabs on each div element with data-tabs attribute.
$(document).ready(function () {
    $('div[data-tabs]').tabs();
});