Imports System.Drawing
Imports ProgramNotes.API

Public Class Program
    Implements IPlugin

    Public ReadOnly Property Icon As Image Implements IPlugin.Icon
        Get
            Return ProgramNotes.Resources.img_app
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IPlugin.Name
        Get
            Return "Преобразовать SQL запрос SPA в синонимы XA и XN"
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
            sDstText = sSrcText.ToLower().Replace("cbpa.", "xa_").Replace("cbp_a.", "xa_").Replace("cbp_c.", "xn_").Replace("spa_cbp_09.", "xn_").Replace("cbpn.", "xn_").Replace("cbp1.", "xn_").Replace("cbp_p2.", "xn_")

            Context.ClipboardPaster.SetDataText(sDstText)
        End If

        Return Context
    End Function

End Class
