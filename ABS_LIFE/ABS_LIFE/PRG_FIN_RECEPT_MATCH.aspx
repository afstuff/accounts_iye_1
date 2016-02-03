<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PRG_FIN_RECEPT_MATCH.aspx.vb"
    Inherits="ABS_LIFE.PRG_FIN_RECEPT_MATCH" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script language="JavaScript" src="calendar_eu.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="Script/ScriptJS.js"></script>

    <link rel="Stylesheet" href="SS_ILIFE.css" type="text/css" />
    <link rel="stylesheet" href="calendar.css" />
    <link href="css/general.css" rel="stylesheet" type="text/css" />
    <link href="css/grid.css" rel="stylesheet" type="text/css" />
    <link href="css/rounded.css" rel="stylesheet" type="text/css" />

    <script src="jquery-1.11.0.js" type="text/javascript"></script>

    <script src="jquery.simplemodal.js" type="text/javascript"></script>

    <title>Receipt Match</title>

    <script type="text/javascript">
        $(document).ready(function() {
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
        })
    </script>

</head>
<body onload="<%=publicMsgs%>" onclick="return cancelEvent('onbeforeunload')">
    <form id="PRG_FIN_RECEPT_MATCH" runat="server" submitdisabledcontrols="true">
    <div class="newpage">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Literal runat="server" Visible="false" ID="litMsgs"></asp:Literal>
                        <asp:Label runat="server" ID="Status" Font-Bold="true" ForeColor="Red" Visible="true"
                            Text="Status:"> </asp:Label>
                        <asp:Label runat="server" ID="lblError" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="grid" style="width:900px !important;">
                <div class="rounded">
                    <div class="top-outer">
                        <div class="top-inner">
                            <div class="top">
                                <h2>
                                    Match&nbsp; Reciepts</h2>
                            </div>
                        </div>
                    </div>
                    <div class="mid-outer">
                        <div class="mid-inner">
                            <div class="mid">
                                <table class="tbl_menu_new">
                                    <tr>
                                        <td colspan="4" class="myMenu_Title" align="center">
                                            match Reciepts
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            &nbsp;<asp:Label ID="lblData_Source" runat="server" Text="Batch Date:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBatchNo" runat="server" Width="88px"></asp:TextBox>
                                            &nbsp; (yyyymm)&nbsp;
                                            <asp:Button ID="cmdMatch" runat="server" Enabled="True" Font-Bold="true" Font-Size="Large"
                                                Text="Match " Height="25px" />
                                        </td>
                                        <td class="style16">
                                            &nbsp;
                                        </td>
                                        <td class="style17">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="style16">
                                            &nbsp;
                                        </td>
                                        <td class="style17">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="cmdUnMatch" runat="server" Enabled="True" Font-Bold="true" Font-Size="Large"
                                                Text="View UnMatched" />
                                        </td>
                                        <td class="style16">
                                            <asp:Label ID="lblErr_List" runat="server" Enabled="true" ForeColor="Red" Text="Error:"
                                                Visible="false"></asp:Label>
                                        </td>
                                        <td class="style17">
                                            <asp:DropDownList ID="cboErr_List" runat="server" Visible="false" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" colspan="4">
                                            <asp:GridView ID="grdUnMthPropNos" runat="server" AllowPaging="True" AutoGenerateColumns="False">
                                                <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                                                <PagerStyle CssClass="grd_page_style" />
                                                <HeaderStyle CssClass="grd_header_style" />
                                                <RowStyle CssClass="grd_row_style" />
                                                <SelectedRowStyle CssClass="grd_selrow_style" />
                                                <EditRowStyle CssClass="grd_editrow_style" />
                                                <AlternatingRowStyle CssClass="grd_altrow_style" />
                                                <FooterStyle CssClass="grd_footer_style" />
                                                <Columns>
                                                    <%--  
                                 <asp:TemplateField> 
                                     <ItemTemplate>
                                         <asp:CheckBox ID="chkPolicyNo" runat="server" />
                                     </ItemTemplate>
                                 </asp:TemplateField>--%>
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_POL_NO" HeaderText="Proposal No." />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_CUST_NAME" HeaderText="Customer Name">
                                                     <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_DEP_SLP_CARD_NO" HeaderText="Dep. Slip/ Card No" />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_PAYMENT" HeaderText="Payment" />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_AMOUNT" HeaderText="Amount" />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_PAY_MODE" HeaderText="Mode of Payt." />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_BANK" HeaderText="Bank">
                                                        <ItemStyle Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_COLL_ACCT" HeaderText="Coll. Acct.">
                                                        <ItemStyle Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_NARRATION" HeaderText="Narration" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <%--<table class="tbl_menu_new">
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
                                            <asp:TextBox ID="txtMainAcctDebitDesc" runat="server" Width="300px" 
                                               Enabled="true" BorderStyle="None"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubAcctDebitDesc" runat="server" Width="300px" 
                                                BorderStyle="None"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAssuredName" runat="server" Width="270px" 
                                                BorderStyle="None"></asp:TextBox>
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
                                            <asp:TextBox ID="txtMainAcctCreditDesc" runat="server" Width="300px" 
                                                BorderStyle="None"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubAcctCreditDesc" runat="server" Width="300px" 
                                                BorderStyle="None"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgentName" runat="server" Width="270px" Enabled="false" BorderStyle="None"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>--%>
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
