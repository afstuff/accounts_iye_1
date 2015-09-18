Public Partial Class PrintView
    Inherits System.Web.UI.Page
    Dim strKey As String
    Protected FirstMsg As String
    Protected ReportURL As String
    Dim rParams As String()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            rParams = CType(Session("ReportParams"), String())
            ReportURL = "img/ABS_Print_Canvas_02.jpg"

        Else
            rParams = CType(Session("ReportParams"), String())

        End If

    End Sub

    Protected Sub butView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles butView.Click
        Dim myParams As String = String.Empty
        For x As Integer = 1 To rParams.Length() - 1
            myParams = myParams + rParams(x)
        Next
        Try

            ReportURL = "http://192.168.10.73:88/ABSReportSvr?/ABSLIFEReports/" + rParams(0).ToString + "&rs:Command=Render&" + myParams + "rc:LinkTarget=main&rc:Parameters=False target=main','','left=50,top=10,width=1024,height=600,titlebar=yes,z-lock=yes,address=yes,channelmode=1,fullscreen=0,directories=yes,location=yes,toolbar=yes,menubar=yes,status=yes,scrollbars=1,resizable=yes"
            '    'FirstMsg = "javascript:window.close();" & mystrURL
            'FirstMsg = "javascript:" & mystrURL
        Catch ex As Exception
            ' Me.lblMsg.Text = "<br />Unable to connect to report viewer. <br />Reason: " & ex.Message.ToString

        End Try
    End Sub
    Protected Sub analyseParams()


        '        myParams = Replace(myParams, "=", "", 1, , CompareMethod.Text)


    End Sub

    Protected Sub butClsoe_Click(ByVal sender As Object, ByVal e As EventArgs) Handles butClsoe.Click
        Response.Redirect("PRG_FIN_RECPT_LIST_PRINT.aspx")
    End Sub
End Class