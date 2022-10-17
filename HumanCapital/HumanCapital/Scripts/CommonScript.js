var msgAlert = {
    HeadConfDel: 'Do you want to delete data?',
    HeadConfBack: 'Do you want to go previous page ?',
    HeadDelComplete: 'Delete complete',
    HeadSaveComplete: 'Save complete',
    HeadWarning: 'Warning',
    HeadDelWarnSelectItem: 'Data much be selected',
    HeadConfChangeStatus: 'Do you want to change status?',
    HeadConfSave: "คุณต้องการที่จะบันทึกข้อมูลนี้หรือไม่?",
    HeadConfAcceptMail: "คุณต้องการที่จะเข้าร่วมการประชุมนี้หรือไม่?",

    HeadConfChangeOrder: 'Confirm change order',
    HeadConfConfirm: 'Confirmation',

    Info: "แจ้งผลการดำเนินการ",
    Error: "เกิดข้อผิดพลาด",
    Confirm: "ยืนยันการทำรายการ",
    Warning: "แจ้งผลการดำเนินการ",

    msgConfDel: 'คุณต้องการที่จะลบข้อมูล?',
    msgConfDelAll: 'คุณต้องการที่จะลบข้อมูลทั้งหมด?',
    msgConfDelFile: 'คุณต้องการที่จะลบไฟล์?',

    msgConfBack: 'Click confirm to Back and data is not saved in database',
    msgDelComplete: 'Data have been deleted',
    msgSaveComplete: 'บันทึกข้อมูลเรียบร้อยแล้ว',
    msgSaveCompleteEmail: 'บันทึกข้อมูลและส่งอีเมล์แจ้งเรียบร้อยแล้ว',

    msgWarnSelectItem: 'Please select at least one record',
    msgConfChangeStatus: 'คุณต้องการที่จะเปลี่ยนแปลงสถานะ',
    msgConfSave: "คุณต้องการที่จะบันทึกข้อมูลนี้หรือไม่?",
    msgConfSavePoster: "คุณจะต้องบันทึกข้อมูลก่อนเพื่อไปยังหน้าสร้างโปสเตอร์หรือไม่?",
    msgConfChangeOrder: 'Do you want to change order',
    msgConfRegister: "คุณต้องการที่จะลงทะเบียนข้อมูลนี้หรือไม่?",
    msgConfJoin: "คุณต้องการที่จะลงชื่อเข้าร่วมประชุมนี้หรือไม่?",
    msgConfInvite: "คุณต้องการที่จะเชิญเข้าร่วมประชุมนี้หรือไม่?",
    msgConfInviteAll: "คุณต้องการที่จะเชิญเข้าร่วมประชุมทั้งหมดหรือไม่?",
    msgConfReg: "คุณต้องการที่จะลงทะเบียนข้อมูลนี้หรือไม่?",
    msgConfNotReg: "คุณต้องการที่จะยกเลิกลงทะเบียนข้อมูลนี้หรือไม่?",
    msgConfRegOut: "คุณต้องการที่จะลงชื่อออกหรือไม่?",
    msgConfDelMeeting: "คุณต้องการที่จะลบการประชุมรอบนี้หรือไม่?",


    msgReset: "คุณต้องการที่จะรีเซ็ตสถานะเป็นรอตอบรับหรือไม่?",


    msgDuplicate: "ไม่สามารถบันทึกข้อมูลซ้ำได้",
    msgSessionExpired: "กรุณาล๊อกอินเข้าสู่ระบบ",
    msgFailed: "ไม่สามารถบันทึกข้อมูลได้",
    msgEmailComplete: 'ส่งอีเมล์เรียบร้อยแล้ว',
    msgNotPermission: "คุณไม่มีสิทธิ์เข้าใช้งานเมนูนี้",

    msgConflogout: "Do you want to log out?",

}

$(function () {

});

function SetTooltip() {
    $('button.btn:not(.dropdown-toggle), input.btn, a.btn,i').tooltipster({
        animation: 'fade',
        delay: 100,
        theme: 'tooltipster-borderless',
        trigger: 'hover'
    });
}


function SetTooltip_Control($el) {
    $el.tooltipster({
        animation: 'fade',
        delay: 100,
        theme: 'tooltipster-borderless',
        trigger: 'hover'
    });
}

function Redirect(Url) {
    location.href = Url;
}
function Redirect_blank(Url) {
    window.open(Url, '_blank');
}

