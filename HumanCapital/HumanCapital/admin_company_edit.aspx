<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_company_edit.aspx.cs" Inherits="admin_permission_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        table.radio > tbody > tr > td:last-child {
            padding-left: 20px !important;
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
                    <label class="col-lg-3 col-form-label">Company Type</label>
                    <div class="col-lg-9">
                        <asp:RadioButtonList ID="rdlCompanyType" runat="server" CssClass="radio" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="2" Enabled="false">
                        </asp:RadioButtonList>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Company Name <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" MaxLength="250" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Company Short Name <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:TextBox ID="txtCompanyAbbr" runat="server" CssClass="form-control" MaxLength="20" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Status</label>
                    <div class="col-lg-auto">
                        <asp:RadioButtonList ID="rdlActive" runat="server" CssClass="radio" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="1" Selected="True">Applicable</asp:ListItem>
                            <asp:ListItem Value="0" class="pl-4">Non applicable</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

                <div id="divDJSI" class="table-responsive" style="display: none;">
                    <label class="col-lg-12 col-form-label text-right" style="font-weight: 700; color: red;">Checked for sync data from SAP on every 1st of the month</label>
                    <table id="tbData" class="table table-bordered table-responsive-sm table-responsive-md">
                        <thead>
                            <tr class="valign-middle pad-primary">
                                <th style="width: 8%" class="text-center">
                                    <asp:CheckBox ID="cbHead" runat="server" CssClass="checkbox " Text="No" />
                                </th>
                                <th class="text-center">Required Data</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
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

    <asp:HiddenField ID="hddComID" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnSave = $('button#btnSave');
        var $btnBack = $('button#btnBack');
        var $cbHead = $('input[id$=cbHead]');
        var lstDJSI = [];
        var arrCheckBox = [];

        $(function () {
            if (!isTimeOut) {
                if ($Permission === "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SetValidate();
                    SetData();
                }

                if ($Permission != "A") {
                    $('div#divForm').find('input,select').prop('disabled', true)
                    $btnSave.remove();
                }
            }
        });

        function SetControl() {
            $cbHead.change(function () {
                var IsChecked = $(this).is(':checked');
                var $cbRec = $('input[id*=cbRec_]:checkbox');
                $cbRec.prop('checked', IsChecked);

                $('input[id*=cbRec_]:checkbox').each(function () {
                    var sVal = $(this).attr('id').split('_')[1];
                    if ($(this).is(":checked")) {
                        var obj = {
                            nID: sVal,
                        };
                        arrCheckBox.push(obj);
                    } else {
                        arrCheckBox = Enumerable.From(arrCheckBox).Where(function (w) { return w.nID != sVal }).ToArray();
                    }
                })
            });

            $('#tbData tbody').delegate('input', 'click', function () {
                var $cbRec = $('input[id^="cbRec_"]:checkbox');
                var $cbRec_Checked = $cbRec.filter(':checked');
                var n_$cbRec = $cbRec.length;
                var IsCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_Checked.length : false;
                $cbHead.prop('checked', IsCheckedAll);
            });

            $btnBack.click(function () {
                window.Redirect('admin_company.aspx');
            });

            $btnSave.click(function () {
                SaveData();
                return false;
            });
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("txtCompanyName", objControl.txtbox)] = addValidate_notEmpty("Company Name is required!");
            objValidate[GetElementName("txtCompanyAbbr", objControl.txtbox)] = addValidate_notEmpty("Company Short Name is required!");
            BindValidate("divForm", objValidate);
        }

        function SetData() {
            if (GetValRadioListNotValidate('rdlCompanyType') == "5") {
                //GC/Share Service
                AjaxWebMethod('GetData', { 'nComID': +GetValTextBox('hddComID') }, function (data) {
                    if (data.d.Status == SysProcess.SessionExpired) {
                        PopupSessionTimeOut();
                    } else {
                        lstDJSI = data.d.lstDJSI;
                        BindDJSI();
                        $('#divDJSI').show();
                        UnblockUI();
                    }
                });
            }
        }

        function BindDJSI() {
            $('#divData').html('');
            if (lstDJSI.length > 0) {
                var nCount = 0;
                var lstHead = Enumerable.From(lstDJSI).Where('$.IsHead == true').ToArray();
                for (var i = 0; i < lstHead.length; i++) {
                    var hasD = false;
                    var qHead = lstHead[i];
                    var nHead = i + 1;
                    var sHead = '<tr class="bg-info1"><td class="text-center">' + nHead + '</td><td>' + qHead.sName + '</td></tr>';

                    // #region Gen Sub         
                    var nSub = 1;
                    var sBody = '';
                    var lstSub = Enumerable.From(lstDJSI).Where('$.nItemHead == ' + qHead.nItem + ' && $.nSibling == null').ToArray();
                    for (var ii = 0; ii < lstSub.length; ii++) {
                        var hasData = false;
                        var nItem = 0;
                        var sName = '';
                        var IsChecked = '';

                        var qSub = lstSub[ii];
                        var lstSib = Enumerable.From(lstDJSI).Where('$.nItem == ' + qSub.nItem + ' || $.nSibling == ' + qSub.nItem).ToArray();
                        if (lstSib.length == 1) {
                            var qSib = lstSib[0];
                            if (qSib.IsAutoCal == false) {
                                hasD = hasData = true;
                                nItem = qSib.nItem;
                                sName = qSib.sName;
                                IsChecked = qSib.IsChecked ? "checked=''" : '';
                            }
                        } else {
                            var qSib = Enumerable.From(lstSib).FirstOrDefault(null, '$.IsAutoCal == false');
                            if (qSib != null) {
                                hasD = hasData = true;
                                nItem = qSib.nItem;
                                if (qSib.sName != "") {
                                    sName = qSib.sName;
                                    IsChecked = qSib.IsChecked ? "checked=''" : '';
                                } else {
                                    IsChecked = qSib != null && qSib.IsChecked ? "checked=''" : '';
                                    qSib = Enumerable.From(lstSib).FirstOrDefault(null, '$.IsAutoCal == true && $.sName != ""');
                                    sName = qSib != null ? qSib.sName : "";
                                }
                            }
                        }

                        if (hasData) {
                            var cb = '<td class="text-right">' +
                                     '<div class="checkbox"><input type="checkbox" name="cbRec_' + nItem + '" id="cbRec_' + nItem + '" value="' + nItem + '" ' + IsChecked + '/>' +
                                     '<label for="cbRec_' + nItem + '">' + (nHead + '.' + nSub) + '</label></div>' + '</td>';
                            sBody += '<tr>' + cb + '<td>' + sName + '</td></tr>';

                            nSub++;
                            nCount++;
                        }
                    }
                    // #endregion

                    if (hasD) $('#tbData tbody').append(sHead + sBody);
                }

                var nSelected = Enumerable.From(lstDJSI).Count('$.IsChecked == true');
                if (nCount == nSelected) $('input[id$=cbHead]').prop('checked', true);
            }
        }

        function SaveData() {
            if (CheckValidate('divForm')) {
                DialogConfirmSubmit(function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();
                    var nComType = +GetValRadioListNotValidate('rdlCompanyType');
                    var lstDJSI_ = [];
                    if (nComType == 5) {
                        var $cbRec = $('input[id^="cbRec_"]:checkbox');
                        var $cbRec_Checked = $cbRec.filter(':checked');
                        if ($cbRec_Checked.length > 0) {
                            lstDJSI_ = $.map($cbRec_Checked, function (cb) { return +$(cb).val(); });
                        }
                    }

                    var obj = {
                        'nComID': +GetValTextBox('hddComID'),
                        'nComType': nComType,
                        'sCompanyName': GetValTextBox('txtCompanyName'),
                        'sCompanyAbbr': GetValTextBox('txtCompanyAbbr'),
                        'IsActive': Boolean(+GetValRadioList('rdlActive')),
                        'lstDJSI': lstDJSI_
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
                            DialogSucessRedirect('admin_company.aspx');
                        }
                    });
                });
            }
        }
    </script>
</asp:Content>

