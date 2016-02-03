Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports CustodianLife.Data
Imports CustodianLife.Model
Imports CustodianLife.Repositories
Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO
Partial Public Class PRG_FIN_RECEPT_MATCH
    Inherits System.Web.UI.Page
    Protected STRMENU_TITLE As String
    Protected strStatus As String
    Dim rcMthRepo As ReceiptsMatchRepository
    Dim polinfo As PolicyInfo
    Dim updateFlag As Boolean
    Dim strKey As String
    Dim strSchKey As String
    Dim newReceiptNo As String
    Dim newSerialNum As String
    Dim Rceipt As CustodianLife.Model.Receipts
    Protected publicMsgs As String = String.Empty
    Dim Err As String
    Dim lstErrMsgs As IList(Of String)
    Dim lstUnMthProposalNo As IList(Of String)
    Dim ReadRecords, MatchRecords, UnMatchRecords As Integer


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub cmdMatch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdMatch.Click
        lblError.Text = ""
        If txtBatchNo.Text = "" Then
            lblError.Text = "Missing Batch Date"
            publicMsgs = "javascript:alert('" + lblError.Text + "')"
            Exit Sub
        End If
        ReadRecords = MatchRecords = UnMatchRecords = 0
        rcMthRepo = New ReceiptsMatchRepository()
        'rcMthRepo.GetPolicyInfo(txtBatchNo.Text, lstErrMsgs, lstUnMthProposalNo)
        rcMthRepo.GetPolicyInfo(txtBatchNo.Text, lstErrMsgs, ReadRecords, MatchRecords, UnMatchRecords)
        UnMatchRecords = ReadRecords - MatchRecords
        ' GoTo MyLoop_999a

        'MyLoop_999a:

        'If lstErrMsgs.Count > 1 Then
        '    For i = 0 To lstErrMsgs.Count - 1
        '        cboErr_List.Items.Add(lstErrMsgs.Item(i))
        '    Next

        '    Me.lblErr_List.Visible = True
        '    Me.cboErr_List.Visible = True

        '    lblError.Text = "Read Records: '" & ReadRecords & "', Match Records: '" & MatchRecords & "', UnMatch Records: '" & UnMatchRecords & "'"
        '    publicMsgs = "Javascript:alert('" & RTrim("Not Record saved to receipt file successfully - ") & "')"
        'Else
        '    lblError.Text = "Read Records: '" & ReadRecords & "', Match Records: '" & MatchRecords & "', UnMatch Records: '" & UnMatchRecords & "'"
        '    publicMsgs = "Javascript:alert('" & RTrim("Record saved to receipt file successfully - ") & "')"
        'End If

        If MatchRecords > 0 Then
            If lstErrMsgs.Count > 1 Then
                For i = 0 To lstErrMsgs.Count - 1
                    cboErr_List.Items.Add(lstErrMsgs.Item(i))
                Next
                Me.lblErr_List.Visible = True
                Me.cboErr_List.Visible = True
                lblError.Text = "Read Records: " & ReadRecords & ", Match Records: " & MatchRecords & ", UnMatch Records: " & UnMatchRecords & ""
                publicMsgs = "Javascript:alert('" & RTrim("Some proposal number matches but error encountered - ") & "')"
            Else

                If UnMatchRecords > 0 Then
                    lblError.Text = "Read Records: " & ReadRecords & ", Match Records: " & MatchRecords & ", UnMatch Records: " & UnMatchRecords & ""
                    publicMsgs = "Javascript:alert('" & RTrim("Some proposal number matches but Unmatch Proposal number encountered - ") & "')"
                Else

                    lblError.Text = "Read Records: " & ReadRecords & ", Match Records: " & MatchRecords & ", UnMatch Records: " & UnMatchRecords & ""
                    publicMsgs = "Javascript:alert('" & RTrim("All Proposal number matches and  saved to receipt file successfully - ") & "')"
                End If
            End If
        Else
            lblError.Text = "Read Records: " & ReadRecords & ", Match Records: " & MatchRecords & ", UnMatch Records: " & UnMatchRecords & ""
            publicMsgs = "Javascript:alert('" & RTrim("No Proposal number matches- ") & "')"
        End If
        'If lstUnMthProposalNo.Count > 1 Then
        '    For i = 0 To lstUnMthProposalNo.Count - 1
        '        grdUnMthPropNos.Rows(i).Cells(i).Text = lstUnMthProposalNo.Item(i)
        '    Next
        '    grdUnMthPropNos.DataBind()
        'End If

        GoTo MyLoop_End

MyLoop_End:
    End Sub

    Protected Sub cmdUnMatch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUnMatch.Click
        lblError.Text = ""
        If txtBatchNo.Text <> "" Then
            UnMatchProposalNumbers()
        Else
            lblError.Text = "Missing or Invalid Batch Date"
            lblError.Visible = True
            txtBatchNo.Focus()
            publicMsgs = "javascript:alert('" + lblError.Text + "')"
            Exit Sub
        End If
    End Sub

    Private Sub grdUnMthPropNos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUnMthPropNos.PageIndexChanging
        grdUnMthPropNos.PageIndex = e.NewPageIndex
        UnMatchProposalNumbers()
    End Sub

    Private Sub UnMatchProposalNumbers()
        rcMthRepo = New ReceiptsMatchRepository()
        grdUnMthPropNos.DataSource = rcMthRepo.UnMatchProposalNumbers(Trim(txtBatchNo.Text))
        grdUnMthPropNos.DataBind()
    End Sub
End Class