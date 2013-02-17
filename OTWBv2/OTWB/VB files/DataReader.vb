Public Class DataReader
    Private fileLineArray() As String = Nothing
    Public fileContentArray() As recordtype2 = Nothing
    Public Sub New(ByVal path As String)
        Try
            fileLineArray = System.IO.File.ReadAllLines(path)
        Catch ex As Exception
            MsgBox(ex.Message.ToString())
        End Try
    End Sub

    Public Function GetRecords() As recordtype2()

        ReDim fileContentArray(fileLineArray.Length)
        Try
            Dim i As Integer = 0
            Dim s As String
            fileContentArray(i) = New recordtype2()
            i = i + 1
            For Each s In fileLineArray
                Dim fline As String = s.Split(":")(1)
                fileContentArray(i) = New recordtype2(fline)
                i = i + 1
            Next
        Catch ex As Exception
            MsgBox(ex.Message.ToString())
        End Try

        Return fileContentArray

    End Function

End Class
