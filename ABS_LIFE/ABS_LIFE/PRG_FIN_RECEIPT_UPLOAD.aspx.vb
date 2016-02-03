Imports Microsoft.Office.Interop
Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Configuration
Imports System.Collections
Imports System.Data.Common
Imports System.Linq
Imports System.Web
Imports System.Web.Configuration
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Xml.Linq
Imports System.Globalization
Imports System.Collections.Generic
Imports CustodianGroupLife.Data
Imports CustodianGroupLife.Model
Imports CustodianGroupLife.Repositories
Imports CustodianLife.Data

Partial Public Class PRG_FIN_RECEIPT_UPLOAD
    Inherits System.Web.UI.Page
    Protected publicMsgs As String
    Protected PageLinks As String
    Protected STRMENU_TITLE As String
    Protected strStatus As String
    Protected blnStatus As Boolean
    Protected blnStatusX As Boolean
    Dim hashHlp As hashHelper
    Protected strGen_Msg As String = ""

    Dim strTmp_Value As String = ""
    Dim myarrData() As String


    Dim strPATH As String = ""
    Dim strErrMsg As String
    Dim lstErrMsgs As IList(Of String)


    '*********************************************************
    'Variable declarations for the data from Excel worksheet
    'Copied here from former location _do_save function
    '*********************************************************
    Dim strMyYear As String = ""
    Dim strMyMth As String = ""
    Dim strMyDay As String = ""

    Dim strMyDte As String = ""

    Dim mydteX As String = ""
    Dim mydte As Date = Now

    Dim lngDOB_ANB As Integer = 0

    Dim Dte_Current As Date = Now
    Dim Dte_DOB As Date = Now

    Dim sFT As String = ""
    Dim nRow As Integer = 2
    Dim nCol As Integer = 1

    Dim nROW_MIN As Integer = 0
    Dim nROW_MAX As Integer = 0

    Dim xx As String = ""

    Dim my_Batch_Num As String = ""

    Dim my_intCNT As Long = 0
    Dim my_SNo As String = ""

    Dim my_Dte_DOB As Date = Now
    Dim my_Dte_Start As Date = Now
    Dim my_Dte_End As Date = Now

    Dim my_File_Num As String = ""
    Dim my_Prop_Num As String = ""
    Dim my_Poly_Num As String = ""
    Dim my_Staff_Num As String = ""
    Dim my_Member_Name As String = ""
    Dim my_DOB As String = ""
    Dim my_AGE As String = ""
    Dim my_Gender As String = ""
    Dim my_Designation As String = ""
    Dim my_Start_Date As String = ""
    Dim my_End_Date As String = ""
    Dim my_Tenor As String = ""
    Dim my_SA_Factor As Single = 0
    Dim my_Basic_Sal As Double = 0
    Dim my_House_Allow As Double = 0
    Dim my_Transport_Allow As Double = 0
    Dim my_Other_Allow As Double = 0
    Dim my_Total_Salary As Double = 0
    Dim my_Total_SA As Double = 0

    Dim my_Medical_YN As String = ""

    Dim myRetValue As String = "0"
    Dim myTerm As String = ""
    Dim dteDOB As Date = Now '
    Dim Err As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.cmdFile_Upload.Attributes.Add("OnClick", "javascript:return scrollMSG(" & "'" & Me.SB_CONT.ClientID & "'" & ",'" & Me.SB_MSG.ClientID & "'" & ")")
        If Not (Page.IsPostBack) Then
            Me.txtXLS_Data_Start_No.Text = "1"
            Me.txtXLS_Data_End_No.Text = "1000"
        End If
    End Sub
    Protected Sub DoProc_Data_Source_Change()
        Call gnGET_SelectedItem(Me.cboData_Source, Me.txtData_Source_SW, Me.txtData_Source_Name, Me.lblError)
        Select Case UCase(Trim(Me.txtData_Source_SW.Text))
            Case "M"
                'tr_file_upload.Visible = False
                Me.cmdFile_Upload.Enabled = False
                ' Me.cmdSave_ASP.Enabled = True
            Case "U"
                'tr_file_upload.Visible = True
                'Me.cmdSave_ASP.Enabled = False
            Case Else
                'tr_file_upload.Visible = False
                Me.cmdFile_Upload.Enabled = False
                'Me.cmdSave_ASP.Enabled = False
        End Select
    End Sub

    Protected Sub cmdFile_Upload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdFile_Upload.Click
        lblError.Text = ""
        If Me.txtBatchNo.Text = "" Then
            Me.lblError.Text = "Missing " & Me.lblBatchNo.Text
            publicMsgs = "Javascript:alert('" & Me.lblError.Text & "')"
            Exit Sub
        End If
        Me.cmdFile_Upload.Enabled = False
        Call gnGET_SelectedItem(Me.cboData_Source, Me.txtData_Source_SW, Me.txtData_Source_Name, Me.lblError)
        Select Case UCase(Trim(Me.txtData_Source_SW.Text))
            Case "M"
                'Call Proc_DoSave()
                'Me.tr_file_upload.Visible = False
                Me.cmdFile_Upload.Enabled = False
            Case "U"
                Dim myfil As System.Web.HttpPostedFile = Me.My_File_Upload.PostedFile
                Me.txtFile_Upload.Text = Path.GetFileName(My_File_Upload.PostedFile.FileName)

                If Trim(Me.txtFile_Upload.Text) = "" Then
                    Me.lblError.Text = "Missing document or file name ..."
                    publicMsgs = "Javascript:alert('Missing document or file name. \nPlease select excel document with file extension .XLS or .XLSX')"
                    Me.txtFile_Upload.Text = ""
                    Me.lblError.Visible = True
                    Exit Sub
                End If

                If Right(LCase(Trim(Me.txtFile_Upload.Text)), 3) = "xls" Or _
                   Right(LCase(Trim(Me.txtFile_Upload.Text)), 4) = "xlsx" Then
                Else
                    Me.txtFile_Upload.Text = ""
                    Me.lblError.Text = "Invalid document or file type. Expecting file of type .XLS or .XLSX ..."
                    publicMsgs = "Javascript:alert('Invalid document or file name. \nPlease select excel document with file extension .XLS or .XLSX')"
                    Me.lblError.Visible = True
                    Exit Sub
                End If

                Try
                    'strPATH = CType(ConfigurationManager.ConnectionStrings("LIFE_DOC_PATH").ToString, String)
                    'strPATH = CType(ConfigurationManager.AppSettings("LIFE_DOC_PATH").ToString, String)
                    strPATH = Server.MapPath("~/App_Data/ReceiptUpload/")

                    Dim strFilePath As String = ""
                    'strFilePath = strPATH & Me.txtFile_Upload.Text
                    strFilePath = Server.MapPath("~/App_Data/ReceiptUpload/" & Me.txtFile_Upload.Text)
                    'post file to the server
                    My_File_Upload.PostedFile.SaveAs(strFilePath)


                    'Response.Write("<br/>Path: " & strFilePath)


                Catch ex As Exception
                    Me.txtFile_Upload.Text = ""
                    Me.lblError.Text = "Error has occured. <br />Reason: " & ex.Message.ToString
                    publicMsgs = "Javascript:alert('" & "Unable to upload document or file to the server" & "')"
                    Me.lblError.Visible = True
                    Exit Sub
                End Try

                Me.cmdFile_Upload.Enabled = False

                If Me.chkData_Source.Checked = True Then
                    Call Proc_DoSave_OLE()
                Else
                    Call Proc_DoSave_Upload()
                End If
                'Me.tr_file_upload.Visible = False

            Case Else
                Me.lblError.Text = "Missing or Invalid " & Me.lblData_Source.Text
                publicMsgs = "Javascript:alert('" & Me.lblError.Text & "')"
                Me.lblError.Visible = True
                Exit Sub

        End Select
    End Sub


    Private Sub Proc_DoSave_OLE()
        cboErr_List.Items.Clear()

        If Val(Trim(Me.txtXLS_Data_Start_No.Text)) < 1 Then
            Me.lblError.Text = "Error. Minimum start excel no should be 1 "
            publicMsgs = "Javascript:alert('" & Me.lblError.Text & "')"
            Exit Sub
        End If
        If Val(Trim(Me.txtXLS_Data_End_No.Text)) < 1 Or Val(Trim(Me.txtXLS_Data_End_No.Text)) < Val(Trim(Me.txtXLS_Data_Start_No.Text)) Then
            Me.lblError.Text = "Error. Either excel end no less than 1 or less than excel start no "
            publicMsgs = "Javascript:alert('" & Me.lblError.Text & "')"
            Me.lblError.Visible = True
            Exit Sub
        End If

        'blnStatusX = Proc_Batch_Check()
        'If blnStatusX = False Then
        '    Exit Sub
        'End If

        Me.lblError.Text = "File Name: " & Me.txtFile_Upload.Text

        If Trim(Me.txtFile_Upload.Text) = "" Then
            Me.txtFile_Upload.Text = ""
            Me.lblError.Text = "Missing document or file name ..."
            publicMsgs = "Javascript:alert('Missing document or file name. \nPlease select excel document with file extension .XLS or .XLSX')"
            Me.lblError.Visible = True
            Exit Sub
        End If

        If Right(LCase(Trim(Me.txtFile_Upload.Text)), 3) = "xls" Or _
           Right(LCase(Trim(Me.txtFile_Upload.Text)), 4) = "xlsx" Then
        Else
            Me.txtFile_Upload.Text = ""
            Me.lblError.Text = "Invalid document or file type. Expecting file of type .XLS or .XLSX ..."
            publicMsgs = "Javascript:alert('Invalid document or file name. \nPlease select excel document with file extension .XLS or .XLSX')"
            Me.lblError.Visible = True
            Exit Sub
        End If

        Dim strFilename As String
        Dim strFileNameOnly As String = txtFile_Upload.Text
        strPATH = Server.MapPath("~/App_Data/ReceiptUpload/")
        strFilename = strPATH & Me.txtFile_Upload.Text

        If System.IO.File.Exists(strFilename) = False Then
            Me.lblError.Text = "Document or file does not exist on the server ..."
            publicMsgs = "Javascript:alert('Document or file does not exist on the server')"
            Me.lblError.Visible = True
            Exit Sub
        End If

        Me.cmdFile_Upload.Enabled = False

        sFT = "Y"

        nRow = 2
        nCol = 0

        my_intCNT = 0

        'Dim myxls_workbook As Excel.Workbook
        'Dim myxls_worksheet As Excel.Worksheet

        'Dim myxls_range As Excel.Range

        'Try

        'Catch ex As Exception
        '    Me.lblError.Text = "Error has occured. Reason: " & UCase(ex.Message.ToString)
        '    publicMsgs = "Javascript:alert('" & RTrim("Unable to access data") & "')"
        '    Me.lblError.Visible = True
        '    Exit Sub
        'End Try

        'Dim mystr_con As String = CType(Session("connstr"), String)

        Dim mystr_con As String = ""
        'Dim myole_con As OleDbConnection = New OleDbConnection(mystr_con)

        'Try
        '    myole_con.Open()
        'Catch ex As Exception
        '    Me.lblError.Text = "Unable to connect to database. Reason: " & ex.Message
        '    'publicMsgs = "Javascript:alert('" & Me.txtMsg.Text & "')"
        '    publicMsgs = "Javascript:alert('" & "Unable to connect to database" & "')"
        '    Me.lblError.Visible = True
        '    GoTo MyLoop_End
        'End Try


        Dim myUserIDX As String = ""
        Try
            myUserIDX = CType(Session("MyUserIDX"), String)
        Catch ex As Exception
            myUserIDX = "SYS"
        End Try


        Dim mystr_sql As String = ""
        Dim mystr_sn_param As String = ""
        Dim mycnt As Integer = 0

        mystr_sn_param = "GL_MEMBER_SN"

        strGen_Msg = ""
        Me.lblErr_List.Visible = False
        Me.cboErr_List.Items.Clear()
        Me.cboErr_List.Visible = False

        my_intCNT = 0

        Dim myole_cmd As OleDbCommand = Nothing

        nROW_MIN = Val(Me.txtXLS_Data_Start_No.Text)
        nROW_MAX = Val(Me.txtXLS_Data_End_No.Text)
        nRow = 2

        Try
            'ClientScript.RegisterStartupScript(Me.GetType(), "scrollMSG_JavaScript", "scrollMSG(" & "'" & Me.SB_CONT.ClientID & "'" & ",'" & Me.SB_MSG.ClientID & "'" & ")", True)
        Catch ex As Exception

        End Try
        '*************************************************************************************
        'Gather the validated values from the form and pass 
        'to the hashHelper function
        '*************************************************************************************

        'Added by Azeez
        'Initially GenStart_Date looses value 
        'call the hashhelper function and pass the form values into it
        hashHelper.postFromExcel(strPATH, txtFile_Upload.Text.Trim, myUserIDX, nROW_MIN, nROW_MAX, mystr_con, _
        lstErrMsgs, txtData_Source_SW.Text, CInt(txtBatchNo.Text), String.Empty)
        GoTo MyLoop_999a



