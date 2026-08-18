Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices

Public Class MoveDialog

    Public ReadOnly Property ResultName As String
        Get
            Dim selectedProdile As String = GetSelectedProfile(CopyToProfile)

            If FolderList.Text.Trim() = "" Then
                Return ""
            ElseIf FolderList.Text.Trim() = "Main" Then
                Return "Profile" & selectedProdile
            Else
                Return Path.Combine("Profile" & selectedProdile, FolderList.Text.Trim())
            End If
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
    Private Function GetSelectedProfile(cb As ComboBox) As String

        For i As Integer = 0 To ProfileArray.GetLength(0) - 1
            If ProfileArray(i, 1) = cb.Text Then Return ProfileArray(i, 0)
        Next

        Return ""

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MoveDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        HeadingLabel.Text = $"File: {Path.GetFileNameWithoutExtension(ObjectName)}"

        Dim parentFolder = Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder)
        Dim selectedIdx As Integer

        CopyToProfile.Items.Clear()

        For i As Integer = 0 To ProfileArray.GetLength(0) - 1
            CopyToProfile.Items.Add(ProfileArray(i, 1))
            If ProfileArray(i, 1) = TSWSFM.ProfileSelect.Text Then selectedIdx = i
        Next

        CopyToProfile.SelectedIndex = selectedIdx

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CopyToProfile_Selected(sender As Object, e As EventArgs) Handles CopyToProfile.SelectedValueChanged

        FolderList.Text = ""
        FolderList.Items.Clear()

        Dim currentProfile As String = GetSelectedProfile(CopyToProfile)
        PopulateFolderList(Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder, "Profile" & currentProfile))

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub PopulateFolderList(parentFolder As String)

        Dim currentTab = TSWSFM.TabControl.SelectedTab.Text
        Dim currentProfile As String = GetSelectedProfile(TSWSFM.ProfileSelect)

        FolderList.Items.Clear()
        If currentTab <> "Main" Or currentProfile <> GetSelectedProfile(CopyToProfile) Then FolderList.Items.Add("Main")

        For Each folder As String In Directory.GetDirectories(parentFolder)
            Dim fileName = Path.GetFileName(folder)
            If currentTab <> fileName Or currentProfile <> GetSelectedProfile(CopyToProfile) Then FolderList.Items.Add(fileName)
        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HeadingLabel_GotFocus(sender As Object, e As EventArgs) Handles HeadingLabel.GotFocus
        HideCaret(HeadingLabel.Handle)
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