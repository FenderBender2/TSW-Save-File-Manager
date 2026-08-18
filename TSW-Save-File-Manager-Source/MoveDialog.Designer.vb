<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MoveDialog
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
        PromptLabel = New Label()
        FolderList = New ComboBox()
        TextBox = New TextBox()
        CloseButton = New Button()
        ActionButton = New Button()
        CopyToProfile = New ComboBox()
        Label1 = New Label()
        HeadingLabel = New TextBox()
        SuspendLayout()
        ' 
        ' PromptLabel
        ' 
        PromptLabel.AutoSize = True
        PromptLabel.ImageAlign = ContentAlignment.MiddleRight
        PromptLabel.Location = New Point(32, 90)
        PromptLabel.Name = "PromptLabel"
        PromptLabel.Size = New Size(78, 15)
        PromptLabel.TabIndex = 12
        PromptLabel.Text = "Folder Name:"
        ' 
        ' FolderList
        ' 
        FolderList.DropDownStyle = ComboBoxStyle.DropDownList
        FolderList.FormattingEnabled = True
        FolderList.Location = New Point(114, 87)
        FolderList.Name = "FolderList"
        FolderList.Size = New Size(138, 23)
        FolderList.TabIndex = 2
        ' 
        ' TextBox
        ' 
        TextBox.BorderStyle = BorderStyle.FixedSingle
        TextBox.Location = New Point(116, 87)
        TextBox.Name = "TextBox"
        TextBox.Size = New Size(136, 23)
        TextBox.TabIndex = 9
        ' 
        ' CloseButton
        ' 
        CloseButton.Location = New Point(303, 127)
        CloseButton.Name = "CloseButton"
        CloseButton.Size = New Size(75, 23)
        CloseButton.TabIndex = 4
        CloseButton.Text = "Close"
        CloseButton.UseVisualStyleBackColor = True
        ' 
        ' ActionButton
        ' 
        ActionButton.Location = New Point(222, 127)
        ActionButton.Name = "ActionButton"
        ActionButton.Size = New Size(75, 23)
        ActionButton.TabIndex = 3
        ActionButton.Text = "Save"
        ActionButton.UseVisualStyleBackColor = True
        ' 
        ' CopyToProfile
        ' 
        CopyToProfile.DropDownStyle = ComboBoxStyle.DropDownList
        CopyToProfile.FormattingEnabled = True
        CopyToProfile.Location = New Point(114, 51)
        CopyToProfile.Name = "CopyToProfile"
        CopyToProfile.Size = New Size(250, 23)
        CopyToProfile.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.ImageAlign = ContentAlignment.MiddleRight
        Label1.Location = New Point(19, 54)
        Label1.Name = "Label1"
        Label1.Size = New Size(91, 15)
        Label1.TabIndex = 14
        Label1.Text = "Move to Profile:"
        ' 
        ' HeadingLabel
        ' 
        HeadingLabel.BorderStyle = BorderStyle.None
        HeadingLabel.Location = New Point(64, 17)
        HeadingLabel.Name = "HeadingLabel"
        HeadingLabel.ReadOnly = True
        HeadingLabel.Size = New Size(266, 16)
        HeadingLabel.TabIndex = 15
        HeadingLabel.TabStop = False
        HeadingLabel.TextAlign = HorizontalAlignment.Center
        ' 
        ' MoveDialog
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(391, 162)
        Controls.Add(HeadingLabel)
        Controls.Add(Label1)
        Controls.Add(CopyToProfile)
        Controls.Add(PromptLabel)
        Controls.Add(FolderList)
        Controls.Add(TextBox)
        Controls.Add(CloseButton)
        Controls.Add(ActionButton)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        MinimizeBox = False
        Name = "MoveDialog"
        StartPosition = FormStartPosition.CenterParent
        Text = "MoveDialog"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PromptLabel As Label
    Friend WithEvents FolderList As ComboBox
    Friend WithEvents TextBox As TextBox
    Friend WithEvents CloseButton As Button
    Friend WithEvents ActionButton As Button
    Friend WithEvents CopyToProfile As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents HeadingLabel As TextBox
End Class
