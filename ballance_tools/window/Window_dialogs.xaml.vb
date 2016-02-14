Public Class Window_dialogs

    ''' <summary>
    ''' 初始化窗口
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub window_open(sender As Object, e As RoutedEventArgs)

        Me.Title = window_dialogs_title
        ui_title.Text = window_dialogs_title
        ui_message.Text = window_dialogs_message

        ui_left_btn.Content = window_dialogs_left_btn_text
        ui_right_btn.Content = window_dialogs_right_btn_text

        '按钮个数
        If window_dialogs_btn_count = 1 Then
            ui_btn_width_1.Width = New GridLength(0)
            ui_btn_width_2.Width = New GridLength(0)
        End If

        '输入框
        If window_dialogs_show_inputbox = False Then
            ui_btn_height_1.Height = New GridLength(0)
            ui_btn_height_2.Height = New GridLength(0)
        End If

        System.Media.SystemSounds.Beep.Play()
        Select Case window_dialogs_msg_state
            Case 0
                ui_title_bk.Color = Color.FromArgb(255, 30, 144, 255)
            Case 1
                ui_title_bk.Color = Color.FromArgb(255, 255, 165, 0)
            Case 2
                ui_title_bk.Color = Color.FromArgb(255, 220, 20, 60)
            Case Else
                ui_title_bk.Color = Color.FromArgb(255, 30, 144, 255)
        End Select

    End Sub

    Private Sub ui_left_btn_click(sender As Object, e As RoutedEventArgs) Handles ui_left_btn.Click

        window_dialogs_select_btn = 0
        window_dialogs_input_text = ui_input_text.Text
        Me.Close()

    End Sub

    Private Sub ui_right_btn_click(sender As Object, e As RoutedEventArgs) Handles ui_right_btn.Click

        window_dialogs_select_btn = 1
        window_dialogs_input_text = ui_input_text.Text
        Me.Close()

    End Sub

    ''' <summary>
    ''' 窗口移动
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub window_move(sender As Object, e As MouseButtonEventArgs)

        ReleaseCapture()
        SendMessage(New Interop.WindowInteropHelper(window_dialogs_name).Handle, &HA1, 2, 0)

    End Sub

End Class
