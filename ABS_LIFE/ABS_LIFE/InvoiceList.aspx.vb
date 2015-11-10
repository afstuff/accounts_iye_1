Imports CustodianLife.Data
Imports CustodianLife.Model
Imports CustodianLife.Repositories
Partial Public Class InvoiceList
    Inherits System.Web.UI.Page
    Dim inRepo As InvoiceRepository

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            inRepo = New InvoiceRepository

            Session("chRepo") = inRepo

        Else 'post back
            inRepo = CType(Session("chRepo"), InvoiceRepository)

        End If
    End Sub

    Private Sub butNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles butNew.Click
        Response.Redirect("PRG_FIN_CREDITORS_ENTRY.aspx")
    End Sub

    Private Sub butClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles butClose.Click
        Response.Redirect("EntryMenu1.aspx")
    End Sub
End Class