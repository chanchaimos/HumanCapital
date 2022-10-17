var AlertTitle = {
    Success: "Action Completed",
    Warning: "Warning",
    Error: "Error",
    Info: "Information",
    Confirm: "Confirmation",
    Hint: "Hint"
};

var AlertMsg = {
    SaveComplete: "Data has already been saved.",
    DeleteComplete: "Data is already deleted.",
    SendMailComplete: "The mail has been sent successfully.",
    Warning: "Warning",
    Error: "Some thing went wrong",
    Info: "Info",
    ConfirmSave: "Do you want to save data?",
    ConfirmSubmit: "Do you want to submit data?",
    ConfirmSendApprove: "Do you want to send approval data?",
    ConfirmSendMail: "Do you want to send mail?",
    ConfirmSaveDraft: "Do you want to save draft data?",
    ConfirmDelete: "Do you want to delete data?",
    ConfirmExport: "Do you want to export?",
    ConfirmResetPassword: "Do you want to reset password?",
    ConfirmApprove: "Do you want to approve data?",
    ConfirmAcknowledge: "Do you want to acknowledge data?",
    ConfirmNotApprove: "Do you want to disapprove data?",
    ConfirmRevisit: "Do you want to revisit data?",
    ConfirmReject: "Do you want to reject data?",
    ConfirmRecall: "Do you want to recall data?",
    ConfirmRequestEdit: "Do you want to request edit data?",
    ConfirmCancel: "Do you want to cancel data?",
    ConfirmSyncCompany: "Do you want to sync data?",
    NoPermission: "Access denied",
    SessionTimeOut: "Session time out",
    DeleteFail: 'Please select <i class="far fa-check-square"></i>',
    ExportFail: 'Please select <i class="far fa-check-square"></i>',
    Desc_Warning_Duplicate: 'Data cannot be duplicated. </br> Please re-enter the information.'
};

function PopupNoPermission(sPathRedirect) {
    BBox.Error(AlertTitle.Warning, AlertMsg.NoPermission, function () {
        window.location.href = sPathRedirect !== undefined ? sPathRedirect : "index.aspx";
    });
}

function PopupSessionTimeOut() {
    BBox.Error(AlertTitle.Warning, AlertMsg.SessionTimeOut, function () { window.location.href = "logout.aspx"; });
}

function DialogConfirmSubmit(funcYes, funcNo) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmSubmit, funcYes, funcNo);
}

function DialogConfirmSendApprove(funcYes, funcNo) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmSendApprove, funcYes, funcNo);
}

function DialogConfirmCancel(funcYes, funcNo) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmCancel, funcYes, funcNo);
}

function DialogSucessRedirect(sPath) {
    BBox.Success(AlertTitle.Info, AlertMsg.SaveComplete, function () { window.location.href = sPath });
}

function DialogSucessSendMailRedirect(sPath) {
    BBox.Success(AlertTitle.Info, AlertMsg.SendMailComplete, function () { window.location.href = sPath });
}

function DialogSucess() {
    BBox.Success(AlertTitle.Info, AlertMsg.SaveComplete);
}

function DialogDeleteError() {
    BBox.Error(AlertTitle.Warning, AlertMsg.DeleteFail);
}

function DialogExportError() {
    BBox.Error(AlertTitle.Warning, AlertMsg.ExportFail);
}

function DialogDuplicate() {
    BBox.Error(AlertTitle.Warning, AlertMsg.Desc_Warning_Duplicate);
}

function DialogConfirmDel(funcDel) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmDelete, funcDel);
}

function DialogConfirmApprove(func) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmApprove, func);
}

function DialogConfirmReject(func) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmReject, func);
}

function DialogConfirmRevisit(func) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmRevisit, func);
}

function DialogConfirmRecall(func) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmRecall, func);
}

function DialogConfirmRequestEdit(func) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmRequestEdit, func);
}

function DialogConfirmResetPassword(func) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmResetPassword, func);
}

function DialogConfirmExport(func) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmExport, func);
}

function DialogConfirmSendMail(funcYes, funcNo) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmSendMail, funcYes, funcNo);
}

function DialogWarningDelHasUse(sMsg) {
    BBox.Warning(AlertTitle.Warning, sMsg);
}

function DialogShowDetail(sMsg) {
    BBox.Common(AlertTitle.Info, sMsg);
}

function DialogWarning(sMsg) {
    BBox.Warning(AlertTitle.Warning, sMsg);
}

function DialogLogout() {
    BBox.Confirm(AlertTitle.Confirm, "Do you want to logout?", function () { BlockUI(); window.location.href = "logout.aspx"; });
}

function DialogWarningRedirect(sMsg, sPath) {
    BBox.Warning(AlertTitle.Warning, sMsg, function () { BlockUI(); window.location.href = sPath });
}

function DialogSaveFail(sMsg) {
    BBox.Error(AlertTitle.Warning, (sMsg != "" ? sMsg : "Cannot action"), function () { BlockUI(); location.reload(); });
}

function DialogShowHint(sMsg) {
    BBox.Common("<span style='color:#43d443'>" + AlertTitle.Hint + "</span>", sMsg);
}

function DialogDeleteSucess() {
    BBox.Success(AlertTitle.Info, AlertMsg.DeleteComplete);
}

function DialogDeleteSucessRedirect(sPath) {
    BBox.Success(AlertTitle.Info, AlertMsg.DeleteComplete, function () { window.location.href = sPath });
}

function DialogConfirmSyncCompany(funcYes) {
    BBox.Confirm(AlertTitle.Confirm, AlertMsg.ConfirmSyncCompany, funcYes);
}

var objControl = {
    txtbox: "input",
    txtarea: "textarea",
    dropdown: "select",
    div: "div",
    span: "span",
    rdl: "input",
    cbl: "input",
    btn: "input",
    radio: "radio"
};

//process code behind return
var SysProcess = {
    FileOversize: "OverSize",
    FileInvalidType: "InvalidType",
    Failed: "Failed",
    Success: "Success",
    SessionExpired: "SSEXP",
    Duplicate: "DUP",
    SaveFail: "SaveFail"
}

var IsRequire = " is require!";

function GetElementName(sElement, objType) {
    return $(objType + "[id$=" + sElement + "]").attr("name");
}
function GetElementName2(sElement, objType) {
    return $(objType + "[name$=" + sElement + "]").attr("name");
}

function GetCheckboxlistName(sName) {
    return $('input[name*=' + sName + ']').attr('name');
}

//Validate
function BindValidate(sContainer, objValidate) {
    $("#" + sContainer).formValidation({
        framework: 'bootstrap',
        excluded: ':disabled',
        err: {
            //container: 'tooltip'
        },
        icon: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: objValidate,
        autoFocus: true,

    }).on('err.validator.fv', function (e, data) {
        data.element
            .data('fv.messages')
            // Hide all the messages
            .find('.help-block[data-fv-for="' + data.field + '"]').hide()
            // Show only message associated with current validator
            .filter('[data-fv-validator="' + data.validator + '"]').show();
    });
}

function ADD_Validate(sContainer, fn_validate, name) {
    $("#" + sContainer).formValidation('addField', name, fn_validate);
}

function Remove_Validate(sContainer, fn_validate, name) {
    $("#" + sContainer).formValidation('removeField', name, fn_validate);
}

