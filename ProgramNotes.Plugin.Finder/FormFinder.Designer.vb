<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormFinder
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
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"", "", "", ""}, -1)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormFinder))
        Me.lvItems = New System.Windows.Forms.ListView()
        Me.Заметка = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Расположение = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.txtSearchBox = New System.Windows.Forms.TextBox()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.TimerAnimation = New System.Windows.Forms.Timer(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        Me.pboxSearch = New System.Windows.Forms.PictureBox()
        CType(Me.pboxSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lvItems
        '
        Me.lvItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Заметка, Me.Расположение})
        Me.lvItems.FullRowSelect = True
        Me.lvItems.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1})
        Me.lvItems.Location = New System.Drawing.Point(8, 64)
        Me.lvItems.MultiSelect = False
        Me.lvItems.Name = "lvItems"
        Me.lvItems.ShowItemToolTips = True
        Me.lvItems.Size = New System.Drawing.Size(712, 363)
        Me.lvItems.TabIndex = 2
        Me.lvItems.UseCompatibleStateImageBehavior = False
        Me.lvItems.View = System.Windows.Forms.View.Details
        '
        'Заметка
        '
        Me.Заметка.Text = "Заметка"
        Me.Заметка.Width = 400
        '
        'Расположение
        '
        Me.Расположение.Text = "Расположение"
        Me.Расположение.Width = 300
        '
        'txtSearchBox
        '
        Me.txtSearchBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearchBox.Location = New System.Drawing.Point(9, 34)
        Me.txtSearchBox.Name = "txtSearchBox"
        Me.txtSearchBox.Size = New System.Drawing.Size(712, 22)
        Me.txtSearchBox.TabIndex = 1
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(32, 10)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(218, 14)
        Me.lblInfo.TabIndex = 3
        Me.lblInfo.Text = "Введите название или текст заметки"
        '
        'TimerAnimation
        '
        Me.TimerAnimation.Interval = 200
        '
        'btnExit
        '
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Location = New System.Drawing.Point(16, 40)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(8, 8)
        Me.btnExit.TabIndex = 4
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'pboxSearch
        '
        Me.pboxSearch.Image = Global.ProgramNotes.Resources.img_search
        Me.pboxSearch.Location = New System.Drawing.Point(9, 9)
        Me.pboxSearch.Name = "pboxSearch"
        Me.pboxSearch.Size = New System.Drawing.Size(16, 16)
        Me.pboxSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pboxSearch.TabIndex = 2
        Me.pboxSearch.TabStop = False
        '
        'FormFinder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(727, 433)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.pboxSearch)
        Me.Controls.Add(Me.txtSearchBox)
        Me.Controls.Add(Me.lvItems)
        Me.Controls.Add(Me.btnExit)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormFinder"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Поиск заметок"
        CType(Me.pboxSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lvItems As ListView
    Friend WithEvents txtSearchBox As TextBox
    Friend WithEvents pboxSearch As PictureBox
    Friend WithEvents lblInfo As Label
    Friend WithEvents Заметка As ColumnHeader
    Friend WithEvents Расположение As ColumnHeader
    Friend WithEvents TimerAnimation As Timer
    Friend WithEvents btnExit As Button
End Class
