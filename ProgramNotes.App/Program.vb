Option Explicit On
Option Strict On

Namespace ProgramNotes.App

    ''' <summary>Точка входа в приложение</summary>
    Public Class Program

        Public Shared WithEvents MyContext As MyWorkerContext

        <STAThread>
        Public Shared Sub Main()
            MyContext = New MyWorkerContext
            MyContext.ReloadConfiguration()

            Application.EnableVisualStyles()
            Application.Run(MyContext)
        End Sub

    End Class

End Namespace