MyLoop_Start:
        nRow = nRow + 1

        If nRow < nROW_MIN Then
            GoTo MyLoop_Start
        End If

        If nRow > nROW_MAX Then
            GoTo MyLoop_999
        End If

        ' Initialize variables
        strGen_Msg = ""
MyLoop_888:
        If strGen_Msg <> "" Then
            Me.cboErr_List.Items.Add(strGen_Msg.ToString)
            Me.lblErr_List.Visible = True
            Me.cboErr_List.Visible = True
        End If

        strGen_Msg = ""

        GoTo MyLoop_Start


MyLoop_999:

        If my_intCNT >= 1 Then
            publicMsgs = "Javascript:alert('" & RTrim("File Upload successful - ") & Me.txtFile_Upload.Text & "')"
        Else
            publicMsgs = "Javascript:alert('" & RTrim("File Upload NOT successful - ") & Me.txtFile_Upload.Text & "')"
        End If

MyLoop_999a:
        If lstErrMsgs.Count > 1 Then
            For i = 0 To lstErrMsgs.Count - 1
                cboErr_List.Items.Add(lstErrMsgs.Item(i))
            Next

            Me.lblErr_List.Visible = True
            Me.cboErr_List.Visible = True


            publicMsgs = "Javascript:alert('" & RTrim("File Upload NOT successful - ") & Me.txtFile_Upload.Text & "')"

        Else
            Try
                ClientScript.RegisterStartupScript(Me.GetType(), "scrollMSG_JavaScript", "scrollMSG_End(" & "'" & Me.SB_CONT.ClientID & "'" & ",'" & Me.SB_MSG.ClientID & "'" & ")", True)
            Catch ex As Exception

            End Try

            publicMsgs = "Javascript:alert('" & RTrim("File Upload successful - ") & Me.txtFile_Upload.Text & "')"

        End If
        GoTo MyLoop_End

