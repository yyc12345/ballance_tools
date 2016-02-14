Public Class Window_select_item
    ''' <summary>
    ''' 窗口加载
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub window_select_item_load(sender As Object, e As RoutedEventArgs)
        '对列表的模板进行设置************************************************************************************************
        ui_window_select_item_list.ItemTemplate = CType(Application.Current.Resources("ui_temp_window_select_item_list"), DataTemplate)
        ui_window_select_item_list.ItemsSource = ui_connect_window_select_item_list
        ui_title.Text = ui_connect_window_select_item_list_title

    End Sub

    ''' <summary>
    ''' 选择的项变化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub item_change(sender As Object, e As SelectionChangedEventArgs) Handles ui_window_select_item_list.SelectionChanged

        If ui_window_select_item_list.SelectedIndex <> -1 And ui_connect_window_select_item_list_select_index <> ui_window_select_item_list.SelectedIndex Then

            ui_connect_window_select_item_list_select_index = ui_window_select_item_list.SelectedIndex

            '处理数据
            Dim title As String = ""
            Dim word As String = ""
            Dim linshi As ui_depend_window_select_item_list = New ui_depend_window_select_item_list
            ui_window_select_item_list.ItemsSource = Nothing
            For a = 0 To ui_connect_window_select_item_list.Count - 1

                linshi = New ui_depend_window_select_item_list

                title = ui_connect_window_select_item_list.Item(a).pro_title
                word = ui_connect_window_select_item_list.Item(a).pro_text
                ui_connect_window_select_item_list.RemoveAt(a)
                linshi.pro_title = title
                linshi.pro_text = word

                If a = ui_connect_window_select_item_list_select_index Then
                    '选中，填充画刷
                    linshi.pro_fill = New SolidColorBrush(Color.FromArgb(255, 30, 144, 255))
                Else
                    '未选中，填充画刷
                    linshi.pro_fill = New SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
                End If

                ui_connect_window_select_item_list.Insert(a, linshi)
            Next

            ui_window_select_item_list.ItemsSource = ui_connect_window_select_item_list
            ui_window_select_item_list.SelectedIndex = ui_connect_window_select_item_list_select_index

        End If

    End Sub

    ''' <summary>
    ''' 窗口移动
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub window_move(sender As Object, e As MouseButtonEventArgs)

        ReleaseCapture()
        SendMessage(New Interop.WindowInteropHelper(window_select_item_name).Handle, &HA1, 2, 0)

    End Sub

    ''' <summary>
    ''' 确认
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param> 
    Private Sub mouse_enter_ok(sender As Object, e As RoutedEventArgs) Handles ui_btn.Click

        If ui_window_select_item_list.SelectedIndex <> -1 Then
            ui_connect_window_select_item_list_select_index = ui_window_select_item_list.SelectedIndex
            Me.Close()
        Else
            window_dialogs_show("错误", "你没有选择任何项", 2, 1, False, "确定", "", Application.Current.MainWindow)
        End If

    End Sub

End Class
