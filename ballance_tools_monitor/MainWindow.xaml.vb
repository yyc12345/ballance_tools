Imports ballance_tools_hook

Class MainWindow

#Region "win32函数"
    ''' <summary>
    ''' [系统][ui]向指定窗口的消息循环队列中发送消息
    ''' </summary>
    ''' <param name="hwnd">指定窗口的句柄</param>
    ''' <param name="wMsg">发送内容</param>
    ''' <param name="wParam">附加1</param>
    ''' <param name="lParam">附加2</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

    ''' <summary>
    ''' [系统][ui]取消鼠标按下状态
    ''' </summary>
    ''' <remarks></remarks>
    Public Declare Sub ReleaseCapture Lib "User32" ()

#End Region

#Region "计时器"

    ''' <summary>
    ''' 获取cpu/内存占用的计时器
    ''' </summary>
    ''' <remarks></remarks>
    Public system_timer As New Windows.Threading.DispatcherTimer

    ''' <summary>
    ''' 获取键盘按键的计时器
    ''' </summary>
    ''' <remarks></remarks>
    Public keyboard_timer As New Windows.Threading.DispatcherTimer


#End Region

    ''' <summary>
    ''' 获取cpu占用的函数
    ''' </summary>
    Public cpu_count As System.Diagnostics.PerformanceCounter

    ''' <summary>
    ''' 总内存大小
    ''' </summary>
    Public all_memory As Long = 0

    ''' <summary>
    ''' 当前使用的内存大小
    ''' </summary>
    Public use_memory As Long = 0

    ''' <summary>
    ''' 键盘hook
    ''' </summary>
    Public kh As ballance_tools_hook.KeyboardHook

    ''' <summary>
    ''' 准备状态-true=正在hook false-暂停
    ''' </summary>
    Public ready_state As Boolean = True

#Region "键盘计数"
    Public enter_press As Integer = 0
    Public up_press As Integer = 0
    Public down_press As Integer = 0
    Public left_press As Integer = 0
    Public right_press As Integer = 0
    Public z_press As Integer = 0
    Public esc_press As Integer = 0
    Public space_press As Integer = 0

    ''' <summary>
    ''' 之前的检测文字
    ''' </summary>
    Public before_word As String = ""
    ''' <summary>
    ''' 警告
    ''' </summary>
    Public warning_word As String = ""

    '预定义的允许的键值名
    Public allow_key As ArrayList

#End Region


