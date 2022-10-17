<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="report_djsi_edit.aspx.cs" Inherits="report_djsi_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .btn-requestedit {
            color: #212529;
            background-color: #FFEB3B;
            border-color: #FFEB3B;
        }

            .btn-requestedit:hover {
                background-color: #d8c731;
                border-color: #d8c731;
            }

            .btn-requestedit:focus, .btn-requestedit.focus {
                box-shadow: 0 0 0 0.2rem #d8c731;
            }

            .btn-requestedit.disabled, .btn-requestedit:disabled {
                background-color: #e6d543;
                border-color: #e6d543;
            }

            .btn-requestedit:not(:disabled):not(.disabled):active, .btn-requestedit:not(:disabled):not(.disabled).active,
            .show > .btn-requestedit.dropdown-toggle {
                background-color: #FFEB3B;
                border-color: #FFEB3B;
            }

                .btn-requestedit:not(:disabled):not(.disabled):active:focus, .btn-requestedit:not(:disabled):not(.disabled).active:focus,
                .show > .btn-requestedit.dropdown-toggle:focus {
                    box-shadow: 0 0 0 0.2rem #FFEB3B;
                }

        .bgHead {
            background-color: #fbdbcf;
            font-weight: bold;
        }

        .tbData, .tbData .form-control {
            font-size: 12px !important;
        }

            .tbData input {
                text-align: right;
            }

        .cCustom {
            flex: 1 1 auto;
            padding: 0.5rem !important;
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
                    <label class="col-lg-3 col-form-label">Company <i class="fas fa-info-circle iBlue" title="Budget Owner"></i><span class="text-red">*</span></label>
                    <div class="col-lg-5">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Year/Quarter <span class="text-red">*</span></label>
                    <div class="col-lg-5">
                        <div class="input-group">
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
                            <div class="input-group-append">
                                <label class="input-group-text">/</label>
                            </div>
                            <asp:DropDownList ID="ddlQuarter" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">- Quarter -</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div id="divImportFile" class="form-group row" style="display: none;">
                    <label class="col-lg-3 col-form-label">
                        Import File<br />
                        <a href="UploadFiles/Template.xlsx">[Download Template]</a></label>
                    <div class="col-lg-5">
                        <div id="divFile">
                            <input type="file" name="files" id="txtFile" accept="application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                            <span class="text-muted">* File size limits up to 10MB., Allowed file types: .xls .xlsx</span>
                        </div>
                    </div>
                    <div id="divFileBtn" class="col-lg-auto" style="display: none;">
                        <button id="btnViewFile" type="button" class="btn btn-info" title="Download"><span class="glyphicon glyphicon-zoom-in"></span></button>
                        <button id="btnDelFile" type="button" class="btn btn-danger" title="Delete" data-toggle="tooltip"><span class="fa fa-trash"></span></button>
                    </div>
                    <div id="divBtnFile" class="col-lg-auto"></div>
                </div>

                <%--Document--%>
                <div class="form-group row">
                    <label class="col-sm-3 col-form-label">Attach File </label>
                    <div class="col-sm-9">
                        <div id="divFileDocument">
                            <input type="file" name="files" id="txtFileDocument" multiple="multiple" accept="application/pdf,application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,
                                    application/vnd.ms-powerpoint,
    application/vnd.openxmlformats-officedocument.presentationml.slideshow,
    application/vnd.openxmlformats-officedocument.presentationml.presentation" />
                            <span class="text-muted font-12">* File size limits up to 10MB. Allowed file types: .doc .docx .xls .xlsx .pdf .ppt .pptx</span>
                        </div>
                        <div id="divTB_FileDocument" class="pt-3 table-responsive">
                            <table id="tbData_FileDocument" class="table table-bordered table-responsive-sm table-responsive-md">
                                <thead class="thead-dark darkCustom">
                                    <tr>
                                        <th class="text-center" width="10%">
                                            <asp:CheckBox ID="cbHead_DM" runat="server" CssClass="checkbox" Text="No" /></th>
                                        <th class="text-center">File Name</th>
                                        <th class="text-center" width="40%">Document Description</th>
                                        <th class="text-center" width="10%">View</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <div id="divPaging_DM" class="form-row align-items-center padding-top-20">
                            <div class="col-md-4 mb-3">
                                <button type="button" id="btnDel_DM" class="btn btn-danger" title="Delete"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp;Delete</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label"></label>
                    <div class="col-lg-5">
                        <button id="btnLoadData" type="button" class="btn btn-info"><i class="fa fa-save"></i>&nbsp; Load Data</button>
                    </div>
                </div>

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

                <div id="divDataError" style="display: none;">
                    <div class="table-responsive">
                        <table id="tbData" class="table table-bordered table-hover table-responsive-sm table-responsive-md">
                            <thead>
                                <tr class="valign-middle pad-primary">
                                    <th style="width: 10%" class="text-center" data-sort="">Row Excel</th>
                                    <th style="width: 80%" class="text-center" data-sort="">Message Error</th>
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

                <div id="divHistory" class="card collapHead mb-3" style="display: none;">
                    <div class="card-header collap" data-toggle="collapse" aria-expanded="false" data-target="#collapse_Log" aria-controls="collapse_Log">
                        <div class="form-row">
                            <b>Transaction history</b>
                            <span class="ml-auto"></span>
                        </div>
                    </div>
                    <div class="collapse" id="collapse_Log">
                        <div class="card-body">
                            <div id="divTB_Log" class="pt-3 table-responsive">
                                <table id="tbData_Log" class="table table-bordered">
                                    <thead class="pad-info">
                                        <tr class="valign-middle">
                                            <th class="text-center" style="width: 5%">No</th>
                                            <th class="text-center" style="width: 25%">Status</th>
                                            <th class="text-center" style="width: 25%">Action By</th>
                                            <th class="text-center" style="width: 15%">Action Date</th>
                                            <th class="text-center">Comment</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                                <div id="divNoData_log" class="dataNotFound">No Data</div>

                            </div>
                            <div id="divPaging_log" class="form-row align-items-center pt-3">
                                <div class="col-lg-2 mb-3">
                                    <button type="button" id="btnDel" class="btn btn-danger" title="Delete" style="display: none"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp; Delete</button>
                                </div>
                                <div class="col-lg-8 mb-3 text-center">
                                    <ul id="pagData_log" class="pagination small d-inline-flex"></ul>
                                </div>
                                <div class="col-lg-2 mb-3">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-table"></i></span>
                                        </div>
                                        <asp:DropDownList ID="ddlPageSize_log" runat="server" CssClass="form-control height-custom"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%--style="display: none;"--%>
                <div id="divComment" class="form-group row pl-3" style="display: none;">
                    <label class="col-lg-3 col-form-label">Comment <span class="text-red">*</span></label>
                    <div class="col-lg-8">
                        <asp:TextBox ID="txtComment" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine" disabled="true"></asp:TextBox>
                    </div>
                </div>

            </div>
        </div>
        <div class="card-footer">
            <div class="col-12 text-center">
                <button id="btnBack" type="button" class="btn btn-inverse"><i class="fa fa-arrow-left"></i>&nbsp; Back</button>
                <button id="btnSaveDraft" type="button" class="btn btn-info btnAction" style="display: none;"><i class="fa fa-save"></i>&nbsp; Save Draft</button>
                <button id="btnSubmit" type="button" class="btn btn-primary btnAction" style="display: none;"><i class="fa fa-save"></i>&nbsp; Submit</button>
                <button id="btnApprove" type="button" class="btn btn-primary btnAction" style="display: none;"><i class="fa fa-save"></i>&nbsp; Approve</button>
                <button id="btnReject" type="button" class="btn btn-danger btnAction" style="display: none;"><i class="fa fa-times"></i>&nbsp; Reject</button>
                <button id="btnRecall" type="button" class="btn btn-warning btnAction" style="display: none;"><i class="fa fa-save"></i>&nbsp; Recall</button>
                <button id="btnRequestEdit" type="button" class="btn btn-requestedit btnAction" style="display: none;"><i class="fa fa-save"></i>&nbsp; Request Edit</button>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hddUserID" runat="server" />
    <asp:HiddenField ID="hddReportID" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />
    <asp:HiddenField ID="hddIsL0" runat="server" />
    <asp:HiddenField ID="hddIsL1" runat="server" />
    <asp:HiddenField ID="hddIsL2" runat="server" />
    <asp:HiddenField ID="hddStatusID" runat="server" />
    <asp:HiddenField ID="hddIsCompanyInternal" runat="server" />
    <asp:HiddenField ID="hddFile" runat="server" />
    <asp:HiddenField ID="hddIsLoadData" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $hddIsLoadData = $('input[id$=hddIsLoadData]'); $hddIsLoadData.val(0);
        var IsDisabled = false;

        var $btnLoadData = $('button#btnLoadData');
        var $btnSaveDraft = $('button#btnSaveDraft');
        var $btnSubmit = $('button#btnSubmit');
        var $btnRecall = $('button#btnRecall');
        var $btnReject = $('button#btnReject');
        var $btnRequestEdit = $('button#btnRequestEdit');
        var $btnBack = $('button#btnBack');

        var $btnDelFile = $('button#btnDelFile');

        /* Table Data error */
        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');

        var $objPag = {};
        var $ddlPageSize = $('select[id$=ddlPageSize]');
        function SortingEvent() { }
        /* End Table Data error */

        /* Table Data LOG */
        var $tbData_Log = $('table#tbData_Log');
        var $divNoData_log = $('div#divNoData_log');
        var $divPaging_log = $('div#divPaging_log');

        var $objPag_log = {};
        var $ddlPageSize_log = $('select[id$=ddlPageSize_log]');
        /* End Table Data LOG */


        /* For Check PRMS */
        var IsNew = !Boolean(+GetValTextBox('hddReportID'));
        var $L0 = $('input[id$=hddIsL0]');
        var $L1 = $('input[id$=hddIsL1]');
        var $L2 = $('input[id$=hddIsL2]');
        var $hddStatusID = $('input[id$=hddStatusID]');
        /* ---  End For Check PRMS ----- */
        var $hddIsCompanyInternal = $('input[id$=hddIsCompanyInternal]');

        var lstDJSI = [];
        var lstLog = [];

        $(function () {
            if (!isTimeOut) {

                if ($Permission === "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SetValidate();
                    if (GetValTextBox('hddReportID') != "") {
                        $('#divHistory').show();
                        BlockUI();
                        $('select[id$=ddlCompany],select[id$=ddlYear],select[id$=ddlQuarter]');
                        $('select[id$=ddlCompany]').change();
                        LoadData(true);
                        SetData();
                        obj_File = JSON.parse(GetValTextBox('hddFile'));
                        BindFile(false);
                    } else {
                        CheckPRMS();
                        $('.btnAction').hide();
                    }
                }
                if ($Permission != "A") {
                    SetPRMS_View();
                }
            }
        });

        function SetControl() {
            $('#divData').delegate('input', 'blur', function () {
                CalDJSI(true);
            });

            $('input[id*=rdlSwitch]').on('change', function () {
                var IsExpand = +GetValRadioListNotValidate('rdlSwitch') == 1;
                $('.collapse').collapse(IsExpand ? 'show' : 'hide')
            });

            /* Button Action */
            $btnLoadData.click(function () {
                LoadData(false);
                return false;
            });
            $btnSaveDraft.click(function () {
                SaveData("");
                return false;
            });
            $btnSubmit.click(function () {
                SaveData(1);
                return false;
            });
            $btnRecall.click(function () {
                SaveData(2);
                return false;
            });
            $btnRequestEdit.click(function () {
                SaveData(3);
                return false;
            });
            $btnReject.click(function () {
                SaveData(4);
                return false;
            });
            /*End Button Action */

            $('select[id$=ddlCompany]').on('change', function () {
                BlockUI();
                AjaxWebMethod("CompanyChange", { 'nComID': +GetValDropdown('ddlCompany') }, function (data) {
                    if (data.d.Status == SysProcess.SessionExpired) {
                        PopupSessionTimeOut();
                    } else {
                        if (data.d.Content == "6") {
                            $hddIsCompanyInternal.val(0);
                            $('#divImportFile').show(); $('#btnLoadData').hide();
                        } else {
                            $hddIsCompanyInternal.val(1);
                            $('#divDataError').hide(); $('#divImportFile').hide(); $('#btnLoadData').show();
                        }
                    }
                }, UnblockUI(), null, null);
            });

            $btnBack.on('click', function () {
                window.location = "report_djsi.aspx";
            });

            $ddlPageSize.change(function () {
                var nPageSize = $(this).val();
                $objPag.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag.opts.page; //เลขหน้าปัจจุบัน
                ActiveDataBind_Rev(nPageSize, nPageNo, arrData_Error, $tbData, CreateDataRow, OnDataBound);
                SetTooltip();
            });

            $ddlPageSize_log.change(function () {
                var nPageSize = $(this).val();
                $objPag_log.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag_log.opts.page; //เลขหน้าปัจจุบัน
                ActiveDataBind_Rev(nPageSize, nPageNo, lstLog, $tbData_Log, CreateDataRowLOG, OnDataBoundLOG);
                SetTooltip();
            });
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("ddlCompany", objControl.dropdown)] = addValidate_notEmpty("Company is required!");
            objValidate[GetElementName("ddlYear", objControl.dropdown)] = addValidate_notEmpty("Year is required!");
            objValidate[GetElementName("ddlQuarter", objControl.dropdown)] = addValidate_notEmpty("Quarter is required!");
            BindValidate("divForm", objValidate);

            objValidate = {};
            objValidate[GetElementName("txtComment", objControl.txtarea)] = addValidate_notEmpty("Comment is required!");
            BindValidate("divComment", objValidate);
        }

        function SetPRMS_View() {            
            IsDisabled = true;
            $('div#divForm').find('input,select').not('select[id$=ddlPageSize_log]').not('input[name$=rdlSwitch]').prop('disabled', true);
            //$('div#divForm').find('input,select').not('input[name$=rdlSwitch]').prop('disabled', true)
            $('div#divComment').find('textarea').prop('disabled', true);
            $("input[id$=txtFile]").parent().addClass('fileuploader-disabled');
            $btnSaveDraft.remove();
            $btnSubmit.remove();
            $btnRecall.remove();
            $btnRequestEdit.remove();
            $btnReject.remove();
            $btnDelFile.remove();
            $btnLoadData.hide();

            //Table File Document
            $('table#tbData_FileDocument').find('textarea').prop('disabled', true);
            $("input[id$=txtFileDocument]").parent().addClass('fileuploader-disabled');
            $('#tbData_FileDocument th:nth-child(1),#tbData_FileDocument td:nth-child(1)').not("table#tbData_FileDocument tr[id$=trFileNoData] td").remove()
            $('button#btnDel_DM').hide();
        }

        function Disabled_FileDocument() {
            //Table File Document
            $('table#tbData_FileDocument').find('textarea').prop('disabled', true);
            $("input[id$=txtFileDocument]").parent().addClass('fileuploader-disabled');
            $('#tbData_FileDocument th:nth-child(1),#tbData_FileDocument td:nth-child(1)').not("table#tbData_FileDocument tr[id$=trFileNoData] td").remove()
            $('button#btnDel_DM').hide();
        }

        function CheckPRMS() {            
            if (IsNew) {
                $btnSaveDraft.show();
                $btnSubmit.show();
            } else {
                switch (+$hddStatusID.val()) {
                    case 0://L0 Save Draft
                        if (Boolean(+$L0.val())) {
                            $btnSaveDraft.show();
                            $btnSubmit.show();
                        } else {
                            SetPRMS_View();
                        }
                        break;
                    case 1://Waiting L1 Approve
                        IsDisabled = true;
                        if (Boolean(+$L1.val())) {
                            Disabled_FileDocument();
                            $btnDelFile.remove();
                            $btnLoadData.hide();
                            $('div#divData').find('input,select').prop('disabled', true);
                            $btnSubmit.show();
                            $btnReject.show();
                            $btnSubmit.html('<i class="fas fa-check"></i> Approve');

                            $('#divComment').show();
                            $('div#divComment').find('textarea').prop('disabled', false);
                        } else if (Boolean(+$L0.val())) {
                            Disabled_FileDocument();
                            $btnLoadData.hide();
                            $btnDelFile.remove();
                            $('div#divData').find('input,select').prop('disabled', true);
                            $btnRecall.show();
                            $('#divComment').show();
                            $('div#divComment').find('textarea').prop('disabled', false);
                        } else {
                            SetPRMS_View();
                        }
                        break;
                    case 2://Waiting L2 Approve
                        IsDisabled = true;
                        if (Boolean(+$L2.val())) {
                            //ถ้า ให้ L2 แก้ไขข้อมูลได้ Comment 3 บรรทัดด้านล่าง
                            Disabled_FileDocument(); // - 1
                            $btnDelFile.remove();// - 2
                            $('div#divData').find('input,select').prop('disabled', true);// - 3

                            $btnLoadData.hide();
                            $('div#divComment').find('textarea').prop('disabled', false);
                            $('#divComment').show();
                            $btnSubmit.show();
                            $btnSubmit.html('<i class="fas fa-check"></i> Acknowledge');
                            $btnReject.show();
                        } else if (Boolean(+$L0.val())) {
                            Disabled_FileDocument();
                            $btnLoadData.hide();
                            $btnDelFile.remove();
                            $('div#divData').find('input,select').prop('disabled', true);
                            $btnRequestEdit.show();

                            $('#divComment').show();
                            $('div#divComment').find('textarea').prop('disabled', false);

                        } else {
                            SetPRMS_View();
                        }
                        break;
                    case 3://Completed
                        IsDisabled = true;
                        if (Boolean(+$L0.val())) {
                            Disabled_FileDocument();
                            $btnLoadData.hide();
                            $btnDelFile.remove();
                            $('div#divData').find('input,select').prop('disabled', true);
                            $btnRequestEdit.show();
                            $('#divComment').show();
                            $('div#divComment').find('textarea').prop('disabled', false);

                        } else { SetPRMS_View(); }
                        break;
                    case 4://L0 Recall from Waiting L1 Approve
                    case 5://L0 Request Edit from Waiting L2 Approve
                    case 6://L0 Request Edit from Completed
                        if (Boolean(+$L0.val())) {
                            $btnLoadData.hide();
                            $btnSubmit.show();
                        } else { SetPRMS_View(); }
                        break;
                    case 7://L1 Reject
                    case 8://L2 Reject
                        if ($L0) {
                            $btnSaveDraft.show();
                            $btnSubmit.show();
                        } else {
                            SetPRMS_View();
                        }
                        break;
                }
            }
        }

        function LoadData(IsAuto) {
            if (CheckValidate('divForm')) {
                BlockUI();
                AjaxWebMethod('LoadData', {
                    'nReportID': +GetValTextBox('hddReportID'),
                    'nComID': +GetValDropdown('ddlCompany'),
                    'nYear': +GetValDropdown('ddlYear'),
                    'nQuarter': +GetValDropdown('ddlQuarter'),
                    'IsAuto': IsAuto,
                    'lstDataDJSI': arrData,
                }, function (data) {
                    if (data.d.Status == SysProcess.SessionExpired) {
                        PopupSessionTimeOut();
                    } else if (data.d.Status == SysProcess.Duplicate) {
                        DialogDuplicate();
                        $('#divData').hide(); $('#divSwitch').hide(); $('.btnAction').hide();
                    } else {
                        if (IsNew) CheckPRMS();
                        $hddIsLoadData.val(1);
                        lstDJSI = data.d.lstDJSI;
                        $('#divData').show(); $('#divSwitch').show(); $('input[id$=rdlSwitch_0]').prop("checked", true);
                        BindDJSI();
                        if (!IsAuto) CalDJSI(false);                       
                    }
                }, UnblockUI());
            }
        }

        function BindDJSI() {
            $('#divData').html('');
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

                        var sDisabled1 = (qSub.IsAutoCal) || IsDisabled ? " disabled " : "";
                        var sDisabled2 = hasSib ? ((qSibling_2.IsAutoCal) || IsDisabled ? " disabled " : "") : "";
                        var sDisabled3 = IsSibling4 ? ((qSibling_3.IsAutoCal) || IsDisabled ? " disabled " : "") : "";
                        var sDisabled4 = IsSibling4 ? ((qSibling_4.IsAutoCal) || IsDisabled ? " disabled " : "") : "";

                        var IsRatio = nItem1 == 146;
                        var Is100 = nItem1 == 101 || nItem1 == 126 || nItem1 == 128 || nItem1 == 130 || nItem1 == 132 || nItem1 == 153;

                        var nValM1 = ' value="' + (IsRatio ? (qSub.nMale_1 || 0) : (qSub.nMale_1 != null ? qSub.nMale_1 : '')) + '"';
                        var nValM2 = ' value="' + (IsRatio ? (qSub.nMale_2 || 0) : (qSub.nMale_2 != null ? qSub.nMale_2 : '')) + '"';
                        var nValM3 = ' value="' + (IsRatio ? (qSub.nMale_3 || 0) : (qSub.nMale_3 != null ? qSub.nMale_3 : '')) + '"';
                        var nValF1 = ' value="' + (IsRatio ? (qSub.nFemale_1 || 0) : (qSub.nFemale_1 != null ? qSub.nFemale_1 : '')) + '"';
                        var nValF2 = ' value="' + (IsRatio ? (qSub.nFemale_2 || 0) : (qSub.nFemale_2 != null ? qSub.nFemale_2 : '')) + '"';
                        var nValF3 = ' value="' + (IsRatio ? (qSub.nFemale_3 || 0) : (qSub.nFemale_3 != null ? qSub.nFemale_3 : '')) + '"';

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
                            nValM1 = ' value="' + (qSibling_2.nMale_1 != null ? qSibling_2.nMale_1 : '') + '"';
                            nValM2 = ' value="' + (qSibling_2.nMale_2 != null ? qSibling_2.nMale_2 : '') + '"';
                            nValM3 = ' value="' + (qSibling_2.nMale_3 != null ? qSibling_2.nMale_3 : '') + '"';
                            if ((qSibling_2.IsTotal)) {
                                td2 += '<td class="text-center" colspan="2"><input id="txtM_1_' + nItem2 + '"' + sTCD2 + ' ' + nValM1 + '></td>';//Total 1
                                td2 += '<td class="text-center" colspan="2"><input id="txtM_2_' + nItem2 + '"' + sTCD2 + ' ' + nValM2 + '></td>';//Total 2
                                td2 += '<td class="text-center" colspan="2"><input id="txtM_3_' + nItem2 + '"' + sTCD2 + ' ' + nValM3 + '></td>';//Total 3

                                lstName.push({ 'sName': 'txtM_1_' + nItem2, 'IsDecimal': IsDecimal2 });
                                lstName.push({ 'sName': 'txtM_2_' + nItem2, 'IsDecimal': IsDecimal2 });
                                lstName.push({ 'sName': 'txtM_3_' + nItem2, 'IsDecimal': IsDecimal2 });
                            } else {
                                nValF1 = ' value="' + (qSibling_2.nFemale_1 != null ? qSibling_2.nFemale_1 : '') + '"';
                                nValF2 = ' value="' + (qSibling_2.nFemale_2 != null ? qSibling_2.nFemale_2 : '') + '"';
                                nValF3 = ' value="' + (qSibling_2.nFemale_3 != null ? qSibling_2.nFemale_3 : '') + '"';

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
                                nValM1 = ' value="' + (qSibling_3.nMale_1 != null ? qSibling_3.nMale_1 : '') + '"';
                                nValM2 = ' value="' + (qSibling_3.nMale_2 != null ? qSibling_3.nMale_2 : '') + '"';
                                nValM3 = ' value="' + (qSibling_3.nMale_3 != null ? qSibling_3.nMale_3 : '') + '"';
                                // #region Row 3
                                if ((qSibling_3.IsTotal)) {
                                    td3 += '<td class="text-center" colspan="2"><input id="txtM_1_' + nItem3 + '"' + sTCD3 + ' ' + nValM1 + '></td>';//Total 1
                                    td3 += '<td class="text-center" colspan="2"><input id="txtM_2_' + nItem3 + '"' + sTCD3 + ' ' + nValM2 + '></td>';//Total 2
                                    td3 += '<td class="text-center" colspan="2"><input id="txtM_3_' + nItem3 + '"' + sTCD3 + ' ' + nValM3 + '></td>';//Total 3

                                    lstName.push({ 'sName': 'txtM_1_' + nItem3, 'IsDecimal': IsDecimal3 });
                                    lstName.push({ 'sName': 'txtM_2_' + nItem3, 'IsDecimal': IsDecimal3 });
                                    lstName.push({ 'sName': 'txtM_3_' + nItem3, 'IsDecimal': IsDecimal3 });
                                } else {
                                    nValF1 = ' value="' + (qSibling_3.nFemale_1 != null ? qSibling_3.nFemale_1 : '') + '"';
                                    nValF2 = ' value="' + (qSibling_3.nFemale_2 != null ? qSibling_3.nFemale_2 : '') + '"';
                                    nValF3 = ' value="' + (qSibling_3.nFemale_3 != null ? qSibling_3.nFemale_3 : '') + '"';
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
                                nValM1 = ' value="' + (qSibling_4.nMale_1 != null ? qSibling_4.nMale_1 : '') + '"';
                                nValM2 = ' value="' + (qSibling_4.nMale_2 != null ? qSibling_4.nMale_2 : '') + '"';
                                nValM3 = ' value="' + (qSibling_4.nMale_3 != null ? qSibling_4.nMale_3 : '') + '"';
                                if ((qSibling_4.IsTotal)) {
                                    td4 += '<td class="text-center" colspan="2"><input id="txtM_1_' + nItem4 + '"' + sTCD4 + ' ' + nValM1 + '></td>';//Total 1
                                    td4 += '<td class="text-center" colspan="2"><input id="txtM_2_' + nItem4 + '"' + sTCD4 + ' ' + nValM2 + '></td>';//Total 2
                                    td4 += '<td class="text-center" colspan="2"><input id="txtM_3_' + nItem4 + '"' + sTCD4 + ' ' + nValM3 + '></td>';//Total 3

                                    lstName.push({ 'sName': 'txtM_1_' + nItem4, 'IsDecimal': IsDecimal4 });
                                    lstName.push({ 'sName': 'txtM_2_' + nItem4, 'IsDecimal': IsDecimal4 });
                                    lstName.push({ 'sName': 'txtM_3_' + nItem4, 'IsDecimal': IsDecimal4 });
                                } else {
                                    nValF1 = ' value="' + (qSibling_4.nFemale_1 != null ? qSibling_4.nFemale_1 : '') + '"';
                                    nValF2 = ' value="' + (qSibling_4.nFemale_2 != null ? qSibling_4.nFemale_2 : '') + '"';
                                    nValF3 = ' value="' + (qSibling_4.nFemale_3 != null ? qSibling_4.nFemale_3 : '') + '"';
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

        function GetMonthByQuarter(nID, sTitle, nHead) {
            var th = "";
            var arr = [];
            var nQuarter = +GetValDropdown('ddlQuarter')
            switch (nQuarter) {
                case 1: arr = ["Jan", "Feb", "Mar"]; break;
                case 2: arr = ["Apr", "May", "Jun"]; break;
                case 3: arr = ["Jul", "Aug", "Sep"]; break;
                default: arr = ["Oct", "Nov", "Dec"]; break;
            }

            if (arr.length == 3) {
                th =
                '<div class="card collapHead mb-3">' +
                    '<div class="card-header collap" data-toggle="collapse" aria-expanded="false" data-target="#collapse_' + nID + '" aria-controls="collapse_' + nID + '">' +
                        '<div class="form-row">' +
                            '<b>' + nHead + '. ' + sTitle + '</b>' +
                            '<span class="ml-auto"></span>' +
                        '</div>' +
                    '</div>' +
                    '<div class="collapse" id="collapse_' + nID + '">' +
                        '<div class="card-body cCustom">' +
                            '<div class="table-responsive pb-3">' +
                                '<table id="tbData' + nID + '" class="table table-sm table-bordered tbData">' +
                                    '<thead>' +
                                        '<tr class="valign-middle pad-primary">' +
                                            '<th class="text-center" rowspan="3">Required Data</th>' +
                                            '<th style="width: 14%" class="text-center" rowspan="3">Unit</th>' +
                                            '<th class="text-center" colspan="6">Data Collection Period</th>' +
                                        '</tr>' +
                                        '<tr class="valign-middle pad-primary">' +
                                            '<th class="text-center" colspan="2">' + arr[0] + '</th>' +
                                            '<th class="text-center" colspan="2">' + arr[1] + '</th>' +
                                            '<th class="text-center" colspan="2">' + arr[2] + '</th>' +
                                        '</tr>' +
                                        '<tr class="valign-middle pad-primary">' +
                                            '<th style="width: 10%" class="text-center">Male</th>' +
                                            '<th style="width: 10%" class="text-center">Female</th>' +
                                            '<th style="width: 10%" class="text-center">Male</th>' +
                                            '<th style="width: 10%" class="text-center">Female</th>' +
                                            '<th style="width: 10%" class="text-center">Male</th>' +
                                            '<th style="width: 10%" class="text-center">Female</th>' +
                                        '</tr>' +
                '</thead>' +
                '<tbody>';
            }
            return th;
        }

        function CalDJSI(IsChange) {
            BlockUI();

            var IsPass = true;
            var lstFail = [];

            // #region 3. Total Employee by Employment Contract and by Area

            // #region Permanent contract[4]

            // #region 3.2 Rayong
            var $txtM_1_16 = +GetValTextBox('txtM_1_16').replace(/\,/g, '');
            var $txtF_1_16 = +GetValTextBox('txtF_1_16').replace(/\,/g, '');
            var $txtM_2_16 = +GetValTextBox('txtM_2_16').replace(/\,/g, '');
            var $txtF_2_16 = +GetValTextBox('txtF_2_16').replace(/\,/g, '');
            var $txtM_3_16 = +GetValTextBox('txtM_3_16').replace(/\,/g, '');
            var $txtF_3_16 = +GetValTextBox('txtF_3_16').replace(/\,/g, '');
            // #endregion

            // #region 3.3 Bangkok
            var $txtM_1_17 = +GetValTextBox('txtM_1_17').replace(/\,/g, '');
            var $txtF_1_17 = +GetValTextBox('txtF_1_17').replace(/\,/g, '');
            var $txtM_2_17 = +GetValTextBox('txtM_2_17').replace(/\,/g, '');
            var $txtF_2_17 = +GetValTextBox('txtF_2_17').replace(/\,/g, '');
            var $txtM_3_17 = +GetValTextBox('txtM_3_17').replace(/\,/g, '');
            var $txtF_3_17 = +GetValTextBox('txtF_3_17').replace(/\,/g, '');
            // #endregion           

            // #region 3.4 Other provinces
            var $txtM_1_18 = +GetValTextBox('txtM_1_18').replace(/\,/g, '');
            var $txtF_1_18 = +GetValTextBox('txtF_1_18').replace(/\,/g, '');
            var $txtM_2_18 = +GetValTextBox('txtM_2_18').replace(/\,/g, '');
            var $txtF_2_18 = +GetValTextBox('txtF_2_18').replace(/\,/g, '');
            var $txtM_3_18 = +GetValTextBox('txtM_3_18').replace(/\,/g, '');
            var $txtF_3_18 = +GetValTextBox('txtF_3_18').replace(/\,/g, '');
            // #endregion

            // #region 3.1 Permanent contract[4]
            var $txtM_1_15 = CalNumber($txtM_1_16 + $txtM_1_17 + $txtM_1_18);
            var $txtF_1_15 = CalNumber($txtF_1_16 + $txtF_1_17 + $txtF_1_18);
            $('#txtM_1_15').val($txtM_1_15);
            $('#txtF_1_15').val($txtF_1_15);

            var $txtM_2_15 = CalNumber($txtM_2_16 + $txtM_2_17 + $txtM_2_18);
            var $txtF_2_15 = CalNumber($txtF_2_16 + $txtF_2_17 + $txtF_2_18);
            $('#txtM_2_15').val($txtM_2_15);
            $('#txtF_2_15').val($txtF_2_15);

            var $txtM_3_15 = CalNumber($txtM_3_16 + $txtM_3_17 + $txtM_3_18);
            var $txtF_3_15 = CalNumber($txtF_3_16 + $txtF_3_17 + $txtF_3_18);
            $('#txtM_3_15').val($txtM_3_15);
            $('#txtF_3_15').val($txtF_3_15);
            // #endregion     

            // #endregion

            // #region On contract (Temporary contract) < 1 ปี[5]

            // #region 3.6 Rayong
            var $txtM_1_20 = +GetValTextBox('txtM_1_20').replace(/\,/g, '');
            var $txtF_1_20 = +GetValTextBox('txtF_1_20').replace(/\,/g, '');
            var $txtM_2_20 = +GetValTextBox('txtM_2_20').replace(/\,/g, '');
            var $txtF_2_20 = +GetValTextBox('txtF_2_20').replace(/\,/g, '');
            var $txtM_3_20 = +GetValTextBox('txtM_3_20').replace(/\,/g, '');
            var $txtF_3_20 = +GetValTextBox('txtF_3_20').replace(/\,/g, '');
            // #endregion

            // #region 3.7 Bangkok
            var $txtM_1_21 = +GetValTextBox('txtM_1_21').replace(/\,/g, '');
            var $txtF_1_21 = +GetValTextBox('txtF_1_21').replace(/\,/g, '');
            var $txtM_2_21 = +GetValTextBox('txtM_2_21').replace(/\,/g, '');
            var $txtF_2_21 = +GetValTextBox('txtF_2_21').replace(/\,/g, '');
            var $txtM_3_21 = +GetValTextBox('txtM_3_21').replace(/\,/g, '');
            var $txtF_3_21 = +GetValTextBox('txtF_3_21').replace(/\,/g, '');
            // #endregion           

            // #region 3.8 Other provinces
            var $txtM_1_22 = +GetValTextBox('txtM_1_22').replace(/\,/g, '');
            var $txtF_1_22 = +GetValTextBox('txtF_1_22').replace(/\,/g, '');
            var $txtM_2_22 = +GetValTextBox('txtM_2_22').replace(/\,/g, '');
            var $txtF_2_22 = +GetValTextBox('txtF_2_22').replace(/\,/g, '');
            var $txtM_3_22 = +GetValTextBox('txtM_3_22').replace(/\,/g, '');
            var $txtF_3_22 = +GetValTextBox('txtF_3_22').replace(/\,/g, '');
            // #endregion

            // #region 3.5 On contract (Temporary contract) < 1 ปี[5]
            var $txtM_1_19 = CalNumber($txtM_1_20 + $txtM_1_21 + $txtM_1_22);
            var $txtF_1_19 = CalNumber($txtF_1_20 + $txtF_1_21 + $txtF_1_22);
            $('#txtM_1_19').val($txtM_1_19);
            $('#txtF_1_19').val($txtF_1_19);

            var $txtM_2_19 = CalNumber($txtM_2_20 + $txtM_2_21 + $txtM_2_22);
            var $txtF_2_19 = CalNumber($txtF_2_20 + $txtF_2_21 + $txtF_2_22);
            $('#txtM_2_19').val($txtM_2_19);
            $('#txtF_2_19').val($txtF_2_19);

            var $txtM_3_19 = CalNumber($txtM_3_20 + $txtM_3_21 + $txtM_3_22);
            var $txtF_3_19 = CalNumber($txtF_3_20 + $txtF_3_21 + $txtF_3_22);
            $('#txtM_3_19').val($txtM_3_19);
            $('#txtF_3_19').val($txtF_3_19);
            // #endregion     

            // #endregion

            // #region On contract (Temporary contract) > 1 ปี[5]

            // #region 3.10 Rayong
            var $txtM_1_24 = +GetValTextBox('txtM_1_24').replace(/\,/g, '');
            var $txtF_1_24 = +GetValTextBox('txtF_1_24').replace(/\,/g, '');
            var $txtM_2_24 = +GetValTextBox('txtM_2_24').replace(/\,/g, '');
            var $txtF_2_24 = +GetValTextBox('txtF_2_24').replace(/\,/g, '');
            var $txtM_3_24 = +GetValTextBox('txtM_3_24').replace(/\,/g, '');
            var $txtF_3_24 = +GetValTextBox('txtF_3_24').replace(/\,/g, '');
            // #endregion

            // #region 3.11 Bangkok
            var $txtM_1_25 = +GetValTextBox('txtM_1_25').replace(/\,/g, '');
            var $txtF_1_25 = +GetValTextBox('txtF_1_25').replace(/\,/g, '');
            var $txtM_2_25 = +GetValTextBox('txtM_2_25').replace(/\,/g, '');
            var $txtF_2_25 = +GetValTextBox('txtF_2_25').replace(/\,/g, '');
            var $txtM_3_25 = +GetValTextBox('txtM_3_25').replace(/\,/g, '');
            var $txtF_3_25 = +GetValTextBox('txtF_3_25').replace(/\,/g, '');
            // #endregion           

            // #region 3.12 Other provinces
            var $txtM_1_26 = +GetValTextBox('txtM_1_26').replace(/\,/g, '');
            var $txtF_1_26 = +GetValTextBox('txtF_1_26').replace(/\,/g, '');
            var $txtM_2_26 = +GetValTextBox('txtM_2_26').replace(/\,/g, '');
            var $txtF_2_26 = +GetValTextBox('txtF_2_26').replace(/\,/g, '');
            var $txtM_3_26 = +GetValTextBox('txtM_3_26').replace(/\,/g, '');
            var $txtF_3_26 = +GetValTextBox('txtF_3_26').replace(/\,/g, '');
            // #endregion

            // #region 3.9 On contract (Temporary contract) > 1 ปี[5]
            var $txtM_1_23 = CalNumber($txtM_1_24 + $txtM_1_25 + $txtM_1_26);
            var $txtF_1_23 = CalNumber($txtF_1_24 + $txtF_1_25 + $txtF_1_26);
            $('#txtM_1_23').val($txtM_1_23);
            $('#txtF_1_23').val($txtF_1_23);

            var $txtM_2_23 = CalNumber($txtM_2_24 + $txtM_2_25 + $txtM_2_26);
            var $txtF_2_23 = CalNumber($txtF_2_24 + $txtF_2_25 + $txtF_2_26);
            $('#txtM_2_23').val($txtM_2_23);
            $('#txtF_2_23').val($txtF_2_23);

            var $txtM_3_23 = CalNumber($txtM_3_24 + $txtM_3_25 + $txtM_3_26);
            var $txtF_3_23 = CalNumber($txtF_3_24 + $txtF_3_25 + $txtF_3_26);
            $('#txtM_3_23').val($txtM_3_23);
            $('#txtF_3_23').val($txtF_3_23);
            // #endregion     

            // #endregion

            // #endregion

            // #region 2. Total employee by Area

            // #region 2.1 Rayong
            var $txtM_1_11 = $txtM_1_16 + $txtM_1_20 + $txtM_1_24;
            var $txtF_1_11 = $txtF_1_16 + $txtF_1_20 + $txtF_1_24;
            var $txtM_2_11 = $txtM_2_16 + $txtM_2_20 + $txtM_2_24;
            var $txtF_2_11 = $txtF_2_16 + $txtF_2_20 + $txtF_2_24;
            var $txtM_3_11 = $txtM_3_16 + $txtM_3_20 + $txtM_3_24;
            var $txtF_3_11 = $txtF_3_16 + $txtF_3_20 + $txtF_3_24;

            $('#txtM_1_11').val($txtM_1_11);
            $('#txtF_1_11').val($txtF_1_11);
            $('#txtM_2_11').val($txtM_2_11);
            $('#txtF_2_11').val($txtF_2_11);
            $('#txtM_3_11').val($txtM_3_11);
            $('#txtF_3_11').val($txtF_3_11);
            // #endregion

            // #region 2.2 Bangkok
            var $txtM_1_12 = $txtM_1_17 + $txtM_1_21 + $txtM_1_25;
            var $txtF_1_12 = $txtF_1_17 + $txtF_1_21 + $txtF_1_25;
            var $txtM_2_12 = $txtM_2_17 + $txtM_2_21 + $txtM_2_25;
            var $txtF_2_12 = $txtF_2_17 + $txtF_2_21 + $txtF_2_25;
            var $txtM_3_12 = $txtM_3_17 + $txtM_3_21 + $txtM_3_25;
            var $txtF_3_12 = $txtF_3_17 + $txtF_3_21 + $txtF_3_25;

            $('#txtM_1_12').val($txtM_1_12);
            $('#txtF_1_12').val($txtF_1_12);
            $('#txtM_2_12').val($txtM_2_12);
            $('#txtF_2_12').val($txtF_2_12);
            $('#txtM_3_12').val($txtM_3_12);
            $('#txtF_3_12').val($txtF_3_12);
            // #endregion

            // #region 2.3 Other provinces
            var $txtM_1_13 = $txtM_1_18 + $txtM_1_22 + $txtM_1_26;
            var $txtF_1_13 = $txtF_1_18 + $txtF_1_22 + $txtF_1_26;
            var $txtM_2_13 = $txtM_2_18 + $txtM_2_22 + $txtM_2_26;
            var $txtF_2_13 = $txtF_2_18 + $txtF_2_22 + $txtF_2_26;
            var $txtM_3_13 = $txtM_3_18 + $txtM_3_22 + $txtM_3_26;
            var $txtF_3_13 = $txtF_3_18 + $txtF_3_22 + $txtF_3_26;

            $('#txtM_1_13').val($txtM_1_13);
            $('#txtF_1_13').val($txtF_1_13);
            $('#txtM_2_13').val($txtM_2_13);
            $('#txtF_2_13').val($txtF_2_13);
            $('#txtM_3_13').val($txtM_3_13);
            $('#txtF_3_13').val($txtF_3_13);
            // #endregion

            // #endregion

            // #region 1. Worker            

            // #region 1.2 Total employee[2] => from 2. Total employee by Area
            var $txtM_1_5 = CalNumber($txtM_1_11 + $txtM_1_12 + $txtM_1_13);//Male
            var $txtF_1_5 = CalNumber($txtF_1_11 + $txtF_1_12 + $txtF_1_13);//Female
            var $txtM_1_4 = CalNumber($txtM_1_5 + $txtF_1_5);//Total
            $('#txtM_1_4').val($txtM_1_4);
            $('#txtM_1_5').val($txtM_1_5);
            $('#txtF_1_5').val($txtF_1_5);

            var $txtM_2_5 = CalNumber($txtM_2_11 + $txtM_2_12 + $txtM_2_13);//Male
            var $txtF_2_5 = CalNumber($txtF_2_11 + $txtF_2_12 + $txtF_2_13);//Female
            var $txtM_2_4 = CalNumber($txtM_2_5 + $txtF_2_5);//Total
            $('#txtM_2_4').val($txtM_2_4);
            $('#txtM_2_5').val($txtM_2_5);
            $('#txtF_2_5').val($txtF_2_5);

            var $txtM_3_5 = CalNumber($txtM_3_11 + $txtM_3_12 + $txtM_3_13);//Male
            var $txtF_3_5 = CalNumber($txtF_3_11 + $txtF_3_12 + $txtF_3_13);//Female
            var $txtM_3_4 = CalNumber($txtM_3_5 + $txtF_3_5);//Total
            $('#txtM_3_4').val($txtM_3_4);
            $('#txtM_3_5').val($txtM_3_5);
            $('#txtF_3_5').val($txtF_3_5);
            // #endregion

            // #region 1.3 Contractor[3]            
            var $txtM_1_7 = +GetValTextBox('txtM_1_7').replace(/\,/g, '');
            var $txtF_1_7 = +GetValTextBox('txtF_1_7').replace(/\,/g, '');
            var $txtM_1_6 = CalNumber($txtM_1_7 + $txtF_1_7);
            $('#txtM_1_6').val($txtM_1_6);

            var $txtM_2_7 = +GetValTextBox('txtM_2_7').replace(/\,/g, '');
            var $txtF_2_7 = +GetValTextBox('txtF_2_7').replace(/\,/g, '');
            var $txtM_2_6 = CalNumber($txtM_2_7 + $txtF_2_7);
            $('#txtM_2_6').val($txtM_2_6);

            var $txtM_3_7 = +GetValTextBox('txtM_3_7').replace(/\,/g, '');
            var $txtF_3_7 = +GetValTextBox('txtF_3_7').replace(/\,/g, '');
            var $txtM_3_6 = CalNumber($txtM_3_7 + $txtF_3_7);
            $('#txtM_3_6').val($txtM_3_6);
            // #endregion

            // #region 1.4 Other (please specific, if any)
            var $txtM_1_9 = +GetValTextBox('txtM_1_9').replace(/\,/g, '');
            var $txtF_1_9 = +GetValTextBox('txtF_1_9').replace(/\,/g, '');
            var $txtM_1_8 = CalNumber($txtM_1_9 + $txtF_1_9);
            $('#txtM_1_8').val($txtM_1_8);

            var $txtM_2_9 = +GetValTextBox('txtM_2_9').replace(/\,/g, '');
            var $txtF_2_9 = +GetValTextBox('txtF_2_9').replace(/\,/g, '');
            var $txtM_2_8 = CalNumber($txtM_2_9 + $txtF_2_9);
            $('#txtM_2_8').val($txtM_2_8);

            var $txtM_3_9 = +GetValTextBox('txtM_3_9').replace(/\,/g, '');
            var $txtF_3_9 = +GetValTextBox('txtF_3_9').replace(/\,/g, '');
            var $txtM_3_8 = CalNumber($txtM_3_9 + $txtF_3_9);
            $('#txtM_3_8').val($txtM_3_8);
            // #endregion

            // #region 1.1 Total worker[1]
            var $txtM_1_3 = CalNumber($txtM_1_5 + $txtM_1_7 + $txtM_1_9);
            var $txtF_1_3 = CalNumber($txtF_1_5 + $txtF_1_7 + $txtF_1_9);
            var $txtM_1_2 = CalNumber($txtM_1_3 + $txtF_1_3);
            $('#txtM_1_2').val($txtM_1_2);//Total
            $('#txtM_1_3').val($txtM_1_3);//Male
            $('#txtF_1_3').val($txtF_1_3);//Female

            var $txtM_2_3 = CalNumber($txtM_2_5 + $txtM_2_7 + $txtM_2_9);
            var $txtF_2_3 = CalNumber($txtF_2_5 + $txtF_2_7 + $txtF_2_9);
            var $txtM_2_2 = CalNumber($txtM_2_3 + $txtF_2_3);
            $('#txtM_2_2').val($txtM_2_2);//Total
            $('#txtM_2_3').val($txtM_2_3);//Male
            $('#txtF_2_3').val($txtF_2_3);//Female

            var $txtM_3_3 = CalNumber($txtM_3_5 + $txtM_3_7 + $txtM_3_9);
            var $txtF_3_3 = CalNumber($txtF_3_5 + $txtF_3_7 + $txtF_3_9);
            var $txtM_3_2 = CalNumber($txtM_3_3 + $txtF_3_3);
            $('#txtM_3_2').val($txtM_3_2);//Total
            $('#txtM_3_3').val($txtM_3_3);//Male
            $('#txtF_3_3').val($txtF_3_3);//Female
            // #endregion

            // #endregion                        

            // #region 4. Total Employee by Employment Type

            // #region 4.1 Full-time
            var $txtM_1_28 = +GetValTextBox('txtM_1_28').replace(/\,/g, '');
            var $txtF_1_28 = +GetValTextBox('txtF_1_28').replace(/\,/g, '');
            var $txtM_2_28 = +GetValTextBox('txtM_2_28').replace(/\,/g, '');
            var $txtF_2_28 = +GetValTextBox('txtF_2_28').replace(/\,/g, '');
            var $txtM_3_28 = +GetValTextBox('txtM_3_28').replace(/\,/g, '');
            var $txtF_3_28 = +GetValTextBox('txtF_3_28').replace(/\,/g, '');

            if ($txtM_1_5 < $txtM_1_28) { $txtM_1_28 = 0; $('#txtM_1_28').val(''); IsPass = false; lstFail.push("4.1 Full-time"); }
            if ($txtF_1_5 < $txtF_1_28) { $txtF_1_28 = 0; $('#txtF_1_28').val(''); IsPass = false; lstFail.push("4.1 Full-time"); }
            if ($txtM_2_5 < $txtM_2_28) { $txtM_2_28 = 0; $('#txtM_2_28').val(''); IsPass = false; lstFail.push("4.1 Full-time"); }
            if ($txtF_2_5 < $txtF_2_28) { $txtF_2_28 = 0; $('#txtF_2_28').val(''); IsPass = false; lstFail.push("4.1 Full-time"); }
            if ($txtM_3_5 < $txtM_3_28) { $txtM_3_28 = 0; $('#txtM_3_28').val(''); IsPass = false; lstFail.push("4.1 Full-time"); }
            if ($txtF_3_5 < $txtF_3_28) { $txtF_3_28 = 0; $('#txtF_3_28').val(''); IsPass = false; lstFail.push("4.1 Full-time"); }
            // #endregion

            // #region 4.2 Part-time
            var $txtM_1_29 = $txtM_1_5 - $txtM_1_28;
            var $txtF_1_29 = $txtF_1_5 - $txtF_1_28;
            var $txtM_2_29 = $txtM_2_5 - $txtM_2_28;
            var $txtF_2_29 = $txtF_2_5 - $txtF_2_28;
            var $txtM_3_29 = $txtM_3_5 - $txtM_3_28;
            var $txtF_3_29 = $txtF_3_5 - $txtF_3_28;

            $('#txtM_1_29').val($txtM_1_29);
            $('#txtF_1_29').val($txtF_1_29);
            $('#txtM_2_29').val($txtM_2_29);
            $('#txtF_2_29').val($txtF_2_29);
            $('#txtM_3_29').val($txtM_3_29);
            $('#txtF_3_29').val($txtF_3_29);
            // #endregion

            // #endregion

            // #region 5. Total Employee by Age group

            // #region 5.1 <30 years
            var $txtM_1_32 = +GetValTextBox('txtM_1_32').replace(/\,/g, '');
            var $txtF_1_32 = +GetValTextBox('txtF_1_32').replace(/\,/g, '');
            if ($txtM_1_5 < $txtM_1_32) { $txtM_1_32 = 0; $('#txtM_1_32').val(''); IsPass = false; lstFail.push("5.1 <30 years"); }
            if ($txtF_1_5 < $txtF_1_32) { $txtF_1_32 = 0; $('#txtF_1_32').val(''); IsPass = false; lstFail.push("5.1 <30 years"); }
            var $txtM_1_31 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_32 / $txtM_1_4) * 100) : 0;
            var $txtF_1_31 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_32 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_31').val($txtM_1_31.toFixed(2));
            $('#txtF_1_31').val($txtF_1_31.toFixed(2));

            var $txtM_2_32 = +GetValTextBox('txtM_2_32').replace(/\,/g, '');
            var $txtF_2_32 = +GetValTextBox('txtF_2_32').replace(/\,/g, '');
            if ($txtM_2_5 < $txtM_2_32) { $txtM_2_32 = 0; $('#txtM_2_32').val(''); IsPass = false; lstFail.push("5.1 <30 years"); }
            if ($txtF_2_5 < $txtF_2_32) { $txtF_2_32 = 0; $('#txtF_2_32').val(''); IsPass = false; lstFail.push("5.1 <30 years"); }
            var $txtM_2_31 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_32 / $txtM_2_4) * 100) : 0;
            var $txtF_2_31 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_32 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_31').val($txtM_2_31.toFixed(2));
            $('#txtF_2_31').val($txtF_2_31.toFixed(2));

            var $txtM_3_32 = +GetValTextBox('txtM_3_32').replace(/\,/g, '');
            var $txtF_3_32 = +GetValTextBox('txtF_3_32').replace(/\,/g, '');
            if ($txtM_3_5 < $txtM_3_32) { $txtM_3_32 = 0; $('#txtM_3_32').val(''); IsPass = false; lstFail.push("5.1 <30 years"); }
            if ($txtF_3_5 < $txtF_3_32) { $txtF_3_32 = 0; $('#txtF_3_32').val(''); IsPass = false; lstFail.push("5.1 <30 years"); }
            var $txtM_3_31 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_32 / $txtM_3_4) * 100) : 0;
            var $txtF_3_31 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_32 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_31').val($txtM_3_31.toFixed(2));
            $('#txtF_3_31').val($txtF_3_31.toFixed(2));
            // #endregion

            // #region 5.2 30 - 50 years
            var $txtM_1_34 = +GetValTextBox('txtM_1_34').replace(/\,/g, '');
            var $txtF_1_34 = +GetValTextBox('txtF_1_34').replace(/\,/g, '');
            if ($txtM_1_5 < ($txtM_1_32 + $txtM_1_34)) { $txtM_1_34 = 0; $('#txtM_1_34').val(''); IsPass = false; lstFail.push("5.2 30 - 50 years"); }
            if ($txtF_1_5 < ($txtF_1_32 + $txtF_1_34)) { $txtF_1_34 = 0; $('#txtF_1_34').val(''); IsPass = false; lstFail.push("5.2 30 - 50 years"); }
            var $txtM_1_33 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_34 / $txtM_1_4) * 100) : 0;
            var $txtF_1_33 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_34 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_33').val($txtM_1_33.toFixed(2));
            $('#txtF_1_33').val($txtF_1_33.toFixed(2));

            var $txtM_2_34 = +GetValTextBox('txtM_2_34').replace(/\,/g, '');
            var $txtF_2_34 = +GetValTextBox('txtF_2_34').replace(/\,/g, '');
            if ($txtM_2_5 < ($txtM_2_32 + $txtM_2_34)) { $txtM_2_34 = 0; $('#txtM_2_34').val(''); IsPass = false; lstFail.push("5.2 30 - 50 years"); }
            if ($txtF_2_5 < ($txtF_2_32 + $txtF_2_34)) { $txtF_2_34 = 0; $('#txtF_2_34').val(''); IsPass = false; lstFail.push("5.2 30 - 50 years"); }
            var $txtM_2_33 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_34 / $txtM_2_4) * 100) : 0;
            var $txtF_2_33 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_34 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_33').val($txtM_2_33.toFixed(2));
            $('#txtF_2_33').val($txtF_2_33.toFixed(2));

            var $txtM_3_34 = +GetValTextBox('txtM_3_34').replace(/\,/g, '');
            var $txtF_3_34 = +GetValTextBox('txtF_3_34').replace(/\,/g, '');
            if ($txtM_3_5 < ($txtM_3_32 + $txtM_3_34)) { $txtM_3_34 = 0; $('#txtM_3_34').val(''); IsPass = false; lstFail.push("5.2 30 - 50 years"); }
            if ($txtF_3_5 < ($txtF_3_32 + $txtF_3_34)) { $txtF_3_34 = 0; $('#txtF_3_34').val(''); IsPass = false; lstFail.push("5.2 30 - 50 years"); }
            var $txtM_3_33 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_34 / $txtM_3_4) * 100) : 0;
            var $txtF_3_33 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_34 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_33').val($txtM_3_33.toFixed(2));
            $('#txtF_3_33').val($txtF_3_33.toFixed(2));
            // #endregion

            // #region 5.3 >50 years
            var $txtM_1_36 = ($txtM_1_5 - ($txtM_1_32 + $txtM_1_34)) >= 0 ? ($txtM_1_5 - ($txtM_1_32 + $txtM_1_34)) : 0;
            var $txtF_1_36 = ($txtF_1_5 - ($txtF_1_32 + $txtF_1_34)) >= 0 ? ($txtF_1_5 - ($txtF_1_32 + $txtF_1_34)) : 0;
            var $txtM_1_35 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_36 / $txtM_1_4) * 100) : 0;
            var $txtF_1_35 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_36 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_35').val($txtM_1_35.toFixed(2));
            $('#txtF_1_35').val($txtF_1_35.toFixed(2));
            $('#txtM_1_36').val($txtM_1_36);
            $('#txtF_1_36').val($txtF_1_36);

            var $txtM_2_36 = ($txtM_2_5 - ($txtM_2_32 + $txtM_2_34)) >= 0 ? ($txtM_2_5 - ($txtM_2_32 + $txtM_2_34)) : 0;
            var $txtF_2_36 = ($txtF_2_5 - ($txtF_2_32 + $txtF_2_34)) >= 0 ? ($txtF_2_5 - ($txtF_2_32 + $txtF_2_34)) : 0;
            var $txtM_2_35 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_36 / $txtM_2_4) * 100) : 0;
            var $txtF_2_35 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_36 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_35').val($txtM_2_35.toFixed(2));
            $('#txtF_2_35').val($txtF_2_35.toFixed(2));
            $('#txtM_2_36').val($txtM_2_36);
            $('#txtF_2_36').val($txtF_2_36);

            var $txtM_3_36 = ($txtM_3_5 - ($txtM_3_32 + $txtM_3_34)) >= 0 ? ($txtM_3_5 - ($txtM_3_32 + $txtM_3_34)) : 0;
            var $txtF_3_36 = ($txtF_3_5 - ($txtF_3_32 + $txtF_3_34)) >= 0 ? ($txtF_3_5 - ($txtF_3_32 + $txtF_3_34)) : 0;
            var $txtM_3_35 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_36 / $txtM_3_4) * 100) : 0;
            var $txtF_3_35 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_36 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_35').val($txtM_3_35.toFixed(2));
            $('#txtF_3_35').val($txtF_3_35.toFixed(2));
            $('#txtM_3_36').val($txtM_3_36);
            $('#txtF_3_36').val($txtF_3_36);
            // #endregion

            // #endregion

            // #region 6. Total Employee by Employee Category (level)

            // #region 6.1 Executive (Level 13-18)
            var $txtM_1_39 = +GetValTextBox('txtM_1_39').replace(/\,/g, '');
            var $txtF_1_39 = +GetValTextBox('txtF_1_39').replace(/\,/g, '');
            if ($txtM_1_5 < $txtM_1_39) { $txtM_1_39 = 0; $('#txtM_1_39').val(''); IsPass = false; lstFail.push("6.1 Executive (Level 13-18)"); }
            if ($txtF_1_5 < $txtF_1_39) { $txtF_1_39 = 0; $('#txtF_1_39').val(''); IsPass = false; lstFail.push("6.1 Executive (Level 13-18)"); }
            var $txtM_1_38 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_39 / $txtM_1_4) * 100) : 0;
            var $txtF_1_38 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_39 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_38').val($txtM_1_38.toFixed(2));
            $('#txtF_1_38').val($txtF_1_38.toFixed(2));

            var $txtM_2_39 = +GetValTextBox('txtM_2_39').replace(/\,/g, '');
            var $txtF_2_39 = +GetValTextBox('txtF_2_39').replace(/\,/g, '');
            if ($txtM_2_5 < $txtM_2_39) { $txtM_2_39 = 0; $('#txtM_2_39').val(''); IsPass = false; lstFail.push("6.1 Executive (Level 13-18)"); }
            if ($txtF_2_5 < $txtF_2_39) { $txtF_2_39 = 0; $('#txtF_2_39').val(''); IsPass = false; lstFail.push("6.1 Executive (Level 13-18)"); }
            var $txtM_2_38 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_39 / $txtM_2_4) * 100) : 0;
            var $txtF_2_38 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_39 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_38').val($txtM_2_38.toFixed(2));
            $('#txtF_2_38').val($txtF_2_38.toFixed(2));

            var $txtM_3_39 = +GetValTextBox('txtM_3_39').replace(/\,/g, '');
            var $txtF_3_39 = +GetValTextBox('txtF_3_39').replace(/\,/g, '');
            if ($txtM_3_5 < $txtM_3_39) { $txtM_3_39 = 0; $('#txtM_3_39').val(''); IsPass = false; lstFail.push("6.1 Executive (Level 13-18)"); }
            if ($txtF_3_5 < $txtF_3_39) { $txtF_3_39 = 0; $('#txtF_3_39').val(''); IsPass = false; lstFail.push("6.1 Executive (Level 13-18)"); }
            var $txtM_3_38 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_39 / $txtM_3_4) * 100) : 0;
            var $txtF_3_38 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_39 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_38').val($txtM_3_38.toFixed(2));
            $('#txtF_3_38').val($txtF_3_38.toFixed(2));
            // #endregion

            // #region 6.2 Middle management (Level 10-12)
            var $txtM_1_41 = +GetValTextBox('txtM_1_41').replace(/\,/g, '');
            var $txtF_1_41 = +GetValTextBox('txtF_1_41').replace(/\,/g, '');
            if ($txtM_1_5 < ($txtM_1_39 + $txtM_1_41)) { $txtM_1_41 = 0; $('#txtM_1_41').val(''); IsPass = false; lstFail.push("6.2 Middle management (Level 10-12)"); }
            if ($txtF_1_5 < ($txtF_1_39 + $txtF_1_41)) { $txtF_1_41 = 0; $('#txtF_1_41').val(''); IsPass = false; lstFail.push("6.2 Middle management (Level 10-12)"); }
            var $txtM_1_40 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_41 / $txtM_1_4) * 100) : 0;
            var $txtF_1_40 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_41 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_40').val($txtM_1_40.toFixed(2));
            $('#txtF_1_40').val($txtF_1_40.toFixed(2));

            var $txtM_2_41 = +GetValTextBox('txtM_2_41').replace(/\,/g, '');
            var $txtF_2_41 = +GetValTextBox('txtF_2_41').replace(/\,/g, '');
            if ($txtM_2_5 < ($txtM_2_39 + $txtM_2_41)) { $txtM_2_41 = 0; $('#txtM_2_41').val(''); IsPass = false; lstFail.push("6.2 Middle management (Level 10-12)"); }
            if ($txtF_2_5 < ($txtF_2_39 + $txtF_2_41)) { $txtF_2_41 = 0; $('#txtF_2_41').val(''); IsPass = false; lstFail.push("6.2 Middle management (Level 10-12)"); }
            var $txtM_2_40 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_41 / $txtM_2_4) * 100) : 0;
            var $txtF_2_40 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_41 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_40').val($txtM_2_40.toFixed(2));
            $('#txtF_2_40').val($txtF_2_40.toFixed(2));

            var $txtM_3_41 = +GetValTextBox('txtM_3_41').replace(/\,/g, '');
            var $txtF_3_41 = +GetValTextBox('txtF_3_41').replace(/\,/g, '');
            if ($txtM_3_5 < ($txtM_3_39 + $txtM_3_41)) { $txtM_3_41 = 0; $('#txtM_3_41').val(''); IsPass = false; lstFail.push("6.2 Middle management (Level 10-12)"); }
            if ($txtF_3_5 < ($txtF_3_39 + $txtF_3_41)) { $txtF_3_41 = 0; $('#txtF_3_41').val(''); IsPass = false; lstFail.push("6.2 Middle management (Level 10-12)"); }
            var $txtM_3_40 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_41 / $txtM_3_4) * 100) : 0;
            var $txtF_3_40 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_41 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_40').val($txtM_3_40.toFixed(2));
            $('#txtF_3_40').val($txtF_3_40.toFixed(2));
            // #endregion

            // #region 6.3 Senior (Level 8-9)
            var $txtM_1_43 = +GetValTextBox('txtM_1_43').replace(/\,/g, '');
            var $txtF_1_43 = +GetValTextBox('txtF_1_43').replace(/\,/g, '');
            if ($txtM_1_5 < ($txtM_1_39 + $txtM_1_41 + $txtM_1_43)) { $txtM_1_43 = 0; $('#txtM_1_43').val(''); IsPass = false; lstFail.push("6.3 Senior (Level 8-9)"); }
            if ($txtF_1_5 < ($txtF_1_39 + $txtF_1_41 + $txtF_1_43)) { $txtF_1_43 = 0; $('#txtF_1_43').val(''); IsPass = false; lstFail.push("6.3 Senior (Level 8-9)"); }
            var $txtM_1_42 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_43 / $txtM_1_4) * 100) : 0;
            var $txtF_1_42 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_43 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_42').val($txtM_1_42.toFixed(2));
            $('#txtF_1_42').val($txtF_1_42.toFixed(2));

            var $txtM_2_43 = +GetValTextBox('txtM_2_43').replace(/\,/g, '');
            var $txtF_2_43 = +GetValTextBox('txtF_2_43').replace(/\,/g, '');
            if ($txtM_2_5 < ($txtM_2_39 + $txtM_2_41 + $txtM_2_43)) { $txtM_2_43 = 0; $('#txtM_2_43').val(''); IsPass = false; lstFail.push("6.3 Senior (Level 8-9)"); }
            if ($txtF_2_5 < ($txtF_2_39 + $txtF_2_41 + $txtF_2_43)) { $txtF_2_43 = 0; $('#txtF_2_43').val(''); IsPass = false; lstFail.push("6.3 Senior (Level 8-9)"); }
            var $txtM_2_42 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_43 / $txtM_2_4) * 100) : 0;
            var $txtF_2_42 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_43 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_42').val($txtM_2_42.toFixed(2));
            $('#txtF_2_42').val($txtF_2_42.toFixed(2));

            var $txtM_3_43 = +GetValTextBox('txtM_3_43').replace(/\,/g, '');
            var $txtF_3_43 = +GetValTextBox('txtF_3_43').replace(/\,/g, '');
            if ($txtM_3_5 < ($txtM_3_39 + $txtM_3_41 + $txtM_3_43)) { $txtM_3_43 = 0; $('#txtM_3_43').val(''); IsPass = false; lstFail.push("6.3 Senior (Level 8-9)"); }
            if ($txtF_3_5 < ($txtF_3_39 + $txtF_3_41 + $txtF_3_43)) { $txtF_3_43 = 0; $('#txtF_3_43').val(''); IsPass = false; lstFail.push("6.3 Senior (Level 8-9)"); }
            var $txtM_3_42 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_43 / $txtM_3_4) * 100) : 0;
            var $txtF_3_42 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_43 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_42').val($txtM_3_42.toFixed(2));
            $('#txtF_3_42').val($txtF_3_42.toFixed(2));
            // #endregion

            // #region 6.4 Employee (Level 7 and Below)
            var $txtM_1_45 = +GetValTextBox('txtM_1_45').replace(/\,/g, '');
            var $txtF_1_45 = +GetValTextBox('txtF_1_45').replace(/\,/g, '');
            if ($txtM_1_5 < ($txtM_1_39 + $txtM_1_41 + $txtM_1_43 + $txtM_1_45)) { $txtM_1_45 = 0; $('#txtM_1_45').val(''); IsPass = false; lstFail.push("6.4 Employee (Level 7 and Below)"); }
            if ($txtF_1_5 < ($txtF_1_39 + $txtF_1_41 + $txtF_1_43 + $txtF_1_45)) { $txtF_1_45 = 0; $('#txtF_1_45').val(''); IsPass = false; lstFail.push("6.4 Employee (Level 7 and Below)"); }
            var $txtM_1_44 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_45 / $txtM_1_4) * 100) : 0;
            var $txtF_1_44 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_45 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_44').val($txtM_1_44.toFixed(2));
            $('#txtF_1_44').val($txtF_1_44.toFixed(2));

            var $txtM_2_45 = +GetValTextBox('txtM_2_45').replace(/\,/g, '');
            var $txtF_2_45 = +GetValTextBox('txtF_2_45').replace(/\,/g, '');
            if ($txtM_2_5 < ($txtM_2_39 + $txtM_2_41 + $txtM_2_43 + $txtM_2_45)) { $txtM_2_45 = 0; $('#txtM_2_45').val(''); IsPass = false; lstFail.push("6.4 Employee (Level 7 and Below)"); }
            if ($txtF_2_5 < ($txtF_2_39 + $txtF_2_41 + $txtF_2_43 + $txtF_2_45)) { $txtF_2_45 = 0; $('#txtF_2_45').val(''); IsPass = false; lstFail.push("6.4 Employee (Level 7 and Below)"); }
            var $txtM_2_44 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_45 / $txtM_2_4) * 100) : 0;
            var $txtF_2_44 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_45 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_44').val($txtM_2_44.toFixed(2));
            $('#txtF_2_44').val($txtF_2_44.toFixed(2));

            var $txtM_3_45 = +GetValTextBox('txtM_3_45').replace(/\,/g, '');
            var $txtF_3_45 = +GetValTextBox('txtF_3_45').replace(/\,/g, '');
            if ($txtM_3_5 < ($txtM_3_39 + $txtM_3_41 + $txtM_3_43 + $txtM_3_45)) { $txtM_3_45 = 0; $('#txtM_3_45').val(''); IsPass = false; lstFail.push("6.4 Employee (Level 7 and Below)"); }
            if ($txtF_3_5 < ($txtF_3_39 + $txtF_3_41 + $txtF_3_43 + $txtF_3_45)) { $txtF_3_45 = 0; $('#txtF_3_45').val(''); IsPass = false; lstFail.push("6.4 Employee (Level 7 and Below)"); }
            var $txtM_3_44 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_45 / $txtM_3_4) * 100) : 0;
            var $txtF_3_44 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_45 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_44').val($txtM_3_44.toFixed(2));
            $('#txtF_3_44').val($txtF_3_44.toFixed(2));
            // #endregion

            // #region 6.5 Unclassified[6]
            var $txtM_1_47 = ($txtM_1_5 - ($txtM_1_39 + $txtM_1_41 + $txtM_1_43 + $txtM_1_45)) >= 0 ? ($txtM_1_5 - ($txtM_1_39 + $txtM_1_41 + $txtM_1_43 + $txtM_1_45)) : 0;
            var $txtF_1_47 = ($txtF_1_5 - ($txtF_1_39 + $txtF_1_41 + $txtF_1_43 + $txtF_1_45)) >= 0 ? ($txtF_1_5 - ($txtF_1_39 + $txtF_1_41 + $txtF_1_43 + $txtF_1_45)) : 0;
            var $txtM_1_46 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_47 / $txtM_1_4) * 100) : 0;
            var $txtF_1_46 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_47 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_46').val($txtM_1_46.toFixed(2));
            $('#txtF_1_46').val($txtF_1_46.toFixed(2));
            $('#txtM_1_47').val($txtM_1_47);
            $('#txtF_1_47').val($txtF_1_47);

            var $txtM_2_47 = ($txtM_2_5 - ($txtM_2_39 + $txtM_2_41 + $txtM_2_43 + $txtM_2_45)) >= 0 ? ($txtM_2_5 - ($txtM_2_39 + $txtM_2_41 + $txtM_2_43 + $txtM_2_45)) : 0;
            var $txtF_2_47 = ($txtF_2_5 - ($txtF_2_39 + $txtF_2_41 + $txtF_2_43 + $txtF_2_45)) >= 0 ? ($txtF_2_5 - ($txtF_2_39 + $txtF_2_41 + $txtF_2_43 + $txtF_2_45)) : 0;
            var $txtM_2_46 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_47 / $txtM_2_4) * 100) : 0;
            var $txtF_2_46 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_47 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_46').val($txtM_2_46.toFixed(2));
            $('#txtF_2_46').val($txtF_2_46.toFixed(2));
            $('#txtM_2_47').val($txtM_2_47);
            $('#txtF_2_47').val($txtF_2_47);

            var $txtM_3_47 = ($txtM_3_5 - ($txtM_3_39 + $txtM_3_41 + $txtM_3_43 + $txtM_3_45)) >= 0 ? ($txtM_3_5 - ($txtM_3_39 + $txtM_3_41 + $txtM_3_43 + $txtM_3_45)) : 0;
            var $txtF_3_47 = ($txtF_3_5 - ($txtF_3_39 + $txtF_3_41 + $txtF_3_43 + $txtF_3_45)) >= 0 ? ($txtF_3_5 - ($txtF_3_39 + $txtF_3_41 + $txtF_3_43 + $txtF_3_45)) : 0;
            var $txtM_3_46 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_47 / $txtM_3_4) * 100) : 0;
            var $txtF_3_46 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_47 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_46').val($txtM_3_46.toFixed(2));
            $('#txtF_3_46').val($txtF_3_46.toFixed(2));
            $('#txtM_3_47').val($txtM_3_47);
            $('#txtF_3_47').val($txtF_3_47);
            // #endregion

            // #endregion

            // #region 8. New Employee by Area

            // #region 8.1 Rayong
            var $txtM_1_54 = +GetValTextBox('txtM_1_54').replace(/\,/g, '');
            var $txtF_1_54 = +GetValTextBox('txtF_1_54').replace(/\,/g, '');
            var $txtM_1_55 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_54 / $txtM_1_4) * 100) : 0;
            var $txtF_1_55 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_54 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_55').val($txtM_1_55.toFixed(2));
            $('#txtF_1_55').val($txtF_1_55.toFixed(2));

            var $txtM_2_54 = +GetValTextBox('txtM_2_54').replace(/\,/g, '');
            var $txtF_2_54 = +GetValTextBox('txtF_2_54').replace(/\,/g, '');
            var $txtM_2_55 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_54 / $txtM_2_4) * 100) : 0;
            var $txtF_2_55 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_54 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_55').val($txtM_2_55.toFixed(2));
            $('#txtF_2_55').val($txtF_2_55.toFixed(2));

            var $txtM_3_54 = +GetValTextBox('txtM_3_54').replace(/\,/g, '');
            var $txtF_3_54 = +GetValTextBox('txtF_3_54').replace(/\,/g, '');
            var $txtM_3_55 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_54 / $txtM_3_4) * 100) : 0;
            var $txtF_3_55 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_54 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_55').val($txtM_3_55.toFixed(2));
            $('#txtF_3_55').val($txtF_3_55.toFixed(2));
            // #endregion

            // #region 8.2 Bangkok
            var $txtM_1_56 = +GetValTextBox('txtM_1_56').replace(/\,/g, '');
            var $txtF_1_56 = +GetValTextBox('txtF_1_56').replace(/\,/g, '');
            var $txtM_1_57 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_56 / $txtM_1_4) * 100) : 0;
            var $txtF_1_57 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_56 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_57').val($txtM_1_57.toFixed(2));
            $('#txtF_1_57').val($txtF_1_57.toFixed(2));

            var $txtM_2_56 = +GetValTextBox('txtM_2_56').replace(/\,/g, '');
            var $txtF_2_56 = +GetValTextBox('txtF_2_56').replace(/\,/g, '');
            var $txtM_2_57 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_56 / $txtM_2_4) * 100) : 0;
            var $txtF_2_57 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_56 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_57').val($txtM_2_57.toFixed(2));
            $('#txtF_2_57').val($txtF_2_57.toFixed(2));

            var $txtM_3_56 = +GetValTextBox('txtM_3_56').replace(/\,/g, '');
            var $txtF_3_56 = +GetValTextBox('txtF_3_56').replace(/\,/g, '');
            var $txtM_3_57 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_56 / $txtM_3_4) * 100) : 0;
            var $txtF_3_57 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_56 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_57').val($txtM_3_57.toFixed(2));
            $('#txtF_3_57').val($txtF_3_57.toFixed(2));
            // #endregion

            // #region 8.3 Other provinces
            var $txtM_1_58 = +GetValTextBox('txtM_1_58').replace(/\,/g, '');
            var $txtF_1_58 = +GetValTextBox('txtF_1_58').replace(/\,/g, '');
            var $txtM_1_59 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_58 / $txtM_1_4) * 100) : 0;
            var $txtF_1_59 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_58 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_59').val($txtM_1_59.toFixed(2));
            $('#txtF_1_59').val($txtF_1_59.toFixed(2));

            var $txtM_2_58 = +GetValTextBox('txtM_2_58').replace(/\,/g, '');
            var $txtF_2_58 = +GetValTextBox('txtF_2_58').replace(/\,/g, '');
            var $txtM_2_59 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_58 / $txtM_2_4) * 100) : 0;
            var $txtF_2_59 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_58 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_59').val($txtM_2_59.toFixed(2));
            $('#txtF_2_59').val($txtF_2_59.toFixed(2));

            var $txtM_3_58 = +GetValTextBox('txtM_3_58').replace(/\,/g, '');
            var $txtF_3_58 = +GetValTextBox('txtF_3_58').replace(/\,/g, '');
            var $txtM_3_59 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_58 / $txtM_3_4) * 100) : 0;
            var $txtF_3_59 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_58 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_59').val($txtM_3_59.toFixed(2));
            $('#txtF_3_59').val($txtF_3_59.toFixed(2));
            // #endregion

            // #endregion

            // #region 7. New Employee

            // #region 7.1 New employee
            var $txtM_1_50 = $txtM_1_54 + $txtM_1_56 + $txtM_1_58;
            var $txtF_1_50 = $txtF_1_54 + $txtF_1_56 + $txtF_1_58;
            var $txtM_1_49 = $txtM_1_50 + $txtF_1_50;
            $('#txtM_1_49').val($txtM_1_49);
            $('#txtM_1_50').val($txtM_1_50);
            $('#txtF_1_50').val($txtF_1_50);

            var $txtM_2_50 = $txtM_2_54 + $txtM_2_56 + $txtM_2_58;
            var $txtF_2_50 = $txtF_2_54 + $txtF_2_56 + $txtF_2_58;
            var $txtM_2_49 = $txtM_2_50 + $txtF_2_50;
            $('#txtM_2_49').val($txtM_2_49);
            $('#txtM_2_50').val($txtM_2_50);
            $('#txtF_2_50').val($txtF_2_50);

            var $txtM_3_50 = $txtM_3_54 + $txtM_3_56 + $txtM_3_58;
            var $txtF_3_50 = $txtF_3_54 + $txtF_3_56 + $txtF_3_58;
            var $txtM_3_49 = $txtM_3_50 + $txtF_3_50;
            $('#txtM_3_49').val($txtM_3_49);
            $('#txtM_3_50').val($txtM_3_50);
            $('#txtF_3_50').val($txtF_3_50);
            // #endregion

            // #region 7.2 New hire rate
            var $txtM_1_51 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_49 / $txtM_1_4) * 100) : 0;
            var $txtM_1_52 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_50 / $txtM_1_4) * 100) : 0;
            var $txtF_1_52 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_50 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_51').val($txtM_1_51.toFixed(2));
            $('#txtM_1_52').val($txtM_1_52.toFixed(2));
            $('#txtF_1_52').val($txtF_1_52.toFixed(2));

            var $txtM_2_51 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_49 / $txtM_2_4) * 100) : 0;
            var $txtM_2_52 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_50 / $txtM_2_4) * 100) : 0;
            var $txtF_2_52 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_50 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_51').val($txtM_2_51.toFixed(2));
            $('#txtM_2_52').val($txtM_2_52.toFixed(2));
            $('#txtF_2_52').val($txtF_2_52.toFixed(2));

            var $txtM_3_51 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_49 / $txtM_3_4) * 100) : 0;
            var $txtM_3_52 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_50 / $txtM_3_4) * 100) : 0;
            var $txtF_3_52 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_50 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_51').val($txtM_3_51.toFixed(2));
            $('#txtM_3_52').val($txtM_3_52.toFixed(2));
            $('#txtF_3_52').val($txtF_3_52.toFixed(2));
            // #endregion

            // #endregion            

            // #region 9. New Employee Hire by Age Group

            // #region 9.1 <30 years
            var $txtM_1_61 = +GetValTextBox('txtM_1_61').replace(/\,/g, '');
            var $txtF_1_61 = +GetValTextBox('txtF_1_61').replace(/\,/g, '');
            if ($txtM_1_50 < $txtM_1_61) { $txtM_1_61 = 0; $('#txtM_1_61').val(''); IsPass = false; lstFail.push("9.1 <30 years"); }
            if ($txtF_1_50 < $txtF_1_61) { $txtF_1_61 = 0; $('#txtF_1_61').val(''); IsPass = false; lstFail.push("9.1 <30 years"); }
            var $txtM_1_62 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_61 / $txtM_1_4) * 100) : 0;
            var $txtF_1_62 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_61 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_62').val($txtM_1_62.toFixed(2));
            $('#txtF_1_62').val($txtF_1_62.toFixed(2));

            var $txtM_2_61 = +GetValTextBox('txtM_2_61').replace(/\,/g, '');
            var $txtF_2_61 = +GetValTextBox('txtF_2_61').replace(/\,/g, '');
            if ($txtM_2_50 < $txtM_2_61) { $txtM_2_61 = 0; $('#txtM_2_61').val(''); IsPass = false; lstFail.push("9.1 <30 years"); }
            if ($txtF_2_50 < $txtF_2_61) { $txtF_2_61 = 0; $('#txtF_2_61').val(''); IsPass = false; lstFail.push("9.1 <30 years"); }
            var $txtM_2_62 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_61 / $txtM_2_4) * 100) : 0;
            var $txtF_2_62 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_61 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_62').val($txtM_2_62.toFixed(2));
            $('#txtF_2_62').val($txtF_2_62.toFixed(2));

            var $txtM_3_61 = +GetValTextBox('txtM_3_61').replace(/\,/g, '');
            var $txtF_3_61 = +GetValTextBox('txtF_3_61').replace(/\,/g, '');
            if ($txtM_3_50 < $txtM_3_61) { $txtM_3_61 = 0; $('#txtM_3_61').val(''); IsPass = false; lstFail.push("9.1 <30 years"); }
            if ($txtF_3_50 < $txtF_3_61) { $txtF_3_61 = 0; $('#txtF_3_61').val(''); IsPass = false; lstFail.push("9.1 <30 years"); }
            var $txtM_3_62 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_61 / $txtM_3_4) * 100) : 0;
            var $txtF_3_62 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_61 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_62').val($txtM_3_62.toFixed(2));
            $('#txtF_3_62').val($txtF_3_62.toFixed(2));
            // #endregion

            // #region 9.2 30 - 50 years
            var $txtM_1_63 = +GetValTextBox('txtM_1_63').replace(/\,/g, '');
            var $txtF_1_63 = +GetValTextBox('txtF_1_63').replace(/\,/g, '');
            if ($txtM_1_50 < ($txtM_1_61 + $txtM_1_63)) { $txtM_1_63 = 0; $('#txtM_1_63').val(''); IsPass = false; lstFail.push("9.2 30 - 50 years"); }
            if ($txtF_1_50 < ($txtF_1_61 + $txtF_1_63)) { $txtF_1_63 = 0; $('#txtF_1_63').val(''); IsPass = false; lstFail.push("9.2 30 - 50 years"); }
            var $txtM_1_64 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_63 / $txtM_1_4) * 100) : 0;
            var $txtF_1_64 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_63 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_64').val($txtM_1_64.toFixed(2));
            $('#txtF_1_64').val($txtF_1_64.toFixed(2));

            var $txtM_2_63 = +GetValTextBox('txtM_2_63').replace(/\,/g, '');
            var $txtF_2_63 = +GetValTextBox('txtF_2_63').replace(/\,/g, '');
            if ($txtM_2_50 < ($txtM_2_61 + $txtM_2_63)) { $txtM_2_63 = 0; $('#txtM_2_63').val(''); IsPass = false; lstFail.push("9.2 30 - 50 years"); }
            if ($txtF_2_50 < ($txtF_2_61 + $txtF_2_63)) { $txtF_2_63 = 0; $('#txtF_2_63').val(''); IsPass = false; lstFail.push("9.2 30 - 50 years"); }
            var $txtM_2_64 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_63 / $txtM_2_4) * 100) : 0;
            var $txtF_2_64 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_63 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_64').val($txtM_2_64.toFixed(2));
            $('#txtF_2_64').val($txtF_2_64.toFixed(2));

            var $txtM_3_63 = +GetValTextBox('txtM_3_63').replace(/\,/g, '');
            var $txtF_3_63 = +GetValTextBox('txtF_3_63').replace(/\,/g, '');
            if ($txtM_3_50 < ($txtM_3_61 + $txtM_3_63)) { $txtM_3_63 = 0; $('#txtM_3_63').val(''); IsPass = false; lstFail.push("9.2 30 - 50 years"); }
            if ($txtF_3_50 < ($txtF_3_61 + $txtF_3_63)) { $txtF_3_63 = 0; $('#txtF_3_63').val(''); IsPass = false; lstFail.push("9.2 30 - 50 years"); }
            var $txtM_3_64 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_63 / $txtM_3_4) * 100) : 0;
            var $txtF_3_64 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_63 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_64').val($txtM_3_64.toFixed(2));
            $('#txtF_3_64').val($txtF_3_64.toFixed(2));
            // #endregion

            // #region 9.3 >50 years
            var $txtM_1_65 = ($txtM_1_50 - ($txtM_1_61 + $txtM_1_63)) >= 0 ? ($txtM_1_50 - ($txtM_1_61 + $txtM_1_63)) : 0;
            var $txtF_1_65 = ($txtF_1_50 - ($txtF_1_61 + $txtF_1_63)) >= 0 ? ($txtF_1_50 - ($txtF_1_61 + $txtF_1_63)) : 0;
            var $txtM_1_66 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_65 / $txtM_1_4) * 100) : 0;
            var $txtF_1_66 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_65 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_65').val($txtM_1_65);
            $('#txtF_1_65').val($txtF_1_65);
            $('#txtM_1_66').val($txtM_1_66.toFixed(2));
            $('#txtF_1_66').val($txtF_1_66.toFixed(2));

            var $txtM_2_65 = ($txtM_2_50 - ($txtM_2_61 + $txtM_2_63)) >= 0 ? ($txtM_2_50 - ($txtM_2_61 + $txtM_2_63)) : 0;
            var $txtF_2_65 = ($txtF_2_50 - ($txtF_2_61 + $txtF_2_63)) >= 0 ? ($txtF_2_50 - ($txtF_2_61 + $txtF_2_63)) : 0;
            var $txtM_2_66 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_65 / $txtM_2_4) * 100) : 0;
            var $txtF_2_66 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_65 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_65').val($txtM_2_65);
            $('#txtF_2_65').val($txtF_2_65);
            $('#txtM_2_66').val($txtM_2_66.toFixed(2));
            $('#txtF_2_66').val($txtF_2_66.toFixed(2));

            var $txtM_3_65 = ($txtM_3_50 - ($txtM_3_61 + $txtM_3_63)) >= 0 ? ($txtM_3_50 - ($txtM_3_61 + $txtM_3_63)) : 0;
            var $txtF_3_65 = ($txtF_3_50 - ($txtF_3_61 + $txtF_3_63)) >= 0 ? ($txtF_3_50 - ($txtF_3_61 + $txtF_3_63)) : 0;
            var $txtM_3_66 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_65 / $txtM_3_4) * 100) : 0;
            var $txtF_3_66 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_65 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_65').val($txtM_3_65);
            $('#txtF_3_65').val($txtF_3_65);
            $('#txtM_3_66').val($txtM_3_66.toFixed(2));
            $('#txtF_3_66').val($txtF_3_66.toFixed(2));
            // #endregion

            // #endregion

            // #region 11. Turnover Rate by Age Group

            // #region 11.1 < 30 years
            var $txtM_1_77 = +GetValTextBox('txtM_1_77').replace(/\,/g, '');
            var $txtF_1_77 = +GetValTextBox('txtF_1_77').replace(/\,/g, '');
            var $txtM_1_78 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_77 / $txtM_1_4) * 100) : 0;
            var $txtF_1_78 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_77 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_78').val($txtM_1_78.toFixed(2));
            $('#txtF_1_78').val($txtF_1_78.toFixed(2));

            var $txtM_2_77 = +GetValTextBox('txtM_2_77').replace(/\,/g, '');
            var $txtF_2_77 = +GetValTextBox('txtF_2_77').replace(/\,/g, '');
            var $txtM_2_78 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_77 / $txtM_2_4) * 100) : 0;
            var $txtF_2_78 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_77 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_78').val($txtM_2_78.toFixed(2));
            $('#txtF_2_78').val($txtF_2_78.toFixed(2));

            var $txtM_3_77 = +GetValTextBox('txtM_3_77').replace(/\,/g, '');
            var $txtF_3_77 = +GetValTextBox('txtF_3_77').replace(/\,/g, '');
            var $txtM_3_78 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_77 / $txtM_3_4) * 100) : 0;
            var $txtF_3_78 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_77 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_78').val($txtM_3_78.toFixed(2));
            $('#txtF_3_78').val($txtF_3_78.toFixed(2));
            // #endregion

            // #region 11.2 30 - 50 years
            var $txtM_1_79 = +GetValTextBox('txtM_1_79').replace(/\,/g, '');
            var $txtF_1_79 = +GetValTextBox('txtF_1_79').replace(/\,/g, '');
            var $txtM_1_80 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_79 / $txtM_1_4) * 100) : 0;
            var $txtF_1_80 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_79 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_80').val($txtM_1_80.toFixed(2));
            $('#txtF_1_80').val($txtF_1_80.toFixed(2));

            var $txtM_2_79 = +GetValTextBox('txtM_2_79').replace(/\,/g, '');
            var $txtF_2_79 = +GetValTextBox('txtF_2_79').replace(/\,/g, '');
            var $txtM_2_80 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_79 / $txtM_2_4) * 100) : 0;
            var $txtF_2_80 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_79 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_80').val($txtM_2_80.toFixed(2));
            $('#txtF_2_80').val($txtF_2_80.toFixed(2));

            var $txtM_3_79 = +GetValTextBox('txtM_3_79').replace(/\,/g, '');
            var $txtF_3_79 = +GetValTextBox('txtF_3_79').replace(/\,/g, '');
            var $txtM_3_80 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_79 / $txtM_3_4) * 100) : 0;
            var $txtF_3_80 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_79 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_80').val($txtM_3_80.toFixed(2));
            $('#txtF_3_80').val($txtF_3_80.toFixed(2));
            // #endregion

            // #region 11.3 > 50 years
            var $txtM_1_81 = +GetValTextBox('txtM_1_81').replace(/\,/g, '');
            var $txtF_1_81 = +GetValTextBox('txtF_1_81').replace(/\,/g, '');
            var $txtM_1_82 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_81 / $txtM_1_4) * 100) : 0;
            var $txtF_1_82 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_81 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_82').val($txtM_1_82.toFixed(2));
            $('#txtF_1_82').val($txtF_1_82.toFixed(2));

            var $txtM_2_81 = +GetValTextBox('txtM_2_81').replace(/\,/g, '');
            var $txtF_2_81 = +GetValTextBox('txtF_2_81').replace(/\,/g, '');
            var $txtM_2_82 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_81 / $txtM_2_4) * 100) : 0;
            var $txtF_2_82 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_81 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_82').val($txtM_2_82.toFixed(2));
            $('#txtF_2_82').val($txtF_2_82.toFixed(2));

            var $txtM_3_81 = +GetValTextBox('txtM_3_81').replace(/\,/g, '');
            var $txtF_3_81 = +GetValTextBox('txtF_3_81').replace(/\,/g, '');
            var $txtM_3_82 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_81 / $txtM_3_4) * 100) : 0;
            var $txtF_3_82 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_81 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_82').val($txtM_3_82.toFixed(2));
            $('#txtF_3_82').val($txtF_3_82.toFixed(2));
            // #endregion

            // #endregion            

            // #region 10. Turnover

            // #region 10.1 Total employee turnover rate
            var $txtM_1_69 = $txtM_1_77 + $txtM_1_79 + $txtM_1_81;
            var $txtF_1_69 = $txtF_1_77 + $txtF_1_79 + $txtF_1_81;
            var $txtM_1_68 = $txtM_1_69 + $txtF_1_69;
            var $txtM_1_70 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_68 / $txtM_1_4) * 100) : 0;
            var $txtM_1_71 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_69 / $txtM_1_4) * 100) : 0;
            var $txtF_1_71 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_69 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_68').val($txtM_1_68);
            $('#txtM_1_69').val($txtM_1_69);
            $('#txtF_1_69').val($txtF_1_69);
            $('#txtM_1_70').val($txtM_1_70.toFixed(2));
            $('#txtM_1_71').val($txtM_1_71.toFixed(2));
            $('#txtF_1_71').val($txtF_1_71.toFixed(2));

            var $txtM_2_69 = $txtM_2_77 + $txtM_2_79 + $txtM_2_81;
            var $txtF_2_69 = $txtF_2_77 + $txtF_2_79 + $txtF_2_81;
            var $txtM_2_68 = $txtM_2_69 + $txtF_2_69;
            var $txtM_2_70 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_68 / $txtM_2_4) * 100) : 0;
            var $txtM_2_71 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_69 / $txtM_2_4) * 100) : 0;
            var $txtF_2_71 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_69 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_68').val($txtM_2_68);
            $('#txtM_2_69').val($txtM_2_69);
            $('#txtF_2_69').val($txtF_2_69);
            $('#txtM_2_70').val($txtM_2_70.toFixed(2));
            $('#txtM_2_71').val($txtM_2_71.toFixed(2));
            $('#txtF_2_71').val($txtF_2_71.toFixed(2));

            var $txtM_3_69 = $txtM_3_77 + $txtM_3_79 + $txtM_3_81;
            var $txtF_3_69 = $txtF_3_77 + $txtF_3_79 + $txtF_3_81;
            var $txtM_3_68 = $txtM_3_69 + $txtF_3_69;
            var $txtM_3_70 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_68 / $txtM_3_4) * 100) : 0;
            var $txtM_3_71 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_69 / $txtM_3_4) * 100) : 0;
            var $txtF_3_71 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_69 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_68').val($txtM_3_68);
            $('#txtM_3_69').val($txtM_3_69);
            $('#txtF_3_69').val($txtF_3_69);
            $('#txtM_3_70').val($txtM_3_70.toFixed(2));
            $('#txtM_3_71').val($txtM_3_71.toFixed(2));
            $('#txtF_3_71').val($txtF_3_71.toFixed(2));
            // #endregion

            // #region 10.2 Voluntary employee turnover rate
            var $txtM_1_73 = +GetValTextBox('txtM_1_73').replace(/\,/g, '');
            var $txtF_1_73 = +GetValTextBox('txtF_1_73').replace(/\,/g, '');
            if ($txtM_1_69 < $txtM_1_73) { $txtM_1_73 = 0; $('#txtM_1_73').val(''); IsPass = false; lstFail.push("10.2 Voluntary employee turnover rate"); }
            if ($txtF_1_69 < $txtF_1_73) { $txtF_1_73 = 0; $('#txtF_1_73').val(''); IsPass = false; lstFail.push("10.2 Voluntary employee turnover rate"); }
            var $txtM_1_72 = $txtM_1_73 + $txtF_1_73;
            var $txtM_1_74 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_72 / $txtM_1_4) * 100) : 0;
            var $txtM_1_75 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_73 / $txtM_1_4) * 100) : 0;
            var $txtF_1_75 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_73 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_72').val($txtM_1_72);
            $('#txtM_1_74').val($txtM_1_74.toFixed(2));
            $('#txtM_1_75').val($txtM_1_75.toFixed(2));
            $('#txtF_1_75').val($txtF_1_75.toFixed(2));

            var $txtM_2_73 = +GetValTextBox('txtM_2_73').replace(/\,/g, '');
            var $txtF_2_73 = +GetValTextBox('txtF_2_73').replace(/\,/g, '');
            if ($txtM_2_69 < $txtM_2_73) { $txtM_2_73 = 0; $('#txtM_2_73').val(''); IsPass = false; lstFail.push("10.2 Voluntary employee turnover rate"); }
            if ($txtF_2_69 < $txtF_2_73) { $txtF_2_73 = 0; $('#txtF_2_73').val(''); IsPass = false; lstFail.push("10.2 Voluntary employee turnover rate"); }
            var $txtM_2_72 = $txtM_2_73 + $txtF_2_73;
            var $txtM_2_74 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_72 / $txtM_2_4) * 100) : 0;
            var $txtM_2_75 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_73 / $txtM_2_4) * 100) : 0;
            var $txtF_2_75 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_73 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_72').val($txtM_2_72);
            $('#txtM_2_74').val($txtM_2_74.toFixed(2));
            $('#txtM_2_75').val($txtM_2_75.toFixed(2));
            $('#txtF_2_75').val($txtF_2_75.toFixed(2));

            var $txtM_3_73 = +GetValTextBox('txtM_3_73').replace(/\,/g, '');
            var $txtF_3_73 = +GetValTextBox('txtF_3_73').replace(/\,/g, '');
            if ($txtM_3_69 < $txtM_3_73) { $txtM_3_73 = 0; $('#txtM_3_73').val(''); IsPass = false; lstFail.push("10.2 Voluntary employee turnover rate"); }
            if ($txtF_3_69 < $txtF_3_73) { $txtF_3_73 = 0; $('#txtF_3_73').val(''); IsPass = false; lstFail.push("10.2 Voluntary employee turnover rate"); }
            var $txtM_3_72 = $txtM_3_73 + $txtF_3_73;
            var $txtM_3_74 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_72 / $txtM_3_4) * 100) : 0;
            var $txtM_3_75 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_73 / $txtM_3_4) * 100) : 0;
            var $txtF_3_75 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_73 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_72').val($txtM_3_72);
            $('#txtM_3_74').val($txtM_3_74.toFixed(2));
            $('#txtM_3_75').val($txtM_3_75.toFixed(2));
            $('#txtF_3_75').val($txtF_3_75.toFixed(2));
            // #endregion

            // #endregion

            // #region 12. Turnover by Area

            // #region 12.1 Rayong
            var $txtM_1_84 = +GetValTextBox('txtM_1_84').replace(/\,/g, '');
            var $txtF_1_84 = +GetValTextBox('txtF_1_84').replace(/\,/g, '');
            if ($txtM_1_69 < $txtM_1_84) { $txtM_1_84 = 0; $('#txtM_1_84').val(''); IsPass = false; lstFail.push("12.1 Rayong"); }
            if ($txtF_1_69 < $txtF_1_84) { $txtF_1_84 = 0; $('#txtF_1_84').val(''); IsPass = false; lstFail.push("12.1 Rayong"); }
            var $txtM_1_85 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_84 / $txtM_1_4) * 100) : 0;
            var $txtF_1_85 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_84 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_85').val($txtM_1_85.toFixed(2));
            $('#txtF_1_85').val($txtF_1_85.toFixed(2));

            var $txtM_2_84 = +GetValTextBox('txtM_2_84').replace(/\,/g, '');
            var $txtF_2_84 = +GetValTextBox('txtF_2_84').replace(/\,/g, '');
            if ($txtM_2_69 < $txtM_2_84) { $txtM_2_84 = 0; $('#txtM_2_84').val(''); IsPass = false; lstFail.push("12.1 Rayong"); }
            if ($txtF_2_69 < $txtF_2_84) { $txtF_2_84 = 0; $('#txtF_2_84').val(''); IsPass = false; lstFail.push("12.1 Rayong"); }
            var $txtM_2_85 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_84 / $txtM_2_4) * 100) : 0;
            var $txtF_2_85 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_84 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_85').val($txtM_2_85.toFixed(2));
            $('#txtF_2_85').val($txtF_2_85.toFixed(2));

            var $txtM_3_84 = +GetValTextBox('txtM_3_84').replace(/\,/g, '');
            var $txtF_3_84 = +GetValTextBox('txtF_3_84').replace(/\,/g, '');
            if ($txtM_3_69 < $txtM_3_84) { $txtM_3_84 = 0; $('#txtM_3_84').val(''); IsPass = false; lstFail.push("12.1 Rayong"); }
            if ($txtF_3_69 < $txtF_3_84) { $txtF_3_84 = 0; $('#txtF_3_84').val(''); IsPass = false; lstFail.push("12.1 Rayong"); }
            var $txtM_3_85 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_84 / $txtM_3_4) * 100) : 0;
            var $txtF_3_85 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_84 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_85').val($txtM_3_85.toFixed(2));
            $('#txtF_3_85').val($txtF_3_85.toFixed(2));
            // #endregion

            // #region 12.2 Bangkok
            var $txtM_1_86 = +GetValTextBox('txtM_1_86').replace(/\,/g, '');
            var $txtF_1_86 = +GetValTextBox('txtF_1_86').replace(/\,/g, '');
            if ($txtM_1_69 < ($txtM_1_84 + $txtM_1_86)) { $txtM_1_86 = 0; $('#txtM_1_86').val(''); IsPass = false; lstFail.push("12.2 Bangkok"); }
            if ($txtF_1_69 < ($txtF_1_84 + $txtF_1_86)) { $txtF_1_86 = 0; $('#txtF_1_86').val(''); IsPass = false; lstFail.push("12.2 Bangkok"); }
            var $txtM_1_87 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_86 / $txtM_1_4) * 100) : 0;
            var $txtF_1_87 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_86 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_87').val($txtM_1_87.toFixed(2));
            $('#txtF_1_87').val($txtF_1_87.toFixed(2));

            var $txtM_2_86 = +GetValTextBox('txtM_2_86').replace(/\,/g, '');
            var $txtF_2_86 = +GetValTextBox('txtF_2_86').replace(/\,/g, '');
            if ($txtM_2_69 < ($txtM_2_84 + $txtM_2_86)) { $txtM_2_86 = 0; $('#txtM_2_86').val(''); IsPass = false; lstFail.push("12.2 Bangkok"); }
            if ($txtF_2_69 < ($txtF_2_84 + $txtF_2_86)) { $txtF_2_86 = 0; $('#txtF_2_86').val(''); IsPass = false; lstFail.push("12.2 Bangkok"); }
            var $txtM_2_87 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_86 / $txtM_2_4) * 100) : 0;
            var $txtF_2_87 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_86 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_87').val($txtM_2_87.toFixed(2));
            $('#txtF_2_87').val($txtF_2_87.toFixed(2));

            var $txtM_3_86 = +GetValTextBox('txtM_3_86').replace(/\,/g, '');
            var $txtF_3_86 = +GetValTextBox('txtF_3_86').replace(/\,/g, '');
            if ($txtM_3_69 < ($txtM_3_84 + $txtM_3_86)) { $txtM_3_86 = 0; $('#txtM_3_86').val(''); IsPass = false; lstFail.push("12.2 Bangkok"); }
            if ($txtF_3_69 < ($txtF_3_84 + $txtF_3_86)) { $txtF_3_86 = 0; $('#txtF_3_86').val(''); IsPass = false; lstFail.push("12.2 Bangkok"); }
            var $txtM_3_87 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_86 / $txtM_3_4) * 100) : 0;
            var $txtF_3_87 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_86 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_87').val($txtM_3_87.toFixed(2));
            $('#txtF_3_87').val($txtF_3_87.toFixed(2));
            // #endregion

            // #region 12.3 Other provinces
            var $txtM_1_88 = ($txtM_1_69 - ($txtM_1_84 + $txtM_1_86)) >= 0 ? ($txtM_1_69 - ($txtM_1_84 + $txtM_1_86)) : 0;
            var $txtF_1_88 = ($txtF_1_69 - ($txtF_1_84 + $txtF_1_86)) >= 0 ? ($txtF_1_69 - ($txtF_1_84 + $txtF_1_86)) : 0;
            var $txtM_1_89 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_88 / $txtM_1_4) * 100) : 0;
            var $txtF_1_89 = $txtM_1_4 > 0 ? CalNumber(($txtF_1_88 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_88').val($txtM_1_88);
            $('#txtF_1_88').val($txtF_1_88);
            $('#txtM_1_89').val($txtM_1_89.toFixed(2));
            $('#txtF_1_89').val($txtF_1_89.toFixed(2));

            var $txtM_2_88 = ($txtM_2_69 - ($txtM_2_84 + $txtM_2_86)) >= 0 ? ($txtM_2_69 - ($txtM_2_84 + $txtM_2_86)) : 0;
            var $txtF_2_88 = ($txtF_2_69 - ($txtF_2_84 + $txtF_2_86)) >= 0 ? ($txtF_2_69 - ($txtF_2_84 + $txtF_2_86)) : 0;
            var $txtM_2_89 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_88 / $txtM_2_4) * 100) : 0;
            var $txtF_2_89 = $txtM_2_4 > 0 ? CalNumber(($txtF_2_88 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_88').val($txtM_2_88);
            $('#txtF_2_88').val($txtF_2_88);
            $('#txtM_2_89').val($txtM_2_89.toFixed(2));
            $('#txtF_2_89').val($txtF_2_89.toFixed(2));

            var $txtM_3_88 = ($txtM_3_69 - ($txtM_3_84 + $txtM_3_86)) >= 0 ? ($txtM_3_69 - ($txtM_3_84 + $txtM_3_86)) : 0;
            var $txtF_3_88 = ($txtF_3_69 - ($txtF_3_84 + $txtF_3_86)) >= 0 ? ($txtF_3_69 - ($txtF_3_84 + $txtF_3_86)) : 0;
            var $txtM_3_89 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_88 / $txtM_3_4) * 100) : 0;
            var $txtF_3_89 = $txtM_3_4 > 0 ? CalNumber(($txtF_3_88 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_88').val($txtM_3_88);
            $('#txtF_3_88').val($txtF_3_88);
            $('#txtM_3_89').val($txtM_3_89.toFixed(2));
            $('#txtF_3_89').val($txtF_3_89.toFixed(2));
            // #endregion

            // #region 12.4 Average hiring cost / FTE in the last fiscal year
            var $txtM_1_90 = +GetValTextBox('txtM_1_90').replace(/\,/g, '');
            var $txtM_2_90 = +GetValTextBox('txtM_2_90').replace(/\,/g, '');
            var $txtM_3_90 = +GetValTextBox('txtM_3_90').replace(/\,/g, '');
            // #endregion

            // #endregion            

            // #region 13. Parental Leave

            // #region 13.1 Number of employees entitled to parental leave
            var $txtM_1_92 = +GetValTextBox('txtM_1_92').replace(/\,/g, '');
            var $txtF_1_92 = +GetValTextBox('txtF_1_92').replace(/\,/g, '');
            var $txtM_2_92 = +GetValTextBox('txtM_2_92').replace(/\,/g, '');
            var $txtF_2_92 = +GetValTextBox('txtF_2_92').replace(/\,/g, '');
            var $txtM_3_92 = +GetValTextBox('txtM_3_92').replace(/\,/g, '');
            var $txtF_3_92 = +GetValTextBox('txtF_3_92').replace(/\,/g, '');
            // #endregion

            // #region 13.2 Number of employees taking parental leave
            var $txtM_1_93 = +GetValTextBox('txtM_1_93').replace(/\,/g, '');
            var $txtF_1_93 = +GetValTextBox('txtF_1_93').replace(/\,/g, '');
            var $txtM_2_93 = +GetValTextBox('txtM_2_93').replace(/\,/g, '');
            var $txtF_2_93 = +GetValTextBox('txtF_2_93').replace(/\,/g, '');
            var $txtM_3_93 = +GetValTextBox('txtM_3_93').replace(/\,/g, '');
            var $txtF_3_93 = +GetValTextBox('txtF_3_93').replace(/\,/g, '');
            // #endregion

            // #region 13.3 Number of employees returning to work after parental leave
            var $txtM_1_94 = +GetValTextBox('txtM_1_94').replace(/\,/g, '');
            var $txtF_1_94 = +GetValTextBox('txtF_1_94').replace(/\,/g, '');
            var $txtM_2_94 = +GetValTextBox('txtM_2_94').replace(/\,/g, '');
            var $txtF_2_94 = +GetValTextBox('txtF_2_94').replace(/\,/g, '');
            var $txtM_3_94 = +GetValTextBox('txtM_3_94').replace(/\,/g, '');
            var $txtF_3_94 = +GetValTextBox('txtF_3_94').replace(/\,/g, '');
            // #endregion

            // #region 13.4 Number of employees returning to work after parental leave who were still employed for 12 months after returning
            var $txtM_1_95 = +GetValTextBox('txtM_1_95').replace(/\,/g, '');
            var $txtF_1_95 = +GetValTextBox('txtF_1_95').replace(/\,/g, '');
            var $txtM_2_95 = +GetValTextBox('txtM_2_95').replace(/\,/g, '');
            var $txtF_2_95 = +GetValTextBox('txtF_2_95').replace(/\,/g, '');
            var $txtM_3_95 = +GetValTextBox('txtM_3_95').replace(/\,/g, '');
            var $txtF_3_95 = +GetValTextBox('txtF_3_95').replace(/\,/g, '');
            // #endregion

            // #region 13.5 Employee returning to work retention rate
            var $txt_1_93 = $txtM_1_93 + $txtF_1_93;
            var $txtM_1_96 = $txt_1_93 > 0 ? CalNumber(($txtM_1_95 / $txt_1_93) * 100) : 0;
            var $txtF_1_96 = $txt_1_93 > 0 ? CalNumber(($txtF_1_95 / $txt_1_93) * 100) : 0;
            $('#txtM_1_96').val($txtM_1_96.toFixed(2));
            $('#txtF_1_96').val($txtF_1_96.toFixed(2));

            var $txt_2_93 = $txtM_2_93 + $txtF_2_93;
            var $txtM_2_96 = $txt_2_93 > 0 ? CalNumber(($txtM_2_95 / $txt_2_93) * 100) : 0;
            var $txtF_2_96 = $txt_2_93 > 0 ? CalNumber(($txtF_2_95 / $txt_2_93) * 100) : 0;
            $('#txtM_2_96').val($txtM_2_96.toFixed(2));
            $('#txtF_2_96').val($txtF_2_96.toFixed(2));

            var $txt_3_93 = $txtM_3_93 + $txtF_3_93;
            var $txtM_3_96 = $txt_3_93 > 0 ? CalNumber(($txtM_3_95 / $txt_3_93) * 100) : 0;
            var $txtF_3_96 = $txt_3_93 > 0 ? CalNumber(($txtF_3_95 / $txt_3_93) * 100) : 0;
            $('#txtM_3_96').val($txtM_3_96.toFixed(2));
            $('#txtF_3_96').val($txtF_3_96.toFixed(2));
            // #endregion           

            // #endregion

            // #region 14. Training and Development

            // #region 14.1 Average hours per FTE of training and development     
            var $txtM_1_99 = +GetValTextBox('txtM_1_99').replace(/\,/g, '');
            var $txtM_2_99 = +GetValTextBox('txtM_2_99').replace(/\,/g, '');
            var $txtM_3_99 = +GetValTextBox('txtM_3_99').replace(/\,/g, '');
            var $txtF_1_99 = +GetValTextBox('txtF_1_99').replace(/\,/g, '');
            var $txtF_2_99 = +GetValTextBox('txtF_2_99').replace(/\,/g, '');
            var $txtF_3_99 = +GetValTextBox('txtF_3_99').replace(/\,/g, '');

            var $txtM_1_98 = CalNumber($txtM_1_99 + $txtF_1_99);
            var $txtM_2_98 = CalNumber($txtM_2_99 + $txtF_2_99);
            var $txtM_3_98 = CalNumber($txtM_3_99 + $txtF_3_99);
            $('#txtM_1_98').val($txtM_1_98.toFixed(2));
            $('#txtM_2_98').val($txtM_2_98.toFixed(2));
            $('#txtM_3_98').val($txtM_3_98.toFixed(2));
            // #endregion

            // #region 14.2 Average amount spent per FTE on training and development
            var $txtM_1_100 = +GetValTextBox('txtM_1_100').replace(/\,/g, '');
            var $txtM_2_100 = +GetValTextBox('txtM_2_100').replace(/\,/g, '');
            var $txtM_3_100 = +GetValTextBox('txtM_3_100').replace(/\,/g, '');
            // #endregion

            // #region 14.3 Percentage of open positions filled by internal candidates
            var $txtM_1_101 = +GetValTextBox('txtM_1_101').replace(/\,/g, '');
            var $txtM_2_101 = +GetValTextBox('txtM_2_101').replace(/\,/g, '');
            var $txtM_3_101 = +GetValTextBox('txtM_3_101').replace(/\,/g, '');
            // #endregion

            // #region 14.4 Total investment on employees training
            var $txtM_1_102 = +GetValTextBox('txtM_1_102').replace(/\,/g, '');
            var $txtM_2_102 = +GetValTextBox('txtM_2_102').replace(/\,/g, '');
            var $txtM_3_102 = +GetValTextBox('txtM_3_102').replace(/\,/g, '');
            var $txtF_1_102 = +GetValTextBox('txtF_1_102').replace(/\,/g, '');
            var $txtF_2_102 = +GetValTextBox('txtF_2_102').replace(/\,/g, '');
            var $txtF_3_102 = +GetValTextBox('txtF_3_102').replace(/\,/g, '');
            // #endregion

            // #region 14.6 Executive
            var $txtM_1_104 = +GetValTextBox('txtM_1_104').replace(/\,/g, '');
            var $txtM_2_104 = +GetValTextBox('txtM_2_104').replace(/\,/g, '');
            var $txtM_3_104 = +GetValTextBox('txtM_3_104').replace(/\,/g, '');
            var $txtF_1_104 = +GetValTextBox('txtF_1_104').replace(/\,/g, '');
            var $txtF_2_104 = +GetValTextBox('txtF_2_104').replace(/\,/g, '');
            var $txtF_3_104 = +GetValTextBox('txtF_3_104').replace(/\,/g, '');
            // #endregion

            // #region 14.7 Middle management
            var $txtM_1_105 = +GetValTextBox('txtM_1_105').replace(/\,/g, '');
            var $txtM_2_105 = +GetValTextBox('txtM_2_105').replace(/\,/g, '');
            var $txtM_3_105 = +GetValTextBox('txtM_3_105').replace(/\,/g, '');
            var $txtF_1_105 = +GetValTextBox('txtF_1_105').replace(/\,/g, '');
            var $txtF_2_105 = +GetValTextBox('txtF_2_105').replace(/\,/g, '');
            var $txtF_3_105 = +GetValTextBox('txtF_3_105').replace(/\,/g, '');
            // #endregion

            // #region 14.8 Senior
            var $txtM_1_106 = +GetValTextBox('txtM_1_106').replace(/\,/g, '');
            var $txtM_2_106 = +GetValTextBox('txtM_2_106').replace(/\,/g, '');
            var $txtM_3_106 = +GetValTextBox('txtM_3_106').replace(/\,/g, '');
            var $txtF_1_106 = +GetValTextBox('txtF_1_106').replace(/\,/g, '');
            var $txtF_2_106 = +GetValTextBox('txtF_2_106').replace(/\,/g, '');
            var $txtF_3_106 = +GetValTextBox('txtF_3_106').replace(/\,/g, '');
            // #endregion

            // #region 14.9 Employee
            var $txtM_1_107 = +GetValTextBox('txtM_1_107').replace(/\,/g, '');
            var $txtM_2_107 = +GetValTextBox('txtM_2_107').replace(/\,/g, '');
            var $txtM_3_107 = +GetValTextBox('txtM_3_107').replace(/\,/g, '');
            var $txtF_1_107 = +GetValTextBox('txtF_1_107').replace(/\,/g, '');
            var $txtF_2_107 = +GetValTextBox('txtF_2_107').replace(/\,/g, '');
            var $txtF_3_107 = +GetValTextBox('txtF_3_107').replace(/\,/g, '');
            // #endregion

            // #region 14.5 Average hours of training by employee category (level)
            var $txtM_1_103 = CalNumber(($txtM_1_104 + $txtM_1_105 + $txtM_1_106 + $txtM_1_107) / 4);
            var $txtM_2_103 = CalNumber(($txtM_2_104 + $txtM_2_105 + $txtM_2_106 + $txtM_2_107) / 4);
            var $txtM_3_103 = CalNumber(($txtM_3_104 + $txtM_3_105 + $txtM_3_106 + $txtM_3_107) / 4);

            var $txtF_1_103 = CalNumber(($txtF_1_104 + $txtF_1_105 + $txtF_1_106 + $txtF_1_107) / 4);
            var $txtF_2_103 = CalNumber(($txtF_2_104 + $txtF_2_105 + $txtF_2_106 + $txtF_2_107) / 4);
            var $txtF_3_103 = CalNumber(($txtF_3_104 + $txtF_3_105 + $txtF_3_106 + $txtF_3_107) / 4);

            $('#txtM_1_103').val($txtM_1_103.toFixed(2));
            $('#txtM_2_103').val($txtM_2_103.toFixed(2));
            $('#txtM_3_103').val($txtM_3_103.toFixed(2));
            $('#txtF_1_103').val($txtF_1_103.toFixed(2));
            $('#txtF_2_103').val($txtF_2_103.toFixed(2));
            $('#txtF_3_103').val($txtF_3_103.toFixed(2));
            // #endregion

            // #region 14.10 Quantitative financial benefit of human capital investment over time (e.g. EVA/FTEs, HROI)
            var $txtM_1_108 = +GetValTextBox('txtM_1_108').replace(/\,/g, '');
            var $txtM_2_108 = +GetValTextBox('txtM_2_108').replace(/\,/g, '');
            var $txtM_3_108 = +GetValTextBox('txtM_3_108').replace(/\,/g, '');
            // #endregion

            // #region 14.11 a) Total Revenue
            var $txtM_1_109 = +GetValTextBox('txtM_1_109').replace(/\,/g, '');
            var $txtM_2_109 = +GetValTextBox('txtM_2_109').replace(/\,/g, '');
            var $txtM_3_109 = +GetValTextBox('txtM_3_109').replace(/\,/g, '');
            // #endregion

            // #region 14.12 b) Total Operating expenses
            var $txtM_1_110 = +GetValTextBox('txtM_1_110').replace(/\,/g, '');
            var $txtM_2_110 = +GetValTextBox('txtM_2_110').replace(/\,/g, '');
            var $txtM_3_110 = +GetValTextBox('txtM_3_110').replace(/\,/g, '');
            // #endregion

            // #region 14.13 c) Total employee-related expense (Salaries+Benefits)
            var $txtM_1_111 = +GetValTextBox('txtM_1_111').replace(/\,/g, '');
            var $txtM_2_111 = +GetValTextBox('txtM_2_111').replace(/\,/g, '');
            var $txtM_3_111 = +GetValTextBox('txtM_3_111').replace(/\,/g, '');
            // #endregion

            // #region 14.14 Resulting HC ROI : (a - (b-c)) / c            
            var $txtM_1_112 = $txtM_1_111 > 0 ? CalNumber(($txtM_1_109 - ($txtM_1_110 - $txtM_1_111)) / $txtM_1_111) : 0;
            var $txtM_2_112 = $txtM_2_111 > 0 ? CalNumber(($txtM_2_109 - ($txtM_2_110 - $txtM_2_111)) / $txtM_2_111) : 0;
            var $txtM_3_112 = $txtM_3_111 > 0 ? CalNumber(($txtM_3_109 - ($txtM_3_110 - $txtM_3_111)) / $txtM_3_111) : 0;
            $('#txtM_1_112').val($txtM_1_112.toFixed(2));
            $('#txtM_2_112').val($txtM_2_112.toFixed(2));
            $('#txtM_3_112').val($txtM_3_112.toFixed(2));
            // #endregion

            // #region 14.15 Total FTEs
            var $txtM_1_113 = CalNumber($txtM_1_28 + $txtF_1_28 - $txtM_1_98);
            var $txtM_2_113 = CalNumber($txtM_2_28 + $txtF_2_28 - $txtM_2_98);
            var $txtM_3_113 = CalNumber($txtM_3_28 + $txtF_3_28 - $txtM_3_98);

            $('#txtM_1_113').val($txtM_1_113);
            $('#txtM_2_113').val($txtM_2_113);
            $('#txtM_3_113').val($txtM_3_113);
            // #endregion

            // #endregion

            // #region 15. Talent Attraction and Retention

            // #region 15.1 Management by objectives: systematic use of agreed measurable targets by line superior 
            var $txtM_1_115 = +GetValTextBox('txtM_1_115').replace(/\,/g, '');
            var $txtM_2_115 = +GetValTextBox('txtM_2_115').replace(/\,/g, '');
            var $txtM_3_115 = +GetValTextBox('txtM_3_115').replace(/\,/g, '');
            // #endregion

            // #region 15.2 Multidimensional performance appraisal
            var $txtM_1_116 = +GetValTextBox('txtM_1_116').replace(/\,/g, '');
            var $txtM_2_116 = +GetValTextBox('txtM_2_116').replace(/\,/g, '');
            var $txtM_3_116 = +GetValTextBox('txtM_3_116').replace(/\,/g, '');
            // #endregion

            // #region 15.3 Formal comparative ranking of employees within one employee category
            var $txtM_1_117 = +GetValTextBox('txtM_1_117').replace(/\,/g, '');
            var $txtM_2_117 = +GetValTextBox('txtM_2_117').replace(/\,/g, '');
            var $txtM_3_117 = +GetValTextBox('txtM_3_117').replace(/\,/g, '');
            // #endregion

            // #region 15.4 Employee engagement result
            var $txtM_1_118 = +GetValTextBox('txtM_1_118').replace(/\,/g, '');
            var $txtM_2_118 = +GetValTextBox('txtM_2_118').replace(/\,/g, '');
            var $txtM_3_118 = +GetValTextBox('txtM_3_118').replace(/\,/g, '');

            var $txtM_1_119 = +GetValTextBox('txtM_1_119').replace(/\,/g, '');
            var $txtM_2_119 = +GetValTextBox('txtM_2_119').replace(/\,/g, '');
            var $txtM_3_119 = +GetValTextBox('txtM_3_119').replace(/\,/g, '');
            var $txtF_1_119 = +GetValTextBox('txtF_1_119').replace(/\,/g, '');
            var $txtF_2_119 = +GetValTextBox('txtF_2_119').replace(/\,/g, '');
            var $txtF_3_119 = +GetValTextBox('txtF_3_119').replace(/\,/g, '');
            // #endregion

            // #region 15.5 Employee engagement target
            var $txtM_1_120 = +GetValTextBox('txtM_1_120').replace(/\,/g, '');
            var $txtM_2_120 = +GetValTextBox('txtM_2_120').replace(/\,/g, '');
            var $txtM_3_120 = +GetValTextBox('txtM_3_120').replace(/\,/g, '');

            var $txtM_1_121 = +GetValTextBox('txtM_1_121').replace(/\,/g, '');
            var $txtM_2_121 = +GetValTextBox('txtM_2_121').replace(/\,/g, '');
            var $txtM_3_121 = +GetValTextBox('txtM_3_121').replace(/\,/g, '');
            var $txtF_1_121 = +GetValTextBox('txtF_1_121').replace(/\,/g, '');
            var $txtF_2_121 = +GetValTextBox('txtF_2_121').replace(/\,/g, '');
            var $txtF_3_121 = +GetValTextBox('txtF_3_121').replace(/\,/g, '');
            // #endregion

            // #region 15.6 Coverage
            var $txtM_1_122 = +GetValTextBox('txtM_1_122').replace(/\,/g, '');
            var $txtM_2_122 = +GetValTextBox('txtM_2_122').replace(/\,/g, '');
            var $txtM_3_122 = +GetValTextBox('txtM_3_122').replace(/\,/g, '');

            var $txtM_1_123 = +GetValTextBox('txtM_1_123').replace(/\,/g, '');
            var $txtM_2_123 = +GetValTextBox('txtM_2_123').replace(/\,/g, '');
            var $txtM_3_123 = +GetValTextBox('txtM_3_123').replace(/\,/g, '');
            var $txtF_1_123 = +GetValTextBox('txtF_1_123').replace(/\,/g, '');
            var $txtF_2_123 = +GetValTextBox('txtF_2_123').replace(/\,/g, '');
            var $txtF_3_123 = +GetValTextBox('txtF_3_123').replace(/\,/g, '');
            // #endregion

            // #endregion

            // #region 16. Employee Receiving Regular Performance and Career Development Reviews (Excl. People in Unclassified Group)

            // #region 16.1 Executive           
            var $txtM_1_125 = +GetValTextBox('txtM_1_125').replace(/\,/g, '');
            var $txtF_1_125 = +GetValTextBox('txtF_1_125').replace(/\,/g, '');
            if ($txtM_1_39 < $txtM_1_125) { $txtM_1_125 = 0; $('#txtM_1_125').val(''); IsPass = false; lstFail.push("16.1 Executive"); }
            if ($txtF_1_39 < $txtF_1_125) { $txtF_1_125 = 0; $('#txtF_1_125').val(''); IsPass = false; lstFail.push("16.1 Executive"); }
            var $txtM_1_126 = $txtM_1_39 > 0 ? CalNumber(($txtM_1_125 / $txtM_1_39) * 100) : 0;
            var $txtF_1_126 = $txtF_1_39 > 0 ? CalNumber(($txtF_1_125 / $txtF_1_39) * 100) : 0;
            $('#txtM_1_126').val($txtM_1_126.toFixed(2));
            $('#txtF_1_126').val($txtF_1_126.toFixed(2));

            var $txtM_2_125 = +GetValTextBox('txtM_2_125').replace(/\,/g, '');
            var $txtF_2_125 = +GetValTextBox('txtF_2_125').replace(/\,/g, '');
            if ($txtM_1_39 < $txtM_1_125) { $txtM_1_125 = 0; $('#txtM_1_125').val(''); IsPass = false; lstFail.push("16.1 Executive"); }
            if ($txtF_1_39 < $txtF_1_125) { $txtF_1_125 = 0; $('#txtF_1_125').val(''); IsPass = false; lstFail.push("16.1 Executive"); }
            var $txtM_2_126 = $txtM_2_39 > 0 ? CalNumber(($txtM_2_125 / $txtM_2_39) * 100) : 0;
            var $txtF_2_126 = $txtF_2_39 > 0 ? CalNumber(($txtF_2_125 / $txtF_2_39) * 100) : 0;
            $('#txtM_2_126').val($txtM_2_126.toFixed(2));
            $('#txtF_2_126').val($txtF_2_126.toFixed(2));

            var $txtM_3_125 = +GetValTextBox('txtM_3_125').replace(/\,/g, '');
            var $txtF_3_125 = +GetValTextBox('txtF_3_125').replace(/\,/g, '');
            if ($txtM_3_39 < $txtM_3_125) { $txtM_3_125 = 0; $('#txtM_3_125').val(''); IsPass = false; lstFail.push("16.1 Executive"); }
            if ($txtF_3_39 < $txtF_3_125) { $txtF_3_125 = 0; $('#txtF_3_125').val(''); IsPass = false; lstFail.push("16.1 Executive"); }
            var $txtM_3_126 = $txtM_3_39 > 0 ? CalNumber(($txtM_3_125 / $txtM_3_39) * 100) : 0;
            var $txtF_3_126 = $txtF_3_39 > 0 ? CalNumber(($txtF_3_125 / $txtF_3_39) * 100) : 0;
            $('#txtM_3_126').val($txtM_3_126.toFixed(2));
            $('#txtF_3_126').val($txtF_3_126.toFixed(2));
            // #endregion

            // #region 16.2 Middle management
            var $txtM_1_127 = +GetValTextBox('txtM_1_127').replace(/\,/g, '');
            var $txtF_1_127 = +GetValTextBox('txtF_1_127').replace(/\,/g, '');
            if ($txtM_1_41 < $txtM_1_127) { $txtM_1_127 = 0; $('#txtM_1_127').val(''); IsPass = false; lstFail.push("16.2 Middle management"); }
            if ($txtF_1_41 < $txtF_1_127) { $txtF_1_127 = 0; $('#txtF_1_127').val(''); IsPass = false; lstFail.push("16.2 Middle management"); }
            var $txtM_1_128 = $txtM_1_41 > 0 ? CalNumber(($txtM_1_127 / $txtM_1_41) * 100) : 0;
            var $txtF_1_128 = $txtF_1_41 > 0 ? CalNumber(($txtF_1_127 / $txtF_1_41) * 100) : 0;
            $('#txtM_1_128').val($txtM_1_128.toFixed(2));
            $('#txtF_1_128').val($txtF_1_128.toFixed(2));

            var $txtM_2_127 = +GetValTextBox('txtM_2_127').replace(/\,/g, '');
            var $txtF_2_127 = +GetValTextBox('txtF_2_127').replace(/\,/g, '');
            if ($txtM_2_41 < $txtM_2_127) { $txtM_2_127 = 0; $('#txtM_2_127').val(''); IsPass = false; lstFail.push("16.2 Middle management"); }
            if ($txtF_2_41 < $txtF_2_127) { $txtF_2_127 = 0; $('#txtF_2_127').val(''); IsPass = false; lstFail.push("16.2 Middle management"); }
            var $txtM_2_128 = $txtM_2_41 > 0 ? CalNumber(($txtM_2_127 / $txtM_2_41) * 100) : 0;
            var $txtF_2_128 = $txtF_2_41 > 0 ? CalNumber(($txtF_2_127 / $txtF_2_41) * 100) : 0;
            $('#txtM_2_128').val($txtM_2_128.toFixed(2));
            $('#txtF_2_128').val($txtF_2_128.toFixed(2));

            var $txtM_3_127 = +GetValTextBox('txtM_3_127').replace(/\,/g, '');
            var $txtF_3_127 = +GetValTextBox('txtF_3_127').replace(/\,/g, '');
            if ($txtM_3_41 < $txtM_3_127) { $txtM_3_127 = 0; $('#txtM_3_127').val(''); IsPass = false; lstFail.push("16.2 Middle management"); }
            if ($txtF_3_41 < $txtF_3_127) { $txtF_3_127 = 0; $('#txtF_3_127').val(''); IsPass = false; lstFail.push("16.2 Middle management"); }
            var $txtM_3_128 = $txtM_3_41 > 0 ? CalNumber(($txtM_3_127 / $txtM_3_41) * 100) : 0;
            var $txtF_3_128 = $txtF_3_41 > 0 ? CalNumber(($txtF_3_127 / $txtF_3_41) * 100) : 0;
            $('#txtM_3_128').val($txtM_3_128.toFixed(2));
            $('#txtF_3_128').val($txtF_3_128.toFixed(2));
            // #endregion

            // #region 16.3 Senior
            var $txtM_1_129 = +GetValTextBox('txtM_1_129').replace(/\,/g, '');
            var $txtF_1_129 = +GetValTextBox('txtF_1_129').replace(/\,/g, '');
            if ($txtM_1_43 < $txtM_1_129) { $txtM_1_129 = 0; $('#txtM_1_129').val(''); IsPass = false; lstFail.push("16.3 Senior"); }
            if ($txtF_1_43 < $txtF_1_129) { $txtF_1_129 = 0; $('#txtF_1_129').val(''); IsPass = false; lstFail.push("16.3 Senior"); }
            var $txtM_1_130 = $txtM_1_43 > 0 ? CalNumber(($txtM_1_129 / $txtM_1_43) * 100) : 0;
            var $txtF_1_130 = $txtF_1_43 > 0 ? CalNumber(($txtF_1_129 / $txtF_1_43) * 100) : 0;
            $('#txtM_1_130').val($txtM_1_130.toFixed(2));
            $('#txtF_1_130').val($txtF_1_130.toFixed(2));

            var $txtM_2_129 = +GetValTextBox('txtM_2_129').replace(/\,/g, '');
            var $txtF_2_129 = +GetValTextBox('txtF_2_129').replace(/\,/g, '');
            if ($txtM_2_43 < $txtM_2_129) { $txtM_2_129 = 0; $('#txtM_2_129').val(''); IsPass = false; lstFail.push("16.3 Senior"); }
            if ($txtF_2_43 < $txtF_2_129) { $txtF_2_129 = 0; $('#txtF_2_129').val(''); IsPass = false; lstFail.push("16.3 Senior"); }
            var $txtM_2_130 = $txtM_2_43 > 0 ? CalNumber(($txtM_2_129 / $txtM_2_43) * 100) : 0;
            var $txtF_2_130 = $txtF_2_43 > 0 ? CalNumber(($txtF_2_129 / $txtF_2_43) * 100) : 0;
            $('#txtM_2_130').val($txtM_2_130.toFixed(2));
            $('#txtF_2_130').val($txtF_2_130.toFixed(2));

            var $txtM_3_129 = +GetValTextBox('txtM_3_129').replace(/\,/g, '');
            var $txtF_3_129 = +GetValTextBox('txtF_3_129').replace(/\,/g, '');
            if ($txtM_3_43 < $txtM_3_129) { $txtM_3_129 = 0; $('#txtM_3_129').val(''); IsPass = false; lstFail.push("16.3 Senior"); }
            if ($txtF_3_43 < $txtF_3_129) { $txtF_3_129 = 0; $('#txtF_3_129').val(''); IsPass = false; lstFail.push("16.3 Senior"); }
            var $txtM_3_130 = $txtM_3_43 > 0 ? CalNumber(($txtM_3_129 / $txtM_3_43) * 100) : 0;
            var $txtF_3_130 = $txtF_3_43 > 0 ? CalNumber(($txtF_3_129 / $txtF_3_43) * 100) : 0;
            $('#txtM_3_130').val($txtM_3_130.toFixed(2));
            $('#txtF_3_130').val($txtF_3_130.toFixed(2));
            // #endregion

            // #region 16.4 Employee
            var $txtM_1_131 = +GetValTextBox('txtM_1_131').replace(/\,/g, '');
            var $txtF_1_131 = +GetValTextBox('txtF_1_131').replace(/\,/g, '');
            if ($txtM_1_45 < $txtM_1_131) { $txtM_1_131 = 0; $('#txtM_1_131').val(''); IsPass = false; lstFail.push("16.4 Employee"); }
            if ($txtF_1_45 < $txtF_1_131) { $txtF_1_131 = 0; $('#txtF_1_131').val(''); IsPass = false; lstFail.push("16.4 Employee"); }
            var $txtM_1_132 = $txtM_1_45 > 0 ? CalNumber(($txtM_1_131 / $txtM_1_45) * 100) : 0;
            var $txtF_1_132 = $txtF_1_45 > 0 ? CalNumber(($txtF_1_131 / $txtF_1_45) * 100) : 0;
            $('#txtM_1_132').val($txtM_1_132.toFixed(2));
            $('#txtF_1_132').val($txtF_1_132.toFixed(2));

            var $txtM_2_131 = +GetValTextBox('txtM_2_131').replace(/\,/g, '');
            var $txtF_2_131 = +GetValTextBox('txtF_2_131').replace(/\,/g, '');
            if ($txtM_2_45 < $txtM_2_131) { $txtM_2_131 = 0; $('#txtM_2_131').val(''); IsPass = false; lstFail.push("16.4 Employee"); }
            if ($txtF_2_45 < $txtF_2_131) { $txtF_2_131 = 0; $('#txtF_2_131').val(''); IsPass = false; lstFail.push("16.4 Employee"); }
            var $txtM_2_132 = $txtM_2_45 > 0 ? CalNumber(($txtM_2_131 / $txtM_2_45) * 100) : 0;
            var $txtF_2_132 = $txtF_2_45 > 0 ? CalNumber(($txtF_2_131 / $txtF_2_45) * 100) : 0;
            $('#txtM_2_132').val($txtM_2_132.toFixed(2));
            $('#txtF_2_132').val($txtF_2_132.toFixed(2));

            var $txtM_3_131 = +GetValTextBox('txtM_3_131').replace(/\,/g, '');
            var $txtF_3_131 = +GetValTextBox('txtF_3_131').replace(/\,/g, '');
            if ($txtM_3_45 < $txtM_3_131) { $txtM_3_131 = 0; $('#txtM_3_131').val(''); IsPass = false; lstFail.push("16.4 Employee"); }
            if ($txtF_3_45 < $txtF_3_131) { $txtF_3_131 = 0; $('#txtF_3_131').val(''); IsPass = false; lstFail.push("16.4 Employee"); }
            var $txtM_3_132 = $txtM_3_45 > 0 ? CalNumber(($txtM_3_131 / $txtM_3_45) * 100) : 0;
            var $txtF_3_132 = $txtF_3_45 > 0 ? CalNumber(($txtF_3_131 / $txtF_3_45) * 100) : 0;
            $('#txtM_3_132').val($txtM_3_132.toFixed(2));
            $('#txtF_3_132').val($txtF_3_132.toFixed(2));
            // #endregion

            // #endregion

            // #region 18. Board structure           

            // #region 18.2 Number of executive director
            var $txtM_1_149 = +GetValTextBox('txtM_1_149').replace(/\,/g, '');
            var $txtM_2_149 = +GetValTextBox('txtM_2_149').replace(/\,/g, '');
            var $txtM_3_149 = +GetValTextBox('txtM_3_149').replace(/\,/g, '');

            var $txtF_1_149 = +GetValTextBox('txtF_1_149').replace(/\,/g, '');
            var $txtF_2_149 = +GetValTextBox('txtF_2_149').replace(/\,/g, '');
            var $txtF_3_149 = +GetValTextBox('txtF_3_149').replace(/\,/g, '');
            // #endregion

            // #region 18.3 Number of non-executive directors (excl. independent directors)
            var $txtM_1_150 = +GetValTextBox('txtM_1_150').replace(/\,/g, '');
            var $txtM_2_150 = +GetValTextBox('txtM_2_150').replace(/\,/g, '');
            var $txtM_3_150 = +GetValTextBox('txtM_3_150').replace(/\,/g, '');

            var $txtF_1_150 = +GetValTextBox('txtF_1_150').replace(/\,/g, '');
            var $txtF_2_150 = +GetValTextBox('txtF_2_150').replace(/\,/g, '');
            var $txtF_3_150 = +GetValTextBox('txtF_3_150').replace(/\,/g, '');
            // #endregion

            // #region 18.4 Number of independent directors
            var $txtM_1_151 = +GetValTextBox('txtM_1_151').replace(/\,/g, '');
            var $txtM_2_151 = +GetValTextBox('txtM_2_151').replace(/\,/g, '');
            var $txtM_3_151 = +GetValTextBox('txtM_3_151').replace(/\,/g, '');

            var $txtF_1_151 = +GetValTextBox('txtF_1_151').replace(/\,/g, '');
            var $txtF_2_151 = +GetValTextBox('txtF_2_151').replace(/\,/g, '');
            var $txtF_3_151 = +GetValTextBox('txtF_3_151').replace(/\,/g, '');
            // #endregion

            // #region 18.1 Total number of board members
            var $txtM_1_148 = CalNumber($txtM_1_149 + $txtM_1_150 + $txtM_1_151);
            var $txtM_2_148 = CalNumber($txtM_2_149 + $txtM_2_150 + $txtM_2_151);
            var $txtM_3_148 = CalNumber($txtM_3_149 + $txtM_3_150 + $txtM_3_151);
            var $txtF_1_148 = CalNumber($txtF_1_149 + $txtF_1_150 + $txtF_1_151);
            var $txtF_2_148 = CalNumber($txtF_2_149 + $txtF_2_150 + $txtF_2_151);
            var $txtF_3_148 = CalNumber($txtF_3_149 + $txtF_3_150 + $txtF_3_151);

            $('#txtM_1_148').val($txtM_1_148);
            $('#txtM_2_148').val($txtM_2_148);
            $('#txtM_3_148').val($txtM_3_148);
            $('#txtF_1_148').val($txtF_1_148);
            $('#txtF_2_148').val($txtF_2_148);
            $('#txtF_3_148').val($txtF_3_148);
            // #endregion

            // #endregion

            // #region 17. Gender Diversity and Equal Remuneration

            // #region 17.1 Women in workforce
            var $txtM_1_134 = $txtF_1_5;
            var $txtM_1_135 = $txtM_1_4 > 0 ? CalNumber(($txtM_1_134 / $txtM_1_4) * 100) : 0;
            $('#txtM_1_134').val($txtM_1_134);
            $('#txtM_1_135').val($txtM_1_135.toFixed(2));

            var $txtM_2_134 = $txtF_2_5;
            var $txtM_2_135 = $txtM_2_4 > 0 ? CalNumber(($txtM_2_134 / $txtM_2_4) * 100) : 0;
            $('#txtM_2_134').val($txtM_2_134);
            $('#txtM_2_135').val($txtM_2_135.toFixed(2));

            var $txtM_3_134 = $txtF_3_5;
            var $txtM_3_135 = $txtM_3_4 > 0 ? CalNumber(($txtM_3_134 / $txtM_3_4) * 100) : 0;
            $('#txtM_3_134').val($txtM_3_134);
            $('#txtM_3_135').val($txtM_3_135.toFixed(2));
            // #endregion

            // #region 17.2 Women in management positions
            var $txtM_1_136 = $txtF_1_39 + $txtF_1_41;
            var $txt_1_3941 = ($txtM_1_39 + $txtF_1_39 + $txtM_1_41 + $txtF_1_41);
            var $txtM_1_137 = $txt_1_3941 > 0 ? CalNumber(($txtM_1_136 / $txt_1_3941) * 100) : 0;
            $('#txtM_1_136').val($txtM_1_136);
            $('#txtM_1_137').val($txtM_1_137.toFixed(2));

            var $txtM_2_136 = $txtF_2_39 + $txtF_2_41;
            var $txt_2_3941 = ($txtM_2_39 + $txtF_2_39 + $txtM_2_41 + $txtF_2_41);
            var $txtM_2_137 = $txt_2_3941 > 0 ? CalNumber(($txtM_2_136 / $txt_2_3941) * 100) : 0;
            $('#txtM_2_136').val($txtM_2_136);
            $('#txtM_2_137').val($txtM_2_137.toFixed(2));

            var $txtM_3_136 = $txtF_3_39 + $txtF_3_41;
            var $txt_3_3941 = ($txtM_3_39 + $txtF_3_39 + $txtM_3_41 + $txtF_3_41);
            var $txtM_3_137 = $txt_3_3941 > 0 ? CalNumber(($txtM_3_136 / $txt_3_3941) * 100) : 0;
            $('#txtM_3_136').val($txtM_3_136);
            $('#txtM_3_137').val($txtM_3_137.toFixed(2));
            // #endregion

            // #region 17.3 Women in top management positions
            var $txtM_1_138 = $txtF_1_39;
            var $txt_1_39 = ($txtM_1_39 + $txtF_1_39);
            var $txtM_1_139 = $txt_1_39 > 0 ? CalNumber(($txtM_1_138 / $txt_1_39) * 100) : 0;
            $('#txtM_1_138').val($txtM_1_138);
            $('#txtM_1_139').val($txtM_1_139.toFixed(2));

            var $txtM_2_138 = $txtF_2_39;
            var $txt_2_39 = ($txtM_2_39 + $txtF_2_39);
            var $txtM_2_139 = $txt_2_39 > 0 ? CalNumber(($txtM_2_138 / $txt_2_39) * 100) : 0;
            $('#txtM_2_138').val($txtM_2_138);
            $('#txtM_2_139').val($txtM_2_139.toFixed(2));

            var $txtM_3_138 = $txtF_3_39;
            var $txt_3_39 = ($txtM_3_39 + $txtF_3_39);
            var $txtM_3_139 = $txt_3_39 > 0 ? CalNumber(($txtM_3_138 / $txt_3_39) * 100) : 0;
            $('#txtM_3_138').val($txtM_3_138);
            $('#txtM_3_139').val($txtM_3_139.toFixed(2));
            // #endregion

            // #region 17.4 Women in junior management positions
            var $txtM_1_140 = $txtF_1_41;
            var $txt_1_41 = ($txtM_1_41 + $txtF_1_41);
            var $txtM_1_141 = $txt_1_41 > 0 ? CalNumber(($txtM_1_140 / $txt_1_41) * 100) : 0;
            $('#txtM_1_140').val($txtM_1_140);
            $('#txtM_1_141').val($txtM_1_141.toFixed(2));

            var $txtM_2_140 = $txtF_2_41;
            var $txt_2_41 = ($txtM_2_41 + $txtF_2_41);
            var $txtM_2_141 = $txt_2_41 > 0 ? CalNumber(($txtM_2_140 / ($txtM_2_41 + $txtF_2_41)) * 100) : 0;
            $('#txtM_2_140').val($txtM_2_140);
            $('#txtM_2_141').val($txtM_2_141.toFixed(2));

            var $txtM_3_140 = $txtF_3_41;
            var $txt_3_41 = ($txtM_3_41 + $txtF_3_41);
            var $txtM_3_141 = $txt_3_41 > 0 ? CalNumber(($txtM_3_140 / ($txtM_3_41 + $txtF_3_41)) * 100) : 0;
            $('#txtM_3_140').val($txtM_3_140);
            $('#txtM_3_141').val($txtM_3_141.toFixed(2));
            // #endregion

            // #region 17.5 Women in management positions in revenue generating functions e.g. Sales, Marketing, Operation and BD that under BU
            var $txtM_1_142 = +GetValTextBox('txtM_1_142').replace(/\,/g, '');
            var $txtM_1_143 = CalNumber(($txtM_1_142 / 18) * 100);
            $('#txtM_1_143').val($txtM_1_143.toFixed(2));

            var $txtM_2_142 = +GetValTextBox('txtM_2_142').replace(/\,/g, '');
            var $txtM_2_143 = CalNumber(($txtM_2_142 / 18) * 100);
            $('#txtM_2_143').val($txtM_2_143.toFixed(2));

            var $txtM_3_142 = +GetValTextBox('txtM_3_142').replace(/\,/g, '');
            var $txtM_3_143 = CalNumber(($txtM_3_142 / 18) * 100);
            $('#txtM_3_143').val($txtM_3_143.toFixed(2));
            // #endregion

            // #region 17.6 Number of women on board of directors/supervisory board
            var $txtM_1_144 = $txtF_1_148;
            var $txtM_1_145 = $txtM_1_148 + $txtF_1_148 > 0 ? CalNumber(($txtM_1_144 / ($txtM_1_148 + $txtF_1_148)) * 100) : 0;
            $('#txtM_1_144').val($txtM_1_144);
            $('#txtM_1_145').val($txtM_1_145.toFixed(2));

            var $txtM_2_144 = $txtF_2_148;
            var $txtM_2_145 = $txtM_2_148 + $txtF_2_148 > 0 ? CalNumber(($txtM_2_144 / ($txtM_2_148 + $txtF_2_148)) * 100) : 0;
            $('#txtM_2_144').val($txtM_2_144);
            $('#txtM_2_145').val($txtM_2_145.toFixed(2));

            var $txtM_3_144 = $txtF_3_148;
            var $txtM_3_145 = $txtM_3_148 + $txtF_3_148 > 0 ? CalNumber(($txtM_3_144 / ($txtM_3_148 + $txtF_3_148)) * 100) : 0;
            $('#txtM_3_144').val($txtM_3_144);
            $('#txtM_3_145').val($txtM_3_145.toFixed(2));
            // #endregion

            // #region 17.7 Ratio basic salary women/men
            var $txtM_1_146 = +GetValTextBox('txtM_1_146').replace(/\,/g, '');
            var $txtF_1_146 = +GetValTextBox('txtF_1_146').replace(/\,/g, '');

            var $txtM_2_146 = +GetValTextBox('txtM_2_146').replace(/\,/g, '');
            var $txtF_2_146 = +GetValTextBox('txtF_2_146').replace(/\,/g, '');

            var $txtM_3_146 = +GetValTextBox('txtM_3_146').replace(/\,/g, '');
            var $txtF_3_146 = +GetValTextBox('txtF_3_146').replace(/\,/g, '');
            // #endregion

            // #endregion            

            // #region 19. Labor Practice Indicators

            // #region 19.1 Employees represented by an independent trade union
            var $txtM_1_153 = +GetValTextBox('txtM_1_153').replace(/\,/g, '');
            var $txtF_1_153 = +GetValTextBox('txtF_1_153').replace(/\,/g, '');

            var $txtM_2_153 = +GetValTextBox('txtM_2_153').replace(/\,/g, '');
            var $txtF_2_153 = +GetValTextBox('txtF_2_153').replace(/\,/g, '');

            var $txtM_3_153 = +GetValTextBox('txtM_3_153').replace(/\,/g, '');
            var $txtF_3_153 = +GetValTextBox('txtF_3_153').replace(/\,/g, '');
            // #endregion

            // #endregion

            UnblockUI();

            if (!IsPass && IsChange) {
                lstFail = Enumerable.From(lstFail).Distinct().ToArray();
                DialogWarning("Please check data in section " + lstFail.join(', '));
            }
        }

        function CalNumber(nVal) {
            return +(Math.round(nVal * 10000000000) / 10000000000).toFixed(2);
        }

        function SaveData(sTypeApprove) {
            var sMg = "";
            var IsPassComment = true;
            var lstApprove = [1, 2];
            
            switch (+sTypeApprove) {
                case 1:
                    if (lstApprove.indexOf(+$hddStatusID.val()) > -1) {
                        if (lstApprove.indexOf(+$hddStatusID.val()) == 0) { sMg = AlertMsg.ConfirmApprove; }
                        else { sMg = AlertMsg.ConfirmAcknowledge; }
                        IsPassComment = CheckValidate('divComment');
                    } else {
                        sMg = AlertMsg.ConfirmSubmit;
                    }
                    break;
                case 2: sMg = AlertMsg.ConfirmRecall; IsPassComment = CheckValidate('divComment'); break;
                case 3: sMg = AlertMsg.ConfirmRequestEdit; IsPassComment = CheckValidate('divComment'); break;
                case 4: sMg = AlertMsg.ConfirmReject; IsPassComment = CheckValidate('divComment'); break;
                default: sMg = AlertMsg.ConfirmSaveDraft; break;
            }
            var IsDraft = IsNullOrEmpty(sTypeApprove);
            var IsHasFile = !IsNullOrEmpty(obj_File.sPath);
            var IsCompanyInternal = Boolean(+$hddIsCompanyInternal.val());
            var IsLoadData = Boolean(+$hddIsLoadData.val());
            var IspassFile = (!IsCompanyInternal ? IsHasFile == true : IsCompanyInternal);

            var lstToSave = GetTxtDJSI();
            var lstAutoCalTotal = Enumerable.From(lstToSave).Where('$.IsAutoCal == false && $.IsTotal == true && ($.nMale_1== null || $.nMale_2 == null || $.nMale_3 == null)').ToArray();
            var lstAutoCal = Enumerable.From(lstToSave).Where('$.IsAutoCal == false && $.IsTotal == false && ($.nMale_1== null || $.nMale_2 == null || $.nMale_3 == null || $.nFemale_1== null || $.nFemale_2 == null || $.nFemale_3 == null)').ToArray();
            var IsPassFill = IsDraft == true ? true : (Boolean(lstAutoCalTotal.length == 0) && Boolean(lstAutoCal.length == 0));
            var sMgError_ = "";

            if (!IsPassFill) {
                var lstHead = Enumerable.From(lstDJSI).Where('$.IsHead == true').ToArray();
                var lstAll = lstAutoCal.concat(lstAutoCalTotal);
                lstAll = Enumerable.From(lstAll).OrderBy('$.nItemHead').Select(function (s) { return s.nItemHead }).Distinct().ToArray();
                $.each(lstAll, function (i, el) {
                    var q = Enumerable.From(lstDJSI).Where('$.nItem ==' + el).FirstOrDefault();
                    var Panel = lstHead.indexOf(Enumerable.From(lstHead).FirstOrDefault(null, '$.nItem ==' + el)) + 1;
                    if (q != null) sMgError_ += "<li>" + Panel + '.' + q.sName + ".</li>";
                });
            }

            if (CheckValidate('divForm') && IsPassComment) {
                if (IsPassFill) {
                    if (IspassFile) {
                        if (IsLoadData) {
                            BBox.Confirm(AlertTitle.Confirm, sMg, function () {
                                BBox.ButtonEnabled(false);
                                BlockUI();

                                var obj = {
                                    'nReportID': +GetValTextBox('hddReportID'),
                                    'nCompanyID': +GetValDropdown('ddlCompany'),
                                    'nYear': +GetValDropdown('ddlYear'),
                                    'nQuarter': +GetValDropdown('ddlQuarter'),
                                    'nStatusID': 0,
                                    'lstDJSI': GetTxtDJSI(),
                                    'objFile': obj_File,
                                    /* Check Submit Again*/
                                    'IsL0': Boolean(+$L0.val()),
                                    'IsL1': Boolean(+$L1.val()),
                                    'IsL2': Boolean(+$L2.val()),
                                    'nType': +sTypeApprove,
                                    'nCurrentStatusID': +$hddStatusID.val(),
                                    'sComment': GetValTextArea('txtComment'),
                                    'lstFile': arrData_FileDocument,
                                }

                                AjaxWebMethod("SaveData", { 'item': obj }, function (response) {
                                    if (response.d.Status == SysProcess.SessionExpired) {
                                        PopupSessionTimeOut();
                                    } else if (response.d.Status == SysProcess.Duplicate) {
                                        UnblockUI();
                                        DialogDuplicate();
                                    } else if (response.d.Status == SysProcess.SaveFail) {
                                        UnblockUI();
                                        DialogSaveFail(response.d.Msg);
                                    } else if (response.d.Status == SysProcess.Failed) {
                                        UnblockUI();
                                        DialogWarning(response.d.Msg);
                                    } else {
                                        UnblockUI();
                                        DialogSucessRedirect('report_djsi.aspx');
                                    }
                                });
                            });
                        } else {
                            DialogWarning("Please Load Data!");
                        }
                    } else {
                        DialogWarning("Please Upload File!");
                    }
                } else {
                    var sMg = ' <ul class="list-unstyled">'
                        + '<li><u><strong>Please fill out all information!</strong></u>'
                        + '<ul>'
                            + sMgError_
                        + '</ul>'
                        + '</li>'
                        + '</ul>';
                    DialogWarning(sMg);
                }
            }
        }

        function GetTxtDJSI() {
            var $lstTosave = [];

            var lstDJSISub = Enumerable.From(lstDJSI).Where('$.IsHead == false').ToArray();
            $.each(lstDJSISub, function (i, el) {
                var nItem = el.nItem;
                var IsTotal = el.IsTotal;

                var $M1 = "", $M2 = "", $M3 = "", $F1 = "", $F2 = "", $F3 = "";

                $M1 = GetValTextBox('txtM_1_' + nItem);
                $M2 = GetValTextBox('txtM_2_' + nItem);
                $M3 = GetValTextBox('txtM_3_' + nItem);

                if (!IsTotal) {
                    $F1 = GetValTextBox('txtF_1_' + nItem);
                    $F2 = GetValTextBox('txtF_2_' + nItem);
                    $F3 = GetValTextBox('txtF_3_' + nItem);
                }

                var obj = {
                    nItem: nItem,
                    nMale_1: ($M1 != "" ? +($M1.replace(/\,/g, '')) : null),
                    nMale_2: ($M2 != "" ? +($M2.replace(/\,/g, '')) : null),
                    nMale_3: ($M3 != "" ? +($M3.replace(/\,/g, '')) : null),
                    nFemale_1: ($F1 != "" ? +($F1.replace(/\,/g, '')) : null),
                    nFemale_2: ($F2 != "" ? +($F2.replace(/\,/g, '')) : null),
                    nFemale_3: ($F3 != "" ? +($F3.replace(/\,/g, '')) : null),
                    IsAutoCal: el.IsAutoCal,
                    IsTotal: el.IsTotal,
                    sName: el.sName,
                    nItemHead: el.nItemHead,
                }
                $lstTosave.push(obj);
            });

            return $lstTosave;
        }

        //#region File    
        var sUserID = GetValTextBox('hddUserID');
        var urlAshx = "Ashx/Fileuploader.ashx";

        var tbData_File = $("table[id$=tbData_File] tbody");
        var arrData = [];
        var arrData_Error = [];
        var obj_File = {};
        var arrTypeFile = ['xls', 'xlsx'];
        var IsPassFile = true;

        $(function () {
            $('#btnDelFile').click(function () {
                DialogConfirmDel(function () {
                    arrData = [];
                    arrData_Error = [];
                    obj_File = {};
                    $("input[id$=txtFile]").parent().removeClass('fileuploader-disabled');
                    $("div[id$=divFile]").removeClass('fileuploader-disabled');
                    $('#divFileBtn').hide();
                    $('#divError').hide();
                    $('#btnLoadData').hide();
                    $('#divData :input').val('');
                    $('#divData').hide();
                    $('#divSwitch').hide();
                    $hddIsLoadData.val(0);
                    //return false;
                });
            });

            var filupload1 = $('input[id$="txtFile"]').fileuploader({
                limit: 1,
                fileMaxSize: 10,
                enableApi: true,
                thumbnails: false,
                extensions: arrTypeFile,
                upload: {
                    // upload URL {String}
                    url: urlAshx,

                    // upload data {null, Object}
                    // you can also change this Object in beforeSend callback
                    // example: {case savetoname = "" then generate format filename_DateTime("ddMMyyyyHHmmssff") }
                    data: { funcname: "UPLOAD", savetopath: '../UploadFiles/' + sUserID + '/Temp/', savetoname: '' },

                    // upload type {String}
                    type: 'POST',

                    // upload enctype {String}
                    enctype: 'multipart/form-data',

                    // auto-start file uploading {Boolean}
                    // if it will be false, you can use the API methods to start it (check options example)
                    start: true,

                    // upload the files synchron(อัพโหลดให้แล้วเสร็จทีละไฟล์) {Boolean}
                    synchron: true,

                    onProgress: function (data, item) {
                        apiFile1.disable($('input[id$="txtFile"]'))
                    },

                    // Callback fired if the uploading succeeds
                    // by default we will add a success icon and fadeOut the progressbar
                    // Remember that if you want so show the PHP errors, you will need to process them also here. To prevent it you will need to respond on the upload url with error code in header.
                    onSuccess: function (data, item, listEl, parentEl, newInputEl, inputEl, textStatus, jqXHR) {
                        obj_File = {
                            sPath: data.SaveToPath,
                            sSysFileName: data.SaveToFileName,
                            sFileName: data.FileName
                        };
                        CheckFile();
                        RemoveFile(item);
                    },

                    // Callback fired after all files were uploaded
                    onComplete: function (listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus) {
                        apiFile1.enable();
                    }
                }
            });

            var apiFile1 = $.fileuploader.getInstance(filupload1);

            function RemoveFile(item) {
                apiFile1.remove(item);
            }
        });

        function BindFile(IsError) {
            $('#divFileBtn').show();
            var sFileURL = "";
            var IsShowFile = false;
            if (IsError) {
                sFileURL = obj_File.sPath.replace("../", "") + 'Error_' + obj_File.sSysFileName;
                IsShowFile = true;
            }
            else {
                if (!Boolean(+GetValTextBox('hddIsCompanyInternal'))) {
                    sFileURL = obj_File.sPath.replace("../", "") + obj_File.sSysFileName;
                    IsShowFile = true;
                }
            }
            if (IsShowFile) {
                var onclick = "FancyBox_ViewFile('" + sFileURL + "')";
                $('#btnViewFile').attr('onclick', onclick);
                $('#btnViewFile').text('').append('<span class="glyphicon glyphicon-zoom-in"></span> ' + Sub_string(obj_File.sFileName, 20));
                $("input[id$=txtFile]").parent().addClass('fileuploader-disabled');
            }
        }

        function CheckFile() {
            BlockUI();

            AjaxWebMethod("CheckFile", { 'sPath': obj_File.sPath, 'sSysFileName': obj_File.sSysFileName, 'sFileName': obj_File.sFileName }, function (data) {
                if (data.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else if (data.d.Status == SysProcess.Failed) {
                    UnblockUI();
                    arrData_Error = data.d.lstDataError;
                    $('#btnLoadData').hide();
                    $('#divDataError').show();
                    BindFile(true);
                    Bind_DataError();
                    DialogWarning(data.d.Msg);
                } else {
                    UnblockUI();
                    arrData = data.d.lstData;
                    $('#btnLoadData').show();
                    $('#divDataError').hide();
                    BindFile(false);
                    arrData_Error = [];
                    $("input[id$=txtFile]").parent().addClass('fileuploader-disabled');
                    //$("input[id$=txtFile]").parent().removeClass('fileuploader-disabled');
                }
            }, function () { UnblockUI(); });
        }

        //#endregion Tab File 

        //#region Tab Data Error   
        function Bind_DataError() {
            SortingClear($tbData);
            $objPag = $('ul#pagData').paging(arrData_Error.length, {
                format: '[< nnncnnn >]',
                onFormat: EasyPaging_OnFormat,
                perpage: $ddlPageSize.val(),
                onSelect: function (nPageNo) { //1,2,3,4,5,...
                    var nPageSize = $ddlPageSize.val();
                    ActiveDataBind_Rev(nPageSize, nPageNo, arrData_Error, $tbData, CreateDataRow, OnDataBound);
                    SetTooltip();
                },
            });
        }

        function OnDataBound() {
            CheckDataFound();
            SortingEvent();
        }

        function CheckDataFound() {
            var isFoundData = DataChecking(arrData_Error, $divNoData);
            if (isFoundData) $divPaging.show('fast');
            else $divPaging.hide('fast');
            return isFoundData;
        }

        function CreateDataRow(objData, nRowNo) {
            var sHTML = "";
            var sMsgError = "";
            $.each(objData.lstMgError, function (i, el) {
                sMsgError += el + "</br>";
            });
            sHTML += "<td class='text-center'>" + objData.nRowExcel + "</td>";
            sHTML += "<td class='text-left'>" + sMsgError + "</td>";

            var tr = "<tr>" + sHTML + "</tr>";
            return tr;
        }

        //#endregion Tab Data Error 

        //#region Tab Data LOG    
        function SetData() {
            BlockUI();
            var sProID = GetValTextBox('hddProjectID');
            AjaxWebMethod('GetData', { 'nReportID': +GetValTextBox('hddReportID') }, function (data) {
                if (data.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else {
                    lstLog = data.d.lstLog;
                    Bind_DataLOG();
                    arrData_FileDocument = data.d.lstFile;
                    BindFileDocument();

                }
            }, function () { UnblockUI(); CheckPRMS(); });
        }

        function Bind_DataLOG() {
            SortingClear($tbData_Log);
            $objPag = $('ul#pagData_log').paging(lstLog.length, {
                format: '[< nnncnnn >]',
                onFormat: EasyPaging_OnFormat,
                perpage: $ddlPageSize_log.val(),
                onSelect: function (nPageNo) { //1,2,3,4,5,...
                    var nPageSize = $ddlPageSize_log.val();
                    ActiveDataBind_Rev(nPageSize, nPageNo, lstLog, $tbData_Log, CreateDataRowLOG, OnDataBoundLOG);
                    SetTooltip();
                },
            });
        }
        function OnDataBoundLOG() {
            CheckDataFoundLOG();
            SortingEvent();
        }
        function CheckDataFoundLOG() {
            var isFoundData = DataChecking(lstLog, $divNoData_log);
            if (isFoundData) $divPaging_log.show('fast');
            else $divPaging_log.hide('fast');
            return isFoundData;
        }
        function CreateDataRowLOG(objData, nRowNo) {
            var sHTML = "";
            sHTML += '<td class="text-center">' + nRowNo + "." + '</td>';
            sHTML += '<td>' + objData.sAction + '</td>';
            sHTML += '<td>' + objData.sActionBy + '</td>';
            sHTML += '<td class="text-center">' + objData.sActionDate + '</td>';
            sHTML += '<td>' + (objData.sComment || '') + '</td>';

            var tr = "<tr>" + sHTML + "</tr>";
            return tr;
        }
        //#endregion Tab Data LOG 
    </script>

    <script type="text/javascript">
        //#region Tab Attach File

        var tbData_FileDocument = $("table[id$=tbData_FileDocument] tbody");
        var arrData_FileDocument = [];

        $(function () {

            $("#tbData_FileDocument").delegate('input[id^="cbRec_DM_"]:checkbox', 'change', function () {
                var $cbRec = $('input[id^="cbRec_DM_"]:checkbox');
                var $cbRec_DM_Checked = $cbRec.filter(':checked');
                var n_$cbRec = $cbRec.length;
                var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_DM_Checked.length : false;
                $("input[id$=cbHead_DM]").prop('checked', isCheckedAll);
            });

            $("input[id$=cbHead_DM]").change(function () {
                var isChecked = $(this).is(':checked');
                var $cbRec = $('input[id*=cbRec_DM_]:checkbox');
                $cbRec.prop('checked', isChecked);
            });

            $("button[id$=btnDel_DM]").click(function () {
                var dataHasUse = false;
                var $cbRec = $('input[id^="cbRec_DM_"]:checkbox');
                var $cbRec_DM_Checked = $cbRec.filter(':checked');
                if ($cbRec_DM_Checked.length > 0) {
                    DialogConfirmDel(function () {
                        var arrDel = $.map($cbRec_DM_Checked, function (cb) { return +$(cb).val(); });
                        $.each(arrDel, function (i, el) {
                            var $thisDel = Enumerable.From(arrData_FileDocument).Where(function (w) { return w.nID == el }).FirstOrDefault();
                            if ($thisDel !== undefined) {
                                if ($thisDel.isNew) {
                                    arrData_FileDocument = Enumerable.From(arrData_FileDocument).Where('$.nID != ' + el).ToArray();
                                } else {
                                    $thisDel.isDel = true;
                                }
                            }
                        });
                        BindFileDocument();
                    });
                }
                else DialogDeleteError();
            });

            var filupload1 = $('input[id$="txtFileDocument"]').fileuploader({
                fileMaxSize: 10,
                enableApi: true,
                thumbnails: false,
                extensions: ['xls', 'xlsx', 'pdf', 'doc', 'docx', 'ppt', 'pptx'],
                upload: {
                    // upload URL {String}
                    url: urlAshx,

                    // upload data {null, Object}
                    // you can also change this Object in beforeSend callback
                    // example: {case savetoname = "" then generate format filename_DateTime("ddMMyyyyHHmmssff") }
                    data: { funcname: "UPLOAD", savetopath: '../UploadFiles/' + sUserID + '/Temp/', savetoname: '' },

                    // upload type {String}
                    type: 'POST',

                    // upload enctype {String}
                    enctype: 'multipart/form-data',

                    // auto-start file uploading {Boolean}
                    // if it will be false, you can use the API methods to start it (check options example)
                    start: true,

                    // upload the files synchron(อัพโหลดให้แล้วเสร็จทีละไฟล์) {Boolean}
                    synchron: true,

                    onProgress: function () {
                        $("#divButton button").prop("disabled", true);
                    },

                    // Callback fired if the uploading succeeds
                    // by default we will add a success icon and fadeOut the progressbar
                    // Remember that if you want so show the PHP errors, you will need to process them also here. To prevent it you will need to respond on the upload url with error code in header.
                    onSuccess: function (data, item, listEl, parentEl, newInputEl, inputEl, textStatus, jqXHR) {
                        AddFileDocument(data);
                        RemoveFileDocument(item);
                    },

                    // Callback fired after all files were uploaded
                    onComplete: function (listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus) {
                        var arrData_FileDocument = apiFile1.getFiles();

                        //if edit mode => save file to database
                        if (GetValTextBox("hidID")) {
                            SaveDataFile();
                        } else {
                            BindFileDocument();
                        }

                        //$(".fileuploader-input-caption").html("<span>คลิกที่นี่เพื่อเลือกเอกสารแนบ<span>");
                        $("#divButton button").prop("disabled", false);
                    }
                }
            });
            $(".fileuploader-input-button").html("<span>Browse Files<span>");
            var apiFile1 = $.fileuploader.getInstance(filupload1);

            function RemoveFileDocument(item) {
                apiFile1.remove(item);
            }

            BindFileDocument();
        });

        function AddFileDocument(item) {
            var nID = arrData_FileDocument.length > 0 ? Enumerable.From(arrData_FileDocument).Max('$.nID') + 1 : 1;
            arrData_FileDocument.push({
                nID: nID,
                sPath: item.SaveToPath,
                sSysFileName: item.SaveToFileName,
                sFileName: item.FileName,
                sDescription: "",
                isDel: false,
                isNew: true
            });
        }

        function BindFileDocument() {
            tbData_FileDocument.html("");
            var arrDataAttachfileForblind = Enumerable.From(arrData_FileDocument).Where(function (w) { return w.isDel == false }).ToArray();

            if (arrDataAttachfileForblind.length > 0) {
                var isHideDel = false;//SetPMS_AttachFile_Control();
                if (isHideDel) { $('#tbData_FileDocument th:last').remove(); }

                $.each(arrDataAttachfileForblind, function (i, el) {
                    var sHTML = "";

                    sHTML += '<td class="text-center">' + ('<div class="checkbox padT0"><input type="checkbox" name="cbRec_DM_' + el.nID + '" id="cbRec_DM_' + el.nID + '" value="' + el.nID + '" />' +
                              '<label for="cbRec_DM_' + el.nID + '">' + (i + 1) + '.</label></div>') + '</tds>';

                    sHTML += "<td id='tdFileName_" + el.nID + "' class=\"text-left\" style=\"word-break: break-all;\">" + el.sFileName + "</td>";

                    sHTML += "<td class=\"text-left\"><textarea id=\"txtDescriptionInTable_" + el.nID + "\" class=\"form-control\" name=\"txtDescriptionInTable\" maxlength=\"250\" onchange=\"UpdateData_Description()\" row=\"4\" cols=\"50\">" + el.sDescription + "</textarea></td>";

                    var sFileURL = el.sPath.replace("../", "") + el.sSysFileName.toLowerCase();
                    var onclick = "FancyBox_ViewFile('" + sFileURL + "')"; //"DownloadFile('" + sFileURL + "','" + el.nID + "')";
                    var btn = "<button type=\"button\" class=\"btn btn-info btn-sm\" onclick=\"" + onclick + "\" title='View' data-toggle='tooltip'><i class=\"glyphicon glyphicon-zoom-in\"></i></button>";
                    sHTML += "<td class=\"text-center\">" + btn + "</td>";

                    tbData_FileDocument.append("<tr>" + sHTML + "</tr>");
                });
            } else {
                tbData_FileDocument.append("<tr id='trFileNoData'><td colspan='5' class='dataNotFound' style='padding-top: 30px;'>No Data</td></tr>");
            }

            $("#divPaging_DM").toggle(arrDataAttachfileForblind.length > 0); //have value is true=show
            $("input[id$=cbHead_DM]").prop("checked", false);

            // SetTooltip_Control($("table#tbData_FileDocument > tbody > tr > td > button:not(.dropdown-toggle)"));
        }

        function DelFileDocument() {
            DialogConfirmDel(function () {
                var $thisDel = Enumerable.From(arrData_FileDocument).Where('$.nID == ' + nID).FirstOrDefault();
                if ($thisDel !== undefined) {

                    //if edit mode => save file to database
                    if (GetValTextBox("hidID")) {
                        BlockUI();
                        AjaxWebMethod("DeleteDataFile", {
                            nID: nID,
                            nIntiativeID: GetValTextBox("hidID")
                        }, function (data) {
                            if (data.d.Status == SysProcess.SessionExpired) {
                                PopupSessionTimeOut();
                            } else if (data.d.Status == SysProcess.Duplicate) {
                                DialogWarning("Data is duplicated.");
                            } else if (data.d.Status == SysProcess.Failed) {

                            } else {

                            }
                        }, UnblockUI);
                    }

                    if ($thisDel.isNew) {
                        arrData_FileDocument = Enumerable.From(arrData_FileDocument).Where('$.nID != ' + nID).ToArray();
                    } else {
                        $thisDel.isDel = true;
                    }
                }

                BindFileDocument();
            });
        }

        function UpdateData_Description() {
            $.each(arrData_FileDocument, function (i, el) {
                el.sDescription = $("textarea[id$=txtDescriptionInTable_" + el.nID + "]").val();
            });
        }

        function DownloadFile(sFileURL, nID) {
            var sFileExt = sFileURL.split('.').pop().toLowerCase(); //File Extension

            var filename = $("input[id$=txtFileNameInTable_" + nID + "]").val();
            if (filename == undefined) {
                filename = $("#tdFileName_" + nID).text();
            }

            if (!(filename)) {
                var arr = sFileURL.split("/");
                filename = arr[arr.length - 1];
            }

            filename = filename.replace("." + sFileExt, "");
            location.href = "Ashx/DownloadFile.ashx?filename=" + (filename + "." + sFileExt) + "&fullfile=" + sFileURL;
        }

        //#endregion Tab Attach File
    </script>
</asp:Content>

