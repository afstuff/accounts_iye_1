<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InvoiceList.aspx.vb" Inherits="ABS_LIFE.InvoiceList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<script language="JavaScript" src="calendar_eu.js" type="text/javascript"></script>
	<link rel="stylesheet" href="calendar.css" />
    <link href="css/general.css" rel="stylesheet" type="text/css" />   
    <link href="css/grid.css" rel="stylesheet" type="text/css" />   
    <link href="css/rounded.css" rel="stylesheet" type="text/css" />  
    <script src="jquery-1.11.0.js" type="text/javascript"></script>

    <script type="text/javascript">
        
    // calling jquery functions once document is ready
        $(document).ready(function() {
            $("#dtChoose").hide();

            $("#cmbSearch").on('focusout', function(e) {
                e.preventDefault()
                if ($("#cmbSearch").val() == "TDate") {
                    $("#dtChoose").show();
                }
                else {
                    $("#dtChoose").hide();
                }
                return false;
            });

        });
    </script>
    <style type="text/css">
    .NumberAlign
    {
    	text-align:right!important;
    }
    </style>
    
    <title>Data Entries Listing</title>
</head>
<body>
    <form id="frmReceiptsList" runat="server">
   <div  class="newpage">
      <div class="gridp">
            <div class="rounded">
                <div class="top-outer"><div class="top-inner"><div class="top">
                    <h2>Data Entries Listing</h2>
                </div></div></div>
                <div class="mid-outer"><div class="mid-inner">
                <div class="mid">     
<!-- grid end here-->
<div id="fram">
    <table class="datatable"><tr><td colspan="4">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </td></tr>
        <tr><td>Search</td><td><asp:DropDownList ID="cmbSearch" runat="server">
    <asp:ListItem Value="0">Select</asp:ListItem>
    <asp:ListItem Value="All">All</asp:ListItem>
    <asp:ListItem Value="BatchNo">Batch No</asp:ListItem>
    <asp:ListItem Value="BatchDate">Batch Date</asp:ListItem>
    <asp:ListItem Value="TDate">Trans Date</asp:ListItem>
    <asp:ListItem Value="RefNo">Ref No</asp:ListItem>
        <asp:ListItem Value="PayerName">Payer Name</asp:ListItem>
    </asp:DropDownList>
    </td><td><asp:TextBox ID="txtSearch" runat="server"> </asp:TextBox><span id="dtChoose">                        
    <script language="JavaScript" type="text/javascript">
        new tcal({ 'formname': 'frmReceiptsList', 'controlname': 'txtSearch' });</script> </span><asp:Button ID="butGO" runat="server" Text="Go" Width="35px" Height="35px" /></td><td> 
        <asp:Button
            runat="server" ID="butNew" Text="New" Height="35px"/><asp:Button runat="server" ID="butClose" Text="Close" Height="35px"/>
        </td></tr>
    </table>
    
</div>
<asp:GridView ID="grdData"  runat="server"  AutoGenerateColumns="False" 
        DataSourceID="ods1" AllowSorting="True" CssClass="datatable"
        CellPadding="0" BorderWidth="0px" AlternatingRowStyle-BackColor="#CDE4F1" GridLines="None" HeaderStyle-BackColor="#099cc" ShowFooter="True" >
        <PagerStyle CssClass="pager-row" />
           <RowStyle CssClass="row" />
              <PagerSettings Mode="NumericFirstLast" PageButtonCount="7" FirstPageText="«" LastPageText="»" />      
            <Columns>
            
              <asp:HyperLinkField DataTextField="InId" DataNavigateUrlFields="InId,CompanyCode,BatchNo,BatchDate,SerialNo"
         DataNavigateUrlFormatString="~/PRG_FIN_CREDITORS_ENTRY1.aspx?idd={0},{1},{2},{3},{4}" HeaderText="Id" Visible="false"  
         HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  >
         
<HeaderStyle CssClass="first"></HeaderStyle>

<ItemStyle CssClass="first"></ItemStyle>
                </asp:HyperLinkField>

             <%-- <asp:HyperLinkField DataTextField="CompanyCode" DataNavigateUrlFields="InId,CompanyCode,BatchNo,BatchDate,SerialNo,SubSerialNo,TransType"
         DataNavigateUrlFormatString="~/PRG_FIN_CREDITORS_ENTRY1.aspx?idd={0},{1},{2},{3},{4},{5}&prgKey={6}" HeaderText="Coy"  
         HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  >
         
