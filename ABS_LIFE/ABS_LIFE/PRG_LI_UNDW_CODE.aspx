﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PRG_LI_UNDW_CODE.aspx.vb" Inherits="ABS_LIFE.PRG_LI_UNDW_CODE" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<script language="JavaScript" src="calendar_eu.js" type="text/javascript"></script>
	<link rel="stylesheet" href="calendar.css" />
    <link href="css/general.css" rel="stylesheet" type="text/css" />   
    <link href="css/grid.css" rel="stylesheet" type="text/css" />   
    <link href="css/rounded.css" rel="stylesheet" type="text/css" />   
    <title></title>
    
    
</head>
<body>
 <asp:Label runat=server ID=lblError Font-Bold=true ForeColor=Red Visible=false> </asp:Label>
    <form id="frmIndLifeCodes" runat="server">
<div>
     <div class="grid">
            <div class="rounded">
                <div class="top-outer"><div class="top-inner"><div class="top">
                    <h2>The Individual Life Codes</h2>
                </div></div></div>
                <div class="mid-outer"><div class="mid-inner">
                <div class="mid">     
                	
			<table>
				<tr>
					<td>Code ID</td>
					<td><asp:Dropdownlist ID="cmbCodeID" runat="server" Width="270px">
				<asp:ListItem Selected="True" Value="000">Code ID</asp:ListItem>
	            <asp:ListItem value="L01">Underwriting Codes</asp:ListItem>
	            <asp:ListItem value="L02">Other Codes</asp:ListItem>
				</asp:Dropdownlist></td>
				</tr>
				
				<tr>
					<td>Code Type</td>
						<td><asp:Dropdownlist ID="cmbCodeType" runat="server" Width="270px">
					<asp:ListItem Selected="True" Value="000">Code Types</asp:ListItem>
    	            <asp:ListItem value="001">Disabililty Types</asp:ListItem>
	                <asp:ListItem value="002">Health Status</asp:ListItem>
	                <asp:ListItem value="003">Medical Exam </asp:ListItem>
	                <asp:ListItem value="004">Loading</asp:ListItem>
	                <asp:ListItem value="005">Discount</asp:ListItem>
	                <asp:ListItem value="006">Mortality</asp:ListItem>
	                <asp:ListItem value="007">Medical Illness</asp:ListItem>
	                <asp:ListItem value="008">Medical Clinic</asp:ListItem>
	                <asp:ListItem value="009">Policy Condition</asp:ListItem>
	                <asp:ListItem value="010">Loss Type</asp:ListItem>
	                <asp:ListItem value="011">Treaty Type</asp:ListItem>
	                <asp:ListItem value="012">Loan Codes</asp:ListItem>
	                </asp:Dropdownlist></td>
				</tr>
				<tr>
					<td>Code Item</td>
					<td><asp:TextBox ID="txtCodeItem" runat="server" Width="270px"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Long Desc</td>
					<td><asp:TextBox ID="txtCodeLongDesc" runat="server" Width="270px" TextMode=MultiLine></asp:TextBox></td>
				</tr>
				<tr>
					<td>Short Desc</td>
					<td>
                        <asp:TextBox ID="txtCodeShortDesc" runat="server" Width="270px"></asp:TextBox>
                    </td>
				</tr>
				<tr>
					<td></td><td>
                        <asp:Button ID="butSave" runat="server" Text="Save" onclick="butSave_Click" />
                        <asp:Button ID="butDelete" runat="server" Text="Delete"  /></td>
				</tr>
			</table>
			
			                      </div></div></div>
            <div class="bottom-outer"><div class="bottom-inner">
            <div class="bottom"></div></div></div>                
        </div>      
    </div>
</div>

<div>
      <div class="grid">
            <div class="rounded">
                <div class="top-outer"><div class="top-inner"><div class="top">
                    <h2>The Individual Life Codes</h2>
                </div></div></div>
                <div class="mid-outer"><div class="mid-inner">
                <div class="mid">     
<!-- grid end here-->       
<asp:GridView ID="grdData"  runat="server"  AutoGenerateColumns="False"
        DataSourceID="ods1" AllowSorting="True" CssClass="datatable"
        CellPadding="0" BorderWidth="0px" AlternatingRowStyle-BackColor="#CDE4F1" GridLines="None" HeaderStyle-BackColor="#099cc" ShowFooter="True" >
        <PagerStyle CssClass="pager-row" />
           <RowStyle CssClass="row" />
              <PagerSettings Mode="NumericFirstLast" PageButtonCount="7" FirstPageText="«" LastPageText="»" />      
            <Columns>
  
 
   <asp:HyperLinkField DataTextField="CodeTabId" DataNavigateUrlFields="icId,CodeTabId,CodeType"
         DataNavigateUrlFormatString="~/PRG_LI_UNDW_CODE.aspx?idd={0},{1},{2}" HeaderText="TabId"  
         HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  />
         
                <asp:BoundField DataField="CodeType" HeaderText="Code Type" />
                <asp:BoundField DataField="CodeItem" HeaderText="Code Item" />
                <asp:BoundField DataField="CodeLongDesc" HeaderText="Long Descr" />
                <asp:BoundField DataField="CodeShortDesc" HeaderText="Short Descr" />
            </Columns>
            
        <HeaderStyle HorizontalAlign="Justify" VerticalAlign="Top" />
                <AlternatingRowStyle BackColor="#CDE4F1" />
        </asp:GridView>
                       </div></div></div>
            <div class="bottom-outer"><div class="bottom-inner">
            <div class="bottom"></div></div></div>                
        </div>      
    </div>
         <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="IndividualLifeCodes" TypeName="CustodianLife.Data.IndLifeCodesRepository">
    </asp:ObjectDataSource>  
    </div>
		</form>
</body>
</html>
