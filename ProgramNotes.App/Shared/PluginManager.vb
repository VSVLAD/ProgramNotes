Option Explicit On
Option Strict On

Imports System.IO


Namespace ProgramNotes.API

    Public Class PluginManager

        ''' <summary>Список плагинов</summary>
        Private pluginsList As New List(Of IPlugin)

        ''' <summary>Список всех загруженных плагинов</summary>
        Public ReadOnly Property Plugins As List(Of IPlugin)
            Get
                Return pluginsList
            End Get
        End Property

        ''' <summary>Найти плагин из списка по имени</summary>
        Public ReadOnly Property FindByName(ByVal Name As String) As IPlugin
            Get
                For Each xPlugin In pluginsList
                    If xPlugin.Name = Name Then Return xPlugin
                Next
                Return Nothing
            End Get
        End Property

        ''' <summary>Загрузка всех плагинов из каталога приложения</summary>
        Public Function LoadPlugins() As IEnumerable(Of IPlugin)
            pluginsList.Clear()

            For Each file In Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                Dim plugin As IPlugin = IsPlugin(file)

                If plugin IsNot Nothing Then
                    pluginsList.Add(plugin)
                End If
            Next

            Return pluginsList
        End Function

        ''' <summary>Проверяет является ли переданный файл плагином, если да возвращает ссылку на него</summary>
        Public Function IsPlugin(FileContent() As Byte) As IPlugin
            Try
                Dim assembly As Reflection.Assembly = Reflection.Assembly.Load(FileContent)

                For Each type In assembly.GetTypes()
                    Dim intFace As Type = type.GetInterface("ProgramNotes.API.IPlugin")

                    If intFace IsNot Nothing Then
                        Dim plugin As IPlugin = CType(Activator.CreateInstance(type), IPlugin)
                        Return plugin
                    End If
                Next

            Catch e As Exception
            End Try

            Return Nothing
        End Function

        ''' <summary>Проверяет является ли переданный файл плагином, если да возвращает ссылку на него</summary>
        Public Function IsPlugin(FilePath As String) As IPlugin
            Try
                Dim bt() As Byte = IO.File.ReadAllBytes(FilePath)
                Return IsPlugin(bt)

            Catch e As Exception
            End Try

            Return Nothing
        End Function

    End Class

End Namespace