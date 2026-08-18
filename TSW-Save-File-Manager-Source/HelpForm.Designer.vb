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
        MenuTab = New TabControl()
        TabPage1 = New TabPage()
        TabPage2 = New TabPage()
        TabPage3 = New TabPage()
        TabPage4 = New TabPage()
        CopyRight = New Label()
        MenuTab.SuspendLayout()
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
        CloseButton.Location = New Point(415, 466)
        CloseButton.Name = "CloseButton"
        CloseButton.Size = New Size(75, 23)
        CloseButton.TabIndex = 0
        CloseButton.Text = "Close"
        CloseButton.UseVisualStyleBackColor = True
        ' 
        ' HelpText
        ' 
        HelpText.BorderStyle = BorderStyle.None
        HelpText.Font = New Font("Aptos Display", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        HelpText.Location = New Point(12, 33)
        HelpText.Name = "HelpText"
        HelpText.ReadOnly = True
        HelpText.Size = New Size(478, 422)
        HelpText.TabIndex = 12
        HelpText.Text = ""
        ' 
        ' MenuTab
        ' 
        MenuTab.Controls.Add(TabPage1)
        MenuTab.Controls.Add(TabPage2)
        MenuTab.Controls.Add(TabPage3)
        MenuTab.Controls.Add(TabPage4)
        MenuTab.Location = New Point(12, 9)
        MenuTab.Name = "MenuTab"
        MenuTab.SelectedIndex = 0
        MenuTab.Size = New Size(478, 23)
        MenuTab.TabIndex = 1
        MenuTab.TabStop = False
        ' 
        ' TabPage1
        ' 
        TabPage1.Location = New Point(4, 24)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(3)
        TabPage1.Size = New Size(470, 0)
        TabPage1.TabIndex = 0
        TabPage1.Text = " Overview "
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' TabPage2
        ' 
        TabPage2.Location = New Point(4, 24)
        TabPage2.Name = "TabPage2"
        TabPage2.Padding = New Padding(3)
        TabPage2.Size = New Size(470, 0)
        TabPage2.TabIndex = 1
        TabPage2.Text = " Usage "
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' TabPage3
        ' 
        TabPage3.Location = New Point(4, 24)
        TabPage3.Name = "TabPage3"
        TabPage3.Size = New Size(470, 0)
        TabPage3.TabIndex = 2
        TabPage3.Text = " Profiles "
        TabPage3.UseVisualStyleBackColor = True
        ' 
        ' TabPage4
        ' 
        TabPage4.Location = New Point(4, 24)
        TabPage4.Name = "TabPage4"
        TabPage4.Size = New Size(470, 0)
        TabPage4.TabIndex = 3
        TabPage4.Text = " Folders "
        TabPage4.UseVisualStyleBackColor = True
        ' 
        ' CopyRight
        ' 
        CopyRight.AutoSize = True
        CopyRight.Font = New Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        CopyRight.Location = New Point(22, 470)
        CopyRight.Name = "CopyRight"
        CopyRight.Size = New Size(221, 13)
        CopyRight.TabIndex = 4
        CopyRight.Text = "Copyright © P. Lewis 2026. All rights reserved."
        ' 
        ' HelpForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(502, 499)
        Controls.Add(CopyRight)
        Controls.Add(HelpText)
        Controls.Add(CloseButton)
        Controls.Add(Label1)
        Controls.Add(MenuTab)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        MinimizeBox = False
        Name = "HelpForm"
        StartPosition = FormStartPosition.Manual
        Text = "TSW Save File Manager"
        MenuTab.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents HelpText As RichTextBox
    Friend WithEvents MenuTab As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents CopyRight As Label
End Class