MyLoop_End:
        myole_cmd = Nothing
        Proc_DataBind()
    End Sub


    Private Sub Proc_DoSave_Upload()
        cboErr_List.Items.Clear()

        If Val(Trim(Me.txtXLS_Data_Start_No.Text)) < 1 Then
            Me.lblError.Text = "Error. Minimum start excel no should be 1 "
            publicMsgs = "Javascript:alert('" & Me.lblError.Text & "')"
            Me.lblError.Visible = True
            Exit Sub
        End If
        If Val(Trim(Me.txtXLS_Data_End_No.Text)) < 1 Or Val(Trim(Me.txtXLS_Data_End_No.Text)) < Val(Trim(Me.txtXLS_Data_Start_No.Text)) Then
            Me.lblError.Text = "Error. Either excel end no less than 1 or less than excel start no "
            publicMsgs = "Javascript:alert('" & Me.lblError.Text & "')"
            Me.lblError.Visible = True
            Exit Sub
        End If

        'blnStatusX = Proc_Batch_Check()
        'If blnStatusX = False Then
        '    Exit Sub
        'End If

        Me.lblError.Text = "File Name: " & Me.txtFile_Upload.Text

        If Trim(Me.txtFile_Upload.Text) = "" Then
            Me.txtFile_Upload.Text = ""
            Me.lblError.Text = "Missing document or file name ..."
            publicMsgs = "Javascript:alert('Missing document or file name. \nPlease select excel document with file extension .XLS or .XLSX')"
            Me.lblError.Visible = True
            Exit Sub
        End If

        If Right(LCase(Trim(Me.txtFile_Upload.Text)), 3) = "xls" Or _
           Right(LCase(Trim(Me.txtFile_Upload.Text)), 4) = "xlsx" Then
        Else
            Me.txtFile_Upload.Text = ""
            Me.lblError.Text = "Invalid document or file type. Expecting file of type .XLS or .XLSX ..."
            publicMsgs = "Javascript:alert('Invalid document or file name. \nPlease select excel document with file extension .XLS or .XLSX')"
            Me.lblError.Visible = True
            Exit Sub
        End If

        Dim strFilename As String
        Dim strFileNameOnly As String = txtFile_Upload.Text
        strPATH = Server.MapPath("~/App_Data/ReceiptUpload/")
        strFilename = strPATH & Me.txtFile_Upload.Text

        If System.IO.File.Exists(strFilename) = False Then
            Me.lblError.Text = "Document or file does not exist on the server ..."
            publicMsgs = "Javascript:alert('Document or file does not exist on the server')"
            Me.lblError.Visible = True
            Exit Sub
        End If

        Me.cmdFile_Upload.Enabled = False

        sFT = "Y"

        nRow = 2
        nCol = 0

        my_intCNT = 0

        'Dim myxls_workbook As Excel.Workbook
        'Dim myxls_worksheet As Excel.Worksheet

        'Dim myxls_range As Excel.Range

        'Try

        'Catch ex As Exception
        '    Me.lblError.Text = "Error has occured. Reason: " & UCase(ex.Message.ToString)
        '    publicMsgs = "Javascript:alert('" & RTrim("Unable to access data") & "')"
        '    Me.lblError.Visible = True
        '    Exit Sub

        'End Try

        'Dim mystr_con As String = CType(Session("connstr"), String)
        'Dim myole_con As OleDbConnection = New OleDbConnection(mystr_con)
        Dim mystr_con As String = ""
        'Try
        '    myole_con.Open()
        'Catch ex As Exception
        '    Me.lblError.Text = "Unable to connect to database. Reason: " & ex.Message
        '    'publicMsgs = "Javascript:alert('" & Me.txtMsg.Text & "')"
        '    publicMsgs = "Javascript:alert('" & "Unable to connect to database" & "')"
        '    Me.lblError.Visible = True
        '    GoTo MyLoop_End
        'End Try


        Dim myUserIDX As String = ""
        Try
            myUserIDX = CType(Session("MyUserIDX"), String)
        Catch ex As Exception
            myUserIDX = "SYS"
        End Try


        Dim mystr_sql As String = ""
        Dim mystr_sn_param As String = ""
        Dim mycnt As Integer = 0

        mystr_sn_param = "GL_MEMBER_SN"

        strGen_Msg = ""
        Me.lblErr_List.Visible = False
        Me.cboErr_List.Items.Clear()
        Me.cboErr_List.Visible = False

        my_intCNT = 0

        Dim myole_cmd As OleDbCommand = Nothing

        nROW_MIN = Val(Me.txtXLS_Data_Start_No.Text)
        nROW_MAX = Val(Me.txtXLS_Data_End_No.Text)
        nRow = 2

        Try
            'ClientScript.RegisterStartupScript(Me.GetType(), "scrollMSG_JavaScript", "scrollMSG(" & "'" & Me.SB_CONT.ClientID & "'" & ",'" & Me.SB_MSG.ClientID & "'" & ")", True)
        Catch ex As Exception

        End Try
        '*************************************************************************************
        'Gather the validated values from the form and pass 
        'to the hashHelper function
        '*************************************************************************************

        'Added by Azeez
        'Initially GenStart_Date looses value 
        'call the hashhelper function and pass the form values into it
        hashHelper.postFromExcel(strPATH, txtFile_Upload.Text.Trim, myUserIDX, nROW_MIN, nROW_MAX, mystr_con, _
         lstErrMsgs, txtData_Source_SW.Text, CInt(txtBatchNo.Text), String.Empty)
        GoTo MyLoop_999a



