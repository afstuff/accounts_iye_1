Imports CustodianLife.Data
Imports CustodianLife.Model
Partial Public Class RPT_FIN_UNCOMP_RECEIPTS_REC
    Inherits System.Web.UI.Page
    Dim rParams As String() = {"nw", "nw", "new", "new", "new", "new"}
    Dim rcRepo As ReceiptsRepository

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub butOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles butOK.Click
        Dim str() As String
        '  Dim reportname As String
        If (txtStartBatchNo.Text = "") Then
            Status.Text = "Start batch number must not be empty"
            Exit Sub
        End If
        If (txtEndBatchNo.Text = "") Then
            Status.Text = "End batch number must not be empty"
            Exit Sub
        End If

        Dim startBatchNo = Convert.ToInt32(txtStartBatchNo.Text)
        Dim endBatchNo = Convert.ToInt32(txtEndBatchNo.Text)

        If startBatchNo > endBatchNo Then
            Status.Text = "Start batch number must not be greater than end batch number"
            Exit Sub
        End If
        rcRepo = New ReceiptsRepository()
        rcRepo.GetUnCompletedReceiptRecords(startBatchNo, endBatchNo)

        Dim url As String = HttpContext.Current.Request.Url.AbsoluteUri
        rParams(0) = "rptGetUncompReceiptsRecs"
        rParams(1) = "pStart_BatchNo="
        rParams(2) = startBatchNo.ToString() + "&"
        rParams(3) = "pEnd_BatchNo="
        rParams(4) = endBatchNo.ToString() + "&"
        rParams(5) = url

        Session("ReportParams") = rParams
        Response.Redirect("PrintView.aspx")
        ' Response.Redirect("PrintView.aspx")
    End Sub
End Class