' $Id: FormFunctions.vb 1494 2026-09-21 21:33:13Z Pete $
Imports System.IO
Imports System.Runtime.Intrinsics

Module FormFunctions

    ' ===========================================================================================================
    ' Move and Copy dialog functions
    ' -----------------------------------------------------------------------------------------------------------
    ' Configure the form depending on the function it is performing
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DisplayMove(dialogFrm As Form)

        Dim dlg As MoveDialog = DirectCast(dialogFrm, MoveDialog)

        With dlg
            .HeadingLabel.Text = $"File: {Path.GetFileNameWithoutExtension(.ObjectName)}"
            .CopyToProfile.Items.Clear()

            Dim selectedIdx As Integer

            For i As Integer = 0 To profileArray.GetLength(0) - 1
                .CopyToProfile.Items.Add(profileArray(i, 1))
                If profileArray(i, 1) = TSWSFM.ProfileSelect.Text Then selectedIdx = i
            Next

            .CopyToProfile.SelectedIndex = selectedIdx
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Public Sub PopulateFolderList(dialogFrm As Form, parentFolder As String)

        Dim dlg As MoveDialog = DirectCast(dialogFrm, MoveDialog)

        Dim currentTab = TSWSFM.FolderSelect.SelectedTab.Text
        Dim currentProfile As String = GetCurrentProfile(TSWSFM.ProfileSelect)

        With dlg
            .FolderList.Items.Clear()
            .FolderList.Text = ""

            If currentTab <> "Main" Or currentProfile <> GetCurrentProfile(.CopyToProfile) Then .FolderList.Items.Add("Main")

            For Each folder As String In Directory.GetDirectories(parentFolder)
                Dim fileName = Path.GetFileName(folder)
                If currentTab <> fileName Or currentProfile <> GetCurrentProfile(.CopyToProfile) Then .FolderList.Items.Add(fileName)
            Next
        End With
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DisplayCopy(dialogFrm As Form)

        Dim dlg As CopyDialog = DirectCast(dialogFrm, CopyDialog)

        With dlg
            .FolderLabel.Text = $"Folder to Copy: {Path.GetFileNameWithoutExtension(.ObjectName)}"
            .CopyToVersion.Items.Clear()
        End With

        ' If Not PopulateVersions(dlg.CopyToVersion) Then Exit Sub
        PopulateVersions(dlg.CopyToVersion)

    End Sub

    ' ===========================================================================================================
    ' Dynamic dialog form functions
    ' -----------------------------------------------------------------------------------------------------------
    ' Configure the form depending on the function it is performing
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DisplayDialog(dialogFrm As Form, formMode As String)

        Dim dlg As TSWDialog = DirectCast(dialogFrm, TSWDialog)

        With dlg
            Select Case formMode
                Case "Profile"
                    .HeadingLabel.Text = $"Current profile name: { .ObjectName}"

                Case "Rename"
                    .HeadingLabel.Text = $"Enter a new name for the { .ObjectName} folder"

                Case "New"
                    .HeadingLabel.Text = "Enter a name for the new folder"

                Case Else
                    MessageBox.Show($"Unknown mode: {formMode}", "TSW File Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Select

            .Text = If(formMode = "Profile", "Rename Profile", $"{formMode} folder")
            .TextBox.Text = If(formMode = "Profile" Or formMode = "Rename", .ObjectName, "")
            .PromptLabel.Text = If(formMode = "Profile", "Profile Name", "Folder Name")
            .ActionButton.Text = If(formMode = "New", "Create", "Rename")
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' If we are copying a folder to another version, populate the version selection list
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub PopulateVersions(vl As ComboBox) ' As Boolean

        Dim selectedName = TSWSFM.VersionSelect.SelectedItem.ToString
        Dim selectedAppID As Integer = Integer.Parse(TSWVersions.First(Function(v) v.Name = selectedName).AppID)
        '        Dim found As Boolean = False

        For Each v In TSWVersions
            If v.AppID > selectedAppID Then vl.Items.Add(v.Name)
        Next

        If vl.Items.Count = 1 Then vl.SelectedIndex = 0

        '        Return True

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
End Module
