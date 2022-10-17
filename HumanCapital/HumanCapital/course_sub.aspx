<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="course_sub.aspx.cs" Inherits="course_sub" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">

            <div class="row">
                <div class="col-lg-auto col-12">
                    <div class="form-group ">
                        <a id="btnAdd" class="btn btn-success" href="course_sub_edit.aspx" data-toggle="tooltip"><i class="fa fa-plus" aria-hidden="true"></i>&nbsp; Add</a>
                        <a href="javascript:void(0)" id="btnExport" class="btn btn-info" data-toggle="tooltip"><i class="fa fa-download" aria-hidden="true"></i>&nbsp; Export</a>
                    </div>
                    <asp:Button ID="btnExport_" Text="text" runat="server" OnClick="btnExport__Click" CssClass="hide" />
                </div>

                <div id="divSearch" class="col">
                    <div class="form-row justify-content-end">
                        <div class="col-auto">
                            <div class="form-group">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Course/Sub Course Name" Width="260px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-control" Width="200px"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" Width="190px"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlActive" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="- Status -"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Inactive"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <button class="btn btn-info tooltipstered" id="btnSearch" type="button" data-toggle="tooltip">
                                    <i class="fa fa-search"></i>&nbsp; Search                           
                                </button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="table-responsive">
                <table id="tbData" class="table table-bordered table-hover table-responsive-sm table-responsive-md">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 6%" class="text-center">
                                <asp:CheckBox ID="cbHead" runat="server" CssClass="checkbox" Text="No" />
                            </th>
                            <th style="width: 8%" class="text-center" data-sort="sYear">Year</th>
                            <th style="width: 23%" class="text-center" data-sort="sSubCourseName">Sub Course Name</th>
                            <th style="width: 11%" class="text-center" data-sort="sDateStart">Start Date</th>
                            <th style="width: 10%" class="text-center" data-sort="sDateEnd">End Date</th>
                            <%--<th style="width: 12%" class="text-center" data-sort="sTrainingRoom">Training Room</th>--%>
                            <th style="width: 14%" class="text-center" data-sort="sPrice">Training Cost<br> per year</th>
                            <th style="width: 12%" class="text-center" data-sort="sAmount">Number of<br> Participants</th>
                            <th style="width: 5%" class="text-center"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div id="divNoData" class="dataNotFound">No Data</div>
            </div>

            <div id="divPaging" class="form-row align-items-center pt-3">
                <div class="col-lg-2 mb-3">
                    <button type="button" id="btnDel" class="btn btn-danger" title="Delete"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp; Delete</button>
                </div>
                <div class="col-lg-8 mb-3 text-center">
                    <ul id="pagData" class="pagination small d-inline-flex"></ul>
                </div>
                <div class="col-lg-2 mb-3">
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
    <asp:HiddenField ID="hddID" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />

    <%-- hdd Search Report --%>
    <asp:HiddenField ID="hddtxtName" runat="server" />
    <asp:HiddenField ID="hddCompanyID" runat="server" />
    <asp:HiddenField ID="hddCourseID" runat="server" />
    <asp:HiddenField ID="hddActive" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var sPageEdit = "course_sub_edit.aspx";
        var $Permission = GetValTextBox('hddPermission');
        var $cbHead = $('input[id$=cbHead]');
        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var $btnSearch = $('button#btnSearch');
        var $btnAdd = $('#btnAdd');
        var $btnExport = $('#btnExport');
        var $btnSync = $('#btnSync');
        var $btnDel = $('button#btnDel');
        var arrData = [];
        var arrCheckBox = [];

        var $objPag = {};
        var $ddlPageSize = $('select[id$=ddlPageSize]');
        function SortingEvent() { }

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SearchData();
                    SortingBind($tbData, SortingData);
                }

                if ($Permission != "A") {
                    $btnAdd.remove();
                    $btnDel.remove();

                    $('#tbData thead th:first').html('No');
                }
            }
        });

        function SetControl() {
            $cbHead.change(function () {
                var isChecked = $(this).is(':checked');
                var $cbRec = $('input[id*=cbRec_]:checkbox');
                $cbRec.prop('checked', isChecked);

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

            $tbData
                .delegate('input[id^="cbRec_"]:checkbox', 'change', function () {
                    var $cbRec = $('input[id^="cbRec_"]:checkbox');
                    var $cbRec_Checked = $cbRec.filter(':checked');
                    var n_$cbRec = $cbRec.length;
                    var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_Checked.length : false;
                    $cbHead.prop('checked', isCheckedAll);
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
                .delegate('button[name=btnSearchRec]', 'click', function () {
                    if ($(this).attr('data-sub') != "0") {
                        var sOrgID = $(this).attr('data-orgid');
                        SearchData(sOrgID);
                    }
                });

            $btnSearch.click(function () { SearchData(); });

            $ddlPageSize.change(function () {
                var nPageSize = $(this).val();
                $objPag.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag.opts.page; //เลขหน้าปัจจุบัน
                ActiveDataBind(nPageSize, nPageNo);
                SetTooltip();
            });

            $btnDel.click(function () {
                var dataHasUse = false;
                var $cbRec = $('input[id^="cbRec_"]:checkbox');
                var $cbRec_Checked = $cbRec.filter(':checked');
                if ($cbRec_Checked.length > 0) {
                    DialogConfirmDel(function () {
                        BBox.ButtonEnabled(true);
                        BlockUI();
                        var arrDel = $.map($cbRec_Checked, function (cb) { return +$(cb).val(); });
                        AjaxWebMethod('Delete', { 'lstID': arrDel }, function (data) {
                            if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                                DialogDeleteSucess();
                                arrCheckBox = [];
                                SearchData();
                            }
                        }, function () { if (!dataHasUse) { BBox.Close(); } });
                    });
                }
                else DialogDeleteError();
            });

            $btnSync.click(function () {
                DialogConfirmSyncCompany(function () {
                    BlockUI();
                    AjaxWebMethod('SyncCompany', {}, function (data) {
                        if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                            DialogSucess();
                            SearchData();
                        }
                    });
                });
                return false;
            });

            $btnExport.on('click', function () {
                $("input[id$=btnExport_]").click();
            });
        }

        function SearchData(pageThis) {
            var chkPageThis = pageThis != undefined ? pageThis : "1";
            pageThis = IsNullOrEmptyString(chkPageThis);
            BlockUI();
            AjaxWebMethod('Search', { 'sName': GetValTextBox('txtName'), 'sCompanyID': GetValDropdown('ddlCompany'), 'sActive': GetValDropdown('ddlActive'), 'sCourseID': GetValDropdown('ddlCourse') }, function (data) {
                UnblockUI();
                if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                    arrData = data.d.lstData;
                    SortingClear($tbData);
                    $objPag = $('ul#pagData').paging(arrData.length, {
                        format: '[< nnncnnn >]',
                        onFormat: EasyPaging_OnFormat,
                        perpage: $ddlPageSize.val(),
                        onSelect: function (nPageNo) { //1,2,3,4,5,...
                            var nPageSize = $ddlPageSize.val();
                            ActiveDataBind(nPageSize, nPageNo);
                            SetTooltip();
                        },
                    });
                }
            }, function () {
                $("#pagData a[data-page=" + pageThis + "]").not(".next").click();
                UnblockUI();
                SetTooltip();

                $('input[id$=hddtxtName]').val(GetValTextBox('txtName'));
                $('input[id$=hddCompanyID]').val(GetValDropdown('ddlCompany'));
                $('input[id$=hddCourseID]').val(GetValDropdown('ddlCourse'));
                $('input[id$=hddActive]').val(GetValDropdown('ddlActive'));
            });
        }

        function SortingData(sExpression, sDirection) {
            switch (sExpression) {
                case 'sSubCourseName':
                case 'sDateStart':
                case 'sDateEnd':
                case 'sPrice':
                case 'sAmount':
                case 'sActive':
                case 'sYear':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy('$.' + sExpression).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending('$.' + sExpression).ToArray(); })
                    break;
                case 'sUpdateDate':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); })
                    break;
            }
            ActiveDataBind($ddlPageSize.val(), $objPag.opts.page);
        }

        function CreateDataRow(objData, nRowNo) {
            var sHTML = "";
            var isCanDel = !objData.isCanDel;
            var isChecked = Enumerable.From(arrCheckBox).Where(function (w) { return w.nID == objData.nCourseSubID }).ToArray().length > 0 ? "checked=''" : "";
            sHTML += '<td class="text-center">' +
        ($Permission == "A" ?
        ('<div class="checkbox"><input type="checkbox" name="cbRec_' + objData.nCourseSubID + '" id="cbRec_' + objData.nCourseSubID + '" value="' + objData.nCourseSubID + '" ' + isChecked + '/>' +
        '<label for="cbRec_' + objData.nCourseSubID + '">' + nRowNo + '.</label></div>') : nRowNo + '.') + '</td>';

            sHTML += "<td class='text-center'>" + objData.sYear + "</td>";
            sHTML += "<td class='text-left'>" + objData.sSubCourseName + "</td>";
            sHTML += "<td class='text-center'>" + objData.sDateStart + "</td>";
            sHTML += "<td class='text-center'>" + objData.sDateEnd + "</td>";
            //sHTML += "<td class='text-center'>" + objData.sTrainingRoom + "</td>";
            sHTML += "<td class='text-right'>" + (IsNullOrEmpty(objData.sPrice) ? "" : Set_MoneyFormat(objData.sPrice)) + "</td>";
            sHTML += "<td class='text-center'>" + (IsNullOrEmpty(objData.sAmount) ? "" : Set_MoneyFormat(objData.sAmount)) + "</td>";
            sHTML += '<td class="text-center" valign="top">' +
                    '<a class="btn btn-sm btn-outline-info"  href="' + sPageEdit + '?str=' + objData.sIDEncrypt + '" title="' + ($Permission == "A" ? 'Edit' : 'View') + '">' +
                    '<i class="fa fa-' + ($Permission == "A" ? "edit" : "eye") + '"></i>&nbsp;' +
                    '</a>' +
                    '</td>';

            var tr = "<tr>" + sHTML + "</tr>";
            return tr;
        }

        function OnDataBound() {
            CheckDataFound();
            SortingEvent();

            var $cbRec = $('input[id^="cbRec_"]:checkbox');
            var $cbRec_Checked = $cbRec.filter(':checked');
            var n_$cbRec = $cbRec.length;
            var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_Checked.length : false;
            $cbHead.prop('checked', isCheckedAll);
        }
    </script>
</asp:Content>

