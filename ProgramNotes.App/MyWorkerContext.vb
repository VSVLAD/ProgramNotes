Option Explicit On
Option Strict On

Imports System.IO
Imports Opulos.Core.IO

Imports ProgramNotes.Shared
Imports ProgramNotes.API
Imports System.Text

Namespace ProgramNotes.App

    'Рабочий класс приложения
    Public Class MyWorkerContext
        Inherits ApplicationContext
        Implements IContext

        Private PluginManager As PluginManager

        Private WithEvents notifyIcon As NotifyIcon
        Private components As ComponentModel.Container

        Private WithEvents _clipboardPaster As ClipboardPaster  ' Объект для доступа к буферу обмена
        Private WithEvents _clipboardChange As ClipboardHook    ' Объект для доступа к буферу обмена
        Private _appHotKey As HotkeyHook                        ' Горячая клавиша для основного действия
        Private _notes As List(Of NoteClass)                    ' Список ссылок на все заметки
        Private _folderNotesPath As String                      ' Каталог для поиска заметок
        Private _notesEncoding As Encoding                      ' Кодировка для заметок
        Private _fileConfig As FileConfiguration                ' Конфигурация приложения

#Region "Реализация интерфейса IContext"

        Public Event EncodingChanged(NewEncoding As Encoding) Implements IContext.EncodingChanged

        Public Property AppHotKey As HotkeyHook Implements IContext.AppHotKey
            Get
                Return _appHotKey
            End Get
            Set(ByVal value As HotkeyHook)
                _appHotKey = value
            End Set
        End Property

        Public Property ClipboardPaster As ClipboardPaster Implements IContext.ClipboardPaster
            Get
                Return _clipboardPaster
            End Get
            Set(ByVal value As ClipboardPaster)
                _clipboardPaster = value
            End Set
        End Property

        Public Property ClipboardChange As ClipboardHook Implements IContext.ClipboardChange
            Get
                Return _clipboardChange
            End Get
            Set(ByVal value As ClipboardHook)
                _clipboardChange = value
            End Set
        End Property

        Public Property Notes As List(Of NoteClass) Implements IContext.Notes
            Get
                Return _notes
            End Get
            Set(ByVal value As List(Of NoteClass))
                _notes = value
            End Set
        End Property

        Public Property FolderNotesPath As String Implements IContext.FolderNotesPath
            Get
                Return _folderNotesPath
            End Get
            Set(ByVal value As String)
                _folderNotesPath = value
            End Set
        End Property

        Public Property NotesEncoding As Encoding Implements IContext.NotesEncoding
            Get
                Return _notesEncoding
            End Get
            Set(value As Encoding)
                _notesEncoding = value
                RaiseEvent EncodingChanged(_notesEncoding)
            End Set
        End Property


        Public Property AppFileConfig As FileConfiguration Implements IContext.AppFileConfig
            Get
                Return _fileConfig
            End Get
            Set(value As FileConfiguration)
                _fileConfig = value
            End Set
        End Property

        'Перечитываем пункты меню
        Public Sub RefreshMenuItems(ByVal MenuControl As ContextMenuStrip) Implements IContext.RefreshMenu

            'Очищаем меню от всего лишнего
            For I = MenuControl.Items.Count - 1 To 0 Step -1
                If TypeOf TryCast(MenuControl.Items(I).Tag, NoteClass) Is NoteClass Then
                    MenuControl.Items.Remove(MenuControl.Items(I))
                End If
            Next

            'Загружаем заметки заново
            Program.MyContext.PluginManager.Plugins.Clear()
            Program.MyContext.Notes.Clear()
            Program.MyContext.Notes = EnumerateNotesFiles(Program.MyContext.FolderNotesPath)

            GC.Collect(1, GCCollectionMode.Forced)
            GC.WaitForPendingFinalizers()

            'Сначала добавляем только меню-папки (для лучшей сортировки)
            For Each xNote In Program.MyContext.Notes.Where(Function(x) Not String.IsNullOrEmpty(x.FolderPathLocal)).
                                                        Select(Function(x) x.FolderPathLocal).
                                                        Distinct().
                                                        Select(Function(path) New NoteClass With {.ItemType = NoteType.FolderItem,
                                                                                                  .FolderPathLocal = path})
                AddMenuItem(MenuControl, xNote)
            Next

            'Добавляем и сами меню-файлы
            For Each xNote In Program.MyContext.Notes
                AddMenuItem(MenuControl, xNote)
            Next
        End Sub

