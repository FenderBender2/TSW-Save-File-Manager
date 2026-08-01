Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Module TSWSaveFileManager

    Public Const TSWSaveFolder As String = "Custom Saves"
    Const TSWSaveFileName As String = "TSWSaveGame_*.sav"
    Const TSWTitle As String = "Train Sim World"

    Public ReadOnly TSWSteamRoot As String = GetSteamRoot()
    Public TSWVersions As New List(Of (Name As String, Location As String, AppID As Integer))
    Public TSWcurrentIsSaved As Boolean
    Public IsMyWrite As Boolean = False
    Public lastColumn As Integer
    Public lastOrder As SortOrder
    Public lastVersion As String

    Public arrowUp As Bitmap
    Public arrowDown As Bitmap

    ' -----------------------------------------------------------------------------------------------------------
    ' Find the installation location of Steam applicatons
    ' -----------------------------------------------------------------------------------------------------------
    Private Function GetSteamRoot() As String

        Dim key = Registry.CurrentUser.OpenSubKey("Software\Valve\Steam")

        If key Is Nothing Then
            MessageBox.Show("Steam parent key not found in registry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If

        Dim steamPath = key.GetValue("SteamPath")?.ToString()

        If steamPath Is Nothing Then
            MessageBox.Show("Steam path not found in registry key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If

        Return steamPath

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Get the details of the version
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub GetVersionDetails(lineText As String, folderPath As String)

        Dim dirMatch = Regex.Match(lineText, """installdir""\s+""([^""]+)""")

        If dirMatch.Success Then

            Dim appIdMatch = Regex.Match(lineText, """appid""\s*""(\d+)""") ' TSW app ID
            Dim appId As Integer = Integer.Parse(appIdMatch.Groups(1).Value)
            Dim installDir = dirMatch.Groups(1).Value
            Dim fullPath = Path.Combine(folderPath, "common", installDir)

            TSWVersions.Add((Path.GetFileName(fullPath), fullPath, appId))

        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Find if the current save file is saved anywhere
    ' -----------------------------------------------------------------------------------------------------------
    Private Function FindSaveFile(parentFolder As String, targetTime As String) As String

        TSWcurrentIsSaved = False

        ' Loop through all files in this folder
        For Each fileName As String In Directory.GetFiles(parentFolder)
            If File.GetLastWriteTime(fileName).ToString("yyyyMMddhhmmss") = targetTime Then
                TSWcurrentIsSaved = True
                Dim folderName = Path.GetFileName(parentFolder)

                Return If(folderName = TSWSaveFolder, "Main", folderName)
            End If
        Next

        ' Loop through subfolders (recursive)
        For Each subFolder As String In Directory.GetDirectories(parentFolder)
            Dim result As String = FindSaveFile(subFolder, targetTime)

            If result <> "Not Saved" Then
                TSWcurrentIsSaved = True
                Return result
            End If
        Next

        Return "Not Saved"

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Generate unique filenames
    ' -----------------------------------------------------------------------------------------------------------
    Public Function GetUniqueFilename(basePath As String) As String

        Dim folder As String = Path.GetDirectoryName(basePath)
        Dim name As String = Path.GetFileNameWithoutExtension(basePath)
        Dim ext As String = Path.GetExtension(basePath)

        Dim candidate As String = basePath
        Dim counter As Integer = 1

        While File.Exists(candidate)
            candidate = Path.Combine(folder, $"{name} ({counter}){ext}")
            counter += 1
        End While

        Return candidate

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Get saved settings if they exist
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub GetSettings()

        IsMyWrite = False

        If String.IsNullOrEmpty(My.Settings.lastSortOrder) Or String.IsNullOrEmpty(My.Settings.lastSortColumn) Then
            lastColumn = 1
            lastOrder = SortOrder.Ascending
        Else
            lastColumn = My.Settings.lastSortColumn
            lastOrder = If(My.Settings.lastSortOrder = "Ascending", SortOrder.Ascending, SortOrder.Descending)
        End If

        If String.IsNullOrEmpty(My.Settings.lastTabOrder) Then My.Settings.lastTabOrder = "Date"

        Dim asm = Reflection.Assembly.GetExecutingAssembly()

        arrowUp = CType(Bitmap.FromStream(asm.GetManifestResourceStream("TSW_Save_Game_Manager.ArrowUp.png")), Bitmap)
        arrowDown = CType(Bitmap.FromStream(asm.GetManifestResourceStream("TSW_Save_Game_Manager.ArrowDown.png")), Bitmap)

        ' Set direction arrows for list sort
        With TSWSFM
            With .imgHeaderArrows
                .Images.Clear()
                .Images.Add(arrowUp)
                .Images.Add(arrowDown)
            End With

            With .CustomFileList
                .OwnerDraw = True

                ' Set columns in list item
                With .Columns
                    .Clear()
                    .Add("", 19, HorizontalAlignment.Right)
                    .Add("Filename", 346)
                    .Add("Save Date", 100)
                    .Add("FileTimestamp", 0)
                End With
            End With
        End With

        TSWSFM.TabControl.DrawMode = TabDrawMode.OwnerDrawFixed

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Populate the custom save file list
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ListSaveFiles(folderpath As String)

        If folderpath = TSWSaveFolder Or TSWSFM.CurrentFolder.Text = "" Then Exit Sub

        Dim files = Directory.GetFiles(folderpath)
        TSWSFM.CustomFileList.Items.Clear()
        Dim saveTimeStamp = TSWSFM.SysTimeStamp.Text.Trim()

        For Each f In files
            Dim fileName As String = Path.GetFileName(f)
            Dim formattedDate As String = File.GetLastWriteTime(f).ToString("dd/MM/yyyy HH:mm")
            Dim timeStamp As String = File.GetLastWriteTime(f).ToString("yyyyMMddhhmmss")
            Dim tick As String = If(timeStamp = saveTimeStamp, "   ✔", "")

            Dim item As New ListViewItem(tick)

            With item.SubItems
                .Add(fileName)
                .Add(formattedDate)
                .Add(timeStamp)
            End With

            TSWSFM.CustomFileList.Items.Add(item)
        Next

        With TSWSFM
            Dim savedText = FindSaveFile(Path.Combine(.CurrentFolder.Text, TSWSaveFolder), .SysTimeStamp.Text.Trim())

            With .SaveLocation
                .Text = savedText
                .Font = New Font(.Font, If(savedText = "Not Saved", FontStyle.Bold, FontStyle.Regular))
                .BackColor = If(savedText = "Not Saved", ColorTranslator.FromHtml("#FFEEEE"), SystemColors.Control)
            End With

            With .CustomFileList
                .Columns(1).Width = If(.Items.Count > 9, 329, 346)
                .ListViewItemSorter = New ListViewItemComparer(lastColumn, lastOrder)
                .Sort()
            End With
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Get the details of the TSW save file
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub RefreshSaveFile(parentDir As String)

        Dim files = Directory.GetFiles(parentDir, TSWSaveFileName)

        If files.Length = 0 Then
            TSWSFM.SaveFileName.Text = ""
            MessageBox.Show("Current save game file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        With TSWSFM
            .SaveFileName.Text = Path.GetFileName(files(0))
            .SaveDate.Text = File.GetLastWriteTime(files(0)).ToString("dd/MM/yyyy HH:mm")
            .SysTimeStamp.Text = File.GetLastWriteTime(files(0)).ToString("yyyyMMddhhmmss")
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Populate the version drop-down with all versions of TSW that are installed
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub GetVersions()

        If TSWSteamRoot Is Nothing Then Exit Sub

        Dim libraries As New List(Of String)
        Dim tswInstalls As New List(Of String)
        Dim vdfPath = Path.Combine(TSWSteamRoot, "steamapps", "libraryfolders.vdf")
        If Not File.Exists(vdfPath) Then Exit Sub

        For Each line In File.ReadAllLines(vdfPath)
            Dim m = Regex.Match(line, """path""\s+""([^""]+)""")
            If m.Success Then libraries.Add(m.Groups(1).Value.Replace("\\\\", "\"))
        Next

        For Each libPath In libraries
            Dim manifestDir = Path.Combine(libPath, "steamapps")
            If Not Directory.Exists(manifestDir) Then Continue For

            For Each manifest In Directory.GetFiles(manifestDir, "appmanifest_*.acf")
                Dim text = File.ReadAllText(manifest)
                If text.Contains(TSWTitle) Then GetVersionDetails(text, manifestDir)
            Next
        Next

        If TSWVersions.Count < 1 Then
            MessageBox.Show("No TSW installations found in any Steam folders.", "TSW Save File Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        ElseIf TSWVersions.Count > 1 Then
            TSWVersions = TSWVersions.OrderBy(Function(v) v.Name).ToList()
        End If

        With TSWSFM.VersionSelect

            For Each v In TSWVersions
                .Items.Add(v.Name)
            Next

            If .Items.Count = 1 Then
                .SelectedIndex = 0
            Else
                Dim verIdx = .FindStringExact(My.Settings.lastVersion)
                .SelectedIndex = If(verIdx >= 0, verIdx, 0)
            End If

        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Set up tabs using subfolders within the custom save file folder
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub GetAllTabs()

        Dim folderTab As TabControl = TSWSFM.TabControl
        Dim parentFolder = Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder)

        folderTab.Visible = False
        folderTab.TabPages(0).Select()

        While folderTab.TabPages.Count > 1
            folderTab.TabPages.RemoveAt(1)
        End While

        For Each folder As String In Directory.GetDirectories(parentFolder).OrderBy(Function(f) If(My.Settings.lastTabOrder = "Date", Directory.GetCreationTime(f), Path.GetFileName(f)))
            Dim tp As New TabPage(Path.GetFileName(folder))
            tp.BackColor = Color.White
            folderTab.TabPages.Add(tp)
        Next

        folderTab.Visible = True
        TSWSFM.MenuFileName.Checked = If(My.Settings.lastTabOrder = "Date", False, True)
        TSWSFM.MenuCreationDate.Checked = If(My.Settings.lastTabOrder = "Date", True, False)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Refresh the TWS save file details and custom save file listview
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub UpdateUI(targetFolder As String)

        RefreshSaveFile(targetFolder)

        Dim tabName = TSWSFM.TabControl.SelectedTab.Text
        ListSaveFiles(Path.Combine(targetFolder, TSWSaveFolder, If(tabName = "Main", "", tabName)))

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Show status message until MessageTimer times out
    ' -----------------------------------------------------------------------------------------------------------

    Public CurrentColour As Color
    Public StartColour As Color = SystemColors.HotTrack ' Light blue
    Public EndColour As Color = SystemColors.Control
    Public FadeStep As Integer = 15

    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ShowTempMessage(msg As String)

        With TSWSFM
            Dim flags As TextFormatFlags = TextFormatFlags.NoPadding Or TextFormatFlags.NoClipping Or TextFormatFlags.TextBoxControl
            Dim textSize As Size = TextRenderer.MeasureText(msg, .StatusMessage.Font, New Size(Integer.MaxValue, Integer.MaxValue), flags)
            Dim usableWidth As Integer = .StatusMessage.ClientSize.Width + 8
            Dim wraps As Boolean = (textSize.Width > usableWidth)

            ' Add padding only if the text fits on one line
            Dim rtf As String = "{\rtf1\ansi{\pard\sb" & If(wraps, 0, 80) & " " & msg & "\par}}"

            .StatusMessage.Rtf = rtf
            .StatusMessage.Visible = True
            CurrentColour = StartColour
            .StatusMessage.ForeColor = CurrentColour

            .FadeTimer.Stop()
            .MessageTimer.Stop()
            .MessageTimer.Start()
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Get the selected TSW icon
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub LoadTSWIcon(selectedVersion As (Name As String, Location As String, AppID As Integer))

        Dim WinNoEdit As String = Path.Combine(selectedVersion.Location, "WindowsNoEditor")
        If Not Directory.Exists(WinNoEdit) Then Exit Sub

        Dim exePath As String = Directory.GetFiles(WinNoEdit, "*.exe", SearchOption.TopDirectoryOnly).FirstOrDefault()

        If exePath IsNot Nothing Then
            Dim ico = Icon.ExtractAssociatedIcon(exePath)
            TSWSFM.TSWIcon.Image = ico.ToBitmap()
        Else
            TSWSFM.TSWIcon.Image = Nothing
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Return the currently selected tab name
    ' -----------------------------------------------------------------------------------------------------------
    Public Function GetTab()
        Dim selectedTab = TSWSFM.TabControl.SelectedTab.Text
        Return If(selectedTab = "Main", "", selectedTab)
    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Call the dialog form
    ' -----------------------------------------------------------------------------------------------------------
    Public Function TSWInputBox(dialogType As String, Optional sourceName As String = "") As String

        Using dlg As New Dialog(dialogType)
            dlg.ObjectName = sourceName
            dlg.Mode = dialogType

            If dlg.ShowDialog(TSWSFM) = DialogResult.OK Then
                Dim newName As String = dlg.ResultName
                Return newName
            End If
        End Using

        Return Nothing

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Move one or all files from one folder to another
    ' -----------------------------------------------------------------------------------------------------------
    Public Function MoveFiles(sourceFolder As String, targetFolder As String, Optional fileName As String = "") As Boolean

        If Not Directory.Exists(sourceFolder) Then
            MessageBox.Show("Source folder " & sourceFolder & " does not exist.", "Move Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        ElseIf Not Directory.Exists(targetFolder) Then
            MessageBox.Show("Target folder " & targetFolder & " does not exist.", "Move Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        If fileName <> "" Then
            Dim sourceFile = Path.Combine(sourceFolder, fileName)

            If Not File.Exists(sourceFile) Then
                MessageBox.Show("File " & fileName & " does not exist in folder " & sourceFolder & ".", "Move Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If

            Dim newFileName = GetUniqueFilename(Path.Combine(targetFolder, fileName))
            Dim file1 = Path.GetFileName(fileName)
            Dim file2 = Path.GetFileName(newFileName)

            If fileName <> "" AndAlso file1 <> file2 Then MessageBox.Show("Duplicate file name " & file1 & " found in target folder. New file will be renamed to " & file2 & ".", "Move File")
            File.Move(Path.Combine(sourceFolder, fileName), Path.Combine(targetFolder, newFileName))
        Else

            Dim files = Directory.GetFiles(sourceFolder)

            For Each f In files
                Dim fileNameToMove = Path.GetFileName(f)
                File.Move(Path.Combine(sourceFolder, fileNameToMove), GetUniqueFilename(Path.Combine(targetFolder, fileNameToMove)))
            Next

        End If

        Return True

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Rename a custom tab
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub RenameTabs(tabIndex As Integer)

        If tabIndex = 0 Then
            MessageBox.Show("The Main folder cannot be renamed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim oldName = TSWSFM.TabControl.TabPages(tabIndex).Text
        Dim name = TSWInputBox("Rename", oldName)

        If name = "" Then Exit Sub

        Dim cleanName = String.Concat(name.Where(Function(c) Not Path.GetInvalidFileNameChars.Contains(c)))

        If cleanName = oldName Then Exit Sub

        Dim sourceFolder = Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder, oldName)
        Dim targetFolder = Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder, cleanName)

        If Directory.Exists(targetFolder) Then
            MessageBox.Show("The " & cleanName & " folder already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        TSWSFM.TabControl.TabPages(tabIndex).Text = cleanName
        Directory.Move(sourceFolder, targetFolder)
        ShowTempMessage("Folder " & oldName & " renamed to " & cleanName & " successfully")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Delete a custom tab
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DeleteTabs(tabIndex As Integer)

        Dim filesMoved As Boolean = False
        Dim tabName = TSWSFM.TabControl.TabPages(tabIndex).Text

        If tabIndex = 0 Then
            MessageBox.Show("The Main folder cannot be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim folderPath As String = Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder, tabName)
        If Not Directory.Exists(folderPath) Then Exit Sub

        Dim msg = "Are you sure you want to delete the " & tabName & " folder?"

        If Directory.GetFiles(folderPath, "*.sav").Any() Then

            msg = "The " & tabName & " folder contains save files! Do you want to move these to the Main folder before deleting?"
            Dim response = MessageBox.Show(msg, "Delete Folder", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)

            If response = DialogResult.Cancel Then
                Exit Sub
            ElseIf response = DialogResult.Yes Then
                MoveFiles(folderPath, Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder))
                filesMoved = True
            End If

        ElseIf MessageBox.Show(msg, "Delete Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        TSWSFM.TabControl.TabPages.Remove(TSWSFM.TabControl.TabPages(tabIndex))
        FileIO.FileSystem.DeleteDirectory(folderPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        ShowTempMessage("Folder " & tabName & " deleted " & If(filesMoved, " and files moved to the Main folder", " and moved to the recycle bin"))

        tabName = TSWSFM.TabControl.SelectedTab.Text
        ListSaveFiles(Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder, If(tabName = "Main", "", tabName)))

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Set order of tabs
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ResetTabs(orderName As String)

        My.Settings.lastTabOrder = orderName
        My.Settings.Save()

        GetAllTabs()

        Dim currentTab = TSWSFM.TabControl.SelectedTab.Text
        ListSaveFiles(Path.Combine(TSWSFM.CurrentFolder.Text, TSWSaveFolder, If(currentTab = "Main", "", currentTab)))

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Class for the listview sort function
    ' -----------------------------------------------------------------------------------------------------------
    Public Class ListViewItemComparer
        Implements IComparer

        Private ReadOnly col As Integer
        Private ReadOnly order As SortOrder
        Private ReadOnly dateFormat As String = "dd/MM/yyyy HH:mm"
        Private ReadOnly culture As CultureInfo = CultureInfo.InvariantCulture
        ' ----------------------------------------------------
        Public Sub New(column As Integer, sortOrder As SortOrder)
            col = column
            order = sortOrder
        End Sub
        ' ----------------------------------------------------
        Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim itemX As ListViewItem = CType(x, ListViewItem)
            Dim itemY As ListViewItem = CType(y, ListViewItem)

            Dim valueX As String = itemX.SubItems(col).Text
            Dim valueY As String = itemY.SubItems(col).Text

            Dim result As Integer

            If col = 2 Then
                Dim dx, dy As DateTime

                If DateTime.TryParseExact(valueX, dateFormat, culture, DateTimeStyles.None, dx) AndAlso
                   DateTime.TryParseExact(valueY, dateFormat, culture, DateTimeStyles.None, dy) Then

                    result = DateTime.Compare(dx, dy)
                Else
                    result = String.Compare(valueX, valueY)
                End If
            Else
                result = String.Compare(valueX, valueY)
            End If

            If order = SortOrder.Descending Then result = -result
            Return result
        End Function
    End Class

    ' -----------------------------------------------------------------------------------------------------------

End Module
