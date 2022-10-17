/// <reference path="../jquery-1.11.1.min.js" />
/// <reference path="bootbox.min.js" />

function BBox_Text(sText) {
    sText = sText == undefined ? '' : sText + '';
    if (sText.length == 0) sText = ' ';
    return sText;
}

var BBox = {
    Common: function (sTitle, sMsg, fnClick, sClass) {
        bootbox.dialog({
            title: BBox_Text(sTitle),
            message: BBox_Text(sMsg),
            closeButton: false,
            buttons: {
                ok: {
                    label: "OK",
                    className: "btn-primary btn-sm",
                    callback: fnClick
                }
            },
            className: sClass
        });
    },
    Info: function (sTitle, sMsg, fnClick, sClass) {
        bootbox.dialog({
            title: '<i class="glyphicon glyphicon-info-sign"></i> ' + BBox_Text(sTitle),
            message: BBox_Text(sMsg),
            closeButton: false,
            buttons: {
                ok: {
                    label: "OK",
                    className: "btn-primary btn-sm",
                    callback: fnClick
                }
            },
            className: 'modal-info ' + BBox_Text(sClass)
        });
    },
    Success: function (sTitle, sMsg, fnClick, sClass) {
        bootbox.dialog({
            title: '<i class="glyphicon glyphicon-ok-sign"></i> ' + BBox_Text(sTitle),
            message: BBox_Text(sMsg),
            closeButton: false,
            buttons: {
                ok: {
                    label: "OK",
                    className: "btn-primary btn-sm",
                    callback: fnClick
                }
            },
            className: 'modal-success ' + BBox_Text(sClass)
        });
    },
    Error: function (sTitle, sMsg, fnClick, sClass) {
        bootbox.dialog({
            title: '<i class="glyphicon glyphicon-remove-sign"></i> ' + BBox_Text(sTitle),
            message: BBox_Text(sMsg),
            closeButton: false,
            buttons: {
                ok: {
                    label: "OK",
                    className: "btn-primary btn-sm",
                    callback: fnClick
                }
            },
            className: 'modal-danger ' + BBox_Text(sClass)
        });
    },
    Warning: function (sTitle, sMsg, fnClick, sClass) {
        bootbox.dialog({
            title: '<i class="glyphicon glyphicon-exclamation-sign"></i> ' + BBox_Text(sTitle),
            message: BBox_Text(sMsg),
            closeButton: false,
            buttons: {
                ok: {
                    label: "OK",
                    className: "btn-primary btn-sm",
                    callback: fnClick
                }
            },
            className: 'modal-warning ' + BBox_Text(sClass)
        });
    },
    Confirm: function (sTitle, sMsg, fnConfirm, fnCancel, sClass) {
        bootbox.dialog({
            title: '<i class="glyphicon glyphicon-question-sign"></i> ' + BBox_Text(sTitle),
            message: BBox_Text(sMsg),
            closeButton: false,
            buttons: {
                confirm: {
                    label: "Confirm",
                    className: "btn-success btn-sm",
                    callback: fnConfirm
                },
                cancel: {
                    label: "Cancel",
                    className: "btn-primary btn-sm",
                    callback: fnCancel
                }
            },
            className: 'modal-primary ' + BBox_Text(sClass)
        });
    },
    ConfirmYN: function (sTitle, sMsg, fnYes, fnNo, sClass) {
        bootbox.dialog({
            title: '<i class="glyphicon glyphicon-question-sign"></i> ' + BBox_Text(sTitle),
            message: BBox_Text(sMsg),
            closeButton: false,
            buttons: {
                confirm: {
                    label: "Yes",
                    className: "btn-success btn-sm",
                    callback: fnYes
                },
                cancel: {
                    label: "No",
                    className: "btn-primary btn-sm",
                    callback: fnNo
                }
            },
            className: 'modal-primary ' + BBox_Text(sClass)
        });
    },
    ButtonEnabled: function (enabled) {
        enabled = Boolean(enabled);
        $('.bootbox .modal-footer button').prop('disabled', !enabled);
    },
    Close: bootbox.hideAll
};