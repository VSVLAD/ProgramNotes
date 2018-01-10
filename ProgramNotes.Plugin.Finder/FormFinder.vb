Option Explicit On
Option Strict On
Imports System.Text
Imports ProgramNotes.API

Public Class FormFinder

    Private WithEvents appContext As IContext

    Private flagAnimRotator As Boolean
    Private countAnimRotator As Integer


    Public Sub InitForm(Context As IContext)
        Me.appContext = Context
    End Sub

    'При загрузке формы
    Private Sub FormFinder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim imagesListView As New ImageList

        imagesListView.Images.Add("File", ProgramNotes.Resources.img_file)
        lvItems.SmallImageList = imagesListView
    End Sub

    'При изменении текста ищем
    Private Sub txtSearchBox_TextChanged(sender As Object, e As EventArgs) Handles txtSearchBox.TextChanged

        'Если пусто - не ищем ничего
        If String.IsNullOrEmpty(txtSearchBox.Text) Then
            lvItems.Items.Clear()
            Return
        End If

        'Ищем совпадения
        lvItems.Items.Clear()

        For Each xNote In appContext.Notes.Where(Function(nc) nc.ItemType = NoteType.TextItem AndAlso
                                                             (nc.ItemName.ToLower().IndexOf(txtSearchBox.Text.ToLower()) >= 0 Or
                                                              nc.ItemValue.ToString().ToLower().IndexOf(txtSearchBox.Text.ToLower()) >= 0))
            'Добавляем в листвью
            Dim vElement As New ListViewItem(xNote.ItemName, "File")

            vElement.SubItems.Add(xNote.FilePathLocal)
            vElement.Tag = xNote

            lvItems.Items.Add(vElement)
        Next
    End Sub

    Private Sub lvItems_ItemActivate(sender As Object, e As EventArgs) Handles lvItems.ItemActivate
        Dim ItemClass As NoteClass = CType(lvItems.SelectedItems(0).Tag, NoteClass)

        'Обновляем буфер
        appContext.ClipboardPaster.SetDataText(ItemClass.ItemValue.ToString())

        'Анимируем
        TimerAnimation.Start()
    End Sub

    Private Sub TimerAnimation_Tick(sender As Object, e As EventArgs) Handles TimerAnimation.Tick
        flagAnimRotator = Not flagAnimRotator
        countAnimRotator += 1

        If countAnimRotator > 4 Then
            countAnimRotator = 0
            flagAnimRotator = False

            TimerAnimation.Stop()
            Me.Close()
        End If

        Me.Text = "Поиск заметок" & If(flagAnimRotator, " - Заметка скопирована в буфер обмена !!!", "")
    End Sub

    'Кнопка выход, закрытие формы
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

End Class