<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="report_1.aspx.cs" Inherits="report_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>

        <div class="card-body">
            <div class="row">
                <div id="divSearch" class="col">

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Company <span class="text-red">*</span></label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Type </label>
                        <div class="col-lg-auto">
                            <asp:RadioButtonList ID="rdlType" runat="server" CssClass="radio" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="1" Selected="True">Quarter</asp:ListItem>
                                <asp:ListItem Value="2" class="pl-4">Year</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Year <span class="text-red">*</span></label>
                        <div class="col-lg-5">
                            <div class="input-group">
                                <asp:DropDownList ID="ddlYearStr" runat="server" CssClass="form-control"></asp:DropDownList>
                                <%--<div id="divYearEnd">--%>
                                <div class="input-group-append dEnd" style="display: none">
                                    <label class="input-group-text">/</label>
                                </div>
                                <asp:DropDownList ID="ddlYearEnd" runat="server" CssClass="form-control dEnd" Style="display: none">
                                </asp:DropDownList>
                                <%--</div>--%>
                            </div>

                        </div>
                    </div>

                    <div class="form-group row divQuerter">
                        <label class="col-lg-3 col-form-label">Quarter <span class="text-red">*</span></label>
                        <div class="col-lg-5">
                            <div class="input-group">
                                <asp:DropDownList ID="ddlQuarterStr" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="">- Quarter -</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                </asp:DropDownList>
                                <div class="input-group-append">
                                    <label class="input-group-text">/</label>
                                </div>
                                <asp:DropDownList ID="ddlQuarterEnd" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="">- Quarter -</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
            <div class="clearfix"></div>

            <%--<div class="card-footer">--%>
            <div class="form-group row">
                <div class="col-12 text-center">
                    <button id="btnClear" type="button" class="btn btn-secondary"><i class="fa fa-undo"></i>&nbsp; Clear</button>
                    <button id="btnSearch" type="button" class="btn btn-info"><i class="fa fa-search"></i>&nbsp; Search</button>
                    <button id="btnExport" type="button" class="btn btn-info" style="display: none;"><i class="fa fa-download"></i>&nbsp; Export</button>
                    <asp:Button ID="btnExport_" Text="text" runat="server" OnClick="btnExport__Click" CssClass="hide" />
                </div>
            </div>
            <%--</div>--%>

            <div id="divSwitch" class="form-group row" style="display: none;">
                <div class="col-sm-5">
                    <asp:RadioButtonList ID="rdlSwitch" runat="server" CssClass="radio cRadioSw" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Value="0" Selected="True">Collapse All</asp:ListItem>
                        <asp:ListItem Value="1" class="pl-4">Expand All</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <div id="divData" style="display: none;">
            </div>
        </div>

    </div>

    <asp:HiddenField ID="hddPermission" runat="server" />
    <asp:HiddenField ID="hddCompanyID" runat="server" />
    <asp:HiddenField ID="hddType" runat="server" />
    <asp:HiddenField ID="hddYearStr" runat="server" />
    <asp:HiddenField ID="hddYearEnd" runat="server" />
    <asp:HiddenField ID="hddQuarterStr" runat="server" />
    <asp:HiddenField ID="hddQuarterEnd" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script src="Scripts/tableHeadFixer/tableHeadFixer.js"></script>
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');

        var lstDJSI = [];
        var lstQuarter_Year = [];

        var $rdlType = $('input[name$=rdlType]');
        var $btnClear = $('button#btnClear');
        var $btnSearch = $('button#btnSearch');
        var $btnExport = $('button#btnExport');
        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    Setcontrol();
                }
            }
        });

        function Setcontrol() {
            SetValidate();
            $btnSearch.click(function () { $('#btnExport').show(); LoadData(); });

            $btnExport.on('click', function () {
                $("input[id$=btnExport_]").click();
            });

            EnableValidateControl('divSearch', $('select[id$=ddlYearEnd]').attr('name'), false);
            $rdlType.on('change', function () {
                var IsQuarter = +$(this).val() == 1;
                if (IsQuarter) {
                    EnableValidateControl('divSearch', $('select[id$=ddlYearEnd]').attr('name'), false);
                    EnableValidateControl('divSearch', $('select[id$=ddlQuarterStr]').attr('name'), true);
                    EnableValidateControl('divSearch', $('select[id$=ddlQuarterEnd]').attr('name'), true);
                    $('.dEnd').hide(); $('.divQuerter').show();
                }
                else {
                    EnableValidateControl('divSearch', $('select[id$=ddlYearEnd]').attr('name'), true);
                    EnableValidateControl('divSearch', $('select[id$=ddlQuarterStr]').attr('name'), false);
                    EnableValidateControl('divSearch', $('select[id$=ddlQuarterEnd]').attr('name'), false);
                    $('.dEnd').show(); $('.divQuerter').hide();
                }
            });

            $('select[id$=ddlQuarterStr]').on('change', function () {
                QuarterChange(1);
            });

            $('select[id$=ddlQuarterEnd]').on('change', function () {
                QuarterChange(2);
            });

            $('select[id$=ddlYearStr]').on('change', function () {
                YearChange(1);
            });

            $('select[id$=ddlYearEnd]').on('change', function () {
                YearChange(2);
            });

            $btnClear.on('click', function () {
                $('select[id$=ddlCompany]').val('');
                $('select[id$=ddlYearStr]').val('');
                $('select[id$=ddlYearEnd]').val('');
                $('select[id$=ddlQuarterStr]').val('');
                $('select[id$=ddlQuarterEnd]').val('');

                NOT_VALIDATED('divSearch', 'select', 'ddlCompany');
                NOT_VALIDATED('divSearch', 'select', 'ddlYearStr');
                NOT_VALIDATED('divSearch', 'select', 'ddlYearEnd');
                NOT_VALIDATED('divSearch', 'select', 'ddlQuarterStr');
                NOT_VALIDATED('divSearch', 'select', 'ddlQuarterEnd');
            });

            $('input[id*=rdlSwitch]').on('change', function () {
                var IsExpand = +GetValRadioListNotValidate('rdlSwitch') == 1;
                $('.collapse').collapse(IsExpand ? 'show' : 'hide')
            });
        }

        function QuarterChange(Selected) {
            var id = "";
            switch (+Selected) {
                case 1: id = "ddlQuarterStr"; break;
                case 2: id = "ddlQuarterEnd"; break;
            }

            var sStr = GetValDropdown('ddlQuarterStr');
            var sEnd = GetValDropdown('ddlQuarterEnd');
            if (!IsNullOrEmpty(sStr) && !IsNullOrEmpty(sEnd)) {
                if (sStr > sEnd) {
                    $('select[id$=' + id + ']').val('');
                    ReValidateFieldControl('divSearch', $('select[id$=' + id + ']').attr("name"));
                } else if (sEnd < sStr) {
                    $('select[id$=' + id + ']').val('');
                    ReValidateFieldControl('divSearch', $('select[id$=' + id + ']').attr("name"));
                }
            }
        }

        function YearChange(Selected) {
            var id = "";
            switch (+Selected) {
                case 1: id = "ddlYearStr"; break;
                case 2: id = "ddlYearEnd"; break;
            }

            var sStr = GetValDropdown('ddlYearStr');
            var sEnd = GetValDropdown('ddlYearEnd');
            if (!IsNullOrEmpty(sStr) && !IsNullOrEmpty(sEnd)) {
                if (sStr > sEnd) {
                    $('select[id$=' + id + ']').val('');
                    ReValidateFieldControl('divSearch', $('select[id$=' + id + ']').attr("name"));
                } else if (sEnd < sStr) {
                    $('select[id$=' + id + ']').val('');
                    ReValidateFieldControl('divSearch', $('select[id$=' + id + ']').attr("name"));
                }
            }
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("ddlCompany", objControl.dropdown)] = addValidate_notEmpty("Company is required!");
            objValidate[GetElementName("ddlYearStr", objControl.dropdown)] = addValidate_notEmpty("Year Start is required!");
            objValidate[GetElementName("ddlYearEnd", objControl.dropdown)] = addValidate_notEmpty("Year End is required!");
            objValidate[GetElementName("ddlQuarterStr", objControl.dropdown)] = addValidate_notEmpty("Quarter Start is required!");
            objValidate[GetElementName("ddlQuarterEnd", objControl.dropdown)] = addValidate_notEmpty("Quarter End is required!");

            BindValidate("divSearch", objValidate);
        }

        function LoadData(IsAuto) {
            if (CheckValidate('divSearch')) {
                BlockUI();
                AjaxWebMethod('LoadData', {
                    'sComID': GetValDropdown('ddlCompany'),
                    'nType': +GetValRadioList('rdlType'),
                    'sYearStr': GetValDropdown('ddlYearStr'),
                    'sYearEnd': GetValDropdown('ddlYearEnd'),
                    'sQuarterStr': GetValDropdown('ddlQuarterStr'),
                    'sQuarterEnd': GetValDropdown('ddlQuarterEnd'),
                }, function (data) {
                    if (data.d.Status == SysProcess.SessionExpired) {
                        PopupSessionTimeOut();
                    } else {
                        lstDJSI = data.d.lstDJSI;
                        lstQuarter_Year = data.d.lstQuarter_Year;
                        $('#divData').show();
                        $('#divSwitch').show();
                        $('input[id$=rdlSwitch_0]').prop("checked", true);
                        BindDJSI();

                        $('input[id$=hddCompanyID]').val(GetValDropdown('ddlCompany'));
                        $('input[id$=hddType]').val(GetValRadioList('rdlType'));
                        $('input[id$=hddYearStr]').val(GetValDropdown('ddlYearStr'));
                        $('input[id$=hddYearEnd]').val(GetValDropdown('ddlYearEnd'));
                        $('input[id$=hddQuarterStr]').val(GetValDropdown('ddlQuarterStr'));
                        $('input[id$=hddQuarterEnd]').val(GetValDropdown('ddlQuarterEnd'));
                    }
                });
            }
        }

        function AfterCreateBindTB() {
            $('table[id^=tbData]').each(function (i, el) {
                $(this).tableHeadFixer({ "left": 2, head: true });
            })
        }

        function BindDJSI() {
            $('#divData').html('');
            var sType = +GetValRadioList('rdlType');

            if (lstDJSI.length > 0) {
                var lstName = [];
                var lstHead = Enumerable.From(lstDJSI).Where('$.IsHead == true').ToArray();
                //lstQuarter_Year = [2019, 2020, 2021, 2022];//, 2020, 2021
                for (var i = 0; i < lstHead.length; i++) {

                    // #region Gen Head
                    var qHead = lstHead[i];
                    var sHead = GetMonthByQuarter(qHead.nItem, qHead.sName, (i + 1), sType, lstQuarter_Year);
                    // #endregion                   

                    // #region Gen Sub
                    var sBody = '';
                    var lstSub = Enumerable.From(lstDJSI).Where('$.nItemHead == ' + qHead.nItem + ' && $.nSibling == null').ToArray();
                    for (var ii = 0; ii < lstSub.length; ii++) {
                        // #region Declare & Define Variable
                        var qSub = lstSub[ii];
                        var lstSibling = Enumerable.From(lstDJSI).Where('$.nSibling == ' + qSub.nItem).ToArray();
                        var hasSib = lstSibling.length > 0;
                        var nSiblingCount = (hasSib ? lstSibling.length : 0) + 1;

                        var IsSibling4 = nSiblingCount == 4;
                        var qSibling_2 = Enumerable.From(lstDJSI).FirstOrDefault(null, '$.nSibling == ' + qSub.nItem);
                        var qSibling_3 = IsSibling4 ? lstSibling[1] : null;
                        var qSibling_4 = IsSibling4 ? lstSibling[2] : null;

                        var nItem1 = qSub.nItem;
                        var nItem2 = qSibling_2 != null ? qSibling_2.nItem : 0;
                        var nItem3 = qSibling_3 != null ? qSibling_3.nItem : 0;
                        var nItem4 = qSibling_4 != null ? qSibling_4.nItem : 0;

                        var IsDecimal1 = qSub.IsDecimal;
                        var IsDecimal2 = qSibling_2 != null ? qSibling_2.IsDecimal : false;
                        var IsDecimal3 = qSibling_3 != null ? qSibling_3.IsDecimal : false;
                        var IsDecimal4 = qSibling_4 != null ? qSibling_4.IsDecimal : false;

                        var IsRatio = nItem1 == 146;
                        var Is100 = nItem1 == 101 || nItem1 == 126 || nItem1 == 128 || nItem1 == 130 || nItem1 == 132 || nItem1 == 153;

                        //TCD = Type,Class,Disabled
                        var sTCD1 = (IsDecimal1 ? (IsRatio ? 'cDecimal1' : (Is100 ? 'cDecimal100' : 'cDecimal')) : 'cInt');
                        var sTCD2 = (IsDecimal2 ? 'cDecimal' : 'cInt')
                        var sTCD3 = (IsDecimal3 ? 'cDecimal' : 'cInt');
                        var sTCD4 = (IsDecimal4 ? 'cDecimal' : 'cInt');

                        var td1 = '';
                        var td2 = '';
                        var td3 = '';
                        var td4 = '';
                        // #endregion

                        td1 += '<td rowspan="' + nSiblingCount + '">' + (i + 1) + '.' + (ii + 1) + ' ' + qSub.sName + '</td>';

                        if (!hasSib) {
                            td1 += '<td class="text-center">' + qSub.sUnit + '</td>';
                        } else {
                            if (qSub.nUnit == qSibling_2.nUnit) {
                                td1 += '<td rowspan="2" class="text-center">' + qSub.sUnit + '</td>';//Unit
                            } else {
                                td1 += '<td class="text-center">' + qSub.sUnit + '</td>';//Unit
                                td2 += '<td class="text-center">' + qSibling_2.sUnit + '</td>';//Unit
                            }

                            if (IsSibling4) {
                                if (qSibling_3.nUnit == qSibling_4.nUnit) {
                                    td3 += '<td rowspan="2" class="text-center">' + qSibling_3.sUnit + '</td>';//Unit
                                } else {
                                    td3 += '<td class="text-center">' + qSibling_3.sUnit + '</td>';//Unit
                                    td4 += '<td class="text-center">' + qSibling_4.sUnit + '</td>';//Unit
                                }
                            }
                        }

                        var nCountColumn = 0;
                        switch (sType) {
                            case 1: nCountColumn = (lstQuarter_Year.length * 3); break;
                            case 2: nCountColumn = lstQuarter_Year.length; break;
                        }

                        var Index = 0;
                        var Index_Sibling_2 = 0;
                        var Index_qSibling_3 = 0;
                        var Index_qSibling_4 = 0;
                        for (var iii = 0; iii < nCountColumn; iii++) {
                            if (!hasSib) {
                                // #region No Sibling                            
                                if ((qSub.IsTotal)) {
                                    td1 += '<td class="text-right ' + sTCD1 + '" colspan="2" id="txtM_1_' + nItem1 + '">' + qSub.lst[Index] + '</td>';//Total 1
                                } else {
                                    td1 += '<td class="text-right ' + sTCD1 + '" id="txtM_1_' + nItem1 + '">' + qSub.lst[Index] + '</td>';//Total 1.1
                                    Index++;
                                    td1 += '<td class="text-right ' + sTCD1 + '" id="txtF_1_' + nItem1 + '">' + qSub.lst[Index] + '</td>';//Total 1.2 
                                }
                                // #endregion
                            } else {
                                // #region Has Sibling    

                                // #region Row 1
                                if ((qSub.IsTotal)) {
                                    td1 += '<td class="text-right ' + sTCD1 + '" colspan="2" id="txtM_1_' + nItem1 + '">' + qSub.lst[Index] + '</td>';//Total 1
                                } else {
                                    td1 += '<td class="text-right ' + sTCD1 + '" id="txtM_1_' + nItem1 + '">' + qSub.lst[Index] + '</td>';//Total 1.1
                                    Index++;
                                    td1 += '<td class="text-right ' + sTCD1 + '" id="txtF_1_' + nItem1 + '">' + qSub.lst[Index] + '</td>';//Total 1.2
                                }
                                // #endregion

                                // #region Row 2
                                if ((qSibling_2.IsTotal)) {
                                    td2 += '<td class="text-right ' + sTCD2 + '" colspan="2" id="txtM_1_' + nItem2 + '">' + qSibling_2.lst[Index_Sibling_2] + '</td>';//Total 1
                                } else {
                                    td2 += '<td class="text-right ' + sTCD2 + '" id="txtM_1_' + nItem2 + '">' + qSibling_2.lst[Index_Sibling_2] + '</td>';//Total 1.1
                                    Index_Sibling_2++;
                                    td2 += '<td class="text-right ' + sTCD2 + '" id="txtF_1_' + nItem2 + '">' + qSibling_2.lst[Index_Sibling_2] + '</td>';//Total 1.2
                                }
                                // #endregion

                                // #region Row 3-4
                                if (IsSibling4) {
                                    // #region Row 3
                                    if ((qSibling_3.IsTotal)) {
                                        td3 += '<td class="text-right ' + sTCD3 + '" colspan="2" id="txtM_1_' + nItem3 + '">' + qSibling_3.lst[Index_qSibling_3] + '</td>';//Total 1
                                    } else {
                                        td3 += '<td class="text-right ' + sTCD3 + '" id="txtM_1_' + nItem3 + '">' + qSibling_3.lst[Index_qSibling_3] + '</td>';//Total 1.1
                                        Index_qSibling_3++;
                                        td3 += '<td class="text-right ' + sTCD3 + '" id="txtF_1_' + nItem3 + '">' + qSibling_3.lst[Index_qSibling_3] + '</td>';//Total 1.2
                                    }
                                    // #endregion

                                    // #region Row 4                                
                                    if ((qSibling_4.IsTotal)) {
                                        td4 += '<td class="text-right ' + sTCD4 + '" colspan="2" id="txtM_1_' + nItem4 + '">' + qSibling_4.lst[Index_qSibling_4] + '</td>';//Total 1
                                    } else {
                                        td4 += '<td class="text-right ' + sTCD4 + '" id="txtM_1_' + nItem4 + '">' + qSibling_4.lst[Index_qSibling_4] + '</td>';//Total 1.1
                                        Index_qSibling_4++;
                                        td4 += '<td class="text-right ' + sTCD4 + '" id="txtF_1_' + nItem4 + '">' + qSibling_4.lst[Index_qSibling_4] + '</td>';//Total 1.2
                                    }
                                    // #endregion
                                }
                                // #endregion

                                // #endregion
                            }
                            Index++;
                            Index_Sibling_2++;
                            Index_qSibling_3++;
                            Index_qSibling_4++;
                        }

                        sBody += '<tr>' + td1 + '</tr>' + (hasSib ? ('<tr>' + td2 + '</tr>') : '') + (IsSibling4 ? ('<tr>' + td3 + '</tr><tr>' + td4 + '</tr>') : '');
                    }
                    // #endregion

                    $('#divData').append(sHead + sBody + '</tbody></table></div></div></div></div>');
                }

                //Set Input Type
                InputMaskIntegerNotMoney('.cInt', 9);
                InputMaskDecimal('.cDecimal', 9, 2);
                InputMaskDecimalMinMax('.cDecimal1', 9, 2, true, false, 0, 1);
                InputMaskDecimalMinMax('.cDecimal100', 9, 2, true, false, 0, 100);
            }
            AfterCreateBindTB();
            UnblockUI();
        }
        //sType
        //1= Quarter , 2 = Year
        function GetMonthByQuarter(nID, sTitle, nHead, sType, lst) {
            var th = "", rowSpan = 0, sHeadname = "", tr = "";
            var widthTitle = 300, widthUnit = 150, widthItem = 120, widthSex = "", widthMin = "";
            var sTyleWidthTB = ""//"width: 2550px; max-width: 2550px;";
            switch (+sType) {
                case 1:
                    sHeadname = "Quarter";
                    rowSpan = lst.length * 6;

                    var thSex = "", thHQ = "", thQ = "";

                    for (var i = 0; i < lst.length; i++) {
                        thHQ += '<th class="text-center" colspan="6">Q' + lst[i] + '</th>';

                        thSex += '<th style="width: ' + widthItem + 'px" class="text-center">Male</th>' +
                                 '<th style="width: ' + widthItem + 'px" class="text-center">Female</th>' +
                                 '<th style="width: ' + widthItem + 'px" class="text-center">Male</th>' +
                                 '<th style="width: ' + widthItem + 'px" class="text-center">Female</th>' +
                                 '<th style="width: ' + widthItem + 'px" class="text-center">Male</th>' +
                                 '<th style="width: ' + widthItem + 'px" class="text-center">Female</th>';

                        var arrQ = [1, 2, 3];
                        switch (lst[i]) {
                            case 2: arrQ = [4, 5, 6]; break;
                            case 3: arrQ = [7, 8, 9]; break;
                            case 4: arrQ = [10, 11, 12]; break;
                            default: break;
                        }

                        thQ += '<th class="text-center" colspan="2">' + arrQ[0] + '</th>';
                        thQ += '<th class="text-center" colspan="2">' + arrQ[1] + '</th>';
                        thQ += '<th class="text-center" colspan="2">' + arrQ[2] + '</th>';
                    }

                    widthSex = (lst.length * 6) * widthItem;
                    sTyleWidthTB = 'width: ' + (widthTitle + widthUnit + widthSex) + 'px;';

                    tr = '<tr class="valign-middle pad-primary">' + thHQ +
                          '</tr>' +
                          '<tr class="valign-middle pad-primary">' + thQ +
                          '</tr>' +
                          '<tr class="valign-middle pad-primary">' + thSex +
                          '</tr>';
                    break;
                case 2:
                    sHeadname = "Year";
                    rowSpan = lst.length * 2;

                    var thSex = "";
                    var thYear = "";
                    for (var i = 0; i < lst.length; i++) {
                        thSex += '<th style="width: ' + widthItem + 'px" class="text-center">Male</th>';
                        thSex += '<th style="width: ' + widthItem + 'px" class="text-center">Female</th>';
                        thYear += '<th class="text-center" colspan="2">' + lst[i] + '</th>';
                    }
                    widthSex = (lst.length * 2) * widthItem;
                    sTyleWidthTB = 'width: ' + (widthTitle + widthUnit + widthSex) + 'px;';

                    tr = '<tr class="valign-middle pad-primary">' +
                            '<th class="text-center" colspan="' + rowSpan + '">' + sHeadname + '</th>' +
                         '</tr>' +
                         '<tr class="valign-middle pad-primary">' + thYear +
                         '</tr>' +
                 '<tr class="valign-middle pad-primary">' + thSex +
                 '</tr>';
                    break;
            }

            th =
            '<div class="card collapHead mb-3">' +
                '<div class="card-header collap" data-toggle="collapse" aria-expanded="false" data-target="#collapse_' + nID + '" aria-controls="collapse_' + nID + '">' +
                    '<div class="form-row">' +
                        '<b>' + nHead + '. ' + sTitle + '</b>' +
                        '<span class="ml-auto"></span>' +
                    '</div>' +
                '</div>' +
                '<div class="collapse" id="collapse_' + nID + '">' +
                    '<div class="card-body">' +
                        '<div class="table-responsive pb-3">' +
                            '<table id="tbData' + nID + '" class="table table-sm table-bordered tbData" style="' + sTyleWidthTB + '; min-width: 1096px;">' +
                                '<thead>' +
                                    '<tr class="valign-middle pad-primary">' +
                                        '<th style="width: ' + widthTitle + 'px" class="text-center" rowspan="4">Required Data</th>' +
                                        '<th style="width: ' + widthUnit + 'px" class="text-center" rowspan="4">Unit</th>' +
                                        '<th class="text-center" colspan="' + rowSpan + '">Data Collection Period</th>' +
                                    '</tr>' + tr +

            '</thead>' +
            '<tbody>';
            return th;
        }
    </script>
</asp:Content>
