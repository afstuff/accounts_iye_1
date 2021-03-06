﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PRG_FIN_ACC_GRP_SETUP.aspx.vb" Inherits="ABS_LIFE.PRG_FIN_ACC_GRP_SETUP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
    
    
</head>
<body>
   
    <form id="PRG_FIN_ACC_GRP_SETUP" runat="server">
 <div class="newpage">

    <div>
    <table>
        <tr>
        <td>
        
            <asp:Literal runat="server" Visible="false" ID="litMsgs"></asp:Literal>
        <asp:Label runat="server" ID="Status" Font-Bold="true" ForeColor="Red" Visible="true" Text="Status:"> </asp:Label>
         <asp:Label runat="server" ID="Label1" Font-Bold="true" ForeColor="Red" Visible="false"> </asp:Label>
          <asp:Label runat="server" ID="lblError" Font-Bold="true" ForeColor="Red" Visible="false"> </asp:Label>

        </td>
        </tr>
    </table>

         <div class="gridp">
                <div class="rounded">
                    <div class="top-outer"><div class="top-inner"><div class="top">
                        <h2>Chart of Accounts Grouping Codes</h2>
                    </div></div></div>
                    <div class="mid-outer"><div class="mid-inner">
                    <div class="mid">     
                    	
			    <table class="tbl_menu_new">
		           <tr><td colspan="4" class="myMenu_Title" align="center">Chart of Accounts Grouping</td><td></td><td></td><td></td></tr>

				    <tr>
					    <td>Company Code</td>
                        <td><asp:Dropdownlist ID="cmbCoyCode" runat="server" Width="270px">
        				    </asp:Dropdownlist></td>
				    </tr>
    				
				    <tr>
					    <td>Account Group Code</td>
					    <td><asp:TextBox ID="txtGroupCode" runat="server" Width="270px" ></asp:TextBox></td>
				    </tr>
				    <tr>
					    <td>Group Long Desc</td>
					    <td><asp:TextBox ID="txtLongDesc" runat="server" Width="270px"></asp:TextBox></td>
				    </tr>
				    <tr>
					    <td>Group Short Desc</td>
					    <td><asp:TextBox ID="txtShortDesc" runat="server" Width="270px"></asp:TextBox></td>
				    </tr>
				    <tr>
					    <td>Main Group Code</td>
					    <td><asp:TextBox ID="txtMainGroupCode" runat="server" Width="270px" ></asp:TextBox></td>
				    </tr>
				    <tr>
					    <td>Main Group Code Desc</td>
					    <td><asp:TextBox ID="txtMainGroupDesc" runat="server" Width="270px" ></asp:TextBox></td>
				    </tr>
				    <tr>
					    <td>Ledger Type</td>
					    <td>
                            <asp:DropDownList ID="cmbLedgerType" runat="server" Width = "150px">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="K">Bank</asp:ListItem>
                                    <asp:ListItem Value="L">Liability</asp:ListItem>
                                    <asp:ListItem Value="E">Expenses</asp:ListItem>
                                    <asp:ListItem Value="C">Capital</asp:ListItem>
                                    <asp:ListItem Value="T">Debtors</asp:ListItem>
                                    <asp:ListItem Value="M">Claims</asp:ListItem>
                                    <asp:ListItem Value="F">Fixed Assets</asp:ListItem>
                                    <asp:ListItem Value="G">General Ledger</asp:ListItem>
                                    <asp:ListItem Value="I">Investment</asp:ListItem>
                                    <asp:ListItem Value="S">Stock</asp:ListItem>
                                    <asp:ListItem Value="D">Staff Debtors</asp:ListItem>
                                    <asp:ListItem Value="R">Creditors</asp:ListItem>
                                    <asp:ListItem Value="L">Loans</asp:ListItem>
                                    <asp:ListItem Value="C">Commissions</asp:ListItem>
                                    <asp:ListItem Value="X">Unexpired Risks</asp:ListItem>
                                    <asp:ListItem Value="Y">Claims Outstanding</asp:ListItem>
                                    <asp:ListItem Value="Z">Contigency Reserve</asp:ListItem>
                                </asp:DropDownList>
					    </td>
				    </tr>

				    <tr>
					    <td></td><td>
                            <asp:Button ID="butSave" runat="server" Text="Save" onclick="butSave_Click" Visible=false />
                            <asp:Button ID="butSaveN" runat="server" Text="Save" OnClientClick="JavaSave_Rtn()"  />
                            <asp:Button ID="butDelete" runat="server" Text="Delete"  />
                            <asp:Button ID="butClose" runat="server" Text="Close" Visible="True" /></td>
                            
				    </tr>
			    </table>
    			
			                          </div></div></div>
                <div class="bottom-outer"><div class="bottom-inner">
                <div class="bottom"></div></div></div>                
            </div>      
        </div>
    </div>

    <div>
          <div class="gridp">
                <div class="rounded">
                    <div class="top-outer"><div class="top-inner"><div class="top">
                        <h2>Chart of Accounts Grouping Code Listing </h2>
                    </div></div></div>
                    <div class="mid-outer"><div class="mid-inner">
                    <div class="mid">     
    <!-- grid end here-->       
    <asp:GridView ID="grdData"  runat="server"  AutoGenerateColumns="False"
            DataSourceID="ods1" AllowSorting="True" CssClass="datatable" AllowPaging="True" PageSize=10
            CellPadding="0" BorderWidth="0px" AlternatingRowStyle-BackColor="#CDE4F1" GridLines="None" HeaderStyle-BackColor="#099cc" ShowFooter="True" >
            <PagerStyle CssClass="pager-row" />
               <RowStyle CssClass="row" />
                  <PagerSettings Mode="NumericFirstLast" PageButtonCount="7" FirstPageText="«" LastPageText="»" />      
                <Columns>
                             <asp:HyperLinkField DataTextField="agId" DataNavigateUrlFields="agId,CompanyCode,AccountGroupCode"
                                     DataNavigateUrlFormatString="~/PRG_FIN_ACC_GRP_SETUP.aspx?idd={0},{1},{2}" HeaderText="Id"  
                                     HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  />
                                     
                             <asp:HyperLinkField DataTextField="CompanyCode" DataNavigateUrlFields="agId,CompanyCode,AccountGroupCode"
                                     DataNavigateUrlFormatString="~/PRG_FIN_ACC_GRP_SETUP.aspx?idd={0},{1},{2}" HeaderText="Coy Code"   
                                     HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  />

                             <asp:HyperLinkField DataTextField="AccountGroupCode" DataNavigateUrlFields="agId,CompanyCode,AccountGroupCode"
                                     DataNavigateUrlFormatString="~/PRG_FIN_ACC_GRP_SETUP.aspx?idd={0},{1},{2}" HeaderText="Group Code"   
                                     HeaderStyle-CssClass="first" ItemStyle-CssClass="first"  />

                    <asp:BoundField DataField="AccountGroupLongDesc" HeaderText="Long Desc" />
                    <asp:BoundField DataField="AccountGroupShortDesc" HeaderText="Short Desc" />
                   
                </Columns>
                
            <HeaderStyle HorizontalAlign="Justify" VerticalAlign="Top" />
                    <AlternatingRowStyle BackColor="#CDE4F1" />
            </asp:GridView>
                           </div></div></div>
                <div class="bottom-outer"><div class="bottom-inner">
                <div class="bottom"></div></div></div>                
            </div>      
        </div>
             <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="AccountGroupDetails" TypeName="CustodianLife.Data.AccountGroupsRepository">
        </asp:ObjectDataSource>  
        </div>
 </div>
  </form>
</body>
</html>