MyLoop_Start:
        nRow = nRow + 1

        If nRow < nROW_MIN Then
            GoTo MyLoop_Start
        End If

        If nRow > nROW_MAX Then
            GoTo MyLoop_999
        End If

        ' Initialize variables
        strGen_Msg = ""
MyLoop_888:
        If strGen_Msg <> "" Then
            Me.cboErr_List.Items.Add(strGen_Msg.ToString)
            Me.lblErr_List.Visible = True
            Me.cboErr_List.Visible = True
        End If

        strGen_Msg = ""

        GoTo MyLoop_Start


MyLoop_999:

        If my_intCNT >= 1 Then
            publicMsgs = "Javascript:alert('" & RTrim("File Upload successful - ") & Me.txtFile_Upload.Text & "')"
        Else
            publicMsgs = "Javascript:alert('" & RTrim("File Upload NOT successful - ") & Me.txtFile_Upload.Text & "')"
        End If

MyLoop_999a:
        If lstErrMsgs.Count > 1 Then
            For i = 0 To lstErrMsgs.Count - 1
                cboErr_List.Items.Add(lstErrMsgs.Item(i))
            Next

            Me.lblErr_List.Visible = True
            Me.cboErr_List.Visible = True


            publicMsgs = "Javascript:alert('" & RTrim("File Upload NOT successful - ") & Me.txtFile_Upload.Text & "')"

        Else
            Try
                ClientScript.RegisterStartupScript(Me.GetType(), "scrollMSG_JavaScript", "scrollMSG_End(" & "'" & Me.SB_CONT.ClientID & "'" & ",'" & Me.SB_MSG.ClientID & "'" & ")", True)
            Catch ex As Exception

            End Try

            publicMsgs = "Javascript:alert('" & RTrim("File Upload successful - ") & Me.txtFile_Upload.Text & "')"

        End If
        GoTo MyLoop_End

