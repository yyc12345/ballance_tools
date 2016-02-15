Imports Microsoft.Win32

Class MainWindow

    ''' <summary>
    ''' 打开包文件的打开框
    ''' </summary>
    Public open_packups_file As New Microsoft.Win32.OpenFileDialog
    ''' <summary>
    ''' 打开源文件的打开框
    ''' </summary>
    Public open_exe_file As New Microsoft.Win32.OpenFileDialog
    ''' <summary>
    ''' 打开ballance主文件文件的打开框
    ''' </summary>
    Public open_ballance_dir As New Microsoft.Win32.OpenFileDialog

    ''' <summary>
    ''' 保存包文件的保存框
    ''' </summary>
    Public save_packups_file As New Microsoft.Win32.SaveFileDialog

    ''' <summary>
    ''' cache要验证的md5,允许安装的文件的md5
    ''' </summary>
    Public cache_md5 As String = ""
    ''' <summary>
    ''' 材质要验证的md5,允许安装的文件的md5
    ''' </summary>
    Public material_md5 As String = ""
    ''' <summary>
    ''' 背景要验证的md5,允许安装的文件的md5
    ''' </summary>
    Public background_md5 As String = ""
    ''' <summary>
    ''' 音效要验证的md5,允许安装的文件的md5
    ''' </summary>
    Public wave_md5 As String = ""
    ''' <summary>
    ''' bgm要验证的md5,允许安装的文件的md5
    ''' </summary>
    Public bgm_md5 As String = ""
    ''' <summary>
    ''' bml要验证的md5,允许安装的文件的md5
    ''' </summary>
    Public bml_md5 As String = ""
    ''' <summary>
    ''' 关卡要验证的md5,允许安装的文件的md5
    ''' </summary>
    Public level_md5 As String = ""

    ''' <summary>
    ''' Ballance路径，带\
    ''' </summary>
    Public ballance_path As String = ""


    ''' <summary>
    ''' 选择文件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub select_packups(sender As Object, e As RoutedEventArgs)


        For a = 1 To 7
            Dim tabitem_linshi As TabItem = CType(tab_contorl.Items.Item(a), TabItem)

            If tabitem_linshi.IsSelected = True Then

                '区分分析，输入
                If a = 7 Then
                    open_exe_file.ShowDialog()
                    If System.IO.File.Exists(open_exe_file.FileName) = True Then
                        ui_bml_text.Text = open_exe_file.FileName
                    End If

                Else
                    open_packups_file.ShowDialog()
                    If System.IO.File.Exists(open_packups_file.FileName) = True Then
                        Select Case a
                            Case 1
                                ui_cache_text.Text = open_packups_file.FileName
                            Case 2
                                ui_material_text.Text = open_packups_file.FileName
                            Case 3
                                ui_background_text.Text = open_packups_file.FileName
                            Case 4
                                ui_wave_text.Text = open_packups_file.FileName
                            Case 5
                                ui_bgm_text.Text = open_packups_file.FileName
                            Case 6
                                ui_level_text.Text = open_packups_file.FileName
                        End Select
                    End If

                End If

            End If


        Next


    End Sub

    ''' <summary>
    ''' 部署
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub file_free(sender As Object, e As RoutedEventArgs)

        '等待窗口
        Dim linshi = New Window_wait
        linshi.Owner = Me
        linshi.Show()

        For a = 1 To 7
            Dim tabitem_linshi As TabItem = CType(tab_contorl.Items.Item(a), TabItem)

            If tabitem_linshi.IsSelected = True Then
                Select Case a
                    Case 1
                        If System.IO.File.Exists(ui_cache_text.Text) = True Then
                            Dim word_arr() As String = cache_md5.Split(",")
                            Dim file_md5 As String = get_file_md5(ui_cache_text.Text)
                            Dim yes As Boolean = False
                            For b = 0 To word_arr.Count - 1
                                If word_arr(b) = file_md5 Then
                                    yes = True
                                    Exit For
                                End If
                            Next

                            If yes = True Then

                                Try
                                    '解压缩
                                    Dim bbb As New ICSharpCode.SharpZipLib.Zip.FastZip
                                    bbb.ExtractZip(ui_cache_text.Text,
                                           Environment.CurrentDirectory & "\cache\", Nothing)
                                    MsgBox("部署成功", 64, "Ballance工具箱部署器")

                                Catch ex As Exception
                                    MsgBox("该文件部署时发生错误，部署失败！" & vbCrLf &
                                   "原因：" & ex.Message & vbCrLf &
                                   "详细消息：" & vbCrLf & ex.StackTrace, 16, "Ballance工具箱部署器")
                                End Try

                            Else
                                MsgBox("该文件不是有效的，所以无法部署！请确认其来源是否正确！", 16, "Ballance工具箱部署器")
                            End If
                        Else
                            MsgBox("要部署的包文件不存在！", 16, "Ballance工具箱部署器")
                        End If
                    Case 2
                        If System.IO.File.Exists(ui_material_text.Text) = True Then
                            Dim word_arr() As String = material_md5.Split(",")
                            Dim file_md5 As String = get_file_md5(ui_material_text.Text)
                            Dim yes As Boolean = False
                            For b = 0 To word_arr.Count - 1
                                If word_arr(b) = file_md5 Then
                                    yes = True
                                    Exit For
                                End If
                            Next

                            If yes = True Then
                                System.IO.File.Delete(Environment.CurrentDirectory & "\system_nmo\normal_material.zip")
                                System.IO.File.Copy(ui_material_text.Text, Environment.CurrentDirectory & "\system_nmo\normal_material.zip")
                                MsgBox("部署成功", 64, "Ballance工具箱部署器")
                            Else
                                MsgBox("该文件不是有效的，所以无法部署！请确认其来源是否正确！", 16, "Ballance工具箱部署器")
                            End If
                        Else
                            MsgBox("要部署的包文件不存在！", 16, "Ballance工具箱部署器")
                        End If
                    Case 3
                        If System.IO.File.Exists(ui_background_text.Text) = True Then
                            Dim word_arr() As String = background_md5.Split(",")
                            Dim file_md5 As String = get_file_md5(ui_background_text.Text)
                            Dim yes As Boolean = False
                            For b = 0 To word_arr.Count - 1
                                If word_arr(b) = file_md5 Then
                                    yes = True
                                    Exit For
                                End If
                            Next

                            If yes = True Then
                                System.IO.File.Delete(Environment.CurrentDirectory & "\system_nmo\normal_background.zip")
                                System.IO.File.Copy(ui_background_text.Text, Environment.CurrentDirectory & "\system_nmo\normal_background.zip")
                                MsgBox("部署成功", 64, "Ballance工具箱部署器")
                            Else
                                MsgBox("该文件不是有效的，所以无法部署！请确认其来源是否正确！", 16, "Ballance工具箱部署器")
                            End If
                        Else
                            MsgBox("要部署的包文件不存在！", 16, "Ballance工具箱部署器")
                        End If
                    Case 4
                        If System.IO.File.Exists(ui_wave_text.Text) = True Then
                            Dim word_arr() As String = wave_md5.Split(",")
                            Dim file_md5 As String = get_file_md5(ui_wave_text.Text)
                            Dim yes As Boolean = False
                            For b = 0 To word_arr.Count - 1
                                If word_arr(b) = file_md5 Then
                                    yes = True
                                    Exit For
                                End If
                            Next

                            If yes = True Then
                                System.IO.File.Delete(Environment.CurrentDirectory & "\system_nmo\normal_wave.zip")
                                System.IO.File.Copy(ui_wave_text.Text, Environment.CurrentDirectory & "\system_nmo\normal_wave.zip")
                                MsgBox("部署成功", 64, "Ballance工具箱部署器")
                            Else
                                MsgBox("该文件不是有效的，所以无法部署！请确认其来源是否正确！", 16, "Ballance工具箱部署器")
                            End If
                        Else
                            MsgBox("要部署的包文件不存在！", 16, "Ballance工具箱部署器")
                        End If
                    Case 5
                        If System.IO.File.Exists(ui_bgm_text.Text) = True Then
                            Dim word_arr() As String = bgm_md5.Split(",")
                            Dim file_md5 As String = get_file_md5(ui_bgm_text.Text)
                            Dim yes As Boolean = False
                            For b = 0 To word_arr.Count - 1
                                If word_arr(b) = file_md5 Then
                                    yes = True
                                    Exit For
                                End If
                            Next

                            If yes = True Then
                                System.IO.File.Delete(Environment.CurrentDirectory & "\system_nmo\normal_bgm.zip")
                                System.IO.File.Copy(ui_bgm_text.Text, Environment.CurrentDirectory & "\system_nmo\normal_bgm.zip")
                                MsgBox("部署成功", 64, "Ballance工具箱部署器")
                            Else
                                MsgBox("该文件不是有效的，所以无法部署！请确认其来源是否正确！", 16, "Ballance工具箱部署器")
                            End If
                        Else
                            MsgBox("要部署的包文件不存在！", 16, "Ballance工具箱部署器")
                        End If

                    Case 6

                        If System.IO.File.Exists(ui_level_text.Text) = True Then
                            Dim word_arr() As String = level_md5.Split(",")
                            Dim file_md5 As String = get_file_md5(ui_level_text.Text)
                            Dim yes As Boolean = False
                            For b = 0 To word_arr.Count - 1
                                If word_arr(b) = file_md5 Then
                                    yes = True
                                    Exit For
                                End If
                            Next

                            If yes = True Then

                                Try
                                    '解压缩
                                    Dim bbb As New ICSharpCode.SharpZipLib.Zip.FastZip
                                    '确认文件夹
                                    If System.IO.Directory.Exists(Environment.CurrentDirectory & "\system_nmo\level") = False Then
                                        System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\system_nmo\level")
                                    End If

                                    bbb.ExtractZip(ui_level_text.Text,
                                           Environment.CurrentDirectory & "\system_nmo\level\", Nothing)
                                    MsgBox("部署成功", 64, "Ballance工具箱部署器")

                                Catch ex As Exception
                                    MsgBox("该文件部署时发生错误，部署失败！" & vbCrLf &
                                   "原因：" & ex.Message & vbCrLf &
                                   "详细消息：" & vbCrLf & ex.StackTrace, 16, "Ballance工具箱部署器")
                                End Try

                            Else
                                MsgBox("该文件不是有效的，所以无法部署！请确认其来源是否正确！", 16, "Ballance工具箱部署器")
                            End If
                        Else
                            MsgBox("要部署的包文件不存在！", 16, "Ballance工具箱部署器")
                        End If

                    Case 7
                        If System.IO.File.Exists(ui_bml_text.Text) = True Then
                            Dim word_arr() As String = bml_md5.Split(",")
                            Dim file_md5 As String = get_file_md5(ui_bml_text.Text)
                            Dim yes As Boolean = False
                            For b = 0 To word_arr.Count - 1
                                If word_arr(b) = file_md5 Then
                                    yes = True
                                    Exit For
                                End If
                            Next

                            If yes = True Then
                                System.IO.File.Delete(Environment.CurrentDirectory & "\system_nmo\bml_install.exe")
                                System.IO.File.Copy(ui_bml_text.Text, Environment.CurrentDirectory & "\system_nmo\bml_install.exe")
                                MsgBox("部署成功", 64, "Ballance工具箱部署器")
                            Else
                                MsgBox("该文件不是有效的，所以无法部署！请确认其来源是否正确！", 16, "Ballance工具箱部署器")
                            End If
                        Else
                            MsgBox("要部署的包文件不存在！", 16, "Ballance工具箱部署器")
                        End If
                End Select
            End If

        Next

        linshi.Close()

    End Sub


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

    ''' <summary>
    ''' 应用启动
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub app_start(sender As Object, e As RoutedEventArgs)

        If System.IO.File.Exists(Environment.CurrentDirectory & "\ballance_tools.exe") = False Then

            MsgBox("该程序在错误的目录执行，请确保本程序在ballance_tools的根目录运行！", 16, "Ballance工具箱部署器")
            Environment.Exit(1)

        End If
        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\system_nmo") = False Then

            MsgBox("该程序在错误的目录执行，请确保本程序在ballance_tools的根目录运行！", 16, "Ballance工具箱部署器")
            Environment.Exit(1)

        End If

        app_init_reg()


        open_packups_file.Filter = "ZIP包文件|*.zip"
        open_exe_file.Filter = "源文件/可执行程序|*.exe"

        save_packups_file.Filter = "ZIP包文件|*.zip"

    End Sub

    ''' <summary>
    ''' 设置保存的zip文件地址
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub set_save_zip_path(sender As Object, e As RoutedEventArgs)

        For a = 1 To 6
            Dim tabitem_linshi As TabItem = CType(tab_contorl_pack.Items.Item(a), TabItem)

            If tabitem_linshi.IsSelected = True Then
                Select Case a
                    Case 1
                        save_packups_file.Filter = "材质包文件（*.bab）|*.bab"
                    Case 2
                        save_packups_file.Filter = "背景包文件（*.bbb）|*.bbb"
                    Case 3
                        save_packups_file.Filter = "BGM包文件（*.bgb）|*.bgb"
                    Case 4
                        save_packups_file.Filter = "音效包文件（*.bwb）|*.bwb"
                    Case 5
                        save_packups_file.Filter = "Mod包文件（*.bob）|*.bob"
                    Case 6
                        save_packups_file.Filter = "模型包文件（*.bpb）|*.bpb"
                End Select

                save_packups_file.ShowDialog()

                Select Case a
                    Case 1
                        ui_pack_material_text.Text = save_packups_file.FileName
                    Case 2
                        ui_pack_background_text.Text = save_packups_file.FileName
                    Case 3
                        ui_pack_bgm_text.Text = save_packups_file.FileName
                    Case 4
                        ui_pack_wave_text.Text = save_packups_file.FileName
                    Case 5
                        ui_pack_mod_text.Text = save_packups_file.FileName
                    Case 6
                        ui_pack_ph_text.Text = save_packups_file.FileName
                End Select


            End If


        Next

    End Sub

    ''' <summary>
    ''' 打包文件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub save_zip_pack(sender As Object, e As RoutedEventArgs)

        '等待窗口
        Dim linshi = New Window_wait
        linshi.Owner = Me
        linshi.Show()

        For a = 1 To 6
            Dim tabitem_linshi As TabItem = CType(tab_contorl_pack.Items.Item(a), TabItem)

            If tabitem_linshi.IsSelected = True Then

                Select Case a
                    Case 1

                        If ui_pack_material_text.Text = "" Then
                            MsgBox("未填写输出包地址，无法打包！", 16, "Ballance工具箱部署器")
                            linshi.Close()
                            Exit Sub
                        End If

                        Try
                            Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                            aaa.CreateZip(ui_pack_material_text.Text, ballance_path & "Textures", False, "")
                            MsgBox("打包成功", 64, "Ballance工具箱部署器")
                        Catch ex As Exception
                            MsgBox("该文件打包时发生错误，打包失败！" & vbCrLf &
                                   "原因：" & ex.Message & vbCrLf &
                                   "详细消息：" & vbCrLf & ex.StackTrace, 16, "Ballance工具箱部署器")
                            linshi.Close()
                            Exit Sub
                        End Try

                    Case 2

                        If ui_pack_background_text.Text = "" Then
                            MsgBox("未填写输出包地址，无法打包！", 16, "Ballance工具箱部署器")
                            linshi.Close()
                            Exit Sub
                        End If

                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Back.BMP", ballance_path & "Textures\Sky\Sky_Back.new")
                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Down.BMP", ballance_path & "Textures\Sky\Sky_Down.new")
                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Front.BMP", ballance_path & "Textures\Sky\Sky_Front.new")
                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Left.BMP", ballance_path & "Textures\Sky\Sky_Left.new")
                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Right.BMP", ballance_path & "Textures\Sky\Sky_Right.new")

                        Try
                            Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                            aaa.CreateZip(ui_pack_material_text.Text, ballance_path & "Textures\Sky", False, "new")
                            MsgBox("打包成功", 64, "Ballance工具箱部署器")
                        Catch ex As Exception
                            MsgBox("该文件打包时发生错误，打包失败！" & vbCrLf &
                                   "原因：" & ex.Message & vbCrLf &
                                   "详细消息：" & vbCrLf & ex.StackTrace, 16, "Ballance工具箱部署器")

                            System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Back.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Back.BMP")
                            System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Down.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Down.BMP")
                            System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Front.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Front.BMP")
                            System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Left.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Left.BMP")
                            System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Right.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Right.BMP")

                            linshi.Close()
                            Exit Sub

                        End Try

                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Back.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Back.BMP")
                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Down.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Down.BMP")
                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Front.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Front.BMP")
                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Left.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Left.BMP")
                        System.IO.File.Move(ballance_path & "Textures\Sky\Sky_Right.new", ballance_path & "Textures\Sky\Sky_" & Mid(ui_pack_background_group.Text, 6, 1) & "_Right.BMP")

                    Case 3

                        If ui_pack_bgm_text.Text = "" Then
                            MsgBox("未填写输出包地址，无法打包！", 16, "Ballance工具箱部署器")
                            linshi.Close()
                            Exit Sub
                        End If

                        For d = 1 To 3
                            For f = 1 To 3
                                System.IO.File.Move(ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".wav", ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".new")
                            Next
                        Next

                        Try
                            Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                            aaa.CreateZip(ui_pack_material_text.Text, ballance_path & "Sounds", False, "new")
                            MsgBox("打包成功", 64, "Ballance工具箱部署器")
                        Catch ex As Exception
                            MsgBox("该文件打包时发生错误，打包失败！" & vbCrLf &
                                   "原因：" & ex.Message & vbCrLf &
                                   "详细消息：" & vbCrLf & ex.StackTrace, 16, "Ballance工具箱部署器")

                            For d = 1 To 3
                                For f = 1 To 3
                                    System.IO.File.Move(ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".new", ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".wav")
                                Next
                            Next

                            linshi.Close()
                            Exit Sub

                        End Try

                        For d = 1 To 3
                            For f = 1 To 3
                                System.IO.File.Move(ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".new", ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".wav")
                            Next
                        Next

                    Case 4

                        If ui_pack_wave_text.Text = "" Then
                            MsgBox("未填写输出包地址，无法打包！", 16, "Ballance工具箱部署器")
                            linshi.Close()
                            Exit Sub
                        End If

                        For d = 1 To 3
                            For f = 1 To 3
                                System.IO.File.Move(ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".wav", ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".new")
                            Next
                        Next

                        Try
                            Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                            aaa.CreateZip(ui_pack_material_text.Text, ballance_path & "Sounds", False, "wav")
                            MsgBox("打包成功", 64, "Ballance工具箱部署器")
                        Catch ex As Exception
                            MsgBox("该文件打包时发生错误，打包失败！" & vbCrLf &
                                   "原因：" & ex.Message & vbCrLf &
                                   "详细消息：" & vbCrLf & ex.StackTrace, 16, "Ballance工具箱部署器")

                            For d = 1 To 3
                                For f = 1 To 3
                                    System.IO.File.Move(ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".new", ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".wav")
                                Next
                            Next

                            linshi.Close()
                            Exit Sub

                        End Try

                        For d = 1 To 3
                            For f = 1 To 3
                                System.IO.File.Move(ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".new", ballance_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".wav")
                            Next
                        Next

                    Case 5

                        If ui_pack_mod_text.Text = "" Then
                            MsgBox("未填写输出包地址，无法打包！", 16, "Ballance工具箱部署器")
                            linshi.Close()
                            Exit Sub
                        End If

                        If System.IO.File.Exists(ballance_path & "ModLoader\ModLoader.nmo") = True Then

                            Try
                                Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                                aaa.CreateZip(ui_pack_material_text.Text, ballance_path & "ModLoader\Mods", False, "nmo")
                                MsgBox("打包成功", 64, "Ballance工具箱部署器")
                            Catch ex As Exception
                                MsgBox("该文件打包时发生错误，打包失败！" & vbCrLf &
                                   "原因：" & ex.Message & vbCrLf &
                                   "详细消息：" & vbCrLf & ex.StackTrace, 16, "Ballance工具箱部署器")

                                linshi.Close()
                                Exit Sub

                            End Try

                        Else
                            MsgBox("无法打包，因为你当前未安装BML，所以没有任何Mod可以打包", 16, "Ballance工具箱部署器")

                            linshi.Close()
                            Exit Sub
                        End If

                    Case 6

                        If ui_pack_ph_text.Text = "" Then
                            MsgBox("未填写输出包地址，无法打包！", 16, "Ballance工具箱部署器")
                            linshi.Close()
                            Exit Sub
                        End If

                        Try
                            Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                            aaa.CreateZip(ui_pack_material_text.Text, ballance_path & "3D Entities\PH", False, "nmo")
                            MsgBox("打包成功", 64, "Ballance工具箱部署器")
                        Catch ex As Exception
                            MsgBox("该文件打包时发生错误，打包失败！" & vbCrLf &
                                   "原因：" & ex.Message & vbCrLf &
                                   "详细消息：" & vbCrLf & ex.StackTrace, 16, "Ballance工具箱部署器")

                            linshi.Close()
                            Exit Sub

                        End Try

                End Select

                '是否需要验证md5
                If MsgBox("确认需要输出包文件的MD5？这在你以后提交该文件验证时会有用", 32 + 1, "Ballance工具箱部署器") = 1 Then

                    Dim md5_word As String = ""
                    Select Case a
                        Case 1
                            md5_word = get_file_md5(ui_pack_material_text.Text)
                        Case 2
                            md5_word = get_file_md5(ui_pack_background_text.Text)
                        Case 3
                            md5_word = get_file_md5(ui_pack_bgm_text.Text)
                        Case 4
                            md5_word = get_file_md5(ui_pack_wave_text.Text)
                        Case 5
                            md5_word = get_file_md5(ui_pack_mod_text.Text)
                        Case 6
                            md5_word = get_file_md5(ui_pack_ph_text.Text)
                    End Select

                    MsgBox("文件的MD5为" & vbCrLf & md5_word, 64, "Ballance工具箱部署器")

                End If

            End If
        Next

        linshi.Close()


    End Sub


    Public Sub app_init_reg()

        '读取注册表

        Dim Key As RegistryKey = Registry.LocalMachine
        Dim b_full_screen As RegistryKey
        Dim info As String = ""
        Try
            If Environment.Is64BitOperatingSystem = True Then
                '64位系统
                b_full_screen = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)
            Else
                '32位
                b_full_screen = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)
            End If

            '地址
            If ballance_path = "" Then
                info = b_full_screen.GetValue("TargetDir").ToString


                '解决多行字符串读不出来的问题
                If info = "System.String[]" Then

                    '是multi_sz字符串读不出来
                    Dim ccc As String() = CType(b_full_screen.GetValue("TargetDir"), String())
                    info = ccc.GetValue(0).ToString()

                End If

                If Mid(info, info.Length, 1) <> "\" Then
                    info = info & "\"
                End If

                ballance_path = info

            End If

        Catch ex As Exception

            '错误意味着没有注册表项，默认一切，只写部分写注册表

            Dim b_full_screen_2 As RegistryKey
            Dim b_full_screen_3 As RegistryKey

            If Environment.Is64BitOperatingSystem = True Then
                '64位系统
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\Wow6432Node\ballance")
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\Wow6432Node\ballance\Settings")


                b_full_screen_3 = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)
                b_full_screen_3.SetValue("Fullscreen", "1", RegistryValueKind.DWord)

            Else
                '32位
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\ballance")
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\ballance\Settings")


                b_full_screen_3 = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)
                b_full_screen_3.SetValue("Fullscreen", "1", RegistryValueKind.DWord)

            End If

        End Try


        open_ballance_dir.Filter = "Ballance主启动程序|Startup.exe"
        open_ballance_dir.Title = "选择你安装的Ballance的主启动程序Startup.exe"

        If ballance_path = "" Then
            '没有读到地址
            MsgBox("没有找到Ballance的安装位置，请选择你安装的Ballance的主启动程序Startup.exe（位于 Ballance安装目录 下）", MsgBoxStyle.Exclamation, "Ballance工具箱部署器")

            open_ballance_dir.ShowDialog()

            If open_ballance_dir.FileName <> "" Then

                Dim info_2 As String = ""
                info_2 = open_ballance_dir.FileName
                info_2 = Mid(info_2, 1, info_2.Length - 11)

                If Mid(info_2, info_2.Length, 1) <> "\" Then
                    info_2 = info_2 & "\"
                End If
                ballance_path = info_2

            Else

                MsgBox("您必须选择Ballance主启动程序Startup.exe，否则这将无法开启工具箱部署器，现在程序即将退出！", 16, "Ballance工具箱部署器")
                Environment.Exit(1)
            End If

            If Environment.Is64BitOperatingSystem = True Then
                '64位系统
                b_full_screen = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)
            Else
                '32位
                b_full_screen = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)
            End If

            b_full_screen.SetValue("TargetDir", Mid(ballance_path, 1, ballance_path.Length - 1), RegistryValueKind.String)

        End If

        Key.Dispose()

    End Sub

    ''' <summary>
    ''' 选择背景编号
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_pack_background_select(sender As Object, e As RoutedEventArgs)

        Dim aaa As New ui_depend_window_select_item_list
        ui_connect_window_select_item_list.Clear()
        ui_connect_window_select_item_list_select_index = 0

        aaa.pro_title = "序号A"
        aaa.pro_text = "第 3 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号B"
        aaa.pro_text = "第 10 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号C"
        aaa.pro_text = "第 5 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号D"
        aaa.pro_text = "第 7 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号E"
        aaa.pro_text = "第 2 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号F"
        aaa.pro_text = "第 13 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号G"
        aaa.pro_text = "第 8 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号H"
        aaa.pro_text = "第 6 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号I"
        aaa.pro_text = "第 12 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号J"
        aaa.pro_text = "第 11 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号K"
        aaa.pro_text = "第 9 关背景"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "序号L"
        aaa.pro_text = "第 1 关背景"
        ui_connect_window_select_item_list.Add(aaa)

        '显示对话框

        Dim linshi = New Window_select_item
        ui_connect_window_select_item_list_select_index = -1
        linshi.Owner = Me
        linshi.ShowDialog()

        '安装
        If ui_connect_window_select_item_list_select_index <> -1 Then

            '处理文件
            Dim bk_list As String = ""
            Select Case ui_connect_window_select_item_list_select_index
                Case 0
                    bk_list = "A"
                Case 1
                    bk_list = "B"
                Case 2
                    bk_list = "C"
                Case 3
                    bk_list = "D"
                Case 4
                    bk_list = "E"
                Case 5
                    bk_list = "F"
                Case 6
                    bk_list = "G"
                Case 7
                    bk_list = "H"
                Case 8
                    bk_list = "I"
                Case 9
                    bk_list = "J"
                Case 10
                    bk_list = "K"
                Case 11
                    bk_list = "L"
                Case Else
                    MsgBox("发生错误", 16, "Ballance工具箱部署器")
                    Exit Sub
            End Select

            ui_pack_background_group.Text = "保存编号：" & bk_list

        End If

    End Sub

End Class