function CheckValidate(sContainer) {
    var isValid = $("#" + sContainer).data('formValidation').validate().isValid();
    if (!isValid) {
        ScrollTopToElements($($("div#" + sContainer).data('formValidation').$invalidFields[0]).attr("id"));//$("div#" + sContainer).data('formValidation').$invalidFields[0].focus();
    }
    return isValid;
}

function ScrollTopToElements(sElementID) {
    $('html, body').animate({ scrollTop: $("#" + sElementID).offset().top - 120 }, 'fast');
}

function addValidate_notEmpty(msg) {
    return {
        validators: {
            notEmpty: {
                message: msg
            }
        }
    };
}

function addValidate_Url(msg) {
    return {
        validators: {
            uri: {
                message: msg
            }
        }
    };
}

function addValidate_notEmpty_Length(msg, maxLength, msgmaxLength) {
    return {
        validators: {
            notEmpty: {
                message: msg
            },
            stringLength: {
                max: maxLength,
                message: msgmaxLength
            }
        }
    };
}

function addValidate_TextAreaMax(msg, maxLength) {
    return {
        validators: {
            stringLength: {
                max: maxLength,
                message: msg
            }
        }
    };
}

function addValidate_notEmpty_Field(msg, sNameField) {
    return {
        validators: {
            callback: {
                message: msg,
                callback: function (value, validator, $field) {
                    var sHID = $("input[id$=" + sNameField + "]").val();
                    return !(value + "" == "" || value == null || value == undefined) ? !(sHID + "" == "" || sHID == null || sHID == undefined) : false;
                }
            }
        }
    };
}

function addValidate_notEmpty_Field_DDL(msg, sNameField, DDL) {
    return {
        validators: {
            callback: {
                message: msg,
                callback: function (value, validator, $field) {
                    var sHID = $("input[id$=" + sNameField + "]").val();
                    var sDDl = $("select[id$=" + DDL + "]").val();
                    return (sDDl == "1") ? !(sHID + "" == "" || sHID == null || sHID == undefined) : value !== "";
                    //return !(value + "" == "" || value == null || value == undefined) ?  : false;
                }
            }
        }
    };
}

function addValidate_notEmpty_OtherRadio(msg, sNameRadio, isTrue) {
    return {
        validators: {
            callback: {
                message: msg,
                callback: function (value, validator, $field) {
                    var sName = $("input[name$=" + sNameRadio + "]:checked").val();
                    //ถ้า รถภายใน 
                    return (sName == isTrue) ? true : value !== "";
                }
            }
        }
    };
}

function addValidate_notEmpty_OtherDropDown(msg, sNameDDL, isTrue) {
    return {
        validators: {
            callback: {
                message: msg,
                callback: function (value, validator, $field) {
                    var sName = $("select[id$=" + sNameDDL + "]").val();
                    //ถ้า รถภายใน 
                    return (sName == isTrue || sName == "") ? true : value !== "";
                }
            }
        }
    };
}

function addValidate_rdlNotEmpty(msg) {
    return {
        validators: {
            choice: {
                min: 1,
                message: msg
            }
        }
    };
}

function addValidate_cblNotEmpty(msg) {
    return {
        validators: {
            choice: {
                min: 1,
                message: msg
            }
        }
    };
}

//เวลาที่เริ่มต้น ไอดีเวลาสิ้นสุด
function addValidate_ComPareStartTime(msgNoempty, msgCompare, TimeEndID) {
    return {
        validators: {
            notEmpty: {
                message: msgNoempty
            },
            callback: {
                message: msgCompare,
                callback: function (value, validator, $field) {

                    var endTime = $("input[id$=" + TimeEndID + "]").val(); //validator.getFieldElements(TimeEndID).val();
                    if (endTime === '' || endTime === '-- : --') {
                        return true;
                    }
                    var startHour = parseInt(value.split(':')[0], 10),
                        startMinutes = parseInt(value.split(':')[1], 10),
                        endHour = parseInt(endTime.split(':')[0], 10),
                        endMinutes = parseInt(endTime.split(':')[1], 10);

                    if (startHour < endHour || (startHour == endHour && startMinutes < endMinutes)) {
                        // The end time is also valid
                        // So, we need to update its status
                        validator.updateStatus('endTime', validator.STATUS_VALID, 'callback');
                        return true;
                    }

                    return false;
                }
            }
        }
    }
}

//เวลาที่เริ่มต้น ไอดีเวลาเริ่มต้น
function addValidate_ComPareEndTime(msgNoempty, msgCompare, StartEndID) {
    return {
        validators: {
            notEmpty: {
                message: msgNoempty
            },
            callback: {
                message: msgCompare,
                callback: function (value, validator, $field) {
                    var startTime = $("input[id$=" + StartEndID + "]").val(); //validator.getFieldElements(StartEndID).val();
                    if (startTime == '' || startTime === '-- : --') {
                        return true;
                    }
                    var startHour = parseInt(startTime.split(':')[0], 10),
                        startMinutes = parseInt(startTime.split(':')[1], 10),
                        endHour = parseInt(value.split(':')[0], 10),
                        endMinutes = parseInt(value.split(':')[1], 10);

                    if (endHour > startHour || (endHour == startHour && endMinutes > startMinutes)) {
                        // The start time is also valid
                        // So, we need to update its status
                        validator.updateStatus('startTime', validator.STATUS_VALID, 'callback');
                        return true;
                    }

                    return false;
                }
            }
        }
    }
}

function addValidateEmail_notEmpty() {//กรณีที่มีการกำหนด data-inputmask เนื่องจากไม่สามารถใช้ notEmpty ได้
    return {
        validators: {
            regexp: {
                regexp: "^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$",
                //message: "รูปแบบ E-mail ไม่ถูกต้อง" 
                message: "Invalid e-mail format"
            },
            callback: {
                //message: "ระบุ E-mail",
                message: "E-mail is require!",
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                }
            }
        }
    };
}

function addValidatePassword_notEmpty(maxLength, msgEmpty) {
    return {
        validators: {
            regexp: {
                //regexp: "^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.*[!@#\$%\^&\*])(?=.{8,})",
                regexp: "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
                //message: "รหัสผ่านจะต้องมีอย่างน้อย 8 ตัวอักษร และประกอบไปด้วย ตัวเลข ตัวอักษรตัวพิมพ์เล็ก ตัวอักษรตัวพิมพ์ใหญ่ และสัญลักษณ์ อย่างน้อย 1 ตัว"//Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols.
                message: "Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols."//Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols.
            },
            callback: {
                message: (msgEmpty != undefined && msgEmpty != "") ? msgEmpty : "Password is require!",
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                }
            },
            stringLength: {
                max: maxLength,
                //message: 'ขนาดไม่เกิน ' + maxLength + ' อักษร'//msgmaxLength
                message: 'Not more than ' + maxLength + ' characters'//msgmaxLength
            }
        }
    };
}

