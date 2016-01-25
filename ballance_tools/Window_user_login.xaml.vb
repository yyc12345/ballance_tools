Public Class Window_user_login
    ''' <summary>
    ''' 确认
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_window_user_login_login(sender As Object, e As RoutedEventArgs)

        If ui_window_user_login_user_name.Text <> "" And ui_window_user_login_user_password.Password <> "" Then
            user_name = ui_window_user_login_user_name.Text
            user_password = ui_window_user_login_user_password.Password
            Me.Close()
        Else
            MsgBox("用户名或密码不得为空", 16, "Ballance工具箱")
        End If

    End Sub

    ''' <summary>
    ''' 注册
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_window_user_login_get_user(sender As Object, e As RoutedEventArgs)
        '调用浏览器
        System.Diagnostics.Process.Start("http://jxtoolbox.sinaapp.com/baiduRegister.php")
    End Sub

    ''' <summary>
    ''' 跳过
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_window_user_login_jump(sender As Object, e As RoutedEventArgs)
        If MsgBox("确认跳过？这将使你无法查看英雄榜、留言，而且该窗口会反复弹出", 32 + 1, "Ballance工具箱") = MsgBoxResult.Ok Then
            Me.Close()
        End If
    End Sub
End Class
