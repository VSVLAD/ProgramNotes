Option Explicit On
Option Strict On

Imports System.IO
Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports Microsoft.Win32.SafeHandles

Namespace Opulos.Core.IO

    ''' <summary>
    ''' A faster way to get file information than System.IO.FileInfo.
    ''' </summary>
    ''' <remarks>
    ''' This enumerator is substantially faster than using <see cref="Directory.GetFiles(String)"/>
    ''' and then creating a new FileInfo object for each path.  Use this version when you 
    ''' will need to look at the attibutes of each file returned (for example, you need
    ''' to check each file in a directory to see if it was modified after a specific date).
    ''' </remarks>
    <Serializable>
    Public Class FastFileInfo

        Public ReadOnly Attributes As FileAttributes


        Public ReadOnly Property CreationTime() As Date
            Get
                Return Me.CreationTimeUtc.ToLocalTime()
            End Get
        End Property

        ''' <summary>
        ''' File creation time in UTC
        ''' </summary>
        Public ReadOnly CreationTimeUtc As Date

        ''' <summary>
        ''' Gets the last access time in local time.
        ''' </summary>
        Public ReadOnly Property LastAccesTime() As Date
            Get
                Return Me.LastAccessTimeUtc.ToLocalTime()
            End Get
        End Property

        ''' <summary>
        ''' File last access time in UTC
        ''' </summary>
        Public ReadOnly LastAccessTimeUtc As Date

        ''' <summary>
        ''' Gets the last access time in local time.
        ''' </summary>
        Public ReadOnly Property LastWriteTime() As Date
            Get
                Return Me.LastWriteTimeUtc.ToLocalTime()
            End Get
        End Property

        ''' <summary>
        ''' File last write time in UTC
        ''' </summary>
        Public ReadOnly LastWriteTimeUtc As Date

        ''' <summary>
        ''' Size of the file in bytes
        ''' </summary>
        Public ReadOnly Length As Long

        ''' <summary>
        ''' Name of the file
        ''' </summary>
        Public ReadOnly Name As String

        ''' <summary>
        ''' Shortened version of Name that has the tidle character
        ''' </summary>
        Public ReadOnly AlternateName As String

        ''' <summary>
        ''' Full path to the file.
        ''' </summary>
        Public ReadOnly FullName As String

        Public ReadOnly Property DirectoryName() As String
            Get
                Return Path.GetDirectoryName(FullName)
            End Get
        End Property

        Public ReadOnly Property Exists() As Boolean
            Get
                Return File.Exists(FullName)
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

        Public Sub New(filename As String)
            Me.New(New FileInfo(filename))
        End Sub

        Public Sub New(file As FileInfo)
            Me.Name = file.Name
            Me.FullName = file.FullName
            If file.Exists Then
                Me.Length = file.Length
                Me.Attributes = file.Attributes
                Me.CreationTimeUtc = file.CreationTimeUtc
                Me.LastAccessTimeUtc = file.LastAccessTimeUtc
                Me.LastWriteTimeUtc = file.LastWriteTimeUtc
            End If
        End Sub

        ''' <summary>Initializes a new instance of the <see cref="FastFileInfo"/> class.</summary>
        ''' <param name="dir">The directory that the file is stored at</param>
        ''' <param name="findData">WIN32_FIND_DATA structure that this object wraps.</param>
        Friend Sub New(dir As String, findData As WIN32_FIND_DATA)
            Me.Attributes = findData.dwFileAttributes
            Me.CreationTimeUtc = ConvertDateTime(findData.ftCreationTime_dwHighDateTime, findData.ftCreationTime_dwLowDateTime)
            Me.LastAccessTimeUtc = ConvertDateTime(findData.ftLastAccessTime_dwHighDateTime, findData.ftLastAccessTime_dwLowDateTime)
            Me.LastWriteTimeUtc = ConvertDateTime(findData.ftLastWriteTime_dwHighDateTime, findData.ftLastWriteTime_dwLowDateTime)
            Me.Length = CombineHighLowInts(findData.nFileSizeHigh, findData.nFileSizeLow)
            Me.Name = findData.cFileName
            Me.AlternateName = findData.cAlternateFileName
            Me.FullName = Path.Combine(dir, findData.cFileName)
        End Sub

        Private Shared Function CombineHighLowInts(high As UInteger, low As UInteger) As Long
            Return (CLng(high) << &H20) Or low
        End Function

        Private Shared Function ConvertDateTime(high As UInteger, low As UInteger) As Date
            Dim fileTime As Long = CombineHighLowInts(high, low)
            Return Date.FromFileTimeUtc(fileTime)
        End Function


        '---------------------------------
        ' static methods:

        ''' <summary>
        ''' Gets <see cref="FastFileInfo"/> for all the files in a directory.
        ''' </summary>
        ''' <param name="path">The path to search.</param>
        ''' <returns>An object that implements <see cref="IEnumerable{FastFileInfo}"/> and 
        ''' allows you to enumerate the files in the given directory.</returns>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="path"/> is a null reference (Nothing in VB)
        ''' </exception>
        Public Shared Function EnumerateFiles(path As String) As IEnumerable(Of FastFileInfo)
            Return EnumerateFiles(path, "*")
        End Function

        ''' <summary>
        ''' Gets <see cref="FastFileInfo"/> for all the files in a directory that match a 
        ''' specific filter.
        ''' </summary>
        ''' <param name="path">The path to search.</param>
        ''' <param name="searchPattern">The search string to match against files in the path.</param>
        ''' <returns>An object that implements <see cref="IEnumerable{FastFileInfo}"/> and 
        ''' allows you to enumerate the files in the given directory.</returns>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="path"/> is a null reference (Nothing in VB)
        ''' </exception>
        Public Shared Function EnumerateFiles(path As String, searchPattern As String) As IEnumerable(Of FastFileInfo)
            Return EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly)
        End Function

        ''' <summary>
        ''' Gets <see cref="FastFileInfo"/> for all the files in a directory that match a specific filter, optionally including all sub directories.
        ''' </summary>
        ''' <param name="path">The path to search.</param>
        ''' <param name="searchPattern">The search string to match against files in the path.</param>
        ''' <param name="searchOptions">
        ''' One of the SearchOption values that specifies whether the search 
        ''' operation should include all subdirectories or only the current directory.
        ''' </param>
        ''' <returns>An object that implements <see cref="IEnumerable{FastFileInfo}"/> and allows enumerating files in the specified directory.</returns>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="path"/> is a null reference (Nothing in VB)
        ''' </exception>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' <paramref name="searchOptions"/> is not one of the valid values of the
        ''' <see cref="System.IO.SearchOption"/> enumeration.
        ''' </exception>
        Public Shared Function EnumerateFiles(path As String, searchPattern As String, searchOptions As SearchOption) As IEnumerable(Of FastFileInfo)
            If path Is Nothing Then
                Throw New ArgumentNullException("path")
            End If

            If searchPattern Is Nothing Then
                Throw New ArgumentNullException("searchPattern")
            End If

            If (searchOptions <> SearchOption.TopDirectoryOnly) AndAlso (searchOptions <> SearchOption.AllDirectories) Then
                Throw New ArgumentOutOfRangeException("searchOption")
            End If

            Dim fullPath As String = System.IO.Path.GetFullPath(path)

            Return New FileEnumerable(fullPath, searchPattern, searchOptions)
        End Function

        Public Shared Function GetFiles2(path As String, Optional searchPattern As String = "*", Optional searchSubfolders As Boolean = False) As IList(Of FastFileInfo)
            Dim searchOptions As SearchOption = (If(searchSubfolders, SearchOption.AllDirectories, SearchOption.TopDirectoryOnly))
            Return GetFiles(path, searchPattern, searchOptions)
        End Function

        ''' <summary>
        ''' Gets <see cref="FastFileInfo"/> for all the files in a directory that match a specific filter.
        ''' </summary>
        ''' <param name="path">The path to search.</param>
        ''' <param name="searchPattern">The search string to match against files in the path. Multiple can be specified separated by the pipe character.</param>
        ''' <returns>An list of FastFileInfo objects that match the specified search pattern.</returns>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="path"/> is a null reference (Nothing in VB)
        ''' </exception>
        Public Shared Function GetFiles(path As String, Optional searchPattern As String = "*", Optional searchOptions As SearchOption = SearchOption.TopDirectoryOnly) As IList(Of FastFileInfo)
            Dim list As New List(Of FastFileInfo)()
            Dim arr As String() = searchPattern.Split(New Char() {"|"c}, StringSplitOptions.RemoveEmptyEntries)
            Dim ht As Hashtable = (If(arr.Length > 1, New Hashtable(), Nothing))
            ' don't need to worry about case since it should be consistent
            For Each sp As String In arr
                Dim sp2 As String = sp.Trim()
                If sp2.Length = 0 Then
                    Continue For
                End If

                Dim e As IEnumerable(Of FastFileInfo) = EnumerateFiles(path, sp2, searchOptions)
                If ht Is Nothing Then
                    list.AddRange(e)
                Else
                    Dim e2 = e.GetEnumerator()
                    If ht.Count = 0 Then
                        While e2.MoveNext()
                            Dim f As FastFileInfo = e2.Current
                            list.Add(f)
                            ht(f.FullName) = f
                        End While
                    Else
                        While e2.MoveNext()
                            Dim f As FastFileInfo = e2.Current
                            If Not ht.Contains(f.FullName) Then
                                list.Add(f)
                                ht(f.FullName) = f
                            End If
                        End While
                    End If
                End If
            Next

            Return list
        End Function

        Private Class FileEnumerable
            Implements IEnumerable(Of FastFileInfo)
            Private ReadOnly path As String
            Private ReadOnly filter As String
            Private ReadOnly searchOptions As SearchOption

            Public Sub New(path As String, filter As String, searchOption As SearchOption)
                Me.path = path
                Me.filter = filter
                Me.searchOptions = searchOption
            End Sub

            Public Function GetEnumerator() As IEnumerator(Of FastFileInfo) Implements IEnumerable(Of FastFileInfo).GetEnumerator
                Return New FileEnumerator(path, filter, searchOptions, True, False, False)
            End Function

            Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return New FileEnumerator(path, filter, searchOptions, True, False, False)
            End Function
        End Class

        ' Wraps a FindFirstFile handle.
        Private NotInheritable Class SafeFindHandle
            Inherits SafeHandleZeroOrMinusOneIsInvalid
            <ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)>
            <DllImport("kernel32.dll")>
            Private Shared Function FindClose(handle As IntPtr) As Boolean
            End Function

            <SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode:=True)>
            Friend Sub New()
                MyBase.New(True)
            End Sub

            ''' <summary>
            ''' When overridden in a derived class, executes the code required to free the handle.
            ''' </summary>
            ''' <returns>
            ''' true if the handle is released successfully; otherwise, in the 
            ''' event of a catastrophic failure, false. In this case, it 
            ''' generates a releaseHandleFailed MDA Managed Debugging Assistant.
            ''' </returns>
            Protected Overrides Function ReleaseHandle() As Boolean
                Return FindClose(MyBase.handle)
            End Function
        End Class


        <Security.SuppressUnmanagedCodeSecurity>
        Public Class FileEnumerator
            Implements IEnumerator(Of FastFileInfo)

            <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
            Private Shared Function FindFirstFile(fileName As String, <[In], Out> data As WIN32_FIND_DATA) As SafeFindHandle
            End Function

            <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
            Private Shared Function FindNextFile(hndFindFile As SafeFindHandle, <[In], Out, MarshalAs(UnmanagedType.LPStruct)> lpFindFileData As WIN32_FIND_DATA) As Boolean
            End Function

            <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
            Private Shared Function FindFirstFileEx(fileName As String, infoLevel As Integer, <[In], Out> data As WIN32_FIND_DATA, searchScope As Integer, notUsedNull As String, additionalFlags As Integer) As SafeFindHandle
            End Function

            Private initialFolder As String
            Private searchOption As SearchOption
            Private searchFilter As String
            '---
            Private currentFolder As String
            Private hndFile As SafeFindHandle
            Private findData As WIN32_FIND_DATA
            Private currentPathIndex As Integer
            Private currentPaths As IList(Of String)
            Private pendingFolders As IList(Of String)
            Private queue As Queue(Of IList(Of String))
            Private advanceNext As Boolean
            Private usePendingFolders As Boolean = False
            Private useGetDirectories As Boolean = False
            Private hasCurrent As Boolean = False
            '---
            Private useEx As Boolean = False
            Private infoLevel As Integer = 0
            Private searchScope As Integer = 0
            ' always files (1 = limit to directories, 2 = limit to devices (not supported))
            Private additionalFlags As Integer = 0

            Public Sub New(initialFolder As String, searchFilter As String, searchOption As SearchOption)
                init(initialFolder, searchFilter, searchOption)
            End Sub

            ' basicInfoOnly is about 30% faster. E.g. the C:\Windows\ directory takes 4.3 sec for standard info, and 3.3 sec for basic info.
            ' basicInfoOnly excludes getting the cAlternateName, which is the short name with the tidle character)
            '
            ' Note: case sensitive only works if \HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel\obcaseinsensitive is set to 0
            ' which is probably not a good idea.
            Public Sub New(initialFolder As String, searchFilter As String, searchOption As SearchOption, basicInfoOnly As Boolean, caseSensitive As Boolean, largeBuffer As Boolean)
                init(initialFolder, searchFilter, searchOption)
                useEx = True
                infoLevel = (If(basicInfoOnly, 1, 0))
                ' 0 is standard (includes the cAlternateName, which is the short name with the tidle character)
                additionalFlags = additionalFlags Or (If(caseSensitive, 1, 0))
                additionalFlags = additionalFlags Or (If(largeBuffer, 2, 0))
            End Sub

            Private Sub init(initialFolder As String, searchFilter As String, searchOptions As SearchOption)
                Me.initialFolder = initialFolder
                Me.searchFilter = searchFilter
                Me.searchOption = searchOptions
                ' usePendingFolders is 60% faster. E.g. the C:\Windows\ directory takes 7.7 seconds if using Directory.GetDirectories
                ' but only takes 3.3 seconds if the folders are cached as they are encountered.
                Me.usePendingFolders = (searchFilter = "*" OrElse searchFilter = "*.*") AndAlso searchOptions = SearchOption.AllDirectories
                ' The problem is that when a filter like *.txt is used, none of the directories are returned by FindNextFile.
                Me.useGetDirectories = Not usePendingFolders AndAlso searchOptions = SearchOption.AllDirectories
                Reset()
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose
                If hndFile IsNot Nothing Then
                    hndFile.Dispose()
                    hndFile = Nothing
                End If
            End Sub

            Public ReadOnly Property Current() As FastFileInfo Implements IEnumerator(Of FastFileInfo).Current
                Get
                    Return New FastFileInfo(currentFolder, findData)
                End Get
            End Property

            Private ReadOnly Property System_Collections_IEnumerator_Current() As Object Implements IEnumerator.Current
                Get
                    Return New FastFileInfo(currentFolder, findData)
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                While True
                    If advanceNext Then
                        hasCurrent = FindNextFile(hndFile, findData)
                    End If

                    If hasCurrent OrElse Not advanceNext Then
                        ' first skip over any directories, but store them if usePendingFolders is true
                        While (findData.dwFileAttributes And FileAttributes.Directory) = FileAttributes.Directory
                            If usePendingFolders Then
                                Dim c As String = findData.cFileName
                                If Not (c(0) = "."c AndAlso (c.Length = 1 OrElse c(1) = "."c AndAlso c.Length = 2)) Then
                                    ' skip folders '.' and '..'
                                    pendingFolders.Add(Path.Combine(currentFolder, c))
                                End If
                            End If
                            hasCurrent = FindNextFile(hndFile, findData)
                            If Not hasCurrent Then
                                Exit While
                            End If
                        End While
                    End If

                    If hasCurrent Then
                        advanceNext = True
                        Return True
                    End If

                    If useGetDirectories Then
                        ' even though the docs claim searchScope '1' only returns directories, it actually returns files and directories
                        Dim h = FindFirstFileEx(Path.Combine(currentFolder, "*"), 1, findData, 1, Nothing, 0)
                        If Not h.IsInvalid Then
                            While True
                                If (findData.dwFileAttributes And FileAttributes.Directory) = FileAttributes.Directory Then
                                    Dim c As String = findData.cFileName
                                    If Not (c(0) = "."c AndAlso (c.Length = 1 OrElse c(1) = "."c AndAlso c.Length = 2)) Then
                                        ' skip folders '.' and '..'
                                        pendingFolders.Add(Path.Combine(currentFolder, c))
                                    End If
                                End If

                                If Not FindNextFile(h, findData) Then
                                    Exit While
                                End If
                            End While
                        End If

                        ' using this code is twice as slow. E.g. the C:\Windows\ folder took 7.4 sec versus 3.8 sec.
                        'try {
                        '	pendingFolders = Directory.GetDirectories(currentFolder);
                        '} catch {} // Access to the path '...\System Volume Information' is denied.

                        Try
                            pendingFolders = Directory.GetDirectories(currentFolder)
                        Catch ex As Exception

                        End Try

                        h.Dispose()
                    End If

                    ' at this point, the current folder is exhausted. If search subfolders then enqueue them.
                    If pendingFolders.Count > 0 Then
                        queue.Enqueue(pendingFolders)
                        pendingFolders = New List(Of String)()
                    End If

                    currentPathIndex += 1
                    If currentPathIndex = currentPaths.Count Then
                        ' at the end of the current paths
                        If queue.Count = 0 Then
                            currentPathIndex -= 1
                            ' so that calling MoveNext() after very last has no impact
                            ' no more paths to process
                            Return False
                        End If
                        currentPaths = queue.Dequeue()
                        currentPathIndex = 0
                    End If

                    Dim f As String = currentPaths(currentPathIndex)
                    InitFolder(f)
                End While

                Return False
            End Function

            ' returns true if the folder can be searched
            Private Sub InitFolder(folder As String)
                If hndFile IsNot Nothing Then
                    hndFile.Dispose()
                End If

                Dim fp As New FileIOPermission(FileIOPermissionAccess.PathDiscovery, folder)
                fp.Demand()

                Dim searchPath As String = Path.Combine(folder, searchFilter)
                If useEx Then
                    hndFile = FindFirstFileEx(searchPath, infoLevel, findData, searchScope, Nothing, additionalFlags)
                Else
                    hndFile = FindFirstFile(searchPath, findData)
                End If
                currentFolder = folder
                advanceNext = False
                hasCurrent = Not hndFile.IsInvalid
                ' e.g. unaccessible C:\System Volume Information or filter like *.txt in a directory with no text files
            End Sub

            Public Sub Reset() Implements IEnumerator.Reset
                currentPathIndex = 0
                advanceNext = False
                hasCurrent = False
                currentPaths = New List(Of String) From {initialFolder}
                findData = New WIN32_FIND_DATA
                pendingFolders = New List(Of String)
                queue = New Queue(Of IList(Of String))
                InitFolder(initialFolder)
            End Sub
        End Class
    End Class

    ''' <summary>
    ''' Contains information about the file that is found by the FindFirstFile or FindNextFile functions.
    ''' </summary>
    <Serializable, StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto), BestFitMapping(False)>
    Friend Class WIN32_FIND_DATA
        Public dwFileAttributes As FileAttributes
        Public ftCreationTime_dwLowDateTime As UInteger
        Public ftCreationTime_dwHighDateTime As UInteger
        Public ftLastAccessTime_dwLowDateTime As UInteger
        Public ftLastAccessTime_dwHighDateTime As UInteger
        Public ftLastWriteTime_dwLowDateTime As UInteger
        Public ftLastWriteTime_dwHighDateTime As UInteger
        Public nFileSizeHigh As UInteger
        Public nFileSizeLow As UInteger
        Public dwReserved0 As Integer
        Public dwReserved1 As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
        Public cFileName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=14)>
        Public cAlternateFileName As String

        Public Overrides Function ToString() As String
            Return Convert.ToString("File name=") & cFileName
        End Function
    End Class

End Namespace