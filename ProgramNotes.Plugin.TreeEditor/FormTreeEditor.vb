Option Explicit On
Option Strict On

Imports System.IO
Imports System.Text
Imports ProgramNotes.API

Public Class FormTreeEditor

    Private WithEvents appContext As IContext

    Private selectedFolderPath As String = ""
    Private selectedFileName As String = ""
    Private folderPlaceHolder As String = ".."

    'Загрузка формы
    Public Sub InitForm(Context As IContext)
        Me.appContext = Context
    End Sub

    Private Sub FormTreeEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim imagesListTree As New ImageList

        imagesListTree.Images.Add("Drive", ProgramNotes.Resources.img_drive)
        imagesListTree.Images.Add("Folder", ProgramNotes.Resources.img_folder)
        imagesListTree.Images.Add("File", ProgramNotes.Resources.img_file)
        imagesListTree.Images.Add("Folder_Error", ProgramNotes.Resources.img_folder_err)

        tvFiles.ImageList = imagesListTree
        tvFiles.ImageKey = "Folder"
        tvFiles.SelectedImageKey = "Folder"

        RefreshTreeView()
    End Sub

    'Загрузка всего списка
    Public Sub RefreshTreeView()
        tvFiles.Nodes.Clear()

        'Добавить корневой диск (он же корневая папка с заметками)
        Dim DriveNode = tvFiles.Nodes.Add("Заметки", "Заметки", "Drive", "Drive")
        DriveNode.Tag = appContext.FolderNotesPath

        TreeLoadNodes(tvFiles, appContext.FolderNotesPath, DriveNode) 'Загружаем начальный путь
        DriveNode.Expand() 'Развернуть диск
    End Sub

    'Загружаем ветки
    Public Sub TreeLoadNodes(ByVal TreeView As TreeView, ByVal FullPath As String, Optional Node As TreeNode = Nothing)
        Dim dirInfo As DirectoryInfo = New DirectoryInfo(FullPath)
        Dim dirNodes As TreeNodeCollection = If(Node IsNot Nothing, Node.Nodes, TreeView.Nodes)

        TreeView.BeginUpdate()

        For Each xDirectory In dirInfo.EnumerateDirectories()
            Dim addDirNode = dirNodes.Add(xDirectory.Name, xDirectory.Name, "Folder", "Folder")
            addDirNode.Tag = xDirectory.FullName

            'Добавляем виртуальный элемент для "+" у папки
            Try
                If xDirectory.EnumerateFileSystemInfos().Count() > 0 Then
                    addDirNode.Nodes.Add(folderPlaceHolder, folderPlaceHolder, "Folder", "Folder")
                End If
            Catch ex As UnauthorizedAccessException
                addDirNode.ImageKey = "Folder_Error"
                addDirNode.SelectedImageKey = "Folder_Error"
            End Try
        Next

        For Each xFile In dirInfo.EnumerateFiles()
            Dim addFileNode = dirNodes.Add(xFile.Name, xFile.Name, "File", "File")
            addFileNode.Tag = xFile.FullName
        Next

        TreeView.EndUpdate()
    End Sub

    'Выделение элемента
    Private Sub tvFiles_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvFiles.NodeMouseDoubleClick
        If e.Node.ImageKey <> "File" Then Return

        Dim strFullPath As String = e.Node.Tag.ToString()
        If Not File.Exists(strFullPath) Then Return

        txtNote.Text = File.ReadAllText(strFullPath, appContext.NotesEncoding)
    End Sub

    'Перед раскрытием удаляем виртуальный элемент и подгружаем новые
    Private Sub tvFiles_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles tvFiles.BeforeExpand
        If e.Node.Nodes.ContainsKey(folderPlaceHolder) Then
            e.Node.Nodes.RemoveByKey(folderPlaceHolder)
            TreeLoadNodes(CType(sender, TreeView), e.Node.Tag.ToString(), e.Node)
        End If
    End Sub

    'После выбора
    Private Sub tvFiles_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvFiles.AfterSelect
        Select Case e.Node.ImageKey
            Case "Drive"
                selectedFolderPath = e.Node.Tag.ToString()
                selectedFileName = ""
                mnuFolderRename.Enabled = False
                mnuFolderDelete.Enabled = False

            Case "Folder"
                selectedFolderPath = e.Node.Tag.ToString()
                selectedFileName = ""
                mnuFolderRename.Enabled = True
                mnuFolderDelete.Enabled = True
            Case "File"
                selectedFolderPath = Path.GetDirectoryName(e.Node.Tag.ToString())
                selectedFileName = e.Node.Tag.ToString()
                mnuFolderRename.Enabled = True
                mnuFolderDelete.Enabled = True
        End Select

        Me.Text = Application.ProductName & " - " & selectedFolderPath
    End Sub

