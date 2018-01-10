Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices

Namespace ProgramNotes.Shared

    Public Class ClipboardPaster

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
        Private Shared Function OpenClipboard(ByVal hWndNewOwner As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
        Private Shared Function CloseClipboard() As Boolean
        End Function

        Private WithEvents setDataTimer As New Timer() With {.Interval = DELAY_BETWEEN_TRIES, .Enabled = False}

        Private Const MAX_TRIES As Integer = 10
        Private Const DELAY_BETWEEN_TRIES As Integer = 250

        Private workOperation As Boolean
        Private intDataTries As Integer
        Private dataText As String


        'Прочитать из буфера
        Public Function GetDataText() As String
            If workOperation Then Return String.Empty

            workOperation = True
            dataText = String.Empty
            intDataTries = 0

            Do While intDataTries <= MAX_TRIES
                Try
                    If Clipboard.ContainsText Then dataText = Clipboard.GetText()
                    Exit Do
                Catch ex As ExternalException
                    intDataTries += 1
                    Threading.Thread.Sleep(DELAY_BETWEEN_TRIES)
                End Try
            Loop

            workOperation = False

            Return dataText
        End Function

        'Добавить в буфер обмена
        Public Sub SetDataText(clipboardData As String)
            If workOperation Then Return

            workOperation = True
            intDataTries = 0
            dataText = clipboardData

            internalSetDataText()
        End Sub


        Private Sub internalSetDataText()
            Try
                If Not String.IsNullOrEmpty(dataText) Then
                    Clipboard.Clear()
                    Clipboard.SetText(dataText, TextDataFormat.UnicodeText)

                End If

                setDataTimer.Enabled = False
                workOperation = False
                dataText = String.Empty

            Catch ex As ExternalException
                If intDataTries <= MAX_TRIES Then 'Если не смогли добавить, попробовать повторно
                    intDataTries += 1
                    setDataTimer.Enabled = True
                Else
                    setDataTimer.Enabled = False 'Если все попытки вышли, выходим нах =(
                    workOperation = False
                End If
            End Try

        End Sub

        'Попробовать повторить
        Private Sub OnSetDataTimerElapsed(sender As Object, elapsedEventArgs As EventArgs) Handles setDataTimer.Tick
            internalSetDataText()
        End Sub

        'Функция конвертации текста из CP1252 в CP1251
        Public Function ToWindows1251(ByVal TextWindows1252 As String) As String
            Dim w1252 As String = "€ƒˆŠŒŽ˜šœžŸ¡¢£¥¨ª¯²³´¸¹º¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ"
            Dim w1251 As String = "ЂЃѓ€ЉЊЌЋЏђљњќћџЎўЈҐЁЄЇІіґё№єјЅѕїАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюя"

            For I = 0 To w1251.Length - 1
                If TextWindows1252.IndexOf(w1252.Substring(I, 1)) >= 0 Then
                    TextWindows1252 = TextWindows1252.Replace(w1252.Substring(I, 1), w1251.Substring(I, 1))
                End If
            Next

            Return TextWindows1252
        End Function

    End Class

End Namespace