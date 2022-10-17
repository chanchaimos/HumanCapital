<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="report_2.aspx.cs" Inherits="report_2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
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
                        <%-- <span class="text-red">*</span> --%>
                        <label class="col-lg-3 col-form-label">Sub-Course Year </label>
                        <div class="col-lg-2">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                </div>
                                <asp:TextBox runat="server" ID="txtSubYear" CssClass="form-control" placeholder="----"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Project Year </label>
                        <div class="col-lg-2">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                </div>
                                <asp:TextBox runat="server" ID="txtProYear" CssClass="form-control" placeholder="----"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Sub-Course Name </label>
                        <div class="col-lg-6">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text"><i class="fa fa-search"></i></span>
                                </div>
                                <asp:TextBox ID="txtSubCoursename" runat="server" CssClass="form-control" placeholder="Search by Sub Course Name" />
                                <asp:TextBox ID="txtSubCourseID" runat="server" CssClass="form-control hide"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">Project Name </label>
                        <div class="col-lg-6">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text"><i class="fa fa-search"></i></span>
                                </div>
                                <asp:TextBox ID="txtProname" runat="server" CssClass="form-control" placeholder="Search by Project Name" />
                                <asp:TextBox ID="txtProID" runat="server" CssClass="form-control hide"></asp:TextBox>
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

            <div class="table-responsive">
                <table id="tbData" class="table table-bordered table-hover table-responsive-sm table-responsive-md">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 11%" class="text-center" data-sort="sSubC_Year">Sub-Course Year</th>
                            <th style="width: 15%" class="text-center" data-sort="sSubC_Name">Sub-Course Name</th>
                            <th style="width: 13%" class="text-center" data-sort="sTraining_Cost">Training Cost (THB/Year)</th>
                            <th style="width: 10%" class="text-center">Project Year</th>
                            <th style="width: 15%" class="text-center">Project Name</th>
                            <th style="width: 10%" class="text-center">Return/Non-Monetary (THB/Year)</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div id="divNoData" class="dataNotFound">No Data</div>
            </div>

            <div id="divPaging" class="form-row align-items-center pt-3">
                <div class="col-lg-2 mb-3">
                    <%--<button type="button" id="btnDel" class="btn btn-danger" title="Delete"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp; Delete</button>--%>
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
    <asp:HiddenField ID="hddSubYear" runat="server" />
    <asp:HiddenField ID="hddProYear" runat="server" />
    <asp:HiddenField ID="hddSubCourseID" runat="server" />
    <asp:HiddenField ID="hddProID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var nLength = 3;

        var $btnClear = $('button#btnClear');
        var $btnSearch = $('button#btnSearch');
        var $btnExport = $('button#btnExport');

        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var arrData = [];

        var $objPag = {};
        var $ddlPageSize = $('select[id$=ddlPageSize]');
        function SortingEvent() { }

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    //SetValidate();
                    Setcontrol();
                    SearchData();
                    SortingBind($tbData, SortingData);
                }
            }
        });

        function Setcontrol() {
            SetAutoCompleteSubCourse();
            SetAutoCompleteProject();
            SetYearPickerValidate('divSearch', $('input[id$=txtSubYear]'));
            SetYearPickerValidate('divSearch', $('input[id$=txtProYear]'));

            $('input[id$=txtSubYear]').inputmask({ mask: "9999" }).change(function () {
                ReValidateFieldControl("divSearch", $('input[id$=txtSubYear]').attr("name"));
            });

            $('input[id$=txtProYear]').inputmask({ mask: "9999" }).change(function () {
                ReValidateFieldControl("divSearch", $('input[id$=txtProYear]').attr("name"));
            });

            $btnClear.on('click', function () {
                $('input[id$=txtSubYear]').val('');
                $('input[id$=txtProYear]').val('');
                $('input[id$=txtSubCoursename]').val('');
                $('input[id$=txtProname]').val('');

                $('input[id$=txtSubCourseID]').val('');
                $('input[id$=txtProID]').val('');

                NOT_VALIDATED('divSearch', 'input', 'txtSubYear');
                NOT_VALIDATED('divSearch', 'input', 'txtProYear');
                NOT_VALIDATED('divSearch', 'input', 'txtSubCoursename');
                NOT_VALIDATED('divSearch', 'input', 'txtProname');
            });

            //$btnSearch.on('click', function () {
            //    if (CheckValidate('divSearch')) {
            //        SearchData();
            //    }
            //});

            $btnExport.on('click', function () {
                $("input[id$=btnExport_]").click();
            });

            $btnSearch.click(function () { SearchData(); });

            $ddlPageSize.change(function () {
                var nPageSize = $(this).val();
                $objPag.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag.opts.page; //เลขหน้าปัจจุบัน
                ActiveDataBind(nPageSize, nPageNo);
                SetTooltip();
            });
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("txtSubYear", objControl.txtbox)] = addValidate_notEmpty("Sub Course Year is required!");
            objValidate[GetElementName("txtProYear", objControl.txtbox)] = addValidate_notEmpty("Project Year is required!");
            objValidate[GetElementName("txtSubCoursename", objControl.txtbox)] = addValidate_notEmpty("Sub Course Name is required!");
            objValidate[GetElementName("txtProname", objControl.txtbox)] = addValidate_notEmpty("Project Name Start is required!");
            BindValidate("divSearch", objValidate);
        }

        function SearchData(pageThis) {
            var chkPageThis = pageThis != undefined ? pageThis : "1";
            pageThis = IsNullOrEmptyString(chkPageThis);

            BlockUI();
            var obj = {
                'sSubCourseYear': GetValTextBox('txtSubYear'),
                'sProYear': GetValTextBox('txtProYear'),
                'sSubCourseID': GetValTextBox('txtSubCourseID'),
                'sProID': GetValTextBox('txtProID'),
            };
            AjaxWebMethod('SearchData', obj, function (data) {
                UnblockUI();
                if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                    arrData = data.d.lstData;
                    SortingClear($tbData);

                    if (arrData.length > 0) { $btnExport.show(); } else { $btnExport.hide(); }

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
                $('input[id$=hddSubYear]').val(GetValTextBox('txtSubYear'));
                $('input[id$=hddProYear]').val(GetValTextBox('txtProYear'));
                $('input[id$=hddSubCourseID]').val(GetValTextBox('txtSubCourseID'));
                $('input[id$=hddProID]').val(GetValTextBox('txtProID'));

                $("#pagData a[data-page=" + pageThis + "]").not(".next").click();
                UnblockUI();
                SetTooltip();
            });
        }

        function SortingData(sExpression, sDirection) {
            switch (sExpression) {
                case 'sSubC_Year':
                case 'sSubC_Name':
                case 'sTraining_Cost':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy('$.' + sExpression).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending('$.' + sExpression).ToArray(); })
                    break;
                    //case 'sUpdateDate':
                    //    DataSort(sDirection,
                    //        function () { arrData = Enumerable.From(arrData).OrderBy(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); },
                    //        function () { arrData = Enumerable.From(arrData).OrderByDescending(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); })
                    //    break;
            }
            ActiveDataBind($ddlPageSize.val(), $objPag.opts.page);
        }

        function CreateDataRow(objData, nRowNo) {
            var sHTML = "";
            var RowSapn = objData.lstData.length == 0 ? 1 : objData.lstData.length;
            sHTML += "<td class='text-center' rowspan='" + RowSapn + "'>" + objData.sSubC_Year + "</td>";
            sHTML += "<td class='text-left' rowspan='" + RowSapn + "'>" + objData.sSubC_Name + "</td>";
            sHTML += "<td class='text-right' rowspan='" + RowSapn + "'>" + Set_MoneyFormat(objData.sTraining_Cost) + "</td>";
            if (objData.lstData.length == 0) {
                sHTML += "<td class='text-center'> </td>";
                sHTML += "<td class='text-left'> </td>";
                sHTML += "<td class='text-right'> </td>";
            } else {
                $.each(objData.lstData, function (i, el) {
                    if (i != 0) sHTML += "<tr>";
                    sHTML += "<td class='text-center'>" + el.sPro_Year + "</td>";
                    sHTML += "<td class='text-left'>" + el.sPro_Name + "</td>";
                    sHTML += "<td class='text-right'>" + Set_MoneyFormat(el.sPro_Return) + "</td>";
                    if (i != 0) sHTML += "</tr>";
                });
            }

            var tr = "<tr>" + sHTML + "</tr>";
            return tr;
        }

        function OnDataBound() {
            CheckDataFound();
            SortingEvent();
        }
    </script>

    <script type="text/javascript">
        var IsSelectedtxtSubCourseName = false;
        function SetAutoCompleteSubCourse() {
            $("input[id$=txtSubCoursename]")
               .on("change", function () {
                   if (!IsSelectedtxtSubCourseName || !IsBrowserFirefox()) {
                       $("input[id$=txtSubCoursename]").val("");
                       $("input[id$=txtSubCourseID]").val("");
                       //ReValidateFieldControl("divCourseSub", GetElementName('txtSubCoursename', objControl.txtbox));
                       VALIDATED("divSearch", objControl.txtbox, 'txtSubCoursename');
                   }
               }).focus(function () {
                   IsSelectedtxtSubCourseName = false;
               })
           .autocomplete({
               source: function (request, response) {
                   IsSelectedtxtSubCourseName = false;
                   if (request.term.replace(/\s/g, "") != "" && request.term.replace(/\s/g, "").length >= nLength) {
                       AjaxWebMethod(UrlSearchSubCourse(), { 'sSearch': request.term }, function (data) {
                           if (data.d.Status == SysProcess.SessionExpired) {
                               PopupSessionTimeOut();
                           } else {
                               UnblockUI();
                               response($.map(data.d.lstData, function (item) {
                                   return {
                                       value: item.sName,
                                       label: item.sName,
                                       nID: item.nCourseSubID,
                                   }
                               }));
                           }
                       });
                   }
               },
               minLength: nLength,
               select: function (event, ui) {
                   IsSelectedtxtSubCourseName = true;
                   $("input[id$=txtSubCourseID]").val(ui.item.nID);
                   if (IsBrowserFirefox()) {
                       $("input[id$=txtSubCoursename]").blur();;
                   }
               }
           });
        }

        function UrlSearchSubCourse() {
            BlockUI();
            return 'GetSubCourse';
        }

        var IsSelectedtxtProjectName = false;
        function SetAutoCompleteProject() {
            $("input[id$=txtProname]")
               .on("change", function () {
                   if (!IsSelectedtxtProjectName || !IsBrowserFirefox()) {
                       $("input[id$=txtProname]").val("");
                       $("input[id$=txtProID]").val("");
                       //ReValidateFieldControl("divCourseSub", GetElementName('txtProname', objControl.txtbox));
                       VALIDATED("divSearch", objControl.txtbox, 'txtProname');
                   }
               }).focus(function () {
                   IsSelectedtxtProjectName = false;
               })
           .autocomplete({
               source: function (request, response) {
                   IsSelectedtxtProjectName = false;
                   if (request.term.replace(/\s/g, "") != "" && request.term.replace(/\s/g, "").length >= nLength) {
                       AjaxWebMethod(UrlSearchProject(), { 'sSearch': request.term }, function (data) {
                           if (data.d.Status == SysProcess.SessionExpired) {
                               PopupSessionTimeOut();
                           } else {
                               UnblockUI();
                               response($.map(data.d.lstData, function (item) {
                                   return {
                                       value: item.sProjectName,
                                       label: item.sProjectName,
                                       nID: item.nProjectID,
                                   }
                               }));
                           }
                       });
                   }
               },
               minLength: nLength,
               select: function (event, ui) {
                   IsSelectedtxtProjectName = true;
                   $("input[id$=txtProID]").val(ui.item.nID);
                   if (IsBrowserFirefox()) {
                       $("input[id$=txtProname]").blur();;
                   }
               }
           });
        }

        function UrlSearchProject() {
            BlockUI();
            return 'GetProject';
        }
    </script>
</asp:Content>

