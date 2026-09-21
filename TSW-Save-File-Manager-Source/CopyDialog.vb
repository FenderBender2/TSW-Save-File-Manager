' $Id: CopyDialog.vb 1494 2026-09-21 21:33:13Z Pete $
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class CopyDialog

    Public ReadOnly Property ResultName As String
        Get
            If CopyToVersion.Text = "" Or CopyToProfile.Text = "" Then Return Nothing
            Return CopyToVersion.Text & "|" & CopyToProfile.Text
        End Get
    End Property

    ' -----------------------------------------------------------------------------------------------------------

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ObjectName As String = ""

    ' -----------------------------------------------------------------------------------------------------------

    <DllImport("user32.dll")>
    Private Shared Function HideCaret(hWnd As IntPtr) As Boolean
    End Function

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CopyDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisplayCopy(Me)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CopyToversion_Selected(sender As Object, e As EventArgs) Handles CopyToVersion.SelectedValueChanged
        PopulateProfileArray(GetSaveFolder(sender.text), copyProfileArray, CopyToProfile)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HeadingLabel_GotFocus(sender As Object, e As EventArgs) Handles FolderLabel.GotFocus
        HideCaret(FolderLabel.Handle)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles ActionButton.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
End Class