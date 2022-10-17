<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="course_edit.aspx.cs" Inherits="course_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">
            <div id="divForm">

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Company <span class="text-red">*</span></label>
                    <div class="col-lg-6">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Course Name <span class="text-red">*</span></label>
                    <div class="col-lg-6">
                        <asp:TextBox ID="txtCoursename" runat="server" CssClass="form-control" MaxLength="250" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Course Category <span class="text-red">*</span></label>
                    <div class="col-lg-6">
                        <asp:DropDownList ID="ddlCourseCat" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Description </label>
                    <div class="col-lg-6">
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" MaxLength="250" TextMode="MultiLine" Rows="3" />
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

            </div>
        </div>
        <div class="card-footer">
            <div class="col-12 text-center">
                <button id="btnBack" type="button" class="btn btn-secondary"><i class="fa fa-arrow-left"></i>&nbsp; Back</button>
                <button id="btnSave" type="button" class="btn btn-info"><i class="fa fa-save"></i>&nbsp; Save</button>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hddID" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnSave = $('button#btnSave');
        var $btnBack = $('button#btnBack');

        $(function () {
            if (!isTimeOut) {
                if ($Permission === "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SetValidate();
                }

                if ($Permission != "A") {
                    $('div#divForm').find('input,select').prop('disabled', true)
                    $btnSave.remove();
                }
            }
        });

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("ddlCompany", objControl.dropdown)] = addValidate_notEmpty("Company" + IsRequire);
            objValidate[GetElementName("txtCoursename", objControl.txtbox)] = addValidate_notEmpty("Course Name" + IsRequire);
            objValidate[GetElementName("ddlCourseCat", objControl.dropdown)] = addValidate_notEmpty("Course Category" + IsRequire);
            BindValidate("divForm", objValidate);
        }

        function SetControl() {
            $btnBack.click(function () {
                window.Redirect('course.aspx');
            });

            $btnSave.click(function () {
                SaveData();
                return false;
            });
        }

        function SaveData() {
            if (CheckValidate('divForm')) {
                DialogConfirmSubmit(function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();

                    var obj = {
                        'nCourseID': +GetValTextBox('hddID'),
                        'sCompany': GetValDropdown('ddlCompany'),
                        'sCoursename': GetValTextBox('txtCoursename'),
                        'sCourseCat': GetValDropdown('ddlCourseCat'),
                        'sDes': GetValTextArea('txtDescription'),
                        'IsActive': Boolean(+GetValRadioList('rdlActive')),
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
                            DialogSucessRedirect('course.aspx');
                        }
                    });
                });
            }
        }

    </script>
</asp:Content>

