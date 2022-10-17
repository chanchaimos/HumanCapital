<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="project_edit.aspx.cs" Inherits="project_edit" %>

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
                    <label class="col-lg-3 col-form-label">Project Name <span class="text-red">*</span></label>
                    <div class="col-lg-6">
                        <asp:TextBox ID="txtProjectname" runat="server" CssClass="form-control" MaxLength="250" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Organization <span class="text-red">*</span></label>
                    <div class="col-lg-6">
                        <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" MaxLength="250" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Objective <span class="text-red">*</span></label>
                    <div class="col-lg-6">
                        <asp:TextBox ID="txtObjective" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Actual benefits per year </label>
                    <div class="col-lg-auto">
                        <asp:RadioButtonList ID="rdlProductivity" runat="server" CssClass="radio" RepeatDirection="Horizontal" RepeatLayout="Flow" OnChange="ProductivityChange()">
                            <asp:ListItem Value="1" Selected="True">Return</asp:ListItem>
                            <asp:ListItem Value="0" class="pl-4">Non-Monetary</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

                <div id="DivProductivity">
                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label"></label>
                        <div class="col-lg-9 row">
                            <label for="txtEconomic" class="col-lg-2 col-form-label">Economic</label>
                            <div class="col-lg-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtEconomic" runat="server" CssClass="form-control" />
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">Baht</span>
                                    </div>
                                </div>
                            </div>

                            <label for="txtSocial" class="col-sm-12 col-lg-2 col-form-label">Social</label>
                            <div class="col-lg-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtSocial" runat="server" CssClass="form-control" />
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">Baht</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label"></label>
                        <div class="col-lg-9 row">
                            <label for="txtEnvironment" class="col-lg-2 col-form-label">Environment</label>
                            <div class="col-lg-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtEnvironment" runat="server" CssClass="form-control" />
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">Baht</span>
                                    </div>
                                </div>
                            </div>

                            <label for="txtOther" class="col-lg-2 col-form-label">Other</label>
                            <div class="col-lg-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtOther" runat="server" CssClass="form-control" />
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">Baht</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="DivDescription">
                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Description <span id="spDes"></span></label>
                        <div class="col-lg-6">
                            <asp:TextBox ID="txtDes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Expense per year </label>
                    <div class="col-lg-9 row">
                        <label for="txtOpex" class="col-lg-2 col-form-label">OPEX</label>
                        <div class="col-lg-4">
                            <div class="input-group">
                                <asp:TextBox ID="txtOpex" runat="server" CssClass="form-control" />
                                <div class="input-group-prepend">
                                    <span class="input-group-text">Baht</span>
                                </div>
                            </div>
                        </div>

                        <label for="txtCapex" class="col-lg-2 col-form-label">CAPEX</label>
                        <div class="col-lg-4">
                            <div class="input-group">
                                <asp:TextBox ID="txtCapex" runat="server" CssClass="form-control" />
                                <div class="input-group-prepend">
                                    <span class="input-group-text">Baht</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Start Date</label>
                    <div class="col-lg-3">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                            </div>
                            <asp:TextBox ID="txtStartdate" runat="server" CssClass="form-control" placeholder="--/--/----" />
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">End Date</label>
                    <div class="col-lg-3">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                            </div>
                            <asp:TextBox ID="txtEnddate" runat="server" CssClass="form-control" placeholder="--/--/----" />
                        </div>
                    </div>
                </div>

                <hr />

                <div id="divCourseSub">
                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Sub Course Name  <i class="fas fa-info-circle iBlue" title="Section or Module Name"></i><span class="text-red">*</span></label>
                        <div class="col-lg-6 form-group">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text"><i class="fa fa-search"></i></span>
                                </div>
                                <asp:TextBox ID="txtCourseSubName" runat="server" CssClass="form-control" placeholder="Sub Course Name" />
                                <asp:TextBox ID="txtCourseSubID" runat="server" CssClass="form-control hide"></asp:TextBox>
                                <asp:TextBox ID="txtCompanyID" runat="server" CssClass="form-control hide"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Benefit Allocation (%) <span class="text-red">*</span></label>
                        <div class="col-lg-3">
                            <asp:TextBox ID="txtBenefit" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-lg-3"></label>
                        <div class="col-lg-9">
                            <button type="button" id="btnAddCourse" class="btn btn-info" title="Add"><i class="fa fa-plus" aria-hidden="true"></i>&nbsp; Add</button>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-md-3 col-sm-12 col-xs-12 col-form-label"></label>
                    <div class="col-md-9 col-sm-12 col-xs-12 form-group">
                        <div class="table-responsive">
                            <table id="tbData" class="table table-bordered table-hover table-responsive-sm table-responsive-md">
                                <thead>
                                    <tr class="valign-middle pad-primary">
                                        <th class="text-center" data-sort="sProjectName" style="width: 8%">No</th>
                                        <th class="text-center" data-sort="sDeptABBR">Sub Course Name</th>
                                        <th class="text-center" data-sort="nBenefit" style="width: 25%">Benefit Allocation (%)</th>
                                        <th class="text-center" data-sort="" style="width: 8%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                            <div id="divNoData" class="dataNotFound">No Data</div>
                        </div>
                        <div id="divPaging" class="form-row align-items-center padding-top-20">
                            <div class="col-md-4 mb-3">
                                <%--<button type="button" id="btnDel" class="btn btn-danger" title="Delete"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp;Delete</button>--%>
                            </div>
                            <div class="col-md-6 mb-3 d-none">
                                <ul id="pagData" class="pagination small"></ul>
                            </div>
                            <div class="col-md-2 mb-3 d-none">
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"><i class="fa fa-table"></i></span>
                                    </div>
                                    <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control height-custom"></asp:DropDownList>
                                </div>
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
                <div class="clearfix"></div>
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
    <asp:HiddenField ID="hddIsPageload" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');

        var nLength = 3;

        //Set Monetary
        var nDigit = 2;
        var nIntegerDigits = 9;

        var $txtStartdate = $('input[id$=txtStartdate]');
        var $txtEnddate = $('input[id$=txtEnddate]');
        var $ddlCompany = $('select[id$=ddlCompany]');
        var $rdlProductivity = $('input[name$=rdlProductivity]');

        var $btnAddCourse = $('button[id$=btnAddCourse]');
        var $btnBack = $('button[id$=btnBack]');
        var $btnSave = $('button[id$=btnSave]');
        var arrData = [];

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetAutoComplete();
                    SetValidate();
                    Setcontrol();
                    Getdata();
                }
            }
        });

        function Setcontrol() {

            //SetDatePicker($txtStartdate);
            //SetDatePicker($txtEnddate);
            SetDateRangePicker($txtStartdate, $txtEnddate);

            InputMaskDecimal_Align($('input[id$=txtEconomic]'), nIntegerDigits, nDigit, true, false, true);
            InputMaskDecimal_Align($('input[id$=txtSocial]'), nIntegerDigits, nDigit, true, false, true);
            InputMaskDecimal_Align($('input[id$=txtEnvironment]'), nIntegerDigits, nDigit, true, false, true);
            InputMaskDecimal_Align($('input[id$=txtOther]'), nIntegerDigits, nDigit, true, false, true);
            InputMaskDecimal_Align($('input[id$=txtOpex]'), nIntegerDigits, nDigit, true, false, true);
            InputMaskDecimal_Align($('input[id$=txtCapex]'), nIntegerDigits, nDigit, true, false, true);
            InputMaskIntegerMinMax($('input[id$=txtBenefit]'), 3, 1, 100, false);

            $('input[id$=txtBenefit]').keyup(function () {
                ReValidateFieldControl('divCourseSub', $('input[id$=txtBenefit]').attr('name'));
            });

            $btnAddCourse.on('click', function () {
                if (CheckValidate('divCourseSub')) {
                    var nCourseSubID = +GetValTextBox('txtCourseSubID');

                    var q = Enumerable.From(arrData).Where(function (w) { return w.nCourseSubID == nCourseSubID }).FirstOrDefault();
                    if (q != null) {
                        DialogDuplicate();
                    } else {
                        var nBenefit = +GetValTextBox('txtBenefit');
                        var nBenefitAll = arrData.length > 0 ? Enumerable.From(arrData).Sum('$.nBenefit') : 0;
                        debugger
                        if (nBenefit + nBenefitAll <= 100) {
                            var obj = {
                                'nCourseSubID': nCourseSubID,
                                'nCompanyID': +GetValTextBox('txtCompanyID'),
                                'sName': GetValTextBox('txtCourseSubName'),
                                'nBenefit': nBenefit
                            };

                            DialogConfirmSubmit(function () {
                                arrData.push(obj);
                                ClearCourse();
                                BindTB_Course();
                                DialogSucess();
                            });
                        } else {
                            DialogWarning("Benefit Allocation (%) of all cannot over 100 %.");
                        }                      
                    }
                }
            });

            $btnSave.on('click', function () {
                SaveData();
            });

            $ddlCompany.on('change', function () {
                var sVal = $(this).val();
                var sCompanyOld = 0;
                var q = Enumerable.From(arrData).FirstOrDefault();
                if (q != null) {
                    sCompanyOld = q.nCompanyID;
                }

                if (arrData.length > 0) {
                    BBox.Confirm(AlertTitle.Confirm, "Do you want to delete data (Courese sub)?", function () {
                        arrData = [];
                        BindTB_Course();
                        $ddlCompany.val(sVal)
                    }, $ddlCompany.val(sCompanyOld));
                } else {
                    arrData = [];
                }
            });

            ProductivityChange(Boolean(+GetValTextBox('hddIsPageload')));

            $btnBack.on('click', function () {
                window.Redirect('project.aspx');
            });

            $txtStartdate.on('changeDate', function () {
                CheckDate(1);
            });

            $txtEnddate.on('changeDate', function () {
                CheckDate(2);
            });
        }

        function Getdata() {
            BlockUI();
            AjaxWebMethod("GetData", { 'nProjectID': +GetValTextBox('hddID') }, function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else {
                    arrData = response.d.lstDataCourseSub;
                    BindTB_Course();
                }
            }, UnblockUI(), null, null);
        }

        function ProductivityChange(IsPageload) {
            var sVal = +GetValRadioList('rdlProductivity');
            switch (sVal) {
                case 1:
                    if (IsPageload) $('#DivProductivity input').prop('disabled', false);
                    else $('#DivProductivity input').prop('disabled', false);//.val('')
                    //$('div#DivDescription').hide();
                    $('#spDes').html('');
                    $('div#DivProductivity').show();
                    EnableValidateControl('divForm', $('textarea[id$=txtDes]').attr('name'), false);
                    break;
                case 0:
                    $('#DivProductivity input').prop('disabled', true);//.val('')
                    //$('div#DivDescription').show();
                    $('#spDes').append('<span class="text-red">*</span>');
                    $('div#DivProductivity').hide();
                    EnableValidateControl('divForm', $('textarea[id$=txtDes]').attr('name'), true);
                    break;
            }
            SetNotValidateTextarea('divForm', 'txtDes');
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("ddlCompany", objControl.dropdown)] = addValidate_notEmpty("Company" + IsRequire);
            objValidate[GetElementName("txtProjectname", objControl.txtbox)] = addValidate_notEmpty("Project Name" + IsRequire);
            objValidate[GetElementName("txtOrganization", objControl.txtbox)] = addValidate_notEmpty("Organization" + IsRequire);
            objValidate[GetElementName("txtObjective", objControl.txtarea)] = addValidate_notEmpty("Objective" + IsRequire);
            objValidate[GetElementName("txtDes", objControl.txtarea)] = addValidate_notEmpty("Description" + IsRequire);
            BindValidate("divForm", objValidate);

            objValidate = {};
            objValidate[GetElementName("txtCourseSubName", objControl.txtbox)] = addValidate_notEmpty("Course name" + IsRequire);
            objValidate[GetElementName("txtBenefit", objControl.txtbox)] = addValidate_notEmpty("Benefit Allocation (%)" + IsRequire);
            BindValidate("divCourseSub", objValidate);
        }

        function BindTB_Course() {
            var $table = $('table[id$=tbData]');
            var $tbody = $table.children('tbody');
            $tbody.children('tr').remove();
            var td = "", tr = "";
            if (arrData.length > 0) {
                var nRow = 1;
                $.each(arrData, function (i, el) {
                    td = '<td class="text-center">' + nRow + '.</td>';
                    td += '<td class="text-left">' + el.sName + '</td>';
                    td += '<td class="text-center">' + el.nBenefit + '</td>';
                    td += '<td class="text-center" valign="top">' + ($Permission == "A" ? '<button class="btn btn-sm btn-danger data-toggle="tooltip" title="Delete" onclick="DelCourse(' + el.nCourseSubID + ')"><i class="fa fa-trash"></i></button>' : "") + '</td>';
                    tr += "<tr>" + td + "</tr>";
                    nRow++;
                });
                $tbody.append(tr);
                $('div#divNoData').hide();
                SetTooltip();
            }
            else $('div#divNoData').show();

        }

        function DelCourse(nCourseSubID) {
            //DialogConfirmDel(function () {
            arrData = Enumerable.From(arrData).Where(function (w) { return w.nCourseSubID != nCourseSubID }).ToArray();
            BindTB_Course();
            DialogSucess();
            //  });
        }

        function ClearCourse() {
            $('input[id$=txtCourseSubName]').val('');
            $('input[id$=txtCourseSubID]').val('');
            $('input[id$=txtCompanyID]').val('');
            $('input[id$=txtBenefit]').val('');
            SetNotValidateTextbox('divCourseSub', 'txtCourseSubName');
            SetNotValidateTextbox('divCourseSub', 'txtBenefit');
        }

        var IsSelectedtxtCourseSubName = false;
        function SetAutoComplete() {
            $("input[id$=txtCourseSubName]")
               .on("change", function () {
                   if (!IsSelectedtxtCourseSubName || !IsBrowserFirefox()) {
                       $("input[id$=txtCourseSubName]").val("");
                       $("input[id$=txtCourseSubID]").val("");
                       $("input[id$=txtCompanyID]").val("");
                       //ReValidateFieldControl("divCourseSub", GetElementName('txtCourseSubName', objControl.txtbox));
                       VALIDATED("divCourseSub", objControl.txtbox, 'txtCourseSubName');
                   }
               }).focus(function () {
                   IsSelectedtxtCourseSubName = false;
               })
           .autocomplete({
               source: function (request, response) {
                   IsSelectedtxtCourseSubName = false;
                   if (request.term.replace(/\s/g, "") != "" && request.term.replace(/\s/g, "").length >= nLength) {
                       if (GetValDropdown('ddlCompany')) {
                           AjaxWebMethod(UrlSearchCourse(), { 'sSearch': request.term, 'sCompany': GetValDropdown('ddlCompany') }, function (data) {
                               if (data.d.Status == SysProcess.SessionExpired) {
                                   PopupSessionTimeOut();
                               } else {
                                   UnblockUI();
                                   response($.map(data.d.lstData, function (item) {
                                       return {
                                           value: item.sName,
                                           label: item.sName,
                                           nID: item.nCourseSubID,
                                           nCompanyID: item.nCompanyID,
                                       }
                                   }));
                               }
                           });
                       } else DialogWarning("Please choose company!");
                   }
               },
               minLength: nLength,
               select: function (event, ui) {
                   IsSelectedtxtCourseSubName = true;
                   $("input[id$=txtCourseSubID]").val(ui.item.nID);
                   $("input[id$=txtCompanyID]").val(ui.item.nCompanyID);
                   if (IsBrowserFirefox()) {
                       $("input[id$=txtCourseSubName]").blur();;
                   }
               }
           });
        }

        function UrlSearchCourse() {
            BlockUI();
            return 'GetCourse';
        }

        function SaveData() {
            var IsNone_mon = Boolean(+GetValRadioList('rdlProductivity')) == false;
            //if (IsNone_mon) {
            //    EnableValidateControl('divForm', $('input[id$=txtDes]').attr('name'), true);
            //    ReValidateFieldControl('divForm', $('input[id$=txtDes]').attr('name'));
            //} else {
            //    EnableValidateControl('divForm', $('input[id$=txtDes]').attr('name'), false);
            //    ReValidateFieldControl('divForm', $('input[id$=txtDes]').attr('name'));
            //}


            var Ispass = CheckValidate('divForm');
            var IsPass_Productivity = CheckProductivity();

            if (Ispass) {
                if (IsPass_Productivity) {
                    DialogConfirmSubmit(function () {
                        var obj = {
                            'nProjectID': +GetValTextBox('hddID'),
                            'nCompany': +GetValDropdown('ddlCompany'),
                            'sProjectname': GetValTextBox('txtProjectname'),
                            'sOrganization': GetValTextBox('txtOrganization'),
                            'sObjective': GetValTextArea('txtObjective'),
                            'nProductivity': +GetValRadioList('rdlProductivity'),
                            'sEconomic': GetValTextBox('txtEconomic').replace(/\,/g, ''),
                            'sSocial': GetValTextBox('txtSocial').replace(/\,/g, ''),
                            'sEnvironment': GetValTextBox('txtEnvironment').replace(/\,/g, ''),
                            'sOther': GetValTextBox('txtOther').replace(/\,/g, ''),
                            'sOpex': GetValTextBox('txtOpex').replace(/\,/g, ''),
                            'sCapex': GetValTextBox('txtCapex').replace(/\,/g, ''),
                            'sStartdate': GetValTextBox('txtStartdate'),
                            'sEnddate': GetValTextBox('txtEnddate'),
                            'lstCourseSub': arrData,
                            'IsActive': Boolean(+GetValRadioList('rdlActive')),
                            'sDes': GetValTextArea('txtDes'),
                        };
                        BlockUI();
                        AjaxWebMethod("SaveData", { 'item': obj }, function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupSessionTimeOut();
                            } else if (response.d.Status == SysProcess.Duplicate) {
                                DialogDuplicate();
                            } else if (response.d.Status == SysProcess.SaveFail) {
                                DialogSaveFail(response.d.Msg);
                            } else {
                                UnblockUI();
                                DialogSucessRedirect('project.aspx');
                            }
                        }, UnblockUI(), null, null);
                    });
                } else DialogWarning('Please specify at least 1 productivity.');
            }
        }

        function CheckDate(sType) {
            var sDatestr = $txtStartdate.val();
            var sDateEnd = $txtEnddate.val();
            if (sDatestr && sDateEnd) {
                BlockUI();
                AjaxWebMethod("CheckDate", { 'sDatestr': sDatestr, 'sDateEnd': sDateEnd }, function (response) {
                    if (response.d.Status == SysProcess.SessionExpired) {
                        PopupSessionTimeOut();
                    } else if (response.d.Status == SysProcess.Failed) {
                        switch (+sType) {
                            case 1: $txtStartdate.val(''); break;
                            case 2: $txtEnddate.val(''); break;
                            default: break;
                        }
                        DialogWarning(response.d.Msg);
                    }
                }, UnblockUI(), null, null);
            }
        }

        function CheckProductivity() {
            var sVal = +GetValRadioList('rdlProductivity');
            if (sVal == 1 && (GetValTextBox('txtEconomic') || GetValTextBox('txtSocial') || GetValTextBox('txtEnvironment') || GetValTextBox('txtOther'))) return true;
            else if (sVal == 0) return true;
            else return false;
        }
    </script>
</asp:Content>

