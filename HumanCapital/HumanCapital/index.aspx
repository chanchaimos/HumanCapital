<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        #boxChart {
            position: relative;
            /*float: right;*/
            display: block;
            /* width: 350px; */
            margin-top: 20px;
            z-index: 1;
            /*-webkit-box-shadow: 0px 0px 10px 0px rgba(0, 0, 0, 0.75);
            -moz-box-shadow: 0px 0px 10px 0px rgba(0, 0, 0, 0.75);
            box-shadow: 0px 0px 10px 0px rgba(0, 0, 0, 0.75);*/
        }

        .box-stat {
            width: 100%;
            max-width: 100%;
            margin-bottom: 20px;
        }

            .box-stat > .box-item {
                /*display: flex;*/
                /*display: inline-grid;*/
            }

                .box-stat > .box-item + .box-item {
                    border-top: 1px solid #7092b7;
                }

                .box-stat > .box-item > .box-title {
                    min-width: 120px;
                    padding: 0 10px;
                    line-height: 1;
                    text-align: center;
                    margin: 20px 0;
                }

                    .box-stat > .box-item > .box-title > .title-tag {
                        /*padding: 15px 10px;
                        text-align: left;
                        color: #ffffff;
                        background-color: #bd2130;*/
                    }

                        .box-stat > .box-item > .box-title > .title-tag > .tag-label-name {
                            font-weight: 500;
                            /*font-size: 1.7em;*/
                        }

                        .box-stat > .box-item > .box-title > .title-tag > .tag-label-amount {
                            margin-top: 2px;
                            font-size: 1em;
                            font-weight: bold;
                        }

                        .box-stat > .box-item > .box-title > .title-tag > .tag-label-unit {
                            margin-top: 5px;
                            font-size: 0.8em;
                        }

                    .box-stat > .box-item > .box-title > .title-label {
                        display: inline-block;
                        text-align: left;
                        color: #516979;
                        margin: 20px 0;
                    }

                        .box-stat > .box-item > .box-title > .title-label > .label-name {
                            font-size: 1em;
                        }

                        .box-stat > .box-item > .box-title > .title-label > .label-date {
                            font-size: 0.8em;
                        }

                .box-stat > .box-item > .box-body {
                    /*flex-basis: 100%;
                    border-left: 1px solid #7092b7;*/
                    flex-basis: 100%;
                    /* border-left: 1px solid #7092b7; */
                    display: block;
                    padding: 10px;
                    text-align: center;
                    background-color: rgba(239, 238, 238, 0.5);
                    border-radius: 15px;
                }

                    .box-stat > .box-item > .box-body > .box-chart {
                        display: block;
                        width: 460px;
                        height: 300px;
                        font-size: 0.8em;
                    }

        .titleGraph {
            text-align: center !important;
            font-size: 16px;
            font-weight: 700;
            padding-bottom: 25px;
            padding-top: 25px;
        }

        .dataNotFoundGraph {
            text-align: center !important;
            font-size: 15px;
            color: red;
        }

        .IconBlue {
            /*color: #3030b9ab;*/
            color: rgb(63,80,232);
            cursor: pointer;
            font-size: 14px;
        }
    </style>
    <style type="text/css">
        i.colorBlue {
            color: #5659ce;
        }
        /*Table Modal*/
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

        .modal-header {
            padding: 0.75rem !important;
            background-color: #5f9ea0 !important;
            color: #ffff;
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
                    <div class="form-row justify-content-end">
                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0" Text="Quarter"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Year"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
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
            <div class="clearfix"></div>
            <div class="table-responsive">
                <table id="tbData" class="table table-bordered table-hover table-responsive-sm table-responsive-md">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 25%" class="text-center" data-sort="">Q1</th>
                            <th style="width: 25%" class="text-center" data-sort="">Q2</th>
                            <th style="width: 25%" class="text-center" data-sort="">Q3</th>
                            <th style="width: 25%" class="text-center" data-sort="">Q4</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div id="divNoData" class="dataNotFound">No Data</div>
            </div>

            <div id="boxChart">
                <div class="row justify-content-center">
                    <%--<label class="form-label">As of Quarter : </label>--%>
                    <label id="spYear"></label>
                </div>
                <div class="row justify-content-center">
                    <div class="col-xl-6 col-lg-6 cGraph1">
                        <div class="box-stat">
                            <div class="box-item">
                                <div class="box-title">
                                    <div class="title-tag">
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div id="chart1" class="box-chart"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 cGraph2">
                        <div class="box-stat">
                            <div class="box-item">
                                <div class="box-title">
                                    <div class="title-tag">
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div id="chart2" class="box-chart"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row justify-content-center">
                    <div class="col-xl-6 col-lg-6 cGraph1">
                        <div class="box-stat">
                            <div class="box-item">
                                <div class="box-title">
                                    <div class="title-tag">
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div id="chart3" class="box-chart"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 cGraph2">
                        <div class="box-stat">
                            <div class="box-item">
                                <div class="box-title">
                                    <div class="title-tag">
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div id="chart4" class="box-chart"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hddPermission" runat="server" />
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
    <!-- amcharts_3.20.3 -->
    <script type="text/javascript" src="Scripts/amcharts_3.20.3/amcharts/amcharts.js"></script>
    <script type="text/javascript" src="Scripts/amcharts_3.20.3/amcharts/pie.js"></script>
    <script type="text/javascript" src="Scripts/amcharts_3.20.3/amcharts/serial.js"></script>
    <script type="text/javascript" src="Scripts/amcharts_3.20.3/amcharts/themes/light.js"></script>

    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $btnSearch = $('button#btnSearch');
        var arrData = [];
        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                }
            }
        });

        function SetControl() {
            SearchData();
            $btnSearch.click(function () { SearchData(); });
        }

        function SearchData() {
            BlockUI();
            AjaxWebMethod('Search', { 'sComID': GetValDropdown('ddlCompany'), 'sYear': GetValDropdown('ddlYear'), 'cType': GetValDropdown('ddlType') }, function (data) {
                UnblockUI();
                if (data.d.Status == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                    arrData = data.d.lstData;
                    BindData_Quarter(arrData);
                    var sType = GetValDropdown('ddlType') == "0" ? "Quarter" : "Year";
                    $('label#spYear').html('<b>As of ' + sType + ' : </b>' + (data.d.Content || "-"));
                    if (data.d.lstDash_1.length > 0) BuildStackedChart(chart1, data.d.lstDash_1, "Total Worker");
                    else $('#chart1').html(''); $('#chart1').append('<div class="titleGraph">Worker : Person</div><div class="dataNotFoundGraph" style="">No Data</div>');

                    if (data.d.lstDash_2.length > 0) BuildStackedChart(chart2, data.d.lstDash_2, "Employee by Area");
                    else $('#chart2').html(''); $('#chart2').append('<div class="titleGraph">Employee by Area</div><div class="dataNotFoundGraph" style="">No Data</div>');

                    if (data.d.lstDash_3.length > 0) BuildStackedChart(chart3, data.d.lstDash_3, "Employee by Age group");
                    else $('#chart3').html(''); $('#chart3').append('<div class="titleGraph">Employee by Age group</div><div class="dataNotFoundGraph" style="">No Data</div>');

                    if (data.d.lstDash_4.length > 0) BuildStackedChart(chart4, data.d.lstDash_4, "New/Turnover");
                    else $('#chart4').html(''); $('#chart4').append('<div class="titleGraph">New/Turnover</div><div class="dataNotFoundGraph" style="">No Data</div>');

                }
            }, function () {
                UnblockUI();
            });
        }

        function BindData_Quarter(arr) {
            var sHTML = "", tr = "";
            var $tbody = $tbData.children('tbody');
            $tbody.children('tr').remove();
            //if (arr.length > 0) {
            for (var i = 1; i <= 4; i++) {
                var qData = Enumerable.From(arr).Where(function (w) { return w.nQuarter == i }).FirstOrDefault();
                if (qData != null) sHTML += "<td class='text-center'>" + qData.sStatus + ((qData.sMg) ? " <a onclick='Comment(" + qData.nQuarter + ")'><i class='fas fa-comment IconBlue'></i></a>" : "") + "</td>";
                else sHTML += "<td class='text-center'>N/A</td>";
            }
            tr += "<tr> " + sHTML + "</tr>";
            $tbody.append(tr);
            $divNoData.hide('fast').css('display', 'none');
            //} else {
            //    $divNoData.show('fast').css('display', '');
            //}
        }

        function Comment(sVal) {
            var qData = Enumerable.From(arrData).Where(function (w) { return w.nQuarter == sVal }).FirstOrDefault();
            if (qData != null) {
                //DialogShowDetail((qData.sMg || '-'));
                $("#divMPPopContent").html((qData.sMg || ''));
                $("#MPhTitle").html("<i class='glyphicon glyphicon-info-sign'></i> Detail");
                $("#MPPopContent").modal();
                $('#MPPopContent').on('hidden.bs.modal', function (e) {
                    $("#divMPPopContent").html("");
                });
            }
        }
    </script>
    <!-- Chart code -->
    <script type="text/javascript">
        function BuildStackedChart(divGh, datasource, chartTitle) {
            var chart = AmCharts.makeChart(divGh, {
                "type": "serial",
                "theme": "light",
                "depth3D": 20,
                "angle": 30,
                "legend": {
                    "horizontalGap": 10,
                    "useGraphSettings": true,
                    "markerSize": 10
                },
                "dataProvider": datasource,
                "valueAxes": [{
                    "stackType": "regular",
                    //"axisAlpha": 0,
                    //"gridAlpha": 0
                }],
                "titles": [{ text: '' + chartTitle, size: 14 }],
                "graphs": [{
                    "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
                    "fillAlphas": 0.8,
                    "labelText": "[[value]]",
                    "lineAlpha": 0.3,
                    "title": "Male",
                    "type": "column",
                    "newStack": true,
                    "color": "#000000",
                    "valueField": "sValue1",
                    "labelPosition": "top",
                }, {
                    "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
                    "fillAlphas": 0.8,
                    "labelText": "[[value]]",
                    "lineAlpha": 0.3,
                    "title": "Female",
                    "type": "column",
                    "newStack": true,
                    "color": "#000000",
                    "valueField": "sValue2",
                    "labelPosition": "top",
                }],

                "categoryField": "sName",
                "categoryAxis": {
                    "gridPosition": "start",
                    "axisAlpha": 0,
                    "gridAlpha": 0,
                    "position": "left"
                },
                "export": {
                    "enabled": true
                }

            });
        }
    </script>

</asp:Content>
