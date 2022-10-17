<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="project.aspx.cs" Inherits="project" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        i.colorBlue {
            color: #5659ce;
        }
        /*Table Modal*/
        .modal-header {
            padding: 0.75rem !important;
            background-color: #5f9ea0 !important;
            color: #ffff;
        }

        table.dataTable {
            border-radius: 6px;
            width: 100%;
        }

            table.dataTable tr:nth-child(even) {
                background-color: #f9f9f9;
            }

            table.dataTable tr:nth-child(odd) {
                background-color: #ffffff;
            }

            table.dataTable thead th {
                background-color: #ecf0f8;
            }

            table.dataTable tr.odd:hover,
            table.dataTable tr.even:hover {
                background-color: #ecf0f0;
                opacity: 0.6;
            }

            table.dataTable th.dt-left,
            table.dataTable td.dt-left {
                text-align: left;
            }

            table.dataTable thead th.dt-head-center,
            table.dataTable thead td.dt-head-center,
            table.dataTable tfoot th.dt-head-center,
            table.dataTable tfoot td.dt-head-center {
                text-align: center;
            }

            table.dataTable tbody th.dt-body-left,
            table.dataTable tbody td.dt-body-left {
                text-align: left;
            }

            table.dataTable tbody th.dt-body-center,
            table.dataTable tbody td.dt-body-center {
                text-align: center;
            }

            table.dataTable th {
                font-size: 12px;
            }

            table.dataTable thead th.dt-head-center {
                vertical-align: middle;
            }

        div.csetpop {
            border-top-left-radius: 6px;
            border-top-right-radius: 6px;
            background-color: #5bc0de;
            color: #ffffff;
        }

        .modal-header {
            padding: 0.75rem !important;
        }

        .table-responsive-custom {
            overflow-x: auto;
            display: table !important;
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
                <div class="col-lg-auto col-12">
                    <div class="form-group ">
                        <a id="btnAdd" class="btn btn-success" href="project_edit.aspx" data-toggle="tooltip"><i class="fa fa-plus" aria-hidden="true"></i>&nbsp; Add</a>
                        <a href="javascript:void(0)" id="btnExport" class="btn btn-info" data-toggle="tooltip"><i class="fa fa-download" aria-hidden="true"></i>&nbsp; Export</a>
                    </div>
                    <asp:Button ID="btnExport_" Text="text" runat="server" OnClick="btnExport__Click" CssClass="hide" />
                </div>

                <div id="divSearch" class="col">
                    <div class="form-row justify-content-end">
                        <div class="col-auto">
                            <div class="form-group">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Project Name"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" Width="200px"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" placeholder="- Year -"></asp:TextBox>
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
                            <th style="width: 8%" class="text-center">
                                <asp:CheckBox ID="cbHead" runat="server" CssClass="checkbox" Text="No" />
                            </th>
                            <th style="width: 25%" class="text-center" data-sort="sProjectName">Project Name</th>
                            <th style="width: 25%" class="text-center" data-sort="sCompanyName">Company</th>
                            <th style="width: 13%" class="text-center" data-sort="sProductivityName">Actual benefits<br>
                                per year</th>
                            <th style="width: 7%" class="text-center">Course</th>
                            <th style="width: 11%" class="text-center" data-sort="sStartDate">Start Date</th>
                            <th style="width: 10%" class="text-center" data-sort="sEndDate">End Date</th>
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

    <asp:HiddenField ID="hddPermission" runat="server" />

    <%-- hdd Search Report --%>
    <asp:HiddenField ID="hddtxtName" runat="server" />
    <asp:HiddenField ID="hddCompanyID" runat="server" />
    <asp:HiddenField ID="hddYear" runat="server" />
    <asp:HiddenField ID="hddActive" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <!-- Modal -->
    <div id="MPPopContent" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="MPhTitle"></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="divMPPopContent"></div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var sPageEdit = "project_edit.aspx";
        var $Permission = GetValTextBox('hddPermission');
        var $cbHead = $('input[id$=cbHead]');
        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var $btnSearch = $('button#btnSearch');
        var $btnAdd = $('#btnAdd');
        var $btnDel = $('button#btnDel');
        var $btnExport = $('#btnExport');
        var arrData = [];
        var arrCheckBox = [];

        var $txtYear = $('input[id$=txtYear]');

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
            }
        });


        function SetControl() {
            SetYearPicker($txtYear);
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

            $btnExport.on('click', function () {
                $("input[id$=btnExport_]").click();
            });
        }

        function SearchData(pageThis) {
            var chkPageThis = pageThis != undefined ? pageThis : "1";
            pageThis = IsNullOrEmptyString(chkPageThis);

            BlockUI();
            AjaxWebMethod('Search', { 'sName': GetValTextBox('txtName'), 'sCompanyID': GetValDropdown('ddlCompany'), 'sActive': GetValDropdown('ddlActive'), 'sYear': GetValTextBox('txtYear') }, function (data) {
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
                $('input[id$=hddYear]').val(GetValTextBox('txtYear'));
                $('input[id$=hddActive]').val(GetValDropdown('ddlActive'));
            });
        }

        function SortingData(sExpression, sDirection) {
            switch (sExpression) {
                case 'sProjectName':
                case 'sCompanyName':
                case 'sProductivityName':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy('$.' + sExpression).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending('$.' + sExpression).ToArray(); })
                    break;
                case 'sStartDate':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); })
                    break;
                case 'sEndDate':
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
            var isChecked = Enumerable.From(arrCheckBox).Where(function (w) { return w.nID == objData.nProjectID }).ToArray().length > 0 ? "checked=''" : "";
            sHTML += '<td class="text-center">' +
        ($Permission == "A" ?
        ('<div class="checkbox"><input type="checkbox" name="cbRec_' + objData.nProjectID + '" id="cbRec_' + objData.nProjectID + '" value="' + objData.nProjectID + '" ' + isChecked + '/>' +
        '<label for="cbRec_' + objData.nProjectID + '">' + nRowNo + '.</label></div>') : nRowNo + '.') + '</td>';

            sHTML += "<td class='text-left'>" + objData.sProjectName + "</td>";

            sHTML += "<td class='text-left'>" + objData.sCompanyName + "</td>";

            sHTML += "<td class='text-center'>" + objData.sProductivityName + "</td>";

            //sHTML += "<td class='text-center'>" + objData.nCourse + " <i class='fas fa-th-list'></i></td>";
            sHTML += '<td class="text-center">' + (objData.nCourse != "0" ? '<a href="javascript:void(0)" onclick="ShowDetail(' + objData.nProjectID + ')">' + objData.nCourse + ' <i class="fas fa-th-list" aria-hidden="true"></i><a>' : '-') + '</td>';

            sHTML += "<td class='text-center'>" + objData.sStartDate + "</td>";

            sHTML += "<td class='text-center'>" + objData.sEndDate + "</td>";

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

        function ShowDetail(id) {
            BlockUI();
            AjaxWebMethod("Get_detail", { nProjectID: id }, function (response) {
                UnblockUI();
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else {
                    if (response.d.lstDataCourseSub.length > 0) {
                        var divData = '<table class="table dataTable table-bordered table-responsive table-hover">';
                        divData += '<thead><tr><th class="dt-head-center" style="width: 1%">No.</th><th class="dt-head-center" style="width: 14%">Sub Course Name</th></tr></thead>';
                        divData += '<tbody>'
                        var nRow = 1;
                        for (var i = 0; i < response.d.lstDataCourseSub.length; i++) {
                            divData += '<tr>'
                                    + '<td class="dt-body-center">' + nRow + '</td>'
                                    + '<td class="dt-body-left">' + response.d.lstDataCourseSub[i].sName + '</td>'
                                    + '</tr>';
                            nRow++;
                        }
                        divData += '</tbody></table>';

                        $("#divMPPopContent").html(divData);
                        $("#MPhTitle").html("<i class='glyphicon glyphicon-info-sign'></i> Course(Sub)");
                        $("#MPPopContent").modal();
                        $('#MPPopContent').on('hidden.bs.modal', function (e) {
                            $("#divMPPopContent").html("");
                        });
                        // BBox.Info("Member", divData);
                    }
                }
            }, UnblockUI);
        }
    </script>
</asp:Content>
