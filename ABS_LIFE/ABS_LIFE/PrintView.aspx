<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintView.aspx.vb" Inherits="ABS_LIFE.PrintView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="css/general.css" rel="stylesheet" type="text/css" />   
    <link href="css/grid.css" rel="stylesheet" type="text/css" />   
    <link href="css/rounded.css" rel="stylesheet" type="text/css" />   

    <title></title>
</head>
<body onload=<%=FirstMsg%> >
    <form id="form1" runat="server">
     <div  class=newpage1 >
        <div class="grid">
                 <div class="rounded">
                    <div class="top-outer"><div class="top-inner"><div class="top">
                        <h2><asp:Label ID="lblDesc2" runat="server" Text="Print Services"></asp:Label>  </h2>
                    </div></div></div>
                    <div class="mid-outer"><div class="mid-inner">
                    <div class="mid">     
                    	
    <!-- grid end here-->

        <div>
                    <asp:Button ID="butView" Text="View/Print" runat="server" />
                    <asp:Button ID="butClsoe" Text="Close" runat="server" />
        </div>
        <div id="PrintDialog">
            
            <iframe id="frReport" scrolling="auto" width="1000px" height="500px" 
            src=<%=ReportURL%> > </iframe>

        </div>
    <div>    
    </div>
                           </div></div></div>
                <div class="bottom-outer"><div class="bottom-inner">
                <div class="bottom"></div></div></div>                
            </div>      
        </div>
    </div>
    
    
    
    


    </form>
</body></html>