function addValidatePassword_notEmpty_Confirm(txt1) {
    return {
        validators: {
            callback: {
                callback: function (value, validator, $field) {
                    var value = $field.val();
                    if (value === '') {
                        return {
                            valid: false,
                            //message: 'ระบุ รหัสผ่าน'
                        };
                    }

                    // Check the password strength
                    if (value.length < 8) {
                        return {
                            valid: false,
                            //message: 'รหัสผ่านไม่ตรงกัน'
                            message: 'Passwords do not match.'
                            //message: 'ระบุ รหัสผ่านอย่างน้อย 8 ตัวอักษร'
                        };
                    }

                    // The password doesn't contain any uppercase character
                    if (value === value.toLowerCase()) {
                        return {
                            valid: false,
                            //message: 'รหัสผ่านไม่ตรงกัน'
                            message: 'Passwords do not match.'
                            //message: 'ระบุ ตัวอักษรตัวพิมพ์ใหญ่อย่างน้อย 1 ตัวอักษร'
                        }
                    }

                    // The password doesn't contain any uppercase character
                    if (value === value.toUpperCase()) {
                        return {
                            valid: false,
                            //message: 'รหัสผ่านไม่ตรงกัน'
                            message: 'Passwords do not match.'
                            //message: 'ระบุ ตัวอักษรตัวพิมพ์เล็กอย่างน้อย 1 ตัวอักษร'
                        }
                    }

                    // The password doesn't contain any digit
                    if (value.search(/[0-9]/) < 0) {
                        return {
                            valid: false,
                            //message: 'รหัสผ่านไม่ตรงกัน'
                            message: 'Passwords do not match.'
                            //message: 'ระบุ ตัวเลขอย่างน้อย 1 ตัว'
                        }
                    }

                    // The password doesn't contain any digit
                    if (value != $("input[name$=" + txt1 + "]").val()) {
                        return {
                            valid: false,
                            //message: 'รหัสผ่านไม่ตรงกัน'
                            message: 'Passwords do not match.'
                            //message: 'ระบุ รหัสผ่านไม่ตรงกัน'
                        }
                    }
                    return true;
                }
            }
        }
    };
}

function addValidate_notEmpty_LengthMin(msg, minLength, msgmaxLength) {
    return {
        validators: {
            notEmpty: {
                message: msg
            },
            stringLength: {
                min: minLength,
                message: msgmaxLength
            }
        }
    };
}

