Imports System.IO
Imports System.Threading

Public Class TSWSFM

    Dim TSWappId As Integer
    Private Shared mutex As Mutex

    ' -----------------------------------------------------------------------------------------------------------
    ' Form load
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub TSWSFM_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim createdNew As Boolean
        mutex = New Mutex(True, "TSW_SaveManager_SingleInstance", createdNew)

        If Not createdNew Then
            MessageBox.Show("TSW Save Manager is already running.", "TSW Save File Manager")
            Me.Close()
            Return
        End If

        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        IsMyWrite = False

        GetLatestVersion()

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Save button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        If NewFileName.Text = "" Then Exit Sub

        Dim cleanStr = String.Concat(NewFileName.Text.Where(Function(c) Not Path.GetInvalidFileNameChars.Contains(c)))
        Dim sourceFile = Path.Combine(CurrentFolder.Text, SaveFileName.Text)
        Dim destFile = Path.Combine(CurrentFolder.Text, "Custom Saves", cleanStr)

        If Not Path.HasExtension(destFile) Then
            destFile &= ".sav"
        End If

        If File.Exists(destFile) Then
            Dim response = MessageBox.Show(NewFileName.Text & " already exists. Overwrite?", "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If response = DialogResult.No Then
                ShowTempMessage("Save file cancelled")
                Exit Sub
            End If
        End If

        File.Copy(sourceFile, destFile, True)
        UpdateUI(CurrentFolder.Text)
        NewFileName.Text = ""

        ShowTempMessage("File saved successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Run TSW button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RunButton_Click(sender As Object, e As EventArgs) Handles RunButton.Click

        If VersionSelect.Text = "" Or TSWappId.ToString() = "" Then Exit Sub

        Try
            Dim psi As New ProcessStartInfo()
            psi.FileName = "steam://rungameid/" & TSWappId
            psi.UseShellExecute = True   ' REQUIRED for URI protocols

            Process.Start(psi)

        Catch ex As Exception
            MessageBox.Show("Launch failed: " & ex.Message, "Start Game", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Restore button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RestoreButton_Click(sender As Object, e As EventArgs) Handles RestoreButton.Click

        Dim folderPath As String = IO.Path.Combine(CurrentFolder.Text, "Custom Saves")
        Dim targetFolder As String = CurrentFolder.Text

        If CustomFileList.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a custom save file.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim sourceFile As String = IO.Path.Combine(folderPath, CustomFileList.SelectedItems(0).SubItems(1).Text)
        Dim destFile As String = IO.Path.Combine(targetFolder, SaveFileName.Text)
        Dim msgstring As String
        Dim boxIcon As MessageBoxIcon

        If IO.File.Exists(destFile) Then
            msgstring = If(TSWcurrentIsSaved, "Are you sure you want to overwrite the TSW save file", "Warning: The current TSW save file has not been saved to your custom list. Continue to overwrite")
            msgstring &= " with " & CustomFileList.SelectedItems(0).SubItems(1).Text & "?"
            boxIcon = If(TSWcurrentIsSaved, MessageBoxIcon.Question, MessageBoxIcon.Warning)

            Dim response = MessageBox.Show(msgstring, If(Not TSWcurrentIsSaved, "Warning: ", "") & "Confirm Overwrite", MessageBoxButtons.YesNo, boxIcon)

            If response = DialogResult.No Then
                ShowTempMessage("Restore cancelled")
                Exit Sub
            End If
        End If

        IsMyWrite = True
        IO.File.Copy(sourceFile, destFile, True)
        UpdateUI(CurrentFolder.Text)
        IsMyWrite = False

        ShowTempMessage("File restored successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Delete button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        Dim folderPath As String = IO.Path.Combine(CurrentFolder.Text, "Custom Saves")

        If CustomFileList.SelectedItems.Count = 0 Then
            MessageBox.Show("No file selected.")
            Exit Sub
        End If

        Dim sourceFile As String = IO.Path.Combine(folderPath, CustomFileList.SelectedItems(0).SubItems(1).Text)

        If IO.File.Exists(sourceFile) Then
            Dim response = MessageBox.Show(
                "Are you sure you want to delete " & CustomFileList.SelectedItems(0).SubItems(1).Text & "? Deleted files are sent to the recycle bin.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )

            If response = DialogResult.No Then Exit Sub
        End If

        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(sourceFile, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin)
        ListSaveFiles(folderPath)

        ShowTempMessage("File deleted and saved in Recycle Bin")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Close button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Catch listview double-click event
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_DoubleClick(sender As Object, e As EventArgs) Handles CustomFileList.DoubleClick
        RestoreButton_Click(sender, e)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' When the selected TSW version is changed
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub VersionSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles VersionSelect.SelectedIndexChanged

        Dim selectedName As String = VersionSelect.SelectedItem.ToString()
        Dim selected = TSWVersions.First(Function(v) v.Name = selectedName)

        TSWappId = selected.AppID

        CurrentFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\My Games\" & VersionSelect.Text.Replace(" ", "") & "\Saved\SaveGames"
        RefreshSaveFile(CurrentFolder.Text)

        Dim folderPath As String = IO.Path.Combine(CurrentFolder.Text, "Custom Saves")

        If Not IO.Directory.Exists(folderPath) Then
            IO.Directory.CreateDirectory(folderPath)
        End If

        ListSaveFiles(folderPath)
        SetupWatcher()
        LoadTSWIcon(TSWVersions(VersionSelect.SelectedIndex))

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Trap selection of a custom save file
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CustomFileList.Click
        NewFileName.Text = CustomFileList.SelectedItems(0).SubItems(1).Text
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Rename button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RenameButton_Click(sender As Object, e As EventArgs) Handles RenameButton.Click

        If NewFileName.Text = "" Or CustomFileList.SelectedItems.Count = 0 Then Exit Sub

        Dim folderPath As String = IO.Path.Combine(CurrentFolder.Text, "Custom Saves")
        Dim sourceFile As String = IO.Path.Combine(folderPath, CustomFileList.SelectedItems(0).SubItems(1).Text)

        If IO.Path.Combine(folderPath, NewFileName.Text) = sourceFile Then Exit Sub

        If IO.File.Exists(sourceFile) Then
            Dim response = MessageBox.Show(
                "Are you sure you want to rename " & CustomFileList.SelectedItems(0).SubItems(1).Text & " to " & NewFileName.Text & "?",
                "Confirm Rename",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )

            If response = DialogResult.No Then Exit Sub
        End If

        IO.File.Move(IO.Path.Combine(folderPath, CustomFileList.SelectedItems(0).SubItems(1).Text), IO.Path.Combine(folderPath, NewFileName.Text))
        ListSaveFiles(folderPath)

        ShowTempMessage("File renamed successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' About -> help selection
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub mnuHelp_Click(sender As Object, e As EventArgs) Handles mnuHelp.Click
        HelpForm.Show()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MessageTimer_Tick(sender As Object, e As EventArgs) Handles MessageTimer.Tick
        StatusMessage.Visible = False
        MessageTimer.Stop()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Custom draw section to format the listview with bold headings
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles CustomFileList.DrawColumnHeader

        Dim boldFont As New Font(CustomFileList.Font, FontStyle.Bold)
        Dim headerHeight As Integer = e.Bounds.Height - 4   ' reduce height slightly
        Dim rect As New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, headerHeight)

        ' Background
        e.Graphics.FillRectangle(SystemBrushes.Control, rect)

        ' Text alignment
        Dim sf As New StringFormat()
        sf.LineAlignment = StringAlignment.Center
        sf.Alignment = StringAlignment.Near

        ' Add left padding so it lines up with column values
        Dim paddedRect As New Rectangle(rect.X + 4, rect.Y, rect.Width - 4, rect.Height)

        ' Draw bold header text
        e.Graphics.DrawString(e.Header.Text, boldFont, Brushes.Black, paddedRect, sf)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles CustomFileList.DrawItem
        e.DrawDefault = True
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles CustomFileList.DrawSubItem
        e.DrawDefault = True
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Sort the columns in the listview
    ' -----------------------------------------------------------------------------------------------------------

    Private lastColumn As Integer = -1
    Private lastOrder As SortOrder = SortOrder.Ascending

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub ListView1_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles CustomFileList.ColumnClick

        If e.Column = lastColumn Then
            lastOrder = If(lastOrder = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending)
        Else
            lastColumn = e.Column
            lastOrder = SortOrder.Ascending
        End If

        CustomFileList.ListViewItemSorter = New ListViewItemComparer(e.Column, lastOrder)
        CustomFileList.Sort()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Watch for changes to the TSW save file
    ' -----------------------------------------------------------------------------------------------------------

    Private WithEvents watcher As New IO.FileSystemWatcher()
    Private lastEvent As DateTime = DateTime.MinValue

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SetupWatcher()

        watcher.EnableRaisingEvents = False

        watcher.Path = CurrentFolder.Text
        watcher.Filter = SaveFileName.Text
        watcher.NotifyFilter = IO.NotifyFilters.LastWrite Or IO.NotifyFilters.FileName Or IO.NotifyFilters.Size

        watcher.EnableRaisingEvents = True

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub watcher_Changed(sender As Object, e As IO.FileSystemEventArgs) Handles watcher.Changed

        If IsMyWrite Or (DateTime.Now - lastEvent).TotalMilliseconds < 200 Then Exit Sub
        lastEvent = DateTime.Now

        If Me.InvokeRequired Then
            Me.Invoke(Sub()
                          UpdateUI(CurrentFolder.Text)
                      End Sub)
        Else
            UpdateUI(CurrentFolder.Text)
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
End Class
