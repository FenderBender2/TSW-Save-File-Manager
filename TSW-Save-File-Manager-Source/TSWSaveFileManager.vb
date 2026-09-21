' $Id: TSWSaveFileManager.vb 1494 2026-09-21 21:33:13Z Pete $
Imports System.IO
Imports System.Runtime
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Module TSWSaveFileManager

    Private Const TSWTITLE As String = "Train Sim World"

    Public Const TSWSAVEFILENAME As String = "TSWSaveGame_"
    Public Const TSWSAVEFOLDER As String = "Custom Saves"

    Public TSWCustomParent As String
    Public TSWCurrentProfile As String
    Public TSWCurrentProfilePath As String
    Public TSCappId As Integer = 0
    Public TSWappId As Integer
    Public TSWVersions As New List(Of (Name As String, Location As String, AppID As Integer))
    Public TSWcurrentIsSaved As Boolean
    Public TSWEnableFunctions As Boolean = False

    Public isMyWrite As Boolean = False
    Public lastColumn As Integer
    Public lastOrder As SortOrder
    Public lastVersion As String
    Public rightClickedTabIndex As Integer = -1
    Public suppressColumnEvents As Boolean = False

    Public arrowUp As Bitmap
    Public arrowDown As Bitmap

    Public customFixedWidths() As Integer = {19, 346, 100, 0}
    Public profileArray(,) As String
    Public copyProfileArray(,) As String

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
    ' Find the save game folder
    ' -----------------------------------------------------------------------------------------------------------
    Public Function GetSaveFolder(TSWName As String) As String

        Dim folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & $"\My Games\{TSWName.Replace(" ", "")}\Saved\SaveGames"
        Dim fld As String = Directory.GetDirectories(folder).Select(Function(f) Path.GetFileName(f)).FirstOrDefault(Function(n) Not n.Equals(TSWSAVEFOLDER, StringComparison.OrdinalIgnoreCase))

        Return Path.Combine(folder, If(fld = "", "", fld))

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Process the selected version
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub SelectVersion(ii As PictureBox, vs As ComboBox, cf As TextBox)

        Dim selectedName = vs.SelectedItem.ToString
        Dim selectedItem = TSWVersions.First(Function(v) v.Name = selectedName)
        Dim folder = GetSaveFolder(vs.Text)

        LoadTSWIcon(ii, TSWVersions(vs.SelectedIndex))

        TSWappId = selectedItem.AppID

        If Not Directory.Exists(folder) Then
            With TSWSFM
                .ProfileSelect.Items.Clear()
                .ProfileSelect.Text = ""
                .SaveFileName.Text = ""
                .SavedAsFileName.Text = ""
                .SaveDate.Text = ""
                .SaveLocation.Text = ""
                .NewFileName.Text = ""
                .FileCount.Text = ""
                .CustomFileList.Items.Clear()
            End With

            MessageBox.Show($"Unable to find save game folder for {vs.SelectedItem}. You may need to run the game first and save a game.",
                            "Version Select", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        TSWCustomParent = Path.Combine(folder, TSWSAVEFOLDER)
        cf.Text = folder
        GetAllProfiles(folder)

        With My.Settings
            .lastVersion = selectedName
            .Save()
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Get the details of the version
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub GetVersionDetails(lineText As String, folderPath As String, Optional isTWS As Boolean = True)

        Dim dirMatch = Regex.Match(lineText, """installdir""\s+""([^""]+)""")

            If dirMatch.Success Then
            Dim appIdMatch = Regex.Match(lineText, """appid""\s*""(\d+)""") ' TSW app ID
            Dim appId As Integer = Integer.Parse(appIdMatch.Groups(1).Value)
            Dim installDir = dirMatch.Groups(1).Value
            Dim fullPath = Path.Combine(folderPath, "common", installDir)

            If isTWS Then
                TSWVersions.Add((Path.GetFileName(fullPath), fullPath, appId))
            ElseIf File.Exists("TSCEnabledb-HDfhymdS447pwdbvH.flag") Then
                TSCappId = appId
            End If
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Process the selected version
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub SelectProfile(vs As ComboBox, fs As TabControl, folder As String)

        Dim profileName = GetCurrentProfile(TSWSFM.ProfileSelect, 0)

        RefreshSaveFile(folder, profileName)
        TSWCurrentProfile = "Profile" & profileName

        ' Create version save folder parent if it doesnt exist
        TSWCurrentProfilePath = Path.Combine(TSWCustomParent, TSWCurrentProfile)
        If Not Directory.Exists(TSWCurrentProfilePath) Then Directory.CreateDirectory(TSWCurrentProfilePath)

        GetAllTabs(fs)
        ListSaveFiles(TSWCurrentProfilePath)

        With My.Settings
            .lastProfile = GetCurrentProfile(TSWSFM.ProfileSelect, 1)
            .Save()
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Find if the current save file is saved anywhere
    ' -----------------------------------------------------------------------------------------------------------
    Private Function FindSaveFile(parentFolder As String, targetTime As String) As String

        TSWcurrentIsSaved = False

        ' Loop through all files in this folder
        For Each fileName As String In Directory.GetFiles(parentFolder, "*.sav")
            If File.GetLastWriteTime(fileName).ToString("yyyyMMddhhmmss") = targetTime Then
                TSWcurrentIsSaved = True
                Dim folderName = Path.GetFileName(parentFolder)

                Return Path.GetFileNameWithoutExtension(fileName) & "|" & If(folderName = TSWCurrentProfile, "Main", folderName)
            End If
        Next

        ' Loop through subfolders (recursive)
        For Each subFolder As String In Directory.GetDirectories(parentFolder)
            Dim result As String = FindSaveFile(subFolder, targetTime)

            If result <> "Not Saved|" Then
                TSWcurrentIsSaved = True
                Return result
            End If
        Next

        Return "Not Saved|"

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Get the currently selected profile (0 = profile ID, 1 = profile friendly name)
    ' -----------------------------------------------------------------------------------------------------------
    Public Function GetCurrentProfile(cb As ComboBox, Optional col As Integer = 0) As String

        Dim idx As Integer = cb.SelectedIndex
        If idx < 0 Then Return ""

        Return profileArray(idx, col)

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

        isMyWrite = False

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
                    .Add("", customFixedWidths(0), HorizontalAlignment.Right)
                    .Add("Filename", customFixedWidths(1))
                    .Add("Save Date", customFixedWidths(2))
                    .Add("FileTimestamp", customFixedWidths(3))
                End With
            End With
        End With

        TSWSFM.FolderSelect.DrawMode = TabDrawMode.OwnerDrawFixed
        TSWSFM.LongTextDisplay.OwnerDraw = True

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Populate the version drop-down with all versions of TSW that are installed
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub GetVersions()

        Dim steamRoot = GetSteamRoot()
        If steamRoot Is Nothing Then Exit Sub

        Dim libraries As New List(Of String)
        Dim vdfPath = Path.Combine(steamRoot, "steamapps", "libraryfolders.vdf")
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

                If text.Contains(TSWTITLE) Then
                    GetVersionDetails(text, manifestDir)
                ElseIf text.Contains("Train Simulator Classic") Then
                    GetVersionDetails(text, manifestDir, False)
                End If
            Next
        Next

        TSWSFM.RunTSC.Visible = TSCappId <> 0

        If TSWVersions.Count < 1 Then
            MessageBox.Show("No TSW installations found in any Steam folders.",
                            "TSW Save File Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        ElseIf TSWVersions.Count > 1 Then
            TSWVersions = TSWVersions.OrderBy(Function(v) v.AppID).ToList()
        End If

        With TSWSFM.VersionSelect
            For Each v In TSWVersions
                .Items.Add(v.Name)
            Next

            If .Items.Count = 1 Then ' This will trigger the selection event on the Version select dropdown
                .SelectedIndex = 0
            ElseIf My.Settings.lastVersion <> "" Then
                Dim verIdx = .FindStringExact(My.Settings.lastVersion)
                .SelectedIndex = If(verIdx >= 0, verIdx, 0)
            End If
        End With

        TSWEnableFunctions = True

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Populate the custom save file list
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ListSaveFiles(folderpath As String)

        If folderpath = TSWSAVEFOLDER Or TSWSFM.CurrentFolder.Text = "" Then Exit Sub

        Dim files = Directory.GetFiles(folderpath)
        Dim saveTimeStamp = TSWSFM.SysTimeStamp.Text.Trim()

        With TSWSFM
            .CustomFileList.Items.Clear()
            .NewFileName.Text = ""
        End With

        For Each f In files
            If Path.GetExtension(f) <> ".sav" Then Continue For

            Dim fileName As String = Path.GetFileNameWithoutExtension(f)
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
            Dim savedText() = Split(FindSaveFile(TSWCurrentProfilePath, .SysTimeStamp.Text.Trim()), "|"c, 2)

            ' Set attributes of TSW save file location
            With .SavedAsFileName
                .Text = savedText(0)
                .Font = New Font(.Font, If(savedText(0) = "Not Saved", FontStyle.Bold, FontStyle.Regular))
                .BackColor = If(savedText(0) = "Not Saved", ColorTranslator.FromHtml("#FFEEEE"), SystemColors.Control)
            End With

            .SaveLocation.Text = savedText(1)
            suppressColumnEvents = True

            ' Sort the custom list by the last used order
            With .CustomFileList
                customFixedWidths(1) = If(.Items.Count > 9, 329, 346)
                .Columns(1).Width = If(.Items.Count > 9, 329, 346)
                .ListViewItemSorter = New ListViewItemComparer(lastColumn, lastOrder)
                .Sort()
            End With

            suppressColumnEvents = False
        End With

        Dim count = Directory.EnumerateFiles(TSWCurrentProfilePath, "*.sav", SearchOption.AllDirectories).Count()
        TSWSFM.FileCount.Text = $"Total File Count:  {count}"

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Get the details of the TSW save file
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub RefreshSaveFile(parentDir As String, profileName As String)

        Dim files = Directory.GetFiles(parentDir, TSWSAVEFILENAME & profileName & ".sav")

        If files.Length = 0 Then
            TSWSFM.SaveFileName.Text = ""
            MessageBox.Show("Current save game file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        With TSWSFM
            .SaveFileName.Text = Path.GetFileNameWithoutExtension(files(0))
            .SaveDate.Text = File.GetLastWriteTime(files(0)).ToString("dd/MM/yyyy HH:mm")
            .SysTimeStamp.Text = File.GetLastWriteTime(files(0)).ToString("yyyyMMddhhmmss")
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Populate the profile drop-down with all profiles in the selected version
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub PopulateProfileArray(folder As String, ByRef pArray(,) As String, pl As ComboBox)

        Dim savefiles = Directory.GetFiles(folder, TSWSAVEFILENAME & "*.sav")
        ReDim pArray(savefiles.Length - 1, 1)

        pl.Items.Clear()

        For i As Integer = 0 To savefiles.Length - 1

            Dim fileName As String = Path.GetFileName(savefiles(i))
            fileName = Path.GetFileNameWithoutExtension(fileName)

            If fileName.StartsWith(TSWSAVEFILENAME) Then
                Dim profileID As String = fileName.Substring(TSWSAVEFILENAME.Length)
                Dim friendlyName As String = profileID
                '                Dim profileDir = Path.Combine(TSWCustomParent, "Profile" & profileID)
                Dim profileDir = Path.Combine(folder, TSWSAVEFOLDER, "Profile" & profileID)

                If Directory.Exists(profileDir) Then
                    Dim tagfiles = Directory.GetFiles(profileDir, "*.tag")

                    For Each tagFile As String In tagfiles
                        friendlyName = Path.GetFileNameWithoutExtension(tagFile)
                        Exit For
                    Next
                End If

                pArray(i, 0) = profileID
                pArray(i, 1) = friendlyName

                ' Add friendly name to ComboBox
                pl.Items.Add(friendlyName)
            End If
        Next

        If pl.Items.Count = 1 Then pl.SelectedIndex = 0

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Public Sub GetAllProfiles(folder As String)

        With TSWSFM
            .ProfileSelect.Items.Clear()
            .ProfileSelect.Text = ""

            PopulateProfileArray(folder, profileArray, .ProfileSelect)

            If .ProfileSelect.Items.Count = 0 Then
                .SaveFileName.Text = ""
                .SavedAsFileName.Text = ""
                .SaveDate.Text = ""
                .SaveLocation.Text = ""
                .NewFileName.Text = ""
                .FileCount.Text = ""
                .CustomFileList.Items.Clear()

                MessageBox.Show("No save game files found for this version.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End With

        With TSWSFM.ProfileSelect ' This will trigger the selection event on the Profile select dropdown
            If .Items.Count = 1 Then
                .SelectedIndex = 0
            ElseIf My.Settings.lastProfile <> "" Then
                Dim verIdx = .FindStringExact(My.Settings.lastProfile)
                .SelectedIndex = If(verIdx >= 0, verIdx, 0)
            End If
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Refresh the TWS save file details and custom save file listview
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub UpdateUI(targetFolder As String)

        Dim profileName = GetCurrentProfile(TSWSFM.ProfileSelect, 0)
        RefreshSaveFile(targetFolder, profileName)

        Dim tabName = TSWSFM.FolderSelect.SelectedTab.Text
        ListSaveFiles(Path.Combine(targetFolder, TSWSAVEFOLDER, TSWCurrentProfile, If(tabName = "Main", "", tabName)))

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Sort the columns in the custom file list using the custom ListViewItemComparer class
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ColumnHeaderClick(cf As ListView, col As Integer)

        If col < 1 Then
            Exit Sub
        ElseIf col = lastColumn Then
            lastOrder = If(lastOrder = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending)
        Else
            lastColumn = col
            lastOrder = SortOrder.Ascending
        End If

        With cf
            Dim headerRect As New Rectangle(0, 0, .Width, .Font.Height + 8)

            .ListViewItemSorter = New ListViewItemComparer(col, lastOrder)
            .Sort()
            .Invalidate(headerRect)
        End With

        With My.Settings
            .lastSortColumn = lastColumn
            .lastSortOrder = lastOrder.ToString()
            .Save()
        End With

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
            Dim usableWidth As Integer = .StatusMessage.ClientSize.Width
            Dim wraps As Boolean = (textSize.Width > usableWidth - 2)

            ' Add padding only if the text fits on one line
            Dim rtf As String = $"{{\rtf1\ansi{{\pard\sb{If(wraps, 0, 100)} {msg}\par}}}}"

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
    ' Calculate colour fade for each channel R G B
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
    ' Fade colour on each tick of the fade timer
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub FadeColours(tm As Timer)

        Dim r = MoveChannel(CurrentColour.R, EndColour.R)
        Dim g = MoveChannel(CurrentColour.G, EndColour.G)
        Dim b = MoveChannel(CurrentColour.B, EndColour.B)

        CurrentColour = Color.FromArgb(r, g, b)
        TSWSFM.StatusMessage.ForeColor = CurrentColour

        ' Stop when close enough to the target colour
        If r = EndColour.R AndAlso g = EndColour.G AndAlso b = EndColour.B Then
            tm.Stop()
            TSWSFM.StatusMessage.Visible = False
            TSWSFM.StatusMessage.ForeColor = StartColour
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Get the selected TSW icon
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub LoadTSWIcon(ii As PictureBox, selectedVersion As (Name As String, Location As String, AppID As Integer))

        Dim WinNoEdit As String = Path.Combine(selectedVersion.Location, "WindowsNoEditor")
        If Not Directory.Exists(WinNoEdit) Then Exit Sub

        Dim exePath As String = Directory.GetFiles(WinNoEdit, "*.exe", SearchOption.TopDirectoryOnly).FirstOrDefault()

        If exePath IsNot Nothing Then
            Dim ico = Icon.ExtractAssociatedIcon(exePath)
            ii.Image = ico.ToBitmap()
        Else
            ii.Image = Nothing
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Call the dialog form
    ' -----------------------------------------------------------------------------------------------------------
    Public Function TSWInputBox(dialogType As String, Optional sourceName As String = "") As String

        Dim dlg = If(dialogType = "Move", New MoveDialog(), If(dialogType = "Copy", New CopyDialog(), New TSWDialog(dialogType)))
        Dim newDlg = If(dialogType = "Move", DirectCast(dlg, MoveDialog), If(dialogType = "Copy", DirectCast(dlg, CopyDialog), DirectCast(dlg, TSWDialog)))

        If TypeOf dlg Is TSWDialog Then
            newDlg.ObjectName = sourceName
            newDlg.Mode = dialogType
        ElseIf TypeOf dlg Is MoveDialog Then
            newDlg.ObjectName = sourceName
        ElseIf TypeOf dlg Is CopyDialog Then
            newDlg.ObjectName = sourceName
            '            If Not PopulateVersions(Nothing) Then Return Nothing
        End If

        If dlg.ShowDialog(TSWSFM) = DialogResult.OK Then Return newDlg.ResultName

        Return Nothing

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Move one or all files from one folder to another
    ' -----------------------------------------------------------------------------------------------------------
    Public Function MoveFiles(sourceFolder As String, targetFolder As String, Optional fileName As String = "") As Boolean

        If Not Directory.Exists(sourceFolder) Then
            MessageBox.Show($"Source folder {sourceFolder} does not exist.",
                            "Move Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        ElseIf Not Directory.Exists(targetFolder) Then
            MessageBox.Show($"Target folder {targetFolder} does not exist.",
                            "Move Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        If fileName <> "" Then
            Dim sourceFile = Path.Combine(sourceFolder, fileName)
            Dim file1 = Path.GetFileNameWithoutExtension(fileName)

            If Not File.Exists(sourceFile) Then
                MessageBox.Show($"File {file1} does not exist in folder {sourceFolder}.",
                                "Move Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If

            Dim newName = GetUniqueFilename(Path.Combine(targetFolder, fileName))
            Dim file2 = Path.GetFileNameWithoutExtension(newName)

            If file1 <> file2 Then MessageBox.Show($"Duplicate file name {file1} found in target folder. New file will be renamed to {file2}.", "Move File")
            File.Move(Path.Combine(sourceFolder, fileName), Path.Combine(targetFolder, newName))
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
    ' Rename a profile
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub RenameProfile(cb As ComboBox)

        Dim currName As String = GetCurrentProfile(TSWSFM.ProfileSelect, 1)
        Dim newName = TSWInputBox("Profile", currName)
        If newName Is Nothing Then Exit Sub

        newName = String.Concat(newName.Where(Function(c) Not Path.GetInvalidFileNameChars.Contains(c)))
        If currName = newName Then Exit Sub

        Dim idx As Integer = TSWSFM.ProfileSelect.SelectedIndex
        If idx < 0 Then Exit Sub

        Dim createTag As Boolean = True

        If newName = "" Then
            newName = profileArray(idx, 0)
            createTag = False
        Else
            If Directory.GetFiles(TSWCustomParent, newName & ".tag", SearchOption.AllDirectories).Length > 0 Then
                MessageBox.Show($"Profile {newName} already exists.", "Rename Profile", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If

        Dim oldFileName = Path.Combine(TSWCurrentProfilePath, currName & ".tag")
        If File.Exists(oldFileName) Then File.Delete(oldFileName)

        If createTag Then File.WriteAllText(Path.Combine(TSWCurrentProfilePath, newName & ".tag"), "")
        profileArray(idx, 1) = newName
        cb.Items(cb.SelectedIndex) = newName

        ShowTempMessage($"Profile {currName} renamed to {newName}")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Custom draw the listview  headers
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DrawListHeaders(sender As Object, e As DrawListViewColumnHeaderEventArgs)

        Dim cf As ListView = DirectCast(sender, ListView)
        Dim headerHeight As Integer = e.Bounds.Height - 4
        Dim rect As New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, headerHeight)

        Using bgBrush As New SolidBrush(Color.FromArgb(240, 240, 240)) ' light grey
            e.Graphics.FillRectangle(bgBrush, rect)
        End Using

        ' --- Draw bold text, left aligned ---
        Dim boldFont As New Font(cf.Font, FontStyle.Bold)
        Dim sf As New StringFormat() With {.LineAlignment = StringAlignment.Near}
        Dim paddedRect As New Rectangle(rect.X + 4, rect.Y, rect.Width - 4, rect.Height)

        e.Graphics.DrawString(e.Header.Text, boldFont, Brushes.Black, paddedRect, sf)

        ' --- Draw arrow on sorted column ---
        If e.ColumnIndex = lastColumn Then

            Dim arrowImg As Image = TSWSFM.imgHeaderArrows.Images(If(lastOrder = SortOrder.Ascending, 0, 1))

            Dim scale As Single = 0.5F   ' 50% size
            Dim newW As Integer = CInt(arrowImg.Width * (scale + 0.1))
            Dim newH As Integer = CInt(arrowImg.Height * (scale - 0.1))

            Dim x As Integer = rect.Right - newW - 4
            Dim y As Integer = rect.Top + (rect.Height - newH) \ 2

            e.Graphics.DrawImage(arrowImg, New Rectangle(x, y, newW, newH))

        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Format column 0 of the custom list for the display of ticks
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DrawTick(sender As Object, e As DrawListViewSubItemEventArgs)

        Dim cf As ListView = DirectCast(sender, ListView)

        If e.ColumnIndex = 0 Then
            Dim text As String = e.SubItem.Text

            If text <> "" Then
                Dim flags As TextFormatFlags = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine
                Dim textColor As Color = Color.DarkRed

                TextRenderer.DrawText(e.Graphics, text, cf.Font, e.Bounds, textColor, flags)
            End If
        Else
            e.DrawDefault = True
        End If
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Display a tooltip for text boxes with long text
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ShowTooltip(tt As ToolTip, sender As Object)
        Dim tb = DirectCast(sender, TextBox)
        tt.SetToolTip(tb, If(TextRenderer.MeasureText(tb.Text, tb.Font).Width > tb.Width, tb.Text, Nothing))
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Public Sub DrawTooltip(e As DrawToolTipEventArgs)
        e.Graphics.FillRectangle(Brushes.LightYellow, e.Bounds) ' Background
        ControlPaint.DrawBorder(e.Graphics, e.Bounds, Color.Black, ButtonBorderStyle.Solid) ' Border

        Using f As New Font("Segoe UI", 9, FontStyle.Regular)
            TextRenderer.DrawText(e.Graphics, e.ToolTipText, f, e.Bounds, Color.Black) ' Text
        End Using
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    Public Sub SetTooltipSize(tt As ToolTip, e As PopupEventArgs)
        Dim textSize = TextRenderer.MeasureText(tt.GetToolTip(e.AssociatedControl), New Font("Segoe UI", 9, FontStyle.Regular))
        e.ToolTipSize = New Size(textSize.Width + 10, textSize.Height + 10)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
End Module
