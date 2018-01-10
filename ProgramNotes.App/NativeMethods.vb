Public Class NativeMethods

    Public Declare Auto Function SendMessage Lib "user32" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Public Declare Auto Function PostMessage Lib "user32" (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Public Declare Auto Function SetForegroundWindow Lib "user32" (ByVal hWnd As IntPtr) As Integer

End Class
