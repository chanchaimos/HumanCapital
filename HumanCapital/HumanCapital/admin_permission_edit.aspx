<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_permission_edit.aspx.cs" Inherits="admin_permission_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        #tbData tbody tr td span {
            display: inline-flex;
        }

        table.checkbox {
            width: 100%;
        }

            table.checkbox > tbody > tr > td {
                width: 50% !important;
                display: inline-flex !important;
                vertical-align: top !important;
            }

            table.checkbox label {
                padding-left: 15px !important;
            }

        table.radio > tbody > tr > td:last-child {
            padding-left: 20px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <input id="username" style="display: none" type="text" name="fakeusernameremembered">
    <input id="password" style="display: none" type="password" name="fakepasswordremembered">

    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">

            <div id="divForm">

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Role <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Company Group</label>
                    <div class="col-lg-4">
                        <asp:RadioButtonList ID="rdlUserType" runat="server" CssClass="radio" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="2">
                        </asp:RadioButtonList>
                    </div>
                </div>

                <div id="divGC" class="form-group row">
                    <label class="col-lg-3 col-form-label">Name - Surname <span class="text-red">*</span></label>
                    <div class="col-lg-5">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fa fa-search"></i></span>
                            </div>
                            <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" placeholder="Username / Name-Surname" />
                            <asp:TextBox ID="txtEmpID" runat="server" CssClass="form-control hide"></asp:TextBox>
                            <asp:TextBox ID="txtFName" runat="server" CssClass="form-control hide"></asp:TextBox>
                            <asp:TextBox ID="txtLName" runat="server" CssClass="form-control hide"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div id="divNonGC">
                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Name <span class="text-red">*</span></label>
                        <div class="col-lg-4">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100" />
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Surname <span class="text-red">*</span></label>
                        <div class="col-lg-4">
                            <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control" MaxLength="100" />
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Username <span class="text-red">*</span></label>
                        <div class="col-lg-4">
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="20" />
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Password <span class="text-red">*</span></label>
                        <div class="col-lg-4">
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" MaxLength="20" />
                        </div>
                        <div class="col-auto">
                            <button type="button" id="btnResetPass" class="btn btn-primary hide"><i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset password</button>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Email <span class="text-red">*</span></label>
                    <div class="col-lg-5">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="40" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Status</label>
                    <div class="col-lg-auto">
                        <asp:RadioButtonList ID="rdlActive" runat="server" CssClass="radio" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                            <asp:ListItem Value="0" class="pl-4">Inactive</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Permission Scope <span class="text-red">*</span></label>
                    <div class="col-lg-9">
                        <div id="divDJSI" class="table-responsive">
                            <table id="tbCom" class="table table-bordered table-responsive-sm table-responsive-md">
                                <thead>
                                    <tr class="valign-middle pad-primary">
                                        <th style="width: 10%" class="text-center">
                                            <asp:CheckBox ID="cbHead" runat="server" CssClass="checkbox " Text="No" />
                                        </th>
                                        <th class="text-center">Company</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                            <div id="divNoDataCom" class="dataNotFound">No Data</div>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Permission</label>
                    <div class="col-lg-9">
                        <div id="divData" class="col-sm-12" style="margin-left: -15px;">
                            <div class="table-responsive">
                                <table id="tbData" class="table table-bordered table-hover">
                                    <thead class="pad-info">
                                        <tr class="valign-middle">
                                            <th class="text-center" data-sort="">Menu</th>
                                            <th class="text-center" data-sort="" style="width: 20%">Not View</th>
                                            <th class="text-center" data-sort="" style="width: 20%">View</th>
                                            <th class="text-center" data-sort="" style="width: 20%">Add/Edit/Delete</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                                <div id="divNoData" class="dataNotFound">No Data</div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </div>
        <div class="card-footer">
            <div class="col-12 text-center">
                <button id="btnBack" type="button" class="btn btn-secondary"><i class="fa fa-arrow-left"></i>&nbsp; Back</button>
                <button id="btnSave" type="button" class="btn btn-info"><i class="fa fa-save"></i>&nbsp; Save</button>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hddnUserID" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnSave = $('button#btnSave');
        var $btnBack = $('button#btnBack');
        var $cbHead = $('input[id$=cbHead]');
        var arrNotAdd = [1, 8, 12, 13, 15];
        var arrMenuBackend = [7, 8, 9, 10, 14];
        var arrMenuFront = [1, 2, 3, 4, 5, 6];
        var nLength = 6;

        var arrDataMenu = [];
        var arrCompany = [];
        var arrDataCompany = [];

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SetValidate();
                    GetData_Default();
                    SetUserType(true);
                    SetAutoComplete();
                }

                if ($Permission != "A") {
                    $('div#divForm').find('input,select').prop('disabled', true)
                    $btnSave.remove();
                    $('#btnResetPass').remove();
                }
            }
        });

        function GetData_Default() {
            BlockUI();
            var nUserID = GetValTextBox('hddnUserID') != "" ? +GetValTextBox('hddnUserID') : null;
            AjaxWebMethod("GetData_Default", { 'nUserID': nUserID }, function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else {
                    arrDataMenu = response.d.lstDataMenu;
                    BindTable_Menu();
                    arrCompany = response.d.lstCompanay;
                    arrDataCompany = response.d.lstDataCompany;
                    BindTable_Company();

                    SetMenuByRole(true);
                }
            }, function () {
                UnblockUI();
                if ($Permission == "V") {
                    $('#tbData').find('input,select').prop('disabled', true);
                }
            });
        }

        function BindTable_Menu() {
            var $Table = $('#tbData tbody');
            $Table.html('');
            if (arrDataMenu.length > 0) {
                var lstData = arrDataMenu;

                for (var i = 0; i < lstData.length; i++) {
                    var sID = lstData[i].nMenuID;
                    var CanAdd = arrNotAdd.indexOf(sID) > -1;
                    var IsHead = lstData[i].IsHead;
                    var nLevel = lstData[i].nLevel;

                    var sTD = '<td>' + (nLevel == 1 ? "" : "&emsp;-") + " " + lstData[i].sMenuName + '</td>';
                    sTD += !IsHead ? ('<td class="text-center"><span class="radio"><input id="rdoNotView_' + sID + '" type="radio" name="rdoPer_' + sID + '" value="0"><label for="rdoNotView_' + sID + '"></span></td>') : '<td colspan="3"></td>';
                    sTD += !IsHead ? ('<td class="text-center"><span class="radio"><input id="rdoView_' + sID + '" type="radio" name="rdoPer_' + sID + '" value="1"><label for="rdoView_' + sID + '"></span></td>') : '';
                    sTD += !IsHead ? (!CanAdd ? ('<td class="text-center"><span class="radio"><input id="rdoAll_' + sID + '" type="radio" name="rdoPer_' + sID + '" value="2"><label for="rdoAll_' + sID + '"></span></td>') : '<td></td>') : '';

                    $Table.append('<tr>' + sTD + '</tr>');

                    var nPermission = lstData[i].nPermission;
                    if (nPermission != null) {
                        var sType = nPermission == 2 ? "All" : (nPermission == 1 ? "View" : "NotView");
                        $('input[id$=rdo' + sType + '_' + sID + ']').prop('checked', true);
                    }
                }
                $('#divNoData').hide("fast");
            } else {
                $('#divNoData').show("fast");
            }
        }

        function BindTable_Company() {
            var $Table = $('#tbCom tbody');
            $Table.html('');
            if (arrCompany.length > 0) {
                $.each(arrCompany, function (i, el) {
                    var nItem = el.nCompanyID;
                    var IsChecked = "";
                    var cb = '<td class="text-center">' +
                                     '<div class="checkbox"><input type="checkbox" name="cbRec_' + nItem + '" id="cbRec_' + nItem + '" value="' + nItem + '" ' + IsChecked + '/>' +
                                     '<label for="cbRec_' + nItem + '">' + (i + 1) + '.</label></div></td>';

                    $Table.append('<tr>' + cb + '<td>' + el.sCompanyName + '</td></tr>');
                });

                if (arrDataCompany.length > 0) {
                    $.each(arrDataCompany, function (i, el) {
                        $('input[name*=cbRec_]:checkbox[value=' + el + ']').prop("checked", "true");
                    });
                }

                if (arrDataCompany.length == arrCompany.length) $('input[id$=cbHead]').prop('checked', true);

                $('#divNoDataCom').hide("fast");
            } else {
                $('#divNoDataCom').show("fast");
            }
        }

        function SetControl() {
            if (GetValTextBox('hddnUserID') != "") {
                $('input[name*=rdlUserType]').prop('disabled', true);
                $('#btnResetPass').removeClass('hide');
            }

            $cbHead.change(function () {
                var IsChecked = $(this).is(':checked');
                var $cbRec = $('input[id*=cbRec_]:checkbox');
                $cbRec.prop('checked', IsChecked);
            });

            $('#tbCom tbody').delegate('input', 'click', function () {
                var $cbRec = $('input[id^="cbRec_"]:checkbox');
                var $cbRec_Checked = $cbRec.filter(':checked');
                var n_$cbRec = $cbRec.length;
                var IsCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_Checked.length : false;
                $cbHead.prop('checked', IsCheckedAll);
            });

            $('input[name*=rdlUserType]').on('change', function () {
                SetUserType(false);
            });

            $('select[id$=ddlRole]').change(function () {
                SetMenuByRole(false);
            });

            $btnBack.click(function () {
                window.Redirect('admin_permission.aspx');
            });

            $('#btnResetPass').click(function () {
                var nUserID = +GetValTextBox('hddnUserID');
                if (nUserID > 0) {
                    DialogConfirmResetPassword(function () {
                        BlockUI();
                        AjaxWebMethod('ResetPassword', { 'nUserID': nUserID }, function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupSessionTimeOut();
                            } else if (response.d.Status == SysProcess.SaveFail) {
                                UnblockUI();
                                DialogSaveFail(response.d.Msg);
                            } else {
                                UnblockUI();
                                DialogSucessRedirect('admin_permission.aspx');
                            }
                        });
                    });
                }

                return false;
            });

            $btnSave.click(function () {
                SaveData();
                return false;
            });
        }

        function SetUserType(IsFirst) {
            var IsEdit = GetValTextBox('hddnUserID') != "";
            var IsGC = GetValRadioList('rdlUserType') == "5";
            var nRole = +GetValDropdown('ddlRole');
            var arrRole = [];
            if (IsGC) {
                $('#divGC').show();
                $('#divNonGC').hide();

            } else {
                $('#divGC').hide();
                $('#divNonGC').show();
            }

            EnableValidateControl('divForm', $('input[id$=txtEmpName]').attr('name'), IsGC);

            EnableValidateControl('divForm', $('input[id$=txtName]').attr('name'), !IsGC);
            EnableValidateControl('divForm', $('input[id$=txtSurname]').attr('name'), !IsGC);
            EnableValidateControl('divForm', $('input[id$=txtUsername]').attr('name'), !IsGC);
            EnableValidateControl('divForm', $('input[id$=txtPassword]').attr('name'), !IsGC);

            SetNotValidateTextbox('divForm', 'txtEmail');
            EnableValidateControl('divForm', $('input[id$=txtEmail]').attr('name'), !IsGC);
            $('input[id$=txtEmail]').prop('disabled', IsGC);

            if (!IsFirst) {
                $('#divGC input,#divNonGC input,input[id$=txtEmail]').val('');
            }
        }

        function SetMenuByRole(IsFirst) {
            var sVal = GetValDropdown('ddlRole');
            var IsAdmin = sVal == "1";

            if (!IsFirst && sVal != "") {
                if (GetValTextBox('hddnUserID') == "") {
                    $("input[id$=txtEmpName]").val("");
                    $("input[id$=txtEmpID]").val("");
                    $("input[id$=txtFName]").val("");
                    $("input[id$=txtLName]").val("");
                    if (GetValRadioListNotValidate('rdlUserType') == "5") {
                        $('input[id$=txtEmail]').val('');
                    }
                    SetNotValidateTextbox("divForm", 'txtEmpName');
                }

                for (var i = 0; i < arrMenuFront.length; i++) {
                    $('input[id*=rdo' + (arrNotAdd.indexOf(arrMenuFront[i]) == -1 ? 'All' : 'View') + '_' + arrMenuFront[i] + ']').prop('checked', true);
                }

                for (var i = 0; i < arrMenuBackend.length; i++) {
                    $('input[id*=rdo' + (IsAdmin ? (arrMenuBackend[i] != 8 ? 'All' : 'View') : 'NotView') + '_' + arrMenuBackend[i] + ']').prop('checked', true);
                }
            }
        }

        function SetValidate() {
            //$("input[id*=cblCompany]").attr("name", "cblCompany");

            var objValidate = {};
            objValidate[GetElementName("ddlRole", objControl.dropdown)] = addValidate_notEmpty("Role" + IsRequire);
            objValidate[GetElementName("txtEmpName", objControl.txtbox)] = addValidate_notEmpty("Name - Surname" + IsRequire);
            objValidate[GetElementName("txtName", objControl.txtbox)] = addValidate_notEmpty("Name" + IsRequire);
            objValidate[GetElementName("txtSurname", objControl.txtbox)] = addValidate_notEmpty("Surname" + IsRequire);
            objValidate[GetElementName("txtUsername", objControl.txtbox)] = addValidate_notEmpty("Username" + IsRequire);
            objValidate[GetElementName("txtPassword", objControl.txtbox)] = addValidatePassword_notEmpty(20, '');
            objValidate[GetElementName("txtEmail", objControl.txtbox)] = addValidateEmail_notEmpty();
            //objValidate["cblCompany"] = addValidate_cblNotEmpty("Company" + IsRequire);

            BindValidate("divForm", objValidate);
            //$('i[data-fv-icon-for="cblCompany"]').css('left', '700px').css('top', '-20px');

            SetNotValidateTextbox('divForm', 'txtPassword')
        }

        var IsSelectedtxtEmpName = false;
        function SetAutoComplete() {
            $("input[id$=txtEmpName]")
               .on("change", function () {
                   if (!IsSelectedtxtEmpName || !IsBrowserFirefox()) {
                       $("input[id$=txtEmpName]").val("");
                       $("input[id$=txtEmpID]").val("");
                       $("input[id$=txtFName]").val("");
                       $("input[id$=txtLName]").val("");
                       ReValidateFieldControl("divForm", GetElementName('txtEmpName', objControl.txtbox));
                   }
               }).focus(function () {
                   IsSelectedtxtEmpName = false;
               })
           .autocomplete({
               source: function (request, response) {
                   IsSelectedtxtEmpName = false;
                   if (request.term.replace(/\s/g, "") != "" && request.term.replace(/\s/g, "").length >= nLength) {
                       AjaxWebMethod(UrlSearchEmp(), { 'sSearch': request.term }, function (data) {
                           if (data.d.Status == SysProcess.SessionExpired) {
                               PopupSessionTimeOut();
                           } else {
                               UnblockUI();
                               response($.map(data.d.lstData.d.results, function (item) {
                                   return {
                                       value: item.EmployeeID + ' - ' + item.Name,
                                       label: item.EmployeeID + ' - ' + item.Name,
                                       sUserID: item.EmployeeID,
                                       FName: item.Name.split(' ')[0] + " " + item.ENFirstName,
                                       LName: item.ENLastName,
                                       Email: item.EmailAddress,
                                   }
                               }));
                           }
                       });
                   }
               },
               minLength: nLength,
               select: function (event, ui) {
                   IsSelectedtxtEmpName = true;
                   $("input[id$=txtEmpID]").val(ui.item.sUserID);
                   $("input[id$=txtFName]").val(ui.item.FName);
                   $("input[id$=txtLName]").val(ui.item.LName);
                   $("input[id$=txtEmail]").val(ui.item.Email);
                   SetNotValidateTextbox("divForm", 'txtEmail');
                   if (IsBrowserFirefox()) {
                       $("input[id$=txtEmpName]").blur();;
                   }
               }
           });
        }

        function UrlSearchEmp() {
            BlockUI();
            return 'GetEmp';
        }

        function SaveData() {
            if (CheckValidate('divForm')) {
                DialogConfirmSubmit(function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();

                    var $cbRec = $('input[id^="cbRec_"]:checkbox');
                    var $cbRec_Checked = $cbRec.filter(':checked');
                    var lstCompany = $cbRec_Checked.length > 0 ? $.map($cbRec_Checked, function (cb) { return +$(cb).val(); }) : [];

                    var lstPermission = [];
                    $.each($('#tbData input[id*=rdoNotView_]'), function (i, el) {
                        var sID = el.id.replace('rdoNotView_', '')
                        var sPer = GetValRadioListNotValidate('rdoPer_' + sID);
                        lstPermission.push({ 'nMenuID': +sID, 'nPermission': (sPer != "" ? +sPer : null) });
                    });

                    var IsGC = GetValRadioList('rdlUserType') == "5";
                    var obj = {
                        'nUserID': +GetValTextBox('hddnUserID'),
                        'sUserID': IsGC ? GetValTextBox('txtEmpID') : GetValTextBox('txtUsername'),
                        'sPassword': !IsGC ? GetValTextBox('txtPassword') : '',
                        'sFirstname': !IsGC ? GetValTextBox('txtName') : GetValTextBox('txtFName'),
                        'sLastname': !IsGC ? GetValTextBox('txtSurname') : GetValTextBox('txtLName'),
                        'sEmail': GetValTextBox('txtEmail'),
                        'IsGC': IsGC,
                        'nRole': +GetValDropdown('ddlRole'),
                        'lstPermission': lstPermission,
                        'IsActive': Boolean(+GetValRadioList('rdlActive')),
                        'lstCompany': lstCompany,
                    }

                    AjaxWebMethod("SaveData", obj, function (response) {
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupSessionTimeOut();
                        } else if (response.d.Status == SysProcess.Duplicate) {
                            UnblockUI();
                            BBox.Error(AlertTitle.Warning, response.d.Msg);
                            //DialogDuplicate();
                        } else if (response.d.Status == SysProcess.SaveFail) {
                            UnblockUI();
                            DialogSaveFail(response.d.Msg);
                        } else {
                            UnblockUI();
                            DialogSucessRedirect('admin_permission.aspx');
                        }
                    });
                });
            }
        }
    </script>
</asp:Content>

