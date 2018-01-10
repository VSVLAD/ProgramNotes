Option Explicit On
Option Strict On

Namespace ProgramNotes.API

    Public Enum NoteType
        FolderItem
        TextItem
    End Enum

    Public Class NoteClass

        ''' <summary>Название заметки отображаемое в меню</summary>
        Public Property ItemName As String

        ''' <summary>Текст заметки который будет вставлен в редактор</summary>
        Public Property ItemValue As String

        ''' <summary>Тип заметки: папка или файл/плагин</summary>
        Public Property ItemType As NoteType

        ''' <summary>Интерфейс на запускаемый плагин</summary>
        Public Property ItemPlugin As IPlugin

        ''' <summary>Полный путь к файлу с заметкой</summary>
        Public Property FilePathAbsolute As String

        ''' <summary>Относительный путь от текущей папке к файлу с заметкой</summary>
        Public Property FilePathLocal As String

        ''' <summary>Относительный путь от текущей папке к папке, где хранится файл с заметкой</summary>
        Public Property FolderPathLocal As String
    End Class

End Namespace
