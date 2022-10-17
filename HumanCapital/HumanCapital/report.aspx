<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="report.aspx.cs" Inherits="report" %>

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
                        <label class="col-lg-3 col-form-label">Company </label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Type</label>
                        <div class="col-lg-auto">
                            <asp:RadioButtonList ID="rdlType" runat="server" CssClass="radio" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="1" Selected="True">Quarter</asp:ListItem>
                                <asp:ListItem Value="0" class="pl-4">Year</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Year </label>
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
                        <label class="col-lg-3 col-form-label">Quarter </label>
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
        </div>
        <div class="card-footer">
            <div class="col-12 text-center">
                <button id="btnClear" type="button" class="btn btn-secondary"><i class="fa fa-undo"></i>&nbsp; Clear</button>
                <button id="btnSearch" type="button" class="btn btn-info"><i class="fa fa-search"></i>&nbsp; Search</button>
            </div>
        </div>

        <div id="divData" style="display: none; padding-top: 20px">
        </div>

    </div>

    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');

        var lstYear = [2017, 2018, 2019];
        var lstQuarter = [1, 2, 3];
        var lstDJSI = [];

        var $rdlType = $('input[name$=rdlType]');
        var $btnClear = $('button#btnClear');
        var $btnSearch = $('button#btnSearch');
        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    Setcontrol();
                    //LoadData();

                }
            }
        });

        function Setcontrol() {
            $btnSearch.click(function () { LoadData(); });
            //$rdlType.change();
            $rdlType.on('change', function () {
                var IsQuarter = +$(this).val() == 1;
                if (IsQuarter) { $('.dEnd').hide(); $('.divQuerter').show(); } else { $('.dEnd').show(); $('.divQuerter').hide(); }
            });
        }

        function LoadData(IsAuto) {
            //            'nYear': +GetValDropdown('ddlYear'),
            //'nQuarter': +GetValDropdown('ddlQuarter'),
            //if (CheckValidate('divForm')) {
            BlockUI();
            AjaxWebMethod('LoadData', {
                'nComID': +GetValDropdown('ddlCompany'),
            }, function (data) {
                if (data.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else {
                    lstDJSI = data.d.lstDJSI;
                    $('#divData').show();
                    BindDJSI();
                }
            });
            //}
        }
        function BindDJSI() {
            $('#divData').html('');

            if (lstDJSI.length > 0) {
                var lstName = [];
                var lstHead = Enumerable.From(lstDJSI).Where('$.IsHead == true').ToArray();
                var lstYear = [2019, 2020, 2021];
                for (var i = 0; i < lstHead.length; i++) {

                    // #region Gen Head
                    var qHead = lstHead[i];
                    var sHead = GetMonthByQuarter(qHead.nItem, qHead.sName, (i + 1), 1, lstYear);
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

                        var sDisabled1 = (qSub.IsAutoCal) ? " disabled " : "";
                        var sDisabled2 = hasSib ? ((qSibling_2.IsAutoCal) ? " disabled " : "") : "";
                        var sDisabled3 = IsSibling4 ? ((qSibling_3.IsAutoCal) ? " disabled " : "") : "";
                        var sDisabled4 = IsSibling4 ? ((qSibling_4.IsAutoCal) ? " disabled " : "") : "";

                        var IsRatio = nItem1 == 146;
                        var Is100 = nItem1 == 101 || nItem1 == 126 || nItem1 == 128 || nItem1 == 130 || nItem1 == 132 || nItem1 == 153;

                        var nValM1 = ' value="' + (IsRatio ? (qSub.nMale_1 || 0) : (qSub.nMale_1 || '')) + '"';
                        var nValM2 = ' value="' + (IsRatio ? (qSub.nMale_2 || 0) : (qSub.nMale_2 || '')) + '"';
                        var nValM3 = ' value="' + (IsRatio ? (qSub.nMale_3 || 0) : (qSub.nMale_3 || '')) + '"';
                        var nValF1 = ' value="' + (IsRatio ? (qSub.nFemale_1 || 0) : (qSub.nFemale_1 || '')) + '"';
                        var nValF2 = ' value="' + (IsRatio ? (qSub.nFemale_2 || 0) : (qSub.nFemale_2 || '')) + '"';
                        var nValF3 = ' value="' + (IsRatio ? (qSub.nFemale_3 || 0) : (qSub.nFemale_3 || '')) + '"';

                        //TCD = Type,Class,Disabled
                        var sTCD1 = ' type="text" class="form-control ' + (IsDecimal1 ? (IsRatio ? 'cDecimal1' : (Is100 ? 'cDecimal100' : 'cDecimal')) : 'cInt') + '"' + sDisabled1;
                        var sTCD2 = ' type="text" class="form-control ' + (IsDecimal2 ? 'cDecimal' : 'cInt') + '"' + sDisabled2;
                        var sTCD3 = ' type="text" class="form-control ' + (IsDecimal3 ? 'cDecimal' : 'cInt') + '"' + sDisabled3;
                        var sTCD4 = ' type="text" class="form-control ' + (IsDecimal4 ? 'cDecimal' : 'cInt') + '"' + sDisabled4;

                        var td1 = '';
                        var td2 = '';
                        var td3 = '';
                        var td4 = '';
                        // #endregion

                        td1 += '<td rowspan="' + nSiblingCount + '">' + (i + 1) + '.' + (ii + 1) + ' ' + qSub.sName + '</td>';

                        if (!hasSib) {
                            // #region No Sibling
                            td1 += '<td class="text-center">' + qSub.sUnit + '</td>';//Unit                                 
                            if ((qSub.IsTotal)) {
                                td1 += '<td class="text-center" colspan="2"><input id="txtM_1_' + nItem1 + '"' + sTCD1 + ' ' + nValM1 + '></td>';//Total 1
                                td1 += '<td class="text-center" colspan="2"><input id="txtM_2_' + nItem1 + '"' + sTCD1 + ' ' + nValM2 + '></td>';//Total 2
                                td1 += '<td class="text-center" colspan="2"><input id="txtM_3_' + nItem1 + '"' + sTCD1 + ' ' + nValM3 + '></td>';//Total 3

                                lstName.push({ 'sName': 'txtM_1_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtM_2_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtM_3_' + nItem1, 'IsDecimal': IsDecimal1 });
                            } else {
                                td1 += '<td class="text-center"><input id="txtM_1_' + nItem1 + '"' + sTCD1 + ' ' + nValM1 + '></td>';//Total 1.1
                                td1 += '<td class="text-center"><input id="txtF_1_' + nItem1 + '"' + sTCD1 + ' ' + nValF1 + '></td>';//Total 1.2
                                td1 += '<td class="text-center"><input id="txtM_2_' + nItem1 + '"' + sTCD1 + ' ' + nValM2 + '></td>';//Total 2.1
                                td1 += '<td class="text-center"><input id="txtF_2_' + nItem1 + '"' + sTCD1 + ' ' + nValF2 + '></td>';//Total 2.2
                                td1 += '<td class="text-center"><input id="txtM_3_' + nItem1 + '"' + sTCD1 + ' ' + nValM3 + '></td>';//Total 3.1
                                td1 += '<td class="text-center"><input id="txtF_3_' + nItem1 + '"' + sTCD1 + ' ' + nValF3 + '></td>';//Total 3.2

                                lstName.push({ 'sName': 'txtM_1_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtM_2_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtM_3_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtF_1_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtF_2_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtF_3_' + nItem1, 'IsDecimal': IsDecimal1 });
                            }
                            // #endregion
                        } else {
                            // #region Has Sibling          

                            // #region Unit
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
                            // #endregion

                            // #region Row 1
                            if ((qSub.IsTotal)) {
                                td1 += '<td class="text-center" colspan="2"><input id="txtM_1_' + nItem1 + '"' + sTCD1 + ' ' + nValM1 + '></td>';//Total 1
                                td1 += '<td class="text-center" colspan="2"><input id="txtM_2_' + nItem1 + '"' + sTCD1 + ' ' + nValM2 + '></td>';//Total 2
                                td1 += '<td class="text-center" colspan="2"><input id="txtM_3_' + nItem1 + '"' + sTCD1 + ' ' + nValM3 + '></td>';//Total 3

                                lstName.push({ 'sName': 'txtM_1_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtM_2_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtM_3_' + nItem1, 'IsDecimal': IsDecimal1 });
                            } else {
                                td1 += '<td class="text-center"><input id="txtM_1_' + nItem1 + '"' + sTCD1 + ' ' + nValM1 + '></td>';//Total 1.1
                                td1 += '<td class="text-center"><input id="txtF_1_' + nItem1 + '"' + sTCD1 + ' ' + nValF1 + '></td>';//Total 1.2
                                td1 += '<td class="text-center"><input id="txtM_2_' + nItem1 + '"' + sTCD1 + ' ' + nValM2 + '></td>';//Total 2.1
                                td1 += '<td class="text-center"><input id="txtF_2_' + nItem1 + '"' + sTCD1 + ' ' + nValF2 + '></td>';//Total 2.2
                                td1 += '<td class="text-center"><input id="txtM_3_' + nItem1 + '"' + sTCD1 + ' ' + nValM3 + '></td>';//Total 3.1
                                td1 += '<td class="text-center"><input id="txtF_3_' + nItem1 + '"' + sTCD1 + ' ' + nValF3 + '></td>';//Total 3.2          

                                lstName.push({ 'sName': 'txtM_1_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtM_2_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtM_3_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtF_1_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtF_2_' + nItem1, 'IsDecimal': IsDecimal1 });
                                lstName.push({ 'sName': 'txtF_3_' + nItem1, 'IsDecimal': IsDecimal1 });
                            }
                            // #endregion

                            // #region Row 2
                            if ((qSibling_2.IsTotal)) {
                                nValM1 = ' value="' + (qSibling_2.nMale_1 || '') + '"';
                                nValM2 = ' value="' + (qSibling_2.nMale_2 || '') + '"';
                                nValM3 = ' value="' + (qSibling_2.nMale_3 || '') + '"';
                                td2 += '<td class="text-center" colspan="2"><input id="txtM_1_' + nItem2 + '"' + sTCD2 + ' ' + nValM1 + '></td>';//Total 1
                                td2 += '<td class="text-center" colspan="2"><input id="txtM_2_' + nItem2 + '"' + sTCD2 + ' ' + nValM2 + '></td>';//Total 2
                                td2 += '<td class="text-center" colspan="2"><input id="txtM_3_' + nItem2 + '"' + sTCD2 + ' ' + nValM3 + '></td>';//Total 3

                                lstName.push({ 'sName': 'txtM_1_' + nItem2, 'IsDecimal': IsDecimal2 });
                                lstName.push({ 'sName': 'txtM_2_' + nItem2, 'IsDecimal': IsDecimal2 });
                                lstName.push({ 'sName': 'txtM_3_' + nItem2, 'IsDecimal': IsDecimal2 });
                            } else {
                                nValM1 = ' value="' + (qSibling_2.nMale_1 || '') + '"';
                                nValM2 = ' value="' + (qSibling_2.nMale_2 || '') + '"';
                                nValM3 = ' value="' + (qSibling_2.nMale_3 || '') + '"';
                                nValF1 = ' value="' + (qSibling_2.nFemale_1 || '') + '"';
                                nValF2 = ' value="' + (qSibling_2.nFemale_2 || '') + '"';
                                nValF3 = ' value="' + (qSibling_2.nFemale_3 || '') + '"';

                                td2 += '<td class="text-center"><input id="txtM_1_' + nItem2 + '"' + sTCD2 + ' ' + nValM1 + '></td>';//Total 1.1
                                td2 += '<td class="text-center"><input id="txtF_1_' + nItem2 + '"' + sTCD2 + ' ' + nValF1 + '></td>';//Total 1.2
                                td2 += '<td class="text-center"><input id="txtM_2_' + nItem2 + '"' + sTCD2 + ' ' + nValM2 + '></td>';//Total 2.1
                                td2 += '<td class="text-center"><input id="txtF_2_' + nItem2 + '"' + sTCD2 + ' ' + nValF2 + '></td>';//Total 2.2
                                td2 += '<td class="text-center"><input id="txtM_3_' + nItem2 + '"' + sTCD2 + ' ' + nValM3 + '></td>';//Total 3.1
                                td2 += '<td class="text-center"><input id="txtF_3_' + nItem2 + '"' + sTCD2 + ' ' + nValF3 + '></td>';//Total 3.2 

                                lstName.push({ 'sName': 'txtM_1_' + nItem2, 'IsDecimal': IsDecimal2 });
                                lstName.push({ 'sName': 'txtM_2_' + nItem2, 'IsDecimal': IsDecimal2 });
                                lstName.push({ 'sName': 'txtM_3_' + nItem2, 'IsDecimal': IsDecimal2 });
                                lstName.push({ 'sName': 'txtF_1_' + nItem2, 'IsDecimal': IsDecimal2 });
                                lstName.push({ 'sName': 'txtF_2_' + nItem2, 'IsDecimal': IsDecimal2 });
                                lstName.push({ 'sName': 'txtF_3_' + nItem2, 'IsDecimal': IsDecimal2 });
                            }
                            // #endregion

                            // #region Row 3-4
                            if (IsSibling4) {
                                // #region Row 3
                                if ((qSibling_3.IsTotal)) {
                                    nValM1 = ' value="' + (qSibling_3.nMale_1 || '') + '"';
                                    nValM2 = ' value="' + (qSibling_3.nMale_2 || '') + '"';
                                    nValM3 = ' value="' + (qSibling_3.nMale_3 || '') + '"';
                                    td3 += '<td class="text-center" colspan="2"><input id="txtM_1_' + nItem3 + '"' + sTCD3 + ' ' + nValM1 + '></td>';//Total 1
                                    td3 += '<td class="text-center" colspan="2"><input id="txtM_2_' + nItem3 + '"' + sTCD3 + ' ' + nValM2 + '></td>';//Total 2
                                    td3 += '<td class="text-center" colspan="2"><input id="txtM_3_' + nItem3 + '"' + sTCD3 + ' ' + nValM3 + '></td>';//Total 3

                                    lstName.push({ 'sName': 'txtM_1_' + nItem3, 'IsDecimal': IsDecimal3 });
                                    lstName.push({ 'sName': 'txtM_2_' + nItem3, 'IsDecimal': IsDecimal3 });
                                    lstName.push({ 'sName': 'txtM_3_' + nItem3, 'IsDecimal': IsDecimal3 });
                                } else {
                                    nValM1 = ' value="' + (qSibling_3.nMale_1 || '') + '"';
                                    nValM2 = ' value="' + (qSibling_3.nMale_2 || '') + '"';
                                    nValM3 = ' value="' + (qSibling_3.nMale_3 || '') + '"';
                                    nValF1 = ' value="' + (qSibling_3.nFemale_1 || '') + '"';
                                    nValF2 = ' value="' + (qSibling_3.nFemale_2 || '') + '"';
                                    nValF3 = ' value="' + (qSibling_3.nFemale_3 || '') + '"';
                                    td3 += '<td class="text-center"><input id="txtM_1_' + nItem3 + '"' + sTCD3 + ' ' + nValM1 + '></td>';//Total 1.1
                                    td3 += '<td class="text-center"><input id="txtF_1_' + nItem3 + '"' + sTCD3 + ' ' + nValF1 + '></td>';//Total 1.2
                                    td3 += '<td class="text-center"><input id="txtM_2_' + nItem3 + '"' + sTCD3 + ' ' + nValM2 + '></td>';//Total 2.1
                                    td3 += '<td class="text-center"><input id="txtF_2_' + nItem3 + '"' + sTCD3 + ' ' + nValF2 + '></td>';//Total 2.2
                                    td3 += '<td class="text-center"><input id="txtM_3_' + nItem3 + '"' + sTCD3 + ' ' + nValM3 + '></td>';//Total 3.1
                                    td3 += '<td class="text-center"><input id="txtF_3_' + nItem3 + '"' + sTCD3 + ' ' + nValF3 + '></td>';//Total 3.2

                                    lstName.push({ 'sName': 'txtM_1_' + nItem3, 'IsDecimal': IsDecimal3 });
                                    lstName.push({ 'sName': 'txtM_2_' + nItem3, 'IsDecimal': IsDecimal3 });
                                    lstName.push({ 'sName': 'txtM_3_' + nItem3, 'IsDecimal': IsDecimal3 });
                                    lstName.push({ 'sName': 'txtF_1_' + nItem3, 'IsDecimal': IsDecimal3 });
                                    lstName.push({ 'sName': 'txtF_2_' + nItem3, 'IsDecimal': IsDecimal3 });
                                    lstName.push({ 'sName': 'txtF_3_' + nItem3, 'IsDecimal': IsDecimal3 });
                                }
                                // #endregion

                                // #region Row 4                                
                                if ((qSibling_4.IsTotal)) {
                                    nValM1 = ' value="' + (qSibling_4.nMale_1 || '') + '"';
                                    nValM2 = ' value="' + (qSibling_4.nMale_2 || '') + '"';
                                    nValM3 = ' value="' + (qSibling_4.nMale_3 || '') + '"';
                                    td4 += '<td class="text-center" colspan="2"><input id="txtM_1_' + nItem4 + '"' + sTCD4 + ' ' + nValM1 + '></td>';//Total 1
                                    td4 += '<td class="text-center" colspan="2"><input id="txtM_2_' + nItem4 + '"' + sTCD4 + ' ' + nValM2 + '></td>';//Total 2
                                    td4 += '<td class="text-center" colspan="2"><input id="txtM_3_' + nItem4 + '"' + sTCD4 + ' ' + nValM3 + '></td>';//Total 3

                                    lstName.push({ 'sName': 'txtM_1_' + nItem4, 'IsDecimal': IsDecimal4 });
                                    lstName.push({ 'sName': 'txtM_2_' + nItem4, 'IsDecimal': IsDecimal4 });
                                    lstName.push({ 'sName': 'txtM_3_' + nItem4, 'IsDecimal': IsDecimal4 });
                                } else {
                                    nValM1 = ' value="' + (qSibling_4.nMale_1 || '') + '"';
                                    nValM2 = ' value="' + (qSibling_4.nMale_2 || '') + '"';
                                    nValM3 = ' value="' + (qSibling_4.nMale_3 || '') + '"';
                                    nValF1 = ' value="' + (qSibling_4.nFemale_1 || '') + '"';
                                    nValF2 = ' value="' + (qSibling_4.nFemale_2 || '') + '"';
                                    nValF3 = ' value="' + (qSibling_4.nFemale_3 || '') + '"';
                                    td4 += '<td class="text-center"><input id="txtM_1_' + nItem4 + '"' + sTCD4 + ' ' + nValM1 + '></td>';//Total 1.1
                                    td4 += '<td class="text-center"><input id="txtF_1_' + nItem4 + '"' + sTCD4 + ' ' + nValF1 + '></td>';//Total 1.2
                                    td4 += '<td class="text-center"><input id="txtM_2_' + nItem4 + '"' + sTCD4 + ' ' + nValM2 + '></td>';//Total 2.1
                                    td4 += '<td class="text-center"><input id="txtF_2_' + nItem4 + '"' + sTCD4 + ' ' + nValF2 + '></td>';//Total 2.2
                                    td4 += '<td class="text-center"><input id="txtM_3_' + nItem4 + '"' + sTCD4 + ' ' + nValM3 + '></td>';//Total 3.1
                                    td4 += '<td class="text-center"><input id="txtF_3_' + nItem4 + '"' + sTCD4 + ' ' + nValF3 + '></td>';//Total 3.2

                                    lstName.push({ 'sName': 'txtM_1_' + nItem4, 'IsDecimal': IsDecimal4 });
                                    lstName.push({ 'sName': 'txtM_2_' + nItem4, 'IsDecimal': IsDecimal4 });
                                    lstName.push({ 'sName': 'txtM_3_' + nItem4, 'IsDecimal': IsDecimal4 });
                                    lstName.push({ 'sName': 'txtF_1_' + nItem4, 'IsDecimal': IsDecimal4 });
                                    lstName.push({ 'sName': 'txtF_2_' + nItem4, 'IsDecimal': IsDecimal4 });
                                    lstName.push({ 'sName': 'txtF_3_' + nItem4, 'IsDecimal': IsDecimal4 });
                                }
                                // #endregion
                            }
                            // #endregion

                            // #endregion
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
            UnblockUI();
        }
        //sType
        //1= Quarter , 2 = Year
        function GetMonthByQuarter(nID, sTitle, nHead, sType, lst) {
            lst = [2019, 2020, 2021];
            var lst1 = ["Q1", "Q2", "Q3"];
            var th = "", rowSpan = 0, sHeadname = "", tr = "";

            switch (+sType) {
                case 1:
                    sHeadname = "Quarter";
                    rowSpan = lst1.length * 6;

                    var thSex = "", thHQ = "", thQ = "";

                    for (var i = 0; i < lst1.length; i++) {
                        thHQ += '<th class="text-center" colspan="6">' + lst1[i] + '</th>';

                        thSex += '<th style="width: 10%" class="text-center">Male</th>' +
                                 '<th style="width: 10%" class="text-center">Female</th>' +
                                 '<th style="width: 10%" class="text-center">Male</th>' +
                                 '<th style="width: 10%" class="text-center">Female</th>' +
                                 '<th style="width: 10%" class="text-center">Male</th>' +
                                 '<th style="width: 10%" class="text-center">Female</th>';

                        thQ += '<th class="text-center" colspan="2">1</th>';
                        thQ += '<th class="text-center" colspan="2">2</th>';
                        thQ += '<th class="text-center" colspan="2">3</th>';
                    }

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
                        thSex += '<th style="width: 10%" class="text-center">Male</th>';
                        thSex += '<th style="width: 10%" class="text-center">Female</th>';
                        thYear += '<th class="text-center" colspan="2">' + lst[i] + '</th>';
                    }


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
                            '<table id="tbData' + nID + '" class="table table-sm table-bordered tbData">' +
                                '<thead>' +
                                    '<tr class="valign-middle pad-primary">' +
                                        '<th class="text-center" rowspan="4">Required Data</th>' +
                                        '<th style="width: 14%" class="text-center" rowspan="4">Unit</th>' +
                                        '<th class="text-center" colspan="' + rowSpan + '">Data Collection Period</th>' +
                                    '</tr>' + tr +

            '</thead>' +
            '<tbody>';
            return th;
        }
    </script>
</asp:Content>