function IsNullOrEmpty(str) {
    var isNull = (str == null);
    var isEmpty = !isNull ? (str == '') : true;
    return isEmpty;
} //Boolean
function IsNullOrEmpty_Zero(str) {
    var isNull = (str == null);
    var isEmpty = !isNull ? (str == '') : true;
    return isEmpty && str !== 0;
} //Boolean
function IsNullOrEmptyString(str) {
    return ((str) && (str != "null") ? str : "");
} //""
function IsNullOrEmptyString2(str) {
    return ((str) ? str : "-");
} //"-"

//SET SwAlert
function CheckSwAlert(status, sMsg) {
    switch (status) {
        case "DUP":
            SwAlert.Error(msgAlert.Error, ((sMsg) ? sMsg : msgAlert.msgDuplicate));
            break;
        case "Failed":
            SwAlert.Error(msgAlert.Error, ((sMsg) ? sMsg : msgAlert.msgFailed));
            break;
        case "SSEXP":
            SwAlert.Error(AlertTitle.Warning, AlertMsg.SessionTimeOut, function () { window.location.href = "AD/index.aspx"; });
            break;
    }
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
        SwAlert.Confirm(msgAlert.Confirm, msgAlert.msgConfDelFile, fnDeleteFile);
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
        SwAlert.Confirm(msgAlert.Confirm, msgAlert.msgConfDelFile, fnDeleteFile);
    });

    if (!IsNullOrEmpty($hidSysFileName.val())) {
        $txtFileName.prop('disabled', true);
        $btnBrowseFile.prop('disabled', true);
        $btnViewFile.show();
        $btnDelFile.show();
    }

    return objUploadFunc;
}

Number.prototype.padLeft = function (n, str) {
    return Array(n - String(this).length + 1).join(str || '0') + this;
};
String.prototype.padLeft = function (n, str) {
    return Array(n - String(this).length + 1).join(str || '0') + this;
};
Number.prototype.padRight = function (n, str) {
    return this + Array(n - String(this).length + 1).join(str || '0');
}
String.prototype.padRight = function (n, str) {
    return this + Array(n - String(this).length + 1).join(str || '0');
}
String.prototype.replaceAll = function (strOld, strNew) {
    var target = this;
    return target.split(strOld).join(strNew);
};
function FancyBox_ViewFile(sFileURL) {
    var sFileExt = sFileURL.split('.').pop(); //File Extension
    var isImage = $.inArray(sFileExt, Extension.Image) > -1; //Extension.Image.indexOf(sFileExt) > -1;
    var arrFile = sFileURL.split('/');
    var sFileName = arrFile[arrFile.length - 1];
    sFileURL = sFileURL.replace(sFileName, '') + encodeURIComponent(sFileName);

    if (isImage || sFileExt == 'pdf') {
        $.fancybox.open({
            src: sFileURL,
            type: isImage ? 'image' : 'iframe',
            iframe: { preload: false }, // fixes issue with iframe and IE
            padding: 5,
            openEffect: 'elastic',
            openSpeed: 150,
            closeEffect: 'elastic',
            closeSpeed: 150,
            closeClick: true,
            helpers: {
                overlay: {
                    locked: false
                }
            }
        });
    } else {
        window.open(sFileURL, '_blank');
    }
}

//DataTable
function DDLOrderOnChange(sID, nRowNo) {
    //BlockUI();
    AjaxWebMethod('DDLOrderOnChange', { sID_Encrypt: sID, ordered: $('select[id$=' + nRowNo + 'ddlOrder]').val() }
        , function (response) {
            var status = response.d.Status;
            if (status == "Success") {
                SearchData();
            }
            else {
                //จะเข้าไปหา CASE แล้ว Alter
                //CheckSwAlert(status);
            }
        }); //, UnblockUI
}
function switchStatus(sEdit, thisInput) {
    SwAlert.Confirm(msgAlert.Confirm
        , msgAlert.msgConfChangeStatus
        , function () {
            swal.disableButtons();
            BlockUI();
            var result = $('input[id$=' + thisInput.id + ']').prop('checked') ? 'Y' : 'N';
            AjaxWebMethod('switchStatus', { 'sEdit': sEdit, 'sStatus': result }, function () { }, function (data) {
                if (data.d == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                }
                else {
                    swal.close();
                    SearchData($('ul[id$=pagData] li.active').text());
                }
            });
        }, function myfunction() {
            $('input[id$=' + thisInput.id + ']').prop('checked', !$('input[id$=' + thisInput.id + ']').prop('checked'));
            swal.close();
        });
}
function ActiveDataBind(nPageSize, nPageNo) {
    var arrDataPaging = DataPaging(arrData, nPageSize, nPageNo);
    DataBind($tbData, arrDataPaging, nPageSize, nPageNo, CreateDataRow, OnDataBound);
}
function CheckDataFound() {
    var isFoundData = DataChecking(arrData, $divNoData);
    if (isFoundData) $divPaging.show('fast');
    else $divPaging.hide('fast');
    return isFoundData;
}
function OnDataBound() {
    CheckDataFound();
    SortingEvent();
    $cbHead.prop('checked', false);
}
function CheckDataFound_Rev(arrData, $divNoData, $divPaging) {
    var isFoundData = DataChecking(arrData, $divNoData);
    if (isFoundData) $divPaging.show('fast');
    else $divPaging.hide('fast');
    return isFoundData;
}
function ActiveDataBind_Rev(nPageSize, nPageNo, arrData, $tbData, CreateDataRow, OnDataBound) {
    var arrDataPaging = DataPaging(arrData, nPageSize, nPageNo);
    DataBind($tbData, arrDataPaging, nPageSize, nPageNo, CreateDataRow, OnDataBound);
}

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