function addValidate_Hastag(msg) {
    return {
        validators: {
            callback: {
                message: msg,
                callback: function (value, validator, $field) {
                    var arr = value.split(",");
                    var nFail = 0;
                    $.each(arr, function (i, el) {

                        if (el) {
                            if ((/^#\w/).test(el)) {

                            } else {
                                nFail++;
                            }
                        }
                    });

                    return (nFail == 0);
                }
            }
        }
    };
}


function ADD_Validate(sContainer, fn_validate, name) {
    $("#" + sContainer).formValidation('addField', name, fn_validate);
}

function Remove_Validate(sContainer, fn_validate, name) {
    $("#" + sContainer).formValidation('removeField', name, fn_validate);
}

function ReValidateFieldControl(sContainer, ctrlName) {
    $('#' + sContainer).formValidation('revalidateField', ctrlName);
}

function EnableValidateControl(sContainer, ctrlName, Status) {
    $('#' + sContainer).formValidation('enableFieldValidators', ctrlName, Status);
}

function VALIDATED(sNameDiv, sTypeCtr, sNameCtr) {
    $("#" + sNameDiv + "").formValidation('updateStatus', '' + $("" + sTypeCtr + "[id$=" + sNameCtr + "]").attr("name") + '', 'VALIDATED');
}
function NOT_VALIDATED(sNameDiv, sTypeCtr, sNameCtr) {
    $("#" + sNameDiv + "").formValidation('updateStatus', '' + $("" + sTypeCtr + "[id$=" + sNameCtr + "]").attr("name") + '', 'NOT_VALIDATED');
}
function SetNotValidateTextbox(sNameDiv, sNameCtr) {
    $("#" + sNameDiv + "").formValidation("updateStatus", GetElementName(sNameCtr, objControl.txtbox), "NOT_VALIDATED");
}
function SetNotValidateSelect(sNameDiv, sNameCtr) {
    $("#" + sNameDiv + "").formValidation("updateStatus", GetElementName(sNameCtr, objControl.dropdown), "NOT_VALIDATED");
}
function SetNotValidateTextarea(sNameDiv, sNameCtr) {
    $("#" + sNameDiv + "").formValidation("updateStatus", GetElementName(sNameCtr, objControl.txtarea), "NOT_VALIDATED");
}
function SetNotValidateCheckboxList(sNameDiv, sNameCtr) {
    $("#" + sNameDiv + "").formValidation("updateStatus", GetElementName(sNameCtr), "NOT_VALIDATED");
}
function SetNotValidateRadioList(sNameDiv, sNameCtr) {
    $("#" + sNameDiv + "").formValidation("updateStatus", GetElementName2(sNameCtr, 'input'), "NOT_VALIDATED");
}

//WebMedthod
function AjaxWebMethod(sWebMetodName, objJSON, fnSuccess, fnComplete, fnError, showError) {
    if (sWebMetodName != undefined) {
        var sLocationPath = location.pathname;
        $.ajax({
            type: 'POST',
            url: sLocationPath + '/' + sWebMetodName, //fileName.aspx/FunctionName
            data: JSON.stringify(objJSON), //Variable in function
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: fnSuccess != undefined ? fnSuccess : function () { },
            error: function (xhr, status, err) {
                var funcError = function () { if (fnError != undefined) fnError(); };

                if (Boolean(showError)) {
                    var objResponse = eval('(' + xhr.responseText + ')');

                    var sNewLine = '<br />';
                    var sErrorMessage =
                        '<div class="text-left">' +
                        '<b>ExceptionType :</b>&nbsp;' + objResponse.ExceptionType + sNewLine +
                        '<b>Message :</b>&nbsp;' + objResponse.Message + sNewLine +
                        '<b>StackTrace :</b>' + sNewLine + objResponse.StackTrace.replace(/(?:\r\n|\r|\n)/g, sNewLine) +
                        '</div>';

                    DialogWarning(sErrorMessage)
                    //SwAlert.Error('Something went wrong!', sErrorMessage, funcError);
                }
                else funcError();
            },
            complete: fnComplete != undefined ? fnComplete : function () { },
        });
    }
} //Void

//WebMedthod
function AjaxWebMethod_Async(sWebMetodName, objJSON, fnSuccess, fnComplete, fnError, showError, IsAsync) {
    if (sWebMetodName != undefined) {
        var sLocationPath = location.pathname;
        $.ajax({
            type: 'POST',
            url: sLocationPath + '/' + sWebMetodName, //fileName.aspx/FunctionName
            data: JSON.stringify(objJSON), //Variable in function
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: IsAsync,
            success: fnSuccess != undefined ? fnSuccess : function () { },
            error: function (xhr, status, err) {
                var funcError = function () { if (fnError != undefined) fnError(); };

                if (Boolean(showError)) {
                    var objResponse = eval('(' + xhr.responseText + ')');

                    var sNewLine = '<br />';
                    var sErrorMessage =
                        '<div class="text-left">' +
                        '<b>ExceptionType :</b>&nbsp;' + objResponse.ExceptionType + sNewLine +
                        '<b>Message :</b>&nbsp;' + objResponse.Message + sNewLine +
                        '<b>StackTrace :</b>' + sNewLine + objResponse.StackTrace.replace(/(?:\r\n|\r|\n)/g, sNewLine) +
                        '</div>';

                    DialogWarning(sErrorMessage)
                    //SwAlert.Error('Something went wrong!', sErrorMessage, funcError);
                }
                else funcError();
            },
            complete: fnComplete != undefined ? fnComplete : function () { },
        });
    }
} //Void

//WebMedthod Bypass
function AjaxWebMethod_Bypass(sWebMetodName, objJSON, fnSuccess, fnComplete, fnError, showError) {
    if (sWebMetodName != undefined) {
        var arrUrl = location.pathname.split("/");
        var sLocationPath = arrUrl[arrUrl.length - 1] == "bypass.aspx" ? location.pathname : location.pathname + "/bypass.aspx";
        $.ajax({
            type: 'POST',
            url: sLocationPath + '/' + sWebMetodName, //fileName.aspx/FunctionName
            data: JSON.stringify(objJSON), //Variable in function
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: fnSuccess != undefined ? fnSuccess : function () { },
            error: function (xhr, status, err) {
                var funcError = function () { if (fnError != undefined) fnError(); };

                if (Boolean(showError)) {
                    var objResponse = eval('(' + xhr.responseText + ')');

                    var sNewLine = '<br />';
                    var sErrorMessage =
                        '<div class="text-left">' +
                        '<b>ExceptionType :</b>&nbsp;' + objResponse.ExceptionType + sNewLine +
                        '<b>Message :</b>&nbsp;' + objResponse.Message + sNewLine +
                        '<b>StackTrace :</b>' + sNewLine + objResponse.StackTrace.replace(/(?:\r\n|\r|\n)/g, sNewLine) +
                        '</div>';

                    DialogWarning(sErrorMessage)
                    //SwAlert.Error('Something went wrong!', sErrorMessage, funcError);
                }
                else funcError();
            },
            complete: fnComplete != undefined ? fnComplete : function () { },
        });
    }
} //Void

// paging
function DataChecking(arrData, $ObjNoData) {
    var isDataFound = arrData.length > 0;
    var $noData = ($ObjNoData);
    if (isDataFound) $noData.hide('fast').css('display', 'none');
    else $noData.show('fast').css('display', '');

    return isDataFound;
} //Boolean
function DataPageCount(arrData, nPageSize) {
    nPageSize = +nPageSize;
    var isExactDivision = arrData.length % nPageSize == 0; //หารลงตัว
    return (arrData.length / nPageSize) + (isExactDivision ? 0 : 1); //จำนวนหน้าข้อมูล
} //Integer
function DataPaging(arrData, nPageSize, nPageNo) {
    //slice(nStartIndex, nEndIndex) - [0,1,2,3,4,5,6,7,8,9,0].slice(1,3) -> [1,2]
    nPageSize = +nPageSize;
    nPageNo = +nPageNo;
    var nStartIndex_Take = (nPageNo - 1) * nPageSize;
    return arrData.slice(nStartIndex_Take, nStartIndex_Take + nPageSize);
} //Array<Data>
function DataBind($table, arrData, nPageSize, nPageNo, fnCreateRow, fnDataBound) {
    nPageSize = +nPageSize;
    nPageNo = +nPageNo;

    var $tbody = $table.children('tbody');
    $tbody.children('tr').remove();
    var nPageNoStart = ((nPageNo - 1) * nPageSize) + 1;
    var nDataRow = arrData.length;
    for (var i = 0; i < nDataRow; i++) {
        var tr = fnCreateRow(arrData[i], nPageNoStart + i);
        $tbody.append(tr);
    }
    if (fnDataBound != undefined) fnDataBound();
} //Void

function Material_Popup(sHead, sBody) {
    $("#myModal").modal();
    $("#myModal .modal-header").html(sHead);
    $("#myModal .modal-body").html(sBody);
}

function Material_PopupConfirm(sHead, fnYes) {
    $("#myModal2").modal({
        backdrop: 'static',
        keyboard: false
    });
    $("#myModal2 .modal-header").html(sHead);
    $("button[id$=btnSubmitModal]").attr("onclick", fnYes);
}

//UploadFile
var Extension = {
    Image: ['jpg', 'jpeg', 'png', 'gif'],
    Video: ['mov', 'wmv', 'avi', 'mp4'],
    Document: ['doc', 'docx', 'xls', 'xlsx', 'pdf'],
    Other: ['rar', 'zip'],
    MED: ['jpg', 'jpeg', 'png', 'pdf', 'doc', 'docx', 'xls', 'xlsx', 'ppt', 'pptx'],
    MED_Poster: ['jpg', 'jpeg', 'png', 'pdf'],
    MED_Rest: ['jpg', 'jpeg', 'png'],
    GetAll: function () {
        var arrExt = [];
        for (var key in this) {
            if (key != 'GetAll') arrExt = arrExt.concat(this[key]);
        }
        return arrExt;
    },
    Limitsize_1MB: 1048576,
    Limitsize_2MB: 2097152,
    Limitsize_5MB: 5242880,
};

function FileName_SetMaxLength(sFileName, sFileExt, nMaxLength) {
    var arrFileName = sFileName.split('.');
    var newMaxLength = nMaxLength - (sFileExt.length + 1);
    return arrFileName.slice(0, -1).join('.').substr(0, newMaxLength) + '.' + sFileExt;
}

function AsyncUploadFile_BindEvent(arrAllowFile, sTempPath, $afu, $txtFileName, $hidFileName, $hidSysFileName, $hidFilePath,
    $btnBrowseFile, $btnViewFile, $btnDelFile, $imgFileLoading, $element_DisableOnStarted, sMsgError, nLimitSize) {

    var objUploadFunc = {
        Started: function (sender, args) {
            BlockUI();
            $txtFileName.prop('disabled', true);
            $btnBrowseFile.prop('disabled', true);
            var filename = args.get_fileName();
            if (filename != '') {
                var arr = filename.split("\\");
                var img = arr[arr.length - 1];
                var filext = (img.substring(img.lastIndexOf('.') + 1)).toLowerCase(); //File Extension
                var sFileName_New = FileName_SetMaxLength(img, filext, 100);
                $txtFileName.val(sFileName_New);
                $hidFileName.val(sFileName_New);
                // Checking Extension
                if ($.inArray(filext, arrAllowFile) > -1) {
                    $imgFileLoading.show('fast');
                    if ($element_DisableOnStarted != undefined) $element_DisableOnStarted.prop('disabled', true);
                    //Generate new FILENAME
                    var dNow = new Date();
                    var sYear = dNow.getFullYear().toString();
                    var sMonth = (dNow.getMonth() + 1).padLeft(2, '0').toString();
                    var sDay = dNow.getDate().padLeft(2, '0').toString();
                    var sHour = dNow.getHours().padLeft(2, '0').toString();
                    var sMinute = dNow.getMinutes().padLeft(2, '0').toString();
                    var sSecond = dNow.getSeconds().padLeft(2, '0').toString();
                    var sFileName = sYear + sMonth + sDay + sHour + sMinute + sSecond + '.' + filext;
                    $hidSysFileName.val(sFileName);
                }
                else {
                    var $element = $(sender._element); //jQuery Object (<span> ที่ถูกสร้างโดย Control โดยมี ElementID เป็นรหัสที่ลงท้ายด้วย ID ของ Control)
                    $element.find('input:file').val('');
                    //SwAlert.Error("เกิดข้อผิดพลาด", sMsgError);
                    DialogWarning(sMsgError)
                    UnblockUI();
                    return false;
                }
            }
            else return false;
        },
        Complete: function (sender, args) {
            var filesizeuploaded = parseInt(args.get_length());
            if (filesizeuploaded > nLimitSize) { //2097152 [2 MB] 1048576 [1 MB]
                $imgFileLoading.hide('fast');
                $afu.val('');
                $hidSysFileName.val('');
                $hidFilePath.val('');
                $hidFileName.val('');
                $btnBrowseFile.prop('disabled', false);
                $txtFileName.val('').prop('disabled', false);
                //SwAlert.Error("เกิดข้อผิดพลาด", sMsgError);
                DialogWarning(sMsgError)
                UnblockUI();
                return false;
            } else {
                $hidFilePath.val(sTempPath);
                $imgFileLoading.hide('fast');
                $btnViewFile.show('fast');
                $btnDelFile.show('fast');
                if ($element_DisableOnStarted != undefined) $element_DisableOnStarted.prop('disabled', false);
                $txtFileName.blur();
                UnblockUI();
                return true;
            }
        },
        Error: function (sender, args) {
            $imgFileLoading.hide('fast');
            $afu.val('');
            $hidSysFileName.val('');
            $hidFilePath.val('');
            $hidFileName.val('');
            $btnBrowseFile.prop('disabled', false);
            if ($element_DisableOnStarted != undefined) $element_DisableOnStarted.prop('disabled', false);
            $txtFileName.val('').prop('disabled', false);
            $txtFileName.blur();
            UnblockUI();
        }
    };

    $txtFileName
        .keydown(function () { return false; })
        .click(function () {
            if ($(this).is(':enabled')) $afu[0].click();
            return false;
        });

    $btnBrowseFile.click(function () {
        $txtFileName.click();
        return false;
    });

    $btnViewFile.click(function () {
        var sFileURL = $hidFilePath.val().replace('~/', '') + $hidSysFileName.val();
        //var $ancTemp = $('a#ancTemp');
        //$ancTemp.attr('href', sFileURL);
        //$ancTemp[0].click();
        FancyBox_ViewFile(sFileURL);
    });

    $btnDelFile.click(function () {
        var fnDeleteFile = function () {
            swal.disableButtons();
            BlockUI();
            var fnSuccess = function () {
                $btnBrowseFile.prop('disabled', false);
                $btnViewFile.prop('disabled', false).hide('fast');
                $btnDelFile.hide('fast');
                $afu.val('');
                $hidSysFileName.val('');
                $hidFilePath.val('');
                $hidFileName.val('');
                $txtFileName.val('').prop('disabled', false).blur();
            };
            var fnComplete = function () { swal.close(); UnblockUI(); };
            AjaxWebMethod('DeleteTempFile', { 'sFileName': $hidSysFileName.val() }, fnSuccess, fnComplete);
        };
        SwAlert.Confirm(AlertTitle.Confirm, AlertTitle.msgConfDelFile, fnDeleteFile);
    });

    if (!IsNullOrEmpty($hidSysFileName.val())) {
        $txtFileName.prop('disabled', true);
        $btnBrowseFile.prop('disabled', true);
        $btnViewFile.show();
        $btnDelFile.show();
    }

    return objUploadFunc;
}


function AsyncUploadFile_BindEventMulti(arrAllowFile, sTempPath, $afu, $txtFileName, $hidFileName, $hidSysFileName, $hidFilePath,
    $btnBrowseFile, $btnViewFile, $btnDelFile, $imgFileLoading, $element_DisableOnStarted, sMsgError, nLimitSize) {
    var objUploadFunc = {
        Started: function (sender, args) {
            BlockUI();
            //$txtFileName.prop('disabled', true);
            //$btnBrowseFile.prop('disabled', true);
            var filename = args.get_fileName();
            if (filename != '') {
                var arr = filename.split("\\");
                var img = arr[arr.length - 1];
                var filext = (img.substring(img.lastIndexOf('.') + 1)).toLowerCase(); //File Extension
                var sFileName_New = FileName_SetMaxLength(img, filext, 100);
                $txtFileName.val(sFileName_New);
                $hidFileName.val(sFileName_New);
                // Checking Extension
                if ($.inArray(filext, arrAllowFile) > -1) {
                    $imgFileLoading.show('fast');
                    if ($element_DisableOnStarted != undefined) $element_DisableOnStarted.prop('disabled', true);
                    //Generate new FILENAME
                    var dNow = new Date();
                    var sYear = dNow.getFullYear().toString();
                    var sMonth = (dNow.getMonth() + 1).padLeft(2, '0').toString();
                    var sDay = dNow.getDate().padLeft(2, '0').toString();
                    var sHour = dNow.getHours().padLeft(2, '0').toString();
                    var sMinute = dNow.getMinutes().padLeft(2, '0').toString();
                    var sSecond = dNow.getSeconds().padLeft(2, '0').toString();
                    var sFileName = sYear + sMonth + sDay + sHour + sMinute + sSecond + '.' + filext;
                    $hidSysFileName.val(sFileName);
                }
                else {
                    var $element = $(sender._element); //jQuery Object (<span> ที่ถูกสร้างโดย Control โดยมี ElementID เป็นรหัสที่ลงท้ายด้วย ID ของ Control)
                    $element.find('input:file').val('');
                    SwAlert.Error("เกิดข้อผิดพลาด", sMsgError);
                    UnblockUI();
                    return false;
                }
            }
            else return false;
        },
        Complete: function (sender, args) {
            var filesizeuploaded = parseInt(args.get_length());
            if (filesizeuploaded > nLimitSize) { //2097152 [2 MB] 1048576 [1 MB]
                $imgFileLoading.hide('fast');
                $afu.val('');
                $hidSysFileName.val('');
                $hidFilePath.val('');
                $hidFileName.val('');
                $btnBrowseFile.prop('disabled', false);
                $txtFileName.val('').prop('disabled', false);
                SwAlert.Error("เกิดข้อผิดพลาด", sMsgError);
                UnblockUI();
                return false;
            } else {
                $hidFilePath.val(sTempPath);
                $imgFileLoading.hide('fast');
                $btnViewFile.show('fast');
                $btnDelFile.show('fast');
                if ($element_DisableOnStarted != undefined) $element_DisableOnStarted.prop('disabled', false);
                $txtFileName.blur();
                //$("button[id$=btnAddFile]").click();
                AddFile();
                return true;
            }
        },
        Error: function (sender, args) {
            $imgFileLoading.hide('fast');
            $afu.val('');
            $hidSysFileName.val('');
            $hidFilePath.val('');
            $hidFileName.val('');
            $btnBrowseFile.prop('disabled', false);
            if ($element_DisableOnStarted != undefined) $element_DisableOnStarted.prop('disabled', false);
            $txtFileName.val('').prop('disabled', false);
            $txtFileName.blur();
            UnblockUI();
        }
    };

    $txtFileName
        .keydown(function () { return false; })
        .click(function () {
            if ($(this).is(':enabled')) $afu[0].click();
            return false;
        });

    $btnBrowseFile.click(function () {
        $txtFileName.click();
        return false;
    });

    $btnViewFile.click(function () {
        var sFileURL = $hidFilePath.val().replace('~/', '') + $hidSysFileName.val();
        //var $ancTemp = $('a#ancTemp');
        //$ancTemp.attr('href', sFileURL);
        //$ancTemp[0].click();
        FancyBox_ViewFile(sFileURL);
    });

    $btnDelFile.click(function () {
        var fnDeleteFile = function () {
            swal.disableButtons();
            BlockUI();
            var fnSuccess = function () {
                $btnBrowseFile.prop('disabled', false);
                $btnViewFile.prop('disabled', false).hide('fast');
                $btnDelFile.hide('fast');
                $afu.val('');
                $hidSysFileName.val('');
                $hidFilePath.val('');
                $hidFileName.val('');
                $txtFileName.val('').prop('disabled', false).blur();
            };
            var fnComplete = function () { swal.close(); UnblockUI(); };
            AjaxWebMethod('DeleteTempFile', { 'sFileName': $hidSysFileName.val() }, fnSuccess, fnComplete);
        };
        SwAlert.Confirm(AlertTitle.Confirm, AlertTitle.msgConfDelFile, fnDeleteFile);
    });

    if (!IsNullOrEmpty($hidSysFileName.val())) {
        $txtFileName.prop('disabled', true);
        $btnBrowseFile.prop('disabled', true);
        $btnViewFile.show();
        $btnDelFile.show();
    }

    return objUploadFunc;
}

//DatePicker
function SetDatePicker($txtDate) {
    $txtDate
        .datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true
        })
        .keydown(function (e) {
            if ($(this).is('[readonly]')) {
                if (e.which == 8 || e.which == 46) {
                    $(this).val('').change();
                    return false;
                }
            }
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
}

function SetDatePickerEndNow($txtDate) {
    $txtDate
        .datepicker({
            maxDate: 'now',
            format: 'dd/mm/yyyy',
            autoclose: true
        })
        .keydown(function (e) {
            if ($(this).is('[readonly]')) {
                if (e.which == 8 || e.which == 46) {
                    $(this).val('').change();
                    return false;
                }
            }
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
}

function SetDatePickerValidate($divValidate, $txtDate) {
    $txtDate
        .datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true
        })
        .change(function () {
            ReValidateFieldControl($divValidate, $txtDate.attr('name'));
        })
        .keydown(function (e) {
            if ($(this).is('[readonly]')) {
                if (e.which == 8 || e.which == 46) {
                    $(this).val('').change();
                    return false;
                }
            }
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
}

function SetDateRangePicker($txtDateBEGIN, $txtDateEND) {
    if ($txtDateBEGIN != "" && $txtDateEND != "") {
        SetDatePicker($txtDateBEGIN);
        SetDatePicker($txtDateEND);

        $txtDateBEGIN
            .change(function () {
                var thisVal = $(this).val();
                if (thisVal != '') $txtDateEND.datepicker('setStartDate', thisVal);
                else SetDatePicker($txtDateEND.datepicker('remove'));
            })
            .keydown(function (e) {
                //if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                //    return false;
                //}
            });

        $txtDateEND
            .change(function () {
                var thisVal = $(this).val();
                if (thisVal != '') $txtDateBEGIN.datepicker('setEndDate', thisVal);
                else SetDatePicker($txtDateBEGIN.datepicker('remove'));
            })
            .keydown(function (e) {
                //if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                //    return false;
                //}
            });
    } else if ($txtDateEND == "") {
        SetDatePicker($txtDateBEGIN);

        $txtDateBEGIN
            .keydown(function (e) {
                if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                    return false;
                }
            });
    }
}

function SetDateRangePicker_Edit($txtDateStart, $txtDateEnd, Val_Start, Val_End) {
    if (Val_Start) {
        $txtDateStart.datepicker('update', Val_Start);
        $txtDateStart.datepicker('setEndDate', Val_End);
    }
    else {
        $txtDateStart.datepicker("remove");
        SetDatePicker($txtDateStart);
    }

    if (Val_End) {
        $txtDateEnd.datepicker('update', Val_End);
        $txtDateEnd.datepicker('setStartDate', Val_Start);
    }
    else {
        $txtDateEnd.datepicker("remove");
        SetDatePicker($txtDateEnd);
    }
}

function SetMonthYearPicker($txtDate) {
    $txtDate
        .datepicker({
            format: 'mm/yyyy',
            minViewMode: "months",
            autoclose: true
        })
        .keydown(function (e) {
            if ($(this).is('[readonly]')) {
                if (e.which == 8 || e.which == 46) {
                    $(this).val('').change();
                    return false;
                }
            }
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
}

function SetMonthYearPicker_($txtDate) {
    $txtDate
        .datepicker({
            format: 'mm/yyyy',
            minViewMode: "months",
            autoclose: true
        })
        .keydown(function (e) {
            if ($(this).is('[readonly]')) {
                if (e.which == 8 || e.which == 46) {
                    $(this).val('').change();
                    return false;
                }
            }
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
}

function SetMonthYearPickerValidate($divValidate, $txtDate) {
    $txtDate
        .datepicker({
            format: 'mm/yyyy',
            minViewMode: "months",
            autoclose: true
        })
        .keydown(function (e) {
            if ($(this).is('[readonly]')) {
                if (e.which == 8 || e.which == 46) {
                    $(this).val('').change();
                    return false;
                }
            }
        })
        .change(function () {
            ReValidateFieldControl($divValidate, $txtDate.attr('name'));
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
}

function SetYearRangePicker($txtDateBEGIN, $txtDateEND) {
    if ($txtDateBEGIN != "" && $txtDateEND != "") {
        SetYearPicker($txtDateBEGIN);
        SetYearPicker($txtDateEND);

        $txtDateBEGIN
            .change(function () {
                var thisVal = $(this).val();
                if (thisVal != '') $txtDateEND.datepicker('setStartDate', thisVal);
                else SetYearPicker($txtDateEND.datepicker('remove'));

                $txtDateEND.focus();
            })
            .keydown(function (e) {
                //if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                //    return false;
                //}
            });

        $txtDateEND
            .change(function () {
                var thisVal = $(this).val();
                if (thisVal != '') $txtDateBEGIN.datepicker('setEndDate', thisVal);
                else SetYearPicker($txtDateBEGIN.datepicker('remove'));
            })
            .keydown(function (e) {
                //if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                //    return false;
                //}
            });
    } else if ($txtDateEND == "") {
        SetYearPicker($txtDateBEGIN);

        $txtDateBEGIN
            .keydown(function (e) {
                if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                    return false;
                }
            });
    }
}

function SetYearPicker($txtDate) {
    $txtDate
        .datepicker({
            format: 'yyyy',
            minViewMode: "years",
            autoclose: true
        })
        .keydown(function (e) {
            if ($(this).is('[readonly]')) {
                if (e.which == 8 || e.which == 46) {
                    $(this).val('').change();
                    return false;
                }
            }
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
}

function SetYearPickerValidate($divValidate, $txtDate) {
    $txtDate
        .datepicker({
            format: 'yyyy',
            minViewMode: "years",
            autoclose: true
        })
        .keydown(function (e) {
            if ($(this).is('[readonly]')) {
                if (e.which == 8 || e.which == 46) {
                    $(this).val('').change();
                    return false;
                }
            }
        })
        .change(function () {
            ReValidateFieldControl($divValidate, $txtDate.attr('name'));
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
}

function SetMonthYearRangePicker($txtDateBEGIN, $txtDateEND) {
    if ($txtDateBEGIN != "" && $txtDateEND != "") {
        SetMonthYearPicker_($txtDateBEGIN);
        SetMonthYearPicker_($txtDateEND);

        $txtDateBEGIN
            .change(function () {
                var thisVal = $(this).val();
                if (thisVal != '') $txtDateEND.datepicker('setStartDate', thisVal);
                else SetDatePicker($txtDateEND.datepicker('remove'));
            })
            .keydown(function (e) {
                //if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                //    return false;
                //}
            });

        $txtDateEND
            .change(function () {
                var thisVal = $(this).val();
                if (thisVal != '') $txtDateBEGIN.datepicker('setEndDate', thisVal);
                else SetDatePicker($txtDateBEGIN.datepicker('remove'));
            })
            .keydown(function (e) {
                //if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                //    return false;
                //}
            });
    } else if ($txtDateEND == "") {
        SetDatePicker($txtDateBEGIN);

        $txtDateBEGIN
            .keydown(function (e) {
                if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                    return false;
                }
            });
    }
}

function SetMonthYearRangePickerValidate($divID, $txtDateBEGIN, $txtDateEND) {
    if ($txtDateBEGIN != "" && $txtDateEND != "") {
        SetMonthYearPicker_($txtDateBEGIN);
        SetMonthYearPicker_($txtDateEND);

        $txtDateBEGIN
            .on('change', function () {
                var thisVal = $(this).val();
                if (thisVal != '') $txtDateEND.datepicker('setStartDate', thisVal);
                else { SetMonthYearPicker_($txtDateEND.datepicker('setStartDate', '')); }
                ReValidateFieldControl($divID, $txtDateBEGIN.attr('name'));
                ReValidateFieldControl($divID, $txtDateEND.attr('name'));
            });

        $txtDateEND
            .on('change', function () {
                var thisVal = $(this).val();
                if (thisVal != '') $txtDateBEGIN.datepicker('setEndDate', thisVal);
                else { SetMonthYearPicker_($txtDateBEGIN.datepicker('setEndDate', '')); }
                ReValidateFieldControl($divID, $txtDateBEGIN.attr('name'));
                ReValidateFieldControl($divID, $txtDateEND.attr('name'));
            });
    } else if ($txtDateEND == "") {
        SetMonthYearPicker_($txtDateBEGIN);
    }
}

function SetClockpicker($txtTime, DivrevalidateField) {
    $txtTime
        .clockpicker({
            autoclose: true
        }).on('change', function (e) {
            // Revalidate the date field
            $('#' + DivrevalidateField + '').formValidation('revalidateField', $txtTime.attr("name"));
        });
}

function SetDateJson(sDate) {
    var myDate = new Date(parseJsonDate(sDate));
    return Padleft(myDate.getDate() + "", "00") + '/' + Padleft((myDate.getMonth() + 1) + "", "00") + '/' + (myDate.getFullYear() + 543);
}

function SetDateJsonEn(sDate) {
    var myDate = new Date(parseJsonDate(sDate));
    return Padleft(myDate.getDate() + "", "00") + '/' + Padleft((myDate.getMonth() + 1) + "", "00") + '/' + (myDate.getFullYear());
}

function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}

function Padleft(str, pad) {
    var ans = pad.substring(0, pad.length - str.length) + str;
    return ans;
}

function SetTxtNumber($txt) {
    //เหตุผลที่ keypress คือพิมพ์ไทยติดเข้ามาได้ เลยต้องใช้ keypress
    $txt.keypress(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl/cmd+A
            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl/cmd+C
            (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl/cmd+X
            (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    //เหตุผลที่ keydown คือพิมพ์ En ติดเข้ามาได้บางตัว เลยต้องใช้ keydown
    $txt.keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl/cmd+A
            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl/cmd+C
            (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl/cmd+X
            (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
}

function SetTxtNumber_INT($txt) {
    //เหตุผลที่ keypress คือพิมพ์ไทยติดเข้ามาได้ เลยต้องใช้ keypress
    $txt.keypress(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl/cmd+A
            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl/cmd+C
            (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl/cmd+X
            (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }

        var index = $txt.val().indexOf('.');
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    //เหตุผลที่ keydown คือพิมพ์ En ติดเข้ามาได้บางตัว เลยต้องใช้ keydown
    $txt.keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter
        // Not Allow : dot(.)[keyCode : 110, 190]
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
        //else {
        //    // Not Allow key 0 at the first char
        //    if ($(this).val().length == 0 && (e.keyCode == 48 || e.keyCode == 96)) e.preventDefault();
        //}
    });
}

function GetValueMultiSelect(ID) {
    var sValue = "";
    var chkArray = $('select[id$=' + ID + ']').val();
    if (typeof chkArray != "string") {
        var arrDocNo = [];

        if (chkArray) {
            sValue = chkArray.join(',');
        }
    }
    else {
        sValue = $('select[id$=' + ID + ']').val();
    }

    return sValue;
}

function MultiSelectForEdit(ID, sVal) {
    var Sourcedata = [];

    if ((sVal + "") != "null") {
        if (sVal.length > 0) {
            Sourcedata = sVal.split(",");

            $('select[id$=' + ID + ']').val(Sourcedata);
            // Then refresh
            $('select[id$=' + ID + ']').multiselect("refresh");
        }
        else {
            $('select[id$=' + ID + ']').multiselect("clearSelection");
        }
    }
    else {
        $('select[id$=' + ID + ']').multiselect("clearSelection");
    }
}

function checkPhoneNum(str) {
    //var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    // correct format be like 999-999-999 ต่อ 99
    var phoneNum = /^[0-9\(\) #\,\-\.ต่อ]*$/;
    var arr = new Array;
    arr = str.val().split(",");
    for (var i = 0; i < arr.length; i++) {
        var bb = arr[i];
        if (!(phoneNum.test(bb))) {
            return false;
        }
    }
    return true;
}

/************Get Data *************/
function GetValTextBox(txtID) {
    return $("input[id$=" + txtID + "]").val();
}
function GetValDropdown(ddlID) {
    return $("select[id$=" + ddlID + "]").val();
}
function GetValRadioList(rblID) {
    return $("input[id*=" + rblID + "]:checked").val();
}
function GetValCheckBox(cbID) {
    return $("input[id*=" + cbID + "]").is(':checked');
}
function GetValTextArea(txtID) {
    return $("textarea[id$=" + txtID + "]").val();
}
function GetValueAttrValue(txtID) {
    return $("input[id$=" + txtID + "]").attr("value");
}
function GetValueInputMask(txtID) {
    return $("input[id$=" + txtID + "]").inputmask('unmaskedvalue');
}

function GetValRadioListNotValidate(name) {
    var sVal = $('input[name*=' + name + ']:checked').val();
    if (typeof sVal !== "undefined") {
        return sVal;
    }
    else {
        return "";
    }
}

function GetValRadioListInDiv(divID, name) {
    var sVal = $('' + divID + ' input[name*=' + name + ']:checked').val();
    if (typeof sVal !== "undefined") {
        return sVal;
    }
    else {
        return "";
    }
}

function GetValCheckboxList(sName) {
    var arrRet = [];
    $('input[name*=' + sName + ']').each(function (i, el) {
        if ($(this).is(':checked')) {
            arrRet.push($(this).val())
        }
    });
    return arrRet;
}

function IsBrowserFirefox() {
    var mybrowser = navigator.userAgent;
    if (mybrowser.indexOf('Firefox') >= 0 || mybrowser.indexOf('Mozilla') >= 0 || mybrowser.indexOf('Chrome') >= 0) {
        return true;
    }
    else
        return false;
}

function BackToList(sPath) {
    return window.location.href = (sPath) ? sPath : "default.aspx";
}

function SetScrollTopValidate() {
    return $('html, body').animate({ scrollTop: $('.has-error').offset().top }, 'fast');
}

//integerDigits : จำนวนหลักของของค่าจำนวนเต็ม,digits : จำนวนหลักของค่าทศนิยม
function InputMaskDecimal(objCtrl, integerDigits, digits, allowPlus, allowMinus) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: digits, //จำนวนหลักของค่าทศนิยม
        radixPoint: '.', //จุดทศนิยม
        groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
        allowPlus: Boolean(allowPlus), //อนุญาตใส่เครื่องหมายบวก
        allowMinus: Boolean(allowMinus), //อนุญาตใส่เครื่องหมายลบ,
        rightAlign: false,
    });
}
//integerDigits : จำนวนหลักของของค่าจำนวนเต็ม,digits : จำนวนหลักของค่าทศนิยม
function InputMaskDecimal_Align(objCtrl, integerDigits, digits, allowPlus, allowMinus, rightAlign) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: digits, //จำนวนหลักของค่าทศนิยม
        radixPoint: '.', //จุดทศนิยม
        groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
        allowPlus: Boolean(allowPlus), //อนุญาตใส่เครื่องหมายบวก
        allowMinus: Boolean(allowMinus), //อนุญาตใส่เครื่องหมายลบ,
        rightAlign: rightAlign,
    });
}

//integerDigits : จำนวนหลักของของค่าจำนวนเต็ม,digits : จำนวนหลักของค่าทศนิยม
function InputMaskDecimalMinMax(objCtrl, integerDigits, digits, allowPlus, allowMinus, Min, Max) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: digits, //จำนวนหลักของค่าทศนิยม
        radixPoint: '.', //จุดทศนิยม
        groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
        allowPlus: Boolean(allowPlus), //อนุญาตใส่เครื่องหมายบวก
        allowMinus: Boolean(allowMinus), //อนุญาตใส่เครื่องหมายลบ,
        rightAlign: false,
        min: Min,
        max: Max,
    });
}

//integerDigits : จำนวนหลักของของค่าจำนวนเต็ม,digits : จำนวนหลักของค่าทศนิยม
function InputMaskDecimalMinMax_Align(objCtrl, integerDigits, digits, allowPlus, allowMinus, Min, Max, rightAlign) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: digits, //จำนวนหลักของค่าทศนิยม
        radixPoint: '.', //จุดทศนิยม
        groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
        allowPlus: Boolean(allowPlus), //อนุญาตใส่เครื่องหมายบวก
        allowMinus: Boolean(allowMinus), //อนุญาตใส่เครื่องหมายลบ,
        rightAlign: rightAlign,
        min: Min,
        max: Max,
    });
}

function InputMaskIntegerNotMoney(objCtrl, integerDigits) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: 0, //จำนวนหลักของค่าทศนิยม
        groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
        allowPlus: false, //อนุญาตใส่เครื่องหมายบวก
        allowMinus: false, //อนุญาตใส่เครื่องหมายลบ,    
        rightAlign: false,
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
    });
}

