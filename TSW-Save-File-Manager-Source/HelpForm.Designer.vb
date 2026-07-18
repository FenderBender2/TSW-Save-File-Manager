<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HelpForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Label1 = New Label()
        CloseButton = New Button()
        HelpText = New RichTextBox()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(30, 21)
        Label1.Name = "Label1"
        Label1.Size = New Size(0, 15)
        Label1.TabIndex = 0
        ' 
        ' CloseButton
        ' 
        CloseButton.Location = New Point(313, 320)
        CloseButton.Name = "CloseButton"
        CloseButton.Size = New Size(75, 23)
        CloseButton.TabIndex = 1
        CloseButton.Text = "Close"
        CloseButton.UseVisualStyleBackColor = True
        ' 
        ' HelpText
        ' 
        HelpText.BorderStyle = BorderStyle.None
        HelpText.Location = New Point(12, 12)
        HelpText.Name = "HelpText"
        HelpText.ReadOnly = True
        HelpText.Size = New Size(376, 298)
        HelpText.TabIndex = 2
        HelpText.Text = ""
        ' 
        ' HelpForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(401, 351)
        Controls.Add(HelpText)
        Controls.Add(CloseButton)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximumSize = New Size(417, 390)
        MinimumSize = New Size(417, 390)
        Name = "HelpForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "TSW Save Game Manager"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents HelpText As RichTextBox
End Class