var Sorting = {
    ASCENDING: 'asc',
    DESCENDING: 'desc'
};

//sorting
function SortingClear($table) {
    $($table).find('thead [data-sort]').removeClass('sort-asc sort-desc').addClass('sort-non');
}
function SortingBind($table, fnSorting) {
    $table = $($table);
    var $col_sort = $table.find('thead [data-sort]').addClass('sort sort-non');

    $col_sort.click(function () {
        var $col = $(this);
        var sExpression = $col.attr('data-sort');
        var sDirection_Current = $col.is('.sort-asc') ? Sorting.ASCENDING : Sorting.DESCENDING;

        if (!IsNullOrEmpty(sExpression)) {
            var sDirection_New = sDirection_Current == Sorting.ASCENDING ? Sorting.DESCENDING : Sorting.ASCENDING;

            fnSorting(sExpression, sDirection_New);

            $col.removeClass('sort-non sort-' + sDirection_Current).addClass('sort-' + sDirection_New);
            $col_sort.not($col).removeClass('sort-asc sort-desc').addClass('sort-non');
        }
    });
}
function DataSort(sDirection, fnByAscending, fnByDescending) {
    if (sDirection == Sorting.ASCENDING) fnByAscending();
    else fnByDescending();
}

function SetActiveMenu() {
    /*menu handler*/
    var arrPath = window.location.pathname.split('/');
    var action = arrPath[arrPath.length - 1].toLowerCase();

    // If there's no action, highlight the first menu item
    if (action == "") {
        $('ul li:first').addClass('active');
    } else {
        action = action.replace('_edit', '');

        // Highlight current menu
        if ("etk_verify.aspx" == action || "etk_ranking.aspx" == action || "etk_verify_lst.aspx" == action || "etk_ranking_lst.aspx" == action) {
            var type = window.location.href.split('?')[1].split('=')[1].substring(1, 0);
            $('ul.cd-primary-nav li#sMenu_3' + type).addClass('active');
            $('ul.cd-primary-nav li#sMenu_3' + type).parent().parent().addClass('active');
        } else {
            $('ul.cd-primary-nav li a[href="' + action + '"]').parent().addClass('active');
        }

        //// Highlight parent menu item
        $('ul.cd-primary-nav li a[href="' + action + '"]').parent().parent().parent().addClass('active');
    }
}

function BlockUI() {
    $.blockUI({
        //message: '<div id="loading-wrapper"><div id="loading-text">LOADING</div><div id="loading-content"></div></div>',
        message: '<div class="cssload-box-loading"></div>',
        css: {
            border: 'none',
            //padding: '10px',
            backgroundColor: 'none',
            //'-webkit-border-radius': '10px',
            //'-moz-border-radius': '10px',
            //opacity: .5,
            color: '#FFF',
            fontFamily: 'THSarabunNew'
        },
        baseZ: 5000
    });
} //Void

function UnblockUI() {
    $.unblockUI();
} //Void

function MakeContentOverFooter() {
    var nHeight_divMainFooter = $('#divMainFooter').height();
    var isSmallSize = $('body').width() <= 1169;
    $('#mainBody').css('padding-bottom', isSmallSize ? $('#divCopyRight').outerHeight() + 80 : nHeight_divMainFooter);
}

function RemoveItemInArray(array, item) {
    for (var i in array) {
        if (array[i] == item) {
            array.splice(i, 1);
            break;
        }
    }
}

