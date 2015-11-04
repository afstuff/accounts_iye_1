<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PRG_FIN_RECPT_ISSUE.aspx.vb"
    Inherits="ABS_LIFE.PRG_FIN_RECPT_ISSUE" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script language="JavaScript" src="calendar_eu.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="Script/ScriptJS.js"></script>

    <link rel="Stylesheet" href="SS_ILIFE.css" type="text/css" />
    <link rel="stylesheet" href="calendar.css" />
    <link href="css/general.css" rel="stylesheet" type="text/css" />
    <link href="css/grid.css" rel="stylesheet" type="text/css" />
    <link href="css/rounded.css" rel="stylesheet" type="text/css" />

    <script src="jquery-1.11.0.js" type="text/javascript"></script>

    <script src="jquery.simplemodal.js" type="text/javascript"></script>

    <title></title>

    <script type="text/javascript">

        //warn before browser is closed or location is changed
        window.onbeforeunload = confirmExit;
        function confirmExit() {
            window.event.returnValue = 'If you navigate away from this page any unsaved changes will be lost!';
        }

        function cancelEvent(event) {
            window[event] = function() { null }

        }


        // calling jquery functions once document is ready
        $(document).ready(function() {

            var resultValueDR;
            var resultValueCR;
            var resultValue;
            var resultDesc;

            //Refresh screen with post back values
            function checkMode() {
                switch ($('#cmbMode').val()) {
                    case "C":
                        $('#ChequeRow').hide();
                        break;
                    default:
                        $('#ChequeRow').show();
                }

            }
            function GetReceiptType() {

                $("#cmbReceiptType").val($("#txtReceiptCode").val());
            }

            function CheckReceiptType() {
                $('#txtReceiptCode').val($('#cmbReceiptType').val());
                switch ($('#cmbReceiptType').val()) {
                    case "D":
                        $('#lblRefNo').text('Proposal No');

                        break;
                    case "P":
                        $('#lblRefNo').text('Policy No');
                        break;
                    case "X":
                    case "L":
                    case "V":
                    case "R":
                    case "I":
                    case "T":
                    case "S":
                    case "G":
                    case "O":
                        $('#lblMainAcctCR').show();
                        $('#txtMainAcctCredit').show();
                        $('#txtMainAcctCreditDesc').show();
                        $('#MainAccountCreditSearch').show();
                        $('#lblSubAcctCR').show();
                        $('#txtSubAcctCredit').show();
                        $('#txtSubAcctCreditDesc').show();
                        break;
                    default:
                        $('#lblMainAcctCR').hide();
                        $('#txtMainAcctCredit').hide();
                        $('#MainAcctCreditAdd').hide();
                        $('#txtMainAcctCreditDesc').hide();
                        $('#MainAccountCreditSearch').hide();
                        $('#lblSubAcctCR').hide();
                        $('#txtSubAcctCredit').hide();
                        $('#txtSubAcctCreditDesc').hide();
                        $('#SubAccountCreditSearch').hide();
                        $('#SubAcctCreditAdd').hide();
                        $('#lblRefNo').text('Ref No');
                        break;

                }

            }

            checkMode();
            CheckReceiptType();

            $('#txtReceiptAmtLC').on('focusout', function(e) {
                e.preventDefault();
                //                var currency = $('#txtReceiptAmtLC').val();
                //                var number = Number(currency.replace(/[^0-9\.]+/g, ""));
                //                $("#txtReceiptAmtLC").val(number);
                $('#txtReceiptAmtFC').attr('value', $('#txtReceiptAmtLC').val());
            });

            $('#cmbMode').on('focusout', function(e) {
                e.preventDefault();
                checkMode();

                //return false;
            });


            $('#cmbReceiptType, #cmbCommissions').on('focusout', function(e) {
                e.preventDefault();
                CheckReceiptType();
            });

            //FORMAT EFFECTIVE DATE AUTOMATICALLY. Make the slashes jump into place

            $("#txtEffectiveDate").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtEffectiveDate").val() != "") {
                    var effDate = $("#txtEffectiveDate").val();
                    effDateLen = effDate.length
                    if (effDateLen == 8 && $.isNumeric(effDate)) {
                        $("#txtEffectiveDate").val(FormatDateAuto(effDate))
                    }
                    else if (effDateLen != 8 && $.isNumeric(effDate)) {
                        alert("Auto date format allows only 8 digit numbers (ddmmyyyy)");
                        $("#txtEffectiveDate").focus();
                    }
                }
                //return false;
            });

            //Format Cheque date Automatically
            $("#txtChequeDate").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtChequeDate").val() != "") {
                    var effDate = $("#txtChequeDate").val();
                    effDateLen = effDate.length
                    if (effDateLen == 8 && $.isNumeric(effDate)) {
                        $("#txtChequeDate").val(FormatDateAuto(effDate))
                    }
                }
                //return false;
            });

            $("#txtBatchNo").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtBatchNo").val() != "") {
                    var BatchNo = $("#txtBatchNo").val();
                    BatchNoLen = BatchNo.length
                    if ($.isNumeric(BatchNo)) {
                        if (BatchNoLen != 6) {
                            alert("Invalid batch date");
                            $("#txtBatchNo").focus();
                        }
                        else {
                            var lastTwoDigit = BatchNo.substring(4);
                            if (Number(lastTwoDigit) >= 1 && Number(lastTwoDigit) <= 12) {
                            }
                            else {
                                alert("Batch date month part is invalid")
                                $("#txtBatchNo").focus();
                            }
                        }
                    }
                    else {
                        alert("Batch date contains non numeric character")
                        $("#txtBatchNo").focus();
                    }
                }
                //return false;
            });


            //retrieve data on focus loss
            $("#txtMainAcctDebit").on('focusout', function(e) {
            e.preventDefault();
            $("#txtMainAcctDebitDesc").val('')
            $("#txtSubAcctDebitDesc").val('')
                if ($("#txtMainAcctDebit").val() != "" && $("#txtSubAcctDebit").val() != "")
                    LoadChartInfo("txtSubAcctDebit", "txtMainAcctDebit", "Main", "DR");
                $("#txtSubAcctDebit").focus();
                //return false;
            });

            //retrieve data on focus loss
            $("#txtSubAcctDebit").on('focusout', function(e) {
                e.preventDefault();
                $("#txtMainAcctDebitDesc").val('')
                $("#txtSubAcctDebitDesc").val('')
                if ($("#txtSubAcctDebit").val() != "" && $("#txtMainAcctDebit").val() != "")
                    LoadChartInfo("txtSubAcctDebit", "txtMainAcctDebit", "Sub", "DR");
                //return false;
            });


            //retrieve data on focus loss
            $("#txtMainAcctCredit").on('focusout', function(e) {
            $("#txtMainAcctCreditDesc").val('')
            $("#txtSubAcctCreditDesc").val('')
                e.preventDefault();
                if ($("#txtMainAcctCredit").val() != "" && $("#txtSubAcctCredit").val() != "")
                //LoadChartInfo("txtSubAcctDebit", "txtMainAcctCredit", "Main", "CR");
                    LoadChartInfo("txtSubAcctCredit", "txtMainAcctCredit", "Main", "CR");
                $("#txtSubAcctCredit").focus();
                //return false;
            });

            //retrieve data on focus loss
            $("#txtSubAcctCredit").on('focusout', function(e) {
                e.preventDefault();

                if ($("#txtSubAcctCredit").val() != "" && $("#txtMainAcctCredit").val() != "")
                //  LoadChartInfo("txtSubAcctCredit", "txtMainAcctDebit", "Sub", "CR");
                    LoadChartInfo("txtSubAcctCredit", "txtMainAcctCredit", "Sub", "CR");
                //return false;
            });

            //retrieve data on focus loss
            $("#txtReceiptRefNo").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtReceiptRefNo").val() != "" && ($("#lblRefNo").text() == "Proposal No" || $("#lblRefNo").text() == "Policy No")) {
                    LoadPolicyInfoObject();
                }
                return false;
            });

            //retrieve data on focus loss branches

            $("#txtBranchCode").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtBranchCode").val() != "")
                    LoadBranchInfoObject();
                //return false;
            });

            //retrieve data on focus loss currency

            $("#txtCurrencyCode").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtCurrencyCode").val() != "")
                    LoadCurrencyObject();
                //return false;
            });


            $("#txtReceiptCode").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtReceiptCode").val() != "") {
                    GetReceiptType();
                }
                //return false;
            });

            //Get receipt mode description
            $("#txtMode").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtMode").val() != "") {
                    GetReceiptMode();
                }
                //return false;
            });

            $("#cmbReceiptType").on('focusout', function(e) {
                e.preventDefault();
                if ($("#cmbReceiptCode").val() != "")
                    $('#txtReceiptCode').val($("#cmbReceiptCode").val())

                //return false;
            });



            //call popup to browse the Policies
            $('#imgReceiptRefNo').click(function(e) {
                e.preventDefault();
                var src = "\PolicyBrowse.aspx";
                $.modal('<iframe id="simplemodal-container" src="' + src + '" height="500" width="830" style="border:0">', {
                    closeHTML: "<a  class='modalCloseImg' href='#'></a>",
                    containerCss: {
                        backgroundColor: "#fff",
                        borderColor: "#fff",
                        height: 500,
                        padding: 0,
                        width: 830
                    },
                    appendTo: 'form',
                    persist: true,
                    overlayClose: true,
                    opacity: 30,
                    overlayCss: { backgroundColor: "black" },
                    onClose: function(dialog) {

                        var resultPolicy = $("iframe[src='PolicyBrowse.aspx']").contents().find("#txtValue").val();
                        var resultProposal = $("iframe[src='PolicyBrowse.aspx']").contents().find("#txtValue1").val();
                        //                       alert(resultPolicy + "--" + resultProposal);

                        switch ($('#cmbReceiptType').val()) {
                            case "D":
                                if (resultProposal.length > 0)
                                    $('#txtReceiptRefNo').attr('value', resultProposal); // proposal code

                                break;
                            case "P":

                                if (resultPolicy.length > 0)
                                    $('#txtReceiptRefNo').attr('value', resultPolicy); // policy code
                                break;
                            default:
                                //$('#lblRefNo').text('Ref No');
                                break;
                        }


                        dialog.data.fadeOut('200', function() {
                            dialog.container.slideUp('200', function() {
                                dialog.overlay.fadeOut('200', function() {
                                    $.modal.close();
                                });
                            });
                        });
                    }
                });
            });


            //call print receipt popup 
            $('#butPrint').click(function(e) {
                e.preventDefault();
                //copy content of receipt number to the print dialog receipt number
                $('#txtRecptNo').attr('value', document.getElementById('txtReceiptNo').value)

                $('#PrintDialog').css({ display: true });
                $('#PrintDialog').modal({
                    containerCss: {
                        backgroundColor: "#fff",
                        borderColor: "#fff",
                        height: 400,
                        padding: 5,
                        width: 500
                    },
                    appendTo: 'form',
                    persist: true,
                    overlayClose: false,
                    opacity: 40,
                    overlayCss: { backgroundColor: "black" }

                }
              );
            });
            //call popup to add to the main account
            $('#MainAcctDebitAdd, #SubAcctDebitAdd,#MainAcctCreditAdd,#SubAcctCreditAdd').click(function(e) {
                e.preventDefault();
                var src = "\ChartOfAccountsEntryBrowse.aspx";
                $.modal('<iframe id="simplemodal-container" src="' + src + '" height="500" width="1100" style="border:0">', {
                    closeHTML: "<a  class='modalCloseImg' href='#'></a>",
                    containerCss: {
                        backgroundColor: "#fff",
                        borderColor: "#fff",
                        height: 500,
                        padding: 0,
                        width: 1100
                    },
                    appendTo: 'form',
                    persist: true,
                    overlayClose: true,
                    opacity: 50,
                    overlayCss: { backgroundColor: "black" },
                    onClose: function(dialog) {

                        dialog.data.fadeOut('200', function() {
                            dialog.container.slideUp('200', function() {
                                dialog.overlay.fadeOut('200', function() {
                                    $.modal.close();
                                });
                            });
                        });
                    }
                });
            });


            //call popup to browse the main account
            $('#MainAccountDebitSearch').click(function(e) {
                e.preventDefault();
                var src = "\AccountChartBrowse.aspx";
                $.modal('<iframe id="simplemodal-container" src="' + src + '" height="500" width="830" style="border:0">', {
                    closeHTML: "<a  class='modalCloseImg' href='#'></a>",
                    containerCss: {
                        backgroundColor: "#fff",
                        borderColor: "#fff",
                        height: 500,
                        padding: 0,
                        width: 830
                    },
                    appendTo: 'form',
                    persist: true,
                    overlayClose: true,
                    opacity: 30,
                    overlayCss: { backgroundColor: "black" },
                    onClose: function(dialog) {

                        var resultValueDR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtValue").val();
                        var resultDescDR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtDesc").val();

                        if (resultValueDR.length > 0)
                        //$('#txtMainAcctDebit').attr('value', resultValueDR); // Main account code
                            $('#txtMainAcctDebit').val(resultValueDR)
                        if (resultDescDR.length > 0)
                            $('#txtMainAcctDebitDesc').attr('value', resultDescDR); // Main account description

                        var resultValSubDR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtValue1").val();

                        var resultDescSubDR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtDesc1").val();
                        if (resultValSubDR.length > 0)
                            document.getElementById('txtSubAcctDebit').value = resultValSubDR
                        if (resultDescSubDR.length > 0)
                            $('#txtSubAcctDebitDesc').attr('value', resultDescSubDR);


                        dialog.data.fadeOut('200', function() {
                            dialog.container.slideUp('200', function() {
                                dialog.overlay.fadeOut('200', function() {
                                    $.modal.close();
                                });
                            });
                        });
                    }
                });
            });

            //call popup to browse the sub account
            $('#SubAccountDebitSearch').click(function(e) {
                e.preventDefault();
                //var src = "\AccountChartBrowse.aspx?MainAcct=" + $('#txtMainAcctDebit').val();
                //               var src = "\AccountChartBrowse.aspx?MainAcct=" + $('#txtMainAcctDebit').val();
                var src = "\AccountChartBrowse.aspx";
                $.modal('<iframe id="simplemodal-container" src="' + src + '" height="500" width="830" style="border:0">', {
                    closeHTML: "<a  class='modalCloseImg' href='#'></a>",
                    containerCss: {
                        backgroundColor: "#fff",
                        borderColor: "#fff",
                        height: 500,
                        padding: 0,
                        width: 830
                    },
                    appendTo: 'form',
                    persist: true,
                    overlayClose: true,
                    opacity: 30,
                    overlayCss: { backgroundColor: "black" },
                    onClose: function(dialog) {

                        var resultValSubDR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtValue1").val();

                        var resultDescSubDR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtDesc1").val();
                        if (resultValSubDR.length > 0)
                            document.getElementById('txtSubAcctDebit').value = resultValSubDR
                        if (resultDescSubDR.length > 0)
                            $('#txtSubAcctDebitDesc').attr('value', resultDescSubDR);

                        dialog.data.fadeOut('200', function() {
                            dialog.container.slideUp('200', function() {
                                dialog.overlay.fadeOut('200', function() {
                                    $.modal.close();
                                });
                            });
                        });
                    }

                });
            });

            //call popup to browse
            $('#MainAccountCreditSearch').click(function(e) {
                e.preventDefault();
                var src = "\AccountChartBrowse.aspx";
                $.modal('<iframe id="simplemodal-container" src="' + src + '" height="500" width="830" style="border:0">', {
                    closeHTML: "<a  class='modalCloseImg' href='#'></a>",
                    onOpen: function(dialog) {
                        dialog.overlay.fadeIn('slow', function() {
                            dialog.container.slideDown('100', function() {
                                dialog.data.fadeIn('fast');
                            });
                        });
                    },
                    preventDefault: true,
                    containerCss: {
                        backgroundColor: "#fff",
                        borderColor: "#fff",
                        height: 500,
                        padding: 0,
                        width: 830
                    },
                    appendTo: 'form',
                    persist: true,
                    overlayClose: true,
                    opacity: 30,
                    overlayCss: { backgroundColor: "black" },
                    onClose: function(dialog) {

                        var resultValueCR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtValue").val();
                        var resultDescCR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtDesc").val();
                        if (resultValueCR.length > 0)
                            $('#txtMainAcctCredit').attr('value', resultValueCR);

                        if (resultDescCR.length > 0)
                            $('#txtMainAcctCreditDesc').attr('value', resultDescCR);

                        var resultValSubCR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtValue1").val();
                        var resultDescSubCR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtDesc1").val();

                        if (resultValSubCR.length > 0)
                            $('#txtSubAcctCredit').attr('value', resultValSubCR);

                        if (resultDescSubCR.length > 0)
                            $('#txtSubAcctCreditDesc').attr('value', resultDescSubCR);

                        dialog.data.fadeOut('200', function() {
                            dialog.container.slideUp('200', function() {
                                dialog.overlay.fadeOut('200', function() {
                                    $.modal.close();
                                });
                            });
                        });
                    }
                });
            });

            //call popup to browse the sub account
            $('#SubAccountCreditSearch').click(function(e) {
                e.preventDefault();
                // var src = "\AccountChartBrowse.aspx?MainAcct=" + $('#txtMainAcctCredit').val();
                var src = "\AccountChartBrowse.aspx";
                $.modal('<iframe id="simplemodal-container" src="' + src + '" height="500" width="830" style="border:0">', {
                    closeHTML: "<a  class='modalCloseImg' href='#'></a>", containerCss: {
                        backgroundColor: "#fff",
                        borderColor: "#fff",
                        height: 500,
                        padding: 0,
                        width: 830
                    },
                    appendTo: 'form',
                    persist: true,
                    overlayClose: true,
                    opacity: 30,
                    overlayCss: { backgroundColor: "black" },
                    onClose: function(dialog) {

                        var resultValSubCR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtValue1").val();
                        var resultDescSubCR = $("iframe[src='AccountChartBrowse.aspx']").contents().find("#txtDesc1").val();

                        if (resultValSubCR.length > 0)
                            $('#txtSubAcctCredit').attr('value', resultValSubCR);

                        if (resultDescSubCR.length > 0)
                            $('#txtSubAcctCreditDesc').attr('value', resultDescSubCR);

                        dialog.data.fadeOut('200', function() {
                            dialog.container.slideUp('200', function() {
                                dialog.overlay.fadeOut('200', function() {
                                    $.modal.close();
                                });
                            });
                        });
                    }
                });
            });

            //retrieve data on focus loss
            $("#txtTransDesc1").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtTransDesc1").val() != "")
                    LoadPeriodsCover();
                return false;
            });

            //retrieve data on focus
            // $("#txtTransDesc1").on('focusout', function(e) {
            //var keyCode = e.keyCode || e.which;
            //trap the TAB key
            //if (keyCode == 9) {
            //    e.preventDefault();
            // call custom function here
            //LoadMOPPremium();
            // return false;
            //   }
            //});


            //loading screen functionality - this part is additional - start
            $("#divTable").ajaxStart(OnAjaxStart);
            $("#divTable").ajaxError(OnAjaxError);
            $("#divTable").ajaxSuccess(OnAjaxSuccess);
            $("#divTable").ajaxStop(OnAjaxStop);
            $("#divTable").ajaxComplete(OnAjaxComplete);
            //loading screen functionality - this part is additional - end
        });

        // ajax call to load account chart information
        function LoadChartInfo(subaccountcode, mainaccountcode, ctype, drcr) {
            console.log(document.getElementById(subaccountcode).value)
            console.log(document.getElementById(mainaccountcode).value)
            $.ajax({
                type: "POST",
                url: "PRG_FIN_RECPT_ISSUE.aspx/GetAccountChartDetails",
                //                data: JSON.stringify({ _accountcode: document.getElementById(accountcode).value, _type: ctype }),
                data: JSON.stringify({ _accountsubcode: document.getElementById(subaccountcode).value, _accountmaincode: document.getElementById(mainaccountcode).value }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    var xmlDoc = $.parseXML(data.d);
                    var xml = $(xmlDoc);
                    var accountcharts = xml.find("Table");
                    retrieve_AccountChartInfoValues(accountcharts, ctype, drcr)
                    if (ctype == "Sub" && drcr == "DR") {
                        $("#txtMainAcctCredit").focus();
                        $("#txtSubAcctDebit").val("000000");
                    }
                    //retrieve_AccountChartInfoValues(accountcharts, drcr)
                },
                failure: OnFailure,
                //error: OnError_LoadChartInfo(ctype, drcr)
                error: function() {
                    alert('Error!: Account Chart could not be Retrieved. Parameters sent is empty or invalid. Please Re-Confirm' + '<br/>');
                    if (ctype == "Sub" && drcr == "DR") {
                        $("#txtMainAcctDebit").focus();
                        $("#txtSubAcctDebit").val("000000");
                    }
                    else if (ctype == "Sub" && drcr == "CR") {
                        $("#txtMainAcctCredit").focus();
                        $("#txtSubAcctCredit").val("000000");
                    }
                }
            });
            // this avoids page refresh on button click
            return false;
        }
        // retrieve the values and
        function retrieve_AccountChartInfoValues(accountcharts, ctype, drcr) {
            //debugger;
            $.each(accountcharts, function() {
                var accountchart = $(this);

                if (ctype == "Main" && drcr == "DR") {

                    document.getElementById('txtMainAcctDebitDesc').value = $(this).find("sMainDesc").text()
                    // document.getElementById('txtSubAcctDebit').value = $(this).find("sSubCode").text()
                    // document.getElementById('txtSubAcctDebitDesc').value = $(this).find("sSubDesc").text()
                    document.getElementById('txtSubAcctDebit').value = $(this).find("sSubCode").text()
                    document.getElementById('txtSubAcctDebitDesc').value = $(this).find("sSubDesc").text()
                }
                else if (ctype == "Main" && drcr == "CR") {
                    document.getElementById('txtMainAcctCreditDesc').value = $(this).find("sMainDesc").text()
                    //  document.getElementById('txtSubAcctCredit').value = $(this).find("sSubCode").text()
                    //  document.getElementById('txtSubAcctCreditDesc').value = $(this).find("sSubDesc").text()

                    document.getElementById('txtSubAcctCredit').value = $(this).find("sSubCode").text()
                    document.getElementById('txtSubAcctCreditDesc').value = $(this).find("sSubDesc").text()
                }
                else if (ctype == "Sub" && drcr == "DR") {

                    document.getElementById('txtSubAcctDebit').value = $(this).find("sSubCode").text()
                    document.getElementById('txtSubAcctDebitDesc').value = $(this).find("sSubDesc").text()
                    document.getElementById('txtMainAcctDebit').value = $(this).find("sMainCode").text()
                    document.getElementById('txtMainAcctDebitDesc').value = $(this).find("sMainDesc").text()

                }
                else if (ctype == "Sub" && drcr == "CR") {
                    document.getElementById('txtSubAcctCredit').value = $(this).find("sSubCode").text()
                    document.getElementById('txtSubAcctCreditDesc').value = $(this).find("sSubDesc").text()
                    document.getElementById('txtMainAcctCredit').value = $(this).find("sMainCode").text()
                    document.getElementById('txtMainAcctCreditDesc').value = $(this).find("sMainDesc").text()
                }

            });
        }


        // ajax call to load policy information
        function LoadBranchInfoObject() {
            $.ajax({
                type: "POST",
                url: "PRG_FIN_RECPT_ISSUE.aspx/GetBranchInformation",
                data: JSON.stringify({ _branchcode: document.getElementById('txtBranchCode').value }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess_LoadBranchInfoObject,
                failure: OnFailure,
                error: OnError_LoadBranchInfoObject
            });
            // this avoids page refresh on button click
            return false;
        }
        function OnSuccess_LoadBranchInfoObject(response) {
            //debugger;

            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var branches = xml.find("Table");
            retrieve_BranchInfoValues(branches);

        }
        // retrieve the values for branch
        function retrieve_BranchInfoValues(branches) {
            //debugger;
            $.each(branches, function() {
                var branch = $(this);
                $("#cmbBranchCode").val($(this).find("sCode").text())
            });
        }


        // ajax call to load policy information
        function LoadPolicyInfoObject() {
            $.ajax({
                type: "POST",
                url: "PRG_FIN_RECPT_ISSUE.aspx/GetPolicyInformation",
                data: JSON.stringify({ _polnum: document.getElementById('txtReceiptRefNo').value, _type: document.getElementById('cmbReceiptType').value }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess_LoadPolicyInfoObject,
                failure: OnFailure,
                error: OnError_LoadPolicyInfoObject
            });
            // this avoids page refresh on button click

            return false;
        }
        // on sucess get the xml
        function OnSuccess_LoadPolicyInfoObject(response) {
            //debugger;

            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var policyholders = xml.find("Table");
            retrieve_PolicyInfoValues(policyholders);

        }
        // retrieve the values and
        function retrieve_PolicyInfoValues(policyholders) {
            //debugger;
            $.each(policyholders, function() {
                var policyholder = $(this);
                document.getElementById('txtInsuredCode').value = $(this).find("TBIL_POLY_ASSRD_CD").text();
                document.getElementById('txtAgentCode').value = $(this).find("TBIL_POLY_AGCY_CODE").text();
                document.getElementById('txtAssuredName').value = $(this).find("Insured_Name").text();
                document.getElementById('txtAssuredAddress').value = $(this).find("Insured_Address").text();
                document.getElementById('txtPayeeName').value = $(this).find("Insured_Name").text();
                document.getElementById('txtPolRegularContrib').value = $(this).find("TBIL_POL_PRM_DTL_MOP_PRM_LC").text();
                document.getElementById('txtAgentName').value = $(this).find("Agent_Name").text();
                document.getElementById('txtMOP').value = $(this).find("Payment_Mode").text();
                document.getElementById('txtMOPDesc').value = $(this).find("Payment_Mode_Desc").text();
                document.getElementById('txtPolicyEffDate').value = $(this).find("TBIL_POLICY_EFF_DT").text();
                document.getElementById('txtFileNo').value = $(this).find("File_No").text();
                document.getElementById('txtProductCode').value = $(this).find("Product_Code").text();

                if ($(this).find("TBIL_POLY_ASSRD_CD").text() == "" || $(this).find("TBIL_POLY_AGCY_CODE").text() == "" || $(this).find("TBIL_POL_PRM_DTL_MOP_PRM_LC").text() == "0.00" || $(this).find("Payment_Mode").text() == "")
                    alert("Please contact technical department, record not completed for policy no " + document.getElementById('txtReceiptRefNo').value);
                $("#txtReceiptRefNo").focus();


            });

        }

        // ajax call to load receipts cover details
        function LoadPeriodsCover() {
            $.ajax({
                type: "POST",
                url: "PRG_FIN_RECPT_ISSUE.aspx/PaymentsPeriodCover",
                data: JSON.stringify({ _polnum: document.getElementById('txtReceiptRefNo').value,
                    _mop: document.getElementById('txtMOP').value,
                    _effdate: document.getElementById('txtPolicyEffDate').value,
                    _contrib: document.getElementById('txtPolRegularContrib').value,
                    _amtpaid: document.getElementById('txtReceiptAmtLC').value
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess_LoadPeriodsCover,
                failure: OnFailure_LoadPeriodsCover,
                error: OnError_LoadPeriodsCover
            });
            // this avoids page refresh on button click
            return false;
        }
        // on sucess get the xml
        function OnSuccess_LoadPeriodsCover(response) {
            //debugger;
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var coverdets = xml.find("Table");
            retrieve_LoadPeriodsCover(coverdets)

        }
        // retrieve the values and
        function retrieve_LoadPeriodsCover(coverdets) {
            //debugger;
            $.each(coverdets, function() {
                var coverdet = $(this);

                document.getElementById('txtTransDesc2').value = $(this).find("sPeriodsCoverRange").text()
            });
        }
        function OnFailure_LoadPeriodsCover(response) {
            //debugger;
            //alert('Failure!!!' + '<br/>' + response.reponseText);
            alert('Failure!: Periods Covered Failed. Parameters sent is empty or invalid. Please Re-Confirm' + '<br/>');
        }

        // function OnError_LoadChartInfo(response) {
        function OnError_LoadChartInfo(ctype, drcr) {
            //debugger;
            //var errorText = response.responseText;
            //alert('Error!!!' + '\n\n' + errorText);
            alert('Error!: Account Chart could not be Retrieved. Parameters sent is empty or invalid. Please Re-Confirm' + '<br/>');
        }
        function OnError_LoadBranchInfoObject(response) {
            //debugger;
            //var errorText = response.responseText;
            //alert('Error!!!' + '\n\n' + errorText);
            alert('Error!: Branch Infomation Not Found. Parameters Empty or Invalid. Please Re-Confirm' + '<br/>');
            $('#txtBranchCode').focus();
        }
        function OnError_LoadPolicyInfoObject(response) {
            //debugger;
            //var errorText = response.responseText;
            //alert('Error!!!' + '\n\n' + errorText);
            alert('Error!: Policy Infomation Not Found. Parameters Empty or Invalid. Please Re-Confirm' + '<br/>');
            $('#cmbMode').focus();
        }
        function OnError_LoadPeriodsCover(response) {
            //debugger;
            //var errorText = response.responseText;
            //alert('Error!!!' + '\n\n' + errorText);
            alert('Error!: Load Periods Cover Not Found. Parameters Empty or Invalid. Please Re-Confirm' + '<br/>');
        }
        function OnError_LoadCurrencyObject(response) {
            //debugger;
            //var errorText = response.responseText;
            //alert('Error!!!' + '\n\n' + errorText);
            alert('Error!: Currency Code Not Found. Parameters Empty or Invalid. Please Re-Confirm' + '<br/>');
            $('#txtCurrencyCode').focus();
        }


        // loading screen functionality functions - this part is additional - start
        function OnAjaxStart() {
            //debugger;
            //alert('Starting...');
            $("#divLoading").css("display", "block");
        }
        function OnFailure(response) {
            //debugger;
            alert('Failure!!!' + '<br/>' + response.reponseText);
        }
        function OnError(response) {
            //debugger;
            var errorText = response.responseText;
            alert('Error!!!' + '\n\n' + errorText);
        }
        function OnAjaxError() {
            //debugger;
            alert('Error!: Invalid Ajax Call');
        }
        function OnAjaxSuccess() {
            //debugger;
            //alert('Sucess!!!');
            $("#divLoading").css("display", "none");
        }
        function OnAjaxStop() {
            //debugger;
            //alert('Stop!!!');
            $("#divLoading").css("display", "none");
        }
        function OnAjaxComplete() {
            //debugger;
            //alert('Completed!!!');
            $("#divLoading").css("display", "none");
        }
        // loading screen functionality functions - this part is additional - end


        // ajax call to load currency type
        function LoadCurrencyObject() {
            $.ajax({
                type: "POST",
                url: "PRG_FIN_RECPT_ISSUE.aspx/GetCurrencyInformation",
                data: JSON.stringify({ _currencycode: document.getElementById('txtCurrencyCode').value }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess_LoadCurrencyObject,
                failure: OnFailure,
                error: OnError_LoadCurrencyObject
            });
            // this avoids page refresh on button click
            return false;
        }
        function OnSuccess_LoadCurrencyObject(response) {
            //debugger;

            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var currencies = xml.find("Table");
            retrieve_LoadCurrencyObject(currencies);

        }
        // retrieve the values for currency
        function retrieve_LoadCurrencyObject(currencies) {
            //debugger;
            $.each(currencies, function() {
                var currency = $(this);
                $("#cmbCurrencyType").val($(this).find("TBIL_COD_ITEM").text())
            });
        }


        function GetReceiptMode() {
            receiptmode = $('#txtMode').val()
            if (receiptmode == "C") {
                $("#cmbMode").val('C');
            }
            else if (receiptmode == "Q") {
                $("#cmbMode").val('Q');
            }
            else if (receiptmode == "D") {
                $("#cmbMode").val('D');
            }
            else if (receiptmode == "T") {
                $("#cmbMode").val('T');
            }
            else if (receiptmode == "C") {
                $("#cmbMode").val('C');
            }
            else {
                alert("Receipt mode not found");
                $("#cmbMode").val(0);
            }
        }

        function FormatDateAuto(effDate) {
              var effDateDay = effDate.substring(0, 2);
              var effDateMonth = effDate.substring(2, 4);
              var effDateYear = effDate.substring(4);
              return effDateDay + "/" + effDateMonth + "/" + effDateYear;    
        }
    </script>

</head>
<body onload="<%=publicMsgs%>" onclick="return cancelEvent('onbeforeunload')">
    <form id="PRG_FIN_RECPT_ISSUE" runat="server" submitdisabledcontrols="true">
    <div class="newpage">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Literal runat="server" Visible="false" ID="litMsgs"></asp:Literal>
                        <asp:Label runat="server" ID="Status" Font-Bold="true" ForeColor="Red" Visible="true"
                            Text="Status:"> </asp:Label>
                        <asp:Label runat="server" ID="lblError" Font-Bold="true" ForeColor="Red" Visible="false"> </asp:Label>
                    </td>
                </tr>
            </table>
            <div class="grid">
                <div class="rounded">
                    <div class="top-outer">
                        <div class="top-inner">
                            <div class="top">
                                <h2>
                                    Individual Premium Reciepts Entry</h2>
                            </div>
                        </div>
                    </div>
                    <div class="mid-outer">
                        <div class="mid-inner">
                            <div class="mid">
                                <table class="tbl_menu_new">
                                    <tr>
                                        <td colspan="4" class="myMenu_Title" align="center">
                                            Individual Premium Reciepts Entry
                                        </td>
                                        <td class="style5">
                                        </td>
                                        <td class="style5">
                                        </td>
                                        <td class="style5">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            <asp:CheckBox ID="chkReceiptNo" runat="server" AutoPostBack="True" Text="Receipt Number" />
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReceiptNo" runat="server" Width="150px" Enabled="False" AutoPostBack="True"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            Entry Date
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtEntryDate" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Company Code
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCompanyCode" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            Serial Number
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtSerialNo" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style9">
                                            Batch Date
                                        </td>
                                        <td class="style3">
                                            <asp:TextBox ID="txtBatchNo" runat="server" Width="150px">0</asp:TextBox>(yyyymm)
                                        </td>
                                        <td class="style10">
                                            Transaction Type
                                        </td>
                                        <td class="style11">
                                            <asp:DropDownList ID="cmbTransType" runat="server" Width="150px">
                                                <asp:ListItem Value="R" Text="Receipt" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Temp. Receipt No
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="txtTempReceiptNo" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            Effective Date
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtEffectiveDate" runat="server" Width="150px"></asp:TextBox>

                                            <script language="JavaScript" type="text/javascript">
                                                new tcal({ 'formname': 'PRG_FIN_RECPT_ISSUE', 'controlname': 'txtEffectiveDate' });</script>

                                            (dd/mm/yyyy)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style13">
                                            Receipt Mode
                                        </td>
                                        <td class="style4">
                                            <asp:TextBox ID="txtMode" runat="server" Width="26px"></asp:TextBox>
                                            <asp:DropDownList ID="cmbMode" runat="server" Width="120px" AutoPostBack="True">
                                                <asp:ListItem Value="0" Text="Receipt Mode"></asp:ListItem>
                                                <asp:ListItem Value="C" Text="C-Cash"></asp:ListItem>
                                                <asp:ListItem Value="Q" Text="Q-Cheque"></asp:ListItem>
                                                <asp:ListItem Value="D" Text="D-Direct Payment"></asp:ListItem>
                                                <asp:ListItem Value="T" Text="T-Teller"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="style14">
                                            Receipt Type
                                        </td>
                                        <td class="style15">
                                            <asp:TextBox ID="txtReceiptCode" runat="server" Width="18px"></asp:TextBox>
                                            <asp:DropDownList ID="cmbReceiptType" runat="server" Width="126px" OnSelectedIndexChanged="cmbReceiptType_SelectedIndexChanged"
                                                AutoPostBack="True">
                                                <asp:ListItem Value="0" Text="Receipt Type"></asp:ListItem>
                                                <asp:ListItem Value="D" Text="D-Premium Deposit"></asp:ListItem>
                                                <asp:ListItem Value="P" Text="P-Regular Premium"></asp:ListItem>
                                                <asp:ListItem Value="U" Text="U-Lumpsum"></asp:ListItem>
                                                <asp:ListItem Value="F" Text="F-FAC"></asp:ListItem>
                                                <asp:ListItem Value="C" Text="C-Co-Insurance"></asp:ListItem>
                                                <asp:ListItem Value="L" Text="L-Lease"></asp:ListItem>
                                                <asp:ListItem Value="X" Text="X-Cheque Exchange"></asp:ListItem>
                                                <asp:ListItem Value="V" Text="V-Dividend"></asp:ListItem>
                                                <asp:ListItem Value="R" Text="R-Refund"></asp:ListItem>
                                                <asp:ListItem Value="Y" Text="Y-Claim Recovery"></asp:ListItem>
                                                <asp:ListItem Value="I" Text="I-Interest Received"></asp:ListItem>
                                                <asp:ListItem Value="T" Text="T-Investment Income"></asp:ListItem>
                                                <asp:ListItem Value="S" Text="S-Share Sale"></asp:ListItem>
                                                <asp:ListItem Value="G" Text="G-Salvage"></asp:ListItem>
                                                <asp:ListItem Value="O" Text="O-Others"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            <asp:Label ID="lblRefNo" runat="server" EnableViewState="False" Text="Ref No."></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReceiptRefNo" runat="server" Width="250px"></asp:TextBox><img
                                                src="img/glass1.png" id="imgReceiptRefNo" alt="search" class="searchImage" />
                                        </td>
                                        <td class="style16">
                                            Insured Code
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtInsuredCode" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Teller No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTellerNo" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            Assured Address
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtAssuredAddress" runat="server" Width="290px" Enabled="false"
                                                BorderStyle="None"></asp:TextBox>
                                            <asp:TextBox ID="txtFileNo" runat="server" Width="150px" BorderStyle="None" CssClass="popupOffset"></asp:TextBox>
                                            <asp:TextBox ID="txtProductCode" runat="server" Width="150px" CssClass="popupOffset"
                                                BorderStyle="None"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Currency Type
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCurrencyCode" runat="server" Width="36px"></asp:TextBox>
                                            <asp:DropDownList ID="cmbCurrencyType" runat="server" Width="110px" Height="21px"
                                                AutoPostBack="True">
                                                <asp:ListItem Value="0" Text="Currency Type"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CustomValidator ID="csValidateCurrencyType" runat="server" ErrorMessage="Please Select the Currency Type">*</asp:CustomValidator>
                                        </td>
                                        <td class="style16">
                                            Agent Code
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtAgentCode" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="ChequeRow">
                                        <td class="style6">
                                            Cheque Number
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChequeNo" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            Cheque Date
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtChequeDate" runat="server" Width="150px"></asp:TextBox>

                                            <script language="JavaScript" type="text/javascript">
                                                new tcal({ 'formname': 'PRG_FIN_RECPT_ISSUE', 'controlname': 'txtChequeDate' });</script>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            <asp:Label ID="Polyeffdate" runat="server" CssClass="popupOffset"></asp:Label>
                                        </td>
                                        <td class="style12">
                                            <asp:TextBox ID="txtPolicyEffDate" runat="server" Width="150px" CssClass="popupOffset">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Receipt Amount (LC)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReceiptAmtLC" runat="server" Width="150px" Text="0.00" AutoPostBack="True"></asp:TextBox><%--<asp:RegularExpressionValidator
                                                ID="rvDecimal" runat="server" ErrorMessage="Please Enter a Valid Receipt Amount"
                                                ValidationExpression="^(-)?\d+(\.\d\d)?$" ControlToValidate="txtReceiptAmtLC">*</asp:RegularExpressionValidator>--%>
                                        </td>
                                        <td class="style16">
                                            Receipt Amount (FC)
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtReceiptAmtFC" runat="server" Width="150px" Text="0.00" 
                                                Enabled="False"></asp:TextBox><%--<asp:RegularExpressionValidator
                                                ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please Enter a Valid Receipt Amount"
                                                ValidationExpression="^(-)?\d+(\.\d\d)?$" ControlToValidate="txtReceiptAmtFC">*</asp:RegularExpressionValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Payee Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPayeeName" runat="server" Width="270px"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            Branch
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtBranchCode" runat="server" Width="40px"></asp:TextBox>
                                            <asp:DropDownList ID="cmbBranchCode" runat="server" Width="150px" AutoPostBack="True">
                                                <asp:ListItem Value="0" Text="Branch Code"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Trans. Desc. 1
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransDesc1" runat="server" Width="270px"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            <asp:Label ID="lblBankGLCode" runat="server" Text="Bank GL Code" CssClass="popupOffset"> </asp:Label>
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtBankGLCode" runat="server" Width="270px" CssClass="popupOffset"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Trans. Desc. 2
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransDesc2" runat="server" Width="270px"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            Comm. Applicable?
                                        </td>
                                        <td class="style17">
                                            <asp:DropDownList ID="cmbCommissions" runat="server" Width="100px">
                                                <asp:ListItem Value="0" Text="Commissions Applicable?"></asp:ListItem>
                                                <asp:ListItem Value="Y" Text="YES" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="N" Text="NO"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CustomValidator ID="csValidateCommissions" runat="server" OnServerValidate="csValidateCommissions_ServerValidate"
                                                ControlToValidate="cmbCommissions" ErrorMessage="Please Select Commission Applicable">*</asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Policy Regular Contrib.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPolRegularContrib" runat="server" Width="150px" Text="0.00"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            Mode of Payment
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtMOP" runat="server" Width="20px"> </asp:TextBox><asp:TextBox
                                                ID="txtMOPDesc" runat="server" Width="250px" BorderStyle="None"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Main Account (Debit)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMainAcctDebit" runat="server" Width="150px"></asp:TextBox>
                                            <img src="img/glass1.png" id="MainAccountDebitSearch" alt="search" class="searchImage" /><img
                                                src="img/plusimage.png" id="MainAcctDebitAdd" alt="add record" class="searchImage" />
                                        </td>
                                        <td class="style16">
                                            <asp:Label ID="lblMainAcctCR" runat="server" Text="Main A/C (Credit)"></asp:Label>
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtMainAcctCredit" runat="server" Width="150px"></asp:TextBox><img
                                                src="img/glass1.png" id="MainAccountCreditSearch" alt="search" class="searchImage" /><img
                                                    src="img/plusimage.png" id="MainAcctCreditAdd" alt="add record" class="searchImage" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            Sub Account (Debit)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubAcctDebit" runat="server" Width="150px"></asp:TextBox>
                                            <img src="img/glass1.png" id="SubAccountDebitSearch" alt="search" class="searchImage" /><img
                                                src="img/plusimage.png" id="SubAcctDebitAdd" alt="add record" class="searchImage" />
                                        </td>
                                        <td class="style16">
                                            <asp:Label ID="lblSubAcctCR" runat="server" Text="Sub A/C (Credit)"></asp:Label>
                                        </td>
                                        <td class="style17">
                                            <asp:TextBox ID="txtSubAcctCredit" runat="server" Width="150px"></asp:TextBox><img
                                                src="img/glass1.png" id="SubAccountCreditSearch" alt="search" class="searchImage" /><img
                                                    src="img/plusimage.png" id="SubAcctCreditAdd" alt="add record" class="searchImage" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                        </td>
                                        <td>
                                        </td>
                                        <td class="style16">
                                        </td>
                                        <td class="style17">
                                            <asp:Button ID="butSave" runat="server" Text="Save" OnClick="butSave_Click" Visible="false" />
                                            <asp:Button ID="butSaveN" runat="server" Text="Save" OnClientClick="JavaSave_Rtn()" />
                                            <asp:Button ID="butDelete" runat="server" Text="Delete" />
                                            <asp:Button ID="butNew" runat="server" Text="New Rec" />
                                            <asp:Button ID="butPrint" runat="server" Text="Print" Visible="True" />
                                            <asp:Button ID="butClose" runat="server" Text="Close" Visible="True" />
                                        </td>
                                    </tr>
                                </table>
                                <table class="tbl_menu_new">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMainDescDisp" runat="server" Text="Main A/C DR"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Sub A/C DR"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Assured Name"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMainAcctDebitDesc" runat="server" Width="300px" BorderStyle="None"
                                                Enabled="false"> </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubAcctDebitDesc" runat="server" Width="300px" BorderStyle="None"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAssuredName" runat="server" Width="270px" Enabled="false" BorderStyle="None"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Main A/C CR"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Sub A/C CR"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="Agent's Name"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMainAcctCreditDesc" runat="server" Width="300px" BorderStyle="None"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubAcctCreditDesc" runat="server" Width="300px" BorderStyle="None"
                                                Enabled="false"> </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgentName" runat="server" Width="270px" Enabled="false" BorderStyle="None"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="bottom-outer">
                        <div class="bottom-inner">
                            <div class="bottom">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divTable">
        </div>
        <div id="PrintDialog" class="nodisplay">
            <table>
                <tr>
                    <td>
                        Receipt Number
                    </td>
                    <td>
                        <asp:TextBox ID="txtRecptNo" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="butPrintReceipt" runat="server" Text="Print" Visible="True" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
