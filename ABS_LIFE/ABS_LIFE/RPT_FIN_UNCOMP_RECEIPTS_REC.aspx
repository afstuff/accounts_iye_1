<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RPT_FIN_UNCOMP_RECEIPTS_REC.aspx.vb"
    Inherits="ABS_LIFE.RPT_FIN_UNCOMP_RECEIPTS_REC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="Stylesheet" href="SS_ILIFE.css" type="text/css" />
    <link rel="stylesheet" href="calendar.css" />
    <link href="css/general.css" rel="stylesheet" type="text/css" />
    <link href="css/grid.css" rel="stylesheet" type="text/css" />
    <link href="css/rounded.css" rel="stylesheet" type="text/css" />

    <script src="jquery-1.11.0.js" type="text/javascript"></script>

    <script src="jquery.simplemodal.js" type="text/javascript"></script>

    <script language="JavaScript" src="calendar_eu.js" type="text/javascript"></script>

    <script src="../calendar_eu.js" type="text/javascript"></script>

    <link href="../calendar.css" rel="stylesheet" type="text/css" />
    <title></title>

    <script>
        $(document).ready(function() {
            $("#txtStartBatchNo").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtStartBatchNo").val() != "") {
                    var BatchNo = $("#txtStartBatchNo").val();
                    BatchNoLen = BatchNo.length
                    if ($.isNumeric(BatchNo)) {
                        if (BatchNoLen != 6) {
                            alert("Invalid batch date");
                            $("#txtStartBatchNo").focus();
                        }
                        else {
                            var lastTwoDigit = BatchNo.substring(4);
                            if (Number(lastTwoDigit) >= 1 && Number(lastTwoDigit) <= 12) {
                            }
                            else {
                                alert("Batch date month part is invalid")
                                $("#txtStartBatchNo").focus();
                            }
                        }
                    }
                    else {
                        alert("Batch date contains non numeric character")
                        $("#txtStartBatchNo").focus();
                    }
                }
                //return false;
            });

            $("#txtEndBatchNo").on('focusout', function(e) {
                e.preventDefault();
                if ($("#txtEndBatchNo").val() != "") {
                    var BatchNo = $("#txtEndBatchNo").val();
                    BatchNoLen = BatchNo.length
                    if ($.isNumeric(BatchNo)) {
                        if (BatchNoLen != 6) {
                            alert("Invalid batch date");
                            $("#txtEndBatchNo").focus();
                        }
                        else {
                            var lastTwoDigit = BatchNo.substring(4);
                            if (Number(lastTwoDigit) >= 1 && Number(lastTwoDigit) <= 12) {
                            }
                            else {
                                alert("Batch date month part is invalid")
                                $("#txtEndBatchNo").focus();
                            }
                        }
                    }
                    else {
                        alert("Batch date contains non numeric character")
                        $("#txtEndBatchNo").focus();
                    }
                }
                //return false;
            });
        });
    </script>

</head>
<body>
    <form id="RPT_FIN_UNCOMP_RECEIPTS_REC" runat="server">
    <div>
    </div>
    <div class="newpage" align="center">
        <table>
            <tr>
                <td>
                    <asp:Literal runat="server" Visible="false" ID="litMsgs"></asp:Literal>
                    <asp:Label runat="server" ID="Status" Font-Bold="true" ForeColor="Red" Visible="true"
                        Text="Status:"> </asp:Label>
                    <asp:Label runat="server" ID="Label1" Font-Bold="true" ForeColor="Red" Visible="false"> </asp:Label>
                </td>
            </tr>
        </table>
        <div class="grid" style="width: 600px !important;">
            <div class="rounded">
                <div class="top-outer">
                    <div class="top-inner">
                        <div class="top">
                            <h2>
                                PRINT: Incomplete receipt records</h2>
                        </div>
                    </div>
                </div>
                <div class="mid-outer">
                    <div class="mid-inner">
                        <div class="mid">
                            <table class="tbl_menu_new">
                                <tr>
                                    <td colspan="2" class="myMenu_Title" align="center">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Start Batch No
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStartBatchNo" runat="server" Width="150px" MaxLength="10"></asp:TextBox>yyyymm
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        End Batch No
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndBatchNo" runat="server" Width="150px" MaxLength="10"></asp:TextBox>yyyymm
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                    <td>
                                        <asp:Button ID="butOK" runat="server" Text="OK" Style="height: 26px" />
                                        <asp:Button ID="butClose" runat="server" Text="Close" />
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
    </form>
</body>
</html>
