Public Class HelpForm

    ' -----------------------------------------------------------------------------------------------------------
    ' Form load - Help text in RTF format
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HelpForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim c As Color = ColorTranslator.FromHtml("#005500") ' Dark green

        Dim rtf As String =
          "{\rtf1\ansi{\colortbl;\red" & c.R & "\green" & c.G & "\blue" & c.B & ";}" &
            "{\pard\sb100\sa150\qc\b TSW Save Manager " & Application.ProductVersion & " Help\b0\par}" &
            "{\pard\ql\li130\sa100\ri100 This app allows you to manage multiple Train Sim World save files. Select your version from the drop down " &
               "if you have more than one version installed. Your selection will be remembered for the next time you start the app.\par}"

        rtf &= "{\pard\li130\sa100\b\ul Saved Files\ul0\b0\par}" &
            "{\pard\ql\li130\sa100\ri100 You can save and restore TSW save files as required. Your saved files appear in the Custom Save Files " &
               "list. If the current TSW file is in your custom list, the custom file will be highlighted with a tick.\par" &
            "{\pard\li130\ri100\sa150 All your files are stored in a Custom Saves folder in your TSW profile folder (usually \i My Documents\\My Games" &
               "\\TrainSimWorldx\i0 ).\par}"

        rtf &= "{\pard\li130\sa100\b\ul Custom File List Order\ul0\b0\par}" &
            "\pard\ql\li130\sa100\ri100 The default order of the list is by file name. Click on a column heading to order by the values in that column. " &
               "Click the same heading again to sort in reverse order. Your sort order will be remembered for the next time you start the app.\par}"

        rtf &= "{\pard\li130\sa100\b\ul Running TSW\ul0\b0\par}" &
            "{\pard\li130\ri100\sa100 You can run TSW directly from this app by clicking the \cf1\b Run TSW\b0\cf0  button. This will run the selected version " &
               "of the game. The app will remain open while you run TSW so you can save games on the fly (use Alt-Tab to switch between the game and the " &
               "app). If you create a new save file, the app will automatically refresh the save file details.\par}"

        rtf &= "{\pard\li130\sa100\b\ul Usage\ul0\b0\par}" &
            "{\pard\li250\ri100\sa50\fi-119\bullet  \cf1\b Save\b0\cf0  current save file. Enter a meaningful file name into the New Save File Name box, " &
               "then click Save.\par}" &
            "{\pard\li250\ri100\sa50\fi-119\bullet  \cf1\b Restore\b0\cf0  save file. Select an existing save file from the custom file list then click " &
               "Restore. You can also restore a save file by double-clicking the file name in the list.\par}" &
            "{\pard\li250\ri100\sa50\fi-119\bullet  \cf1\b Rename\b0\cf0  save file. Select an existing save file from the list, enter a new name into the New " &
               "Save File Name box, then click Rename.\par}" &
            "{\pard\li250\ri100\sa100\fi-119\bullet  \cf1\b Delete\b0\cf0  file safely. Select a file in the custom file list then click Delete. The file " &
               "will be moved to the Recycle Bin.\par}" &
            "{\pard\li250\ri100\sa100\fi-119\bullet  \cf1\b Move\b0\cf0  files to other folders. Select a file in the custom file list then click Move to " &
               "display the Move File dialog. Select a folder from the drop-down list in the dialog and click Move. The file will be moved to your chosen " &
               "folder. See the section below on folders.\par}"

        rtf &= "{\pard\li130\sa100\b\ul Folders\ul0\b0\par}" &
            "{\pard\li130\ri100\sa100 To help with the management of save files you can create folders to group your files. When you create a folder a tab " &
               "is added above the custom file list.\par}" &
            "{\pard\li130\ri100\sa100 There is a menu of controls for maintaining folders. Click on \cf1\b Save Folders\b0\cf0  in the menu bar. You can also " &
               "right-click on the tabs to rename or delete them.\par}" &
            "{\pard\li130\ri100\sa100 Save files can be saved directly to any folder, or moved between folders as required. Drag & Drop is enabled " &
               "allowing you to drag file names from the custom file list to drop them on a folder tab. Alternatively, you can use the Move button. If " &
               "a file is moved to a location where another file with the same name exists, the moved file will be renamed with an incremental suffix. " &
               "e.g. (1), (2) etc..\par}" &
            "{\pard\li130\ri100\sa100 Each version has its own set of folders. The Main folder is the parent Custom Save folder - it cannot be renamed " &
               "or deleted.\par}"

        rtf &= "{\pard\li130\sa100\b\ul Folder Usage\ul0\b0\par}" &
            "{\pard\li250\ri100\sa50\fi-119\bullet  \cf1\b New Folder\b0\cf0  - To create a new folder, select the option from the menu, provide a name for " &
               "the folder in the dialog box then click Create.\par}" &
            "{\pard\li250\ri100\sa50\fi-119\bullet  \cf1\b Rename Folder\b0\cf0  - To rename a folder, make the folder active by clicking the tab then select " &
               "\i Rename Folder\i0  from the menu. Next, provide a new name for the folder in the dialog box then click Rename.\par}" &
            "{\pard\li250\ri100\sa30\fi-119\bullet  \cf1\b Delete Folder\b0\cf0  - To delete a folder, click its tab then select \i Delete Folder\i0 . If the " &
               "folder contains any save files you will be asked if you want to move them to the Main folder.\par}" &
            "{\pard\li370\ri100\fi-119 - If you choose \cf1\b Yes\b0\cf0  the files are moved and the folder is deleted, including any non-save files that may " &
               "be present in the folder.\par}" &
            "{\pard\li370\ri100\sa100\fi-119 - If you choose \cf1\b No\b0\cf0  the folder and all of its files are moved to the recycle bin.\par}" &
            "{\pard\li250\ri100\sa30\fi-119\bullet  \cf1\b Order Tabs By\b0\cf0  - Change the order of the tabs using this option.\par}" &
            "{\pard\li370\ri100\fi-119 - \i Created Date\i0  sets the tab order by the order " &
               "in which you created the tabs.\par}" &
            "{\pardli370\ri100\sa50\fi-119 - \i Folder Name\i0  orders by the names of the folders.\par}" &
          "}"

        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        HelpText.Rtf = rtf

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Cloe button
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.Close()
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
End Class
