Option Explicit On
Option Strict On

Imports ProgramNotes.API

Namespace ProgramNotes.App

    Public Class PluginNote
        Implements IPlugin

        Private _noteClass As NoteClass

        Public Sub New(selectedNoteClass As NoteClass)
            Me._noteClass = selectedNoteClass
        End Sub

        Public ReadOnly Property Icon As Image Implements IPlugin.Icon
            Get
                Return Resources.img_file
            End Get
        End Property

        Public ReadOnly Property Name As String Implements IPlugin.Name
            Get
                Return Me._noteClass.ItemName
            End Get
        End Property

        Public ReadOnly Property Version As String Implements IPlugin.Version
            Get
                Return "1.0"
            End Get
        End Property

        Public Function DoAction(Context As IContext) As Object Implements IPlugin.DoAction
            Dim sNewNote As String = Me._noteClass.ItemValue.ToString()
            Dim sOldNote As String = Context.ClipboardPaster.GetDataText()

            'Обновляем буфер
            Context.ClipboardPaster.SetDataText(sNewNote)

            'Эмуляция Ctrl+V
            Context.AppHotKey.EmulateControlV()

            'Восстанавливаем буфер
            Context.ClipboardPaster.SetDataText(sOldNote)

            Return Context
        End Function

    End Class

End Namespace
