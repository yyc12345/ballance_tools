Class MainWindow

#Region "预定义"

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

    Private Sub window_move(sender As Object, e As MouseButtonEventArgs)

        ReleaseCapture()
        SendMessage(New Interop.WindowInteropHelper(MainWindow_name).Handle, &HA1, 2, 0)

    End Sub

    ''' <summary>
    ''' 初始化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub app_init(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded

        'Dim aaa As New ui.ui_depend_user_list
        'For a = 0 To 20
            'aaa.pro_name = "wssb"
            'aaa.pro_ip = "0.0.0.0"
            'aaa.pro_port = "0000"
            'aaa.pro_state = "作弊"
            'aaa.pro_bk = New SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
            'ui.ui_connect_user_list.Add(aaa)
            'aaa = New ui.ui_depend_user_list
        'Next

        '设置源
        ui_user_list.ItemsSource = ui.ui_use.ui_connect_user_list

    End Sub

End Class
