Option Explicit On
Option Strict On

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Namespace ProgramNotes.Shared

    Public Class FileConfiguration

        <DllImport("kernel32")>
        Private Shared Function WritePrivateProfileString(section As String, key As String, val As String, filePath As String) As Integer
        End Function

        <DllImport("kernel32")>
        Private Shared Function GetPrivateProfileString(section As String, key As String, def As String, retVal() As Byte, size As Integer, filePath As String) As Integer
        End Function

        <DllImport("kernel32")>
        Private Shared Function GetPrivateProfileSection(section As String, retVal() As Byte, size As Integer, filePath As String) As Integer
        End Function

        Private Const NullChar As Char = CChar(vbNullChar)
        Private Const BufferSize As Integer = 65536
        Private strFileConfiguration As String


        'Конструктор принимает путь к файлу с конфигурацией
        Public Sub New(FilePath As String)
            strFileConfiguration = FilePath

            If Not Directory.Exists(Path.GetDirectoryName(strFileConfiguration)) Then
                strFileConfiguration = Path.Combine(Application.StartupPath, strFileConfiguration)
            End If

            If Not File.Exists(strFileConfiguration) Then
                Using File.Create(strFileConfiguration)
                End Using
            End If
        End Sub

        'Записать параметр в файл
        Public Sub SaveValue(ByVal Section As String, ByVal Key As String, ByVal Value As String)
            WritePrivateProfileString(Section, Key, Value, Me.strFileConfiguration)
        End Sub

        'Прочитать параметр из файла
        Public Function LoadValue(ByVal Section As String, ByVal Key As String, ByVal Optional DefaultValue As String = "") As String
            Dim Buffer(BufferSize) As Byte
            Dim retValue As Integer = GetPrivateProfileString(Section, Key, DefaultValue, Buffer, BufferSize, Me.strFileConfiguration)

            Return Encoding.GetEncoding(1251).GetString(Buffer).Trim(NullChar)
        End Function

        'Возвращает список всех параметров из секции
        Public Iterator Function LoadKeys(ByVal Section As String) As IEnumerable(Of String)
            Dim Buffer(BufferSize) As Byte

            GetPrivateProfileSection(Section, Buffer, BufferSize, Me.strFileConfiguration)

            For Each xItem In Encoding.GetEncoding(1251).GetString(Buffer).Trim(NullChar).Split(NullChar)
                If Not xItem.Trim().StartsWith("#") AndAlso Not xItem.Trim().StartsWith(";") Then
                    If xItem.IndexOf("=") > 0 Then
                        Yield xItem.Substring(0, xItem.IndexOf("="))
                    Else
                        Yield xItem
                    End If
                End If
            Next
        End Function

        'Возвращает список всех секций
        Public Iterator Function LoadSections() As IEnumerable(Of String)
            Dim Buffer(BufferSize) As Byte
            Dim retValue As Integer = GetPrivateProfileString(Nothing, Nothing, String.Empty, Buffer, BufferSize, Me.strFileConfiguration)

            For Each xItem In Encoding.GetEncoding(1251).GetString(Buffer).Trim(NullChar).Split(NullChar)
                Yield xItem
            Next
        End Function

    End Class

End Namespace