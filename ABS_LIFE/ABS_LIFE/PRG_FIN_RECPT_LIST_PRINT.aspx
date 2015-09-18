<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PRG_FIN_RECPT_LIST_PRINT.aspx.vb" Inherits="ABS_LIFE.PRG_FIN_RECPT_LIST_PRINT" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link rel="Stylesheet" href="SS_ILIFE.css" type="text/css" />
	<link rel="stylesheet" href="calendar.css" />
    <link href="css/general.css" rel="stylesheet" type="text/css" />   
    <link href="css/grid.css" rel="stylesheet" type="text/css" />   
    <link href="css/rounded.css" rel="stylesheet" type="text/css" />   
    <script src="jquery-1.11.0.js" type="text/javascript"></script>
    <script src="jquery.simplemodal.js" type="text/javascript"></script>
    <script language="JavaScript" src="calendar_eu.js" type="text/javascript"></script>
    <title></title>
    
    
</head>
<body>
    <form id="LifeReceiptsPrint" runat="server">
    <div>
    </div>
    <div  class="newpage">
    <table>
    <tr>
    <td>
        <asp:Literal runat="server" Visible="false" ID="litMsgs"></asp:Literal>
        <asp:Label runat="server" ID="Status" Font-Bold="true" ForeColor="Red" Visible="true" Text="Status:"> </asp:Label>
        <asp:Label runat="server" ID="Label1" Font-Bold="true" ForeColor="Red" Visible="false"> </asp:Label>
    </td></tr>
    </table>

     <div class="grid">
            <div class="rounded">
                <div class="top-outer"><div class="top-inner"><div class="top">
                    <h2>PRINT: Individual Life Receipts List</h2>
                </div></div></div>
                <div class="mid-outer"><div class="mid-inner">
                <div class="mid">     
                	

                <table class="tbl_menu_new">
			        <tr><td colspan="2" class="myMenu_Title" align="center"><asp:Label ID="lblDesc1" runat="server" Text="Receipt Print"> </asp:Label> </td><td></td><td></td><td></td></tr>
				    <tr>
					    <td>Start Date</td>
					    <td><asp:TextBox ID="txtStartDate" runat="server" Width="150px" MaxLength=10 ></asp:TextBox>
					    					<script language="JavaScript" type="text/javascript">
					    					    new tcal({ 'formname': 'LifeReceiptsPrint', 'controlname': 'txtStartDate' });</script>dd/mm/yyyy</td>
				    </tr>
    				
				    <tr>
					    <td>End Date</td>
					    <td><asp:TextBox ID="txtEndDate" runat="server" Width="150px" MaxLength=10 ></asp:TextBox>
					    					    					<script language="JavaScript" type="text/javascript">
					    					    					    new tcal({ 'formname': 'LifeReceiptsPrint', 'controlname': 'txtEndDate' });</script>dd/mm/yyyy</td>
				    </tr>
				    <tr>
					    <td>Report Type</td>
					    <td><asp:RadioButtonList ID=rblTransType runat=server >
                <asp:ListItem Text="Individual Life" Value="IndReceiptList"></asp:ListItem>
                <asp:ListItem Text="Other Receipts" Value="rptOtherReceiptList"></asp:ListItem>
                <asp:ListItem Text="Payments" Value="rptOtherPaymentsList"></asp:ListItem>
                </asp:RadioButtonList></td>
				    </tr>

				<tr>
					<td></td>
					<td>
                        <asp:Button ID="butOK" runat="server" Text="OK"/>
                        <asp:Button ID="butClose" runat="server" Text="Close" />
                     </td>
				</tr>

			    </table>
			     </div></div></div>
            <div class="bottom-outer"><div class="bottom-inner">
            <div class="bottom"></div></div></div>                
        </div>      
    </div>

</div>
    </form>
</body>
</html>