#Region "计时器刷新函数"

    ''' <summary>
    ''' cpu/内存的计时器刷新函数
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub system_timer_function(ByVal sender As Object, ByVal e As EventArgs)

        'get free memory
        Dim mos As System.Management.ManagementClass = New Management.ManagementClass("Win32_OperatingSystem")
        For Each mo As System.Management.ManagementObject In mos.GetInstances
            If mo("FreePhysicalMemory") <> Nothing Then
                use_memory = 1024 * Long.Parse(mo("FreePhysicalMemory").ToString)
            End If
        Next

        Dim memory_value As Double = Int((use_memory / all_memory) * 100)
        ui_memory_progress.Value = memory_value
        ui_memory_progress_text.Text = memory_value.ToString & "%"

        Dim cpu_value As Double = Int(cpu_count.NextValue.ToString)
        ui_cpu_progress.Value = cpu_value
        ui_cpu_progress_text.Text = cpu_value.ToString & "%"

    End Sub

    ''' <summary>
    ''' 键盘函数
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub keyboard_timer_function(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

        If e.KeyData = System.Windows.Forms.Keys.Enter Then
            enter_press += 1
        End If
        If e.KeyData = System.Windows.Forms.Keys.Right Then
            right_press += 1
        End If
        If e.KeyData = System.Windows.Forms.Keys.Left Then
            left_press += 1
        End If
        If e.KeyData = System.Windows.Forms.Keys.Up Then
            up_press += 1
        End If
        If e.KeyData = System.Windows.Forms.Keys.Down Then
            down_press += 1
        End If
        If e.KeyData = System.Windows.Forms.Keys.Z Then
            z_press += 1
        End If
        If e.KeyData = System.Windows.Forms.Keys.Escape Then
            esc_press += 1
        End If
        If e.KeyData = System.Windows.Forms.Keys.Space Then
            space_press += 1
        End If

        If allow_key.Contains(e.KeyData.ToString) = False Then
            '元素不在,发出警告，并且刷新界面
            warning_word = warning_word & vbCrLf & "未允许的按键---" & e.KeyData.ToString
            ui_message.Text = warning_word
        End If



    End Sub

#End Region


    Private Sub window_move(sender As Object, e As MouseButtonEventArgs)

        ReleaseCapture()
        SendMessage(New Interop.WindowInteropHelper(MainWindow_name).Handle, &HA1, 2, 0)

    End Sub

    ''' <summary>
    ''' 初始化应用
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub app_init(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded

        'init_cpu
        cpu_count = New PerformanceCounter("Processor", "% Processor Time", "_Total")
        cpu_count.MachineName = "."
        cpu_count.NextValue()

        'init_memory
        Dim mc As System.Management.ManagementClass = New System.Management.ManagementClass("Win32_ComputerSystem")
        Dim moc As System.Management.ManagementObjectCollection = mc.GetInstances
        For Each mo As Management.ManagementObject In moc
            If mo("TotalPhysicalMemory") <> Nothing Then
                all_memory = Long.Parse(mo("TotalPhysicalMemory").ToString)
            End If
        Next

        'biling function
        system_timer = New Windows.Threading.DispatcherTimer
        system_timer.Interval = TimeSpan.FromMilliseconds(500)
        AddHandler system_timer.Tick, AddressOf system_timer_function
        system_timer.Start()

        'keyboard_timer = New Windows.Threading.DispatcherTimer
        'keyboard_timer.Interval = TimeSpan.FromSeconds(5)
        'AddHandler keyboard_timer.Tick, AddressOf keyboard_timer_function
        'keyboard_timer.Start()

        'init_hook
        kh = New ballance_tools_hook.KeyboardHook
        kh.SetHook()
        AddHandler kh.OnKeyDownEvent, AddressOf keyboard_timer_function

        allow_key = New ArrayList
        allow_key.Add(System.Windows.Forms.Keys.Enter.ToString)
        allow_key.Add(System.Windows.Forms.Keys.Z.ToString)
        allow_key.Add(System.Windows.Forms.Keys.Right.ToString)
        allow_key.Add(System.Windows.Forms.Keys.Down.ToString)
        allow_key.Add(System.Windows.Forms.Keys.Up.ToString)
        allow_key.Add(System.Windows.Forms.Keys.Left.ToString)
        allow_key.Add(System.Windows.Forms.Keys.Escape.ToString)
        allow_key.Add(System.Windows.Forms.Keys.Space.ToString)

        ready_state = True

    End Sub

    ''' <summary>
    ''' 暂停/继续
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_continue(sender As Object, e As RoutedEventArgs)

        If ready_state = True Then
            '暂停

            '改颜色
            ui_message.Foreground = New SolidColorBrush(Color.FromArgb(255, 255, 255, 255))

            '写入内容
            If before_word <> "" Then
                before_word = before_word & vbCrLf
            End If
            before_word = before_word & "按下的 Enter 键次数：" & enter_press & vbCrLf
            before_word = before_word & "按下的 上 键次数：" & up_press & vbCrLf
            before_word = before_word & "按下的 下 键次数：" & down_press & vbCrLf
            before_word = before_word & "按下的 左 键次数：" & left_press & vbCrLf
            before_word = before_word & "按下的 右 键次数：" & right_press & vbCrLf
            before_word = before_word & "按下的 Z 键次数：" & z_press & vbCrLf
            before_word = before_word & "按下的 Esc 键次数：" & esc_press & vbCrLf
            before_word = before_word & "按下的 空格 键次数：" & space_press & vbCrLf
            before_word = before_word & "未允许按键记录：" & warning_word & vbCrLf
            before_word = before_word & "记录时间：" & Now() & vbCrLf

            enter_press = 0
            up_press = 0
            down_press = 0
            left_press = 0
            right_press = 0
            z_press = 0
            esc_press = 0
            space_press = 0
            warning_word = ""

            ui_message.Text = before_word

            '暂停hook
            kh.UnHook()

        Else
            '继续

            '改颜色
            ui_message.Foreground = New SolidColorBrush(Color.FromArgb(255, 255, 0, 0))

            ui_message.Text = warning_word

            '启动hook
            kh.SetHook()

        End If

        ready_state = Not (ready_state)

    End Sub

    ''' <summary>
    ''' 结束
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_stop(sender As Object, e As RoutedEventArgs)

        If ready_state = False Then

            'TODO:未来放传输数据到ballance_tools里

            Environment.Exit(0)

        Else
            MsgBox("您必须先暂停才能结束！", 16, "Ballance监测")
        End If

    End Sub

    ''' <summary>
    ''' 退出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub app_exit(sender As Object, e As EventArgs) Handles MyBase.Closed
        '解除hook
        kh.UnHook()
    End Sub
End Class
