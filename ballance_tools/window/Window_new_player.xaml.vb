﻿Public Class Window_new_player

    ''' <summary>
    ''' 普通事件处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_link_down(sender As Object, e As MouseButtonEventArgs)

        Dim tb As TextBlock = CType(sender, TextBlock)

        System.Diagnostics.Process.Start(tb.Text)

    End Sub

    ''' <summary>
    ''' 什么是Ballance的事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ui_link_about(sender As Object, e As MouseButtonEventArgs)

        System.Diagnostics.Process.Start("http://baike.baidu.com/view/68114.htm")

    End Sub
End Class
