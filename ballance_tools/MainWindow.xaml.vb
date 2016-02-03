Imports Microsoft.Win32

Class MainWindow

    ''' <summary>
    ''' 播放背景音乐的soundplayer
    ''' </summary>
    Public play_bgm As System.Media.SoundPlayer

#Region "文件打开对话框"
    ''' <summary>
    ''' 打开地图包文件的打开框
    ''' </summary>
    Public open_map_bag_file As New Microsoft.Win32.OpenFileDialog
    ''' <summary>
    ''' 打开地图文件的打开框
    ''' </summary>
    Public open_map_file As New Microsoft.Win32.OpenFileDialog
    ''' <summary>
    ''' 打开背景包文件的打开框
    ''' </summary>
    Public open_background_file As New Microsoft.Win32.OpenFileDialog
    ''' <summary>
    ''' 打开材质包文件的打开框
    ''' </summary>
    Public open_material_file As New Microsoft.Win32.OpenFileDialog
    ''' <summary>
    ''' 打开音效包文件的打开框
    ''' </summary>
    Public open_wave_file As New Microsoft.Win32.OpenFileDialog
    ''' <summary>
    ''' 打开bgm包文件的打开框
    ''' </summary>
    Public open_bgm_file As New Microsoft.Win32.OpenFileDialog
    ''' <summary>
    ''' 打开mod包文件的打开框
    ''' </summary>
    Public open_mod_file As New Microsoft.Win32.OpenFileDialog
    ''' <summary>
    ''' 打开模组包文件的打开框
    ''' </summary>
    Public open_ph_file As New Microsoft.Win32.OpenFileDialog
#End Region

#Region "程序核心"

    Private Sub app_init(sender As Object, e As RoutedEventArgs)

        '等待窗口
        Dim linshi = New Window_wait
        linshi.Owner = Me
        linshi.Show()

        '初始化目录
        app_init_set_folder()

        '接受命令行
        If Command() <> "" Then
            '有命令行

            If Command() = "NoCheck" Then
                '不执行资源检测

                Try
                    app_init_reg(Nothing, False)
                    app_init_map(Nothing, False)
                    app_init_backups(Nothing, False)
                    app_init_background_bag(Nothing, False)
                    app_init_material(Nothing, False)
                    app_init_wave(Nothing, False)
                    app_init_bgm(Nothing, False)
                    app_init_mod(Nothing, False)
                    app_init_ph(Nothing, False)

                    app_init_user_contorl(Nothing, False)

                    app_progress_background(Nothing, False)
                    app_init_background(Nothing, False)
                Catch ex As Exception
                    MsgBox(ex.Message, 16, "Ballance工具箱---发生错误")
                    Environment.Exit(250)
                End Try

            End If

        Else
            '没有命令行，直接检测开始

            '事件侦测装置
            Dim run_log As New System.IO.StreamWriter(Environment.CurrentDirectory & "\RunLog.log", False, System.Text.Encoding.Default)
            run_log.WriteLine("------------------" & Now().ToString & "------------------")
            run_log.WriteLine("已启动事件侦测功能")
            run_log.WriteLine("")

            Try
                run_log.WriteLine("---app_init_reg 加载注册表---")
                app_init_reg(run_log, True)
                run_log.WriteLine("")
                run_log.WriteLine("---app_init_map 加载地图---")
                app_init_map(run_log, True)
                run_log.WriteLine("")
                run_log.WriteLine("---app_init_backups 加载tdb文件备份---")
                app_init_backups(run_log, True)
                run_log.WriteLine("")
                run_log.WriteLine("---app_init_background 加载背景包---")
                app_init_background_bag(run_log, True)
                run_log.WriteLine("")
                run_log.WriteLine("---app_init_material 加载材质包---")
                app_init_material(run_log, True)
                run_log.WriteLine("")
                run_log.WriteLine("---app_init_wave 加载音效包---")
                app_init_wave(run_log, True)
                run_log.WriteLine("")
                run_log.WriteLine("---app_init_bgm 加载bgm包---")
                app_init_bgm(run_log, True)
                run_log.WriteLine("")
                run_log.WriteLine("---app_init_mod 加载mod---")
                app_init_mod(run_log, True)
                run_log.WriteLine("")
                run_log.WriteLine("---app_init_ph 加载模型(ph)---")
                app_init_ph(run_log, True)
                run_log.WriteLine("")

                run_log.WriteLine("---app_init_user_contorl 初始化用户控件---")
                app_init_user_contorl(run_log, True)
                run_log.WriteLine("")

                run_log.WriteLine("---app_init_progress_background 处理ballance下texture中old文件覆盖---")
                app_progress_background(run_log, True)
                run_log.WriteLine("")
                run_log.WriteLine("---app_init_background 加载程序中关卡背景预览背景---")
                app_init_background(run_log, True)
                run_log.WriteLine("")
            Catch ex As Exception
                run_log.WriteLine("------------------发生异常------------------")
                run_log.WriteLine("---主要消息---")
                run_log.WriteLine(ex.Message)
                run_log.WriteLine("---导致错误的对象或应用程序的名称---")
                run_log.WriteLine(ex.Source)
                run_log.WriteLine("---调用堆栈上的直接帧的字符串形式---")
                run_log.WriteLine(ex.StackTrace)
                run_log.WriteLine("---引发当前异常的方法---")
                run_log.WriteLine(ex.TargetSite)
                MsgBox(ex.Message, 16, "Ballance工具箱---发生错误")
                run_log.Dispose()
                Environment.Exit(250)
            End Try

            run_log.WriteLine("检测服务已终止")
            run_log.WriteLine(Now().ToString)
            run_log.Dispose()

        End If

        System.GC.Collect()

        linshi.Close()


    End Sub


    ''' <summary>
    ''' 初始化注册表阶段
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_reg(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)

        '读取注册表

        Dim Key As RegistryKey = Registry.LocalMachine
        Dim b_full_screen As RegistryKey
        Dim info As String = ""
        Try
            If Environment.Is64BitOperatingSystem = True Then
                '64位系统
                b_full_screen = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)

                If use_debug_log = True Then
                    debug_log.WriteLine("已检测到64位系统，定位到SOFTWARE\Wow6432Node\ballance\Settings下")
                End If

            Else
                '32位
                b_full_screen = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)

                If use_debug_log = True Then
                    debug_log.WriteLine("已检测到32位系统，定位到SOFTWARE\ballance\Settings下")
                End If

            End If

            info = b_full_screen.GetValue("Fullscreen").ToString

            If use_debug_log = True Then
                debug_log.WriteLine("获取Fullscreen：" & info)
            End If

            If info = "0" Then
                ui_form_setting_form_1_fullscreen.Text = "当前全屏状态：窗口化"
                ui_form_setting_form_1_fullscreen_btn.Content = "修改为 全屏"
            Else
                ui_form_setting_form_1_fullscreen.Text = "当前全屏状态：全屏"
                ui_form_setting_form_1_fullscreen_btn.Content = "修改为 窗口化"
            End If

            If use_debug_log = True Then
                debug_log.WriteLine(ui_form_setting_form_1_fullscreen.Text)
            End If

            '地址
            If ballance_start_path = "" Then
                info = b_full_screen.GetValue("TargetDir").ToString

                If use_debug_log = True Then
                    debug_log.WriteLine("获取TargetDir：" & info)
                End If

                '解决多行字符串读不出来的问题
                If info = "System.String[]" Then

                    If use_debug_log = True Then
                        debug_log.WriteLine("TargetDir是multi_sz格式，切换读取模式")
                    End If

                    '是multi_sz字符串读不出来
                    Dim ccc As String() = CType(b_full_screen.GetValue("TargetDir"), String())
                    info = ccc.GetValue(0).ToString()

                    If use_debug_log = True Then
                        debug_log.WriteLine("TargetDir：" & info)
                    End If

                End If

                If Mid(info, info.Length, 1) <> "\" Then
                    info = info & "\"
                End If

                If use_debug_log = True Then
                    debug_log.WriteLine("反斜杠处理完毕，当前路径：" & info)
                End If

                ballance_start_path = info
                ui_form_start_ballance_path.Text = ballance_start_path

                If use_debug_log = True Then
                    debug_log.WriteLine("已写入ballance_start_path和程序开始页面文本框")
                End If

            End If


            '语言
            info = b_full_screen.GetValue("Language").ToString
            Select Case info
                Case "1"
                    ui_form_setting_form_1_language.Text = "当前语言状态：德语（Deutsch）"
                Case "2"
                    ui_form_setting_form_1_language.Text = "当前语言状态：英语（Endlish）"
                Case "3"
                    ui_form_setting_form_1_language.Text = "当前语言状态：西班牙语（El español）"
                Case "4"
                    ui_form_setting_form_1_language.Text = "当前语言状态：意大利语（In Italiano）"
                Case "5"
                    ui_form_setting_form_1_language.Text = "当前语言状态：法语（Français）"
                Case Else
                    ui_form_setting_form_1_language.Text = "当前语言状态：未知（Unknow）"
            End Select

            If use_debug_log = True Then
                debug_log.WriteLine("检测语言：" & info & "当前语言：")
            End If

            '分辨率
            Dim info_2 As Long = CType(b_full_screen.GetValue("VideoMode").ToString(), Long)
            info = info_2.ToString("X8")
            Dim win_width As Integer = Integer.Parse(Mid(info, 1, 4), System.Globalization.NumberStyles.HexNumber)
            Dim win_height As Integer = Integer.Parse(Mid(info, 5, 4), System.Globalization.NumberStyles.HexNumber)
            ui_form_setting_form_1_size_width.Text = win_width
            ui_form_setting_form_1_size_height.Text = win_height

            If use_debug_log = True Then
                debug_log.WriteLine("检测分辨率：" & win_width.ToString & " x " & win_height.ToString)
            End If

        Catch ex As Exception

            '错误意味着没有注册表项，默认一切，写注册表

            If use_debug_log = True Then
                debug_log.WriteLine("发生错误，正在重写注册表")
            End If

            ui_form_setting_form_1_fullscreen.Text = "当前全屏状态：全屏"
            ui_form_setting_form_1_fullscreen_btn.Content = "修改为 窗口化"
            ui_form_setting_form_1_language.Text = "当前语言状态：英语（English）"
            '由于游戏分辨率不影响开游戏，所以这里由于重写注册表无需写，这里就显示800x600
            ui_form_setting_form_1_size_width.Text = "800"
            ui_form_setting_form_1_size_height.Text = "600"

            Dim b_full_screen_2 As RegistryKey
            Dim b_full_screen_3 As RegistryKey

            If Environment.Is64BitOperatingSystem = True Then
                '64位系统
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\Wow6432Node\ballance")
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\Wow6432Node\ballance\Settings")

                If use_debug_log = True Then
                    debug_log.WriteLine("已创建Ballance相关键层次文件夹")
                End If

                b_full_screen_3 = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)
                b_full_screen_3.SetValue("Fullscreen", "1", RegistryValueKind.DWord)

                If use_debug_log = True Then
                    debug_log.WriteLine("已写入Fullscreen")
                End If

                '地址恢复
                If ballance_start_path <> "" Then
                    b_full_screen_3.SetValue("TargetDir", Mid(ballance_start_path, 1, ballance_start_path.Length - 1), RegistryValueKind.String)

                    If use_debug_log = True Then
                        debug_log.WriteLine("已重写TargetDir")
                    End If
                Else
                    '没有读到，在待会实现

                    If use_debug_log = True Then
                        debug_log.WriteLine("没有找到TargetDir，所以待会再重写")
                    End If
                End If


                b_full_screen_3.SetValue("Language", "1", RegistryValueKind.DWord)

                If use_debug_log = True Then
                    debug_log.WriteLine("已重写Language")
                End If

                '恢复启动SetupCommand
                b_full_screen_3.SetValue("SetupCommand", "", RegistryValueKind.String)

                If use_debug_log = True Then
                    debug_log.WriteLine("已重写SetupCommand")
                End If

            Else
                '32位
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\ballance")
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\ballance\Settings")

                If use_debug_log = True Then
                    debug_log.WriteLine("已创建Ballance相关键层次文件夹")
                End If

                b_full_screen_3 = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)
                b_full_screen_3.SetValue("Fullscreen", "1", RegistryValueKind.DWord)

                If use_debug_log = True Then
                    debug_log.WriteLine("已写入Fullscreen")
                End If

                '地址恢复
                If ballance_start_path <> "" Then
                    b_full_screen_3.SetValue("TargetDir", Mid(ballance_start_path, 1, ballance_start_path.Length - 1), RegistryValueKind.String)

                    If use_debug_log = True Then
                        debug_log.WriteLine("已重写TargetDir")
                    End If
                Else
                    '没有读到，在待会实现

                    If use_debug_log = True Then
                        debug_log.WriteLine("没有找到TargetDir，所以待会再重写")
                    End If
                End If


                b_full_screen_3.SetValue("Language", "1", RegistryValueKind.DWord)

                If use_debug_log = True Then
                    debug_log.WriteLine("已重写Language")
                End If

                '恢复启动SetupCommand
                b_full_screen_3.SetValue("SetupCommand", "", RegistryValueKind.String)

                If use_debug_log = True Then
                    debug_log.WriteLine("已重写SetupCommand")
                End If

            End If

        End Try


        open_ballance_dir.Filter = "Ballance主启动程序|Startup.exe"
        open_ballance_dir.Title = "选择你安装的Ballance的主启动程序Startup.exe"

        If use_debug_log = True Then
            debug_log.WriteLine("已写入打开Ballance主程序文打开框相关数据")
        End If

        If ballance_start_path = "" Or System.IO.File.Exists(ballance_start_path & "Startup.exe") = False Then
            '没有读到地址
            MsgBox("没有找到Ballance的安装位置，请选择你安装的Ballance的主启动程序Startup.exe（位于 Ballance安装目录 下）", MsgBoxStyle.Exclamation, "Ballance工具箱")
            If use_debug_log = True Then
                debug_log.WriteLine("没有读到启动地址，已要求用户强制输入")
            End If

            open_ballance_dir.ShowDialog()

            If open_ballance_dir.FileName <> "" Then

                Dim info_2 As String = ""
                info_2 = open_ballance_dir.FileName
                info_2 = Mid(info_2, 1, info_2.Length - 11)

                If Mid(info_2, info_2.Length, 1) <> "\" Then
                    info_2 = info_2 & "\"
                End If
                ballance_start_path = info_2
                ui_form_start_ballance_path.Text = ballance_start_path

                If use_debug_log = True Then
                    debug_log.WriteLine("已获得地址：" & info)
                End If
            Else
                If use_debug_log = True Then
                    debug_log.WriteLine("用户未输入主程序地址，强制退出")
                End If
                MsgBox("您必须选择Ballance主启动程序Startup.exe，否则这将无法开启工具箱，现在程序即将退出！", 16, "Ballance工具箱")
                Environment.Exit(1)
            End If

            If Environment.Is64BitOperatingSystem = True Then
                '64位系统
                b_full_screen = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)
            Else
                '32位
                b_full_screen = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)
            End If

            b_full_screen.SetValue("TargetDir", Mid(ballance_start_path, 1, ballance_start_path.Length - 1), RegistryValueKind.String)
            If use_debug_log = True Then
                debug_log.WriteLine("已重写注册表TargetDir")
            End If

        End If

        Key.Dispose()

    End Sub

    ''' <summary>
    ''' 初始化用户控件阶段
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_user_contorl(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)


        '对各个列表的模板进行设置************************************************************************************************
        If (Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor >= 2) Or Environment.OSVersion.Version.Major > 6 Then
            'windows8以上
            ui_form_level_form_check_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_level_form_check_list"), DataTemplate)
            ui_form_level_form_level_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_level_form_level_list"), DataTemplate)
            ui_form_level_form_hero_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_level_form_hero_list"), DataTemplate)
            ui_form_setting_form_local_hero_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_setting_form_local_hero_list"), DataTemplate)
            ui_form_photo_form_background_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_photo_form_background_list"), DataTemplate)
            ui_form_photo_form_material_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_photo_form_material_list"), DataTemplate)
            ui_form_wave_form_wave_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_wave_form_wave_list"), DataTemplate)
            ui_form_wave_form_bgm_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_wave_form_bgm_list"), DataTemplate)
            ui_form_nmo_form_mod_mod_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_nmo_form_mod_mod_list"), DataTemplate)
            ui_form_nmo_form_mod_ph_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_nmo_form_mod_ph_list"), DataTemplate)
            ui_form_setting_form_local_hero_backups_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_setting_form_local_hero_backups_list"), DataTemplate)
            ui_form_web_form_people_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows10_form_web_form_people_list"), DataTemplate)

            If use_debug_log = True Then
                debug_log.WriteLine("检测到windows8(包括)以上os")
            End If

        Else
            'windows8以下
            ui_form_level_form_check_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_level_form_check_list"), DataTemplate)
            ui_form_level_form_level_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_level_form_level_list"), DataTemplate)
            ui_form_level_form_hero_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_level_form_hero_list"), DataTemplate)
            ui_form_setting_form_local_hero_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_setting_form_local_hero_list"), DataTemplate)
            ui_form_photo_form_background_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_photo_form_background_list"), DataTemplate)
            ui_form_photo_form_material_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_photo_form_material_list"), DataTemplate)
            ui_form_wave_form_wave_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_wave_form_wave_list"), DataTemplate)
            ui_form_wave_form_bgm_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_wave_form_bgm_list"), DataTemplate)
            ui_form_nmo_form_mod_mod_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_nmo_form_mod_mod_list"), DataTemplate)
            ui_form_nmo_form_mod_ph_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_nmo_form_mod_ph_list"), DataTemplate)
            ui_form_setting_form_local_hero_backups_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_setting_form_local_hero_backups_list"), DataTemplate)
            ui_form_web_form_people_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_windows7_form_web_form_people_list"), DataTemplate)

            If use_debug_log = True Then
                debug_log.WriteLine("检测到windows8(不包括)以下os")
            End If

        End If

        If use_debug_log = True Then
            debug_log.WriteLine("列表模板应用完毕")
        End If

        '设置列表集合
        ui_form_level_form_check_list.ItemsSource = ui_connect_form_level_form_check_list
        ui_form_level_form_level_list.ItemsSource = ui_connect_form_level_form_level_list
        ui_form_level_form_hero_list.ItemsSource = Nothing
        ui_form_setting_form_local_hero_list.ItemsSource = Nothing
        ui_form_photo_form_background_list.ItemsSource = ui_connect_form_photo_form_background_list
        ui_form_photo_form_material_list.ItemsSource = ui_connect_form_photo_form_material_list
        ui_form_wave_form_wave_list.ItemsSource = ui_connect_form_wave_form_wave_list
        ui_form_wave_form_bgm_list.ItemsSource = ui_connect_form_wave_form_bgm_list
        ui_form_nmo_form_mod_mod_list.ItemsSource = ui_connect_form_nmo_form_mod_mod_list
        ui_form_nmo_form_mod_ph_list.ItemsSource = ui_connect_form_nmo_form_mod_ph_list
        ui_form_setting_form_local_hero_backups_list.ItemsSource = ui_connect_form_setting_form_local_hero_backups_list
        ui_form_web_form_people_list.ItemsSource = Nothing

        If use_debug_log = True Then
            debug_log.WriteLine("列表资源已建立连接")
        End If

        '设置打开文件框
        open_map_bag_file.Filter = "地图包文件（*.bmb）|*.bmb|ZIP打包的地图包文件（*.zip）|*.zip"
        open_map_file.Filter = "NMO文件（*.nmo）|*.nmo"
        open_background_file.Filter = "背景包文件（*.bbb）|*.bbb"
        open_material_file.Filter = "材质包文件（*.bab）|*.bab"
        open_wave_file.Filter = "音效包文件（*.bwb）|*.bwb"
        open_bgm_file.Filter = "BGM包文件（*.bgb）|*.bgb"
        open_mod_file.Filter = "Mod包文件（*.bob）|*.bob"
        open_ph_file.Filter = "模型包文件（*.bpb）|*.bpb"

        If use_debug_log = True Then
            debug_log.WriteLine("文件打开框已完成设置")
        End If

        '设置地图预览状态
        ui_form_level_form_level_show.Background = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))

        If use_debug_log = True Then
            debug_log.WriteLine("UI中地图预先预览模式已完成设置")
        End If

        '设置每个listbox的状态
        ui_update_list()

        If use_debug_log = True Then
            debug_log.WriteLine("UI中Listbox没有项的情况已完成设置")
        End If

    End Sub

    ''' <summary>
    ''' 初始化地图阶段
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_map(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)

        Dim folder As New System.IO.DirectoryInfo(Environment.CurrentDirectory & "\map\")
        Dim file_arr() As System.IO.FileInfo = folder.GetFiles()

        If use_debug_log = True Then
            debug_log.WriteLine("已获取" & Environment.CurrentDirectory & "\map\下所有地图文件，共" & file_arr.Count & "个")
        End If

        '读取已存在的下载的地图图片
        Dim file As New System.IO.StreamReader(Environment.CurrentDirectory & "\cache_map.dat", System.Text.Encoding.UTF8)
        Dim have_map_list As New ArrayList
        Dim word As String = ""
        Do
            word = file.ReadLine

            If word = "" Then
                Exit Do
            End If

            have_map_list.Add(word)
        Loop

        file.Dispose()

        If use_debug_log = True Then
            debug_log.WriteLine("已获取已存在的下载过的地图文件名，共：" & have_map_list.Count & "个")
        End If



        If use_debug_log = True Then
            debug_log.WriteLine("****************************获取sm在线地图列表")
        End If
        '*******************************************sm获取区*******************************************
        '获取地图列表
        Dim OnLineMap = New List(Of ScoreManager.Data.Map)

        If My.Computer.Network.IsAvailable = True Then

            If use_debug_log = True Then
                debug_log.WriteLine("网络有效，准备获取sm地图列表资源")
            End If

            Dim c As System.Net.WebClient = New System.Net.WebClient
            c.Headers.Add("Referer", "http://jxtoolbox.sinaapp.com/api/map_json.php")
            Dim reader = New System.IO.StreamReader(c.OpenRead("http://jxtoolbox.sinaapp.com/api/map_json.php"),
                                                System.Text.Encoding.UTF8)
            Dim text As String = reader.ReadToEnd

            OnLineMap = ScoreManager.IO.Deserialize(Of ScoreManager.Data.Map).JsonDeserializeListData(text)

            If use_debug_log = True Then
                debug_log.WriteLine("已从sm获取到地图列表资源")
            End If
        End If

        '**************************************************************************************

        '已下载地图文件的写入
        Dim file_wr As New System.IO.StreamWriter(Environment.CurrentDirectory & "\cache_map.dat", True, System.Text.Encoding.UTF8)
        If use_debug_log = True Then
            debug_log.WriteLine("准备开始下载地图图片")
        End If



        If use_debug_log = True Then
            debug_log.WriteLine("****************************对sm在线地图执行地图下载")
        End If
        '*******************************************sm下载区*******************************************

        For a = 0 To OnLineMap.Count - 1
            '是否是下载过的文件
            If use_debug_log = True Then
                debug_log.WriteLine("对 " & OnLineMap.Item(a).Title & " 执行下载图片")
            End If

            If have_map_list.Contains(OnLineMap.Item(a).Title) = False Then
                If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\map\" & OnLineMap.Item(a).Title & ".jpg") = False Then
                    '尝试下载
                    Try
                        My.Computer.Network.DownloadFile(OnLineMap.Item(a).ImageLink, Environment.CurrentDirectory & "\cache\map\" & OnLineMap.Item(a).Title & ".jpg")

                        '记录已经下载过了
                        file_wr.WriteLine(OnLineMap.Item(a).Title)

                        If use_debug_log = True Then
                            debug_log.WriteLine("记录下载过 " & OnLineMap.Item(a).Title)
                        End If

                    Catch ex As Exception
                        '发生错误
                        '不下载
                        file_wr.WriteLine(OnLineMap.Item(a).Title)
                        If use_debug_log = True Then
                            debug_log.WriteLine("下载时发生了错误不再下载")
                        End If
                    End Try

                Else
                    '记录已经下载过了
                    file_wr.WriteLine(OnLineMap.Item(a).Title)
                    If use_debug_log = True Then
                        debug_log.WriteLine("未下载的文件，但本地已有文件，不下载，添加到下载过列表")
                    End If
                End If
            Else
                If use_debug_log = True Then
                    debug_log.WriteLine("下载过的文件，不下载")
                End If
            End If

        Next

        '**************************************************************************************

        Dim aaa As New ui_depend_form_level_form_level_list



        If use_debug_log = True Then
            debug_log.WriteLine("****************************对本地的baidu地图图片下载")
        End If
        '*******************************************baidu下载区*******************************************
        If file_arr.Count <> 0 Then
            For a = 0 To file_arr.Count - 1

                aaa.pro_title = Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)

                If use_debug_log = True Then
                    debug_log.WriteLine("对 " & aaa.pro_title & "执行获取信息")
                End If

                '获得本地信息
                '临时容纳下载地址得变量
                Dim download_path As String = ""
                search_map_list_file(aaa.pro_title, aaa.pro_creator, aaa.pro_description, aaa.pro_difficulty, aaa.pro_stars, download_path)

                '是否下载过
                If have_map_list.Contains(aaa.pro_title) = False Then
                    '是否存在这个文件
                    If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\mod\" & aaa.pro_title & ".jpg") = False Then
                        'map是否提供了下载地址
                        If download_path <> "" Then
                            '是否可以下载
                            If My.Computer.Network.IsAvailable = True Then

                                '准备下载图片
                                If use_debug_log = True Then
                                    debug_log.WriteLine("准备下载 " & aaa.pro_title)
                                End If
                                '尝试下载
                                Try
                                    My.Computer.Network.DownloadFile(download_path, Environment.CurrentDirectory & "\cache\map\" & aaa.pro_title & ".jpg")
                                    If use_debug_log = True Then
                                        debug_log.WriteLine("已下载完成")
                                    End If
                                Catch ex As Exception
                                    '发生错误
                                    '不下载
                                    If use_debug_log = True Then
                                        debug_log.WriteLine("下载发生错误")
                                    End If

                                End Try

                                '记录下载过了
                                file_wr.WriteLine(aaa.pro_title)
                                If use_debug_log = True Then
                                    debug_log.WriteLine("已记录下载过该map图片")
                                End If

                            Else
                                '无法下载,没有网络
                                If use_debug_log = True Then
                                    debug_log.WriteLine("没记录下载过，没有网络无法下载")
                                End If

                            End If
                        Else
                            '无法下载,不记录下载过
                            If use_debug_log = True Then
                                debug_log.WriteLine("没记录下载过，但是有这个文件没有给定下载地址")
                            End If
                        End If

                    Else
                        '没记录下载过，但是有这个文件存在，记录下载过
                        file_wr.WriteLine(aaa.pro_title)
                        If use_debug_log = True Then
                            debug_log.WriteLine("没记录下载过，但是有这个文件存在，记录下载过该map图片")
                        End If
                    End If
                Else
                    '下载过了，不下载
                    If use_debug_log = True Then
                        debug_log.WriteLine("下载过了，不下载该map图片")
                    End If
                End If

            Next
        End If

        '**************************************************************************************
        file_wr.Dispose()



        If use_debug_log = True Then
            debug_log.WriteLine("****************************索引文件列表，添加md5")
        End If
        '*****************************************索引文件列表，添加md5*********************************************

        '读取已存在的map的md5数据
        Dim file3 As New System.IO.StreamReader(Environment.CurrentDirectory & "\map.dat", System.Text.Encoding.UTF8)
        Dim have_map_data_list As New ArrayList
        word = ""
        Do
            word = file3.ReadLine

            If word = "" Then
                Exit Do
            End If

            file3.ReadLine()
            have_map_data_list.Add(word)
        Loop

        file3.Dispose()


        '索引文件列表,添加md5
        If file_arr.Count <> 0 Then

            '地图文件数据检测数据库
            Dim file_wrt As New System.IO.StreamWriter(Environment.CurrentDirectory & "\map.dat", True, System.Text.Encoding.UTF8)

            For a = 0 To file_arr.Count - 1

                '不存在，写入
                If have_map_data_list.Contains(Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)) = False Then
                    '写入数据库
                    file_wrt.WriteLine(Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4))
                    file_wrt.WriteLine(get_file_md5(file_arr(a).FullName))

                    If use_debug_log = True Then
                        debug_log.WriteLine("写入文件md5数据至数据库map.dat")
                    End If
                Else
                    If use_debug_log = True Then
                        debug_log.WriteLine("地图已存在，不写入数据库map.dat")
                    End If
                End If

            Next
        End If

        '**************************************************************************************


        If use_debug_log = True Then
            debug_log.WriteLine("****************************添加地图至ui列表")
        End If
        '**************************************写入列表************************************************

        Dim bbb As New ui_depend_form_level_form_level_list
        '写入地图数据，加入列表
        If file_arr.Count <> 0 Then
            For a = 0 To file_arr.Count - 1

                bbb = New ui_depend_form_level_form_level_list

                '写入标题头
                bbb.pro_title = Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)

                If use_debug_log = True Then
                    debug_log.WriteLine("向地图列表添加项")
                End If

                Dim yes As Boolean = False

                '*****************************************检测sm里面是否有资源
                If OnLineMap.Count <> 0 Then
                    For b = 0 To OnLineMap.Count - 1
                        If OnLineMap.Item(b).Title = bbb.pro_title Then
                            '有相同的

                            bbb.pro_id = OnLineMap.Item(b).ID
                            bbb.pro_creator = "制作者：" & OnLineMap.Item(b).Creator
                            bbb.pro_description = "描述：" & OnLineMap.Item(b).Description
                            bbb.pro_difficulty = "难度：" & OnLineMap.Item(b).Difficulty
                            bbb.pro_downloadcount = "下载次数：" & OnLineMap.Item(b).DownloadCount
                            bbb.pro_playcount = "玩的次数：" & OnLineMap.Item(b).PlayCount
                            bbb.pro_stars = "星级："
                            If OnLineMap.Item(b).Stars <> 0 Then
                                For c = 0 To OnLineMap.Item(b).Stars - 1
                                    bbb.pro_stars = bbb.pro_stars & "★"
                                Next
                            End If

                            If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\map\" & bbb.pro_title & ".jpg") = True Then
                                bbb.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\map\" & bbb.pro_title & ".jpg")))
                            Else
                                '没有，默认背景
                                bbb.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))
                            End If

                            If use_debug_log = True Then
                                debug_log.WriteLine("使用sm资源")
                            End If

                            yes = True
                            Exit For

                        End If
                    Next
                End If

                '************************************是否需要使用本地资源
                If yes = False Then

                    '本地资源
                    Dim null As String = ""
                    search_map_list_file(bbb.pro_title, bbb.pro_creator, bbb.pro_description, bbb.pro_difficulty, bbb.pro_stars, null)
                    bbb.pro_id = "关卡ID：无"
                    bbb.pro_downloadcount = "下载次数：未知"
                    bbb.pro_playcount = "玩的次数：未知"

                    If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\map\" & bbb.pro_title & ".jpg") = True Then
                        bbb.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\map\" & bbb.pro_title & ".jpg")))
                    Else
                        '没有，默认背景
                        bbb.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))
                    End If

                    If use_debug_log = True Then
                        debug_log.WriteLine("使用本地资源")
                    End If
                End If

                '收尾，强制使用本地能使用的资源
                Dim null_1 As String = ""
                search_map_list_file(bbb.pro_title, bbb.pro_creator, bbb.pro_description, bbb.pro_difficulty, bbb.pro_stars, null_1, False)

                If use_debug_log = True Then
                    debug_log.WriteLine("已强制使用部分本地资源")
                End If

                '加入列表

                ui_connect_form_level_form_level_list.Add(bbb)

                If use_debug_log = True Then
                    debug_log.WriteLine("已将：" & file_arr(a).Name & "加入地图列表")
                End If

            Next
        End If

        '**************************************************************************************

    End Sub

    ''' <summary>
    ''' 初始化备份表
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_backups(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)
        Dim folder As New System.IO.DirectoryInfo(Environment.CurrentDirectory & "\backups\")
        Dim file_arr() As System.IO.FileInfo = folder.GetFiles()

        If use_debug_log = True Then
            debug_log.WriteLine("获取备份文件列表成功，共" & file_arr.Count & "个文件")
        End If

        Dim aaa As New ui_depend_form_setting_form_local_hero_backups_list
        ui_connect_form_setting_form_local_hero_backups_list.Clear()

        If file_arr.Count <> 0 Then
            For a = 0 To file_arr.Count - 1
                If use_debug_log = True Then
                    debug_log.WriteLine("已添加至列表：" & file_arr(a).Name)
                End If

                aaa.pro_title = Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)
                ui_connect_form_setting_form_local_hero_backups_list.Add(aaa)
                aaa = New ui_depend_form_setting_form_local_hero_backups_list
            Next
        End If

    End Sub

    ''' <summary>
    ''' 初始化背景函数
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_background(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)
        Dim bk As New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_A_Front.BMP")))
        If use_debug_log = True Then
            debug_log.WriteLine("Front正面背景完成")
        End If
        ui_form_photo_form_background_bk_up.Background = bk
        bk = New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_A_Back.BMP")))
        If use_debug_log = True Then
            debug_log.WriteLine("Back背面背景完成")
        End If
        ui_form_photo_form_background_bk_down.Background = bk
        bk = New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_A_Left.BMP")))
        If use_debug_log = True Then
            debug_log.WriteLine("Left左面背景完成")
        End If
        ui_form_photo_form_background_bk_left.Background = bk
        bk = New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_A_Right.BMP")))
        If use_debug_log = True Then
            debug_log.WriteLine("Right右面背景完成")
        End If
        ui_form_photo_form_background_bk_right.Background = bk
        bk = New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_A_Down.BMP")))
        If use_debug_log = True Then
            debug_log.WriteLine("Down下面背景完成")
        End If
        ui_form_photo_form_background_bk_middle.Background = bk

    End Sub

    ''' <summary>
    ''' 处理背景和bgm的更新
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_progress_background(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)
        'a-l

        Dim bk_list As String = "A"
        For a = 0 To 11
            Select Case a
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
                    Exit For
            End Select

            If use_debug_log = True Then
                debug_log.WriteLine("正在处理背景号：" & bk_list)
            End If

            If System.IO.File.Exists(ballance_start_path & "Textures\Sky\Sky_" & bk_list & "_Down.old") = True Then
                '该背景需要替换

                If use_debug_log = True Then
                    debug_log.WriteLine("需要处理")
                End If

                System.IO.File.Delete(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Back.bmp")
                System.IO.File.Delete(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Front.bmp")
                System.IO.File.Delete(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Left.bmp")
                System.IO.File.Delete(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Right.bmp")
                System.IO.File.Delete(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Down.bmp")

                If use_debug_log = True Then
                    debug_log.WriteLine("已删除源文件")
                End If

                System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Back.old", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Back.bmp")
                System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Front.old", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Front.bmp")
                System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Left.old", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Left.bmp")
                System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Right.old", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Right.bmp")
                System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Down.old", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Down.bmp")

                If use_debug_log = True Then
                    debug_log.WriteLine("已将old文件替换")
                End If

            End If

            If use_debug_log = True Then
                debug_log.WriteLine("背景完毕")
            End If

        Next


        'bgm更新
        If System.IO.File.Exists(ballance_start_path & "Sounds\Music_Theme_1_1.new") = True Then
            '需要替换
            If use_debug_log = True Then
                debug_log.WriteLine("需要处理")
            End If

            '改名
            For d = 1 To 3
                For f = 1 To 3
                    System.IO.File.Delete(ballance_start_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".wav")
                    System.IO.File.Move(ballance_start_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".new", ballance_start_path & "Sounds\Music_Theme_" & d.ToString & "_" & f.ToString & ".wav")
                Next
            Next

            If use_debug_log = True Then
                debug_log.WriteLine("bgm完毕")
            End If
        End If





    End Sub

    ''' <summary>
    ''' 初始化背景包
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_background_bag(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)
        Dim folder As New System.IO.DirectoryInfo(Environment.CurrentDirectory & "\background\")
        Dim file_arr() As System.IO.FileInfo = folder.GetFiles()
        Dim aaa As New ui_depend_form_photo_form_background_list

        If use_debug_log = True Then
            debug_log.WriteLine("已获取到文件：共" & file_arr.Count & "个")
        End If

        If file_arr.Count <> 0 Then
            For a = 0 To file_arr.Count - 1
                aaa.pro_title = Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)
                'TODO:未来：可以预览地图包的背景样式，现在先用无地图的样式
                aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))

                If use_debug_log = True Then
                    debug_log.WriteLine("已将：" & file_arr(a).Name & "添加到列表")
                End If

                ui_connect_form_photo_form_background_list.Add(aaa)
                aaa = New ui_depend_form_photo_form_background_list
            Next
        End If

    End Sub

    ''' <summary>
    ''' 初始化材质包
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_material(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)
        Dim folder As New System.IO.DirectoryInfo(Environment.CurrentDirectory & "\material\")
        Dim file_arr() As System.IO.FileInfo = folder.GetFiles()
        Dim aaa As New ui_depend_form_photo_form_material_list

        If use_debug_log = True Then
            debug_log.WriteLine("已获取到文件：共" & file_arr.Count & "个")
        End If

        If file_arr.Count <> 0 Then
            For a = 0 To file_arr.Count - 1
                aaa.pro_title = Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)
                'TODO:未来：可以预览材质包的背景样式，现在先用无地图的样式
                aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))

                If use_debug_log = True Then
                    debug_log.WriteLine("已将：" & file_arr(a).Name & "添加到列表")
                End If

                ui_connect_form_photo_form_material_list.Add(aaa)
                aaa = New ui_depend_form_photo_form_material_list
            Next
        End If

    End Sub

    ''' <summary>
    ''' 初始化音效包
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_wave(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)
        Dim folder As New System.IO.DirectoryInfo(Environment.CurrentDirectory & "\wave\")
        Dim file_arr() As System.IO.FileInfo = folder.GetFiles()
        Dim aaa As New ui_depend_form_wave_form_wave_list

        If use_debug_log = True Then
            debug_log.WriteLine("已获取到文件：共" & file_arr.Count & "个")
        End If

        If file_arr.Count <> 0 Then
            For a = 0 To file_arr.Count - 1
                aaa.pro_title = Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)
                'TODO:未来：可以展示音效包的有关主题图片，现在先用无地图的样式
                aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))

                If use_debug_log = True Then
                    debug_log.WriteLine("已将：" & file_arr(a).Name & "添加到列表")
                End If

                ui_connect_form_wave_form_wave_list.Add(aaa)
                aaa = New ui_depend_form_wave_form_wave_list
            Next
        End If
    End Sub

    ''' <summary>
    ''' 初始化bgm包
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_bgm(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)
        Dim folder As New System.IO.DirectoryInfo(Environment.CurrentDirectory & "\bgm\")
        Dim file_arr() As System.IO.FileInfo = folder.GetFiles()
        Dim aaa As New ui_depend_form_wave_form_bgm_list

        If use_debug_log = True Then
            debug_log.WriteLine("已获取到文件：共" & file_arr.Count & "个")
        End If

        If file_arr.Count <> 0 Then
            For a = 0 To file_arr.Count - 1
                aaa.pro_title = Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)
                'TODO:未来：可以展示bgm包的有关主题图片，现在先用无地图的样式
                aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))

                If use_debug_log = True Then
                    debug_log.WriteLine("已将：" & file_arr(a).Name & "添加到列表")
                End If

                ui_connect_form_wave_form_bgm_list.Add(aaa)
                aaa = New ui_depend_form_wave_form_bgm_list
            Next
        End If
    End Sub

    ''' <summary>
    ''' 初始化mod列表 检测是否安装bml
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_mod(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)

        If System.IO.File.Exists(ballance_start_path & "ModLoader\ModLoader.nmo") = True Then
            '有bml

            Dim folder As New System.IO.DirectoryInfo(ballance_start_path & "ModLoader\Mods\")
            Dim file_arr() As System.IO.FileInfo = folder.GetFiles()
            Dim aaa As New ui_depend_form_nmo_form_mod_mod_list

            If use_debug_log = True Then
                debug_log.WriteLine("已获取到文件：共" & file_arr.Count & "个")
            End If

            '读取已存在的下载的mod图片
            Dim file As New System.IO.StreamReader(Environment.CurrentDirectory & "\cache_mod.dat", System.Text.Encoding.UTF8)
            Dim have_mod_list As New ArrayList
            Dim word As String = ""
            Do
                word = file.ReadLine

                If word = "" Then
                    Exit Do
                End If

                have_mod_list.Add(word)
            Loop
            file.Dispose()

            If use_debug_log = True Then
                debug_log.WriteLine("已读取下载过的mod图片")
            End If

            '加入列表

            '地图文件数据检测数据库
            Dim file_wr As New System.IO.StreamWriter(Environment.CurrentDirectory & "\cache_mod.dat", True, System.Text.Encoding.UTF8)

            If file_arr.Count <> 0 Then
                For a = 0 To file_arr.Count - 1
                    aaa.pro_title = Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)

                    If use_debug_log = True Then
                        debug_log.WriteLine("对 " & aaa.pro_title & "执行获取信息")
                    End If

                    '获得本地信息
                    search_mod_list_file(get_file_md5(file_arr(a).FullName), aaa.pro_name, aaa.pro_pid, aaa.pro_creator, aaa.pro_describe, aaa.pro_download_photo)

                    '是否下载过
                    If have_mod_list.Contains(aaa.pro_title) = False Then
                        '是否存在这个文件
                        If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\mod\" & aaa.pro_title & ".jpg") = False Then
                            'mod是否提供了下载地址
                            If aaa.pro_download_photo <> "" And Mid(aaa.pro_name, 4) <> "未知" Then
                                '是否可以下载
                                If My.Computer.Network.IsAvailable = True Then

                                    '准备下载图片
                                    If use_debug_log = True Then
                                        debug_log.WriteLine("准备下载 " & aaa.pro_title)
                                    End If
                                    '尝试下载
                                    Try
                                        My.Computer.Network.DownloadFile(aaa.pro_download_photo, Environment.CurrentDirectory & "\cache\mod\" & aaa.pro_title & ".jpg")
                                        If use_debug_log = True Then
                                            debug_log.WriteLine("已下载完成")
                                        End If
                                    Catch ex As Exception
                                        '发生错误
                                        '不下载
                                        If use_debug_log = True Then
                                            debug_log.WriteLine("下载发生错误")
                                        End If

                                    End Try

                                    '记录下载过了
                                    file_wr.WriteLine(aaa.pro_title)
                                    If use_debug_log = True Then
                                        debug_log.WriteLine("已记录下载过该mod图片")
                                    End If

                                Else
                                    '无法下载,没有网络
                                    If use_debug_log = True Then
                                        debug_log.WriteLine("没记录下载过，没有网络无法下载")
                                    End If

                                End If
                            Else
                                '无法下载,不记录下载过
                                If use_debug_log = True Then
                                    debug_log.WriteLine("没记录下载过，但是有这个文件没有给定下载地址")
                                End If
                            End If

                        Else
                            '没记录下载过，但是有这个文件存在，记录下载过
                            file_wr.WriteLine(aaa.pro_title)
                            If use_debug_log = True Then
                                debug_log.WriteLine("没记录下载过，但是有这个文件存在，记录下载过该mod图片")
                            End If
                        End If
                    Else
                        '下载过了，不下载
                        If use_debug_log = True Then
                            debug_log.WriteLine("下载过了，不下载该mod图片")
                        End If
                    End If

                    '***************************

                    If use_debug_log = True Then
                        debug_log.WriteLine("添加就绪")
                    End If

                    '添加描述图片
                    If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\mod\" & aaa.pro_title & ".jpg") = True Then
                        aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\mod\" & aaa.pro_title & ".jpg")))
                    Else
                        aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))
                    End If

                    If use_debug_log = True Then
                        debug_log.WriteLine("已完成图片输入")
                    End If

                    If use_debug_log = True Then
                        debug_log.WriteLine("已将：" & file_arr(a).Name & "添加到列表")
                    End If

                    ui_connect_form_nmo_form_mod_mod_list.Add(aaa)
                    aaa = New ui_depend_form_nmo_form_mod_mod_list
                Next
            End If

            file_wr.Dispose()

            '设置bml
            ui_form_nmo_form_mod_bml_describe.Text = "当前Ballance Mod Loader安装状态：已安装"
            BML_have = True

            If use_debug_log = True Then
                debug_log.WriteLine("已设置bml")
            End If
        Else
            '没有bml
            If use_debug_log = True Then
                debug_log.WriteLine("未找到bml，已忽略")
            End If
            ui_form_nmo_form_mod_bml_describe.Text = "当前Ballance Mod Loader安装状态：没有安装"
            BML_have = False
        End If


        '****************************************************************
        '附属：以后视bml的完善而删除
        '检测无限命和倍速球的函数
        Select Case get_file_md5(ballance_start_path & "3D Entities\Gameplay.nmo")
            Case "5ecfaa557bc5419b9c72d0f6056f3b87"
                ui_form_nmo_form_normal_life_state.Text = "当前无限命的状态是：关闭"
            Case "dc5762829c65ec17af7f384eb6a7c2b1"
                ui_form_nmo_form_normal_life_state.Text = "当前无限命的状态是：打开"
            Case Else
                ui_form_nmo_form_normal_life_state.Text = "当前无限命的状态是：未知"
        End Select

        Select Case get_file_md5(ballance_start_path & "3D Entities\Balls.nmo")
            Case "5faa4a0532ba8f006270f4c00957b7a1"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：1.75倍速"
            Case "722d41c070887e14683c22380e2d7636"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：2倍速"
            Case "5edacfbd8da603f59829163bac856db2"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：3倍速"
            Case "37ead9ff6774f1a2f4ef8ce4e4838fc6"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：4倍速"
            Case "ddb0d1bf0408f7f109465655cc8ba0d6"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：5倍速"
            Case "7c034cfce3c726bedbd06142b16cfaab"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：6倍速"
            Case "e62bae5b1ba3af725efde61b00f6df42"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：7倍速"
            Case "b8448d778ceb6c88fee23029c41bfd2a"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：8倍速"
            Case "8db3efa487d5591aef341a1b6b2d22a2"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：9倍速"
            Case "3c655cd499d1f5cdfa09ebdefc3ce564"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：10倍速"
            Case "05eddb9f514b413f0887b859380c4d1c"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：三高球"
            Case "fb29d77e63aad08499ce38d36266ec33"
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：正常速度"
            Case Else
                ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：未知"
        End Select

        If use_debug_log = True Then
            debug_log.WriteLine("已检查球和生命的模式")
        End If


    End Sub

    ''' <summary>
    ''' 初始化模组列表
    ''' </summary>
    ''' <param name="debug_log">要使用的debug文件写入变量</param>
    ''' <param name="use_debug_log">是否使用debug_log</param>
    Public Sub app_init_ph(ByRef debug_log As System.IO.StreamWriter, ByVal use_debug_log As Boolean)
        Dim folder As New System.IO.DirectoryInfo(ballance_start_path & "3D Entities\PH\")
        Dim file_arr() As System.IO.FileInfo = folder.GetFiles()
        Dim aaa As New ui_depend_form_nmo_form_mod_ph_list

        If use_debug_log = True Then
            debug_log.WriteLine("已获取到文件：共" & file_arr.Count & "个")
        End If


        '读取已存在的下载的mod图片
        Dim file As New System.IO.StreamReader(Environment.CurrentDirectory & "\cache_ph.dat", System.Text.Encoding.UTF8)
        Dim have_ph_list As New ArrayList
        Dim word As String = ""
        Do
            word = file.ReadLine

            If word = "" Then
                Exit Do
            End If

            have_ph_list.Add(word)
        Loop
        file.Dispose()

        If use_debug_log = True Then
            debug_log.WriteLine("已读取下载过的ph图片")
        End If

        '加入列表

        '地图文件数据检测数据库
        Dim file_wr As New System.IO.StreamWriter(Environment.CurrentDirectory & "\cache_ph.dat", True, System.Text.Encoding.UTF8)

        If file_arr.Count <> 0 Then
            For a = 0 To file_arr.Count - 1
                If file_arr(a).Name <> "_marker_" Then
                    '排除标记文件

                    aaa.pro_title = Mid(file_arr(a).Name, 1, file_arr(a).Name.Length - 4)

                    If use_debug_log = True Then
                        debug_log.WriteLine("对 " & aaa.pro_title & " 执行获取信息")
                    End If

                    '获得本地信息
                    search_ph_list_file(get_file_md5(file_arr(a).FullName), aaa.pro_name, aaa.pro_pid, aaa.pro_creator, aaa.pro_describe, aaa.pro_download_photo)

                    '是否下载过
                    If have_ph_list.Contains(aaa.pro_title) = False Then
                        '是否存在这个文件
                        If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\ph\" & aaa.pro_title & ".jpg") = False Then
                            'ph是否提供了下载地址
                            If aaa.pro_download_photo <> "" And Mid(aaa.pro_name, 4) <> "未知" Then
                                '是否可以下载
                                If My.Computer.Network.IsAvailable = True Then

                                    '准备下载图片
                                    If use_debug_log = True Then
                                        debug_log.WriteLine("准备下载 " & aaa.pro_title)
                                    End If
                                    '尝试下载
                                    Try
                                        My.Computer.Network.DownloadFile(aaa.pro_download_photo, Environment.CurrentDirectory & "\cache\ph\" & aaa.pro_title & ".jpg")
                                        If use_debug_log = True Then
                                            debug_log.WriteLine("已下载完成")
                                        End If
                                    Catch ex As Exception
                                        '发生错误
                                        '不下载
                                        If use_debug_log = True Then
                                            debug_log.WriteLine("下载发生错误")
                                        End If

                                    End Try

                                    '记录下载过了
                                    file_wr.WriteLine(aaa.pro_title)
                                    If use_debug_log = True Then
                                        debug_log.WriteLine("已记录下载过该ph图片")
                                    End If

                                Else
                                    '无法下载,没有网络
                                    If use_debug_log = True Then
                                        debug_log.WriteLine("没记录下载过，没有网络无法下载")
                                    End If

                                End If
                            Else
                                '无法下载,不记录下载过
                                If use_debug_log = True Then
                                    debug_log.WriteLine("没记录下载过，但是有这个文件没有给定下载地址")
                                End If
                            End If

                        Else
                            '没记录下载过，但是有这个文件存在，记录下载过
                            file_wr.WriteLine(aaa.pro_title)
                            If use_debug_log = True Then
                                debug_log.WriteLine("没记录下载过，但是有这个文件存在，记录下载过该ph图片")
                            End If
                        End If
                    Else
                        '下载过了，不下载
                        If use_debug_log = True Then
                            debug_log.WriteLine("下载过了，不下载该ph图片")
                        End If
                    End If

                    '***************************

                    If use_debug_log = True Then
                        debug_log.WriteLine("添加就绪")
                    End If

                    '添加描述图片
                    If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\ph\" & aaa.pro_title & ".jpg") = True Then
                        aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\ph\" & aaa.pro_title & ".jpg")))
                    Else
                        aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))
                    End If

                    If use_debug_log = True Then
                        debug_log.WriteLine("已添加描述图片")
                    End If

                    If use_debug_log = True Then
                        debug_log.WriteLine("已将：" & file_arr(a).Name & "添加到列表")
                    End If

                    ui_connect_form_nmo_form_mod_ph_list.Add(aaa)
                    aaa = New ui_depend_form_nmo_form_mod_ph_list
                End If
            Next
        End If

        file_wr.Dispose()

    End Sub



    ''' <summary>
    ''' 检测是否需要补充目录的函数
    ''' </summary>
    Public Sub app_init_set_folder()

        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\wave") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\wave")
        End If
        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\material") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\material")
        End If
        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\map") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\map")
        End If
        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\bgm") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\bgm")
        End If
        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\backups") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\backups")
        End If
        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\background") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\background")
        End If


        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\cache\map") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\cache\map")
        End If
        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\cache\mod") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\cache\mod")
        End If
        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\cache\ph") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\cache\ph")
        End If
        If System.IO.Directory.Exists(Environment.CurrentDirectory & "\cache\user") = False Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\cache\user")
        End If

    End Sub

#End Region

#Region "主页"
    '开始游戏
    Private Sub ui_form_start_play_btn_click(sender As Object, e As RoutedEventArgs) Handles ui_form_start_play_btn.Click

        '**********************************************************************启动前步骤1-修改videodriver键值
        '修改注册表
        Dim Key As RegistryKey = Registry.LocalMachine
        Dim select_key As RegistryKey

        Try
            '判断
            If Environment.Is64BitOperatingSystem = True Then
                '64位系统
                select_key = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)
            Else
                '32位
                select_key = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)
            End If

            '写入
            select_key.SetValue("VideoDriver", "0", RegistryValueKind.DWord)

        Catch ex As Exception
            '失败，就不写入了

        End Try

        MsgBox("Ballance即将启动，工具箱将关闭。", 64, "Ballance工具箱")
        Process.Start(ballance_start_path + "Startup.exe")
        Environment.Exit(2)
    End Sub

    '搜索文件
    Private Sub ui_form_start_search_file(sender As Object, e As MouseButtonEventArgs)
        MsgBox("请选择你安装的Ballance的主启动程序Startup.exe（位于 Ballance安装目录 下）", MsgBoxStyle.Exclamation, "Ballance工具箱")

        open_ballance_dir.ShowDialog()
        Dim reg_word As String = ""

        If open_ballance_dir.FileName <> "" Then

            Dim info_2 As String = ""
            info_2 = open_ballance_dir.FileName
            info_2 = Mid(info_2, 1, info_2.Length - 11)

            reg_word = info_2

            If Mid(info_2, info_2.Length, 1) <> "\" Then
                info_2 = info_2 & "\"
            End If
            ballance_start_path = info_2
            ui_form_start_ballance_path.Text = ballance_start_path
        Else
            Exit Sub
        End If

        '重写注册表
        Dim Key As RegistryKey = Registry.LocalMachine
        Dim b_full_screen As RegistryKey
        If Environment.Is64BitOperatingSystem = True Then
            '64位系统
            b_full_screen = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)
            b_full_screen.DeleteValue("TargetDir")
            b_full_screen.SetValue("TargetDir", reg_word, RegistryValueKind.String)

        Else
            '32位
            b_full_screen = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)
            b_full_screen.DeleteValue("TargetDir")
            b_full_screen.SetValue("TargetDir", reg_word, RegistryValueKind.String)

        End If


    End Sub


    '新手
    Private Sub ui_form_start_new_player(sender As Object, e As MouseButtonEventArgs)

        '显示窗口
        Dim linshi = New Window_new_player
        linshi.Owner = Me
        linshi.ShowDialog()

    End Sub
#End Region

#Region "关卡"

    '地图检测************************************************
    '立即检测
    Private Sub ui_form_level_form_check_checking_btn(sender As Object, e As RoutedEventArgs)

        '显示对话框

        Dim linshi = New Window_wait
        linshi.Owner = Me
        linshi.Show()

        Dim level_file As New System.IO.FileInfo(ballance_start_path & "3D entities\level\level_01.nmo")
        Dim item As New ui_depend_form_level_form_check_list

        ui_form_level_form_check_list.ItemsSource = Nothing
        ui_connect_form_level_form_check_list.Clear()

        '原版关卡
        For a = 1 To 13
            If a >= 10 Then
                level_file = New System.IO.FileInfo(ballance_start_path & "3D entities\level\level_" & a & ".nmo")
                item.pro_level_path = ballance_start_path & "3D entities\level\level_" & a & ".nmo"
            Else
                level_file = New System.IO.FileInfo(ballance_start_path & "3D entities\level\level_0" & a & ".nmo")
                item.pro_level_path = ballance_start_path & "3D entities\level\level_0" & a & ".nmo"
            End If

            item.pro_level_name = "Level " & a & "（第" & a & "关）"
            item.pro_map_name = check_map_name(get_file_md5(level_file.FullName))
            ui_connect_form_level_form_check_list.Add(item)
            item = New ui_depend_form_level_form_check_list
        Next

        'mod关卡
        If System.IO.File.Exists(ballance_start_path & "3D entities\level\level_14.nmo") = True Then
            level_file = New System.IO.FileInfo(ballance_start_path & "3D entities\level\level_14.nmo")
            item.pro_level_path = ballance_start_path & "3D entities\level\level_14.nmo"
            item.pro_level_name = "Level 14（第14关）"
            item.pro_map_name = check_map_name(get_file_md5(level_file.FullName))
            ui_connect_form_level_form_check_list.Add(item)
            item = New ui_depend_form_level_form_check_list
        End If
        If System.IO.File.Exists(ballance_start_path & "3D entities\level\level_15.nmo") = True Then
            level_file = New System.IO.FileInfo(ballance_start_path & "3D entities\level\level_15.nmo")
            item.pro_level_path = ballance_start_path & "3D entities\level\level_15.nmo"
            item.pro_level_name = "Level 15（第15关）"
            item.pro_map_name = check_map_name(get_file_md5(level_file.FullName))
            ui_connect_form_level_form_check_list.Add(item)
            item = New ui_depend_form_level_form_check_list
        End If

        ui_form_level_form_check_list.ItemsSource = ui_connect_form_level_form_check_list

        linshi.Close()

        '更新ui-listbox
        ui_update_list()
    End Sub

    '还原
    Private Sub ui_form_level_form_check_restart_btn(sender As Object, e As RoutedEventArgs)
        If ui_connect_form_level_form_check_list.Count <> 0 Then
            Dim select_level(13) As Boolean
            For a = 1 To 13
                select_level(a) = False
            Next

            For b = 0 To ui_form_level_form_check_list.SelectedItems.Count - 1
                For c = 0 To ui_connect_form_level_form_check_list.Count
                    '相同了
                    If ui_connect_form_level_form_check_list.Item(c).Equals(ui_form_level_form_check_list.SelectedItems.Item(b)) = True Then
                        select_level(c + 1) = True
                        Exit For
                    End If
                Next
            Next


            If ui_form_level_form_check_list.SelectedIndex <> -1 Then
                Dim ok_word As String = "确认重置以下关卡吗？"
                For d = 1 To 13
                    If select_level(d) = True Then
                        ok_word = ok_word & vbCrLf & "Level " & d & "（第" & d & "关）"
                    End If
                Next

                If MsgBox(ok_word, MsgBoxStyle.OkCancel + 32, "Ballance工具箱") = MsgBoxResult.Ok Then
                    '开始替换

                End If
            Else
                MsgBox("您没有选中任何关卡", 16, "Ballance工具箱")
            End If

        Else

            MsgBox("请先检测一次所有关卡才能进行重置", 16, "Ballance工具箱")

        End If
    End Sub


    '地图管理************************************************
    '选择项更改
    Private Sub ui_form_level_form_level_list_select_change(sender As Object, e As SelectionChangedEventArgs) Handles ui_form_level_form_level_list.SelectionChanged
        If ui_form_level_form_level_list.SelectedIndex <> -1 Then
            If ui_form_level_form_level_list_form_search.Text = "" Then

                If My.Computer.Network.IsAvailable = True Then

                    '显示对话框

                    Dim linshi_ = New Window_wait
                    linshi_.Owner = Me
                    linshi_.Show()

                    ui_form_level_form_hero_list.ItemsSource = Nothing

                    If ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_id <> "" Then
                        If user_name = "" Or user_password = "" Then
                            Dim linshi = New Window_user_login
                            linshi.Owner = Me
                            linshi.ShowDialog()
                        End If

                        If user_name = "" Or user_password = "" Then
                        Else
                            ui_connect_form_level_form_hero_list.Clear()

                            Dim OnLineScore = New List(Of ScoreManager.Data.Score)

                            'post提交
                            Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create("http://jxtoolbox.sinaapp.com/api/select.php"), System.Net.HttpWebRequest)
                            request.Method = "POST"
                            request.ContentType = "application/x-www-form-urlencoded"
                            'request.ContentLength = System.Text.Encoding.UTF8.GetByteCount(String.Format("ID={0}&Passwd={1}&Index={2}&Type=0", user_name, user_password,
                            'ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_id))
                            'request.CookieContainer = cookie
                            Dim myRequestStream As System.IO.Stream = request.GetRequestStream()
                            Dim myStreamWriter As System.IO.StreamWriter = New System.IO.StreamWriter(myRequestStream, System.Text.Encoding.GetEncoding("gb2312"))
                            myStreamWriter.Write(String.Format("ID={0}&Passwd={1}&Index={2}&Type=0", user_name, user_password,
                                                                                           ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_id))
                            myStreamWriter.Close()



                            Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                            'response.Cookies = cookie.GetCookies(response.ResponseUri);
                            Dim myResponseStream As System.IO.Stream = response.GetResponseStream()
                            Dim myStreamReader As System.IO.StreamReader = New System.IO.StreamReader(myResponseStream, System.Text.Encoding.GetEncoding("utf-8"))
                            Dim retString As String = myStreamReader.ReadToEnd()
                            myStreamReader.Close()
                            myResponseStream.Close()

                            '开始读取
                            Try
                                OnLineScore = ScoreManager.IO.Deserialize(Of ScoreManager.Data.Score).JsonDeserializeListData(retString)

                                If OnLineScore.Count <> 0 Then
                                    For a = 0 To OnLineScore.Count - 1
                                        Try
                                            OnLineScore.Item(a).GetImage()
                                        Catch ex As Exception
                                            System.IO.File.Copy(Environment.CurrentDirectory & "\cache\nothing.jpg",
                                                                  Environment.CurrentDirectory & "\cache\user\" & OnLineScore.Item(a).Player & ".jpg")
                                        End Try
                                    Next

                                    Dim aaa As New ui_depend_form_level_form_hero_list
                                    For a = 0 To OnLineScore.Count - 1
                                        aaa = New ui_depend_form_level_form_hero_list
                                        Select Case a
                                            Case 0
                                                aaa.pro_background = New SolidColorBrush(Color.FromArgb(255, 218, 178, 115))
                                            Case 1
                                                aaa.pro_background = New SolidColorBrush(Color.FromArgb(255, 233, 233, 216))
                                            Case 2
                                                aaa.pro_background = New SolidColorBrush(Color.FromArgb(255, 181, 163, 101))
                                        End Select

                                        If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\user\" & OnLineScore.Item(a).Player & ".jpg") = True Then
                                            aaa.pro_user_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\user\" & OnLineScore.Item(a).Player & ".jpg")))
                                        Else
                                            aaa.pro_user_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))
                                        End If

                                        aaa.pro_player = OnLineScore.Item(a).Player
                                        aaa.pro_point = OnLineScore.Item(a).Points
                                        aaa.pro_creat_time = OnLineScore.Item(a).Time
                                        aaa.pro_time = String.Format("{0}:{1:00.0}", OnLineScore.Item(a).SRTime / 120, (OnLineScore.Item(a).SRTime Mod 120) / 2)
                                        aaa.pro_exter_point = OnLineScore.Item(a).SubExtraPoints
                                        ui_connect_form_level_form_hero_list.Add(aaa)
                                    Next

                                End If

                            Catch ex As Exception
                                MsgBox("您的用户名与密码可能有误，或者网络出现了一些小问题，请在稍后重新点击该关卡以重新获取英雄榜", 16, "Ballance工具箱")
                                user_name = ""
                                user_password = ""
                            End Try

                        End If
                    Else
                        '没有英雄榜的
                        ui_connect_form_level_form_hero_list.Clear()
                    End If

                    ui_form_level_form_hero_list.ItemsSource = ui_connect_form_level_form_hero_list

                    '设置地图预览
                    ui_form_level_form_level_show.Background = ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_image
                    ui_form_level_form_level_show_title.Text = ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_title
                    ui_form_level_form_level_show_text.Text = ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_creator


                    linshi_.Close()

                    '更新ui-listbox
                    ui_update_list()

                End If
            Else
                MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
            End If
        End If

    End Sub

    '安装选中关卡
    Private Sub ui_form_level_form_level_menu_setup_map(sender As Object, e As RoutedEventArgs)

        If ui_form_level_form_level_list_form_search.Text = "" Then
            Dim aaa As New ui_depend_window_select_item_list
            ui_connect_window_select_item_list.Clear()
            ui_connect_window_select_item_list_select_index = 0

            '原版关卡
            For a = 1 To 13
                If a >= 10 Then
                    If System.IO.File.Exists(ballance_start_path & "3d entities\level\level_" & a & ".nmo") = True Then
                        aaa.pro_title = "第" & a & "关"
                        aaa.pro_text = ballance_start_path & "3d entities\level\level_" & a & ".nmo"
                    End If
                Else
                    If System.IO.File.Exists(ballance_start_path & "3d entities\level\level_0" & a & ".nmo") = True Then
                        aaa.pro_title = "第" & a & "关"
                        aaa.pro_text = ballance_start_path & "3d entities\level\level_0" & a & ".nmo"
                    End If
                End If

                ui_connect_window_select_item_list.Add(aaa)
                aaa = New ui_depend_window_select_item_list
            Next

            'mod关卡
            If System.IO.File.Exists(ballance_start_path & "3d entities\level\level_14.nmo") = True Then
                aaa.pro_title = "第14关"
                aaa.pro_text = ballance_start_path & "3d entities\level\level_14.nmo"
                ui_connect_window_select_item_list.Add(aaa)
                aaa = New ui_depend_window_select_item_list
            End If
            If System.IO.File.Exists(ballance_start_path & "3d entities\level\level_15.nmo") = True Then
                aaa.pro_title = "第15关"
                aaa.pro_text = ballance_start_path & "3d entities\level\level_15.nmo"
                ui_connect_window_select_item_list.Add(aaa)
                aaa = New ui_depend_window_select_item_list
            End If

            '显示对话框

            Dim linshi = New Window_select_item
            ui_connect_window_select_item_list_select_index = -1
            linshi.Owner = Me
            linshi.ShowDialog()

            '安装
            If ui_connect_window_select_item_list_select_index <> -1 Then
                ui_connect_window_select_item_list_select_index += 1
                If ui_connect_window_select_item_list_select_index >= 10 Then
                    System.IO.File.Delete(ballance_start_path & "3d entities\level\level_" & ui_connect_window_select_item_list_select_index & ".nmo")
                    System.IO.File.Copy(Environment.CurrentDirectory & "\map\" & ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_title & ".nmo",
                                       ballance_start_path & "3d entities\level\level_" & ui_connect_window_select_item_list_select_index & ".nmo")
                Else
                    System.IO.File.Delete(ballance_start_path & "3d entities\level\level_0" & ui_connect_window_select_item_list_select_index & ".nmo")
                    System.IO.File.Copy(Environment.CurrentDirectory & "\map\" & ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_title & ".nmo",
                           ballance_start_path & "3d entities\level\level_0" & ui_connect_window_select_item_list_select_index & ".nmo")
                End If

                MsgBox("安装完成", 64, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    '安装地图包
    Private Sub ui_form_level_form_level_menu_setup_map_bag(sender As Object, e As RoutedEventArgs)

        If ui_form_level_form_level_list_form_search.Text = "" Then
            open_map_bag_file.ShowDialog()
            If open_map_bag_file.FileName <> "" Then
                '显示对话框

                Dim linshi = New Window_wait
                linshi.Owner = Me
                linshi.Show()

                '解压缩
                Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                aaa.ExtractZip(open_map_file.FileName, Environment.CurrentDirectory & "\map\", Nothing)

                '解决文件夹内部套文件的问题
                Dim folder As New System.IO.DirectoryInfo(Environment.CurrentDirectory & "\map\")
                Dim folder_arr() As System.IO.DirectoryInfo = folder.GetDirectories()
                If folder_arr.Count <> 0 Then
                    For a = 0 To folder_arr.Count - 1
                        '获取目录内部文件列表
                        Dim file_arr() As System.IO.FileInfo = folder_arr(a).GetFiles()
                        '移走文件
                        If file_arr.Count <> 0 Then
                            For b = 0 To file_arr.Count - 1
                                Try
                                    If System.IO.File.Exists(Environment.CurrentDirectory & "\map\" & file_arr(b).Name) = True Then
                                        '有重名的
                                        System.IO.File.Move(file_arr(b).FullName, Environment.CurrentDirectory & "\map\" & Mid(file_arr(b).Name, 1, file_arr(b).Name.Length - 4) & folder_arr(a).Name & ".nmo")
                                    Else
                                        '直接复制
                                        System.IO.File.Move(file_arr(b).FullName, Environment.CurrentDirectory & "\map\" & file_arr(b).Name)
                                    End If

                                Catch ex As Exception
                                    '仍然有重名的，不管
                                End Try

                            Next
                        End If

                    Next
                End If


                '重新构架列表
                ui_form_level_form_level_list.ItemsSource = Nothing
                ui_connect_form_level_form_level_list.Clear()
                app_init_map(Nothing, False)
                ui_form_level_form_level_list.ItemsSource = ui_connect_form_level_form_level_list

                linshi.Close()

                '更新ui-listbox
                ui_update_list()

                MsgBox("添加完成！", 64, "Ballance工具箱")


            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    '添加地图
    Private Sub ui_form_level_form_level_menu_add_map(sender As Object, e As RoutedEventArgs)

        If ui_form_level_form_level_list_form_search.Text = "" Then
            If open_map_file.ShowDialog() = True Then
                If System.IO.File.Exists(Environment.CurrentDirectory & "\map\" & open_map_file.SafeFileName) = True Then
                    '存在了，报错
                    MsgBox("地图 " & open_map_file.SafeFileName & " 已经存在，无法添加", 16, "Ballance工具箱")
                Else
                    '不存在
                    System.IO.File.Copy(open_map_file.FileName,
                                    Environment.CurrentDirectory & "\map\" & open_map_file.SafeFileName)

                    '获取地图列表
                    Dim OnLineMap = New List(Of ScoreManager.Data.Map)

                    If My.Computer.Network.IsAvailable = True Then
                        Dim c As System.Net.WebClient = New System.Net.WebClient
                        c.Headers.Add("Referer", "http://jxtoolbox.sinaapp.com/api/map_json.php")
                        Dim reader = New System.IO.StreamReader(c.OpenRead("http://jxtoolbox.sinaapp.com/api/map_json.php"),
                                                        System.Text.Encoding.UTF8)
                        Dim text As String = reader.ReadToEnd

                        OnLineMap = ScoreManager.IO.Deserialize(Of ScoreManager.Data.Map).JsonDeserializeListData(text)
                    End If

                    '将数据输入列表
                    Dim aaa As New ui_depend_form_level_form_level_list

                    aaa.pro_title = Mid(open_map_file.SafeFileName, 1, open_map_file.SafeFileName.Length - 4)
                    aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))
                    If OnLineMap.Count <> 0 Then
                        For b = 0 To OnLineMap.Count - 1
                            If OnLineMap.Item(b).Title = aaa.pro_title Then
                                '有相同的
                                aaa.pro_id = OnLineMap.Item(b).ID
                                aaa.pro_creator = "制作者：" & OnLineMap.Item(b).Creator
                                aaa.pro_description = "描述：" & OnLineMap.Item(b).Description
                                aaa.pro_difficulty = "难度：" & OnLineMap.Item(b).Difficulty
                                aaa.pro_downloadcount = "下载次数：" & OnLineMap.Item(b).DownloadCount
                                If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\map\" & aaa.pro_title & ".jpg") = True Then
                                    aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\map\" & aaa.pro_title & ".jpg")))
                                End If
                                aaa.pro_playcount = "玩的次数：" & OnLineMap.Item(b).PlayCount

                                aaa.pro_stars = "星级："
                                If OnLineMap.Item(b).Stars <> 0 Then
                                    For c = 0 To OnLineMap.Item(b).Stars - 1
                                        aaa.pro_stars = aaa.pro_stars & "★"
                                    Next
                                End If

                            End If
                        Next
                    End If
                    If aaa.pro_creator = "" Then
                        aaa.pro_id = ""
                        aaa.pro_creator = "制作者：未知"
                        aaa.pro_description = "描述："
                        aaa.pro_difficulty = "难度：未知"
                        aaa.pro_downloadcount = "下载次数：未知"
                        If System.IO.File.Exists(Environment.CurrentDirectory & "\cache\map\" & aaa.pro_title & ".jpg") = True Then
                            aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\map\" & aaa.pro_title & ".jpg")))
                        End If
                        aaa.pro_playcount = "玩的次数：未知"
                        aaa.pro_stars = "星级：未知"

                    End If

                    ui_form_level_form_level_list.ItemsSource = Nothing
                    ui_connect_form_level_form_level_list.Add(aaa)
                    ui_form_level_form_level_list.ItemsSource = ui_connect_form_level_form_level_list

                    '更新ui-listbox
                    ui_update_list()

                    MsgBox("添加完成", 64, "Ballance工具箱")
                End If
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If

    End Sub

    '删除选中地图
    Private Sub ui_form_level_form_level_menu_delete_map(sender As Object, e As RoutedEventArgs)

        If ui_form_level_form_level_list.SelectedIndex <> -1 And ui_form_level_form_level_list_form_search.Text = "" Then
            If MsgBox("确认删除地图 " &
                              ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_title &
                              " 吗？", 32 + 1, "Ballance工具箱") = 1 Then
                '删除
                System.IO.File.Delete(Environment.CurrentDirectory & "\map\" &
                                      ui_connect_form_level_form_level_list.Item(ui_form_level_form_level_list.SelectedIndex).pro_title & ".nmo")

                Dim linshi As Integer = ui_form_level_form_level_list.SelectedIndex
                ui_form_level_form_level_list.ItemsSource = Nothing
                ui_connect_form_level_form_level_list.RemoveAt(linshi)
                ui_form_level_form_level_list.ItemsSource = ui_connect_form_level_form_level_list

                '更新ui-listbox
                ui_update_list()

                MsgBox("删除完成", 64, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If

    End Sub

    '地图列表搜索
    Private Sub ui_form_level_form_level_list_form_search_change(sender As Object, e As TextChangedEventArgs) Handles ui_form_level_form_level_list_form_search.TextChanged
        If ui_form_level_form_level_list_form_search.Text = "" Then
            '不用搜索，强制还原
            ui_form_level_form_level_list.ItemsSource = Nothing
            ui_form_level_form_level_list.ItemsSource = ui_connect_form_level_form_level_list
            ui_connect_form_level_form_level_list_search.Clear()
        Else
            '需要搜索
            ui_form_level_form_level_list.ItemsSource = Nothing
            ui_connect_form_level_form_level_list_search.Clear()

            If ui_connect_form_level_form_level_list.Count <> 0 Then
                For a = 0 To ui_connect_form_level_form_level_list.Count - 1

                    If InStr(ui_connect_form_level_form_level_list.Item(a).pro_title, ui_form_level_form_level_list_form_search.Text, CompareMethod.Text) = 0 Then
                        '没找到 
                        '什么都不做
                    Else
                        '找到了
                        ui_connect_form_level_form_level_list_search.Add(ui_connect_form_level_form_level_list.Item(a))

                    End If

                Next
            End If

            ui_form_level_form_level_list.ItemsSource = ui_connect_form_level_form_level_list_search

        End If
    End Sub


#End Region

#Region "nmo文件"

    '常用补丁************************************************
    '普通球
    Private Sub ui_form_nmo_form_normal_ball_normal(sender As Object, e As RoutedEventArgs)
        System.IO.File.Delete(ballance_start_path & "3D Entities\Balls.nmo")
        System.IO.File.Copy(Environment.CurrentDirectory & "\system_nmo\balls\normal.nmo", ballance_start_path & "3D Entities\Balls.nmo")

        '传递ui
        ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：正常速度"

        MsgBox("安装完成", 64, "Ballane工具箱")
    End Sub
    '倍速球
    Private Sub ui_form_nmo_form_normal_ball_speed(sender As Object, e As RoutedEventArgs)

        Dim aaa As New ui_depend_window_select_item_list
        ui_connect_window_select_item_list.Clear()
        ui_connect_window_select_item_list_select_index = 0

        aaa.pro_title = "1.75倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "2倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "3倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "4倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "5倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "6倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "7倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "8倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "9倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "10倍速"
        aaa.pro_text = ""
        ui_connect_window_select_item_list.Add(aaa)

        '显示对话框

        Dim linshi = New Window_select_item
        ui_connect_window_select_item_list_select_index = -1
        linshi.Owner = Me
        linshi.ShowDialog()

        '安装
        If ui_connect_window_select_item_list_select_index <> -1 Then
            Dim list As Integer = ui_connect_window_select_item_list_select_index + 1

            System.IO.File.Delete(ballance_start_path & "3D Entities\Balls.nmo")
            System.IO.File.Copy(Environment.CurrentDirectory & "\system_nmo\balls\" & list & ".nmo", ballance_start_path & "3D Entities\Balls.nmo")

            '传递ui
            ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：" & ui_connect_window_select_item_list.Item(ui_connect_window_select_item_list_select_index).pro_title

            MsgBox("安装完成", 64, "Ballane工具箱")
        End If

    End Sub
    '三高球
    Private Sub ui_form_nmo_form_normal_high(sender As Object, e As RoutedEventArgs)
        System.IO.File.Delete(ballance_start_path & "3D Entities\Balls.nmo")
        System.IO.File.Copy(Environment.CurrentDirectory & "\system_nmo\balls\high.nmo", ballance_start_path & "3D Entities\Balls.nmo")

        '传递ui
        ui_form_nmo_form_normal_ball_state.Text = "当前球的状态是：三高球"

        MsgBox("安装完成", 64, "Ballane工具箱")
    End Sub

    '开启无限命
    Private Sub ui_form_nmo_form_normal_life_add(sender As Object, e As RoutedEventArgs)
        System.IO.File.Delete(ballance_start_path & "3D Entities\Gameplay.nmo")
        System.IO.File.Copy(Environment.CurrentDirectory & "\system_nmo\life\on.nmo", ballance_start_path & "3D Entities\Gameplay.nmo")

        MsgBox("安装完成", 64, "Ballane工具箱")
    End Sub
    '关闭无限命
    Private Sub ui_form_nmo_form_normal_life_del(sender As Object, e As RoutedEventArgs)
        System.IO.File.Delete(ballance_start_path & "3D Entities\Gameplay.nmo")
        System.IO.File.Copy(Environment.CurrentDirectory & "\system_nmo\life\off.nmo", ballance_start_path & "3D Entities\Gameplay.nmo")

        MsgBox("安装完成", 64, "Ballane工具箱")
    End Sub


    'mod与模组************************************************

    'mod包添加
    Private Sub ui_form_nmo_form_mod_mod_bag_add(sender As Object, e As RoutedEventArgs)
        If ui_form_nmo_form_mod_mod_list_form_search.Text = "" Then
            If BML_have = True Then
                If open_mod_file.ShowDialog() = True Then
                    '显示对话框

                    Dim linshi = New Window_wait
                    linshi.Owner = Me
                    linshi.Show()

                    '解压缩
                    Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                    aaa.ExtractZip(open_mod_file.FileName, ballance_start_path & "ModLoader\Mods\", Nothing)

                    '重新构架列表
                    ui_form_nmo_form_mod_mod_list.ItemsSource = Nothing
                    ui_connect_form_nmo_form_mod_mod_list.Clear()
                    app_init_mod(Nothing, False)
                    ui_form_nmo_form_mod_mod_list.ItemsSource = ui_connect_form_nmo_form_mod_mod_list

                    linshi.Close()

                    '更新ui-listbox
                    ui_update_list()

                    MsgBox("添加完成！", 64, "Ballance工具箱")

                End If

            Else
                MsgBox("您没有安装BML（Ballance Mod Loader），您必须安装BML才能添加Mod包！", 16, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    'mod添加
    Private Sub ui_form_nmo_form_mod_mod_add(sender As Object, e As RoutedEventArgs)
        If ui_form_nmo_form_mod_mod_list_form_search.Text = "" Then
            If BML_have = True Then
                If open_map_file.ShowDialog() = True Then
                    If System.IO.File.Exists(ballance_start_path & "ModLoader\Mods\" & open_map_file.SafeFileName) = False Then
                        '不存在
                        System.IO.File.Copy(open_map_file.FileName,
                                            ballance_start_path & "ModLoader\Mods\" & open_map_file.SafeFileName)

                        ui_form_nmo_form_mod_mod_list.ItemsSource = Nothing
                        Dim aaa As New ui_depend_form_nmo_form_mod_mod_list
                        aaa.pro_title = Mid(open_map_file.SafeFileName, 1, open_map_file.SafeFileName.Length - 4)
                        'TODO:未来：可以展示mod的图片，现在先用无地图的样式--------------位置2
                        aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\Nothing.jpg")))
                        ui_connect_form_nmo_form_mod_mod_list.Add(aaa)
                        ui_form_nmo_form_mod_mod_list.ItemsSource = ui_connect_form_nmo_form_mod_mod_list

                        '更新ui-listbox
                        ui_update_list()
                    Else
                        MsgBox("该Mod已存在，无法继续添加！", 16, "Ballance工具箱")
                    End If

                End If
            Else
                MsgBox("您没有安装BML（Ballance Mod Loader），您必须安装BML才能添加Mod！", 16, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    'mod删除
    Private Sub ui_form_nmo_form_mod_mod_del(sender As Object, e As RoutedEventArgs)
        If ui_form_nmo_form_mod_mod_list.SelectedIndex <> -1 And ui_form_nmo_form_mod_mod_list_form_search.Text = "" Then
            If MsgBox("确认删除Mod " &
                              ui_connect_form_nmo_form_mod_mod_list.Item(ui_form_nmo_form_mod_mod_list.SelectedIndex).pro_title &
                              " 吗？", 32 + 1, "Ballance工具箱") = 1 Then
                '删除
                System.IO.File.Delete(ballance_start_path & "ModLoader\Mods\" &
                                      ui_connect_form_nmo_form_mod_mod_list.Item(ui_form_nmo_form_mod_mod_list.SelectedIndex).pro_title & ".nmo")

                Dim linshi As Integer = ui_form_nmo_form_mod_mod_list.SelectedIndex
                ui_form_nmo_form_mod_mod_list.ItemsSource = Nothing
                ui_connect_form_nmo_form_mod_mod_list.RemoveAt(linshi)
                ui_form_nmo_form_mod_mod_list.ItemsSource = ui_connect_form_nmo_form_mod_mod_list

                '更新ui-listbox
                ui_update_list()

                MsgBox("删除完成", 64, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If
    End Sub

    'mod列表搜索
    Private Sub ui_form_nmo_form_mod_mod_list_form_search_change(sender As Object, e As TextChangedEventArgs) Handles ui_form_nmo_form_mod_mod_list_form_search.TextChanged
        If ui_form_nmo_form_mod_mod_list_form_search.Text = "" Then
            '不用搜索，强制还原
            ui_form_nmo_form_mod_mod_list.ItemsSource = Nothing
            ui_form_nmo_form_mod_mod_list.ItemsSource = ui_connect_form_nmo_form_mod_mod_list
            ui_connect_form_nmo_form_mod_mod_list_search.Clear()
        Else
            '需要搜索
            ui_form_nmo_form_mod_mod_list.ItemsSource = Nothing
            ui_connect_form_nmo_form_mod_mod_list_search.Clear()

            If ui_connect_form_nmo_form_mod_mod_list.Count <> 0 Then
                For a = 0 To ui_connect_form_nmo_form_mod_mod_list.Count - 1

                    If InStr(ui_connect_form_nmo_form_mod_mod_list.Item(a).pro_title, ui_form_nmo_form_mod_mod_list_form_search.Text, CompareMethod.Text) = 0 Then
                        '没找到 
                        '什么都不做
                    Else
                        '找到了
                        ui_connect_form_nmo_form_mod_mod_list_search.Add(ui_connect_form_nmo_form_mod_mod_list.Item(a))

                    End If

                Next
            End If

            ui_form_nmo_form_mod_mod_list.ItemsSource = ui_connect_form_nmo_form_mod_mod_list_search

        End If
    End Sub


    '模组包添加
    Private Sub ui_form_nmo_form_mod_ph_bag_add(sender As Object, e As RoutedEventArgs)
        If ui_form_nmo_form_mod_ph_list_form_search.Text = "" Then
            If open_ph_file.ShowDialog() = True Then
                '显示对话框

                Dim linshi = New Window_wait
                linshi.Owner = Me
                linshi.Show()

                '解压缩
                Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                aaa.ExtractZip(open_ph_file.FileName, ballance_start_path & "3D Entities\PH\", Nothing)

                '重新构架列表
                ui_form_nmo_form_mod_ph_list.ItemsSource = Nothing
                ui_connect_form_nmo_form_mod_ph_list.Clear()
                app_init_ph(Nothing, False)
                ui_form_nmo_form_mod_ph_list.ItemsSource = ui_connect_form_nmo_form_mod_ph_list

                linshi.Close()

                '更新ui-listbox
                ui_update_list()

                MsgBox("添加完成！", 64, "Ballance工具箱")

            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    '模组添加
    Private Sub ui_form_nmo_form_mod_ph_add(sender As Object, e As RoutedEventArgs)
        If ui_form_nmo_form_mod_ph_list_form_search.Text = "" Then
            If open_map_file.ShowDialog() = True Then
                If System.IO.File.Exists(ballance_start_path & "3D Entities\PH\" & open_map_file.SafeFileName) = False Then
                    '不存在
                    System.IO.File.Copy(open_map_file.FileName,
                                        ballance_start_path & "3D Entities\PH\" & open_map_file.SafeFileName)

                    ui_form_nmo_form_mod_ph_list.ItemsSource = Nothing
                    Dim aaa As New ui_depend_form_nmo_form_mod_ph_list
                    aaa.pro_title = Mid(open_map_file.SafeFileName, 1, open_map_file.SafeFileName.Length - 4)
                    'TODO:未来：可以展示mod的图片，现在先用无地图的样式--------------位置2
                    aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\Nothing.jpg")))
                    ui_connect_form_nmo_form_mod_ph_list.Add(aaa)
                    ui_form_nmo_form_mod_ph_list.ItemsSource = ui_connect_form_nmo_form_mod_ph_list

                    '更新ui-listbox
                    ui_update_list()
                Else
                    MsgBox("该模组已存在，无法继续添加！", 16, "Ballance工具箱")
                End If
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    '模组删除
    Private Sub ui_form_nmo_form_mod_ph_del(sender As Object, e As RoutedEventArgs)
        If ui_form_nmo_form_mod_ph_list.SelectedIndex <> -1 And ui_form_nmo_form_mod_ph_list_form_search.Text = "" Then
            If MsgBox("确认删除模组 " &
                              ui_connect_form_nmo_form_mod_ph_list.Item(ui_form_nmo_form_mod_ph_list.SelectedIndex).pro_title &
                              " 吗？删除可能回导致某些地图不能正常加载！", 32 + 1, "Ballance工具箱") = 1 Then
                '删除
                System.IO.File.Delete(ballance_start_path & "3D Entities\PH\" &
                                      ui_connect_form_nmo_form_mod_ph_list.Item(ui_form_nmo_form_mod_ph_list.SelectedIndex).pro_title & ".nmo")

                Dim linshi As Integer = ui_form_nmo_form_mod_ph_list.SelectedIndex
                ui_form_nmo_form_mod_ph_list.ItemsSource = Nothing
                ui_connect_form_nmo_form_mod_ph_list.RemoveAt(linshi)
                ui_form_nmo_form_mod_ph_list.ItemsSource = ui_connect_form_nmo_form_mod_ph_list

                '更新ui-listbox
                ui_update_list()

                MsgBox("删除完成", 64, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If
    End Sub

    '模组列表搜索
    Private Sub ui_form_nmo_form_mod_ph_list_form_search_change(sender As Object, e As TextChangedEventArgs) Handles ui_form_nmo_form_mod_ph_list_form_search.TextChanged
        If ui_form_nmo_form_mod_ph_list_form_search.Text = "" Then
            '不用搜索，强制还原
            ui_form_nmo_form_mod_ph_list.ItemsSource = Nothing
            ui_form_nmo_form_mod_ph_list.ItemsSource = ui_connect_form_nmo_form_mod_ph_list
            ui_connect_form_nmo_form_mod_ph_list_search.Clear()
        Else
            '需要搜索
            ui_form_nmo_form_mod_ph_list.ItemsSource = Nothing
            ui_connect_form_nmo_form_mod_ph_list_search.Clear()

            If ui_connect_form_nmo_form_mod_ph_list.Count <> 0 Then
                For a = 0 To ui_connect_form_nmo_form_mod_ph_list.Count - 1

                    If InStr(ui_connect_form_nmo_form_mod_ph_list.Item(a).pro_title, ui_form_nmo_form_mod_ph_list_form_search.Text, CompareMethod.Text) = 0 Then
                        '没找到 
                        '什么都不做
                    Else
                        '找到了
                        ui_connect_form_nmo_form_mod_ph_list_search.Add(ui_connect_form_nmo_form_mod_ph_list.Item(a))

                    End If

                Next
            End If

            ui_form_nmo_form_mod_ph_list.ItemsSource = ui_connect_form_nmo_form_mod_ph_list_search

        End If
    End Sub

    'bml安装，卸载************************************************
    '安装
    Private Sub ui_form_nmo_form_mod_bml_install_run(sender As Object, e As RoutedEventArgs) Handles ui_form_nmo_form_mod_bml_install.Click

        If MsgBox("确认安装？安装之后Ballance本身的文件将会被修改，而且安装后，如果破了纪录，认证你的记录是合法的将会变得困难，带来诸多不便！", 32 + 1 + 256, "Ballance工具箱") = 1 Then
            If System.IO.File.Exists(Environment.CurrentDirectory & "\system_nmo\bml_install.exe") = True Then

                System.Diagnostics.Process.Start(Environment.CurrentDirectory & "\system_nmo\bml_install.exe")

            Else
                MsgBox("无可用的BML安装源，需要从Ballance工具箱部署器部署有关安装源才能执行安装", 16, "Ballance工具箱")
            End If
        End If

    End Sub

    '卸载
    Private Sub ui_form_nmo_form_mod_bml_uninstall_run(sender As Object, e As RoutedEventArgs) Handles ui_form_nmo_form_mod_bml_uninstall.Click

        'TODO:未来可以卸载bml
        'System.Diagnostics.Process.Start(ballance_start_path & "Uninstall.exe")
        MsgBox("暂无该功能，该功能未开发完毕", 16, "Ballance工具箱")

    End Sub

    '记录证明************************************************
    Private Sub ui_form_nmo_form_promise_open_run(sender As Object, e As RoutedEventArgs) Handles ui_form_nmo_form_promise_open.Click

        'TODO:未来可以记录证明
        MsgBox("暂无该功能，该功能未开发完毕", 16, "Ballance工具箱")

    End Sub

#End Region

#Region "贴图背景"

    '材质************************************************
    '添加材质
    Private Sub ui_form_photo_form_material_add(sender As Object, e As RoutedEventArgs)
        If ui_form_photo_form_material_list_form_search.Text = "" Then
            If open_material_file.ShowDialog() = True Then
                If System.IO.File.Exists(Environment.CurrentDirectory & "\material\" & open_material_file.SafeFileName) = False Then
                    '不存在
                    System.IO.File.Copy(open_material_file.FileName,
                                        Environment.CurrentDirectory & "\material\" & open_material_file.SafeFileName)

                    ui_form_photo_form_material_list.ItemsSource = Nothing
                    Dim aaa As New ui_depend_form_photo_form_material_list
                    aaa.pro_title = Mid(open_material_file.SafeFileName, 1, open_material_file.SafeFileName.Length - 4)
                    'TODO:未来：可以预览地图包的背景样式，现在先用无地图的样式--------------位置2
                    aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\Nothing.jpg")))
                    ui_connect_form_photo_form_material_list.Add(aaa)
                    ui_form_photo_form_material_list.ItemsSource = ui_connect_form_photo_form_material_list

                    '更新ui-listbox
                    ui_update_list()
                Else
                    MsgBox("该材质包已存在，无法继续添加！", 16, "Ballance工具箱")
                End If
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    '删除材质
    Private Sub ui_form_photo_form_material_del(sender As Object, e As RoutedEventArgs)
        If ui_form_photo_form_material_list.SelectedIndex <> -1 And ui_form_photo_form_material_list_form_search.Text = "" Then
            If MsgBox("确认删除材质包 " &
                              ui_connect_form_photo_form_material_list.Item(ui_form_photo_form_material_list.SelectedIndex).pro_title &
                              " 吗？", 32 + 1, "Ballance工具箱") = 1 Then
                '删除
                System.IO.File.Delete(Environment.CurrentDirectory & "\material\" &
                                      ui_connect_form_photo_form_material_list.Item(ui_form_photo_form_material_list.SelectedIndex).pro_title & ".bab")

                Dim linshi As Integer = ui_form_photo_form_material_list.SelectedIndex
                ui_form_photo_form_material_list.ItemsSource = Nothing
                ui_connect_form_photo_form_material_list.RemoveAt(linshi)
                ui_form_photo_form_material_list.ItemsSource = ui_connect_form_photo_form_material_list

                '更新ui-listbox
                ui_update_list()

                MsgBox("删除完成", 64, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    '应用材质
    Private Sub ui_form_photo_form_material_apply(sender As Object, e As RoutedEventArgs)
        If ui_form_photo_form_material_list.SelectedIndex <> -1 And ui_form_photo_form_material_list_form_search.Text = "" Then
            If MsgBox("确认应用材质 " &
                               ui_connect_form_photo_form_material_list.Item(ui_form_photo_form_material_list.SelectedIndex).pro_title &
                              " 吗？", 32 + 1, "Ballance工具箱") = 1 Then

                '显示对话框

                Dim linshi = New Window_wait
                linshi.Owner = Me
                linshi.Show()

                '解压缩
                Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                aaa.ExtractZip(Environment.CurrentDirectory & "\material\" &
                               ui_connect_form_photo_form_material_list.Item(ui_form_photo_form_material_list.SelectedIndex).pro_title & ".bab",
                               ballance_start_path & "Textures\", Nothing)

                linshi.Close()

                MsgBox("应用成功", 64, "Ballance工具箱")
            End If

        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If
    End Sub

    '还原为原版材质
    Private Sub ui_form_photo_form_material_return(sender As Object, e As RoutedEventArgs)

        If MsgBox("确认还原？此操作将消耗一些时间，而且不可恢复", 32 + 1 + 256, "Ballance工具箱") = 1 Then
            If System.IO.File.Exists(Environment.CurrentDirectory & "\system_nmo\normal_material.zip") = True Then

                '等待窗口
                Dim linshi = New Window_wait
                linshi.Owner = Me
                linshi.Show()

                Try
                    '解压缩
                    Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                    aaa.ExtractZip(Environment.CurrentDirectory & "\system_nmo\normal_material.zip", ballance_start_path & "Textures\", Nothing)
                Catch ex As Exception
                    MsgBox("还原时出现错误，无法还原！", 16, "Ballance工具箱")
                End Try

                linshi.Close()

            Else
                MsgBox("无可用的还原资源，需要从Ballance工具箱部署器部署有关资源才能执行还原", 16, "Ballance工具箱")
            End If
        End If

    End Sub

    '材质列表搜索
    Private Sub ui_form_photo_form_material_list_form_search_change(sender As Object, e As TextChangedEventArgs) Handles ui_form_photo_form_material_list_form_search.TextChanged
        If ui_form_photo_form_material_list_form_search.Text = "" Then
            '不用搜索，强制还原
            ui_form_photo_form_material_list.ItemsSource = Nothing
            ui_form_photo_form_material_list.ItemsSource = ui_connect_form_photo_form_material_list
            ui_connect_form_photo_form_material_list_search.Clear()
        Else
            '需要搜索
            ui_form_photo_form_material_list.ItemsSource = Nothing
            ui_connect_form_photo_form_material_list_search.Clear()

            If ui_connect_form_photo_form_material_list.Count <> 0 Then
                For a = 0 To ui_connect_form_photo_form_material_list.Count - 1

                    If InStr(ui_connect_form_photo_form_material_list.Item(a).pro_title, ui_form_photo_form_material_list_form_search.Text, CompareMethod.Text) = 0 Then
                        '没找到 
                        '什么都不做
                    Else
                        '找到了
                        ui_connect_form_photo_form_material_list_search.Add(ui_connect_form_photo_form_material_list.Item(a))

                    End If

                Next
            End If

            ui_form_photo_form_material_list.ItemsSource = ui_connect_form_photo_form_material_list_search

        End If
    End Sub




    '背景包************************************************
    '安装背景包
    Private Sub ui_form_photo_form_background_add_bag(sender As Object, e As RoutedEventArgs)
        If ui_form_photo_form_background_list_form_search.Text = "" Then
            If open_background_file.ShowDialog() = True Then
                If System.IO.File.Exists(Environment.CurrentDirectory & "\background\" & open_background_file.SafeFileName) = False Then
                    '不存在
                    System.IO.File.Copy(open_background_file.FileName,
                                        Environment.CurrentDirectory & "\background\" & open_background_file.SafeFileName)

                    ui_form_photo_form_background_list.ItemsSource = Nothing
                    Dim aaa As New ui_depend_form_photo_form_background_list
                    aaa.pro_title = Mid(open_background_file.SafeFileName, 1, open_background_file.SafeFileName.Length - 4)
                    'TODO:未来：可以预览地图包的背景样式，现在先用无地图的样式--------------位置2
                    aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\Nothing.jpg")))
                    ui_connect_form_photo_form_background_list.Add(aaa)
                    ui_form_photo_form_background_list.ItemsSource = ui_connect_form_photo_form_background_list

                    '更新ui-listbox
                    ui_update_list()
                Else
                    MsgBox("该背景包已存在，无法继续添加！", 16, "Ballance工具箱")
                End If
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    '删除选中背景包
    Private Sub ui_form_photo_form_background_del_bag(sender As Object, e As RoutedEventArgs)
        If ui_form_photo_form_background_list.SelectedIndex <> -1 And ui_form_photo_form_background_list_form_search.Text = "" Then
            If MsgBox("确认删除背景包 " &
                              ui_connect_form_photo_form_background_list.Item(ui_form_photo_form_background_list.SelectedIndex).pro_title &
                              " 吗？", 32 + 1, "Ballance工具箱") = 1 Then
                '删除
                System.IO.File.Delete(Environment.CurrentDirectory & "\background\" &
                                      ui_connect_form_photo_form_background_list.Item(ui_form_photo_form_background_list.SelectedIndex).pro_title & ".bbb")

                Dim linshi As Integer = ui_form_photo_form_background_list.SelectedIndex
                ui_form_photo_form_background_list.ItemsSource = Nothing
                ui_connect_form_photo_form_background_list.RemoveAt(linshi)
                ui_form_photo_form_background_list.ItemsSource = ui_connect_form_photo_form_background_list

                '更新ui-listbox
                ui_update_list()

                MsgBox("删除完成", 64, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If
    End Sub

    '应用背景
    Private Sub ui_form_photo_form_background_apply(sender As Object, e As RoutedEventArgs)
        If ui_form_photo_form_background_list.SelectedIndex <> -1 And ui_form_photo_form_background_list_form_search.Text = "" Then

            If MsgBox("确认应用背景包 " &
                              ui_connect_form_photo_form_background_list.Item(ui_form_photo_form_background_list.SelectedIndex).pro_title &
                              " 吗？", 32 + 1, "Ballance工具箱") = 1 Then

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

                    '显示对话框

                    Dim linshi_2 = New Window_wait
                    linshi_2.Owner = Me
                    linshi_2.Show()

                    '解压缩
                    Dim bbb As New ICSharpCode.SharpZipLib.Zip.FastZip
                    bbb.ExtractZip(Environment.CurrentDirectory & "\background\" &
                           ui_connect_form_photo_form_background_list.Item(ui_form_photo_form_background_list.SelectedIndex).pro_title & ".bbb",
                           ballance_start_path & "Textures\Sky\", Nothing)

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
                            MsgBox("发生错误", 16, "Ballance工具箱")
                            Exit Sub
                    End Select

                    System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_Back.new", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Back.old")
                    System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_Front.new", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Front.old")
                    System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_Left.new", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Left.old")
                    System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_Right.new", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Right.old")
                    System.IO.File.Move(ballance_start_path + "Textures\Sky\Sky_Down.new", ballance_start_path + "Textures\Sky\Sky_" & bk_list & "_Down.old")

                    linshi_2.Close()

                    If MsgBox("已成功应用背景包，背景包要在游戏中应用需要重启程序一次，是否立即关闭程序，手动重启？" & vbCrLf & vbCrLf & "提示：替换多个背景序号的背景可以统一重启程序~", 32 + 1, "Ballance工具箱") = 1 Then
                        Environment.Exit(0)
                    End If

                End If

            End If

        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If

    End Sub

    '还原为原版背景
    Private Sub ui_form_photo_form_background_return(sender As Object, e As RoutedEventArgs)

        If MsgBox("确认还原？此操作将消耗一些时间，而且不可恢复", 32 + 1 + 256, "Ballance工具箱") = 1 Then
            If System.IO.File.Exists(Environment.CurrentDirectory & "\system_nmo\normal_background.zip") = True Then

                '等待窗口
                Dim linshi = New Window_wait
                linshi.Owner = Me
                linshi.Show()

                Try
                    '解压缩
                    Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                    aaa.ExtractZip(Environment.CurrentDirectory & "\system_nmo\normal_background.zip", ballance_start_path & "Textures\Sky\", Nothing)
                Catch ex As Exception
                    MsgBox("还原时出现错误，无法还原！", 16, "Ballance工具箱")
                End Try

                linshi.Close()
            Else
                MsgBox("无可用的还原资源，需要从Ballance工具箱部署器部署有关资源才能执行还原", 16, "Ballance工具箱")
            End If
        End If

    End Sub

    '背景列表搜索
    Private Sub ui_form_photo_form_background_list_form_search_change(sender As Object, e As TextChangedEventArgs) Handles ui_form_photo_form_background_list_form_search.TextChanged
        If ui_form_photo_form_background_list_form_search.Text = "" Then
            '不用搜索，强制还原
            ui_form_photo_form_background_list.ItemsSource = Nothing
            ui_form_photo_form_background_list.ItemsSource = ui_connect_form_photo_form_background_list
            ui_connect_form_photo_form_background_list_search.Clear()
        Else
            '需要搜索
            ui_form_photo_form_background_list.ItemsSource = Nothing
            ui_connect_form_photo_form_background_list_search.Clear()

            If ui_connect_form_photo_form_background_list.Count <> 0 Then
                For a = 0 To ui_connect_form_photo_form_background_list.Count - 1

                    If InStr(ui_connect_form_photo_form_background_list.Item(a).pro_title, ui_form_photo_form_background_list_form_search.Text, CompareMethod.Text) = 0 Then
                        '没找到 
                        '什么都不做
                    Else
                        '找到了
                        ui_connect_form_photo_form_background_list_search.Add(ui_connect_form_photo_form_background_list.Item(a))

                    End If

                Next
            End If

            ui_form_photo_form_background_list.ItemsSource = ui_connect_form_photo_form_background_list_search

        End If
    End Sub

    '背景预览选择序号
    Private Sub ui_form_photo_form_background_select_preview_list(sender As Object, e As RoutedEventArgs)
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
            '获取序号
            Dim word As String = ui_connect_window_select_item_list.Item(ui_connect_window_select_item_list_select_index).pro_title
            word = Mid(word, 3, 1)

            '修改
            Dim bk As New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_" & word & "_Front.BMP")))
            ui_form_photo_form_background_bk_up.Background = bk
            bk = New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_" & word & "_Back.BMP")))
            ui_form_photo_form_background_bk_down.Background = bk
            bk = New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_" & word & "_Left.BMP")))
            ui_form_photo_form_background_bk_left.Background = bk
            bk = New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_" & word & "_Right.BMP")))
            ui_form_photo_form_background_bk_right.Background = bk
            bk = New ImageBrush(New BitmapImage(New Uri(ballance_start_path + "Textures\Sky\Sky_" & word & "_Down.BMP")))
            ui_form_photo_form_background_bk_middle.Background = bk

        End If
    End Sub

#End Region

#Region "音乐"

    '音效************************************************
    '添加音效
    Private Sub ui_form_wave_form_wave_add(sender As Object, e As RoutedEventArgs)
        If ui_form_wave_form_wave_list_form_search.Text = "" Then
            If open_wave_file.ShowDialog() = True Then
                If System.IO.File.Exists(Environment.CurrentDirectory & "\wave\" & open_wave_file.SafeFileName) = False Then
                    '不存在
                    System.IO.File.Copy(open_wave_file.FileName,
                                        Environment.CurrentDirectory & "\wave\" & open_wave_file.SafeFileName)

                    ui_form_wave_form_wave_list.ItemsSource = Nothing
                    Dim aaa As New ui_depend_form_wave_form_wave_list
                    aaa.pro_title = Mid(open_wave_file.SafeFileName, 1, open_wave_file.SafeFileName.Length - 4)
                    'TODO:未来：可以展示音效包的有关主题图片，现在先用无地图的样式--------------位置2
                    aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))
                    ui_connect_form_wave_form_wave_list.Add(aaa)
                    ui_form_wave_form_wave_list.ItemsSource = ui_connect_form_wave_form_wave_list

                    '更新ui-listbox
                    ui_update_list()
                Else
                    MsgBox("该音效包已存在，无法继续添加！", 16, "Ballance工具箱")
                End If
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    '删除音效
    Private Sub ui_form_wave_form_wave_del(sender As Object, e As RoutedEventArgs)
        If ui_form_wave_form_wave_list.SelectedIndex <> -1 And ui_form_wave_form_wave_list_form_search.Text = "" Then
            If MsgBox("确认删除音效包 " &
                              ui_connect_form_wave_form_wave_list.Item(ui_form_wave_form_wave_list.SelectedIndex).pro_title &
                              " 吗？", 32 + 1, "Ballance工具箱") = 1 Then
                '删除
                System.IO.File.Delete(Environment.CurrentDirectory & "\wave\" &
                                      ui_connect_form_wave_form_wave_list.Item(ui_form_wave_form_wave_list.SelectedIndex).pro_title & ".bwb")

                Dim linshi As Integer = ui_form_wave_form_wave_list.SelectedIndex
                ui_form_wave_form_wave_list.ItemsSource = Nothing
                ui_connect_form_wave_form_wave_list.RemoveAt(linshi)
                ui_form_wave_form_wave_list.ItemsSource = ui_connect_form_wave_form_wave_list

                '更新ui-listbox
                ui_update_list()

                MsgBox("删除完成", 64, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If
    End Sub

    '应用音效
    Private Sub ui_form_wave_form_wave_apply(sender As Object, e As RoutedEventArgs)
        If ui_form_wave_form_wave_list.SelectedIndex <> -1 And ui_form_wave_form_wave_list_form_search.Text = "" Then
            If MsgBox("确认应用音效包 " &
                              ui_connect_form_wave_form_wave_list.Item(ui_form_wave_form_wave_list.SelectedIndex).pro_title &
                              " 吗？", 32 + 1, "Ballance工具箱") = 1 Then

                '显示对话框

                Dim linshi = New Window_wait
                linshi.Owner = Me
                linshi.Show()

                '解压缩
                Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                aaa.ExtractZip(Environment.CurrentDirectory & "\wave\" &
                               ui_connect_form_wave_form_wave_list.Item(ui_form_wave_form_wave_list.SelectedIndex).pro_title & ".bwb",
                               ballance_start_path & "Sounds\", Nothing)

                linshi.Close()

                MsgBox("应用成功", 64, "Ballance工具箱")
            End If

        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If
    End Sub

    '还原为原版音效
    Private Sub ui_form_wave_form_wave_return(sender As Object, e As RoutedEventArgs)

        If MsgBox("确认还原？此操作将消耗一些时间，而且不可恢复", 32 + 1 + 256, "Ballance工具箱") = 1 Then
            If System.IO.File.Exists(Environment.CurrentDirectory & "\system_nmo\normal_wave.zip") = True Then

                '等待窗口
                Dim linshi = New Window_wait
                linshi.Owner = Me
                linshi.Show()

                Try
                    '解压缩
                    Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                    aaa.ExtractZip(Environment.CurrentDirectory & "\system_nmo\normal_wave.zip", ballance_start_path & "Sounds\", Nothing)
                Catch ex As Exception
                    MsgBox("还原时出现错误，无法还原！", 16, "Ballance工具箱")
                End Try

                linshi.Close()
            Else
                MsgBox("无可用的还原资源，需要从Ballance工具箱部署器部署有关资源才能执行还原", 16, "Ballance工具箱")
            End If
        End If

    End Sub

    '音效列表搜索
    Private Sub ui_form_wave_form_wave_list_form_search_change(sender As Object, e As TextChangedEventArgs) Handles ui_form_wave_form_wave_list_form_search.TextChanged
        If ui_form_wave_form_wave_list_form_search.Text = "" Then
            '不用搜索，强制还原
            ui_form_wave_form_wave_list.ItemsSource = Nothing
            ui_form_wave_form_wave_list.ItemsSource = ui_connect_form_wave_form_wave_list
            ui_connect_form_wave_form_wave_list_search.Clear()
        Else
            '需要搜索
            ui_form_wave_form_wave_list.ItemsSource = Nothing
            ui_connect_form_wave_form_wave_list_search.Clear()

            If ui_connect_form_wave_form_wave_list.Count <> 0 Then
                For a = 0 To ui_connect_form_wave_form_wave_list.Count - 1

                    If InStr(ui_connect_form_wave_form_wave_list.Item(a).pro_title, ui_form_wave_form_wave_list_form_search.Text, CompareMethod.Text) = 0 Then
                        '没找到 
                        '什么都不做
                    Else
                        '找到了
                        ui_connect_form_wave_form_wave_list_search.Add(ui_connect_form_wave_form_wave_list.Item(a))

                    End If

                Next
            End If

            ui_form_wave_form_wave_list.ItemsSource = ui_connect_form_wave_form_wave_list_search

        End If
    End Sub



    'bgm************************************************
    '添加bgm
    Private Sub ui_form_wave_form_bgm_add(sender As Object, e As RoutedEventArgs)
        If ui_form_wave_form_bgm_list_form_search.Text = "" Then
            If open_bgm_file.ShowDialog() = True Then
                If System.IO.File.Exists(Environment.CurrentDirectory & "\bgm\" & open_bgm_file.SafeFileName) = False Then
                    '不存在
                    System.IO.File.Copy(open_bgm_file.FileName,
                                        Environment.CurrentDirectory & "\bgm\" & open_bgm_file.SafeFileName)

                    ui_form_wave_form_bgm_list.ItemsSource = Nothing
                    Dim aaa As New ui_depend_form_wave_form_bgm_list
                    aaa.pro_title = Mid(open_bgm_file.SafeFileName, 1, open_bgm_file.SafeFileName.Length - 4)
                    'TODO:未来：可以展示bgm包的有关主题图片，现在先用无地图的样式--------------位置2
                    aaa.pro_image = New ImageBrush(New BitmapImage(New Uri(Environment.CurrentDirectory & "\cache\nothing.jpg")))
                    ui_connect_form_wave_form_bgm_list.Add(aaa)
                    ui_form_wave_form_bgm_list.ItemsSource = ui_connect_form_wave_form_bgm_list

                    '更新ui-listbox
                    ui_update_list()
                Else
                    MsgBox("该BGM包已存在，无法继续添加！", 16, "Ballance工具箱")
                End If
            End If
        Else
            MsgBox("不能在搜索状态下执行任务", 16, "Ballance工具箱")
        End If
    End Sub

    '删除bgm
    Private Sub ui_form_wave_form_bgm_del(sender As Object, e As RoutedEventArgs)
        If ui_form_wave_form_bgm_list.SelectedIndex <> -1 And ui_form_wave_form_bgm_list_form_search.Text = "" Then
            If MsgBox("确认删除BGM包 " &
                              ui_connect_form_wave_form_bgm_list.Item(ui_form_wave_form_bgm_list.SelectedIndex).pro_title &
                              " 吗？", 32 + 1, "Ballance工具箱") = 1 Then
                '删除
                System.IO.File.Delete(Environment.CurrentDirectory & "\bgm\" &
                                      ui_connect_form_wave_form_bgm_list.Item(ui_form_wave_form_bgm_list.SelectedIndex).pro_title & ".bgb")

                Dim linshi As Integer = ui_form_wave_form_bgm_list.SelectedIndex
                ui_form_wave_form_bgm_list.ItemsSource = Nothing
                ui_connect_form_wave_form_bgm_list.RemoveAt(linshi)
                ui_form_wave_form_bgm_list.ItemsSource = ui_connect_form_wave_form_bgm_list

                '更新ui-listbox
                ui_update_list()

                MsgBox("删除完成", 64, "Ballance工具箱")
            End If
        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If
    End Sub

    '应用bgm
    Private Sub ui_form_wave_form_bgm_apply(sender As Object, e As RoutedEventArgs)
        If ui_form_wave_form_bgm_list.SelectedIndex <> -1 And ui_form_wave_form_bgm_list_form_search.Text = "" Then
            If MsgBox("确认应用BGM包 " &
                             ui_connect_form_wave_form_bgm_list.Item(ui_form_wave_form_bgm_list.SelectedIndex).pro_title &
                             " 吗？", 32 + 1, "Ballance工具箱") = 1 Then

                '显示对话框

                Dim linshi = New Window_wait
                linshi.Owner = Me
                linshi.Show()

                '解压缩
                Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                aaa.ExtractZip(Environment.CurrentDirectory & "\bgm\" &
                           ui_connect_form_wave_form_bgm_list.Item(ui_form_wave_form_bgm_list.SelectedIndex).pro_title & ".bgb",
                           ballance_start_path & "Sounds\", Nothing)

                linshi.Close()

                MsgBox("应用成功", 64, "Ballance工具箱")

                If MsgBox("已成功应用BGM包，BGM包要在游戏中应用需要重启程序一次，是否立即关闭程序，手动重启？", 32 + 1, "Ballance工具箱") = 1 Then
                    Environment.Exit(0)
                End If

            End If

        Else
            MsgBox("不能在搜索状态下执行任务，或者是没有选中任何项", 16, "Ballance工具箱")
        End If

    End Sub

    '还原为原版bgm
    Private Sub ui_form_wave_form_bgm_return(sender As Object, e As RoutedEventArgs)

        If MsgBox("确认还原？此操作将消耗一些时间，而且不可恢复", 32 + 1 + 256, "Ballance工具箱") = 1 Then
            If System.IO.File.Exists(Environment.CurrentDirectory & "\system_nmo\normal_bgm.zip") = True Then

                '等待窗口
                Dim linshi = New Window_wait
                linshi.Owner = Me
                linshi.Show()

                Try
                    '解压缩
                    Dim aaa As New ICSharpCode.SharpZipLib.Zip.FastZip
                    aaa.ExtractZip(Environment.CurrentDirectory & "\system_nmo\normal_bgm.zip", ballance_start_path & "Sounds\", Nothing)
                Catch ex As Exception
                    MsgBox("还原时出现错误，无法还原！", 16, "Ballance工具箱")
                End Try

                linshi.Close()
            Else
                MsgBox("无可用的还原资源，需要从Ballance工具箱部署器部署有关资源才能执行还原", 16, "Ballance工具箱")
            End If
        End If

    End Sub

    'bgm列表搜索
    Private Sub ui_form_wave_form_bgm_list_form_search_change(sender As Object, e As TextChangedEventArgs) Handles ui_form_wave_form_bgm_list_form_search.TextChanged
        If ui_form_wave_form_bgm_list_form_search.Text = "" Then
            '不用搜索，强制还原
            ui_form_wave_form_bgm_list.ItemsSource = Nothing
            ui_form_wave_form_bgm_list.ItemsSource = ui_connect_form_wave_form_bgm_list
            ui_connect_form_wave_form_bgm_list_search.Clear()
        Else
            '需要搜索
            ui_form_wave_form_bgm_list.ItemsSource = Nothing
            ui_connect_form_wave_form_bgm_list_search.Clear()

            If ui_connect_form_wave_form_bgm_list.Count <> 0 Then
                For a = 0 To ui_connect_form_wave_form_bgm_list.Count - 1

                    If InStr(ui_connect_form_wave_form_bgm_list.Item(a).pro_title, ui_form_wave_form_bgm_list_form_search.Text, CompareMethod.Text) = 0 Then
                        '没找到 
                        '什么都不做
                    Else
                        '找到了
                        ui_connect_form_wave_form_bgm_list_search.Add(ui_connect_form_wave_form_bgm_list.Item(a))

                    End If

                Next
            End If

            ui_form_wave_form_bgm_list.ItemsSource = ui_connect_form_wave_form_bgm_list_search

        End If
    End Sub

    '选择bgm序号
    Private Sub ui_form_wave_form_bgm_select(sender As Object, e As RoutedEventArgs)

        '停止音乐
        Try
            play_bgm.Stop()
            play_bgm.Dispose()
        Catch ex As Exception

        End Try

        Dim aaa As New ui_depend_window_select_item_list
        ui_connect_window_select_item_list.Clear()
        ui_connect_window_select_item_list_select_index = 0

        aaa.pro_title = "主题 1"
        aaa.pro_text = "第 1、5、10 关BGM"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "主题 2"
        aaa.pro_text = "第 3、8 关BGM"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "主题 3"
        aaa.pro_text = "第 4、9、11 关BGM"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "主题 4"
        aaa.pro_text = "第 7、12 关BGM"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "主题 5"
        aaa.pro_text = "第 2、6、13 关BGM"
        ui_connect_window_select_item_list.Add(aaa)

        '显示对话框

        Dim linshi = New Window_select_item
        ui_connect_window_select_item_list_select_index = -1
        linshi.Owner = Me
        linshi.ShowDialog()

        '安装
        If ui_connect_window_select_item_list_select_index <> -1 Then
            Dim linshi2 As Integer = ui_connect_window_select_item_list_select_index + 1

            ui_form_wave_form_bgm_play_title_1.Text = "Music_Theme_" & linshi2 & "_1.wav"
            ui_form_wave_form_bgm_play_title_2.Text = "Music_Theme_" & linshi2 & "_2.wav"
            ui_form_wave_form_bgm_play_title_3.Text = "Music_Theme_" & linshi2 & "_3.wav"

        End If
    End Sub

    '播放第1个bgm
    Private Sub ui_form_wave_form_bgm_play_1(sender As Object, e As MouseButtonEventArgs)
        Try
            play_bgm.Stop()
            play_bgm.Dispose()
        Catch ex As Exception

        End Try
        play_bgm = New System.Media.SoundPlayer(ballance_start_path & "Sounds\" & ui_form_wave_form_bgm_play_title_1.Text)
        play_bgm.Play()
    End Sub
    '播放第2个bgm
    Private Sub ui_form_wave_form_bgm_play_2(sender As Object, e As MouseButtonEventArgs)
        Try
            play_bgm.Stop()
            play_bgm.Dispose()
        Catch ex As Exception

        End Try
        play_bgm = New System.Media.SoundPlayer(ballance_start_path & "Sounds\" & ui_form_wave_form_bgm_play_title_2.Text)
        play_bgm.Play()
    End Sub
    '播放第3个bgm
    Private Sub ui_form_wave_form_bgm_play_3(sender As Object, e As MouseButtonEventArgs)
        Try
            play_bgm.Stop()
            play_bgm.Dispose()
        Catch ex As Exception

        End Try
        play_bgm = New System.Media.SoundPlayer(ballance_start_path & "Sounds\" & ui_form_wave_form_bgm_play_title_3.Text)
        play_bgm.Play()
    End Sub

#End Region

#Region "设置"

    '基础设置************************************************
    '全屏
    Private Sub ui_form_setting_form_1_fullscreen_btn_click(sender As Object, e As RoutedEventArgs) Handles ui_form_setting_form_1_fullscreen_btn.Click

        '读取注册表

        Dim Key As RegistryKey = Registry.LocalMachine
        Dim b_full_screen As RegistryKey
        Dim info As String = ""
        Try
            If Environment.Is64BitOperatingSystem = True Then
                '64位系统
                b_full_screen = Key.OpenSubKey("SOFTWARE\\Wow6432Node\\ballance\\Settings", True)
            Else
                '32位
                b_full_screen = Key.OpenSubKey("SOFTWARE\\ballance\\Settings", True)
            End If

            info = b_full_screen.GetValue("Fullscreen").ToString

            If info = "0" Then
                b_full_screen.SetValue("Fullscreen", "1", RegistryValueKind.DWord)
                ui_form_setting_form_1_fullscreen.Text = "当前全屏状态：全屏"
                ui_form_setting_form_1_fullscreen_btn.Content = "修改为 窗口化"
            Else
                b_full_screen.SetValue("Fullscreen", "0", RegistryValueKind.DWord)
                ui_form_setting_form_1_fullscreen.Text = "当前全屏状态：窗口化"
                ui_form_setting_form_1_fullscreen_btn.Content = "修改为 全屏"
            End If

            MsgBox("成功修改", 64, "Ballance工具箱")

        Catch ex As Exception

            '错误意味着没有注册表项，默认一切，写注册表

            ui_form_setting_form_1_fullscreen.Text = "当前全屏状态：全屏"
            ui_form_setting_form_1_fullscreen_btn.Content = "修改为 窗口化"

            Dim b_full_screen_2 As RegistryKey
            Dim b_full_screen_3 As RegistryKey

            If Environment.Is64BitOperatingSystem = True Then
                '64位系统
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\\Wow6432Node\\ballance")
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\\Wow6432Node\\ballance\\Settings")

                b_full_screen_3 = Key.OpenSubKey("SOFTWARE\\Wow6432Node\\ballance\\Settings", True)
                b_full_screen_3.SetValue("Fullscreen", "1", RegistryValueKind.DWord)

            Else
                '32位
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\\ballance")
                b_full_screen_2 = Key.CreateSubKey("SOFTWARE\\ballance\\Settings")

                b_full_screen_3 = Key.OpenSubKey("SOFTWARE\\ballance\\Settings", True)
                b_full_screen_3.SetValue("Fullscreen", "1", RegistryValueKind.DWord)

            End If

            MsgBox("在操作注册表中出现了错误，已经将窗口模式调整为 全屏 ！", 16, "Ballance工具箱")

        End Try

    End Sub

    '修改语言
    Private Sub ui_form_setting_form_1_language_change(sender As Object, e As RoutedEventArgs)

        Dim aaa As New ui_depend_window_select_item_list
        ui_connect_window_select_item_list.Clear()
        ui_connect_window_select_item_list_select_index = 0

        aaa.pro_title = "德语"
        aaa.pro_text = "Deutsch"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "英语"
        aaa.pro_text = "Endlish"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "西班牙语"
        aaa.pro_text = "El español"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "意大利语"
        aaa.pro_text = "In Italiano"
        ui_connect_window_select_item_list.Add(aaa)
        aaa = New ui_depend_window_select_item_list
        aaa.pro_title = "法语"
        aaa.pro_text = "Français"
        ui_connect_window_select_item_list.Add(aaa)

        '显示对话框

        Dim linshi = New Window_select_item
        ui_connect_window_select_item_list_select_index = -1
        linshi.Owner = Me
        linshi.ShowDialog()

        '安装
        If ui_connect_window_select_item_list_select_index <> -1 Then

            '修改注册表
            Dim Key As RegistryKey = Registry.LocalMachine
            Dim select_key As RegistryKey

            Try
                '判断
                If Environment.Is64BitOperatingSystem = True Then
                    '64位系统
                    select_key = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)
                Else
                    '32位
                    select_key = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)
                End If

                '写入
                Dim linshi_1 As String = ui_connect_window_select_item_list_select_index + 1
                select_key.SetValue("Language", linshi_1, RegistryValueKind.DWord)

                '写入ui
                Select Case ui_connect_window_select_item_list_select_index
                    Case 0
                        ui_form_setting_form_1_language.Text = "当前语言状态：德语（Deutsch）"
                    Case 1
                        ui_form_setting_form_1_language.Text = "当前语言状态：英语（Endlish）"
                    Case 2
                        ui_form_setting_form_1_language.Text = "当前语言状态：西班牙语（El español）"
                    Case 3
                        ui_form_setting_form_1_language.Text = "当前语言状态：意大利语（In Italiano）"
                    Case 4
                        ui_form_setting_form_1_language.Text = "当前语言状态：法语（Français）"
                    Case Else
                        ui_form_setting_form_1_language.Text = "当前语言状态：未知（Unknow）"
                End Select

                MsgBox("修改语言成功！", 64, "Ballance工具箱")

            Catch ex As Exception
                MsgBox("将语言选择写入注册表失败！", 16, "Ballance工具箱")
            End Try

        End If

    End Sub

    '修改分辨率
    Private Sub ui_form_setting_form_1_size_change(sender As Object, e As RoutedEventArgs)

        Dim w_width, w_height As Integer
        Try
            w_width = CType(ui_form_setting_form_1_size_width.Text, Integer)
            w_height = CType(ui_form_setting_form_1_size_height.Text, Integer)
        Catch ex As Exception
            '错误，跳出
            MsgBox("分辨率数值不合要求", 16, "Ballance工具箱")
            Exit Sub
        End Try

        If w_width >= 0 And w_width <= 65535 And w_height >= 0 And w_height <= 65535 Then

            If MsgBox("请确认你要修改的分辨率是支持的，否则游戏无法正常工作，建议在非必要时都使用游戏内置的分辨率修改器！确认修改？", 32 + 256 + 1, "Ballance工具箱") = 1 Then

                '修改注册表
                Dim Key As RegistryKey = Registry.LocalMachine
                Dim select_key As RegistryKey

                Try
                    '判断
                    If Environment.Is64BitOperatingSystem = True Then
                        '64位系统
                        select_key = Key.OpenSubKey("SOFTWARE\Wow6432Node\ballance\Settings", True)
                    Else
                        '32位
                        select_key = Key.OpenSubKey("SOFTWARE\ballance\Settings", True)
                    End If

                    '写入,写入值由于是16进制，转换公式：分辨率宽*67108864+分辨率高
                    Dim linshi_1 As String = w_width.ToString("X4") & w_height.ToString("X4")
                    Dim linshi_2 As String = Long.Parse(linshi_1, System.Globalization.NumberStyles.HexNumber)
                    select_key.SetValue("VideoMode", linshi_2, RegistryValueKind.DWord)

                    MsgBox("修改分辨率成功！", 64, "Ballance工具箱")

                Catch ex As Exception
                    MsgBox("将分辨率选择写入注册表失败！", 16, "Ballance工具箱")
                End Try

            End If

        Else
            '错误，跳出
            MsgBox("分辨率数值不合要求", 16, "Ballance工具箱")
        End If

    End Sub

    '高级设置************************************************
    '读取本地分表
    Private Sub ui_form_setting_form_local_hero_read(sender As Object, e As RoutedEventArgs)

        Dim db As ScoreManager.IO.Database = ScoreManager.IO.DBReader.ReadDB(ballance_start_path & "Database.tdb")
        Dim aaa As New ui_depend_form_setting_form_local_hero_list
        ui_form_setting_form_local_hero_list.ItemsSource = Nothing
        ui_connect_form_setting_form_local_hero_list.Clear()

        For a = 0 To 12
            For b = 0 To 9
                aaa.pro_level = "第" & db.HighScores(a).LevelIndex & "关"
                aaa.pro_name = db.HighScores(a).Play(b).Player
                aaa.pro_score = db.HighScores(a).Play(b).Points

                ui_connect_form_setting_form_local_hero_list.Add(aaa)
                aaa = New ui_depend_form_setting_form_local_hero_list
            Next
        Next

        ui_form_setting_form_local_hero_list.ItemsSource = ui_connect_form_setting_form_local_hero_list
        ui_form_setting_form_local_hero_list.Items.GroupDescriptions.Clear()
        ui_form_setting_form_local_hero_list.Items.GroupDescriptions.Add(New PropertyGroupDescription("pro_level"))

        MsgBox("读取本地成绩列表完成", 64, "Ballance工具箱")

        '更新ui-listbox
        ui_update_list()

    End Sub
    '关卡全开
    Private Sub ui_form_setting_form_local_hero_open_all(sender As Object, e As RoutedEventArgs)
        If MsgBox("确认关卡全开吗？这样导致你的成绩全部消失而且不可恢复！", 32 + 1, "Ballance工具箱") = 1 Then
            System.IO.File.Delete(ballance_start_path & "Database.tdb")
            System.IO.File.Copy(Environment.CurrentDirectory & "\system_nmo\open.tdb", ballance_start_path & "Database.tdb")

            MsgBox("关卡全开成功！", 64, "Ballane")
        End If

    End Sub


    '新备份
    Private Sub ui_form_setting_form_local_hero_list_new(sender As Object, e As RoutedEventArgs)
        Dim file_name As String = Year(Now) & "年" & Month(Now) & "月" & Day(Now) & "日" & Hour(Now) & "时" & Minute(Now) & "分" & Second(Now) & "秒备份"
        System.IO.File.Copy(ballance_start_path & "Database.tdb",
                             Environment.CurrentDirectory & "\backups\" &
                             file_name & ".tdb")

        ui_form_setting_form_local_hero_backups_list.ItemsSource = Nothing
        Dim aaa As New ui_depend_form_setting_form_local_hero_backups_list
        aaa.pro_title = file_name
        ui_connect_form_setting_form_local_hero_backups_list.Add(aaa)
        ui_form_setting_form_local_hero_backups_list.ItemsSource = ui_connect_form_setting_form_local_hero_backups_list

        '更新ui-listbox
        ui_update_list()
    End Sub

    '删除备份
    Private Sub ui_form_setting_form_local_hero_list_del(sender As Object, e As RoutedEventArgs)
        If ui_form_setting_form_local_hero_backups_list.SelectedIndex <> -1 Then
            System.IO.File.Delete(Environment.CurrentDirectory & "\backups\" &
                             ui_connect_form_setting_form_local_hero_backups_list.Item(ui_form_setting_form_local_hero_backups_list.SelectedIndex).pro_title & ".tdb")

            Dim list_select As Integer = ui_form_setting_form_local_hero_backups_list.SelectedIndex
            ui_form_setting_form_local_hero_backups_list.ItemsSource = Nothing
            ui_connect_form_setting_form_local_hero_backups_list.RemoveAt(list_select)
            ui_form_setting_form_local_hero_backups_list.ItemsSource = ui_connect_form_setting_form_local_hero_backups_list

            '更新ui-listbox
            ui_update_list()
        Else
            MsgBox("没有选中任何项", 16, "Ballance工具箱")
        End If
    End Sub

    '应用备份
    Private Sub ui_form_setting_form_local_hero_list_apply(sender As Object, e As RoutedEventArgs)
        If ui_form_setting_form_local_hero_backups_list.SelectedIndex <> -1 Then
            System.IO.File.Delete(ballance_start_path & "Database.tdb")
            System.IO.File.Copy(Environment.CurrentDirectory & "\backups\" &
                                ui_connect_form_setting_form_local_hero_backups_list.Item(ui_form_setting_form_local_hero_backups_list.SelectedIndex).pro_title & ".tdb",
                                ballance_start_path & "Database.tdb")

            '更新ui-listbox
            ui_update_list()

        Else
            MsgBox("没有选中任何项", 16, "Ballance工具箱")
        End If

    End Sub


#End Region

#Region "联网对战"

#End Region

#Region "关于"

    ''' <summary>
    ''' 关于界面的跟随我们
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_form_about_follow(sender As Object, e As MouseButtonEventArgs)
        System.Diagnostics.Process.Start("http://tieba.baidu.com/f?kw=ballance")
    End Sub

    ''' <summary>
    ''' 关于界面的开源地址
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_form_about_open(sender As Object, e As MouseButtonEventArgs)
        System.Diagnostics.Process.Start("https://github.com/yyc12345/ballance_tools")
    End Sub

#End Region

#Region "其他"

    '对于全局listbox没有项进行判断
    Public Sub ui_update_list()

        '**********地图*************
        If ui_form_level_form_check_list.Items.Count = 0 Then
            ui_form_level_form_check_list_empty.Opacity = 1
        Else
            ui_form_level_form_check_list_empty.Opacity = 0
        End If

        If ui_form_level_form_level_list.Items.Count = 0 Then
            ui_form_level_form_level_list_empty.Opacity = 1
        Else
            ui_form_level_form_level_list_empty.Opacity = 0
        End If

        If ui_form_level_form_hero_list.Items.Count = 0 Then
            ui_form_level_form_hero_list_empty.Opacity = 1
        Else
            ui_form_level_form_hero_list_empty.Opacity = 0
        End If

        '**********nmo*************
        If ui_form_nmo_form_mod_mod_list.Items.Count = 0 Then
            ui_form_nmo_form_mod_mod_list_empty.Opacity = 1
        Else
            ui_form_nmo_form_mod_mod_list_empty.Opacity = 0
        End If

        If ui_form_nmo_form_mod_ph_list.Items.Count = 0 Then
            ui_form_nmo_form_mod_ph_list_empty.Opacity = 1
        Else
            ui_form_nmo_form_mod_ph_list_empty.Opacity = 0
        End If

        '**********材质*************
        If ui_form_photo_form_material_list.Items.Count = 0 Then
            ui_form_photo_form_material_list_empty.Opacity = 1
        Else
            ui_form_photo_form_material_list_empty.Opacity = 0
        End If

        If ui_form_photo_form_background_list.Items.Count = 0 Then
            ui_form_photo_form_background_list_empty.Opacity = 1
        Else
            ui_form_photo_form_background_list_empty.Opacity = 0
        End If

        '**********音乐*************
        If ui_form_wave_form_wave_list.Items.Count = 0 Then
            ui_form_wave_form_wave_list_empty.Opacity = 1
        Else
            ui_form_wave_form_wave_list_empty.Opacity = 0
        End If


        If ui_form_wave_form_bgm_list.Items.Count = 0 Then
            ui_form_wave_form_bgm_list_empty.Opacity = 1
        Else
            ui_form_wave_form_bgm_list_empty.Opacity = 0
        End If

        '**********设置*************
        If ui_form_setting_form_local_hero_list.Items.Count = 0 Then
            ui_form_setting_form_local_hero_list_empty.Opacity = 1
        Else
            ui_form_setting_form_local_hero_list_empty.Opacity = 0
        End If

        If ui_form_setting_form_local_hero_backups_list.Items.Count = 0 Then
            ui_form_setting_form_local_hero_backups_list_empty.Opacity = 1
        Else
            ui_form_setting_form_local_hero_backups_list_empty.Opacity = 0
        End If

    End Sub


#End Region


End Class