<HeaderStyle CssClass="first"></HeaderStyle>

<ItemStyle CssClass="first"></ItemStyle>
                </asp:HyperLinkField>--%>
         
         
              <asp:HyperLinkField DataTextField="BatchNo" DataNavigateUrlFields="InId,CompanyCode,BatchNo,BatchDate,SerialNo"
         DataNavigateUrlFormatString="~/PRG_FIN_CREDITORS_ENTRY1.aspx?idd={0},{1},{2},{3},{4}" HeaderText="Bat. #"  
         HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  >
         
<HeaderStyle CssClass="first"></HeaderStyle>

<ItemStyle CssClass="first"></ItemStyle>
                </asp:HyperLinkField>
         
              <asp:HyperLinkField DataTextField="BatchDate" DataNavigateUrlFields="InId,CompanyCode,BatchNo,BatchDate,SerialNo"
         DataNavigateUrlFormatString="~/PRG_FIN_CREDITORS_ENTRY1.aspx?idd={0},{1},{2},{3},{4}" HeaderText="Bat. Dt"  
         HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  >


<HeaderStyle CssClass="first"></HeaderStyle>

<ItemStyle CssClass="first"></ItemStyle>
                </asp:HyperLinkField>

              <asp:HyperLinkField DataTextField="SerialNo" DataNavigateUrlFields="InId,CompanyCode,BatchNo,BatchDate,SerialNo"
         DataNavigateUrlFormatString="~/PRG_FIN_CREDITORS_ENTRY1.aspx?idd={0},{1},{2},{3},{4}" HeaderText="SN"  
         HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  >

<HeaderStyle CssClass="first"></HeaderStyle>

<ItemStyle CssClass="first"></ItemStyle>
                </asp:HyperLinkField>

   <%--<asp:HyperLinkField DataTextField="SubSerialNo" DataNavigateUrlFields="InId,CompanyCode,BatchNo,BatchDate,SerialNo,SubSerialNo,TransType"
         DataNavigateUrlFormatString="~/PRG_FIN_CREDITORS_ENTRY1.aspx?idd={0},{1},{2},{3},{4},{5} &prgKey={6}" HeaderText="SSN"  Visible="false" 
         HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  >

<HeaderStyle CssClass="first"></HeaderStyle>

<ItemStyle CssClass="first"></ItemStyle>
                </asp:HyperLinkField>
--%>

                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No"/>
                <asp:BoundField DataField="TransDate" HeaderText="Trans Date" DataFormatString="{0:dd/MM/yy}"/>
                <asp:BoundField DataField="TransDescription" HeaderText="Description"/>
                <asp:BoundField DataField="TransType" HeaderText="Type"/>
                <asp:BoundField DataField="ItemSize" HeaderText="Item Size"/>
                <asp:BoundField DataField="Quantity" HeaderText="Quantity"/>
                <asp:BoundField DataField="Price" HeaderText="Price"/>
                <asp:BoundField DataField="TransAmt" HeaderText="Amount" DataFormatString="{0:n}" HeaderStyle-CssClass="NumberAlign" ItemStyle-CssClass="NumberAlign"/>
            </Columns>
            
        <HeaderStyle HorizontalAlign="Justify" VerticalAlign="Top" />
                <AlternatingRowStyle BackColor="#CDE4F1" />
        </asp:GridView>
       
                       </div></div></div>
            <div class="bottom-outer"><div class="bottom-inner">
            <div class="bottom"></div></div></div>                
        </div>      
    </div>
        <%-- <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetById" 
           TypeName="CustodianLife.Data.GLTransRepository">--%>
           <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetById" 
           TypeName="CustodianLife.Data.InvoiceRepository">
            <SelectParameters>
                     <%--<asp:Parameter DefaultValue="BatchNo" Name="_key" Type="String" />--%>
                     <asp:ControlParameter ControlID="cmbSearch" DefaultValue="" Name="_key" 
                         PropertyName="SelectedValue" Type="String" />
                     <asp:ControlParameter ControlID="txtSearch" DefaultValue="" Name="_value" 
                         PropertyName="Text" Type="String" />
                     <%--<asp:ControlParameter ControlID="txtProgType" DefaultValue="" Name="_prg" 
                         PropertyName="Text" Type="String" />--%>
                 </SelectParameters>
    </asp:ObjectDataSource>  
    </div>
    <asp:TextBox runat="server" ID="txtProgType" CssClass="popupOffset"> </asp:TextBox>   

    </form>
</body>
</html>
