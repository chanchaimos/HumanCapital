<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="course_sub_edit.aspx.cs" Inherits="course_sub_edit" %>

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

        .dropdown-toggle {
            background-color: white !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">
            <div id="divForm">

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Company <i class="fas fa-info-circle iBlue" title="Budget Owner"></i><span class="text-red">*</span></label>
                    <div class="col-lg-6">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" onchange="ChangeCompany()">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Course Name <span class="text-red">*</span></label>
                    <div class="col-lg-6">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fa fa-search"></i></span>
                            </div>
                            <asp:TextBox ID="txtCoursename" runat="server" CssClass="form-control" placeholder="Search by Code / Course Name" />
                            <asp:TextBox ID="txtCourseID" runat="server" CssClass="form-control hide"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Sub Course Name  <i class="fas fa-info-circle iBlue" title="Section or Module Name"></i><span class="text-red">*</span></label>
                    <div class="col-lg-6">
                        <asp:TextBox ID="txtSubCoursename" runat="server" CssClass="form-control" MaxLength="250" />
                    </div>
                </div>

            </div>

            <div class="form-group row">
                <label class="col-lg-3 col-form-label">Description </label>
                <div class="col-lg-6">
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" MaxLength="250" TextMode="MultiLine" Rows="3" />
                </div>
            </div>

            <div class="form-group row">
                <label class="col-sm-3 control-label">Start Date</label>
                <div class="col-sm-3">
                    <div class="form-group">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="glyphicon glyphicon-calendar"></i></span>
                            </div>
                            <asp:TextBox ID="txtStartDate" name="txtStartDate" runat="server" type="text" CssClass="form-control" MaxLength="10" placeholder="--/--/----" onchange="DateTimeOnchange()"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <label class="col-md-1 col-sm-12 text-center">Time</label>
                <div class="col-sm-3">
                    <div class="form-group">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="glyphicon glyphicon-time"></i></span>
                            </div>
                            <asp:TextBox ID="txtStartTime" name="txtStartTime" runat="server" type="text" CssClass="form-control" MaxLength="5" placeholder="----" onchange="DateTimeOnchange()"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group row">
                <label class="col-sm-3 control-label">End Date </label>
                <div class="col-sm-3">
                    <div class="form-group">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="glyphicon glyphicon-calendar"></i></span>
                            </div>
                            <asp:TextBox ID="txtEndDate" name="txtEndDate" runat="server" type="text" CssClass="form-control" MaxLength="10" placeholder="--/--/----" onchange="DateTimeOnchange()"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <label class="col-md-1 col-sm-12 text-center">Time</label>
                <div class="col-sm-3">
                    <div class="form-group">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="glyphicon glyphicon-time"></i></span>
                            </div>
                            <asp:TextBox ID="txtEndTime" name="txtEndTime" runat="server" type="text" CssClass="form-control" MaxLength="5" placeholder="----" onchange="DateTimeOnchange()"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group row">
                <label class="col-lg-3 col-form-label">Training Method  </label>
                <div class="col-lg-6">
                    <asp:RadioButtonList ID="rdlTrainingmethod" runat="server" CssClass="radio" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="3">
                    </asp:RadioButtonList>
                </div>
            </div>

            <div class="form-group row">
                <label class="col-lg-3 col-form-label">Target Group </label>
                <div class="col-lg-6">
                    <asp:DropDownList ID="ddlTargetGroup" runat="server" CssClass="form-control selectpicker" multiple>
                    </asp:DropDownList>
                    <small class="form-label text-red" id="spDesTarget" style="font-weight: 300"></small>
                </div>
                <%--<div class="col-1" style="padding-top: 8px"><span class="col-form-label"><i id="spTitle" class="fas fa-info-circle iBlue" title="sdffds"></i></span></div>--%>
            </div>

            <div class="form-group row">
                <label class="col-lg-3 col-form-label">Training Cost per year </label>
                <div class="col-lg-3">
                    <div class="input-group">
                        <asp:TextBox ID="txtPrice" name="txtPrice" runat="server" type="text" CssClass="form-control" MaxLength="20"></asp:TextBox>
                        <div class="input-group-prepend">
                            <span class="input-group-text">Baht</span>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group row">
                <label class="col-lg-3 col-form-label">Number of Participants </label>
                <div class="col-lg-3">
                    <div class="input-group">
                        <asp:TextBox ID="txtAmount" name="txtAmount" runat="server" type="text" CssClass="form-control"></asp:TextBox>
                        <div class="input-group-prepend">
                            <span class="input-group-text">Person(s)</span>
                        </div>
                    </div>
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
        <div class="card-footer">
            <div class="col-12 text-center">
                <button id="btnBack" type="button" class="btn btn-secondary"><i class="fa fa-arrow-left"></i>&nbsp; Back</button>
                <button id="btnSave" type="button" class="btn btn-info"><i class="fa fa-save"></i>&nbsp; Save</button>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hddID" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />
    <asp:HiddenField ID="hddTGID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <%-- Clockpicker  --%>
    <link href="Scripts/Clockpicker/dist/jquery-clockpicker.min.css" rel="stylesheet" />
    <script src="Scripts/Clockpicker/dist/jquery-clockpicker.min.js"></script>

    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnSave = $('button#btnSave');
        var $btnBack = $('button#btnBack');
        var $txtStartDate = $('input[id$=txtStartDate]');
        var $txtStartTime = $('input[id$=txtStartTime]');
        var $txtEndDate = $('input[id$=txtEndDate]');
        var $txtEndTime = $('input[id$=txtEndTime]');

        var $txtPrice = $('input[id$=txtPrice]');
        var $txtAmount = $('input[id$=txtAmount]');

        var $ddlCompany = $('select[id$=ddlCompany]');
        var nLength = 1;
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
            objValidate[GetElementName("txtSubCoursename", objControl.txtbox)] = addValidate_notEmpty("Sub Course Name" + IsRequire);
            BindValidate("divForm", objValidate);
        }

        function SetControl() {
            BindSelectpicker();
            SetClockpicker($txtStartTime);
            SetClockpicker($txtEndTime);
            $txtStartTime.inputmask({ mask: "99:99" }).change(function () {
                $txtStartTime.val($txtStartTime.val());
                //ReValidateFieldControl("divContent", "ctl00$ctl00$cphBody$cphBody$txtEndTime");
            });
            $txtEndTime.inputmask({ mask: "99:99" }).change(function () {
                $txtEndTime.val($txtEndTime.val());
                //ReValidateFieldControl("divContent", "ctl00$ctl00$cphBody$cphBody$txtStartTime");
            });

            SetDateRangePicker($txtStartDate, $txtEndDate);
            InputMaskDecimalMinMax_Align($txtPrice, 9, 2, true, false, 0, 1000000000, true);
            InputMaskIntegerMinMax($txtAmount, 5, 0, 100000, true);

            SetAutoComplete();
            $btnBack.click(function () {
                window.Redirect('course_sub.aspx');
            });

            $btnSave.click(function () {
                SaveData();
                return false;
            });

            //$('select[id$=ddlTargetGroup]').on('change', function () {
            //    if ($(this).val()) {
            //        $('#spDesTarget').html($(this).find("option:selected").attr("title"));
            //    } else { $('#spDesTarget').html(''); }
            //});
            //$('select[id$=ddlTargetGroup]').change();

            var arrTG = GetValTextBox('hddTGID').split(',');
            $('.selectpicker').selectpicker('val', arrTG);
        }

        function SaveData() {
            if (CheckValidate('divForm')) {
                DialogConfirmSubmit(function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();

                    var obj = {
                        'nSubCourseID': +GetValTextBox('hddID'),
                        'sCompany': GetValDropdown('ddlCompany'),
                        'nCourseID': +GetValTextBox('txtCourseID'),
                        'sSubCoursename': GetValTextBox('txtSubCoursename'),
                        'sDescription': GetValTextArea('txtDescription'),
                        'sStartDate': GetValTextBox('txtStartDate'),
                        'sStartTime': GetValTextBox('txtStartTime'),
                        'sEndDate': GetValTextBox('txtEndDate'),
                        'sEndTime': GetValTextBox('txtEndTime'),
                        'lstTargetGroup': GetValDropdown('ddlTargetGroup') != null ? GetValDropdown('ddlTargetGroup') : [],
                        'sTraining_method': GetValRadioList('rdlTrainingmethod'),
                        'sPrice': GetValTextBox('txtPrice'),
                        'sAmount': (GetValTextBox('txtAmount') ? GetValTextBox('txtAmount').replace(/\,/g, '') : ""),
                        'IsActive': Boolean(+GetValRadioList('rdlActive')),
                    }

                    AjaxWebMethod("SaveData", { 'item': obj }, function (response) {
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupSessionTimeOut();
                        } else if (response.d.Status == SysProcess.Duplicate) {
                            DialogDuplicate();
                        } else if (response.d.Status == SysProcess.SaveFail) {
                            DialogSaveFail(response.d.Msg);
                        } else {
                            DialogSucessRedirect('course_sub.aspx');
                        }
                    }, UnblockUI());
                });
            }
        }

        var IsSelectedtxtCoursename = false;
        function SetAutoComplete() {
            $("input[id$=txtCoursename]")
               .on("change", function () {
                   if (!IsSelectedtxtCoursename || !IsBrowserFirefox()) {
                       $("input[id$=txtCoursename]").val("");
                       $("input[id$=txtCourseID]").val("");
                       ReValidateFieldControl("divForm", GetElementName('txtCoursename', objControl.txtbox));
                   }
               }).focus(function () {
                   IsSelectedtxtCoursename = false;
               })
           .autocomplete({
               source: function (request, response) {
                   IsSelectedtxtCoursename = false;
                   if (!IsNullOrEmpty($ddlCompany.val())) {
                       if (request.term.replace(/\s/g, "") != "" && request.term.replace(/\s/g, "").length >= nLength) {
                           AjaxWebMethod(UrlSearchEmp(), { 'sSearch': request.term, 'nCompanyID': +$ddlCompany.val() }, function (data) {
                               if (data.d.Status == SysProcess.SessionExpired) {
                                   PopupSessionTimeOut();
                               } else {
                                   UnblockUI();
                                   response($.map(data.d.lstData, function (item) {
                                       return {
                                           value: item.sName,
                                           label: item.sName,
                                           nCourseID: item.nCourseID,
                                       }
                                   }));
                               }
                           });
                       }
                   } else {
                       DialogWarning("Please Select Company");
                   }
               },
               minLength: nLength,
               select: function (event, ui) {
                   IsSelectedtxtCoursename = true;
                   $("input[id$=txtCourseID]").val(ui.item.nCourseID);
                   if (IsBrowserFirefox()) {
                       $("input[id$=txtCoursename]").blur();;
                   }
               }
           });
        }

        function UrlSearchEmp() {
            BlockUI();
            return 'GetCourse';
        }

        function DateTimeOnchange() {
            if (!IsNullOrEmpty($txtStartDate.val()) && !IsNullOrEmpty($txtEndDate.val())) {
                if (!IsNullOrEmpty($txtStartTime.val()) && !IsNullOrEmpty($txtEndTime.val())) {
                    BlockUI();
                    AjaxWebMethod('CheckDateTime', {
                        'sStartDate': GetValTextBox('txtStartDate'),
                        'sEndDate': GetValTextBox('txtEndDate'),
                        'sStartTime': GetValTextBox('txtStartTime'),
                        'sEndTime': GetValTextBox('txtEndTime'),
                    }, function (response) {
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupSessionTimeOut();
                        } else if (response.d.Status == SysProcess.Failed) {
                            DialogWarning(response.d.Msg);
                            $txtEndTime.val('');
                        } else {

                        }
                    }, UnblockUI(), null, null);
                }
            }
        }

        function ChangeCompany() {
            $("input[id$=txtCoursename]").val("");
            $("input[id$=txtCourseID]").val("");
            NOT_VALIDATED("divForm", objControl.txtbox, 'txtCoursename');
        }

    </script>
</asp:Content>

