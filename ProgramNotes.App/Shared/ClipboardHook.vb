Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices

Namespace ProgramNotes.Shared

    Public Class ClipboardHook

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Private Shared Function SetClipboardViewer(hWndNewViewer As IntPtr) As Integer
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Private Shared Function ChangeClipboardChain(hWndRemove As IntPtr, hWndNewNext As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Private Shared Function SendMessage(hwnd As IntPtr, wMsg As UInteger, wParam As IntPtr, lParam As IntPtr) As Integer
        End Function

        Private Const WM_DRAWCLIPBOARD = &H308
        Private Const WM_CHANGECBCHAIN = &H30D

        Public Event ClipboardChanged()

        Private nextViewer As IntPtr
        Private thisViewer As IntPtr
        Private flagFireEvent As Boolean

        Public Sub New(FormObject As Form)
            thisViewer = FormObject.Handle
            nextViewer = CType(SetClipboardViewer(thisViewer), IntPtr)
        End Sub

        Protected Overrides Sub Finalize()
            ChangeClipboardChain(thisViewer, nextViewer)
            MyBase.Finalize()
        End Sub

        'Вызывать для обработки сообщения классом
        Public Sub ProcessMessage(ByRef m As Message)
            Select Case m.Msg
                Case WM_DRAWCLIPBOARD
                    If Not flagFireEvent Then
                        flagFireEvent = True

                        RaiseEvent ClipboardChanged()
                        flagFireEvent = False

                        SendMessage(nextViewer, CUInt(m.Msg), m.WParam, m.LParam)
                    End If

                Case WM_CHANGECBCHAIN
                    If m.WParam = nextViewer Then
                        nextViewer = m.LParam
                    Else
                        SendMessage(nextViewer, CUInt(m.Msg), m.WParam, m.LParam)
                    End If

            End Select
        End Sub
    End Class

End Namespace