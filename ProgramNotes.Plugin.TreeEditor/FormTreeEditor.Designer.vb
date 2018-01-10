<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTreeEditor
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTreeEditor))
        Me.tvFiles = New System.Windows.Forms.TreeView()
        Me.mnuStripMain = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLine0 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFolderNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFolderRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFolderDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLine1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFolderRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtNote = New System.Windows.Forms.TextBox()
        Me.spContainer = New System.Windows.Forms.SplitContainer()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.mnuStripMain.SuspendLayout()
        CType(Me.spContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.spContainer.Panel1.SuspendLayout()
        Me.spContainer.Panel2.SuspendLayout()
        Me.spContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvFiles
        '
        Me.tvFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvFiles.FullRowSelect = True
        Me.tvFiles.ImageKey = "Folder"
        Me.tvFiles.Location = New System.Drawing.Point(0, 0)
        Me.tvFiles.Name = "tvFiles"
        Me.tvFiles.ShowNodeToolTips = True
        Me.tvFiles.Size = New System.Drawing.Size(336, 414)
        Me.tvFiles.TabIndex = 0
        '
        'mnuStripMain
        '
        Me.mnuStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuFolder})
        Me.mnuStripMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuStripMain.Name = "mnuStripMain"
        Me.mnuStripMain.Padding = New System.Windows.Forms.Padding(7, 2, 0, 2)
        Me.mnuStripMain.Size = New System.Drawing.Size(924, 24)
        Me.mnuStripMain.TabIndex = 1
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileNew, Me.mnuFileSave, Me.mnuFileRename, Me.mnuFileDelete, Me.mnuLine0, Me.mnuFileExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(48, 20)
        Me.mnuFile.Text = "&Файл"
        '
        'mnuFileNew
        '
        Me.mnuFileNew.Image = Global.ProgramNotes.Resources.img_file
        Me.mnuFileNew.Name = "mnuFileNew"
        Me.mnuFileNew.Size = New System.Drawing.Size(161, 22)
        Me.mnuFileNew.Text = "Новая заметка"
        '
        'mnuFileSave
        '
        Me.mnuFileSave.Name = "mnuFileSave"
        Me.mnuFileSave.Size = New System.Drawing.Size(161, 22)
        Me.mnuFileSave.Text = "Сохранить"
        '
        'mnuFileRename
        '
        Me.mnuFileRename.Name = "mnuFileRename"
        Me.mnuFileRename.Size = New System.Drawing.Size(161, 22)
        Me.mnuFileRename.Text = "Переименовать"
        '
        'mnuFileDelete
        '
        Me.mnuFileDelete.Name = "mnuFileDelete"
        Me.mnuFileDelete.Size = New System.Drawing.Size(161, 22)
        Me.mnuFileDelete.Text = "Удалить"
        '
        'mnuLine0
        '
        Me.mnuLine0.Name = "mnuLine0"
        Me.mnuLine0.Size = New System.Drawing.Size(158, 6)
        '
        'mnuFileExit
        '
        Me.mnuFileExit.Name = "mnuFileExit"
        Me.mnuFileExit.Size = New System.Drawing.Size(161, 22)
        Me.mnuFileExit.Text = "Выход"
        '
        'mnuFolder
        '
        Me.mnuFolder.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFolderNew, Me.mnuFolderRename, Me.mnuFolderDelete, Me.mnuLine1, Me.mnuFolderRefresh})
        Me.mnuFolder.Name = "mnuFolder"
        Me.mnuFolder.Size = New System.Drawing.Size(54, 20)
        Me.mnuFolder.Text = "&Папки"
        '
        'mnuFolderNew
        '
        Me.mnuFolderNew.Image = Global.ProgramNotes.Resources.img_folder
        Me.mnuFolderNew.Name = "mnuFolderNew"
        Me.mnuFolderNew.Size = New System.Drawing.Size(193, 22)
        Me.mnuFolderNew.Text = "Новая папка"
        '
        'mnuFolderRename
        '
        Me.mnuFolderRename.Name = "mnuFolderRename"
        Me.mnuFolderRename.Size = New System.Drawing.Size(193, 22)
        Me.mnuFolderRename.Text = "Переименовать"
        '
        'mnuFolderDelete
        '
        Me.mnuFolderDelete.Image = Global.ProgramNotes.Resources.img_folder_err
        Me.mnuFolderDelete.Name = "mnuFolderDelete"
        Me.mnuFolderDelete.Size = New System.Drawing.Size(193, 22)
        Me.mnuFolderDelete.Text = "Удалить"
        '
        'mnuLine1
        '
        Me.mnuLine1.Name = "mnuLine1"
        Me.mnuLine1.Size = New System.Drawing.Size(190, 6)
        '
        'mnuFolderRefresh
        '
        Me.mnuFolderRefresh.Name = "mnuFolderRefresh"
        Me.mnuFolderRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.mnuFolderRefresh.Size = New System.Drawing.Size(193, 22)
        Me.mnuFolderRefresh.Text = "Перечитать папки"
        '
        'txtNote
        '
        Me.txtNote.AcceptsReturn = True
        Me.txtNote.AcceptsTab = True
        Me.txtNote.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtNote.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtNote.Location = New System.Drawing.Point(0, 0)
        Me.txtNote.Multiline = True
        Me.txtNote.Name = "txtNote"
        Me.txtNote.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtNote.Size = New System.Drawing.Size(583, 414)
        Me.txtNote.TabIndex = 2
        Me.txtNote.TabStop = False
        '
        'spContainer
        '
        Me.spContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.spContainer.Location = New System.Drawing.Point(0, 24)
        Me.spContainer.Name = "spContainer"
        '
        'spContainer.Panel1
        '
        Me.spContainer.Panel1.Controls.Add(Me.tvFiles)
        Me.spContainer.Panel1MinSize = 250
        '
        'spContainer.Panel2
        '
        Me.spContainer.Panel2.Controls.Add(Me.txtNote)
        Me.spContainer.Panel2.Controls.Add(Me.btnExit)
        Me.spContainer.Size = New System.Drawing.Size(924, 414)
        Me.spContainer.SplitterDistance = 336
        Me.spContainer.SplitterWidth = 5
        Me.spContainer.TabIndex = 3
        '
        'btnExit
        '
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Location = New System.Drawing.Point(515, 8)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(8, 8)
        Me.btnExit.TabIndex = 4
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'FormTreeEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(924, 438)
        Me.Controls.Add(Me.spContainer)
        Me.Controls.Add(Me.mnuStripMain)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.mnuStripMain
        Me.Name = "FormTreeEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Редактор заметок"
        Me.mnuStripMain.ResumeLayout(False)
        Me.mnuStripMain.PerformLayout()
        Me.spContainer.Panel1.ResumeLayout(False)
        Me.spContainer.Panel2.ResumeLayout(False)
        Me.spContainer.Panel2.PerformLayout()
        CType(Me.spContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.spContainer.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tvFiles As TreeView
    Friend WithEvents mnuStripMain As MenuStrip
    Friend WithEvents mnuFile As ToolStripMenuItem
    Friend WithEvents mnuFileExit As ToolStripMenuItem
    Friend WithEvents txtNote As TextBox
    Friend WithEvents spContainer As SplitContainer
    Friend WithEvents mnuFileNew As ToolStripMenuItem
    Friend WithEvents mnuFileSave As ToolStripMenuItem
    Friend WithEvents mnuFileRename As ToolStripMenuItem
    Friend WithEvents mnuFileDelete As ToolStripMenuItem
    Friend WithEvents mnuLine0 As ToolStripSeparator
    Friend WithEvents mnuFolder As ToolStripMenuItem
    Friend WithEvents mnuFolderNew As ToolStripMenuItem
    Friend WithEvents mnuFolderDelete As ToolStripMenuItem
    Friend WithEvents mnuLine1 As ToolStripSeparator
    Friend WithEvents mnuFolderRefresh As ToolStripMenuItem
    Friend WithEvents btnExit As Button
    Friend WithEvents mnuFolderRename As ToolStripMenuItem
End Class
