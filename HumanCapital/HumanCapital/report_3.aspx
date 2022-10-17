<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="report_3.aspx.cs" Inherits="report_3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
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
            <div class="row">
                <div id="divSearch" class="col">

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Year <span class="text-red">*</span></label>
                        <div class="col-lg-3">
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Company <span class="text-red">*</span></label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control selectpicker" multiple data-actions-box="true">
                            </asp:DropDownList>
                        </div>
                    </div>

                </div>
            </div>
            <div class="clearfix"></div>

            <%--<div class="card-footer">--%>
            <div class="form-group row">
                <div class="col-12 text-center">
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
    <asp:HiddenField ID="hddYear" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script src="Scripts/tableHeadFixer/tableHeadFixer.js"></script>
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnSearch = $('button#btnSearch');
        var $btnExport = $('button#btnExport');

        var lstDJSI = [];
        var lstCompanyName = [];

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetValidate();
                    SetControl();
                }
            }
        });

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("ddlCompany", objControl.dropdown)] = addValidate_notEmpty("Company is required!");
            objValidate[GetElementName("ddlYear", objControl.dropdown)] = addValidate_notEmpty("Year is required!");

            BindValidate("divSearch", objValidate);
        }

        function SetControl() {
            BindSelectpicker();

            $btnSearch.click(function () { $btnExport.show(); LoadData(); });

            $btnExport.on('click', function () {
                $("input[id$=btnExport_]").click();
            });

            $('input[id*=rdlSwitch]').on('change', function () {
                var IsExpand = +GetValRadioListNotValidate('rdlSwitch') == 1;
                $('.collapse').collapse(IsExpand ? 'show' : 'hide')
            });
        }

        function LoadData() {
            if (CheckValidate('divSearch')) {
                BlockUI();
                var arrComID = GetValDropdown('ddlCompany') != null ? GetValDropdown('ddlCompany') : [];
                AjaxWebMethod('LoadData', {
                    'arrComID': arrComID,
                    'nYear': +GetValDropdown('ddlYear')
                }, function (data) {
                    if (data.d.Status == SysProcess.SessionExpired) {
                        PopupSessionTimeOut();
                    } else {
                        lstDJSI = data.d.lstDJSI;
                        lstCompanyName = data.d.lstCompanyName;
                        $('#divData').show();
                        $('#divSwitch').show();
                        $('input[id$=rdlSwitch_0]').prop("checked", true);
                        BindDJSI();

                        $('input[id$=hddCompanyID]').val(arrComID.join(','));
                        $('input[id$=hddYear]').val(GetValDropdown('ddlYear'));
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
                for (var i = 0; i < lstHead.length; i++) {
                    // #region Gen Head
                    var qHead = lstHead[i];
                    var sHead = GetMonthByQuarter(qHead.nItem, qHead.sName, (i + 1));
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

                        var nCountColumn = lstCompanyName.length;

                        if (!hasSib) {
                            // #region No Sibling                            
                            if ((qSub.IsTotal)) {
                                for (var x = 0; x < nCountColumn * 2; x = x += 2) {
                                    td1 += '<td class="text-right ' + sTCD1 + '" colspan="2" id="txtM_1_' + nItem1 + '">' + qSub.lstData[x] + '</td>';
                                }
                            } else {
                                $.each(qSub.lstData, function (i, el) { td1 += '<td class="text-right ' + sTCD1 + '" id="txtM_1_' + nItem1 + '">' + el + '</td>'; });
                            }
                            // #endregion
                        } else {
                            // #region Has Sibling    

                            // #region Row 1
                            if ((qSub.IsTotal)) {
                                for (var x = 0; x < nCountColumn * 2; x = x += 2) {
                                    td1 += '<td class="text-right ' + sTCD1 + '" colspan="2" id="txtM_1_' + nItem1 + '">' + qSub.lstData[x] + '</td>';
                                }
                            } else {
                                $.each(qSub.lstData, function (i, el) { td1 += '<td class="text-right ' + sTCD1 + '" id="txtM_1_' + nItem1 + '">' + el + '</td>'; });
                            }
                            // #endregion

                            // #region Row 2
                            if ((qSibling_2.IsTotal)) {
                                for (var x = 0; x < nCountColumn * 2; x = x += 2) {
                                    td2 += '<td class="text-right ' + sTCD2 + '" colspan="2" id="txtM_1_' + nItem2 + '">' + qSibling_2.lstData[x] + '</td>';
                                }
                            } else {
                                $.each(qSibling_2.lstData, function (i, el) { td2 += '<td class="text-right ' + sTCD2 + '" id="txtM_1_' + nItem2 + '">' + el + '</td>'; });
                            }
                            // #endregion

                            // #region Row 3-4
                            if (IsSibling4) {
                                // #region Row 3
                                if ((qSibling_3.IsTotal)) {
                                    for (var x = 0; x < nCountColumn * 2; x = x += 2) {
                                        td3 += '<td class="text-right ' + sTCD3 + '" colspan="2" id="txtM_1_' + nItem3 + '">' + qSibling_3.lstData[x] + '</td>';
                                    }
                                } else {
                                    $.each(qSibling_3.lstData, function (i, el) { td3 += '<td class="text-right ' + sTCD3 + '" id="txtM_1_' + nItem3 + '">' + el + '</td>'; });
                                }
                                // #endregion

                                // #region Row 4                                
                                if ((qSibling_4.IsTotal)) {
                                    for (var x = 0; x < nCountColumn * 2; x = x += 2) {
                                        td4 += '<td class="text-right ' + sTCD4 + '" colspan="2" id="txtM_1_' + nItem4 + '">' + qSibling_4.lstData[x] + '</td>';
                                    }
                                } else {
                                    $.each(qSibling_4.lstData, function (i, el) { td4 += '<td class="text-right ' + sTCD4 + '" id="txtM_1_' + nItem4 + '">' + el + '</td>'; });
                                }
                                // #endregion
                            }
                            // #endregion

                            // #endregion
                        }

                        sBody += '<tr>' + td1 + '</tr>' + (hasSib ? ('<tr>' + td2 + '</tr>') : '') + (IsSibling4 ? ('<tr>' + td3 + '</tr><tr>' + td4 + '</tr>') : '');
                    }
                    //#endregion

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

        function GetMonthByQuarter(nID, sTitle, nHead) {
            var th = "", colspan = 0, tr = "";
            var widthTitle = 300, widthUnit = 150, widthItem = 120;
            colspan = lstCompanyName.length * 2;

            var sCom = "";
            $.each(lstCompanyName, function (i, el) {
                sCom += '<th class="text-center" colspan="2">' + el + '</th>'
            });
            tr += '<tr class="valign-middle pad-primary">' + sCom + '</tr>';

            var sGender = "";
            $.each(lstCompanyName, function (i, el) {
                sGender += '<th style="width: ' + widthItem + 'px" class="text-center">Male</th>'
                        + '<th style="width: ' + widthItem + 'px" class="text-center">Female</th>';
            });
            tr += '<tr class="valign-middle pad-primary">' + sGender + '</tr>';

            var sWidth = 'width: ' + (widthTitle + widthUnit + (widthItem * colspan)) + 'px;';

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
                            '<table id="tbData' + nID + '" class="table table-sm table-bordered tbData" style="' + sWidth + ' min-width: 1096px;">' +
                                '<thead>' +
                                    '<tr class="valign-middle pad-primary">' +
                                        '<th style="width: ' + widthTitle + 'px" class="text-center" rowspan="3">Required Data</th>' +
                                        '<th style="width: ' + widthUnit + 'px" class="text-center" rowspan="3">Unit</th>' +
                                        '<th class="text-center" colspan="' + colspan + '">Data Collection Period</th>' +
                                    '</tr>' + tr +

            '</thead>' +
            '<tbody>';
            return th;
        }
    </script>
</asp:Content>
