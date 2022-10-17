<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_monitoring.aspx.cs" Inherits="admin_monitoring" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .circle {
            width: 30px;
            height: 30px;
            border-radius: 50%;
            padding-top: 3px;
        }

        .cNoAction {
            background-color: #e2e2e2;
        }

        .cSaveDraft {
            background-color: #b1e8f3;
        }

        .cReject {
            background-color: #ff2c2c;
        }

        .cRecall {
            background-color: #ff8d00;
        }

        .cRequestEdit {
            background-color: #FFEB3B;
        }

        .cCompleted {
            background-color: #35d659;
        }

        .ml42 {
            margin-left: 42%;
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
                <div class="col-lg-auto col-12"></div>

                <div id="divSearch" class="col">
                    <div class="form-row justify-content-end">
                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
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
                <table id="tbData" class="table table-bordered table-responsive-sm table-responsive-md">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 8%" class="text-center">No</th>
                            <th class="text-center" data-sort="sCompanyName">Company Name</th>
                            <th style="width: 12%" class="text-center">Q1</th>
                            <th style="width: 12%" class="text-center">Q2</th>
                            <th style="width: 12%" class="text-center">Q3</th>
                            <th style="width: 12%" class="text-center">Q4</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div id="divNoData" class="dataNotFound">No Data</div>
            </div>

            <div id="divPaging" class="form-row align-items-center pt-3">
                <div class="col-lg-2 mb-3"></div>
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

            <div id="divStatus" class="form-row align-items-center pt-3">
                <div class="col-sm-auto pl-3">
                    <div class="form-group">
                        <ul class="list-unstyled">
                            <li>
                                <div class="form-check-inline">
                                    <div class="circle cNoAction"></div>
                                    &emsp;No Action
                                </div>
                            </li>
                            <li>
                                <div class="form-check-inline">
                                    <div class="circle cSaveDraft"></div>
                                    &emsp;Save Draft
                                </div>
                                <li>
                                    <div class="form-check-inline">
                                        <div class="circle cReject"></div>
                                        &emsp;Reject
                                    </div>
                                </li>
                            <li>
                                <div class="form-check-inline">
                                    <div class="circle cRecall"></div>
                                    &emsp;Recall
                                </div>
                            </li>
                            <li>
                                <div class="form-check-inline">
                                    <div class="circle cRequestEdit"></div>
                                    &emsp;Request Edit
                                </div>
                            </li>
                            <li>
                                <div class="form-check-inline">
                                    <div class="circle cCompleted"></div>
                                    &emsp;Completed
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>

                <div class="col-sm-auto pl-3">
                    <div class="form-group">
                        <ul class="list-unstyled">
                            <li>L0&emsp;Data Owner</li>
                            <li>L1&emsp;Approver</li>
                            <li>L2&emsp;HR Corperate</li>
                        </ul>
                    </div>
                </div>

            </div>

        </div>
    </div>
    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $cbHead = $('input[id$=cbHead]');
        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var $btnSearch = $('button#btnSearch');
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
            }
        });

        function SetControl() {
            $btnSearch.click(function () { SearchData(); });

            $ddlPageSize.change(function () {
                var nPageSize = $(this).val();
                $objPag.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag.opts.page; //เลขหน้าปัจจุบัน
                ActiveDataBind(nPageSize, nPageNo);
                SetTooltip();
            });
        }

        function SearchData(pageThis) {
            var chkPageThis = pageThis != undefined ? pageThis : "1";
            pageThis = IsNullOrEmptyString(chkPageThis);

            BlockUI();
            AjaxWebMethod('Search', { 'sCompanyID': GetValDropdown('ddlCompany'), 'sYear': GetValDropdown('ddlYear') }, function (data) {
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
            });
        }

        function SortingData(sExpression, sDirection) {
            switch (sExpression) {
                case 'sCompanyName':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy('$.' + sExpression).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending('$.' + sExpression).ToArray(); })
                    break;
            }
            ActiveDataBind($ddlPageSize.val(), $objPag.opts.page);
        }

        function CreateDataRow(objData, nRowNo) {
            var sHTML = "";
            sHTML += "<td class='text-center'>" + nRowNo + ".</td>";
            sHTML += "<td>" + objData.sCompanyName + "</td>";
            sHTML += "<td class='text-center'>" + objData.sQuarter1 + "</td>";
            sHTML += "<td class='text-center'>" + objData.sQuarter2 + "</td>";
            sHTML += "<td class='text-center'>" + objData.sQuarter3 + "</td>";
            sHTML += "<td class='text-center'>" + objData.sQuarter4 + "</td>";

            var tr = "<tr>" + sHTML + "</tr>";
            return tr;
        }

        function OnDataBound() {
            CheckDataFound();
            SortingEvent();
        }
    </script>
</asp:Content>

