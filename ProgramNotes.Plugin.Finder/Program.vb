Option Explicit On
Option Strict On

Imports ProgramNotes.API


Public Class Program
    Implements IPlugin

    Public ReadOnly Property Icon As Image Implements IPlugin.Icon
        Get
            Return ProgramNotes.Resources.img_search
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IPlugin.Name
        Get
            Return "Найти заметки"
        End Get
    End Property

    Public ReadOnly Property Version As String Implements IPlugin.Version
        Get
            Return "1.0"
        End Get
    End Property


    Public Function DoAction(Context As IContext) As Object Implements IPlugin.DoAction
        Dim frmFinder As New FormFinder

        frmFinder.InitForm(Context)
        frmFinder.ShowDialog()

        Return Context
    End Function

End Class
