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
Imports System.Globalization
Partial Public Class PRG_FIN_CREDITORS_ENTRY1
    Inherits System.Web.UI.Page

    Dim invRepo As InvoiceRepository
    Dim indLifeEnq As IndLifeCodesRepository
    Dim transTypeEnq As TransactionTypesRepository
    Dim updateFlag As Boolean
    Dim strKey As String
    Dim prgKey As String
    Dim Save_Amount As Decimal = 0
    Dim Cum_Detail_Amount As Decimal = 0
    Dim InvTrans As CustodianLife.Model.Invoice
    Dim TotTransAmt As Decimal = 0
    Dim TransAmt As Decimal = 0
    Dim newDocRefNo As String

    
    Dim swEntry As String = "H"
    Dim detailEdit As String = "N"
    Dim TransType As String
    Protected publicMsgs As String = String.Empty
    Protected ci As CultureInfo

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtSubSerialNo.Attributes.Add("disabled", "disabled")
        txtRefAmt.Attributes.Add("disabled", "disabled")
        txtRefDate.Attributes.Add("disabled", "disabled")
        txtLedgerType.Attributes.Add("disabled", "disabled")

        If Not Page.IsPostBack Then
            'set globalization and culture -- date issues
            'Dim li As ListItem = New ListItem
            'li.Text = "Invoice"
            'li.Value = "INV"
            'cmbTransType.Items.Add(li)

            ci = New CultureInfo("en-GB")
            Session("ci") = ci

            invRepo = New InvoiceRepository
            indLifeEnq = New IndLifeCodesRepository
            transTypeEnq = New TransactionTypesRepository

            Session("invRepo") = invRepo
            updateFlag = False
            Session("updateFlag") = updateFlag
            swEntry = "H"
            Session("swEntry") = swEntry

            strKey = Request.QueryString("idd")
            prgKey = Request.QueryString("prgKey")
            Session("prgKey") = prgKey
            txtProgType.Text = prgKey
            Session("gtId") = strKey
            Session("Save_Amount") = Save_Amount
            Session("Cum_Detail_Amount") = Cum_Detail_Amount
            'Company code value to be filled from login
            txtCompanyCode.Text = "001"
            'txtEntryDate.Text = Now.Date.ToString()
            txtEntryDate.Text = Format(Now, "dd/MM/yyyy")
            txtEntryDate.ReadOnly = True
            lblError.Visible = False
            'If prgKey = "journal" Or prgKey = "payment" Then
            '    txtReceiptNo.Enabled = True
            'End If


            SetComboBinding(cmbBranchCode, indLifeEnq.GetById("L02", "003"), "CodeItem_CodeLongDesc", "CodeItem")
            SetComboBinding(cmbDept, indLifeEnq.GetById("L02", "005"), "CodeItem_CodeLongDesc", "CodeItem")
            SetComboBinding(cmbTransDetailType, transTypeEnq.TransactionTypesDetails, "TransactionCode_Description", "TransactionCode")

            cmbTransType.Attributes.Add("readonly", "readonly")
            txtSubAcct.Text = "000000"

            If strKey IsNot Nothing Then
                fillValues()
            Else
                invRepo = CType(Session("invRepo"), InvoiceRepository)
            End If

        Else 'post back

            Me.Validate()
            If (Not Me.IsValid) Then
                Dim msg As String
                ' Loop through all validation controls to see which 
                ' generated the error(s).
                Dim oValidator As IValidator
                For Each oValidator In Validators
                    If oValidator.IsValid = False Then
                        msg = msg & "\n" & oValidator.ErrorMessage
                    End If
                Next

                lblError.Text = msg
                lblError.Visible = True
                publicMsgs = "javascript:alert('" + msg + "')"
            End If
            ci = CType(Session("ci"), CultureInfo)
        End If

    End Sub

    
    Private Sub SetComboBinding(ByVal toBind As ListControl, ByVal dataSource As Object, ByVal displayMember As String, ByVal valueMember As String)
        toBind.DataTextField = displayMember
        toBind.DataValueField = valueMember
        toBind.DataSource = dataSource
        toBind.DataBind()
        toBind.Items.Insert(0, New ListItem("Select", "NA"))
    End Sub

    Private Sub fillValues()

        strKey = CType(Session("gtId"), String)
        invRepo = CType(Session("invRepo"), InvoiceRepository)
        InvTrans = invRepo.GetById(strKey)

        Session("InvTrans") = InvTrans
        If InvTrans IsNot Nothing Then
            With InvTrans
                txtBatchDate.Text = .BatchDate
                txtBatchNo.Text = .BatchNo
                cmbBranchCode.SelectedValue = .BranchCode
                txtCompanyCode.Text = .CompanyCode
                cmbDept.SelectedValue = .DeptCode
                txtReceiptNo.Text = .InvoiceNo
                txtEntryDate.Text = Format(.EntryDate, "dd/MM/yyyy")
                txtRefDate.Text = Format(.RefDate, "dd/MM/yyyy")
                'txtReceiptRefNo1.Text = .RefNo1
                txtMainAcct.Text = .MainAccountDR
                txtReceiptRefNo1.Text = .CreditorCode
                '.OperatorId = "001"
                ' .PostStatus = "U" 'Unposted
                'txtRemarks.Text = .Remarks
                If .SubAccountDR = "" Then
                    txtSubAcct.Text = "000000"
                Else
                    txtSubAcct.Text = .SubAccountDR
                End If

                txtEffectiveDate.Text = Format(.TransDate, "dd/MM/yyyy")
                txtTransDesc.Text = .TransDescription
                txtSerialNo.Text = .SerialNo
                cmbTransType.SelectedValue = .TransType

                txtTransAmt.Text = Format(.TransAmt, "Standard")
                'txtLedgerType.Text = .LedgerTypeCode
                txtProgType.Text = CType(Session("prgKey"), String)

                txtSubSerialNo.Text = .SubSerialNo
                cmbTransType.SelectedValue = .TransType
                Session("InvTrans") = InvTrans
            End With

            updateFlag = True
            Session("updateFlag") = updateFlag
            detailEdit = "Y"
            Session("detailEdit") = detailEdit
            grdData.DataBind()
        End If
    End Sub
    Protected Sub initializeFields()
        txtBatchDate.Text = String.Empty
        txtBatchNo.Text = String.Empty
        cmbBranchCode.SelectedIndex = 0
        txtPrice.Text = String.Empty
        txtCompanyCode.Text = "001"
        cmbDept.SelectedIndex = 0
        txtTotalAmt.Text = String.Empty
        ' txtReceiptNo.Text = String.Empty
        '.OperatorId = "001"
        '.PostStatus = "U" 'Unposted
        'txtRemarks.Text = String.Empty
        txtEffectiveDate.Text = String.Empty
        txtTransDesc.Text = String.Empty
        txtSerialNo.Text = String.Empty
        cmbTransDetailType.SelectedIndex = 0
        txtRefDate.Text = String.Empty
        'txtReceiptRefNo1.Text = String.Empty
        txtRefAmt.Text = 0.0
        txtMainAcct.Text = String.Empty
        txtSubAcct.Text = String.Empty
        txtSubAcctDesc.Text = String.Empty
        txtMainAcctDesc.Text = String.Empty
        txtTransAmt.Text = 0.0
        txtSubSerialNo.Text = 0
        updateFlag = False
        Session("updateFlag") = updateFlag 'ready for a new record
        detailEdit = "N"
        Session("detailEdit") = detailEdit

        swEntry = "H"
        Session("swEntry") = swEntry

        prgKey = Session("prgKey")

    End Sub
    Protected Sub initializeDetailFields()
        cmbTransDetailType.SelectedIndex = 0
        txtRefAmt.Text = String.Empty
        txtRefDate.Text = String.Empty
        txtReceiptRefNo1.Text = String.Empty
        txtRefAmt.Text = 0.0
        txtMainAcct.Text = String.Empty
        txtSubAcct.Text = String.Empty
        txtSubAcctDesc.Text = String.Empty
        txtMainAcctDesc.Text = String.Empty
        txtLedgerType.Text = String.Empty
        txtSubSerialNo.Text = 0
        detailEdit = "N"
        Session("detailEdit") = detailEdit
        swEntry = "D"
        Session("swEntry") = swEntry
    End Sub

    Private Sub butDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles butDelete.Click
        Dim msg As String = String.Empty
        InvTrans = CType(Session("InvTrans"), CustodianLife.Model.Invoice)
        invRepo = CType(Session("invRepo"), InvoiceRepository)
        Try
            invRepo.Delete(InvTrans)
            msg = "Delete Successful"
            lblError.Text = msg
            grdData.DataBind()
        Catch ex As Exception
            msg = ex.Message
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
        End Try
        initializeDetailFields()

    End Sub

    Private Sub butClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles butClose.Click
        Dim msg As String = String.Empty

        Cum_Detail_Amount = CType(Session("Cum_Detail_Amount"), Decimal)
        Save_Amount = CType(Session("Save_Amount"), Decimal)


        'If (Save_Amount <> Cum_Detail_Amount) Then
        '    msg = "Total Amount " & Save_Amount & "is not equal to Detail Amounts!: " & Cum_Detail_Amount & "Pls Review before Closing"
        '    publicMsgs = "javascript:alert('" + msg + "')"

        '    Return 'do nothing
        'End If

        Response.Redirect("InvoiceList.aspx")

    End Sub
    Private Sub grdData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdData.RowDataBound

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim lblPrice As Label = CType(e.Row.FindControl("lblTransAmt"), Label)
            TransType = (DataBinder.Eval(e.Row.DataItem, "DRCR"))
            TransAmt = (Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TransAmt")))
            If TransType = "C" Then
                TransAmt = (TransAmt * -1)
            End If
            TotTransAmt = (TotTransAmt + TransAmt)

        End If
        If (e.Row.RowType = DataControlRowType.Footer) Then
            Dim lblTotal As Label = CType(e.Row.FindControl("lbltxtTotal"), Label)
            lblTotal.Text = Math.Round(TotTransAmt, 2).ToString
        End If

        'format fields
        Dim ea As GridViewRowEventArgs = CType(e, GridViewRowEventArgs)
        If (ea.Row.RowType = DataControlRowType.DataRow) Then
            Dim drv As Invoice = CType(ea.Row.DataItem, Invoice)
            ' Dim ob As Object = drv("GLAmountLC")
            If Not Convert.IsDBNull(drv.TransAmt) Then
                Dim iParsedValue As Decimal = 0
                If Decimal.TryParse(drv.TransAmt.ToString, iParsedValue) Then
                    Dim cell As TableCell = ea.Row.Cells(9)
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:N}", New Object() {iParsedValue})
                End If
            End If
        End If
    End Sub

    Private Sub butNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles butNew.Click
        initializeFields()
    End Sub

    Private Sub butNewDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles butNewDetail.Click
        initializeDetailFields()
    End Sub
    Private Function ValidDate(ByVal DateValue As String) As DateTime
        Dim dateparts() As String = DateValue.Split(Microsoft.VisualBasic.ChrW(47))
        Dim strDateTest As String = dateparts(1) & "/" & dateparts(0) & "/" & dateparts(2)
        Dim dateIn As Date = Format(CDate(strDateTest), "MM/dd/yyyy")
        Return dateIn
    End Function
    Private Function ValidDateFromDB(ByVal DateValue As Date) As String
        Dim dateparts() As String = DateValue.Date.ToString.Split(Microsoft.VisualBasic.ChrW(47))
        Dim strDateTest As String = dateparts(1) & "/" & dateparts(0) & "/" & Left(dateparts(2), 4)
        Return strDateTest
    End Function

    Private Sub butPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles butPrint.Click
        Response.Redirect("prg_fin_othr_recpt_print.aspx?id=" & txtReceiptNo.Text)
    End Sub

    <System.Web.Services.WebMethod()> _
        Public Shared Function GetTransType(ByVal _transcode As String) As String
        Dim transinfo As String = String.Empty
        Dim transRepo As New TransactionTypesRepository()
        'Dim crit As String = 

        Try
            transinfo = transRepo.GetTransInfo(_transcode)
            Return transinfo
        Finally
            If transinfo = "<NewDataSet />" Then
                Throw New Exception()
            End If
        End Try
    End Function
    Private Sub GetAcctDescription()
        Dim dt As DataSet = New DataSet()
        Dim recRep As New ReceiptsRepository()
        dt = recRep.GetAccountChartDetailsDataSet(txtSubAcct.Text, txtMainAcct.Text)
        If dt.Tables(0).Rows().Count <> 0 Then
            txtMainAcctDesc.Text = dt.Tables(0).Rows(0).Item("sMainDesc")
            txtSubAcctDesc.Text = dt.Tables(0).Rows(0).Item("sSubDesc")
        End If
    End Sub

    Private Sub ValidateFields(ByRef ErrorInd)
        Dim msg
        If txtReceiptNo.Text = "" Then
            msg = "Receipt number must not be empty"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtReceiptNo.Focus()
            Exit Sub
        End If
        If txtBatchNo.Text = "" Then
            msg = "Batch number must not be empty"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtBatchNo.Focus()
            Exit Sub
        End If
        If txtBatchNo.Text = 0 Then
            msg = "Batch number must not be equal to zero"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtBatchNo.Focus()
            Exit Sub
        End If
        If txtEffectiveDate.Text = "" Then
            msg = "Invoice date must not be empty"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtEffectiveDate.Focus()
            Exit Sub
        End If

        Dim str() As String
        str = DoDate_Process(txtEffectiveDate.Text, txtEffectiveDate)
        If (str(2) = Nothing) Then
            Dim errMsg = str(0).Insert(18, " Effective date, ")
            msg = errMsg.Replace("Javascript:alert('", "").Replace("');", "")
            lblError.Text = msg
            publicMsgs = "javascript:alert('" + msg + "')"
            lblError.Visible = True
            txtEffectiveDate.Focus()
            ErrorInd = "Y"
            Exit Sub
        Else
            txtEffectiveDate.Text = str(2).ToString()
        End If


        If txtTransDesc.Text = "" Then
            msg = "Transaction type must not be empty"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtTransDesc.Focus()
            Exit Sub
        End If

        If txtItem.Text = "" Then
            msg = "Item must not be empty"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtItem.Focus()
            Exit Sub
        End If

        If txtPrice.Text = "" Then
            msg = "Price must not be empty"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtPrice.Focus()
            Exit Sub
        End If



        If Not IsNumeric(txtPrice.Text) Then
            msg = "Price must be numeric"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtPrice.Focus()
            Exit Sub
        End If

        If txtQty.Text = "" Then
            msg = "Quantity must not be empty"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtItem.Focus()
            Exit Sub
        End If



        If Not IsNumeric(txtQty.Text) Then
            msg = "Quantity must be numeric"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtQty.Focus()
            Exit Sub
        End If

        If txtTransAmt.Text = "" Then
            msg = "Trans Amount must not be empty"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtTransAmt.Focus()
            Exit Sub
        End If



        If Not IsNumeric(txtTransAmt.Text) Then
            msg = "Trans Amount must be numeric"
            ErrorInd = "Y"
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"
            txtTransAmt.Focus()
            Exit Sub
        End If
    End Sub
    Function DoDate_Process(ByVal dateValue As String, ByVal ctrlId As Control) As String()
        Dim rtnMsg(3) As String
        Dim rtnMsg_ As String = Nothing

        'Checking fields for empty values
        If dateValue = "" Then
            rtnMsg_ = " Field is required!"
            rtnMsg(0) = rtnMsg_
            rtnMsg(1) = ctrlId.ID
            Return rtnMsg
        Else
            'Validate date
            Dim myarrData = Split(dateValue, "/")
            'If myarrData.Count <> 3 Then
            If myarrData.Length <> 3 Then
                rtnMsg_ = " Expecting full date in ddmmyyyy format ..."
                rtnMsg_ = "Javascript:alert('" & rtnMsg_ & "')"
                rtnMsg(0) = rtnMsg_
                rtnMsg(1) = ctrlId.ID
                Return rtnMsg
                'Exit Function
            End If
            Dim strMyDay = myarrData(0)
            Dim strMyMth = myarrData(1)
            Dim strMyYear = myarrData(2)

            strMyDay = CType(Format(Val(strMyDay), "00"), String)
            strMyMth = CType(Format(Val(strMyMth), "00"), String)
            strMyYear = CType(Format(Val(strMyYear), "0000"), String)

            Dim strMyDte = Trim(strMyDay) & "/" & Trim(strMyMth) & "/" & Trim(strMyYear)
            'dateValue = Trim(strMyDte)


            Dim blnStatusX = gnTest_TransDate(strMyDte)
            If blnStatusX = False Then
                rtnMsg_ = " is not a valid date..."
                rtnMsg_ = "Javascript:alert('" & rtnMsg_ & "');"
                rtnMsg(0) = rtnMsg_
                rtnMsg(1) = ctrlId.ID
                Return rtnMsg
                'Exit Function
            Else
                rtnMsg(2) = CType(strMyDte, String)
                Return rtnMsg
            End If
            dateValue = RTrim(strMyDte)
            'Exit Sub
        End If



        Return rtnMsg
    End Function

    Public Function gnTest_TransDate(ByVal MyFunc_Date As String) As Boolean

        On Error GoTo MyTestDate_Err1

        Dim pvbln As Boolean

        gnTest_TransDate = False
        pvbln = False

        'If Len(MyFunc_Date) = 10 And Mid(MyFunc_Date, 3, 1) = "/" And Mid(MyFunc_Date, 6, 1) = "/" Then
        'Else
        '    Return pvbln
        '    Exit Function
        'End If

        If (Len(MyFunc_Date) = 10) And _
           (Mid(MyFunc_Date, 3, 1) = "-" Or Mid(MyFunc_Date, 3, 1) = "/") And _
           (Mid(MyFunc_Date, 6, 1) = "-" Or Mid(MyFunc_Date, 6, 1) = "/") Then
        Else
            Return pvbln
            Exit Function
        End If

        Dim strDteMsg As String = "Invalid Date"
        Dim strDteErr As String = "0"
        Dim DteTst As Date

        Dim strDte_Start As String
        Dim strDte_End As String

        Dim strDteYY As String
        Dim strDteMM As String
        Dim strDteDD As String

        strDteMsg = ""
        strDteErr = "0"

        strDteMsg = ""
        strDteErr = "0"

        'MsgBox _
        ' "Left Xter. :" & Left(MyFunc_Date, 2) & vbCrLf & _
        ' "Mid Xter. :" & Mid(MyFunc_Date, 4, 2) & vbCrLf & _
        ' "Right Xter. :" & Right(MyFunc_Date, 4)

        'If MyFunc_Date = "__/__/____" Or _
        '   MyFunc_Date = "" Then
        '    MyTestDate_Trans = True
        '    Exit Function
        'End If

        strDteDD = Left(MyFunc_Date, 2)
        strDteMM = Mid(MyFunc_Date, 4, 2)
        strDteYY = Right(MyFunc_Date, 4)

        strDteDD = Trim(strDteDD)
        strDteMM = Trim(strDteMM)
        strDteYY = Trim(strDteYY)

        'If strDteDD = "" And _
        '   strDteMM = "" And _
        '   strDteYY = "" Then
        '    MyTestDate_Trans = True
        '    Exit Function
        'End If

        'If Val(Left(MyFunc_Date, 2)) = 0 And _
        '   Val(Mid(MyFunc_Date, 4, 2)) = 0 And _
        '   Val(Right(MyFunc_Date, 4)) = 0 Then
        '   MyTestDate_Trans = True
        '   Exit Function
        'End If

        If Trim(strDteDD) < "01" Or _
           Trim(strDteDD) > "31" Then
            strDteMsg = _
              "  -> Day < 01 or Day > 31 ..." & vbCrLf
            strDteErr = "1"
            'MsgBox "Day date error..."
        End If
        If Trim(strDteMM) < "01" Or _
           Trim(strDteMM) > "12" Then
            strDteMsg = strDteMsg & _
              "  -> Month < 01 or Month > 12 ..." & vbCrLf
            strDteErr = "1"
            'MsgBox "Month date error..."
        End If

        If Len(Trim(strDteYY)) < 4 Then
            strDteMsg = strDteMsg & _
              "  -> Year = 0 digit or Year < 4 digits..." & vbCrLf
            strDteErr = "1"
            'MsgBox "Year date error..." & Year(Now)
        End If


        strDte_Start = ""
        strDte_End = ""
        strDte_Start = MyFunc_Date
        strDte_End = MyFunc_Date

        Select Case Trim(strDteMM)
            Case "01", "03", "05", "07", "08", "10", "12"
                If Val(strDteDD) > 31 Then
                    strDteMsg = strDteMsg & _
                    "  -> Invalid day in month. Month <" & strDteMM & ">" & _
                    " ends in <" & " 31 " & ">" & _
                    ". Full Date: " & strDte_End & vbCrLf
                    strDteErr = "1"
                End If

            Case "02"
                If (Val(strDteYY) \ 4) = 0 Then
                    If Val(strDteDD) > 29 Then
                        strDteMsg = strDteMsg & _
                            "  -> Invalid day in month. Month <" & strDteMM & ">" & _
                            " ends in <" & " 29 " & ">" & _
                            ". Full Date: " & strDte_End & vbCrLf
                        strDteErr = "1"
                    End If
                Else
                    If Val(strDteDD) > 28 Then
                        strDteMsg = strDteMsg & _
                            "  -> Invalid day in month. Month <" & strDteMM & ">" & _
                            " ends in <" & " 28 " & ">" & _
                            ". Full Date: " & strDte_End & vbCrLf
                        strDteErr = "1"
                    End If

                End If

            Case "04", "06", "09", "11"
                If Val(strDteDD) > 30 Then
                    strDteMsg = strDteMsg & _
                    "  -> Invalid day in month. Month <" & strDteMM & ">" & _
                    " ends in <" & " 30 " & ">" & _
                    ". Full Date: " & strDte_End & vbCrLf
                    strDteErr = "1"
                End If
        End Select


