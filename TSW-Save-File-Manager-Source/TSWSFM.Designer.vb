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
        Label4 = New Label()
        Label7 = New Label()
        SavedAsFileName = New TextBox()
        SaveLocation = New TextBox()
        SysTimeStamp = New TextBox()
        SaveDate = New TextBox()
        StatusMessage = New RichTextBox()
        RenameButton = New Button()
        RestoreButton = New Button()
        RunButton = New Button()
        CloseButton = New Button()
        DeleteButton = New Button()
        GroupBox2 = New GroupBox()
        FileCount = New Label()
        MoveButton = New Button()
        CustomFileList = New ListView()
        imgHeaderArrows = New ImageList(components)
        TabControl = New TabControl()
        TabPage1 = New TabPage()
        TabMenu = New ContextMenuStrip(components)
        RenameTab = New ToolStripMenuItem()
        DeleteTab = New ToolStripMenuItem()
        MenuStrip1 = New MenuStrip()
        MnuAction = New ToolStripMenuItem()
        MnuNewTab = New ToolStripMenuItem()
        MnuRenameTab = New ToolStripMenuItem()
        MnuDeleteTab = New ToolStripMenuItem()
        ToolStripSeparator1 = New ToolStripSeparator()
        MenuOrderTabs = New ToolStripMenuItem()
        MenuCreationDate = New ToolStripMenuItem()
        MenuFileName = New ToolStripMenuItem()
        MnuProfile = New ToolStripMenuItem()
        MnuRenameProfile = New ToolStripMenuItem()
        MenuAbout = New ToolStripMenuItem()
        MnuHelp = New ToolStripMenuItem()
        MessageTimer = New Timer(components)
        TSWIcon = New PictureBox()
        GroupBox3 = New GroupBox()
        Label6 = New Label()
        ProfileSelect = New ComboBox()
        FadeTimer = New Timer(components)
        GroupBox4 = New GroupBox()
        GroupBox1.SuspendLayout()
        GroupBox2.SuspendLayout()
        TabControl.SuspendLayout()
        TabMenu.SuspendLayout()
        MenuStrip1.SuspendLayout()
        CType(TSWIcon, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox3.SuspendLayout()
        GroupBox4.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(16, 25)
        Label1.Name = "Label1"
        Label1.Size = New Size(67, 15)
        Label1.TabIndex = 0
        Label1.Text = "Save Folder"
        ' 
        ' CurrentFolder
        ' 
        CurrentFolder.BorderStyle = BorderStyle.FixedSingle
        CurrentFolder.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        CurrentFolder.Location = New Point(85, 23)
        CurrentFolder.Name = "CurrentFolder"
        CurrentFolder.ReadOnly = True
        CurrentFolder.Size = New Size(399, 23)
        CurrentFolder.TabIndex = 4
        CurrentFolder.TabStop = False
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(16, 54)
        Label2.Name = "Label2"
        Label2.Size = New Size(52, 15)
        Label2.TabIndex = 2
        Label2.Text = "Save File"
        ' 
        ' SaveFileName
        ' 
        SaveFileName.BorderStyle = BorderStyle.FixedSingle
        SaveFileName.Font = New Font("Segoe UI", 9F)
        SaveFileName.Location = New Point(85, 52)
        SaveFileName.Name = "SaveFileName"
        SaveFileName.ReadOnly = True
        SaveFileName.Size = New Size(297, 23)
        SaveFileName.TabIndex = 5
        SaveFileName.TabStop = False
        ' 
        ' SaveButton
        ' 
        SaveButton.Font = New Font("Segoe UI", 9F)
        SaveButton.Location = New Point(412, 50)
        SaveButton.Name = "SaveButton"
        SaveButton.Size = New Size(72, 23)
        SaveButton.TabIndex = 10
        SaveButton.Text = "Save"
        SaveButton.UseVisualStyleBackColor = True
        ' 
        ' NewFileName
        ' 
        NewFileName.BorderStyle = BorderStyle.FixedSingle
        NewFileName.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        NewFileName.Location = New Point(85, 21)
        NewFileName.Name = "NewFileName"
        NewFileName.Size = New Size(399, 23)
        NewFileName.TabIndex = 8
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(16, 23)
        Label3.Name = "Label3"
        Label3.Size = New Size(60, 15)
        Label3.TabIndex = 6
        Label3.Text = "File Name"
        ' 
        ' VersionSelect
        ' 
        VersionSelect.DropDownStyle = ComboBoxStyle.DropDownList
        VersionSelect.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        VersionSelect.FormattingEnabled = True
        VersionSelect.Location = New Point(130, 20)
        VersionSelect.Name = "VersionSelect"
        VersionSelect.Size = New Size(136, 23)
        VersionSelect.TabIndex = 0
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(16, 23)
        Label5.Name = "Label5"
        Label5.Size = New Size(109, 15)
        Label5.TabIndex = 15
        Label5.Text = "Select TSW Version:"
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(Label4)
        GroupBox1.Controls.Add(Label7)
        GroupBox1.Controls.Add(SavedAsFileName)
        GroupBox1.Controls.Add(SaveLocation)
        GroupBox1.Controls.Add(SysTimeStamp)
        GroupBox1.Controls.Add(SaveDate)
        GroupBox1.Controls.Add(SaveFileName)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(CurrentFolder)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox1.Location = New Point(8, 117)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(499, 119)
        GroupBox1.TabIndex = 16
        GroupBox1.TabStop = False
        GroupBox1.Tag = ""
        GroupBox1.Text = "  TSW Save File  "
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(347, 83)
        Label4.Name = "Label4"
        Label4.Size = New Size(40, 15)
        Label4.TabIndex = 25
        Label4.Text = "Folder"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label7.Location = New Point(16, 83)
        Label7.Name = "Label7"
        Label7.Size = New Size(54, 15)
        Label7.TabIndex = 24
        Label7.Text = "Saved As"
        ' 
        ' SavedAsFileName
        ' 
        SavedAsFileName.BackColor = SystemColors.Control
        SavedAsFileName.BorderStyle = BorderStyle.FixedSingle
        SavedAsFileName.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        SavedAsFileName.ForeColor = Color.Maroon
        SavedAsFileName.Location = New Point(85, 81)
        SavedAsFileName.Name = "SavedAsFileName"
        SavedAsFileName.ReadOnly = True
        SavedAsFileName.Size = New Size(252, 23)
        SavedAsFileName.TabIndex = 6
        SavedAsFileName.TabStop = False
        ' 
        ' SaveLocation
        ' 
        SaveLocation.BackColor = SystemColors.Control
        SaveLocation.BorderStyle = BorderStyle.FixedSingle
        SaveLocation.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        SaveLocation.ForeColor = Color.Maroon
        SaveLocation.Location = New Point(389, 81)
        SaveLocation.Name = "SaveLocation"
        SaveLocation.ReadOnly = True
        SaveLocation.Size = New Size(95, 23)
        SaveLocation.TabIndex = 7
        SaveLocation.TabStop = False
        ' 
        ' SysTimeStamp
        ' 
        SysTimeStamp.BorderStyle = BorderStyle.FixedSingle
        SysTimeStamp.Font = New Font("Segoe UI", 9F)
        SysTimeStamp.Location = New Point(389, 103)
        SysTimeStamp.Name = "SysTimeStamp"
        SysTimeStamp.ReadOnly = True
        SysTimeStamp.Size = New Size(95, 23)
        SysTimeStamp.TabIndex = 16
        SysTimeStamp.TabStop = False
        SysTimeStamp.Visible = False
        ' 
        ' SaveDate
        ' 
        SaveDate.BorderStyle = BorderStyle.FixedSingle
        SaveDate.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        SaveDate.Location = New Point(389, 52)
        SaveDate.Name = "SaveDate"
        SaveDate.ReadOnly = True
        SaveDate.Size = New Size(95, 23)
        SaveDate.TabIndex = 5
        SaveDate.TabStop = False
        ' 
        ' StatusMessage
        ' 
        StatusMessage.BorderStyle = BorderStyle.None
        StatusMessage.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        StatusMessage.ForeColor = SystemColors.HotTrack
        StatusMessage.Location = New Point(85, 46)
        StatusMessage.Name = "StatusMessage"
        StatusMessage.ReadOnly = True
        StatusMessage.ScrollBars = RichTextBoxScrollBars.None
        StatusMessage.Size = New Size(236, 30)
        StatusMessage.TabIndex = 22
        StatusMessage.Text = "Status Message"
        StatusMessage.Visible = False
        ' 
        ' RenameButton
        ' 
        RenameButton.Font = New Font("Segoe UI", 9F)
        RenameButton.Location = New Point(334, 50)
        RenameButton.Name = "RenameButton"
        RenameButton.Size = New Size(72, 23)
        RenameButton.TabIndex = 9
        RenameButton.Text = "Rename"
        RenameButton.UseVisualStyleBackColor = True
        ' 
        ' RestoreButton
        ' 
        RestoreButton.Font = New Font("Segoe UI", 9F)
        RestoreButton.Location = New Point(334, 249)
        RestoreButton.Name = "RestoreButton"
        RestoreButton.Size = New Size(72, 23)
        RestoreButton.TabIndex = 15
        RestoreButton.Text = "Restore"
        RestoreButton.UseVisualStyleBackColor = True
        ' 
        ' RunButton
        ' 
        RunButton.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        RunButton.Location = New Point(413, 53)
        RunButton.Name = "RunButton"
        RunButton.Size = New Size(72, 23)
        RunButton.TabIndex = 3
        RunButton.Text = "Run TSW"
        RunButton.UseVisualStyleBackColor = True
        ' 
        ' CloseButton
        ' 
        CloseButton.Font = New Font("Segoe UI", 9F)
        CloseButton.Location = New Point(412, 249)
        CloseButton.Name = "CloseButton"
        CloseButton.Size = New Size(72, 23)
        CloseButton.TabIndex = 16
        CloseButton.Text = "Close"
        CloseButton.UseVisualStyleBackColor = True
        ' 
        ' DeleteButton
        ' 
        DeleteButton.Font = New Font("Segoe UI", 9F)
        DeleteButton.Location = New Point(178, 250)
        DeleteButton.Name = "DeleteButton"
        DeleteButton.Size = New Size(72, 23)
        DeleteButton.TabIndex = 13
        DeleteButton.Text = "Delete"
        DeleteButton.UseVisualStyleBackColor = True
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(FileCount)
        GroupBox2.Controls.Add(MoveButton)
        GroupBox2.Controls.Add(CustomFileList)
        GroupBox2.Controls.Add(TabControl)
        GroupBox2.Controls.Add(DeleteButton)
        GroupBox2.Controls.Add(CloseButton)
        GroupBox2.Controls.Add(RestoreButton)
        GroupBox2.FlatStyle = FlatStyle.Flat
        GroupBox2.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox2.Location = New Point(8, 330)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(499, 282)
        GroupBox2.TabIndex = 17
        GroupBox2.TabStop = False
        GroupBox2.Text = "  Custom Save Files  "
        ' 
        ' FileCount
        ' 
        FileCount.AutoSize = True
        FileCount.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        FileCount.Location = New Point(18, 254)
        FileCount.Name = "FileCount"
        FileCount.Size = New Size(93, 15)
        FileCount.TabIndex = 15
        FileCount.Text = "Total File Count:"
        ' 
        ' MoveButton
        ' 
        MoveButton.Font = New Font("Segoe UI", 9F)
        MoveButton.Location = New Point(256, 249)
        MoveButton.Name = "MoveButton"
        MoveButton.Size = New Size(72, 23)
        MoveButton.TabIndex = 14
        MoveButton.Text = "Move"
        MoveButton.UseVisualStyleBackColor = True
        ' 
        ' CustomFileList
        ' 
        CustomFileList.Font = New Font("Segoe UI", 9F)
        CustomFileList.FullRowSelect = True
        CustomFileList.Location = New Point(16, 43)
        CustomFileList.MultiSelect = False
        CustomFileList.Name = "CustomFileList"
        CustomFileList.Size = New Size(469, 199)
        CustomFileList.SmallImageList = imgHeaderArrows
        CustomFileList.TabIndex = 12
        CustomFileList.UseCompatibleStateImageBehavior = False
        CustomFileList.View = View.Details
        ' 
        ' imgHeaderArrows
        ' 
        imgHeaderArrows.ColorDepth = ColorDepth.Depth32Bit
        imgHeaderArrows.ImageSize = New Size(16, 16)
        imgHeaderArrows.TransparentColor = Color.Transparent
        ' 
        ' TabControl
        ' 
        TabControl.AllowDrop = True
        TabControl.Controls.Add(TabPage1)
        TabControl.Location = New Point(16, 21)
        TabControl.Name = "TabControl"
        TabControl.SelectedIndex = 0
        TabControl.Size = New Size(469, 36)
        TabControl.TabIndex = 11
        TabControl.TabStop = False
        ' 
        ' TabPage1
        ' 
        TabPage1.Location = New Point(4, 24)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(3)
        TabPage1.Size = New Size(461, 8)
        TabPage1.TabIndex = 0
        TabPage1.Text = "Main"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' TabMenu
        ' 
        TabMenu.Items.AddRange(New ToolStripItem() {RenameTab, DeleteTab})
        TabMenu.Name = "ContextMenuStrip1"
        TabMenu.Size = New Size(118, 48)
        ' 
        ' RenameTab
        ' 
        RenameTab.Name = "RenameTab"
        RenameTab.Size = New Size(117, 22)
        RenameTab.Text = "&Rename"
        ' 
        ' DeleteTab
        ' 
        DeleteTab.Name = "DeleteTab"
        DeleteTab.Size = New Size(117, 22)
        DeleteTab.Text = "&Delete"
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {MnuAction, MnuProfile, MenuAbout})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(515, 24)
        MenuStrip1.TabIndex = 18
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' MnuAction
        ' 
        MnuAction.DropDownItems.AddRange(New ToolStripItem() {MnuNewTab, MnuRenameTab, MnuDeleteTab, ToolStripSeparator1, MenuOrderTabs})
        MnuAction.Name = "MnuAction"
        MnuAction.Size = New Size(84, 20)
        MnuAction.Text = "&Save Folders"
        ' 
        ' MnuNewTab
        ' 
        MnuNewTab.Name = "MnuNewTab"
        MnuNewTab.Size = New Size(153, 22)
        MnuNewTab.Text = "&New Folder"
        ' 
        ' MnuRenameTab
        ' 
        MnuRenameTab.Name = "MnuRenameTab"
        MnuRenameTab.Size = New Size(153, 22)
        MnuRenameTab.Text = "&Rename Folder"
        ' 
        ' MnuDeleteTab
        ' 
        MnuDeleteTab.Name = "MnuDeleteTab"
        MnuDeleteTab.Size = New Size(153, 22)
        MnuDeleteTab.Text = "&Delete Folder"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(150, 6)
        ' 
        ' MenuOrderTabs
        ' 
        MenuOrderTabs.DropDownItems.AddRange(New ToolStripItem() {MenuCreationDate, MenuFileName})
        MenuOrderTabs.Name = "MenuOrderTabs"
        MenuOrderTabs.Size = New Size(153, 22)
        MenuOrderTabs.Text = "Order Tabs By"
        ' 
        ' MenuCreationDate
        ' 
        MenuCreationDate.Name = "MenuCreationDate"
        MenuCreationDate.Size = New Size(146, 22)
        MenuCreationDate.Text = "Creation Date"
        ' 
        ' MenuFileName
        ' 
        MenuFileName.Name = "MenuFileName"
        MenuFileName.Size = New Size(146, 22)
        MenuFileName.Text = "Folder Name"
        ' 
        ' MnuProfile
        ' 
        MnuProfile.DropDownItems.AddRange(New ToolStripItem() {MnuRenameProfile})
        MnuProfile.Name = "MnuProfile"
        MnuProfile.Size = New Size(53, 20)
        MnuProfile.Text = "Profile"
        ' 
        ' MnuRenameProfile
        ' 
        MnuRenameProfile.Name = "MnuRenameProfile"
        MnuRenameProfile.Size = New Size(154, 22)
        MnuRenameProfile.Text = "Rename Profile"
        ' 
        ' MenuAbout
        ' 
        MenuAbout.DropDownItems.AddRange(New ToolStripItem() {MnuHelp})
        MenuAbout.Name = "MenuAbout"
        MenuAbout.Size = New Size(52, 20)
        MenuAbout.Text = "&About"
        ' 
        ' MnuHelp
        ' 
        MnuHelp.Name = "MnuHelp"
        MnuHelp.Size = New Size(99, 22)
        MnuHelp.Text = "Help"
        ' 
        ' MessageTimer
        ' 
        MessageTimer.Interval = 3500
        ' 
        ' TSWIcon
        ' 
        TSWIcon.BorderStyle = BorderStyle.FixedSingle
        TSWIcon.Location = New Point(348, 14)
        TSWIcon.Name = "TSWIcon"
        TSWIcon.Size = New Size(34, 34)
        TSWIcon.TabIndex = 19
        TSWIcon.TabStop = False
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Controls.Add(Label6)
        GroupBox3.Controls.Add(ProfileSelect)
        GroupBox3.Controls.Add(TSWIcon)
        GroupBox3.Controls.Add(RunButton)
        GroupBox3.Controls.Add(Label5)
        GroupBox3.Controls.Add(VersionSelect)
        GroupBox3.Location = New Point(8, 25)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(499, 86)
        GroupBox3.TabIndex = 20
        GroupBox3.TabStop = False
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(46, 56)
        Label6.Name = "Label6"
        Label6.Size = New Size(78, 15)
        Label6.TabIndex = 21
        Label6.Text = "Select Profile:"
        ' 
        ' ProfileSelect
        ' 
        ProfileSelect.DropDownStyle = ComboBoxStyle.DropDownList
        ProfileSelect.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ProfileSelect.FormattingEnabled = True
        ProfileSelect.Location = New Point(130, 53)
        ProfileSelect.Name = "ProfileSelect"
        ProfileSelect.Size = New Size(252, 23)
        ProfileSelect.TabIndex = 2
        ' 
        ' FadeTimer
        ' 
        FadeTimer.Interval = 50
        ' 
        ' GroupBox4
        ' 
        GroupBox4.Controls.Add(StatusMessage)
        GroupBox4.Controls.Add(RenameButton)
        GroupBox4.Controls.Add(SaveButton)
        GroupBox4.Controls.Add(Label3)
        GroupBox4.Controls.Add(NewFileName)
        GroupBox4.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox4.Location = New Point(8, 242)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.Size = New Size(499, 82)
        GroupBox4.TabIndex = 23
        GroupBox4.TabStop = False
        GroupBox4.Text = "  New Save File  "
        ' 
        ' TSWSFM
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(515, 620)
        Controls.Add(GroupBox4)
        Controls.Add(GroupBox3)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox1)
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
        GroupBox2.PerformLayout()
        TabControl.ResumeLayout(False)
        TabMenu.ResumeLayout(False)
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        CType(TSWIcon, ComponentModel.ISupportInitialize).EndInit()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        GroupBox4.ResumeLayout(False)
        GroupBox4.PerformLayout()
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
    Friend WithEvents SaveDate As TextBox
    Friend WithEvents RenameButton As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents MenuAbout As ToolStripMenuItem
    Friend WithEvents MnuHelp As ToolStripMenuItem
    Friend WithEvents SysTimeStamp As TextBox
    Friend WithEvents MessageTimer As Timer
    Friend WithEvents TSWIcon As PictureBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents imgHeaderArrows As ImageList
    Friend WithEvents TabControl As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents MnuAction As ToolStripMenuItem
    Friend WithEvents MnuNewTab As ToolStripMenuItem
    Friend WithEvents MnuRenameTab As ToolStripMenuItem
    Friend WithEvents MnuDeleteTab As ToolStripMenuItem
    Friend WithEvents RenameTab As ToolStripMenuItem
    Friend WithEvents SaveLocation As TextBox
    Friend WithEvents MoveButton As Button
    Friend WithEvents StatusMessage As RichTextBox
    Friend WithEvents TabMenu As ContextMenuStrip
    Friend WithEvents DeleteTab As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents MenuOrderTabs As ToolStripMenuItem
    Friend WithEvents MenuCreationDate As ToolStripMenuItem
    Friend WithEvents MenuFileName As ToolStripMenuItem
    Friend WithEvents FadeTimer As Timer
    Friend WithEvents FileCount As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents SavedAsFileName As TextBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents ProfileSelect As ComboBox
    Friend WithEvents MnuProfile As ToolStripMenuItem
    Friend WithEvents MnuRenameProfile As ToolStripMenuItem

End Class
