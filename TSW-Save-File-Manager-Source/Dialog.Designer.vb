<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Dialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dialog))
        ActionButton = New Button()
        CloseButton = New Button()
        TextBox = New TextBox()
        PromptLabel = New Label()
        HeadingLabel = New TextBox()
        SuspendLayout()
        ' 
        ' ActionButton
        ' 
        ActionButton.Location = New Point(198, 95)
        ActionButton.Name = "ActionButton"
        ActionButton.Size = New Size(75, 23)
        ActionButton.TabIndex = 2
        ActionButton.Text = "OK"
        ActionButton.UseVisualStyleBackColor = True
        ' 
        ' CloseButton
        ' 
        CloseButton.Location = New Point(279, 95)
        CloseButton.Name = "CloseButton"
        CloseButton.Size = New Size(75, 23)
        CloseButton.TabIndex = 3
        CloseButton.Text = "Close"
        CloseButton.UseVisualStyleBackColor = True
        ' 
        ' TextBox
        ' 
        TextBox.BorderStyle = BorderStyle.FixedSingle
        TextBox.Location = New Point(108, 52)
        TextBox.Name = "TextBox"
        TextBox.Size = New Size(214, 23)
        TextBox.TabIndex = 1
        ' 
        ' PromptLabel
        ' 
        PromptLabel.AutoSize = True
        PromptLabel.ImageAlign = ContentAlignment.MiddleRight
        PromptLabel.Location = New Point(24, 54)
        PromptLabel.Name = "PromptLabel"
        PromptLabel.Size = New Size(78, 15)
        PromptLabel.TabIndex = 7
        PromptLabel.Text = "Folder Name:"
        ' 
        ' HeadingLabel
        ' 
        HeadingLabel.BorderStyle = BorderStyle.None
        HeadingLabel.Location = New Point(12, 18)
        HeadingLabel.Name = "HeadingLabel"
        HeadingLabel.ReadOnly = True
        HeadingLabel.Size = New Size(342, 16)
        HeadingLabel.TabIndex = 8
        HeadingLabel.TabStop = False
        HeadingLabel.TextAlign = HorizontalAlignment.Center
        ' 
        ' Dialog
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(366, 130)
        Controls.Add(HeadingLabel)
        Controls.Add(PromptLabel)
        Controls.Add(TextBox)
        Controls.Add(CloseButton)
        Controls.Add(ActionButton)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "Dialog"
        StartPosition = FormStartPosition.CenterParent
        Text = "Dialog"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ActionButton As Button
    Friend WithEvents CloseButton As Button
    Friend WithEvents TextBox As TextBox
    Friend WithEvents PromptLabel As Label
    Friend WithEvents HeadingLabel As TextBox
End Class