#End Region


        'Загрузка конфигурации
        Public Sub ReloadConfiguration()

            'Настройка форм и компонент
            Me.MainForm = New FormStart()
            Me.components = New ComponentModel.Container()
            Me.notifyIcon = New NotifyIcon(components) With {.Text = Application.ProductName, .Icon = Resources.AppIcon, .Visible = True}
            Me.notifyIcon.ContextMenu = New ContextMenu({New MenuItem("Выход", Sub(s, e) Application.Exit())})

            'Читаем конфигурацию
            Dim strConfigPath = Application.ExecutablePath.ToLower().Replace(".exe", ".ini")
            Me.AppFileConfig = New FileConfiguration(If(File.Exists(strConfigPath), strConfigPath, Path.GetTempFileName))

            'Читаем горячие клавиши из настроек
            Dim HK As Keys = CType([Enum].Parse(GetType(Keys), AppFileConfig.LoadValue("Options", "HotKey", "F4")), Keys)
            Dim HM1 As HotKeyModifer = CType([Enum].Parse(GetType(HotKeyModifer), AppFileConfig.LoadValue("Options", "HotModificatorOne", "NO_MODIFICATION")), HotKeyModifer)
            Dim HM2 As HotKeyModifer = CType([Enum].Parse(GetType(HotKeyModifer), AppFileConfig.LoadValue("Options", "HotModificatorTwo", "NO_MODIFICATION")), HotKeyModifer)

            'Кодировка
            Me.NotesEncoding = Encoding.GetEncoding(AppFileConfig.LoadValue("Options", "Encoding", "UTF-8"))

            Me.FolderNotesPath = Application.StartupPath
            Me.Notes = New List(Of NoteClass)

            Me.ClipboardChange = New ClipboardHook(Me.MainForm)
            Me.ClipboardPaster = New ClipboardPaster()

            'Регистрируем хоткей и буферные классы
            Me.AppHotKey = New HotkeyHook(Me.MainForm)
            Me.AppHotKey.Unregister()
            Me.AppHotKey.Register(HK, HM1 Or HM2)

            'Создаём менеджер плагинов
            PluginManager = New PluginManager()
        End Sub


        'Перечисляет все файлы в каталоге заметок и возвращает массив объектов NoteClass
        Private Function EnumerateNotesFiles(ByVal StartupPath As String) As List(Of NoteClass)
            Dim resultList As New List(Of NoteClass)

            For Each xPath In FastFileInfo.EnumerateFiles(StartupPath, "*", SearchOption.AllDirectories).
                                           Where(Function(fi) Not fi.DirectoryName Like "*\~*")
                Try

                    If xPath.Name.ToLower Like "*.txt" OrElse
                       xPath.Name.ToLower Like "*.sql" OrElse
                       xPath.Name.ToLower Like "*.xml" OrElse
                       xPath.Name.ToLower Like "*.vb" OrElse
                       xPath.Name.ToLower Like "*.cs" Then

                        Dim currNote As New NoteClass With {
                                                .FilePathAbsolute = xPath.FullName,
                                                .FilePathLocal = xPath.FullName.Replace(StartupPath, String.Empty),
                                                .FolderPathLocal = xPath.DirectoryName.Replace(StartupPath, String.Empty),
                                                .ItemName = xPath.Name,
                                                .ItemType = NoteType.TextItem
                                            }

                        currNote.ItemPlugin = New PluginNote(currNote)
                        currNote.ItemValue = File.ReadAllText(xPath.FullName, Me.NotesEncoding)

                        resultList.Add(currNote)

                    ElseIf xPath.Name.ToLower Like "*.dll" Then

                        'Если файл DLL это плагин, тогда загружаем ссылку на него
                        Dim pluginClass As IPlugin = PluginManager.IsPlugin(xPath.FullName)

                        If pluginClass IsNot Nothing Then

                            Dim currNote As New NoteClass With {
                                                .FilePathAbsolute = xPath.FullName,
                                                .FilePathLocal = xPath.FullName.Replace(StartupPath, String.Empty),
                                                .FolderPathLocal = xPath.DirectoryName.Replace(StartupPath, String.Empty),
                                                .ItemName = xPath.Name,
                                                .ItemType = NoteType.TextItem
                                            }

                            currNote.ItemPlugin = pluginClass
                            currNote.ItemValue = String.Empty

                            resultList.Add(currNote)
                        End If
                    End If

                Catch ex As Exception
                    MessageBox.Show($"Program.EnumerateNotesFiles() : Невозможно загрузить текст заметки ""{xPath.FullName}""" & vbCrLf & vbCrLf &
                                    ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next

            Return resultList
        End Function


        'Добавление пунктов в менюшку на основе объекта заметки
        Private Sub AddMenuItem(ByVal MenuControl As ContextMenuStrip, ByVal NoteObject As NoteClass)
            Try
                Dim GroupCurrent As ToolStripMenuItem = Nothing
                Dim ItemCurrent As ToolStripMenuItem = Nothing

                'Преобразуем локальный путь в массив групп
                Dim GroupNames = NoteObject.FolderPathLocal.Split(New String() {"\"}, StringSplitOptions.RemoveEmptyEntries)

                'По всем группам
                For I = 0 To GroupNames.GetUpperBound(0)
                    Dim bFound As Boolean = False

                    'Поиск существующей группы (0 - первый уровень (главное меню), 1, 2, ... - выдвигаемые)
                    If I = 0 Then

                        For Each xGroup As ToolStripItem In MenuControl.Items
                            If xGroup.Text = GroupNames(I) Then
                                GroupCurrent = CType(xGroup, ToolStripMenuItem)
                                bFound = True
                                Exit For
                            End If
                        Next

                        If Not bFound Then 'Группа не найдена, добавить новую
                            GroupCurrent = New ToolStripMenuItem(GroupNames(I), If(GroupNames(I).EndsWith("#"), Resources.img_project, Resources.img_folder))
                            GroupCurrent.Tag = NoteObject

                            MenuControl.Items.Insert(MenuControl.Items.IndexOf(MenuControl.Items("mnuEndLine")), GroupCurrent)
                        End If

                    Else
                        For Each xGroup As ToolStripItem In GroupCurrent.DropDownItems
                            If xGroup.Text = GroupNames(I) Then
                                GroupCurrent = CType(xGroup, ToolStripMenuItem)
                                bFound = True
                                Exit For
                            End If
                        Next

                        If Not bFound Then 'Группа не найдена, добавить новую
                            Dim GroupLast As ToolStripMenuItem = GroupCurrent
                            GroupCurrent = New ToolStripMenuItem(GroupNames(I), If(GroupNames(I).EndsWith("#"), Resources.img_project, Resources.img_folder))
                            GroupCurrent.Tag = NoteObject

                            GroupLast.DropDownItems.Add(GroupCurrent)
                        End If

                    End If
                Next

                'Если задан Item (т.е. это не папка), то добавляем
                If NoteObject.ItemType = NoteType.TextItem Then

                    'Создаём
                    ItemCurrent = New ToolStripMenuItem(NoteObject.ItemPlugin.Name, NoteObject.ItemPlugin.Icon)
                    ItemCurrent.Tag = NoteObject

                    'Добавляем итем в главное меню или во вложенную папку, смотря какой путь к файлу/плагину
                    If GroupNames.GetUpperBound(0) < 0 Then
                        MenuControl.Items.Insert(MenuControl.Items.IndexOf(MenuControl.Items("mnuStartLine")), ItemCurrent)
                    Else
                        GroupCurrent.DropDownItems.Add(ItemCurrent)
                    End If

                    'Подписываемся на событие
                    AddHandler ItemCurrent.Click,
                        Sub(sender As Object, e As EventArgs)
                            MenuControl.Close()

                            Dim clickedItem As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
                            Dim clickedNoteClass As NoteClass = TryCast(clickedItem.Tag, NoteClass)

                            If clickedNoteClass IsNot Nothing AndAlso clickedNoteClass.ItemPlugin IsNot Nothing Then
                                'Вызываем плагин и передаём контекст
                                Try
                                    clickedNoteClass.ItemPlugin.DoAction(Program.MyContext)
                                Catch ex As Exception
                                    MessageBox.Show($"IPlugin.DoAction(): плагин ""{NoteObject.ItemName}"" создал исключение!" & vbCrLf & vbCrLf &
                                                    ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End Try
                            End If
                        End Sub

                End If

            Catch ex As Exception
                MessageBox.Show($"Program.AddMenuObject() : Ошибки при обработке заметки ""{NoteObject.FilePathAbsolute}""" & vbCrLf & vbCrLf &
                            ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End Sub


        'Выход из приложения
        Private Sub MyApplicationContext_ThreadExit(sender As Object, e As EventArgs) Handles Me.ThreadExit
            notifyIcon.Visible = False
        End Sub

    End Class


End Namespace