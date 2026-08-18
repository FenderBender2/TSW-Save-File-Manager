Imports System.Runtime.InteropServices

Public Class HelpForm

    ' -----------------------------------------------------------------------------------------------------------
    ' Form load - Help text in RTF format
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HelpForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CopyRight.Text = "Copyright © P. Lewis " & DateTime.Now.Year & ". All rights reserved."
        Me.HelpText.Rtf = ShowText("Overview")
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Close button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Tab control
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub MenuTab_MouseDown(sender As Object, e As MouseEventArgs) Handles MenuTab.MouseDown

        If e.Button = MouseButtons.Right Then Exit Sub

        Me.HelpText.ScrollToCaret() ' Prevents 'ghost' scrollbars
        Me.HelpText.Rtf = ""
        Me.HelpText.Rtf = ShowText(MenuTab.SelectedTab.Text)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Custom border for rich text box
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HelpForm_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

        Dim r As Rectangle = HelpText.Bounds ' paint rectangle on the borders of the help text box

        r.Inflate(1, 1)
        e.Graphics.DrawRectangle(Pens.Black, r)

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Prevent the cursor from appearing in the rich text box
    ' -----------------------------------------------------------------------------------------------------------

    <DllImport("user32.dll")>
    Private Shared Function HideCaret(hWnd As IntPtr) As Boolean
    End Function

    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HelpText_GotFocus(sender As Object, e As EventArgs) Handles HelpText.GotFocus
        HideCaret(HelpText.Handle)
    End Sub

    Private Sub HelpText_MouseDown(sender As Object, e As MouseEventArgs) Handles HelpText.MouseDown
        HideCaret(HelpText.Handle)
    End Sub

    Private Sub HelpText_MouseUp(sender As Object, e As MouseEventArgs) Handles HelpText.MouseUp
        HideCaret(HelpText.Handle)
    End Sub

    Private Sub HelpText_Enter(sender As Object, e As EventArgs) Handles HelpText.Enter
        HideCaret(HelpText.Handle)
    End Sub

    Private Sub HelpText_SelectionChanged(sender As Object, e As EventArgs) Handles HelpText.SelectionChanged
        HideCaret(HelpText.Handle)
    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Small helper function for bold text
    ' -----------------------------------------------------------------------------------------------------------
    Private Function bold(txt As String) As String
        Return "\cf1\b " & txt & "\b0\cf0 "
    End Function
    ' -----------------------------------------------------------------------------------------------------------
    ' Main function to display RTF formatted text in the help box
    ' -----------------------------------------------------------------------------------------------------------
    Private Function ShowText(textName As String) As String

        Dim c As Color = ColorTranslator.FromHtml("#005500") ' Dark green

        Dim mt As String = "{\pard\sb100\sa150\fs24\qc\cf3\b "             ' Main title
        Dim mtc As String = "\b0\cf0\par}"                                 ' Main title close
        Dim st As String = "{\pard\li130\sa100\sb80\fs23\cf2\b\ul "        ' Subtitle
        Dim stc As String = "\ul0\b0\cf0\par}"                             ' Subtitle close
        Dim p As String = "{\pard\ql\li130\sa100\ri100\fs22 "              ' Paragraph
        Dim pc As String = "\par}"                                         ' Paragraph close
        Dim i As String = "{\pard\li550\fi-200\ri100\sa50\fs22 -   "       ' Indent
        Dim il As String = "{\pard\li550\fi-200\ri100\sa100\fs22 -   "     ' Last indent
        Dim b As String = "{\pard\li350\fi-200\ri100\sa100\fs22\bullet   " ' Bullet

        Dim rtfHeader As String = "{\rtf1\ansi" &
                                  "{\colortbl; ; " &                                     ' Define a colour table
                                  "\red" & c.R & "\green" & c.G & "\blue" & c.B & "; " & ' \cf1 dark green
                                  "\red0\green0\blue128; " &                             ' \cf2 mid blue
                                  "\red100\green0\blue0; }"                              ' \cf3 dark red

        Dim rtf As String

        Select Case Trim(textName)
            ' ----------------------------------------
            ' Overview
            ' ----------------------------------------
            Case "Overview"
                rtf = rtfHeader & mt & "TSW Save Manager " & Application.ProductVersion & mtc &
                      p & "This app allows you to manage multiple Train Sim World save files. Select your version from the drop-down if you have " &
                          "more than one version installed. You can also select different profiles if you have more than one defined in TSW - see " &
                          "the Profiles section. Your selections will be remembered for the next time you start the app." & pc

                rtf &= st & "Saved Files" & stc &
                       p & "You can save and restore TSW save files as required. Your files appear in the Custom Save Files list. If the current " &
                           "TSW file is in your custom list, the name of the custom file is displayed in the Saved As box and is also highlighted " &
                           "with a tick." & pc &
                       p & "All your files are stored in a Custom Saves folder in your TSW profile folder (usually \i My Documents\\My Games" &
                           "\\TrainSimWorldx\i0 )." & pc &
                       b & " File extensions are removed if they are included in the name." & pc

                rtf &= st & "Custom File List" & stc &
                       p & "The default order of the list is by file name. Click on a column heading to order by the values in that column. Click " &
                           "the same heading again to sort in reverse order. Your sort order will be remembered for the next time you start the app." & pc

                rtf &= st & "Running TSW" & stc &
                       p & "You can run TSW directly from this app by clicking the " & bold("Run TSW") & " button. This will run the selected version " &
                           "of the game. The app will remain open while you run TSW so you can save games on the fly. Use Alt-Tab to switch between " &
                           "the game and the app." & pc &
                       p & "When you save a game by overwriting an existing save, the app will automatically refresh the file details." & pc &
                       p & "The app cannot run TSW for a specific profile. You will need to manually select the correct profile in TSW if required." & pc

            ' ----------------------------------------
            ' Usage
            ' ----------------------------------------
            Case "Usage"
                rtf = rtfHeader & mt & "Usage" & mtc &
                      b & bold("Save") & " current save file. Enter a meaningful file name into the New Save File Name box, then click Save. " &
                          "The file will be saved to the current open folder - see the section on folders." & pc &
                      b & bold("Restore") & " save file. Select an existing save file from the custom file list then click Restore. You can " &
                          "also restore a save file by double-clicking the file name in the list." & pc &
                      b & bold("Rename") & " save file. Select an existing save file from the list, enter a new name into the New Save File " &
                          "Name box, then click Rename." & pc &
                      b & bold("Delete") & " file safely. Select a file in the custom file list then click Delete. The file will be moved to " &
                          "the Recycle Bin." & pc &
                      b & bold("Move") & " files to other folders. Select a file in the custom file list then click Move to display the Move " &
                          "File dialog. Select a folder from the drop-down list in the dialog and click Move. The file will be moved to your chosen " &
                          "folder." & pc &
                      p & "\li350 You can also use this function to move save files to another profile. Select the profile you want to move the file " &
                          "to before clicking Move." & pc

            ' ----------------------------------------
            ' Profiles
            ' ----------------------------------------
            Case "Profiles"
                rtf = rtfHeader & mt & "Profiles" & mtc &
                      p & "Every user of TSW has at least one profile in which save games and progress etc. are stored. If you have more than one " &
                          "profile that contains a saved game file, you can select the profiles from the drop down-list. This allows you to save " &
                          "your games for each profile. Profiles are discovered automatically, but only when a save game exists for them." & pc &
                      b & "The naming of save files changed in TSW6. Previous versions gave them names that included the profile name in the file " &
                          "name, but TSW6 uses a hex string which makes identifying profiles difficult. To help with this, you can rename any " &
                          "profile in any version of TSW using the " & bold("Rename Profile") & " function from the Profiles menu. Enter a new " &
                          "profile name then click Rename." & pc &
                      p & "\li350 Note: This feature is for convenience only - the profile is not renamed in the game." & pc &
                      b & "To revert the profile name to the default, clear the Profile Name box in the Rename Profile dialog and click Rename." & pc &
                      b & "When a new profile is created and a game is saved for the first time in the profile, you will need to close and re-open " &
                          "this app so that it can discover the new information." & pc

            ' ----------------------------------------
            ' Folders
            ' ----------------------------------------
            Case "Folders"
                rtf = rtfHeader & mt & "Folders" & mtc &
                      p & "To help with the management of save files you can create folders to group your files. When you create a folder a tab " &
                          "is added above the custom file list." & pc &
                      p & "There is a menu of controls for maintaining folders. Click on " & bold("Save Folders") & " in the menu bar. You can also " &
                          "right-click on the tabs to rename or delete them." & pc &
                      p & "Save files can be saved directly to any folder, or moved between folders as required. Drag & Drop is enabled " &
                          "allowing you to drag file names from the custom file list to drop them on a folder tab. Alternatively, you can use the " &
                          "Move button - see Usage section." & pc &
                      p & "If a file is moved to a location where another file with the same name exists, the moved file will be renamed with " &
                          "an incremental suffix. e.g. (1), (2) etc.." & pc &
                      p & "Each TSW version has its own set of folders. The Main folder is the parent Custom Save folder (one for each version) " &
                          "- it cannot be renamed or deleted." & pc

                rtf &= st & "Folder Usage" & stc &
                       b & bold("New Folder") & " - To create a new folder, select the option from the menu, provide a name for the folder in " &
                           "the dialog box then click Create." & pc &
                       b & bold("Rename Folder") & " - To rename a folder, make the folder active by clicking the tab then select \i Rename " &
                           "Folder\i0  from the menu. Next, provide a new name for the folder in the dialog box then click Rename." & pc &
                       b & bold("Delete Folder") & " - To delete a folder, click its tab then select \i Delete Folder\i0 . If the folder " &
                           "contains any save files you will be asked if you want to move them to the Main folder." & pc &
                           i & "If you choose \i Yes\i0  the files are moved and the folder is deleted, including any non-save files that may be " &
                               "present in the folder." & pc &
                          il & "If you choose \i No\i0  the folder and all of its files are moved to the recycle bin." & pc &
                       b & bold("Order Tabs") & " - Change the order of the tabs using this option." & pc &
                           i & "\i Created Date\i0  sets the tab order by the order in which you created the tabs." & pc &
                          il & "\i Folder Name\i0  orders by the names of the folders." & pc
                ' ----------------------------------------
            Case Else
                rtf = ""
        End Select

        Return rtf & "}"

    End Function

    ' -----------------------------------------------------------------------------------------------------------
End Class