MyTestDate_01:
        If strDteErr <> "0" Then
            GoTo MyTestDate_Msg
        End If

        gnTest_TransDate = True
        pvbln = True

        Return pvbln
        Exit Function

MyTestDate_Msg:

        'Call gnASPNET_MsgBox(strDteMsg)
        'Call gnASPNET_MsgBox("Invalid date...")
        'Call gnASPNET_MsgBox_VB(strDteMsg)

        gnTest_TransDate = False
        pvbln = False

        Return pvbln
        Exit Function

MyTestDate_Err1:
        gnTest_TransDate = False
        pvbln = False

        Return pvbln

    End Function

    Private Sub txtQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.TextChanged
        If (txtPrice.Text <> "" And txtQty.Text <> "") Then
            If IsNumeric(txtPrice.Text And txtQty.Text) Then
                txtTransAmt.Text = Format(txtPrice.Text * txtQty.Text, "Standard")
            End If
        End If
    End Sub

    Private Sub txtPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPrice.TextChanged
        If chkInvoiceNo.Checked Then
            txtReceiptNo.Enabled = True
        Else
            txtReceiptNo.Enabled = False
        End If
    End Sub

    Private Sub chkInvoiceNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInvoiceNo.CheckedChanged
        If chkInvoiceNo.Checked Then
            txtReceiptNo.Enabled = True
        Else
            txtReceiptNo.Enabled = False
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click, btnSaveDetail.Click
        lblError.Text = ""
        Dim msg As String = String.Empty
        Dim docMonth As String = String.Empty
        Dim docYear As String = String.Empty
        Dim Err = ""
        ValidateFields(Err)
        If Err = "Y" Then
            Exit Sub
        End If
        Try
            If Me.IsValid Then

                'this routine will persist only one object. 
                '1. The GlTrans object

                updateFlag = CType(Session("updateFlag"), Boolean)


                If Not updateFlag Then 'if new record

                    'create a new instance of the Invoice object
                    InvTrans = New CustodianLife.Model.Invoice()
                    invRepo = New InvoiceRepository()
                    lblError.Visible = False

                    With InvTrans
                        .BatchDate = CType(txtBatchDate.Text, Integer)
                        .BatchNo = CType(txtBatchNo.Text, Integer)
                        .BranchCode = cmbBranchCode.SelectedValue.ToString()
                        .CompanyCode = txtCompanyCode.Text
                        .DeptCode = cmbDept.SelectedValue.ToString()
                        .InvoiceNo = txtReceiptNo.Text
                        .EntryDate = Now.Date

                        .OperatorId = "001"
                        .PostStatus = "U" 'Unposted
                        .ApprovalStatus = "N"
                        .Flag = "A"
                        .ItemSize = txtItem.Text
                        .Price = txtPrice.Text
                        .Quantity = txtQty.Text
                        .MainAccountDR = txtMainAcct.Text
                        .SubAccountDR = txtSubAcct.Text

                        .TransDate = CType(txtEffectiveDate.Text, Date)
                        .TransDescription = txtTransDesc.Text
                        .TransType = cmbTransType.SelectedValue.ToString()

                        .TransId = "H" 'header

                        '.TotalAmt = CType(txtTotalAmt.Text, Decimal)  'detail amounts must total to this
                        Save_Amount = CType(txtTotalAmt.Text, Decimal)

                        Session("Save_Amount") = Save_Amount

                        Cum_Detail_Amount = 0
                        Session("Cum_Detail_Amount") = Cum_Detail_Amount

                        docMonth = Right(txtBatchDate.Text, 2)
                        docYear = Left(txtBatchDate.Text, 4)
                        'get new serial number
                        'L02(other receipts) , 001 -- for main serial number 
                        'L02(other receipts), 002 -- for Sub serial number 
                        If .TransId = "H" Then
                            Dim newSerialNum As String = invRepo.GetNextSerialNumber("L03", "001", docMonth, docYear, " ", "12", "11")


                            prgKey = Session("prgKey")
                            prgKey = Session("prgKey")
                            .TransType = "INV"
                            'get new INVOICE number
                            newDocRefNo = invRepo.GetNextSerialNumber("INV", _
                                                                     "001", _
                                                                     "001", _
                                                                   docYear, _
                                                                  "INV/", _
                                                                      "13", _
                                                                       "12")
                            txtReceiptNo.Text = newDocRefNo
                            txtSerialNo.Text = newSerialNum
                            .SerialNo = CType(txtSerialNo.Text, Long)
                            .InvoiceNo = Trim(txtReceiptNo.Text)

                        End If
                        .DRCR = String.Empty 'D
                        .TransAmt = txtTransAmt.Text
                        If Trim(txtRefDate.Text).Length() > 0 Then
                            .RefDate = CType(txtRefDate.Text, Date)
                        Else
                            .RefDate = #1/1/2014#
                        End If

                        .CreditorCode = txtReceiptRefNo1.Text

                        'save
                        invRepo.Save(InvTrans)
                        grdData.DataBind()
                        Session("InvTrans") = InvTrans
                        updateFlag = True
                        Session("updateFlag") = updateFlag
                        detailEdit = "N"
                        Session("detailEdit") = detailEdit
                        ' initializeDetailFields()
                    End With
                Else
                    swEntry = CType(Session("swEntry"), String)
                    If swEntry = "D" Then
                        InvTrans = New CustodianLife.Model.Invoice() 'a new object but filled with old values with the only changes from the detail part
                        detailEdit = "N"
                        Session("detailEdit") = detailEdit ' detail records in new rec mode

                    Else
                        detailEdit = Session("detailEdit") ' detail records in edit mode
                        InvTrans = CType(Session("InvTrans"), CustodianLife.Model.Invoice)
                    End If
                    invRepo = CType(Session("invRepo"), InvoiceRepository)


                    With InvTrans
                        .BatchDate = CType(txtBatchDate.Text, Integer)
                        .BatchNo = CType(txtBatchNo.Text, Integer)
                        .BranchCode = cmbBranchCode.SelectedValue.ToString()
                        .CompanyCode = txtCompanyCode.Text
                        .DeptCode = cmbDept.SelectedValue.ToString()
                        .InvoiceNo = txtReceiptNo.Text
                        .EntryDate = Now.Date

                        .CreditorCode = txtReceiptRefNo1.Text
                        .MainAccountDR = txtMainAcct.Text
                        .OperatorId = "001"
                        .PostStatus = "U" 'Unposted
                        .ApprovalStatus = "N"
                        .Flag = "A"
                        .ItemSize = txtItem.Text
                        .Price = txtPrice.Text
                        .Quantity = txtQty.Text
                        .TransAmt = txtTransAmt.Text
                        .MainAccountDR = txtMainAcct.Text
                        .SubAccountDR = txtSubAcct.Text

                        .TransDate = CType(txtEffectiveDate.Text, Date)
                        .TransDescription = txtTransDesc.Text
                        .TransType = cmbTransType.SelectedValue.ToString()
                        .EntryDate = Now.Date
                        docMonth = Right(txtBatchDate.Text, 2)
                        docYear = Left(txtBatchDate.Text, 4)
                        .TransDescription = txtTransDesc.Text
                        .SerialNo = CType(txtSerialNo.Text, Long)
                        .InvoiceNo = Trim(txtReceiptNo.Text)
                        '.LedgerTypeCode = Trim(txtLedgerType.Text)
                        .TransId = "D" 'Detail
                        .SerialNo = CType(txtSerialNo.Text, Long)
                        .InvoiceNo = txtReceiptNo.Text
                        .LedgerTypeCode = Trim(txtLedgerType.Text)
                        If Trim(txtRefDate.Text).Length > 0 Then
                            .RefDate = ValidDate(txtRefDate.Text)

                        Else
                            .RefDate = #1/1/2014#
                        End If
                        swEntry = CType(Session("swEntry"), String)
                        If swEntry = "H" Or swEntry = "D" Then
                            swEntry = "D"
                            Session("swEntry") = swEntry
                            'get sub serial number
                            'L02, 002 -- for Sub serial number 
                            detailEdit = CType(Session("detailEdit"), String)
                            If detailEdit = "N" Then
                                Dim newSerialSubNum As String = invRepo.GetNextSerialNumber("L02", "002", docMonth, docYear, " ", "12", "11")
                                txtSubSerialNo.Text = newSerialSubNum
                                .SubSerialNo = CType(txtSubSerialNo.Text, Integer)
                            End If
                        End If

                        prgKey = Session("prgKey")

                        Cum_Detail_Amount = Cum_Detail_Amount + .TransAmt
                        Session("Cum_Detail_Amount") = Cum_Detail_Amount

                        ' If .PostStatus = "U" And .ApprovalStatus = "N" Then  'Unposted and Not Approved -- change is possible

                        invRepo.Save(InvTrans)
                        msg = "Save Operation Successful"

                        lblError.Text = msg
                        lblError.Visible = True
                        publicMsgs = "javascript:alert('" + msg + "')"

                        'End If
                        grdData.DataBind()

                        Session("Gtrans") = InvTrans
                        initializeDetailFields()
                    End With
                End If
                ' initializeFields()
            End If
        Catch ex As Exception
            msg = ex.Message
            lblError.Text = msg
            lblError.Visible = True
            publicMsgs = "javascript:alert('" + msg + "')"

        End Try
    End Sub

    Private Sub txtReceiptNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptNo.TextChanged
        If txtReceiptNo.Text <> "" Then
            initializeFields()
            lblError.Text = ""
            invRepo = CType(Session("invRepo"), InvoiceRepository)

            ' InvTrans = invRepo.GetByInvoiceNo(Trim(txtReceiptNo.Text))
            Dim InvTransList As IList(Of CustodianLife.Model.Invoice)
            InvTransList = invRepo.GetByInvoiceNo(Trim(txtReceiptNo.Text))
            'If InvTrans IsNot Nothing Then
            '    txtReceiptNo.Enabled = False
            '    chkInvoiceNo.Checked = False
            '    txtBatchDate.Text = InvTrans.BatchDate
            '    txtBatchNo.Text = InvTrans.BatchNo
            '    cmbBranchCode.SelectedValue = InvTrans.BranchCode
            '    txtCompanyCode.Text = InvTrans.CompanyCode
            '    cmbDept.SelectedValue = InvTrans.DeptCode
            '    txtReceiptNo.Text = InvTrans.InvoiceNo
            '    txtEntryDate.Text = Format(InvTrans.EntryDate, "dd/MM/yyyy")
            '    txtRefDate.Text = Format(InvTrans.RefDate, "dd/MM/yyyy")
            '    txtMainAcct.Text = InvTrans.MainAccountDR
            '    txtEffectiveDate.Text = Format(InvTrans.TransDate, "dd/MM/yyyy")
            '    txtTransDesc.Text = InvTrans.TransDescription
            '    txtSerialNo.Text = InvTrans.SerialNo
            '    cmbTransType.SelectedValue = InvTrans.TransType
            '    txtTransAmt.Text = Format(InvTrans.TransAmt, "Standard")
            '    txtSerialNo.Text = InvTrans.SerialNo

            '    If InvTrans.SubAccountDR = "" Then
            '        txtSubAcct.Text = "000000"
            '    Else
            '        txtSubAcct.Text = InvTrans.SubAccountDR
            '    End If
            If InvTransList.Count > 0 Then
                ' If InvTransList(0) IsNot Nothing Then
                txtReceiptNo.Enabled = False
                chkInvoiceNo.Checked = False
                txtBatchDate.Text = InvTransList(0).BatchDate
                txtBatchNo.Text = InvTransList(0).BatchNo
                cmbBranchCode.SelectedValue = InvTransList(0).BranchCode
                txtCompanyCode.Text = InvTransList(0).CompanyCode
                cmbDept.SelectedValue = InvTransList(0).DeptCode
                txtReceiptNo.Text = InvTransList(0).InvoiceNo
                txtEntryDate.Text = Format(InvTransList(0).EntryDate, "dd/MM/yyyy")
                txtRefDate.Text = Format(InvTransList(0).RefDate, "dd/MM/yyyy")
                txtMainAcct.Text = InvTransList(0).MainAccountDR
                txtEffectiveDate.Text = Format(InvTransList(0).TransDate, "dd/MM/yyyy")
                txtTransDesc.Text = InvTransList(0).TransDescription
                txtSerialNo.Text = InvTransList(0).SerialNo
                cmbTransType.SelectedValue = InvTransList(0).TransType
                txtTransAmt.Text = Format(InvTransList(0).TransAmt, "Standard")
                txtSerialNo.Text = InvTransList(0).SerialNo

                If InvTransList(0).SubAccountDR = "" Then
                    txtSubAcct.Text = "000000"
                Else
                    txtSubAcct.Text = InvTransList(0).SubAccountDR
                End If

                updateFlag = True
                Session("updateFlag") = updateFlag
                'Session("InvTrans") = InvTrans
                Session("InvTrans") = InvTransList

                If txtMainAcct.Text <> "" Then
                    GetAcctDescription()
                End If
            End If
        End If

    End Sub

    Protected Sub cmbDept_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbDept.SelectedIndexChanged
          lblError.Text = ""

        If cmbDept.SelectedIndex <> 0 Then
            txtDeptCode.Text = cmbDept.SelectedValue
        End If
    End Sub
End Class