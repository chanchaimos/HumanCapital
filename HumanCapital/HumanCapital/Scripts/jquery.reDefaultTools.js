/// <reference path="jquery-1.11.1.min.js" />
$(document)
    .ready(function () {
        $(this).delegate('input:not([type=submit], [type=button])', 'keydown', function (e) {
            if (e.which == 13)  //e.which = keyCode (ENTER : 13)
                return false;
        });
    })
    .keydown(function (e) {
        if (e.which === 8 && !$(e.target).is('input, textarea')) {  //e.which = keyCode (BACKSPACE : 13)
            e.preventDefault();
        }
    });