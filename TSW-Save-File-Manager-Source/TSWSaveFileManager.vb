Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Module TSWSaveGameManager

    Const TSWsaveFileName As String = "TSWSaveGame_*.sav"
    Const TSWTitle As String = "Train Sim World"

    Public ReadOnly TSWSteamRoot As String = GetSteamRoot()
    Public TSWVersions As New List(Of (Name As String, Location As String, AppID As Integer))
    Public TSWcurrentIsSaved As Boolean
    Public IsMyWrite As Boolean = False

    ' -----------------------------------------------------------------------------------------------------------
    ' Find the installation location of Steam applicatons
    ' -----------------------------------------------------------------------------------------------------------
    Private Function GetSteamRoot() As String

        Dim key = Registry.CurrentUser.OpenSubKey("Software\Valve\Steam")

        If key Is Nothing Then
            MessageBox.Show("Steam parent folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If

        Return key.GetValue("SteamPath")?.ToString()

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Populate the custom save file list
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ListSaveFiles(folderpath As String)

        Dim files = IO.Directory.GetFiles(folderpath)

        TSWSFM.CustomFileList.OwnerDraw = True

        TSWSFM.CustomFileList.Items.Clear()
        TSWSFM.CustomFileList.Columns.Clear()
        TSWSFM.CustomFileList.View = View.Details
        TSWSFM.CustomFileList.FullRowSelect = True

        TSWSFM.CustomFileList.Columns.Add("", 19, HorizontalAlignment.Right)
        TSWSFM.CustomFileList.Columns.Add("Filename", 346)
        TSWSFM.CustomFileList.Columns.Add("Save Date", 100)
        TSWSFM.CustomFileList.Columns.Add("FileTimestamp", 0)

        TSWcurrentIsSaved = False

        For Each f In files
            Dim fileName As String = IO.Path.GetFileName(f)
            Dim formattedDate As String = IO.File.GetLastWriteTime(f).ToString("dd/MM/yyyy HH:mm")
            Dim timeStamp As String = IO.File.GetLastWriteTime(f).ToString("yyyyMMddhhmmss")
            Dim tick As String

            If timeStamp = TSWSFM.SysTimeStamp.Text.Trim() Then
                tick = "✔"
                TSWcurrentIsSaved = True
            Else
                tick = " "
            End If

            Dim item As New ListViewItem(tick)

            item.SubItems.Add(fileName)
            item.SubItems.Add(formattedDate)
            item.SubItems.Add(timeStamp)

            TSWSFM.CustomFileList.Items.Add(item)

        Next

        TSWSFM.FileNotSaved.Visible = Not TSWcurrentIsSaved
        TSWSFM.CustomFileList.Columns(1).Width = If(TSWSFM.CustomFileList.Items.Count > 9, 329, 346)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Get the details of the TSW save file
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub RefreshSaveFile(parentDir As String)

        Dim files = IO.Directory.GetFiles(parentDir, TSWsaveFileName)

        If files.Length = 0 Then
            MessageBox.Show("Current save game file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        TSWSFM.SaveFileName.Text = IO.Path.GetFileName(files(0))
        TSWSFM.SaveDate.Text = IO.File.GetLastWriteTime(files(0)).ToString("dd/MM/yyyy HH:mm")
        TSWSFM.SysTimeStamp.Text = IO.File.GetLastWriteTime(files(0)).ToString("yyyyMMddhhmmss")

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Populate the version drop-down with all versions of TSW that are installed
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub GetLatestVersion()

        If TSWSteamRoot Is Nothing Then Exit Sub

        Dim libraries As New List(Of String)
        Dim tswInstalls As New List(Of String)
        Dim vdfPath = Path.Combine(TSWSteamRoot, "steamapps", "libraryfolders.vdf")

        For Each line In File.ReadAllLines(vdfPath)
            Dim m = Regex.Match(line, """path""\s+""([^""]+)""")

            If m.Success Then
                libraries.Add(m.Groups(1).Value.Replace("\\\\", "\"))
            End If
        Next

        ' Now lib is defined here:
        For Each libPath In libraries
            Dim manifestDir = Path.Combine(libPath, "steamapps")

            If Not Directory.Exists(manifestDir) Then Continue For

            For Each manifest In Directory.GetFiles(manifestDir, "appmanifest_*.acf")

                Dim text = File.ReadAllText(manifest)

                If text.Contains(TSWTitle) Then

                    Dim dirMatch = Regex.Match(text, """installdir""\s+""([^""]+)""")
                    Dim appIdMatch = System.Text.RegularExpressions.Regex.Match(text, """appid""\s*""(\d+)""") ' TSW app ID

                    If dirMatch.Success Then

                        Dim appId As Integer = Integer.Parse(appIdMatch.Groups(1).Value)
                        Dim installDir = dirMatch.Groups(1).Value
                        Dim fullPath = Path.Combine(manifestDir, "common", installDir)

                        TSWVersions.Add((IO.Path.GetFileName(fullPath), fullPath, appId))

                    End If
                End If
            Next
        Next

        If TSWVersions.Count > 1 Then TSWVersions = TSWVersions.OrderBy(Function(v) v.Name).ToList()

        For Each v In TSWVersions
            TSWSFM.VersionSelect.Items.Add(v.Name)
        Next

        If TSWSFM.VersionSelect.Items.Count = 1 Then TSWSFM.VersionSelect.SelectedIndex = 0

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Rrefresh the TWS save file details and custom save file listview
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub UpdateUI(targetFolder As String)
        RefreshSaveFile(targetFolder)
        ListSaveFiles(IO.Path.Combine(targetFolder, "Custom Saves"))
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Show status message until MessageTimer times out
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ShowTempMessage(msg As String)

        TSWSFM.StatusMessage.Text = msg
        TSWSFM.StatusMessage.Visible = True
        TSWSFM.MessageTimer.Stop()
        TSWSFM.MessageTimer.Start()

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Get the selected TSW icon
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub LoadTSWIcon(selectedVersion As (Name As String, Location As String, AppID As Integer))

        Dim exePath As String = Directory.GetFiles(Path.Combine(selectedVersion.Location, "WindowsNoEditor"), "*.exe", SearchOption.TopDirectoryOnly).FirstOrDefault()

        If exePath IsNot Nothing Then
            Dim ico = Icon.ExtractAssociatedIcon(exePath)
            TSWSFM.TSWIcon.Image = ico.ToBitmap()
        Else
            TSWSFM.TSWIcon.Image = Nothing
        End If

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
        '   ----------------------------------------------------
        Public Sub New(column As Integer, sortOrder As SortOrder)
            col = column
            order = sortOrder
        End Sub
        '   ----------------------------------------------------
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

End Module
