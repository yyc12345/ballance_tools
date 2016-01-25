Module ui_use

    ''' <summary>
    ''' 窗口-选择项列表集合
    ''' </summary>
    Public ui_connect_window_select_item_list As New List(Of ui_depend_window_select_item_list)
    ''' <summary>
    ''' 窗口-选择项，选择的项序号
    ''' </summary>
    Public ui_connect_window_select_item_list_select_index As Integer = 0

    ''' <summary>
    ''' 窗口-选择项使用的类
    ''' </summary>
    Public Class ui_depend_window_select_item_list
        ''' <summary>
        ''' 标题
        ''' </summary>
        ''' <returns></returns>
        Public Property pro_title As String
        ''' <summary>
        ''' 文本
        ''' </summary>
        ''' <returns></returns>
        Public Property pro_text As String
    End Class

End Module