function InputMaskInteger(objCtrl, integerDigits) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: 0, //จำนวนหลักของค่าทศนิยม
        allowPlus: false, //อนุญาตใส่เครื่องหมายบวก
        allowMinus: false, //อนุญาตใส่เครื่องหมายลบ,    
        rightAlign: false,
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
    });
}

function InputMaskIntegerMinMax(objCtrl, integerDigits, Min, Max, rightAlign) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: 0, //จำนวนหลักของค่าทศนิยม
        radixPoint: '.', //จุดทศนิยม
        groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
        allowPlus: false, //อนุญาตใส่เครื่องหมายบวก
        allowMinus: false, //อนุญาตใส่เครื่องหมายลบ,    
        rightAlign: (rightAlign != undefined && rightAlign != null ? rightAlign : false),
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
        min: Min,
        max: Max,
    });
}


Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
}

Date.prototype.addMonths = function (value) {
    var n = this.getDate();
    this.setDate(1);
    this.setMonth(this.getMonth() + value);
    this.setDate(Math.min(n, this.getDaysInMonth()));
    return this;
};

Date.isLeapYear = function (year) {
    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
};

Date.getDaysInMonth = function (year, month) {
    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
};

Date.prototype.isLeapYear = function () {
    return Date.isLeapYear(this.getFullYear());
};

