Module systems

    ''' <summary>
    ''' 获取文件的md5
    ''' </summary>
    ''' <param name="filename">文件路径</param>
    ''' <returns></returns>
    Public Function get_file_md5(ByVal filename As String)

        Try
            Dim file As System.IO.FileStream = New System.IO.FileStream(filename, IO.FileMode.Open, IO.FileAccess.Read)

            Dim md5 As System.Security.Cryptography.MD5 = New System.Security.Cryptography.MD5CryptoServiceProvider
            Dim retval As Byte() = md5.ComputeHash(file)
            file.Close()

            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
            For i = 0 To retval.Length - 1
                sb.Append(retval(i).ToString("x2"))
            Next

            Return sb.ToString

        Catch ex As Exception
            Return ""
        End Try


    End Function


End Module
