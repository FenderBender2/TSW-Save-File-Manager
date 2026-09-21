' $Id: TSWSFM.vb 1494 2026-09-21 21:33:13Z Pete $
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
'Imports System.Windows.Forms.VisualStyles.VisualStyleElement
'Imports System.Windows.Forms.VisualStyles.VisualStyleElement
'Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class TSWSFM

    <DllImport("user32.dll")>
    Private Shared Function HideCaret(hWnd As IntPtr) As Boolean
    End Function

    Private Shared mutex As Mutex

    ' ===========================================================================================================
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

        GetSettings()
        GetVersions()

        VersionSelect.Focus()

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Hover events for tooltip display
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveFileName_MouseHover(sender As Object, e As EventArgs) Handles SaveFileName.MouseHover
        ShowTooltip(LongTextDisplay, sender)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SavedAsFileName_MouseHover(sender As Object, e As EventArgs) Handles SavedAsFileName.MouseHover
        ShowTooltip(LongTextDisplay, sender)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveLocation_MouseHover(sender As Object, e As EventArgs) Handles SaveLocation.MouseHover
        ShowTooltip(LongTextDisplay, sender)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Functions to support tooltip display
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub LongTextDisplay_Draw(sender As Object, e As DrawToolTipEventArgs) Handles LongTextDisplay.Draw
        DrawTooltip(e)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub LongTextDisplay_Popup(sender As Object, e As PopupEventArgs) Handles LongTextDisplay.Popup
        SetTooltipSize(sender, e)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Do not draw I-Beam for read only text boxes
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveFileName_MouseEnter(sender As Object, e As EventArgs) Handles SaveFileName.MouseEnter
        sender.Cursor = Cursors.Arrow
    End Sub

    Private Sub SaveFileName_MouseMove(sender As Object, e As MouseEventArgs) Handles SaveFileName.MouseMove
        sender.Cursor = Cursors.Arrow
    End Sub

    Private Sub SaveFileName_MouseLeave(sender As Object, e As EventArgs) Handles SaveFileName.MouseLeave
        sender.Cursor = Cursors.Default
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SavedAsFileName_MouseEnter(sender As Object, e As EventArgs) Handles SavedAsFileName.MouseEnter
        sender.Cursor = Cursors.Arrow
    End Sub

    Private Sub SavedAsFileName_MouseMove(sender As Object, e As MouseEventArgs) Handles SavedAsFileName.MouseMove
        sender.Cursor = Cursors.Arrow
    End Sub

    Private Sub SavedAsFileName_MouseLeave(sender As Object, e As EventArgs) Handles SavedAsFileName.MouseLeave
        sender.Cursor = Cursors.Default
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveDate_MouseEnter(sender As Object, e As EventArgs) Handles SaveDate.MouseEnter
        sender.Cursor = Cursors.Arrow
    End Sub

    Private Sub SaveDate_MouseMove(sender As Object, e As MouseEventArgs) Handles SaveDate.MouseMove
        sender.Cursor = Cursors.Arrow
    End Sub

    Private Sub Savedate_MouseLeave(sender As Object, e As EventArgs) Handles SaveDate.MouseLeave
        sender.Cursor = Cursors.Default
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveLocation_MouseEnter(sender As Object, e As EventArgs) Handles SaveLocation.MouseEnter
        sender.Cursor = Cursors.Arrow
    End Sub

    Private Sub SaveLocation_MouseMove(sender As Object, e As MouseEventArgs) Handles SaveLocation.MouseMove
        sender.Cursor = Cursors.Arrow
    End Sub

    Private Sub SaveLocation_MouseLeave(sender As Object, e As EventArgs) Handles SaveLocation.MouseLeave
        sender.Cursor = Cursors.Default
    End Sub

    ' ===========================================================================================================
    ' Select a TSW version from the drop-down
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub VersionSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles VersionSelect.SelectedIndexChanged
        SelectVersion(TSWIcon, sender, CurrentFolder)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Select a profile from the drop-down
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub ProfileSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ProfileSelect.SelectedIndexChanged
        SelectProfile(VersionSelect, FolderSelect, CurrentFolder.Text)
        SetupWatcher()
    End Sub

    ' ===========================================================================================================
    ' All buttons
    ' -----------------------------------------------------------------------------------------------------------
    ' Save button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        SaveFile(FolderSelect, NewFileName.Text, CurrentFolder.Text, SaveFileName.Text)
        NewFileName.Text = ""
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Run TSW button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RunButton_Click(sender As Object, e As EventArgs) Handles RunButton.Click
        RunGame()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Run TSC button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RunTSC_Click(sender As Object, e As EventArgs) Handles RunTSC.Click
        RunGame(True)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Restore button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RestoreButton_Click(sender As Object, e As EventArgs) Handles RestoreButton.Click
        RestoreFile(CustomFileList, FolderSelect, SaveFileName.Text, CurrentFolder.Text)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Delete button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
        DeleteFile(CustomFileList)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Close button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Rename button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RenameButton_Click(sender As Object, e As EventArgs) Handles RenameButton.Click
        RenameFile(CustomFileList, NewFileName.Text)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Move file button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MoveButton_Click(sender As Object, e As EventArgs) Handles MoveButton.Click
        FilesToMove(CustomFileList, FolderSelect)
    End Sub

    ' ===========================================================================================================
    ' Custom file listview events
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
        NewFileName.Text = sender.SelectedItems(0).SubItems(1).Text
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Right-click custom save file
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_MouseDown(sender As Object, e As MouseEventArgs) Handles CustomFileList.MouseDown
        If e.Button = MouseButtons.Right Then DisplayFileMenu(sender, e)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Custom draw section to format the listview with bold headings and sort arrows
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles CustomFileList.DrawColumnHeader
        DrawListHeaders(sender, e)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles CustomFileList.DrawSubItem
        DrawTick(sender, e)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Sort the columns in the listview
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles CustomFileList.ColumnClick
        ColumnHeaderClick(sender, e.Column)
        InvalidateListViewHeader(sender)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' 'Fix' the column widths
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles CustomFileList.ColumnWidthChanged
        If Not suppressColumnEvents Then sender.Columns(e.ColumnIndex).Width = customFixedWidths(e.ColumnIndex)
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
    Private Sub InvalidateListViewHeader(lv As ListView)
        Const LVM_GETHEADER As Integer = &H101F
        Dim headerHandle As IntPtr = SendMessage(lv.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)

        If headerHandle <> IntPtr.Zero Then InvalidateRect(headerHandle, IntPtr.Zero, True)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Tab drag and drop events
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CustomFileList_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles CustomFileList.ItemDrag
        Dim item As ListViewItem = CType(e.Item, ListViewItem)
        DoDragDrop(item, DragDropEffects.Move)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub FolderSelect_DragDrop(sender As Object, e As DragEventArgs) Handles FolderSelect.DragDrop
        Dim draggedItem As ListViewItem = CType(e.Data.GetData(GetType(ListViewItem)), ListViewItem)

        ' Find which tab was dropped onto
        Dim index As Integer = GetTabIndexAtPoint(sender, FolderSelect.PointToClient(New Point(e.X, e.Y)))
        If index >= 0 Then TabDragDrop(sender, index, draggedItem)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub FolderSelect_DragOver(sender As Object, e As DragEventArgs) Handles FolderSelect.DragOver
        TabDragOver(sender, sender.PointToClient(New Point(e.X, e.Y)), e)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub FolderSelect_DragLeave(sender As Object, e As EventArgs) Handles FolderSelect.DragLeave
        ShowTempMessage("")
    End Sub

    ' ===========================================================================================================
    ' Menu control
    ' -----------------------------------------------------------------------------------------------------------
    ' Menu Selections
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuHelp_Click(sender As Object, e As EventArgs) Handles MnuHelp.Click

        Dim hlpfrm As New HelpForm()

        With hlpfrm
            .StartPosition = FormStartPosition.Manual
            .Left = Me.Left + (Me.Width - .Width) \ 2
            .Top = Me.Top + (Me.Height - .Height) \ 2

            .ShowDialog()
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuNewTab_Click(sender As Object, e As EventArgs) Handles MnuNewTab.Click
        If TSWEnableFunctions Then CreateTab(FolderSelect)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuRenameTab_Click(sender As Object, e As EventArgs) Handles MnuRenameTab.Click
        If TSWEnableFunctions Then RenameTabs(FolderSelect)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuCopyTab_Click(sender As Object, e As EventArgs) Handles MnuCopyTab.Click
        If TSWEnableFunctions Then CopyTabs(FolderSelect)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuDeleteTab_Click(sender As Object, e As EventArgs) Handles MnuDeleteTab.Click
        If TSWEnableFunctions Then DeleteTabs(FolderSelect)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MnuRenameProfile_Click(sender As Object, e As EventArgs) Handles MnuRenameProfile.Click
        If TSWEnableFunctions Then RenameProfile(ProfileSelect)
    End Sub

    ' ===========================================================================================================
    ' Folder tab controls
    ' -----------------------------------------------------------------------------------------------------------
    ' Catch tab click event
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub FolderSelect_MouseDown(sender As Object, e As MouseEventArgs) Handles FolderSelect.MouseDown
        If Not e.Button = MouseButtons.Right AndAlso TSWEnableFunctions Then TabSelect(sender, e.Location)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Set order of tabs
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MenuCreationDate_Click(sender As Object, e As EventArgs) Handles MenuCreationDate.Click
        If TSWEnableFunctions Then ResetTabs(FolderSelect, "Date")
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MenuFileName_Click(sender As Object, e As EventArgs) Handles MenuFileName.Click
        If TSWEnableFunctions Then ResetTabs(FolderSelect, "File")
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Drawing options for status message and tabs
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HeadingLabel_GotFocus(sender As Object, e As EventArgs) Handles StatusMessage.GotFocus
        HideCaret(sender.Handle)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveFileName_GotFocus(sender As Object, e As EventArgs) Handles SaveFileName.GotFocus
        HideCaret(sender.Handle)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveDate_GotFocus(sender As Object, e As EventArgs) Handles SaveDate.GotFocus
        HideCaret(sender.Handle)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SavedAsFileName_GotFocus(sender As Object, e As EventArgs) Handles SavedAsFileName.GotFocus
        HideCaret(sender.Handle)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SaveLocation_GotFocus(sender As Object, e As EventArgs) Handles SaveLocation.GotFocus
        HideCaret(sender.Handle)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub FolderSelect_DrawItem(sender As Object, e As DrawItemEventArgs) Handles FolderSelect.DrawItem
        DrawTab(sender, e)
    End Sub

    ' ===========================================================================================================
    ' Right-click context menus for tabs & files
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub FolderSelect_MouseUp(sender As Object, e As MouseEventArgs) Handles FolderSelect.MouseUp
        If e.Button = MouseButtons.Right AndAlso TSWEnableFunctions Then TabRightClick(sender, e.Location)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub RenameTabMenuItem_Click(sender As Object, e As EventArgs) Handles RenameTabMenuItem.Click
        If rightClickedTabIndex > 0 AndAlso rightClickedTabIndex < FolderSelect.TabPages.Count Then RenameTabs(FolderSelect)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub DeleteTabMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteTabMenuItem.Click
        If rightClickedTabIndex > 0 AndAlso rightClickedTabIndex < FolderSelect.TabPages.Count Then DeleteTabs(FolderSelect)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub DeleteFileMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteFileMenuItem.Click
        DeleteFile(CustomFileList)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MoveFileMenuItem_Click(sender As Object, e As EventArgs) Handles MoveFileMenuItem.Click
        FilesToMove(CustomFileList, FolderSelect)
    End Sub

    ' ===========================================================================================================
    ' Fade status messages when timer expires
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MessageTimer_Tick(sender As Object, e As EventArgs) Handles MessageTimer.Tick
        sender.Stop()
        FadeTimer.Start()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub FadeTimer_Tick(sender As Object, e As EventArgs) Handles FadeTimer.Tick
        FadeColours(sender)
    End Sub

    ' ===========================================================================================================
    ' Watch for changes to the TSW save file
    ' -----------------------------------------------------------------------------------------------------------

    Private WithEvents Watcher As New FileSystemWatcher()
    Private lastEvent As DateTime = DateTime.MinValue

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub SetupWatcher()

        With watcher
            .EnableRaisingEvents = False

            .Path = CurrentFolder.Text
            .Filter = SaveFileName.Text & If(Not Path.HasExtension(SaveFileName.Text), ".sav", "")
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

    ' ----------------------------------------------------------------------------------------------------------
End Class
