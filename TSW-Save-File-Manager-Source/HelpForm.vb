Public Class HelpForm

    ' -----------------------------------------------------------------------------------------------------------
    ' Form load - Help text in RTF format
    ' -----------------------------------------------------------------------------------------------------------
    Private Sub HelpForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim rtf As String =
            "{\rtf1\ansi" &
            "{\pard\sb100\sa150\qc\b TSW Save Manager Help\b0\par}" &
            "{\pard\ql\li130\sa100\ri100 This app allows you to manage multiple Train Sim World save files. Select your version from the drop down " &
            "if you have more than one version installed.\par}" &
            "{\pard\ql\li130\sa100\ri100 You can save and restore TSW save game files as required. Your saved files appear in the Custom Save Files " &
            "list. If the current TSW save file is in your list of saves, the custom file will be highlighted with a tick.\par" &
            "\pard\ql\li130\sa100\ri100 The default order of the list is by file name. Click on a column heading to order by the values in that column. " &
            "Click the same heading again to sort in reverse order.\par}" &
            "{\pard\li130\ri100\sa100 You can run TSW directly from this app by clicking the \b Run TSW\b0  button. This will run the selected version " &
            "of the game. The app will remain open while you run TSW so you can save games on the fly (use Alt-Tab to switch between the game and the " &
            "app). If you create a new save file, the app will automatically refresh the save file details.\par}" &
            "{\pard\li130\ri100\sa150 All saves are stored in a Custom Saves folder in your TSW profile folder. Note: This app has been tested with " &
            "versions TSW3 to TSW6.\par}" &
            "{\pard\li130\sa100\ul Usage\ul0\par}" &
            "{\pard\li250\ri100\sa50\fi-119\bullet  \b Save\b0  current save game files. Enter a meaningful file name into the New Save File Name box, " &
            "then click Save.\par}" &
            "{\pard\li250\ri100\sa50\fi-119\bullet  \b Restore\b0  save game files. Select an existing save file from the custom file list then click " &
            "Restore. You can also restore a save file by double-clicking the file name in the list.\par}" &
            "{\pard\li250\ri100\sa50\fi-119\bullet  \b Rename\b0  saves. Select an existing save file from the list, enter a new name into the New " &
            "Save File Name box, then click Rename.\par}" &
            "{\pard\li250\ri100\sa100\fi-119\bullet  \b Delete\b0  files safely. Select a file in the custom file list then click Delete. The file " &
            "will be moved to the Recycle Bin.\par}" &
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