Date.prototype.getDaysInMonth = function () {
    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
};

function SetDateToString(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() == 0 ? 12 : d.getMonth()),
        day = '' + d.getDate(),
        year = (d.getMonth() == 0 ? d.getFullYear() - 1 : d.getFullYear());

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join('/');
}

function SetDateToStringPlusM1(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join('/');
}

function GetActionName(sAction) {
    var sRet = "";
    if (sAction == "D") { sRet = "Save Draft"; }
    else if (sAction == "R") { sRet = "Revisit"; }
    else if (sAction == "S") { sRet = "Submit"; }
    else if (sAction == "A") { sRet = "Approve"; }
    return sRet;
}

function DateForSort(sValue, sDelimiter) {
    var arrVal = sValue.split(sDelimiter);
    var sDate = "";
    if (arrVal.length == 2) {
        sDate = arrVal[1] + arrVal[0];
    } else if (arrVal.length == 3) {
        sDate = arrVal[2] + arrVal[1] + arrVal[0];
    } else if (arrVal.length == 6) {
        sDate = arrVal[2] + arrVal[1] + arrVal[0] + arrVal[5] + arrVal[4] + arrVal[3];
    }
    return sDate;
}

function BindSelectpicker() {
    $('.selectpicker').selectpicker('toggle');
    ScrollTopToElements("BODY");
}

function AshxCall_EncryptParameter(sParameter, fnSuccess) {
    BlockUI();
    AshxCall('Ashx/EncryptParameter.ashx', { 'str': sParameter }, (typeof fnSuccess == 'function' ? fnSuccess : null), UnblockUI);
}
function AshxCall(sAshxPath, objData, fnSuccess, fnComplete) {
    /// <return type="Void"></return>
    $.ajax({
        type: 'GET',
        url: sAshxPath,
        cache: false,
        dataType: 'html',
        data: objData,
        success: fnSuccess != undefined ? fnSuccess : function () { },
        complete: fnComplete != undefined ? fnComplete : function () { }
    });
}