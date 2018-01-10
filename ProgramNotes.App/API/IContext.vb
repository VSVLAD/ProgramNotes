Option Explicit On
Option Strict On

Imports System.Text
Imports ProgramNotes.Shared

Namespace ProgramNotes.API

    Public Interface IContext

        ''' <summary>Событие срабатывает при изменении кодировки пользователем</summary>
        Event EncodingChanged(NewEncoding As Encoding)

        ''' <summary>Объект для доступа к горячей клавиши</summary>
        Property AppHotKey As HotkeyHook

        ''' <summary>Объект для доступа к буферу обмена</summary>
        Property ClipboardPaster As ClipboardPaster

        ''' <summary>Объект для доступа к буферу обмена</summary>
        Property ClipboardChange As ClipboardHook

        ''' <summary>Объект доступа к файлу конфигурации</summary>
        Property AppFileConfig As FileConfiguration

        ''' <summary>Список ссылок на все заметки</summary>
        Property Notes As List(Of NoteClass)

        ''' <summary>Каталог для заметок</summary>
        Property FolderNotesPath As String

        ''' <summary>Кодировка в которой будут обработаны заметки</summary>
        Property NotesEncoding As Encoding

        ''' <summary>Метод перечитывает все заметки из папки и отрисовывает заново меню</summary>
        Sub RefreshMenu(ByVal MenuControl As ContextMenuStrip)


    End Interface

End Namespace