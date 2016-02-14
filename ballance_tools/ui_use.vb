Imports Microsoft.Win32

Module ui_use

    ''' <summary>
    ''' 打开ballance主启动文件的文件打开对话框
    ''' </summary>
    Public open_ballance_dir As New OpenFileDialog
    ''' <summary>
    ''' ballance启动路径（带\）
    ''' </summary>
    Public ballance_start_path As String = ""
    ''' <summary>
    ''' 是否安装有BML
    ''' </summary>
    Public BML_have As Boolean = False

    ''' <summary>
    ''' 用户名
    ''' </summary>
    Public user_name As String = ""
    ''' <summary>
    ''' 用户密码
    ''' </summary>
    Public user_password As String = ""

#Region "UI列表集合"
    ''' <summary>
    ''' 关卡-地图检测列表集合
    ''' </summary>
    Public ui_connect_form_level_form_check_list As New List(Of ui_depend_form_level_form_check_list)
    ''' <summary>
    ''' 关卡-地图列表集合
    ''' </summary>
    Public ui_connect_form_level_form_level_list As New List(Of ui_depend_form_level_form_level_list)
    ''' <summary>
    ''' 关卡-英雄榜列表集合
    ''' </summary>
    Public ui_connect_form_level_form_hero_list As New List(Of ui_depend_form_level_form_hero_list)
    ''' <summary>
    ''' 关卡-本地英雄榜列表集合
    ''' </summary>
    Public ui_connect_form_setting_form_local_hero_list As New List(Of ui_depend_form_setting_form_local_hero_list)
    ''' <summary>
    ''' 图片-背景包列表集合
    ''' </summary>
    Public ui_connect_form_photo_form_background_list As New List(Of ui_depend_form_photo_form_background_list)
    ''' <summary>
    ''' 图片-材质包列表集合
    ''' </summary>
    Public ui_connect_form_photo_form_material_list As New List(Of ui_depend_form_photo_form_material_list)
    ''' <summary>
    ''' 音乐-音效包列表集合
    ''' </summary>
    Public ui_connect_form_wave_form_wave_list As New List(Of ui_depend_form_wave_form_wave_list)
    ''' <summary>
    ''' 音乐-bgm包列表集合
    ''' </summary>
    Public ui_connect_form_wave_form_bgm_list As New List(Of ui_depend_form_wave_form_bgm_list)
    ''' <summary>
    ''' nmo-mod列表集合
    ''' </summary>
    Public ui_connect_form_nmo_form_mod_mod_list As New List(Of ui_depend_form_nmo_form_mod_mod_list)
    ''' <summary>
    ''' nmo-模组列表集合
    ''' </summary>
    Public ui_connect_form_nmo_form_mod_ph_list As New List(Of ui_depend_form_nmo_form_mod_ph_list)
    ''' <summary>
    ''' 联网对战-加入服务器人员列表集合
    ''' </summary>
    Public ui_connect_form_web_form_people_list As New List(Of ui_depend_form_web_form_people_list)
    ''' <summary>
    ''' 设置-本地排行榜列表集合
    ''' </summary>
    Public ui_connect_form_setting_form_local_hero_backups_list As New List(Of ui_depend_form_setting_form_local_hero_backups_list)

#End Region

#Region "UI列表集合--搜索"

    ''' <summary>
    ''' 关卡-地图列表集合-搜索专用
    ''' </summary>
    Public ui_connect_form_level_form_level_list_search As New List(Of ui_depend_form_level_form_level_list)
    ''' <summary>
    ''' 图片-背景包列表集合-搜索专用
    ''' </summary>
    Public ui_connect_form_photo_form_background_list_search As New List(Of ui_depend_form_photo_form_background_list)
    ''' <summary>
    ''' 图片-材质包列表集合-搜索专用
    ''' </summary>
    Public ui_connect_form_photo_form_material_list_search As New List(Of ui_depend_form_photo_form_material_list)
    ''' <summary>
    ''' 音乐-音效包列表集合-搜索专用
    ''' </summary>
    Public ui_connect_form_wave_form_wave_list_search As New List(Of ui_depend_form_wave_form_wave_list)
    ''' <summary>
    ''' 音乐-bgm包列表集合-搜索专用
    ''' </summary>
    Public ui_connect_form_wave_form_bgm_list_search As New List(Of ui_depend_form_wave_form_bgm_list)
    ''' <summary>
    ''' nmo-mod列表集合-搜索专用
    ''' </summary>
    Public ui_connect_form_nmo_form_mod_mod_list_search As New List(Of ui_depend_form_nmo_form_mod_mod_list)
    ''' <summary>
    ''' nmo-模组列表集合-搜索专用
    ''' </summary>
    Public ui_connect_form_nmo_form_mod_ph_list_search As New List(Of ui_depend_form_nmo_form_mod_ph_list)


