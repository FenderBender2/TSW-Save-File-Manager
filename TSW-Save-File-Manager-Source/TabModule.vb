' $Id: TabModule.vb 1494 2026-09-21 21:33:13Z Pete $
Imports System.IO
Imports System.Xml.Xsl

Module TabModule

    ' -----------------------------------------------------------------------------------------------------------
    ' Set up tabs using subfolders within the custom save file folder
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub GetAllTabs(fs As TabControl)

        Dim lastOrder = My.Settings.lastTabOrder

        fs.Visible = False
        fs.TabPages(0).Select()

        While fs.TabPages.Count > 1
            fs.TabPages.RemoveAt(1)
        End While

        For Each folder As String In Directory.GetDirectories(TSWCurrentProfilePath).OrderBy(Function(f) If(lastOrder = "Date", Directory.GetCreationTime(f), Path.GetFileName(f)))
            Dim tp As New TabPage(Path.GetFileName(folder)) With {.BackColor = Color.White}
            fs.TabPages.Add(tp)
        Next

        fs.Visible = True
        TSWSFM.MenuFileName.Checked = lastOrder <> "Date"
        TSWSFM.MenuCreationDate.Checked = lastOrder = "Date"

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Create a new tab
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub CreateTab(fs As TabControl)

        Dim name As String = ""
        Dim cleanName As String = ""

        Do Until name <> ""
            name = TSWInputBox("New")
            If name = "" Then Exit Sub

            cleanName = String.Concat(name.Where(Function(c) Not Path.GetInvalidFileNameChars.Contains(c)))
            cleanName = Path.GetFileNameWithoutExtension(cleanName)

            Dim folderPath As String = Path.Combine(TSWCurrentProfilePath, cleanName)

            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            Else
                MessageBox.Show($"The {cleanName} folder already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                name = ""
            End If
        Loop

        Dim tp As New TabPage(cleanName) With {.BackColor = Color.White}

        fs.TabPages.Add(tp)
        fs.SelectedTab = tp

        ListSaveFiles(Path.Combine(TSWCurrentProfilePath, tp.Text))
        ShowTempMessage($"New folder {tp.Text} created successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Rename a custom tab
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub RenameTabs(fs As TabControl)

        Dim tabindex = fs.SelectedIndex

        If tabindex = 0 Then
            MessageBox.Show("The Main folder cannot be renamed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim oldName = fs.TabPages(tabindex).Text
        Dim name As String = ""
        Dim cleanName As String = ""
        Dim sourceFolder As String = ""
        Dim targetFolder As String = ""

        Do Until name <> ""
            name = TSWInputBox("Rename", oldName)
            If name = "" Then Exit Sub

            cleanName = String.Concat(name.Where(Function(c) Not Path.GetInvalidFileNameChars.Contains(c)))
            cleanName = Path.GetFileNameWithoutExtension(cleanName)

            If cleanName = oldName Then Exit Sub

            sourceFolder = Path.Combine(TSWCurrentProfilePath, oldName)
            targetFolder = Path.Combine(TSWCurrentProfilePath, cleanName)

            If Directory.Exists(targetFolder) Then
                MessageBox.Show($"The {cleanName} folder already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                name = ""
            End If
        Loop

        fs.TabPages(tabindex).Text = cleanName
        Directory.Move(sourceFolder, targetFolder)
        ShowTempMessage($"Folder {oldName} renamed to {cleanName} successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Copy a custom tab to another version
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub CopyTabs(fs As TabControl)

        Dim selectedName = TSWSFM.VersionSelect.SelectedItem.ToString
        Dim selectedAppID As Integer = Integer.Parse(TSWVersions.First(Function(v) v.Name = selectedName).AppID)
        Dim found As Boolean = False

        For Each v In TSWVersions
            If v.AppID > selectedAppID Then
                found = True
                Exit For
            End If
        Next

        If Not found Then
            MessageBox.Show($"{selectedName} is the latest installed version.", "Copy Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim currFolder = fs.SelectedTab.Text
        Dim name As String = ""

        Do Until name <> ""
            name = TSWInputBox("Copy", currFolder)
            If name = "" Then Exit Sub

            Dim versionID = name.Split(New Char() {"|"c})
            Dim profileID As String = ""

            For i As Integer = 0 To copyProfileArray.GetLength(0) - 1
                If copyProfileArray(i, 1) = versionID(1) Then
                    profileID = copyProfileArray(i, 0)
                    Exit For
                End If
            Next

            Dim sourceFolder = Path.Combine(TSWCurrentProfilePath, If(currFolder = "Main", "", currFolder))
            Dim targetfolder = Path.Combine(GetSaveFolder(versionID(0)), TSWSAVEFOLDER, "Profile" & profileID, If(currFolder = "Main", "", currFolder))

            If Directory.Exists(targetfolder) Then
                If MessageBox.Show("The target folder already exists. If there are any duplicate file names, the copied files will be renamed. " &
                                   "e.g. Save file -> Save file (1)"" etc. Do you want to continue?",
                                   "Copy Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = vbNo Then Exit Sub
            Else
                Directory.CreateDirectory(targetfolder)
            End If

            Dim saveFiles = Directory.GetFiles(sourceFolder, "*.sav")

            For Each f In saveFiles
                Dim newName = GetUniqueFilename(Path.Combine(targetfolder, Path.GetFileName(f)))
                File.Copy(f, newName)
            Next

            ShowTempMessage($"Files copied to {versionID(0)} (profile {versionID(1)})")
        Loop

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Delete a custom tab
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DeleteTabs(fs As TabControl)

        Dim tabindex = fs.SelectedIndex
        Dim filesMoved As Boolean = False
        Dim tabName = fs.TabPages(tabindex).Text

        If tabindex = 0 Then
            MessageBox.Show("The Main folder cannot be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim folderPath As String = Path.Combine(TSWCurrentProfilePath, tabName)
        If Not Directory.Exists(folderPath) Then Exit Sub

        Dim msg = $"Are you sure you want to delete the {tabName} folder?"

        If Directory.GetFiles(folderPath, "*.sav", SearchOption.AllDirectories).Length > 0 Then

            msg = $"The {tabName} folder contains save files! Do you want to move these to the Main folder before deleting?"
            Dim response = MessageBox.Show(msg, "Delete Folder", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)

            If response = DialogResult.Cancel Then
                Exit Sub
            ElseIf response = DialogResult.Yes Then
                MoveFiles(folderPath, TSWCurrentProfilePath)
                filesMoved = True
            End If

        ElseIf MessageBox.Show(msg, "Delete Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        fs.TabPages.Remove(fs.TabPages(tabindex))
        FileIO.FileSystem.DeleteDirectory(folderPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        ShowTempMessage($"Folder {tabName} deleted {If(filesMoved, "and files moved to the Main folder", "and moved to the recycle bin")}")

        tabName = fs.SelectedTab.Text
        ListSaveFiles(Path.Combine(TSWCurrentProfilePath, If(tabName = "Main", "", tabName)))

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Set order of tabs
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ResetTabs(fs As TabControl, orderName As String)

        My.Settings.lastTabOrder = orderName
        My.Settings.Save()

        GetAllTabs(fs)

        Dim currentTab = fs.SelectedTab.Text
        ListSaveFiles(Path.Combine(TSWCurrentProfilePath, If(currentTab = "Main", "", currentTab)))

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Tab left clicked
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub TabSelect(fs As TabControl, location As Point)

        For i As Integer = 0 To fs.TabPages.Count - 1
            Dim r As Rectangle = fs.GetTabRect(i)

            If r.Contains(location) Then
                Dim clickedTab As TabPage = fs.TabPages(i)
                Dim tabText As String = clickedTab.Text

                ListSaveFiles(Path.Combine(TSWCurrentProfilePath, If(tabText = "Main", "", tabText)))
            End If
        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Tab right clicked
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub TabRightClick(fs As TabControl, location As Point)

        For i As Integer = 0 To fs.TabPages.Count - 1

            If fs.GetTabRect(i).Contains(location) Then

                If i = 0 Then Exit Sub
                rightClickedTabIndex = i
                TSWSFM.TabMenu.Show(fs, location)
                Exit Sub

            End If
        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Return the currently selected tab name
    ' -----------------------------------------------------------------------------------------------------------
    Public Function GetTab()
        Dim selectedTab = TSWSFM.FolderSelect.SelectedTab.Text
        Return If(selectedTab = "Main", "", selectedTab)
    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Enable drag and drop from listview to tabs
    ' -----------------------------------------------------------------------------------------------------------
    Public Function GetTabIndexAtPoint(tc As TabControl, pt As Point) As Integer

        For i As Integer = 0 To tc.TabPages.Count - 1
            Dim r As Rectangle = tc.GetTabRect(i)
            If r.Contains(pt) Then Return i
        Next

        Return -1

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Control for dragging files to tabs
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub TabDragDrop(tc As TabControl, tabIndex As Integer, dragItem As ListViewItem)

        Dim targetTab As TabPage = tc.TabPages(tabIndex)
        Dim fileName As String = dragItem.SubItems(1).Text
        Dim currentTab = tc.SelectedTab.Text
        Dim newTab As String = targetTab.Text

        ShowTempMessage("")
        If newTab = currentTab Then Exit Sub

        Dim sourcefolder = Path.Combine(TSWCurrentProfilePath, If(currentTab = "Main", "", currentTab))
        Dim targetFolder = Path.Combine(TSWCurrentProfilePath, If(newTab = "Main", "", newTab))
        Dim targetFile = fileName & If(Not Path.HasExtension(fileName), ".sav", "")

        If MoveFiles(sourcefolder, targetFolder, targetFile) Then
            ListSaveFiles(sourcefolder)
            ShowTempMessage($"{fileName} moved to {newTab}")
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Display message when dragover a tab
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub TabDragOver(tc As TabControl, pt As Point, e As DragEventArgs)

        For i As Integer = 0 To tc.TabPages.Count - 1
            Dim r As Rectangle = tc.GetTabRect(i)

            If r.Contains(pt) AndAlso tc.TabPages(i).Text <> tc.SelectedTab.Text Then
                e.Effect = DragDropEffects.Move
                ShowTempMessage($"Folder: {tc.TabPages(i).Text}")
                Exit For
            Else
                e.Effect = DragDropEffects.None
                ShowTempMessage("")
            End If
        Next

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Draw tabs with custom attributes
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DrawTab(sender As Object, e As DrawItemEventArgs)

        Dim tc As TabControl = DirectCast(sender, TabControl)
        Dim tp As TabPage = tc.TabPages(e.Index)

        Dim text As String = tp.Text

        ' Copy the bounds so we can adjust them
        Dim r As Rectangle = e.Bounds

        r.Y += If(e.Index = tc.SelectedIndex, 0, 2)   ' unselected tabs sits slightly lower

        ' Centre alignment
        Dim sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}

        Dim selectedColour As Color = ColorTranslator.FromHtml("#005500")   ' dark green 
        Dim unselectedColour As Color = ColorTranslator.FromHtml("#7E7E7E") ' grey

        ' Colour depending on selection
        Dim fore As Brush = New SolidBrush(If(e.Index = tc.SelectedIndex, selectedColour, unselectedColour))

        e.Graphics.DrawString(text, tc.Font, fore, r, sf)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
End Module
