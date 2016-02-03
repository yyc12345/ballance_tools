Namespace ui

    Module ui_use

        Public ui_connect_user_list As New List(Of ui_depend_user_list)

    End Module

    Module ui_depend

        ''' <summary>
        ''' 用户列表所用类
        ''' </summary>
        Public Class ui_depend_user_list
            ''' <summary>
            ''' 玩家名
            ''' </summary>
            ''' <returns></returns>
            Public Property pro_name As String
            ''' <summary>
            ''' 玩家ip
            ''' </summary>
            ''' <returns></returns>
            Public Property pro_ip As String
            ''' <summary>
            ''' 玩家端口--不显示
            ''' </summary>
            ''' <returns></returns>
            Public Property pro_port As String
            ''' <summary>
            ''' 玩家状态：正常/名次/作弊/掉线
            ''' </summary>
            ''' <returns></returns>
            Public Property pro_state As String
            ''' <summary>
            ''' 玩家背景:第一名：255,0,0 第二：235,0,0 第三：215，0，0 其余:透明
            ''' </summary>
            ''' <returns></returns>
            Public Property pro_bk As SolidColorBrush
        End Class

    End Module

End Namespace