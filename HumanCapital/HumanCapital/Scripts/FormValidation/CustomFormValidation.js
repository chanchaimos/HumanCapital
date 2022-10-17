/// <reference path="../System/SystemFunction.js" />

//#region Form Validation

function BindValidate(sContainer, objValidate) {

    $("#" + sContainer).formValidation({
        framework: 'bootstrap',
        excluded: [':disabled', ':hidden', ':not(:visible)'],//':disabled',
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

function CheckValidate(sContainer) {

    var isValid = $("#" + sContainer).data('formValidation').validate().isValid();
    return isValid;
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

function addValidate_rblICheckNotEmpty(msg) {
    return {
        validators: {
            choice: {
                min: 1,
                message: msg
            }
        }
    };
}

function addValidate_date(msg) {
    return {
        validators: {
            notEmpty: {
                message: 'The date is required'
            },
            date: {
                format: 'DD/MM/YYYY',
                message: 'The date is not valid'
            }
        }
    };

}

function addValidate_Length(msg, maxLength) {
    return {
        validators: {
            stringLength: {
                max: maxLength,
                message: 'Post content must be less than ' + maxLength + ' characters'//msgmaxLength
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
function addValidate_NumberINT(msg, maxLength, msgmaxLength) {
    return {
        validators: {
            regexp: {
                regexp: "(?=[^\0])(?=^([0-9]+){0,1}(\[0-9]{1,5}){0,1}$)", //Check ว่าสามารถกรอกตัวเลข หรือ ทศนิยมได้เท่านั้น ทศนิยม 5 ตำแหน่ง
                message: "กรอกข้อมูล ตัวเลข หรือ ทศนิยม  เท่านั้น"
            },
            notEmpty: {
                message: msg
            },
            stringLength: {
                max: maxLength,
                message: msgmaxLength
            },
            callback: {
                message: msg != undefined && msg != null ? msg : "Please Specify Number",
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                },

            }
        }
    };
}
function addValidate_Number(msg, maxLength, msgmaxLength) {
    return {
        validators: {
            regexp: {
                regexp: "(?=[^\0])(?=^([0-9]+){0,1}(\.[0-9]{1,5}){0,1}$)", //Check ว่าสามารถกรอกตัวเลข หรือ ทศนิยมได้เท่านั้น ทศนิยม 5 ตำแหน่ง
                message: "กรอกข้อมูล ตัวเลข หรือ ทศนิยม  เท่านั้น"
            },
            notEmpty: {
                message: msg
            },
            stringLength: {
                max: maxLength,
                message: msgmaxLength
            },
            callback: {
                message: msg != undefined && msg != null ? msg : "Please Specify Number",
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                },

            }
        }
    };
}
function addValidateEmail_notEmpty(msg, maxLength, msgmaxLength) {

    var MSG_Length = isEmpty(msgmaxLength) ? 'Post content must be less than ' + maxLength + ' characters' : msgmaxLength;

    return {
        validators: {
            regexp: {
                regexp: "^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$",
                message: "Invalid email format"
            },
            stringLength: {
                max: maxLength,
                message: MSG_Length//msgmaxLength
            },
            callback: {
                message: msg != undefined && msg != null ? msg : "Please Specify Email",
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                }
            }
        }
    };
}

function GetElementNameICheck(sElement) {
    return $("input[name$=" + sElement + "").attr("name");
}

function GetElementName(sElement, objType) {

    return $(objType + "[id$=" + sElement + "]").attr("name");
}

function GetElementName2(sElement, objType) {
    return $(objType + "[name$=" + sElement + "]").attr("name");
}

function funcValidationUpdate(cType, txtID) {
    $('#divFormValidate').formValidation('revalidateField', $('' + cType + '[id$=' + txtID + ']'));
}

function ValidationUpdate($txt) {
    $('#divFormValidate').formValidation('revalidateField', $txt);
}

function funcClareValidation(cType, txtID) {
    $('#divFormValidate').formValidation('updateStatus', $('' + cType + '[id$=' + txtID + ']'), 'NOT_VALIDATED');
    $('' + cType + '[id$=' + txtID + ']').val('');
}

function ClareValidation($txt) {
    $('#divFormValidate').formValidation('updateStatus', $txt, 'NOT_VALIDATED');
    $txt.val('');
}

function funcClareValidationAll($divContainer,$divFormCtrl) {
    $divFormCtrl.map(function () {
        $divContainer.formValidation('updateStatus', $(this), 'NOT_VALIDATED');
    });
}

function funcValidationUpdateAll($divContainer, $divFormCtrl) {
    $divFormCtrl.map(function () {
        $divContainer.formValidation('revalidateField', $(this));
    });
}

function addValidate_notEmptyTH(msg, maxLength) {
    return {
        validators: {
            regexp: {
                
                regexp: "^([^a-zA-Z0-9_]*)$",
                message: "Invalid Number and English Characters"
            },
            notEmpty: {
                message: msg
            },
            stringLength: {
                max: maxLength,
                message: 'Post content must be less than ' + maxLength + ' characters'//msgmaxLength
            }
        }
    };
}

function addValidate_notEmptyEN(msg, maxLength) {
    return {
        validators: {
            regexp: {

                regexp: "^([^ก-ฮ0-9_]*)$",
                message: "Invalid Number and Thai Characters"
            },
            notEmpty: {
                message: msg
            },
            stringLength: {
                max: maxLength,
                message: 'Post content must be less than ' + maxLength + ' characters'//msgmaxLength
            }
        }
    };
}

var objControl = {
    txtbox: "input",
    txtarea: "textarea",
    dropdown: "select",
    div: "div",
    span: "span",
    rblICheck: "input",
    btn: "input",
    radio: "radio"
};

//#endregion

function objValidate(page) {

    var objValidate = {};

    switch (page) {

        //#region register

        case 'register':

            var isDataPrefix = function (msg) {

                return {
                    validators: {
                        callback: {
                            message: "Please Specify",//msg != undefined && msg != null ? msg : "Please Specify",
                            callback: function (value, validator, $field) {

                                var thisData = GetValDropDown('ddlPrefix');

                                if (thisData == '9') {
                                    return !(value + "" == "" || value == null || value == undefined);
                                } else return true;

                            }
                        }
                    }
                };

            };
            var isDataFile = function (msg, field) {

                return {
                    validators: {
                        callback: {
                            message: "Please Specify",//msg != undefined && msg != null ? msg : "Please Specify",
                            callback: function (value, validator, $field) {

                                var $type = $('select[id$=ddlTypeArt' + field + ']'),
                                    $name = $('input[id$=txtWorkname' + field + ']'),
                                    $tec = $('input[id$=txtTechnic' + field + ']'),
                                    $photo = $('input[id$=txtPhotoFileName' + field + ']');

                                if (isEmpty($type.val()) && isEmpty($name.val()) && isEmpty($tec.val()) && isEmpty($photo.val())) {
                                    ClareValidation($type);
                                    ClareValidation($name);
                                    ClareValidation($tec);
                                    ClareValidation($photo);
                                    return true;
                                } else {
                                    return !(value + "" == "" || value == null || value == undefined);
                                }


                            }
                        }
                    }
                };

            };
            var isDataNameC = function (msg) {

                return {
                    validators: {
                        regexp: {
                            regexp: "^([^0-9]*)$",
                            message: "Invalid Number"
                        },
                        notEmpty: {
                            message: msg
                        }
                    }
                };

            };

            objValidate[GetElementName("ddlZone", objControl.dropdown)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("ddlAwardName", objControl.dropdown)] = addValidate_notEmpty(" ");
            // objValidate[GetElementName2("userdetails", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("ddlPrefix", objControl.dropdown)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtPrefix", objControl.txtbox)] = isDataPrefix(" ");
            objValidate[GetElementName("txtNameTH", objControl.txtbox)] = isDataNameC(" ");
            objValidate[GetElementName("txtSurNameTH", objControl.txtbox)] = isDataNameC(" ");
            objValidate[GetElementName("txtNameEN", objControl.txtbox)] = isDataNameC(" ");
            objValidate[GetElementName("txtSurNameEN", objControl.txtbox)] = isDataNameC(" ");
            objValidate[GetElementName("txtBirthday", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtAddress", objControl.txtarea)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtPhone", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtEMail", objControl.txtbox)] = addValidateEmail_notEmpty(" ", 100, " ");
            objValidate[GetElementName("txtNameof", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtEducation", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtAddress2", objControl.txtarea)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtsPhone", objControl.txtarea)] = addValidate_notEmpty(" ");


            objValidate[GetElementName("ddlTypeArt", objControl.dropdown)] = isDataFile(" ", "");
            objValidate[GetElementName("txtWorkname", objControl.txtbox)] = isDataFile(" ", "");
            objValidate[GetElementName("txtsIdeas", objControl.txtbox)] = isDataFile(" ", "");
            objValidate[GetElementName("txtTechnic", objControl.txtbox)] = isDataFile(" ", "");
            objValidate[GetElementName("txtPhotoFileName", objControl.txtbox)] = isDataFile(" ", "");

            objValidate[GetElementName("ddlTypeArt2", objControl.dropdown)] = isDataFile(" ", "2");
            objValidate[GetElementName("txtWorkname2", objControl.txtbox)] = isDataFile(" ", "2");
            objValidate[GetElementName("txtsIdeas2", objControl.txtbox)] = isDataFile(" ", "2");
            objValidate[GetElementName("txtTechnic2", objControl.txtbox)] = isDataFile(" ", "2");
            objValidate[GetElementName("txtPhotoFileName2", objControl.txtbox)] = isDataFile(" ", "2");

            objValidate[GetElementName("ddlTypeArt3", objControl.dropdown)] = isDataFile(" ", "3");
            objValidate[GetElementName("txtWorkname3", objControl.txtbox)] = isDataFile(" ", "3");
            objValidate[GetElementName("txtsIdeas3", objControl.txtbox)] = isDataFile(" ", "3");
            objValidate[GetElementName("txtTechnic3", objControl.txtbox)] = isDataFile(" ", "3");
            objValidate[GetElementName("txtPhotoFileName3", objControl.txtbox)] = isDataFile(" ", "3");


            $('input[id$=txtBirthday]').change(function () { ValidationUpdate($(this)); });

            objValidate[GetElementName("txtTeamName", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtPersonelCode", objControl.txtbox)] = addValidate_notEmpty(" ");
            break;

            //#endregion


            //#region userpermission

        case 'userpermission':

            var isDataUser = function (msg, field) {

                return {
                    validators: {
                        regexp: {
                            regexp: "^([^ก-๙]*)$",
                            message: "Invalid Thai Charactor"
                        },
                        callback: {
                            message: "Please Specify",//msg != undefined && msg != null ? msg : "Please Specify",
                            callback: function (value, validator, $field) {

                                var thisData = $('input[name$=rblcType]:checked').val();
                                if (field == 1) {
                                    if (thisData == "2" || thisData == "3") {
                                        return !(value + "" == "" || value == null || value == undefined);
                                    } else return true;
                                } else if (field == 2) {
                                    if (thisData == "2") {
                                        return !(value + "" == "" || value == null || value == undefined);
                                    } else return true;
                                } else if (field == 3) {
                                    if (thisData == "3") {
                                        return !(value + "" == "" || value == null || value == undefined);
                                    } else return true;
                                }
                            }
                        }
                    }
                };

            };

            var isDataPass = function (msg, field) {

                return {
                    validators: {
                        regexp: {
                            regexp: "^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.*[!@#\$%\^&\*])(?=.{8,})",
                            message: "Invalid Password format"
                        },
                        callback: {
                            message: "Please Specify",//msg != undefined && msg != null ? msg : "Please Specify",
                            callback: function (value, validator, $field) {

                                var thisData = $('input[name$=rblcType]:checked').val();
                                if (field == 1) {
                                    if (thisData == "2" || thisData == "3") {
                                        return !(value + "" == "" || value == null || value == undefined);
                                    } else return true;
                                } else if (field == 2) {
                                    if (thisData == "2") {
                                        return !(value + "" == "" || value == null || value == undefined);
                                    } else return true;
                                } else if (field == 3) {
                                    if (thisData == "3") {
                                        return !(value + "" == "" || value == null || value == undefined);
                                    } else return true;
                                }
                            }
                        }
                    }
                };

            };

            var isDataName = function (msg, field) {

                return {
                    validators: {
                        regexp: {
                            regexp: "^([^0-9]*)$",
                            message: "Invalid Number"
                        },
                        callback: {
                            message: "Please Specify",//msg != undefined && msg != null ? msg : "Please Specify",
                            callback: function (value, validator, $field) {

                                var thisData = $('input[name$=rblcType]:checked').val();
                                if (field == 1) {
                                    if (thisData == "2" || thisData == "3") {
                                        return !(value + "" == "" || value == null || value == undefined);
                                    } else return true;
                                } else if (field == 2) {
                                    if (thisData == "2") {
                                        return !(value + "" == "" || value == null || value == undefined);
                                    } else return true;
                                } else if (field == 3) {
                                    if (thisData == "3") {
                                        return !(value + "" == "" || value == null || value == undefined);
                                    } else return true;
                                }
                            }
                        }
                    }
                };

            };

            var isDataEmail = function () {
                return {
                    validators: {
                        regexp: {
                            regexp: "^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$",
                            message: "Invalid email format"
                        },
                        callback: {
                            message: "Please Specify",
                            callback: function (value, validator, $field) {
                                var thisData = $('input[name$=rblcType]:checked').val();

                                if (thisData == "2" || thisData == "3") {
                                    return !(value + "" == "" || value == null || value == undefined);
                                } else return true;
                            }
                        }
                    }
                };
            };

            objValidate[GetElementName("txtName", objControl.txtbox)] = isDataName(" ", 1);
            objValidate[GetElementName("txtUsername", objControl.txtbox)] = isDataUser(" ", 1);
            objValidate[GetElementName("txtPassword", objControl.txtbox)] = isDataPass(" ", 2);
            objValidate[GetElementName("txtUnit", objControl.txtbox)] = isDataUser(" ", 1);
            objValidate[GetElementName("txtPhone", objControl.txtbox)] = isDataUser(" ", 1);
            objValidate[GetElementName("txtEmail", objControl.txtbox)] = isDataEmail();

            objValidate[GetElementName("txtPTT", objControl.txtbox)] = isDataUser(" ", 2);

            break;

            //#endregion

            //#region faq

        case 'faq':


            objValidate[GetElementName("txtQuestion", objControl.txtarea)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtAnswer", objControl.txtarea)] = addValidate_notEmpty(" ");

            break;



            //#endregion

            //#region award

        case 'award':

            objValidate[GetElementName("txtsAward", objControl.txtbox)] = addValidate_notEmpty(" ");

            break;

            //#endregion

            //#region calendar

        case 'calendar':
            objValidate[GetElementName("txtYear", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtTitle", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtDetail", objControl.txtarea)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtDate", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("ddlZone", objControl.dropdown)] = addValidate_notEmpty(" ");


            break;
            //#endregion

            //#region gallery

        case 'gallery':

            var isDataName = function (msg) {

                return {
                    validators: {
                        regexp: {
                            regexp: "^([^0-9]*)$",
                            message: "Invalid Number"
                        },
                        callback: {
                            message: msg,//msg != undefined && msg != null ? msg : "Please Specify",
                            callback: function (value, validator, $field) {
                                return !(value + "" == "" || value == null || value == undefined);
                            }
                        }
                    }
                };

            };

            objValidate[GetElementName("ddlYear", objControl.dropdown)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("ddlAward", objControl.dropdown)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtPhotoFileName", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("sFullName", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtNameTH", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtNameEN", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtsIdeas", objControl.txtarea)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtTrick", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtSize", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtEntryregister", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("ddlCategory", objControl.dropdown)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("ddlTypeAward", objControl.dropdown)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtProperty", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtLocation", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtCurator", objControl.txtbox)] = isDataName(" ");



            break;


            //#endregion

            //#region about

        case 'about':

            // objValidate[GetElementName("txtDetail", objControl.txtarea)] = addValidate_notEmpty(" ");

            break;

            //#endregion

            //#region banner

        case 'banner':

            var isDataBanner = function (msg, field) {

                return {
                    validators: {
                        callback: {
                            message: " ",//msg != undefined && msg != null ? msg : "Please Specify",
                            callback: function (value, validator, $field) {

                                var thisData = $('input[id$=ckbforever]').is(':checked');
                                if (!thisData) {
                                    return !(value + "" == "" || value == null || value == undefined);
                                } else return true
                            }
                        }
                    }
                };

            };

            objValidate[GetElementName("txtName", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtPhotoFileName", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtDate", objControl.txtbox)] = isDataBanner(" ");

            $('input[id$=txtDate]').change(function () {
                funcValidationUpdate('input', 'txtDate');
            });

            $('input[id$=ckbforever]').change(function () {

                if ($(this).is(':checked')) {
                    funcClareValidation('input', 'txtDate');
                } else funcValidationUpdate('input', 'txtDate');
            });


            break;

            //#endregion

            //#region contest

        case 'contest':

            objValidate[GetElementName("txtContestName", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtsDate", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtsContestPlace", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtTitleContest", objControl.txtarea)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtDetail", objControl.txtarea)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtPosterFileName", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtPhotoFileName", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtsDecide", objControl.txtbox)] = addValidate_notEmpty(" ");

            objValidate[GetElementName("txtPosted", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtPlaced", objControl.txtbox)] = addValidate_notEmpty(" ");
            //objValidate[GetElementName("txtGiveAward", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("ddlZone", objControl.dropdown)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtYear", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtsTime", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtSendforlio", objControl.txtbox)] = addValidate_notEmpty(" ");

            $('input[id$=txtsDecide]').change(function () {
                ValidationUpdate($(this));
            });

            $('input[id$=txtPosted]').change(function () {
                ValidationUpdate($(this));
            });

            $('input[id$=txtSendforlio]').change(function () {
                ValidationUpdate($(this));
            });

            break;


            //#endregion

            //#region ContactUS

        case 'ContactUS':

            objValidate[GetElementName("txtPhotoFileName", objControl.txtbox)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtDetail", objControl.txtarea)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("txtMap", objControl.txtarea)] = addValidate_notEmpty(" ");

            break;
            //#endregion

        case 'Report1':

            //objValidate[GetElementName("txtsYear", objControl.txtbox)] = addValidate_notEmpty(" ");
            //objValidate[GetElementName("ddlTime", objControl.dropdown)] = addValidate_notEmpty(" ");
            objValidate[GetElementName("ddlCZone", objControl.dropdown)] = addValidate_notEmpty(" ");
            break;

        case 'Report4':

            objValidate[GetElementName("txtsYear", objControl.txtbox)] = addValidate_notEmpty(" ");
            break;
    }


    BindValidate("divFormValidate", objValidate);

}

function objValidate_Sub(page, div_ID) {

    var objValidate2 = {};

    switch (page) {

        //#region contest
        case 'contest_S1':

            var isDataName = function (msg) {

                return {
                    validators: {
                        regexp: {
                            regexp: "^([^0-9]*)$",
                            message: "Invalid Number"
                        },
                        notEmpty: {
                            message: msg
                        }
                    }
                };

            };

            objValidate2[GetElementName("txtsFullName", objControl.txtbox)] = isDataName(" ");
            objValidate2[GetElementName("ddlPosition", objControl.dropdown)] = addValidate_notEmpty(" ");
            break;
            //#endregion
    }


    BindValidate(div_ID, objValidate2);

}

function bCheckValidate($divID) {

    var b = false;

    if (CheckValidate($divID)) {

        b = true;

    } else {
        //UnBlockUI();
        $('html, body').animate({ scrollTop: $('.has-error').offset().top }, 'slow');
    }

    return b;
}

function ADD_Validate(sContainer, fn_validate, name) {

    $("#" + sContainer).formValidation('addField', name, fn_validate);
}

function Remove_Validate(sContainer, fn_validate, name) {

    $("#" + sContainer).formValidation('removeField', name, fn_validate);
}

function Update_Validate(sContainer, name) {

    $("#" + sContainer).formValidation('updateStatus', name, 'NOT_VALIDATED');
}

function Revalidate_Validate(sContainer, name) {

    $("#" + sContainer).formValidation('revalidateField', name, 'NOT_VALIDATED');
}
