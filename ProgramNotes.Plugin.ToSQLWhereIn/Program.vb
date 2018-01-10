Imports ProgramNotes.API

Public Class Program
    Implements IPlugin

    Public ReadOnly Property Icon As System.Drawing.Image Implements IPlugin.Icon
        Get
            Return ProgramNotes.Resources.img_app
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IPlugin.Name
        Get
            Return "Преобразовать столбец в строку (SQL WHERE IN)"
        End Get
    End Property

    Public ReadOnly Property Version As String Implements IPlugin.Version
        Get
            Return "1.0"
        End Get
    End Property

    Public Function DoAction(Context As IContext) As Object Implements IPlugin.DoAction

        Dim sSrcText As String = Context.ClipboardPaster.GetDataText()
        Dim sDstText As String = String.Empty

        If sSrcText <> "" Then
            Dim sAllLines() As String = Strings.Split(sSrcText, vbCrLf).Where(Function(x) Not String.IsNullOrEmpty(x)).ToArray()
            Dim lngResult As Long

            If sAllLines.Length > 0 Then

                If Not Long.TryParse(sAllLines(0), lngResult) OrElse sAllLines(0) Like "7[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]" Then
                    For I As Integer = 0 To sAllLines.Length - 1
                        sAllLines(I) = String.Format("'{0}'", sAllLines(I))
                    Next
                End If

                sDstText = String.Join(",", sAllLines)
                Context.ClipboardPaster.SetDataText(sDstText)
            End If
        End If

        Return Context
    End Function
End Class