function DropdownToMultiselect(ddl, text) {
    $("select[id$=" + ddl + "]").multiselect({
        buttonClass: 'btn btn-secondary',
        templates: {
            li: '<li><a class="dropdown-item"><label class="m-0 pl-2 pr-0"></label></a></li>',
            //ul: ' <ul class="multiselect-container dropdown-menu p-1 m-0"></ul>',
            filter: '<li class="multiselect-item filter"><div class="input-group m-0"><input class="form-control multiselect-search" type="text"></div></li>',
            filterClearBtn: '<span class="input-group-btn"><button type="button" class="btn btn-secondary multiselect-clear-filter">&times;</button></span>'
        },
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        filterPlaceholder: 'ระบุคำค้นหา',
        includeSelectAllOption: true,
        selectAllText: 'เลือกทั้งหมด',
        numberDisplayed: 1,
        maxHeight: 400,
        //buttonWidth: '100%',
        buttonText: function (options, select) {
            if (options.length === 0) {
                return ((text) ? text : "None selected");
            } else if (options.length > 1) {
                if ((options.length) == $("select[id$=" + ddl + "] option").length) {
                    return text;
                } else {
                    return 'เลือก (' + options.length + ')';
                }
            } else {
                var labels = [];
                options.each(function () {
                    if ($(this).attr('label') !== undefined) {
                        labels.push($(this).attr('label'));
                    }
                    else {
                        labels.push($(this).html());
                    }
                });
                return labels.join(', ') + '';
            }
        },

    });
}

function BindMenubar_Organization(nPage) {
    var sHTML = '<ul class="nav nav-tabs card-header-tabs">'; // 

    var objMenu = [];

    objMenu = [{
        id: 1,
        url: "b_create_function_group.aspx",
        name: "Function Group",
    }, {
        id: 2,
        url: "b_create_division.aspx",
        name: "Division",
    }, {
        id: 3,
        url: "b_create_department.aspx",
        name: "Department",
    }];

    $.each(objMenu, function (i, el) {
        var isActive = (nPage == el.id);
        sHTML += '<li class="nav-item"><a style="cursor: pointer;" class="nav-link ' + (isActive ? 'active' : '') + '" onclick="Redirect(\'' + el.url + '\')" >' + el.name + '</a></li>';
    });

    sHTML += '</ul>';

    return sHTML;
}

function BindMenubar_Initiative(nPage) {
    var sHTML = '<ul class="nav nav-tabs card-header-tabs">'; //

    var objMenu = [];

    objMenu = [{
        id: 1,
        name: "Information",
    }, {
        id: 2,
        name: "Milestone",
    }, {
        id: 3,
        name: "Value Summary Chart",
    }, {
        id: 4,
        name: "Attach File",
    }, {
        id: 5,
        name: "History",
    }];

    $.each(objMenu, function (i, el) {
        var isActive = (nPage == el.id);
        sHTML += '<li class="nav-item"><a id="aMenubar_' + el.id + '" class="nav-link ' + (isActive ? 'active' : '') + '" onclick="SetTab(' + el.id + ')" style="cursor: pointer;">' + el.name + '</a></li>';
    });

    sHTML += '</ul>';

    return sHTML;
}

function Sub_string(txt, max) {
    if (txt.length > max) {
        txt = txt.substr(0, max) + "...";
    }

    return txt;
}

function Set_MoneyFormat(val) {
    return (val + "").replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

function round2Fixed(value) {
    value = +value;

    if (isNaN(value))
        return NaN;

    // Shift
    value = value.toString().split('e');
    value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] + 2) : 2)));

    // Shift back
    value = value.toString().split('e');
    return (+(value[0] + 'e' + (value[1] ? (+value[1] - 2) : -2))).toFixed(2);
}

function ConvertDatetoSort_yyyyMMdd(sDate) {
    var arr = sDate.split("/");
    return arr[2] + arr[1] + arr[0];
}

function ConvertDatetoSort_MMddyyyy(sDate) {
    var arr = sDate.split("/");
    return arr[1] + arr[0] + arr[2];
}

function ConvertDatetoSort_yyyyMM(sDate) {
    var arr = sDate.split("/");
    return arr[1] + arr[0];
}

function ConvertDatetoSort_yyyy(sDate) {
    var arr = sDate.split("/");
    return arr[1];
}

function BindSelectpicker() {
    $('.selectpicker').selectpicker('toggle');
    ScrollTopToElements("boxChart");
}

