Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices

Namespace ProgramNotes.Shared

    <Flags>
    Public Enum HotKeyModifer As UInteger
        NO_MODIFICATION = 0
        ALT = 1
        CONTROL = 2
        SHIFT = 4
        WIN = 8
    End Enum

    Public Class HotkeyHook

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Private Shared Function RegisterHotKey(hWnd As IntPtr, id As Integer, fsModifiers As Integer, vk As Integer) As Boolean
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Private Shared Function UnregisterHotKey(hWnd As IntPtr, id As Integer) As Boolean
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Private Shared Sub keybd_event(bVk As Byte, bScan As Byte, dwFlags As UInteger, dwExtraInfo As Integer)
        End Sub

        Public Event HotKeyPressed(Key As Keys, Modifer As HotKeyModifer)

        Private Const KEYEVENTF_KEYUP = &H2
        Private Const WM_HOTKEY = &H312

        Private m_Modifer As Integer
        Private m_Key As Integer
        Private m_Id As Integer

        Private thisHandle As IntPtr


        Public Sub New(FormObject As Form)
            thisHandle = FormObject.Handle
        End Sub

        'Обработка сообщений
        Public Sub ProcessMessage(ByRef m As Message)
            If m.Msg = WM_HOTKEY Then
                Dim idHotKey As Integer = CInt(m.WParam) 'Получаем идентификатор комбинации
                RaiseEvent HotKeyPressed(CType(m_Key, Keys), CType(m_Modifer, HotKeyModifer))
            End If
        End Sub

        Protected Overrides Sub Finalize()
            If thisHandle.ToInt32 > 0 Then UnregisterHotKey(thisHandle, Me.GetType().GetHashCode())
            MyBase.Finalize()
        End Sub

        'Переопределяем, получаем уникальный ID
        Public Overrides Function GetHashCode() As Integer
            Return m_Modifer + m_Key + thisHandle.ToInt32()
        End Function

        'Регистрация клавиши
        Public Function Register(Key As Keys, Modifer As HotKeyModifer) As Boolean
            m_Modifer = CInt(Modifer)
            m_Key = Key
            m_Id = Me.GetHashCode()

            Return RegisterHotKey(thisHandle, m_Id, m_Modifer, m_Key)
        End Function

        'Снять регистрацию клавиш
        Public Function Unregister() As Boolean
            Return UnregisterHotKey(thisHandle, m_Id)
        End Function

        'Для эмуляции нажатия Ctrl + V
        Public Sub EmulateControlV()
            keybd_event(CByte(Keys.ControlKey), 0, 0, 0)
            keybd_event(CByte(Keys.V), 0, 0, 0)
            Threading.Thread.Sleep(100)

            keybd_event(CByte(Keys.V), 0, KEYEVENTF_KEYUP, 0)
            keybd_event(CByte(Keys.ControlKey), 0, KEYEVENTF_KEYUP, 0)
            Threading.Thread.Sleep(100)
        End Sub

    End Class

End Namespace