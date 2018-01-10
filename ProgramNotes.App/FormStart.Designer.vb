<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormStart
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormStart))
        Me.cxMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuStartLine = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuEndLine = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOptionsRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOptionsBrowseFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileEncoding = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEncodingW1251 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEncodingUTF8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEncodingUnicode = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLineSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.pbProgress = New System.Windows.Forms.ProgressBar()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.cxMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'cxMenu
        '
        Me.cxMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuStartLine, Me.mnuEndLine, Me.mnuOptions, Me.mnuLineSeparator, Me.mnuExit})
        Me.cxMenu.Name = "cxMenu"
        Me.cxMenu.Size = New System.Drawing.Size(128, 66)
        '
        'mnuStartLine
        '
        Me.mnuStartLine.Name = "mnuStartLine"
        Me.mnuStartLine.Size = New System.Drawing.Size(124, 6)
        Me.mnuStartLine.Tag = ""
        '
        'mnuEndLine
        '
        Me.mnuEndLine.MergeIndex = 1
        Me.mnuEndLine.Name = "mnuEndLine"
        Me.mnuEndLine.Size = New System.Drawing.Size(124, 6)
        Me.mnuEndLine.Tag = ""
        '
        'mnuOptions
        '
        Me.mnuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOptionsRefresh, Me.mnuOptionsBrowseFolder, Me.mnuFileEncoding})
        Me.mnuOptions.Image = Global.ProgramNotes.Resources.img_options
        Me.mnuOptions.MergeIndex = 3
        Me.mnuOptions.Name = "mnuOptions"
        Me.mnuOptions.ShowShortcutKeys = False
        Me.mnuOptions.Size = New System.Drawing.Size(127, 22)
        Me.mnuOptions.Tag = ""
        Me.mnuOptions.Text = "Настройки"
        '
        'mnuOptionsRefresh
        '
        Me.mnuOptionsRefresh.Name = "mnuOptionsRefresh"
        Me.mnuOptionsRefresh.ShowShortcutKeys = False
        Me.mnuOptionsRefresh.Size = New System.Drawing.Size(239, 22)
        Me.mnuOptionsRefresh.Tag = ""
        Me.mnuOptionsRefresh.Text = "Перечитать все файлы и папки"
        '
        'mnuOptionsBrowseFolder
        '
        Me.mnuOptionsBrowseFolder.Name = "mnuOptionsBrowseFolder"
        Me.mnuOptionsBrowseFolder.ShowShortcutKeys = False
        Me.mnuOptionsBrowseFolder.Size = New System.Drawing.Size(239, 22)
        Me.mnuOptionsBrowseFolder.Tag = ""
        Me.mnuOptionsBrowseFolder.Text = "Открыть папку с заметками"
        '
        'mnuFileEncoding
        '
        Me.mnuFileEncoding.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEncodingW1251, Me.mnuEncodingUTF8, Me.mnuEncodingUnicode})
        Me.mnuFileEncoding.Name = "mnuFileEncoding"
        Me.mnuFileEncoding.Size = New System.Drawing.Size(239, 22)
        Me.mnuFileEncoding.Text = "Кодировка"
        '
        'mnuEncodingW1251
        '
        Me.mnuEncodingW1251.Name = "mnuEncodingW1251"
        Me.mnuEncodingW1251.Size = New System.Drawing.Size(152, 22)
        Me.mnuEncodingW1251.Text = "Windows-1251"
        '
        'mnuEncodingUTF8
        '
        Me.mnuEncodingUTF8.Name = "mnuEncodingUTF8"
        Me.mnuEncodingUTF8.Size = New System.Drawing.Size(152, 22)
        Me.mnuEncodingUTF8.Text = "UTF-8"
        '
        'mnuEncodingUnicode
        '
        Me.mnuEncodingUnicode.Name = "mnuEncodingUnicode"
        Me.mnuEncodingUnicode.Size = New System.Drawing.Size(152, 22)
        Me.mnuEncodingUnicode.Text = "Unicode"
        '
        'mnuLineSeparator
        '
        Me.mnuLineSeparator.Name = "mnuLineSeparator"
        Me.mnuLineSeparator.Size = New System.Drawing.Size(124, 6)
        Me.mnuLineSeparator.Tag = ""
        '
        'mnuExit
        '
        Me.mnuExit.MergeIndex = 4
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.ShowShortcutKeys = False
        Me.mnuExit.Size = New System.Drawing.Size(127, 22)
        Me.mnuExit.Tag = ""
        Me.mnuExit.Text = "Выход"
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(16, 31)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(520, 24)
        Me.pbProgress.TabIndex = 1
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(16, 8)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(11, 14)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "-"
        Me.lblStatus.UseMnemonic = False
        '
        'FormStart
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(548, 66)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.pbProgress)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormStart"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Program Notes"
        Me.cxMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents mnuEndLine As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOptionsRefresh As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOptionsBrowseFolder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuLineSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuStartLine As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents cxMenu As ContextMenuStrip
    Friend WithEvents mnuFileEncoding As ToolStripMenuItem
    Friend WithEvents mnuEncodingW1251 As ToolStripMenuItem
    Friend WithEvents mnuEncodingUTF8 As ToolStripMenuItem
    Friend WithEvents mnuEncodingUnicode As ToolStripMenuItem
End Class
