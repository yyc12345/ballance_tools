Class MainWindow

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

    Private Sub window_move(sender As Object, e As MouseButtonEventArgs)

        ReleaseCapture()
        SendMessage(New Interop.WindowInteropHelper(MainWindow_name).Handle, &HA1, 2, 0)

    End Sub

End Class