MyLoop_End:
        myole_cmd = Nothing
        Proc_DataBind()
    End Sub
    Public Sub gnGET_SelectedItem(ByVal pvDDL_Control As DropDownList, ByVal pvCtr_Value As TextBox, ByVal pvCtr_Text As TextBox, Optional ByVal pvCtr_Label As Label = Nothing)
        Try

            If pvDDL_Control.SelectedIndex = -1 Or pvDDL_Control.SelectedIndex = 0 Or _
                pvDDL_Control.SelectedItem.Value = "" Or pvDDL_Control.SelectedItem.Value = "*" Then
                pvCtr_Value.Text = ""
                pvCtr_Text.Text = ""
            Else
                pvCtr_Value.Text = pvDDL_Control.SelectedItem.Value
                pvCtr_Text.Text = pvDDL_Control.SelectedItem.Text
            End If
        Catch ex As Exception
            If pvCtr_Label IsNot Nothing Then
                If TypeOf pvCtr_Label Is System.Web.UI.WebControls.Label Then
                    pvCtr_Label.Text = "Error. Reason: " & ex.Message.ToString
                End If
            End If
        End Try
    End Sub
    Public Sub Proc_DataBind()
        hashHlp = New hashHelper()
        grdUpldRecords.DataSource = hashHlp.ProcessBind(txtBatchNo.Text)
        grdUpldRecords.DataBind()

        Dim P As Integer = 0
        Dim C As Integer = 0
        C = 0
        For P = 0 To Me.grdUpldRecords.Rows.Count - 1
            C = C + 1
        Next
        Me.lblResult.Text = "Total Row: " & C.ToString
    End Sub

    Private Sub grdUpldRecords_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUpldRecords.PageIndexChanging
        grdUpldRecords.PageIndex = e.NewPageIndex
        Proc_DataBind()
    End Sub
End Class