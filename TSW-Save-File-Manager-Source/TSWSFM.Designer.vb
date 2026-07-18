<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TSWSFM
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TSWSFM))
        Label1 = New Label()
        CurrentFolder = New TextBox()
        Label2 = New Label()
        SaveFileName = New TextBox()
        SaveButton = New Button()
        NewFileName = New TextBox()
        Label3 = New Label()
        VersionSelect = New ComboBox()
        Label5 = New Label()
        GroupBox1 = New GroupBox()
        StatusMessage = New Label()
        FileNotSaved = New Label()
        SysTimeStamp = New TextBox()
        RenameButton = New Button()
        Label4 = New Label()
        SaveDate = New TextBox()
        RestoreButton = New Button()
        RunButton = New Button()
        CloseButton = New Button()
        DeleteButton = New Button()
        GroupBox2 = New GroupBox()
        CustomFileList = New ListView()
        MenuStrip1 = New MenuStrip()
        HelpToolStripMenuItem = New ToolStripMenuItem()
        mnuHelp = New ToolStripMenuItem()
        MessageTimer = New Timer(components)
        TSWIcon = New PictureBox()
        GroupBox1.SuspendLayout()
        GroupBox2.SuspendLayout()
        MenuStrip1.SuspendLayout()
        CType(TSWIcon, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(15, 23)
        Label1.Name = "Label1"
        Label1.Size = New Size(110, 15)
        Label1.TabIndex = 0
        Label1.Text = "Current Save Folder"
        ' 
        ' CurrentFolder
        ' 
        CurrentFolder.BorderStyle = BorderStyle.FixedSingle
        CurrentFolder.Font = New Font("Segoe UI", 9F)
        CurrentFolder.Location = New Point(15, 41)
        CurrentFolder.Name = "CurrentFolder"
        CurrentFolder.ReadOnly = True
        CurrentFolder.Size = New Size(469, 23)
        CurrentFolder.TabIndex = 1
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(15, 69)
        Label2.Name = "Label2"
        Label2.Size = New Size(87, 15)
        Label2.TabIndex = 2
        Label2.Text = "Save File Name"
        ' 
        ' SaveFileName
        ' 
        SaveFileName.BorderStyle = BorderStyle.FixedSingle
        SaveFileName.Font = New Font("Segoe UI", 9F)
        SaveFileName.Location = New Point(15, 87)
        SaveFileName.Name = "SaveFileName"
        SaveFileName.ReadOnly = True
        SaveFileName.Size = New Size(368, 23)
        SaveFileName.TabIndex = 3
        ' 
        ' SaveButton
        ' 
        SaveButton.Font = New Font("Segoe UI", 9F)
        SaveButton.Location = New Point(412, 163)
        SaveButton.Name = "SaveButton"
        SaveButton.Size = New Size(72, 23)
        SaveButton.TabIndex = 4
        SaveButton.Text = "Save"
        SaveButton.UseVisualStyleBackColor = True
        ' 
        ' NewFileName
        ' 
        NewFileName.BorderStyle = BorderStyle.FixedSingle
        NewFileName.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        NewFileName.Location = New Point(15, 134)
        NewFileName.Name = "NewFileName"
        NewFileName.Size = New Size(469, 23)
        NewFileName.TabIndex = 5
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(15, 116)
        Label3.Name = "Label3"
        Label3.Size = New Size(114, 15)
        Label3.TabIndex = 6
        Label3.Text = "New Save File Name"
        ' 
        ' VersionSelect
        ' 
        VersionSelect.FormattingEnabled = True
        VersionSelect.Location = New Point(139, 39)
        VersionSelect.Name = "VersionSelect"
        VersionSelect.Size = New Size(136, 23)
        VersionSelect.TabIndex = 14
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(24, 42)
        Label5.Name = "Label5"
        Label5.Size = New Size(109, 15)
        Label5.TabIndex = 15
        Label5.Text = "Select TSW Version:"
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(StatusMessage)
        GroupBox1.Controls.Add(FileNotSaved)
        GroupBox1.Controls.Add(SysTimeStamp)
        GroupBox1.Controls.Add(RenameButton)
        GroupBox1.Controls.Add(Label4)
        GroupBox1.Controls.Add(SaveDate)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(NewFileName)
        GroupBox1.Controls.Add(SaveButton)
        GroupBox1.Controls.Add(SaveFileName)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(CurrentFolder)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox1.Location = New Point(8, 79)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(499, 197)
        GroupBox1.TabIndex = 16
        GroupBox1.TabStop = False
        GroupBox1.Tag = ""
        GroupBox1.Text = "  TSW Save File  "
        ' 
        ' StatusMessage
        ' 
        StatusMessage.AutoSize = True
        StatusMessage.ForeColor = SystemColors.HotTrack
        StatusMessage.Location = New Point(15, 160)
        StatusMessage.Name = "StatusMessage"
        StatusMessage.Size = New Size(89, 15)
        StatusMessage.TabIndex = 18
        StatusMessage.Text = "Status message"
        StatusMessage.Visible = False
        ' 
        ' FileNotSaved
        ' 
        FileNotSaved.AutoSize = True
        FileNotSaved.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        FileNotSaved.ForeColor = Color.FromArgb(CByte(180), CByte(0), CByte(0))
        FileNotSaved.Location = New Point(99, 69)
        FileNotSaved.Name = "FileNotSaved"
        FileNotSaved.Size = New Size(95, 15)
        FileNotSaved.TabIndex = 17
        FileNotSaved.Text = "- File Not Saved"
        FileNotSaved.Visible = False
        ' 
        ' SysTimeStamp
        ' 
        SysTimeStamp.BorderStyle = BorderStyle.FixedSingle
        SysTimeStamp.Font = New Font("Segoe UI", 9F)
        SysTimeStamp.Location = New Point(389, 108)
        SysTimeStamp.Name = "SysTimeStamp"
        SysTimeStamp.ReadOnly = True
        SysTimeStamp.Size = New Size(95, 23)
        SysTimeStamp.TabIndex = 16
        SysTimeStamp.Visible = False
        ' 
        ' RenameButton
        ' 
        RenameButton.Font = New Font("Segoe UI", 9F)
        RenameButton.Location = New Point(334, 163)
        RenameButton.Name = "RenameButton"
        RenameButton.Size = New Size(72, 23)
        RenameButton.TabIndex = 15
        RenameButton.Text = "Rename"
        RenameButton.UseVisualStyleBackColor = True
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(389, 69)
        Label4.Name = "Label4"
        Label4.Size = New Size(58, 15)
        Label4.TabIndex = 8
        Label4.Text = "Save Date"
        ' 
        ' SaveDate
        ' 
        SaveDate.BorderStyle = BorderStyle.FixedSingle
        SaveDate.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        SaveDate.Location = New Point(389, 87)
        SaveDate.Name = "SaveDate"
        SaveDate.ReadOnly = True
        SaveDate.Size = New Size(95, 23)
        SaveDate.TabIndex = 7
        ' 
        ' RestoreButton
        ' 
        RestoreButton.Font = New Font("Segoe UI", 9F)
        RestoreButton.Location = New Point(334, 233)
        RestoreButton.Name = "RestoreButton"
        RestoreButton.Size = New Size(72, 23)
        RestoreButton.TabIndex = 9
        RestoreButton.Text = "Restore"
        RestoreButton.UseVisualStyleBackColor = True
        ' 
        ' RunButton
        ' 
        RunButton.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        RunButton.Location = New Point(15, 233)
        RunButton.Name = "RunButton"
        RunButton.Size = New Size(87, 23)
        RunButton.TabIndex = 10
        RunButton.Text = "Run TSW"
        RunButton.UseVisualStyleBackColor = True
        ' 
        ' CloseButton
        ' 
        CloseButton.Font = New Font("Segoe UI", 9F)
        CloseButton.Location = New Point(412, 233)
        CloseButton.Name = "CloseButton"
        CloseButton.Size = New Size(72, 23)
        CloseButton.TabIndex = 11
        CloseButton.Text = "Close"
        CloseButton.UseVisualStyleBackColor = True
        ' 
        ' DeleteButton
        ' 
        DeleteButton.Font = New Font("Segoe UI", 9F)
        DeleteButton.Location = New Point(256, 233)
        DeleteButton.Name = "DeleteButton"
        DeleteButton.Size = New Size(72, 23)
        DeleteButton.TabIndex = 13
        DeleteButton.Text = "Delete"
        DeleteButton.UseVisualStyleBackColor = True
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(CustomFileList)
        GroupBox2.Controls.Add(DeleteButton)
        GroupBox2.Controls.Add(CloseButton)
        GroupBox2.Controls.Add(RunButton)
        GroupBox2.Controls.Add(RestoreButton)
        GroupBox2.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox2.Location = New Point(8, 282)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(499, 265)
        GroupBox2.TabIndex = 17
        GroupBox2.TabStop = False
        GroupBox2.Text = "  Custom Save Files  "
        ' 
        ' CustomFileList
        ' 
        CustomFileList.Font = New Font("Segoe UI", 9F)
        CustomFileList.Location = New Point(15, 28)
        CustomFileList.MultiSelect = False
        CustomFileList.Name = "CustomFileList"
        CustomFileList.Size = New Size(469, 199)
        CustomFileList.TabIndex = 14
        CustomFileList.UseCompatibleStateImageBehavior = False
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {HelpToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(515, 24)
        MenuStrip1.TabIndex = 18
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' HelpToolStripMenuItem
        ' 
        HelpToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {mnuHelp})
        HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        HelpToolStripMenuItem.Size = New Size(52, 20)
        HelpToolStripMenuItem.Text = "About"
        ' 
        ' mnuHelp
        ' 
        mnuHelp.Name = "mnuHelp"
        mnuHelp.Size = New Size(99, 22)
        mnuHelp.Text = "Help"
        ' 
        ' MessageTimer
        ' 
        MessageTimer.Interval = 3500
        ' 
        ' TSWIcon
        ' 
        TSWIcon.Location = New Point(290, 35)
        TSWIcon.Name = "TSWIcon"
        TSWIcon.Size = New Size(37, 37)
        TSWIcon.TabIndex = 19
        TSWIcon.TabStop = False
        ' 
        ' TSWSFM
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(515, 555)
        Controls.Add(TSWIcon)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox1)
        Controls.Add(Label5)
        Controls.Add(VersionSelect)
        Controls.Add(MenuStrip1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MainMenuStrip = MenuStrip1
        Name = "TSWSFM"
        StartPosition = FormStartPosition.CenterScreen
        Text = "TSW Save File Manager"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        CType(TSWIcon, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents CurrentFolder As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents SaveFileName As TextBox
    Friend WithEvents SaveButton As Button
    Friend WithEvents NewFileName As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents VersionSelect As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents RestoreButton As Button
    Friend WithEvents RunButton As Button
    Friend WithEvents CloseButton As Button
    Friend WithEvents DeleteButton As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents CustomFileList As ListView
    Friend WithEvents Label4 As Label
    Friend WithEvents SaveDate As TextBox
    Friend WithEvents RenameButton As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents mnuHelp As ToolStripMenuItem
    Friend WithEvents SysTimeStamp As TextBox
    Friend WithEvents FileNotSaved As Label
    Friend WithEvents StatusMessage As Label
    Friend WithEvents MessageTimer As Timer
    Friend WithEvents TSWIcon As PictureBox

End Class
