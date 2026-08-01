Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Dialog

    ' -----------------------------------------------------------------------------------------------------------
    Public ReadOnly Property ResultName As String
        Get
            Return If(Mode = "Move", FolderList.Text.Trim(), TextBox.Text.Trim())
        End Get
    End Property

    ' -----------------------------------------------------------------------------------------------------------

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Mode As String

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ObjectName As String = ""

    ' -----------------------------------------------------------------------------------------------------------

    <DllImport("user32.dll")>
    Private Shared Function HideCaret(hWnd As IntPtr) As Boolean
    End Function

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub Dialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = If(Mode = "Move", "Move File", If(Mode = "Rename", "Rename", "New") & " Folder")
        Me.ActionButton.Text = If(Mode = "New", "Create", Mode)
        TextBox.Visible = If(Mode = "Move", False, True)
        FolderList.Visible = If(Mode = "Move", True, False)

        If Mode = "Move" Then
            HeadingLabel.Text = "File: " & Path.GetFileNameWithoutExtension(ObjectName)

            Dim parentFolder = Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder)
            Dim currentTab = TSWSFM.TabControl.SelectedTab.Text

            If currentTab <> "Main" Then Me.FolderList.Items.Add("Main")

            For Each folder As String In Directory.GetDirectories(parentFolder)
                Dim fileName = Path.GetFileName(folder)
                If currentTab <> fileName Then FolderList.Items.Add(fileName)
            Next

        ElseIf Mode = "Rename" Then
            HeadingLabel.Text = "Enter a new name for the " & ObjectName & " folder"
            Me.TextBox.Text = ObjectName
        Else
            HeadingLabel.Text = "Enter a name for the new folder"
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HeadingLabel_GotFocus(sender As Object, e As EventArgs) Handles HeadingLabel.GotFocus
        HideCaret(HeadingLabel.Handle)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles ActionButton.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Public Sub New(mode As String)
        InitializeComponent()
        Me.Mode = mode
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
End Class