#Region "Обработка меню"

    'Новая папка
    Private Sub mnuFolderNew_Click(sender As Object, e As EventArgs) Handles mnuFolderNew.Click
        Dim strName As String = InputBox("Введите название папки", Application.ProductName)

        If Not String.IsNullOrEmpty(strName) AndAlso Not String.IsNullOrWhiteSpace(strName) Then
            If Directory.Exists(selectedFolderPath) Then
                Dim strFullPath As String = Path.Combine(selectedFolderPath, strName)

                Try
                    Directory.CreateDirectory(strFullPath)
                    RefreshTreeView()
                Catch ex As Exception
                    MessageBox.Show($"Невозможно создать папку ""{strFullPath}""!" & vbCrLf & ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End Try
            Else
                MessageBox.Show($"Выбранная папка ""{selectedFolderPath}"" не существует!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub

    'Переименовать папку
    Private Sub mnuFolderRename_Click(sender As Object, e As EventArgs) Handles mnuFolderRename.Click

        If Not Directory.Exists(selectedFolderPath) Then
            MessageBox.Show($"Выбранная папка ""{selectedFolderPath}"" не существует!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Dim parentFolder As String = Directory.GetParent(selectedFolderPath).FullName
        Dim strNewName As String = InputBox("Введите новое имя папки", Application.ProductName, New DirectoryInfo(selectedFolderPath).Name)

        If String.IsNullOrEmpty(strNewName) OrElse String.IsNullOrWhiteSpace(strNewName) Then Return

        Dim strOldFullPath As String = selectedFolderPath
        Dim strNewFullPath As String = Path.Combine(parentFolder, strNewName)

        Try
            Directory.Move(strOldFullPath, strNewFullPath)
            RefreshTreeView()
        Catch ex As Exception
            MessageBox.Show($"Невозможно переименовать папку с ""{strOldFullPath}"" на ""{strNewFullPath}""!" & vbCrLf & ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    'Удалить папку
    Private Sub mnuFolderDelete_Click(sender As Object, e As EventArgs) Handles mnuFolderDelete.Click
        If Directory.Exists(selectedFolderPath) AndAlso MessageBox.Show($"Удалить выбранную папку ""{selectedFolderPath}""?", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = vbOK Then
            Try
                Directory.Delete(selectedFolderPath, True)
                RefreshTreeView()
            Catch ex As Exception
                MessageBox.Show($"Невозможно удалить папку ""{selectedFolderPath}""!" & vbCrLf & ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End If
    End Sub

    'Перечитать папки
    Private Sub mnuFolderRefresh_Click(sender As Object, e As EventArgs) Handles mnuFolderRefresh.Click
        RefreshTreeView()
    End Sub

    'Закрытие формы
    Private Sub mnuFileExit_Click(sender As Object, e As EventArgs) Handles mnuFileExit.Click, btnExit.Click
        Me.Close()
    End Sub

    'Новый файл
    Private Sub mnuFileNew_Click(sender As Object, e As EventArgs) Handles mnuFileNew.Click
        Dim strName As String = InputBox("Введите имя файла", Application.ProductName)

        If Not String.IsNullOrEmpty(strName) AndAlso Not String.IsNullOrWhiteSpace(strName) Then
            If Directory.Exists(selectedFolderPath) Then
                Dim strFullPath As String = Path.Combine(selectedFolderPath, strName)

                If File.Exists(strFullPath) AndAlso MessageBox.Show($"Перезаписать существующий файл ""{strFullPath}""?", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = vbCancel Then
                    Return
                End If

                Try
                    File.WriteAllText(strFullPath, txtNote.Text, appContext.NotesEncoding)
                    RefreshTreeView()
                Catch ex As Exception
                    MessageBox.Show($"Невозможно создать файл ""{strFullPath}""!" & vbCrLf & ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End Try
            Else
                MessageBox.Show($"Выбранная папка ""{selectedFolderPath}"" не существует!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub

    'Удалить файл
    Private Sub mnuFileDelete_Click(sender As Object, e As EventArgs) Handles mnuFileDelete.Click
        If File.Exists(selectedFileName) AndAlso MessageBox.Show($"Удалить выбранный файл ""{selectedFileName}""?", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = vbOK Then
            Try
                File.Delete(selectedFileName)
                RefreshTreeView()
            Catch ex As Exception
                MessageBox.Show($"Невозможно удалить файл ""{selectedFolderPath}""!" & vbCrLf & ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End If
    End Sub

    'Переименовать файл
    Private Sub mnuFileRename_Click(sender As Object, e As EventArgs) Handles mnuFileRename.Click
        If Not Directory.Exists(selectedFolderPath) Then
            MessageBox.Show($"Выбранная папка ""{selectedFolderPath}"" не существует!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        If Not File.Exists(selectedFileName) Then
            MessageBox.Show($"Выбранный файл ""{selectedFileName}"" не существует!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Dim strOldName As String = Path.GetFileName(selectedFileName)
        Dim strNewName As String = InputBox("Введите новое имя файла", Application.ProductName, strOldName)

        If String.IsNullOrEmpty(strNewName) OrElse String.IsNullOrWhiteSpace(strNewName) Then Return

        Dim strOldFullPath As String = Path.Combine(selectedFolderPath, strOldName)
        Dim strNewFullPath As String = Path.Combine(selectedFolderPath, strNewName)

        Try
            File.Move(strOldFullPath, strNewFullPath)
            RefreshTreeView()
        Catch ex As Exception
            MessageBox.Show($"Невозможно переименовать файл с ""{strOldFullPath}"" на ""{strNewFullPath}""!" & vbCrLf & ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    'Сохранить заметку
    Private Sub mnuFileSave_Click(sender As Object, e As EventArgs) Handles mnuFileSave.Click
        If File.Exists(selectedFileName) AndAlso MessageBox.Show($"Перезаписать существующий файл ""{selectedFileName}""?", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = vbCancel Then
            Return
        End If

        Try
            File.WriteAllText(selectedFileName, txtNote.Text, appContext.NotesEncoding)
        Catch ex As Exception
            MessageBox.Show($"Невозможно создать файл ""{selectedFileName}""!" & vbCrLf & ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    'Была изменена кодировка
    Private Sub appContext_EncodingChanged(NewEncoding As Encoding) Handles appContext.EncodingChanged
        If File.Exists(selectedFileName) Then
            txtNote.Text = File.ReadAllText(selectedFileName, NewEncoding)
        End If
    End Sub

#End Region

End Class