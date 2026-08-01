Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class TSWSFM

    Dim TSWappId As Integer
    Private Shared mutex As Mutex
    Private RightClickedTabIndex As Integer = -1

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

        With Me
            .FormBorderStyle = FormBorderStyle.FixedDialog
            .MaximizeBox = False
            .MinimizeBox = False
        End With

        GetSettings()
        GetVersions()

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Save button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        If NewFileName.Text = "" Then Exit Sub

        Dim tabtext = TabControl.SelectedTab.Text
        Dim cleanStr = String.Concat(NewFileName.Text.Where(Function(c) Not Path.GetInvalidFileNameChars.Contains(c)))
        Dim sourceFile = Path.Combine(CurrentFolder.Text, SaveFileName.Text)
        Dim destFile = Path.Combine(CurrentFolder.Text, TSWSaveFolder, If(tabtext = "Main", "", tabtext), cleanStr)

        If Not Path.HasExtension(destFile) Then destFile &= ".sav"

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

        ShowTempMessage(Path.GetFileName(destFile) & " saved successfully")

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

        If CustomFileList.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a custom save file.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim filename = CustomFileList.SelectedItems(0).SubItems(1).Text
        Dim folderPath As String = Path.Combine(CurrentFolder.Text, TSWSaveFolder, If(TabControl.SelectedTab.Text = "Main", "", TabControl.SelectedTab.Text))
        Dim targetFolder As String = CurrentFolder.Text
        Dim sourceFile As String = Path.Combine(folderPath, filename)
        Dim destFile As String = Path.Combine(targetFolder, SaveFileName.Text)

        If File.Exists(destFile) Then
            Dim msgstring = If(TSWcurrentIsSaved, "Are you sure you want to overwrite the TSW save file", "Warning: The current TSW save file has not been saved to your custom list. Continue to overwrite")
            msgstring &= " with " & filename & "?"

            Dim boxIcon = If(TSWcurrentIsSaved, MessageBoxIcon.Question, MessageBoxIcon.Warning)
            Dim response = MessageBox.Show(msgstring, If(Not TSWcurrentIsSaved, "Warning: ", "") & "Confirm Overwrite", MessageBoxButtons.YesNo, boxIcon)

            If response = DialogResult.No Then
                ShowTempMessage("Restore cancelled")
                Exit Sub
            End If
        End If

        IsMyWrite = True
        File.Copy(sourceFile, destFile, True)
        UpdateUI(CurrentFolder.Text)
        IsMyWrite = False

        ShowTempMessage(filename & " restored successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Delete button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        If CustomFileList.SelectedItems.Count = 0 Then
            MessageBox.Show("No file selected.", "Delete File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim fileName = CustomFileList.SelectedItems(0).SubItems(1).Text
        Dim folderPath As String = Path.Combine(CurrentFolder.Text, TSWSaveFolder, GetTab)
        Dim sourceFile As String = Path.Combine(folderPath, fileName)

        If File.Exists(sourceFile) Then
            Dim response = MessageBox.Show("Are you sure you want to delete " & Path.GetFileNameWithoutExtension(fileName) & "? Deleted files are sent to the recycle bin.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If response <> DialogResult.Yes Then Exit Sub
        End If

        FileIO.FileSystem.DeleteFile(sourceFile, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        ListSaveFiles(folderPath)

        ShowTempMessage(fileName & " deleted and saved in Recycle Bin")

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
    ' Trap selection of a custom save file
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_Click(sender As Object, e As EventArgs) Handles CustomFileList.Click
        NewFileName.Text = CustomFileList.SelectedItems(0).SubItems(1).Text
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

        ' Create version save folder parent if it doesnt exist
        Dim folderPath As String = Path.Combine(CurrentFolder.Text, TSWSaveFolder) ', GetTab)

        If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

        ListSaveFiles(folderPath)
        SetupWatcher()
        LoadTSWIcon(TSWVersions(VersionSelect.SelectedIndex))
        GetAllTabs()
        NewFileName.Text = ""

        With My.Settings
            .lastVersion = selectedName
            .Save()
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Rename button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RenameButton_Click(sender As Object, e As EventArgs) Handles RenameButton.Click

        If NewFileName.Text = "" Or CustomFileList.SelectedItems.Count = 0 Then Exit Sub

        Dim fileName = CustomFileList.SelectedItems(0).SubItems(1).Text
        Dim newName = NewFileName.Text
        Dim folderPath As String = Path.Combine(CurrentFolder.Text, TSWSaveFolder, GetTab)
        Dim sourceFile As String = Path.Combine(folderPath, fileName)

        If Path.Combine(folderPath, newName) = sourceFile Then Exit Sub

        Dim msg = "Are you sure you want to rename " & fileName & " to " & newName & "?"
        If File.Exists(sourceFile) AndAlso MessageBox.Show(msg, "Confirm Rename", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub

        File.Move(Path.Combine(folderPath, fileName), Path.Combine(folderPath, newName))
        ListSaveFiles(folderPath)
        ShowTempMessage(fileName & " renamed to " & newName)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Move file button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MoveButton_Click(sender As Object, e As EventArgs) Handles MoveButton.Click

        If CustomFileList.SelectedItems.Count = 0 Then
            MessageBox.Show("No file selected.", "Move File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If Me.TabControl.TabCount < 2 Then
            MessageBox.Show("There is only one folder available.", "Move File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim fileName = CustomFileList.SelectedItems(0).SubItems(1).Text
        Dim name = TSWInputBox("Move", fileName)

        If name = "" Then Exit Sub

        Dim currentTab = TabControl.SelectedTab.Text
        Dim sourcefolder = Path.Combine(CurrentFolder.Text, TSWSaveFolder, If(currentTab = "Main", "", currentTab))
        Dim targetFolder = Path.Combine(CurrentFolder.Text, TSWSaveFolder, If(name = "Main", "", name))

        If MoveFiles(sourcefolder, targetFolder, fileName) Then
            ListSaveFiles(sourcefolder)
            ShowTempMessage(Path.GetFileNameWithoutExtension(fileName) & " moved successfully to " & name)
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Menu Selections
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuHelp_Click(sender As Object, e As EventArgs) Handles MnuHelp.Click

        Dim hlpfrm As New HelpForm()

        With hlpfrm
            .StartPosition = FormStartPosition.Manual
            .Left = Me.Left + (Me.Width - .Width) \ 2
            .Top = Me.Top + (Me.Height - .Height) \ 2

            .Show()
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuNewTab_Click(sender As Object, e As EventArgs) Handles MnuNewTab.Click

        Dim name = TSWInputBox("New")
        If name = "" Then Exit Sub

        Dim cleanName = String.Concat(name.Where(Function(c) Not Path.GetInvalidFileNameChars.Contains(c)))
        Dim folderPath As String = Path.Combine(CurrentFolder.Text, TSWSaveFolder, cleanName)

        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        Else
            MessageBox.Show("The " & cleanName & " folder already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim tp As New TabPage(cleanName)

        tp.BackColor = Color.White
        TabControl.TabPages.Add(tp)
        TabControl.SelectedTab = tp
        ListSaveFiles(Path.Combine(CurrentFolder.Text, TSWSaveFolder, tp.Text))
        ShowTempMessage("New folder " & tp.Text & " created successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuRenameTab_Click(sender As Object, e As EventArgs) Handles MnuRenameTab.Click
        RenameTabs(TabControl.SelectedIndex)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuDeleteTab_Click(sender As Object, e As EventArgs) Handles MnuDeleteTab.Click
        DeleteTabs(TabControl.SelectedIndex)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Catch tab click event
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub TabControl_MouseDown(sender As Object, e As MouseEventArgs) Handles TabControl.MouseDown

        If e.Button = MouseButtons.Right Then Exit Sub

        Dim tc = TabControl

        For i As Integer = 0 To tc.TabPages.Count - 1
            Dim r As Rectangle = tc.GetTabRect(i)

            If r.Contains(e.Location) Then
                Dim clickedTab As TabPage = tc.TabPages(i)
                Dim tabText As String = clickedTab.Text

                ListSaveFiles(Path.Combine(CurrentFolder.Text, TSWSaveFolder, If(tabText = "Main", "", tabText)))
            End If
        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Custom draw section to format the listview with bold headings and sort arrows
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles CustomFileList.DrawColumnHeader

        Dim headerHeight As Integer = e.Bounds.Height - 4
        Dim rect As New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, headerHeight)

        Using bgBrush As New SolidBrush(Color.FromArgb(240, 240, 240)) ' light grey
            e.Graphics.FillRectangle(bgBrush, rect)
        End Using

        ' --- Draw bold text, left aligned ---
        Dim boldFont As New Font(CustomFileList.Font, FontStyle.Bold)

        Dim sf As New StringFormat()
        sf.LineAlignment = StringAlignment.Near
        ' sf.Alignment = StringAlignment.Near

        Dim paddedRect As New Rectangle(rect.X + 4, rect.Y, rect.Width - 4, rect.Height)

        e.Graphics.DrawString(e.Header.Text, boldFont, Brushes.Black, paddedRect, sf)

        ' --- Draw arrow on sorted column ---
        If e.ColumnIndex = lastColumn Then

            Dim arrowImg As Image = imgHeaderArrows.Images(If(lastOrder = SortOrder.Ascending, 0, 1))

            Dim scale As Single = 0.5F   ' 50% size
            Dim newW As Integer = CInt(arrowImg.Width * (scale + 0.1))
            Dim newH As Integer = CInt(arrowImg.Height * (scale - 0.1))

            Dim x As Integer = rect.Right - newW - 4
            Dim y As Integer = rect.Top + (rect.Height - newH) \ 2

            e.Graphics.DrawImage(arrowImg, New Rectangle(x, y, newW, newH))

        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles CustomFileList.DrawSubItem

        If e.ColumnIndex = 0 Then
            Dim text As String = e.SubItem.Text

            If text <> "" Then
                Dim flags As TextFormatFlags = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine
                Dim textColor As Color = Color.DarkRed

                TextRenderer.DrawText(e.Graphics, text, CustomFileList.Font, e.Bounds, textColor, flags)
            End If

            Return
        End If

        ' Default drawing for other columns
        e.DrawDefault = True

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Sort the columns in the listview
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles CustomFileList.ColumnClick

        If e.Column < 1 Then
            Exit Sub
        ElseIf e.Column = lastColumn Then
            lastOrder = If(lastOrder = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending)
        Else
            lastColumn = e.Column
            lastOrder = SortOrder.Ascending
        End If

        With CustomFileList
            .ListViewItemSorter = New ListViewItemComparer(e.Column, lastOrder)
            .Sort()
        End With

        InvalidateListViewHeader(CustomFileList)

        With CustomFileList
            Dim headerRect As Rectangle = New Rectangle(0, 0, .Width, .Font.Height + 8)
            .Invalidate(headerRect)
        End With

        With My.Settings
            .lastSortColumn = lastColumn
            .lastSortOrder = lastOrder.ToString()
            .Save()
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Watch for changes to the TSW save file
    ' -----------------------------------------------------------------------------------------------------------

    Private WithEvents watcher As New FileSystemWatcher()
    Private lastEvent As DateTime = DateTime.MinValue

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SetupWatcher()

        With watcher
            .EnableRaisingEvents = False

            .Path = CurrentFolder.Text
            .Filter = SaveFileName.Text
            .NotifyFilter = NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.Size

            .EnableRaisingEvents = True
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub Watcher_Changed(sender As Object, e As FileSystemEventArgs) Handles watcher.Changed

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
    ' Set handlers to erase arrows from columns not sorted
    ' -----------------------------------------------------------------------------------------------------------

    <DllImport("user32.dll")>
    Private Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function InvalidateRect(hWnd As IntPtr, rect As IntPtr, eraseFlag As Boolean) As Boolean
    End Function

    ' -----------------------------------------------------------------------------------------------------------
    Public Sub InvalidateListViewHeader(lv As ListView)
        Const LVM_GETHEADER As Integer = &H101F
        Dim headerHandle As IntPtr = SendMessage(lv.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)

        If headerHandle <> IntPtr.Zero Then InvalidateRect(headerHandle, IntPtr.Zero, True)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Enable drag and drop from listview to tabs
    ' -----------------------------------------------------------------------------------------------------------

    Private Sub CustomFileList_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles CustomFileList.ItemDrag
        Dim item As ListViewItem = CType(e.Item, ListViewItem)
        DoDragDrop(item, DragDropEffects.Move)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub TabControl_DragEnter(sender As Object, e As DragEventArgs) Handles TabControl.DragEnter
        If e.Data.GetDataPresent(GetType(ListViewItem)) Then e.Effect = DragDropEffects.Move
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub TabControl_DragDrop(sender As Object, e As DragEventArgs) Handles TabControl.DragDrop

        Dim draggedItem As ListViewItem = CType(e.Data.GetData(GetType(ListViewItem)), ListViewItem)

        ' Find which tab was dropped onto
        Dim pt As Point = TabControl.PointToClient(New Point(e.X, e.Y))
        Dim index As Integer = GetTabIndexAtPoint(TabControl, pt)

        If index < 0 Then Exit Sub

        Dim targetTab As TabPage = TabControl.TabPages(index)
        Dim fileName As String = draggedItem.SubItems(1).Text
        Dim currentTab = TabControl.SelectedTab.Text
        Dim newTab As String = targetTab.Text

        If newTab = currentTab Then Exit Sub

        Dim sourcefolder = Path.Combine(CurrentFolder.Text, TSWSaveFolder, If(currentTab = "Main", "", currentTab))
        Dim targetFolder = Path.Combine(CurrentFolder.Text, TSWSaveFolder, If(newTab = "Main", "", newTab))

        If MoveFiles(sourcefolder, targetFolder, fileName) Then
            ListSaveFiles(sourcefolder)
            ShowTempMessage(Path.GetFileNameWithoutExtension(fileName) & " moved to " & newTab)
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Function GetTabIndexAtPoint(tc As TabControl, pt As Point) As Integer

        For i As Integer = 0 To tc.TabPages.Count - 1
            Dim r As Rectangle = tc.GetTabRect(i)
            If r.Contains(pt) Then Return i
        Next

        Return -1

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Drawing options for status message and tabs
    ' -----------------------------------------------------------------------------------------------------------

    <DllImport("user32.dll")>
    Private Shared Function HideCaret(hWnd As IntPtr) As Boolean
    End Function

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HeadingLabel_GotFocus(sender As Object, e As EventArgs) Handles StatusMessage.GotFocus
        HideCaret(StatusMessage.Handle)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub TabControl_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl.DrawItem

        Dim tc As TabControl = DirectCast(sender, TabControl)
        Dim tp As TabPage = tc.TabPages(e.Index)

        Dim text As String = tp.Text

        ' Copy the bounds so we can adjust them
        Dim r As Rectangle = e.Bounds

        r.Y += If(e.Index = tc.SelectedIndex, 0, 2)   ' unselected tabs sit slightly lower

        ' Centre alignment
        Dim sf As New StringFormat With {
        .Alignment = StringAlignment.Center,
        .LineAlignment = StringAlignment.Center
    }

        Dim selectedColour As Color = ColorTranslator.FromHtml("#005500") ' dark green 
        Dim unselectedColour As Color = ColorTranslator.FromHtml("#7E7E7E") ' grey

        ' Colour depending on selection
        Dim fore As Brush = New SolidBrush(If(e.Index = tc.SelectedIndex, selectedColour, unselectedColour))

        e.Graphics.DrawString(text, tc.Font, fore, r, sf)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Right-click context menu for tabs
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub TabControl_MouseUp(sender As Object, e As MouseEventArgs) Handles TabControl.MouseUp
        If e.Button = MouseButtons.Right Then
            For i As Integer = 0 To TabControl.TabPages.Count - 1

                If TabControl.GetTabRect(i).Contains(e.Location) Then

                    If i = 0 Then Exit Sub
                    RightClickedTabIndex = i
                    TabMenu.Show(TabControl, e.Location)
                    Exit Sub

                End If
            Next
        End If
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RenameTab_Click(sender As Object, e As EventArgs) Handles RenameTab.Click
        If RightClickedTabIndex > 0 AndAlso RightClickedTabIndex < TabControl.TabPages.Count Then RenameTabs(RightClickedTabIndex)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub DeleteTab_Click(sender As Object, e As EventArgs) Handles DeleteTab.Click
        If RightClickedTabIndex > 0 AndAlso RightClickedTabIndex < TabControl.TabPages.Count Then DeleteTabs(RightClickedTabIndex)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Set order of tabs
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MenuCreationDate_Click(sender As Object, e As EventArgs) Handles MenuCreationDate.Click
        ResetTabs("Date")
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MenuFileName_Click(sender As Object, e As EventArgs) Handles MenuFileName.Click
        ResetTabs("File")
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Fade status messages when timer expires
    ' -----------------------------------------------------------------------------------------------------------
    Private Function MoveChannel(current As Integer, target As Integer) As Integer

        Dim stepSize As Integer = Math.Max(1, Math.Abs(current - target) \ 3)

        If current < target Then
            Return Math.Min(target, current + stepSize)
        ElseIf current > target Then
            Return Math.Max(target, current - stepSize)
        Else
            Return current
        End If

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MessageTimer_Tick(sender As Object, e As EventArgs) Handles MessageTimer.Tick
        MessageTimer.Stop()
        FadeTimer.Start()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub FadeTimer_Tick(sender As Object, e As EventArgs) Handles FadeTimer.Tick

        Dim r = MoveChannel(CurrentColour.R, EndColour.R)
        Dim g = MoveChannel(CurrentColour.G, EndColour.G)
        Dim b = MoveChannel(CurrentColour.B, EndColour.B)

        CurrentColour = Color.FromArgb(r, g, b)
        StatusMessage.ForeColor = CurrentColour

        ' Stop when close enough to the target colour
        If r = EndColour.R AndAlso g = EndColour.G AndAlso b = EndColour.B Then
            FadeTimer.Stop()
            StatusMessage.Visible = False
            StatusMessage.ForeColor = StartColour
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
End Class