#End Region


    ''' <summary>
    ''' 检测地图文件名
    ''' </summary>
    ''' <param name="length">pid</param>
    ''' <returns></returns>
    Public Function check_map_name(ByVal length As String) As String
        Dim map_name As String = "未知地图"
        Dim file As New System.IO.StreamReader(Environment.CurrentDirectory & "\map.dat", System.Text.Encoding.UTF8)
        Dim word As String = ""

        Do
            word = file.ReadLine
            If word = "" Then
                Exit Do
            End If

            If file.ReadLine = length Then
                '是的
                map_name = word

                Exit Do
            End If

        Loop

        file.Dispose()
        Return map_name
    End Function

    ''' <summary>
    ''' 获取本地mod描述文件---没有找到会填写默认内容
    ''' </summary>
    ''' <param name="search_pid">要查找的mod的pid</param>
    ''' <param name="return_name">返回-名字</param>
    ''' <param name="return_pid">返回-pid</param>
    ''' <param name="return_creator">返回-制作者</param>
    ''' <param name="return_describe">返回-描述作用</param>
    ''' <param name="return_download_photo">返回-mod介绍图片下载地址</param>
    Public Sub search_mod_list_file(ByVal search_pid As String, ByRef return_name As String, ByRef return_pid As String, ByRef return_creator As String,
                                         ByRef return_describe As String, ByRef return_download_photo As String)
        Dim file As New System.IO.StreamReader(Environment.CurrentDirectory & "\mod.dat", System.Text.Encoding.UTF8)
        Dim word As String = ""
        Dim after_word As String = ""
        Dim yes As Boolean = False

        Do
            word = file.ReadLine
            If word = "" Then
                Exit Do
            End If

            after_word = word
            word = file.ReadLine
            If word = search_pid Then
                '匹配
                return_name = "名称：" & after_word
                return_pid = "文件标识符：" & word
                return_creator = "制作者：" & file.ReadLine
                return_describe = "Mod描述：" & file.ReadLine
                return_download_photo = file.ReadLine
                yes = True
                Exit Do
            Else
                '不匹配
                file.ReadLine()
                file.ReadLine()
                file.ReadLine()
                '继续

            End If

        Loop

        If yes = False Then
            '都没有匹配，默认
            return_name = "名称：未知"
            return_pid = "文件标识符：未知"
            return_creator = "制作者：未知"
            return_describe = "Mod描述：未知"
            return_download_photo = ""
        End If

        file.Dispose()

    End Sub

    ''' <summary>
    ''' 获取本地模组描述文件---没有找到会填写默认内容
    ''' </summary>
    ''' <param name="search_pid">要查找的mod的pid</param>
    ''' <param name="return_name">返回-名字</param>
    ''' <param name="return_pid">返回-pid</param>
    ''' <param name="return_creator">返回-制作者</param>
    ''' <param name="return_describe">返回-描述作用</param>
    ''' <param name="return_download_photo">返回-mod介绍图片下载地址</param>
    Public Sub search_ph_list_file(ByVal search_pid As String, ByRef return_name As String, ByRef return_pid As String, ByRef return_creator As String,
                                         ByRef return_describe As String, ByRef return_download_photo As String)
        Dim file As New System.IO.StreamReader(Environment.CurrentDirectory & "\ph.dat", System.Text.Encoding.UTF8)
        Dim word As String = ""
        Dim after_word As String = ""
        Dim yes As Boolean = False

        Do
            word = file.ReadLine
            If word = "" Then
                Exit Do
            End If

            after_word = word
            word = file.ReadLine
            If word = search_pid Then
                '匹配
                return_name = "名称：" & after_word
                return_pid = "文件标识符：" & word
                return_creator = "制作者：" & file.ReadLine
                return_describe = "模型描述：" & file.ReadLine
                return_download_photo = file.ReadLine
                yes = True
                Exit Do
            Else
                '不匹配
                file.ReadLine()
                file.ReadLine()
                file.ReadLine()
                '继续

            End If

        Loop

        If yes = False Then
            '都没有匹配，默认
            return_name = "名称：未知"
            return_pid = "文件标识符：未知"
            return_creator = "制作者：未知"
            return_describe = "模型描述：未知"
            return_download_photo = ""
        End If

        file.Dispose()

    End Sub

    ''' <summary>
    ''' 搜索本地文件
    ''' </summary>
    ''' <param name="search_pid">要查找的map的pid</param>
    ''' <param name="return_creator">返回-名字</param>
    ''' <param name="return_description">返回-描述</param>
    ''' <param name="return_difficulty">返回-难度</param>
    ''' <param name="return_stars">返回-星级</param>
    ''' <param name="return_download_path">返回-下载地址</param>
    ''' <param name="allow_rewrite">是否允许复写，就是在没有本地资源的情况下，将返回值全部填成未知，默认复写</param>
    Public Sub search_map_list_file(ByVal search_pid As String, ByRef return_creator As String, ByRef return_description As String, ByRef return_difficulty As String,
                                    ByRef return_stars As String, ByRef return_download_path As String, Optional ByVal allow_rewrite As Boolean = True)

        Dim file As New System.IO.StreamReader(Environment.CurrentDirectory & "\baidu_map.dat", System.Text.Encoding.UTF8)
        Dim word As String = ""
        Dim yes As Boolean = False

        Do
            word = file.ReadLine
            If word = "" Then
                Exit Do
            End If

            If word = search_pid Then
                '匹配
                return_creator = "制作者：" & file.ReadLine
                return_description = "描述：" & file.ReadLine
                return_difficulty = "难度：" & file.ReadLine
                return_stars = "星级：" & file.ReadLine
                return_download_path = file.ReadLine

                yes = True
                Exit Do
            Else
                '不匹配
                file.ReadLine()
                file.ReadLine()
                file.ReadLine()
                file.ReadLine()
                file.ReadLine()
                '继续
            End If

        Loop

        If yes = False And allow_rewrite = True Then
            return_creator = "制作者：未知"
            return_description = "描述：无"
            return_difficulty = "难度：未知"
            return_stars = "星级：未知"
            return_download_path = ""
        End If

        file.Dispose()
    End Sub


End Module
