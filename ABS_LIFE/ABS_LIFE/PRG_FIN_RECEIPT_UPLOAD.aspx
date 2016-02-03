<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PRG_FIN_RECEIPT_UPLOAD.aspx.vb"
    Inherits="ABS_LIFE.PRG_FIN_RECEIPT_UPLOAD" %>

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

    <title>Upload Receipts</title>
    <style type="text/css">
        .NumberAlign
        {
            text-align: right !important;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function Func_File_Change() {
            var c = 0;
            var cx = 0
            var strfile = "";

            strfile = document.getElementById("My_File_Upload").value;
            // strfile = document.getElementById("My_File_Upload").PostedFile.FileName;
            for (c = 0; c < strfile.length; c++) {
                if (strfile.substring(c, 1) == "") {
                }
                else {
                    cx = cx + 1;
                }
            }

            if (cx <= 0) {
                document.getElementById("txtFile_Upload").style.display = "none";
                document.getElementById("txtFile_Upload").style.visibility = "hidden";
                document.getElementById("cmdFile_Upload").disabled = true;
                alert("Missing or Invalid document name...");
                return false;
            }
            else {
                document.getElementById("txtFile_Upload").style.display = "";
                document.getElementById("txtFile_Upload").style.visibility = "visible";
                document.getElementById("txtFile_Upload").value = strfile;
                // document.getElementById("txtFile_Upload").innerHTML = strfile;
                document.getElementById("cmdFile_Upload").disabled = false;
                // 
                return true;
            }
        }
    </script>

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
    <form id="PRG_FIN_RECEIPT_UPLOAD" runat="server" submitdisabledcontrols="true">
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
            <div class="grid" style="width:950px !important;">
                <div class="rounded">
                    <div class="top-outer">
                        <div class="top-inner">
                            <div class="top">
                                <h2>
                                    Upload&nbsp; Reciepts</h2>
                            </div>
                        </div>
                    </div>
                    <div class="mid-outer">
                        <div class="mid-inner">
                            <div class="mid">
                                <table class="tbl_menu_new">
                                    <tr>
                                        <td colspan="4" class="myMenu_Title" align="center">
                                            Upload&nbsp; Reciepts
                                        </td>
                                    </tr>
                                    <tr id="SB_CONT" runat="server" style="display: none;">
                                        <td align="center" colspan="4" valign="top" style="border-style: ridge;">
                                            <div id="SB_DIV" runat="server" align="center" style="background-color: White; color: Black;
                                                font-size: 23px; font-weight: normal;">
                                                &nbsp;<label id="SB_MSG" runat="server"></label>&nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            <asp:Label ID="lblBatchNo" runat="server" Text="Batch Date:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBatchNo" runat="server" Width="88px"></asp:TextBox>
                                            &nbsp;(yyyymm)
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
                                            <asp:CheckBox ID="chkData_Source" runat="server" Text="-" />
                                            &nbsp;<asp:Label ID="lblData_Source" runat="server" Text="Data Source:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboData_Source" runat="server" AutoPostBack="true" OnTextChanged="DoProc_Data_Source_Change"
                                                Width="250px">
                                                <asp:ListItem Selected="True" Value="*">(Select item)</asp:ListItem>
                                                <asp:ListItem Value="U">Upload Data From Excel Document</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtData_Source_SW" runat="server" Visible="false" Width="40"></asp:TextBox>
                                            <asp:TextBox ID="txtData_Source_Name" runat="server" Enabled="false" Visible="false"
                                                Width="40"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            &nbsp;
                                        </td>
                                        <td class="style17">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style13">
                                            <asp:Label ID="lbl_File_Upload0" runat="server" Text="Select Document:"></asp:Label>
                                        </td>
                                        <td class="style4" colspan="3">
                                            <input id="My_File_Upload" runat="server" name="My_File_Upload" onchange="Func_File_Change()"
                                                onclick="return My_File_Upload_onclick()" type="file" /><asp:TextBox ID="txtFile_Upload"
                                                    runat="server" Enabled="false" Visible="true"></asp:TextBox>
                                            <asp:Button ID="cmdFile_Upload" runat="server" Enabled="false" Font-Bold="true" Font-Size="Large"
                                                Text="Upload" />
                                            <asp:Label ID="lblFile_Upload_Warning" runat="server" ForeColor="Red" Text="Excel File of .XLS or .XLSX"
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="background-color: Maroon; color: White;">
                                        <td class="style6">
                                            <asp:Label ID="lblXLS_Data_Start_No" runat="server" Text="Start Excel No"></asp:Label>
                                            <asp:TextBox ID="txtXLS_Data_Start_No" runat="server" Width="60px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblXLS_Data_End_No" runat="server" Text="End Excel No"></asp:Label>
                                            <asp:TextBox ID="txtXLS_Data_End_No" runat="server" Width="60px"></asp:TextBox>
                                        </td>
                                        <td class="style16">
                                            &nbsp;
                                        </td>
                                        <td class="style17">
                                            <asp:Label ID="lblXLS_Data_Remarks" runat="server" Font-Bold="true" Text="Applies to Upload option"></asp:Label>
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
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" colspan="4">
                                            <asp:GridView ID="grdUpldRecords" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                GridLines="Both" ShowFooter="True">
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
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_POL_NO" HeaderText="Proposal No" ControlStyle-Font-Size="12" />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_CUST_NAME" HeaderText="Customer Name" />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_DEP_SLP_CARD_NO" HeaderText="Dep. Slip/ Card No" />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_PAYMENT" HeaderText="Payment" />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_AMOUNT" HeaderText="Amount" DataFormatString="{0:n}"
                                                        HeaderStyle-CssClass="NumberAlign" ItemStyle-CssClass="NumberAlign" />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_PAY_MODE" HeaderText="Mode of Payt." />
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_BANK" HeaderText="Bank">
                                                        <ItemStyle Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_COLL_ACCT" HeaderText="Coll. Acct.">
                                                        <ItemStyle Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TBFN_RECPT_DNLD_NARRATION" HeaderText="Narration">
                                                      <ItemStyle Width="150px" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblResult" runat="server" Text="Result:"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <%--  
                                 <asp:TemplateField> 
                                     <ItemTemplate>
                                         <asp:CheckBox ID="chkPolicyNo" runat="server" />
                                     </ItemTemplate>
                                 </asp:TemplateField>--%>
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
