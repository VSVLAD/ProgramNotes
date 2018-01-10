Imports System.Text
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
            Return "Преобразовать набора значений в таблицу (SQL UNION ALL)"
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

        Dim strRowsCols As New List(Of String())
        Dim sbResult As New StringBuilder
        Dim intMapTypes() As Integer, lngResult As Long

        If sSrcText <> "" Then

            'Делим строки на массив строк, и каждую строку на массив столбцов
            For Each xRow In Strings.Split(sSrcText, vbCrLf)
                If Not String.IsNullOrEmpty(xRow) Then strRowsCols.Add(xRow.Split(vbTab))
            Next

            If strRowsCols.Count > 0 Then

                'Определяемся с типами в шапке, кто число, кто строка
                Dim FirstArr = strRowsCols.First
                Dim FirstBound = FirstArr.GetUpperBound(0)
                ReDim intMapTypes(FirstBound)

                For I As Long = 0 To FirstBound
                    If FirstArr(I) Like "7[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]" Then
                        intMapTypes(I) = 0
                    ElseIf Long.TryParse(FirstArr(I), lngResult) Then
                        intMapTypes(I) = 1
                    Else
                        intMapTypes(I) = 0
                    End If
                Next

                'Добавляем сами данные с учётом типа
                Dim bHeader As Boolean = True

                For Each xCol In strRowsCols
                    If sbResult.Length > 0 Then sbResult.AppendLine(" union all")
                    sbResult.Append("select ")

                    For I = 0 To FirstBound
                        Select Case intMapTypes(I)
                            Case 0
                                sbResult.AppendFormat("'{0}'", xCol(I))
                            Case 1
                                sbResult.Append(xCol(I))
                        End Select

                        If bHeader Then sbResult.AppendFormat(" as f{0}", I + 1)
                        If I < FirstBound Then sbResult.Append(", ")
                    Next

                    sbResult.Append(" from dual")
                    bHeader = False
                Next

                sDstText = sbResult.ToString()
                Context.ClipboardPaster.SetDataText(sDstText)
            End If
        End If

        Return Context
    End Function
End Class
