' $Id: CopyDialog.Designer.vb 1493 2026-09-21 09:08:45Z Pete $
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CopyDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CopyDialog))
        FolderLabel = New TextBox()
        Label1 = New Label()
        CopyToVersion = New ComboBox()
        PromptLabel = New Label()
        CopyToProfile = New ComboBox()
        CloseButton = New Button()
        ActionButton = New Button()
        SuspendLayout()
        ' 
        ' FolderLabel
        ' 
        FolderLabel.BorderStyle = BorderStyle.None
        FolderLabel.Location = New Point(59, 16)
        FolderLabel.Name = "FolderLabel"
        FolderLabel.ReadOnly = True
        FolderLabel.Size = New Size(266, 16)
        FolderLabel.TabIndex = 23
        FolderLabel.TabStop = False
        FolderLabel.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.ImageAlign = ContentAlignment.MiddleRight
        Label1.Location = New Point(19, 52)
        Label1.Name = "Label1"
        Label1.Size = New Size(93, 15)
        Label1.TabIndex = 22
        Label1.Text = "Copy to Version:"
        ' 
        ' CopyToVersion
        ' 
        CopyToVersion.DropDownStyle = ComboBoxStyle.DropDownList
        CopyToVersion.ForeColor = SystemColors.WindowText
        CopyToVersion.FormattingEnabled = True
        CopyToVersion.Location = New Point(118, 49)
        CopyToVersion.Name = "CopyToVersion"
        CopyToVersion.Size = New Size(138, 23)
        CopyToVersion.TabIndex = 16
        ' 
        ' PromptLabel
        ' 
        PromptLabel.AutoSize = True
        PromptLabel.ImageAlign = ContentAlignment.MiddleRight
        PromptLabel.Location = New Point(68, 85)
        PromptLabel.Name = "PromptLabel"
        PromptLabel.Size = New Size(44, 15)
        PromptLabel.TabIndex = 21
        PromptLabel.Text = "Profile:"
        ' 
        ' CopyToProfile
        ' 
        CopyToProfile.DropDownStyle = ComboBoxStyle.DropDownList
        CopyToProfile.FormattingEnabled = True
        CopyToProfile.Location = New Point(118, 82)
        CopyToProfile.Name = "CopyToProfile"
        CopyToProfile.Size = New Size(246, 23)
        CopyToProfile.TabIndex = 17
        ' 
        ' CloseButton
        ' 
        CloseButton.Location = New Point(289, 122)
        CloseButton.Name = "CloseButton"
        CloseButton.Size = New Size(75, 23)
        CloseButton.TabIndex = 19
        CloseButton.Text = "Close"
        CloseButton.UseVisualStyleBackColor = True
        ' 
        ' ActionButton
        ' 
        ActionButton.Location = New Point(202, 122)
        ActionButton.Name = "ActionButton"
        ActionButton.Size = New Size(75, 23)
        ActionButton.TabIndex = 18
        ActionButton.Text = "Copy"
        ActionButton.UseVisualStyleBackColor = True
        ' 
        ' CopyDialog
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(384, 157)
        Controls.Add(FolderLabel)
        Controls.Add(Label1)
        Controls.Add(CopyToVersion)
        Controls.Add(PromptLabel)
        Controls.Add(CopyToProfile)
        Controls.Add(CloseButton)
        Controls.Add(ActionButton)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "CopyDialog"
        StartPosition = FormStartPosition.CenterParent
        Text = "Copy Folder"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents FolderLabel As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PromptLabel As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents ActionButton As Button
    Public WithEvents CopyToVersion As ComboBox
    Public WithEvents CopyToProfile As ComboBox
End Class
