﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="user_info.aspx.cs" Inherits="user_info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <input id="username" style="display: none" type="text" name="fakeusernameremembered">
    <input id="password" style="display: none" type="password" name="fakepasswordremembered">

    <div class="card">
        <div class="card-header bg-info1">
            <i class="fa fa-user-circle"></i>&nbsp; User Info
        </div>
        <div class="card-body">
            <div id="divForm">
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
                    <label class="col-lg-3 col-form-label">Email <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="40" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Username <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="20" />
                    </div>
                </div>
            </div>

            <div id="divPassword">
                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">New Password <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" MaxLength="20" autocomplete="new-password" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Confirm Password <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:TextBox ID="txtPassword1" runat="server" CssClass="form-control" MaxLength="20" autocomplete="new-password" />
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

        $(function () {
            if (!isTimeOut) {
                SetControl();
                SetValidate()
            }
        });

        function SetControl() {
            $('input[id$=txtPassword],input[id$=txtPassword1]').on('keyup', function () {
                ReValidateFieldControl('divPassword', $('input[id$=txtPassword]').attr('name'));
                ReValidateFieldControl('divPassword', $('input[id$=txtPassword1]').attr('name'));

                if (GetValTextBox('txtPassword') == "" && GetValTextBox('txtPassword1') == "") {
                    SetNotValidateTextbox('divPassword', 'txtPassword');
                    SetNotValidateTextbox('divPassword', 'txtPassword1');
                }
            });

            $btnBack.click(function () {
                window.Redirect('index.aspx');
            });

            $btnSave.click(function () {
                SaveData();
                return false;
            });
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("txtName", objControl.txtbox)] = addValidate_notEmpty("Name is required!");
            objValidate[GetElementName("txtSurname", objControl.txtbox)] = addValidate_notEmpty("Surname is required!");
            objValidate[GetElementName("txtUsername", objControl.txtbox)] = addValidate_notEmpty("Username is required!");
            objValidate[GetElementName("txtEmail", objControl.txtbox)] = addValidateEmail_notEmpty();
            BindValidate("divForm", objValidate);

            objValidate = {};
            objValidate[GetElementName("txtPassword", objControl.txtbox)] = addValidatePassword_notEmpty(20, '');
            objValidate[GetElementName("txtPassword1", objControl.txtbox)] = addValidatePassword_notEmpty_Confirm('txtPassword');
            BindValidate("divPassword", objValidate);

            SetNotValidateTextbox('divPassword', 'txtPassword')
        }

        function SaveData() {
            if (CheckValidate('divForm') && (GetValTextBox('txtPassword') != "" || GetValTextBox('txtPassword') != "" ? CheckValidate('divPassword') : true)) {
                DialogConfirmSubmit(function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();

                    var IsGC = GetValRadioList('rdlUserType') == "1";
                    var obj = {
                        'nUserID': +GetValTextBox('hddnUserID'),
                        'sUserID': GetValTextBox('txtUsername'),
                        'sPassword': GetValTextBox('txtPassword'),
                        'sFirstname': GetValTextBox('txtName'),
                        'sLastname': GetValTextBox('txtSurname'),
                        'sEmail': GetValTextBox('txtEmail')
                    }

                    AjaxWebMethod("SaveData", obj, function (response) {
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupSessionTimeOut();
                        } else if (response.d.Status == SysProcess.Duplicate) {
                            UnblockUI();
                            DialogDuplicate();
                        } else if (response.d.Status == SysProcess.SaveFail) {
                            UnblockUI();
                            DialogSaveFail(response.d.Msg);
                        } else {
                            UnblockUI();
                            DialogSucessRedirect('index.aspx');
                        }
                    });
                });
            }
        }
    </script>
</asp:Content>

