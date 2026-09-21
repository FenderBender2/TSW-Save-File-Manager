' $Id: TSWDialog.vb 1469 2026-08-24 08:52:18Z Pete $
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class TSWDialog

    ' -----------------------------------------------------------------------------------------------------------
    Public ReadOnly Property ResultName As String
        Get
            Return TextBox.Text.Trim()
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
        DisplayDialog(Me, Mode)
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