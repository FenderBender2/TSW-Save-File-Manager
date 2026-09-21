' $Id: Buttons.vb 1494 2026-09-21 21:33:13Z Pete $
Imports System.IO

Module Buttons

    ' -----------------------------------------------------------------------------------------------------------
    ' Run the selected version of TSW or TSC
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub RunGame(Optional runTSC As Boolean = False)

        Dim appID = If(runTSC, TSCappId, TSWappId)
        If Not TSWEnableFunctions AndAlso appID.ToString() = "" Then Exit Sub

        Try
            Dim psi As New ProcessStartInfo With {.FileName = "steam://rungameid/" & appID, .UseShellExecute = True}  ' REQUIRED for URI protocols
            Process.Start(psi)

        Catch ex As Exception
            MessageBox.Show($"Launch failed: {ex.Message}", "Start Game", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Save a new file
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub SaveFile(tc As TabControl, newName As String, folder As String, TSWFile As String)

        If newName = "" Or Not TSWEnableFunctions Then Exit Sub

        Dim tabtext = tc.SelectedTab.Text
        Dim cleanStr = Path.GetFileNameWithoutExtension(String.Concat(newName.Where(Function(c) Not Path.GetInvalidFileNameChars.Contains(c))))
        Dim sourceFile = Path.Combine(folder, TSWFile & If(Not Path.HasExtension(TSWFile), ".sav", ""))
        Dim destFile = Path.Combine(TSWCurrentProfilePath, If(tabtext = "Main", "", tabtext), cleanStr)

        destFile &= If(Not Path.HasExtension(destFile), ".sav", "")

        If File.Exists(destFile) Then
            Dim response = MessageBox.Show($"{cleanStr} already exists. Overwrite?", "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If response = DialogResult.No Then
                ShowTempMessage("Save file cancelled")
                Exit Sub
            End If
        End If

        File.Copy(sourceFile, destFile, True)
        UpdateUI(folder)
        ShowTempMessage($"{Path.GetFileName(cleanStr)} saved successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Restore a file
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub RestoreFile(cf As ListView, tc As TabControl, saveFileName As String, folder As String)

        If cf.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a custom save file.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim filename = cf.SelectedItems(0).SubItems(1).Text
        Dim folderPath As String = Path.Combine(TSWCurrentProfilePath, If(tc.SelectedTab.Text = "Main", "", tc.SelectedTab.Text))
        Dim targetFolder As String = folder
        Dim sourceFile As String = Path.Combine(folderPath, filename & If(Not Path.HasExtension(filename), ".sav", ""))
        Dim destFile As String = Path.Combine(targetFolder, saveFileName & If(Not Path.HasExtension(saveFileName), ".sav", ""))

        If File.Exists(destFile) Then
            Dim msg = If(TSWcurrentIsSaved, "Are you sure you want to overwrite the TSW save file",
                                            "Warning: The current TSW save file has not been saved to your custom list. Continue to overwrite")
            msg &= $" with {filename}?"

            Dim boxIcon = If(TSWcurrentIsSaved, MessageBoxIcon.Question, MessageBoxIcon.Warning)
            Dim response = MessageBox.Show(msg, $"{If(Not TSWcurrentIsSaved, "Warning: ", "")}Confirm Overwrite", MessageBoxButtons.YesNo, boxIcon)

            If response = DialogResult.No Then
                ShowTempMessage("Restore cancelled")
                Exit Sub
            End If
        End If

        isMyWrite = True
        File.Copy(sourceFile, destFile, True)
        UpdateUI(folder)
        isMyWrite = False

        ShowTempMessage($"{filename} restored successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Delete a file
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DeleteFile(cf As ListView)

        If cf.SelectedItems.Count = 0 Then
            MessageBox.Show("No file selected.", "Delete File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim fileName = cf.SelectedItems(0).SubItems(1).Text
        Dim folderPath As String = Path.Combine(TSWCurrentProfilePath, GetTab)
        Dim sourceFile As String = Path.Combine(folderPath, fileName & If(Not Path.HasExtension(fileName), ".sav", ""))

        If File.Exists(sourceFile) Then
            Dim response = MessageBox.Show($"Are you sure you want to delete {fileName}?" & vbCrLf & vbCrLf &
                                            "Deleted files are sent to the recycle bin.",
                                            "Confirm Delete",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Warning)
            If response <> DialogResult.Yes Then Exit Sub
        End If


        FileIO.FileSystem.DeleteFile(sourceFile, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        ListSaveFiles(folderPath)

        ShowTempMessage($"{fileName} deleted and saved in Recycle Bin")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Rename a file
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub RenameFile(cf As ListView, newFileName As String)

        If newFileName = "" Or cf.SelectedItems.Count = 0 Or Not TSWEnableFunctions Then Exit Sub

        Dim fileName = cf.SelectedItems(0).SubItems(1).Text
        Dim newName = Path.GetFileNameWithoutExtension(String.Concat(newFileName.Where(Function(c) Not Path.GetInvalidFileNameChars.Contains(c))))

        Dim folderPath As String = Path.Combine(TSWCurrentProfilePath, GetTab)
        Dim sourceFile As String = Path.Combine(folderPath, fileName & If(Not Path.HasExtension(fileName), ".sav", ""))
        Dim targetFile As String = Path.Combine(folderPath, newName & If(Not Path.HasExtension(newName), ".sav", ""))

        If targetFile = sourceFile Then Exit Sub

        If File.Exists(targetFile) Then
            If MessageBox.Show($"File {newName} already exists in this folder. Do you want to create a new version of the file?",
                               "Rename File", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = vbNo Then Exit Sub

            targetFile = GetUniqueFilename(targetFile)
            newName = Path.GetFileNameWithoutExtension(targetFile)
        Else
            Dim msg = "Are you sure you want to rename " & fileName & " to " & newName & "?"
            If File.Exists(sourceFile) AndAlso MessageBox.Show(msg, "Confirm Rename", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        End If

        File.Move(sourceFile, targetFile)
        ListSaveFiles(folderPath)
        ShowTempMessage($"{fileName} renamed to {newName}")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Select a file to move
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub FilesToMove(cf As ListView, tc As TabControl)

        If cf.SelectedItems.Count = 0 Then
            MessageBox.Show("No file selected.", "Move File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If tc.TabCount < 2 Then
            Dim profileCount = profileArray.GetLength(0)
            Dim iconName = If(profileCount < 2, MessageBoxIcon.Warning, MessageBoxIcon.Information)
            Dim msg = If(profileCount > 1, " You can move files to other profiles.", "")

            MessageBox.Show("There no other folders available in the current profile." & msg, "Move File", MessageBoxButtons.OK, iconName)

            If profileCount < 2 Then Exit Sub
        End If

        Dim fileName = cf.SelectedItems(0).SubItems(1).Text
        Dim folderName As String = TSWInputBox("Move", fileName)
        If folderName = "" Then Exit Sub

        Dim currentTab = tc.SelectedTab.Text
        Dim sourcefolder = Path.Combine(TSWCurrentProfilePath, If(currentTab = "Main", "", currentTab))
        Dim targetFile = fileName & If(Not Path.HasExtension(fileName), ".sav", "")
        Dim targetFolder = Path.Combine(Path.Combine(TSWCustomParent, If(folderName = "Main", "", folderName)))

        If MoveFiles(sourcefolder, targetFolder, targetFile) Then
            ListSaveFiles(sourcefolder)

            If folderName.StartsWith("Profile") Then

                Dim profileID As String = folderName.Replace("Profile", "")

                For i As Integer = 0 To profileArray.GetLength(0) - 1
                    If profileArray(i, 0) = profileID Then folderName = profileArray(i, 1)
                Next

                ShowTempMessage($"{fileName} moved successfully to profile {folderName}")
            Else
                ShowTempMessage($"{fileName} moved successfully to {folderName}")
            End If
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Right-Click performed on listview - display file menu
    ' -----------------------------------------------------------------------------------------------------------

    Public Sub DisplayFileMenu(cf As ListView, e As MouseEventArgs)

        TSWSFM.FolderSelect.AllowDrop = False
        Dim info = cf.HitTest(e.X, e.Y)

        If info.Item IsNot Nothing Then
            info.Item.Selected = True
            TSWSFM.FileMenu.Show(cf, e.Location)
        End If

        TSWSFM.FolderSelect.AllowDrop = True

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
End Module
