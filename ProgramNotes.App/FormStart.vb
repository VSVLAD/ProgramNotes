Option Strict On
Option Explicit On

Imports System.Text
Imports ProgramNotes.API
Imports ProgramNotes.App
Imports ProgramNotes.Shared


Public Class FormStart

    Private WithEvents appContext As IContext = Program.MyContext

    'Скрываем временно форму, пока нет прогресс бара!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Отображается автоматически, т.к. это MainForm у AppContext
    Private Sub FormStart_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Visible = False
        Me.Hide()
    End Sub

    'Начальная загрузка
    Private Sub FormOptions_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Подписываемся на события объектов
        AddHandler appContext.ClipboardChange.ClipboardChanged, AddressOf ClipboardChange_Event
        AddHandler appContext.AppHotKey.HotKeyPressed, AddressOf FormViewHotKey_Event

        'Загружаем меню впервые
        appContext.RefreshMenu(cxMenu)

        'Отображаем пункт выбранной кодировки
        Select Case appContext.AppFileConfig.LoadValue("Options", "Encoding", "UTF-8")
            Case "Windows-1251"
                mnuEncodingW1251.Checked = True
            Case "UTF-8"
                mnuEncodingUTF8.Checked = True
            Case "Unicode"
                mnuEncodingUnicode.Checked = True
            Case Else
                MessageBox.Show("В настройках выбрана неизвестная кодировка! Принудительно выбрана UTF-8", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                appContext.NotesEncoding = Encoding.UTF8

                mnuEncodingUTF8.Checked = True
        End Select

        MessageBox.Show($"К работе готов! Нажимайте горячую клавишу F4 или свою, указанную в настройках", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    'Перенаправляем события для обработки
    Protected Overrides Sub WndProc(ByRef m As Message)
        If appContext.AppHotKey IsNot Nothing Then appContext.AppHotKey.ProcessMessage(m)
        If appContext.ClipboardChange IsNot Nothing Then appContext.ClipboardChange.ProcessMessage(m)

        MyBase.WndProc(m)
    End Sub

    'Событие вызывается каждый раз при нажатии горячей клавиши
    Private Sub FormViewHotKey_Event(Key As Keys, Modifer As HotKeyModifer)
        NativeMethods.SetForegroundWindow(cxMenu.Handle)   'Устанавливаем окно на передний план
        cxMenu.Show(MousePosition.X + 6, MousePosition.Y + 6)
        'PostMessage(cxMenu.Handle, 0, 0, 0)  'Посылаем сообщение окну чтобы скрыть Context Menu, если клик мимо него
    End Sub

    'Событие вызывается каждый раз при обновлении буфер обмена
    Private Sub ClipboardChange_Event()
    End Sub

    'Меню -> Выход
    Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click
        cxMenu.Close()
        If MessageBox.Show("Вы действительно хотите завершить работу приложения?", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then Application.Exit()
    End Sub

    'Меню -> Настройки -> Перечитать заново всё меню
    Private Sub mnuOptionsRefresh_Click(sender As Object, e As EventArgs) Handles mnuOptionsRefresh.Click
        cxMenu.Close()

        appContext.RefreshMenu(cxMenu)

        MessageBox.Show("Все файлы и папки с обновленными заметками были загружены!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    'Меню -> Настройки -> Открыть папку
    Private Sub mnuOptionsBrowseFolder_Click(sender As Object, e As EventArgs) Handles mnuOptionsBrowseFolder.Click
        Try
            cxMenu.Close()
            Process.Start("explorer.exe", $"""{appContext.FolderNotesPath}""")
        Catch
        End Try
    End Sub

    'Меню -> Настройки -> Кодировка
    Private Sub mnuEncodingAll_Click(sender As Object, e As EventArgs) Handles mnuEncodingW1251.Click,
                                                                               mnuEncodingUTF8.Click,
                                                                               mnuEncodingUnicode.Click
        mnuEncodingW1251.Checked = False
        mnuEncodingUTF8.Checked = False
        mnuEncodingUnicode.Checked = False

        Dim selectedMenuStrip As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        selectedMenuStrip.Checked = True

        Select Case selectedMenuStrip.Text
            Case "Windows-1251"
                appContext.NotesEncoding = Encoding.GetEncoding(1251)
            Case "UTF-8"
                appContext.NotesEncoding = Encoding.UTF8
            Case "Unicode"
                appContext.NotesEncoding = Encoding.Unicode
            Case Else
                MessageBox.Show("Выбрана неизвестная кодировка! Принудительно выбрана UTF-8", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                appContext.NotesEncoding = Encoding.UTF8
        End Select

        'Сохраняем кодировку и перезагружаем принудительно менюшки
        appContext.AppFileConfig.SaveValue("Options", "Encoding", selectedMenuStrip.Text)
        appContext.RefreshMenu(cxMenu)
    End Sub

End Class
