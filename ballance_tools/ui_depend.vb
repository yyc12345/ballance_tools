''' <summary>
''' 地图-检测所用的类
''' </summary>
Public Class ui_depend_form_level_form_check_list
    ''' <summary>
    ''' 关卡序号
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_level_name As String
    ''' <summary>
    ''' 完整路径
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_level_path As String
    ''' <summary>
    ''' 检测到的地图名称
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_map_name As String
End Class

''' <summary>
''' 地图-地图列表所用的类
''' </summary>
Public Class ui_depend_form_level_form_level_list
    ''' <summary>
    ''' 关卡名称
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_title As String
    ''' <summary>
    ''' 关卡ID
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_id As String
    ''' <summary>
    ''' 关卡创建者
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_creator As String
    ''' <summary>
    ''' 关卡描述
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_description As String
    ''' <summary>
    ''' 关卡图片
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_image As ImageBrush
    ''' <summary>
    ''' 关卡难度
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_difficulty As String
    ''' <summary>
    ''' 关卡玩的次数
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_playcount As String
    ''' <summary>
    ''' 关卡星级
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_stars As String
    ''' <summary>
    ''' 关卡下载次数
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_downloadcount As String

End Class

''' <summary>
''' 关卡-英雄榜列表所用的类
''' </summary>
Public Class ui_depend_form_level_form_hero_list
    ''' <summary>
    ''' 用户名
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_player As String
    ''' <summary>
    ''' 分的点数
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_point As String
    ''' <summary>
    ''' 记录创建时间
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_creat_time As String
    ''' <summary>
    ''' 记录SR时间
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_time As String

    ''' <summary>
    ''' 人名背景颜色1=金色 2=银色 3=铜色
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_background As SolidColorBrush
    ''' <summary>
    ''' 用户头像
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_user_image As ImageBrush
    ''' <summary>
    ''' 额外点数
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_exter_point As String



End Class

''' <summary>
''' 设置-本地英雄榜列表所用的类
''' </summary>
Public Class ui_depend_form_setting_form_local_hero_list
    ''' <summary>
    ''' 用户名
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_name As String
    ''' <summary>
    ''' 分值
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_score As String
    ''' <summary>
    ''' 所对关卡
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_level As String

End Class

''' <summary>
''' 图片-背景包列表所用的类
''' </summary>
Public Class ui_depend_form_photo_form_background_list
    ''' <summary>
    ''' 背景包名称
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_title As String
    ''' <summary>
    ''' 背景包图片
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_image As ImageBrush

End Class

''' <summary>
''' 图片-材质包列表所用的类
''' </summary>
Public Class ui_depend_form_photo_form_material_list
    ''' <summary>
    ''' 材质包名称
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_title As String
    ''' <summary>
    ''' 材质包图片
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_image As ImageBrush

End Class

''' <summary>
''' 音乐-音效包列表所用的类
''' </summary>
Public Class ui_depend_form_wave_form_wave_list
    ''' <summary>
    ''' 音效包名称
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_title As String
    ''' <summary>
    ''' 音效包图片
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_image As ImageBrush

End Class

''' <summary>
''' 音乐-bgm包列表所用的类
''' </summary>
Public Class ui_depend_form_wave_form_bgm_list
    ''' <summary>
    ''' bgm包名称
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_title As String
    ''' <summary>
    ''' bgm包图片
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_image As ImageBrush

End Class

''' <summary>
''' nmo-mod列表所用的类
''' </summary>
Public Class ui_depend_form_nmo_form_mod_mod_list
    ''' <summary>
    ''' mod名称---文件名
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_title As String
    ''' <summary>
    ''' mod图片
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_image As ImageBrush

    ''' <summary>
    ''' mod名字
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_name As String
    ''' <summary>
    ''' mod标识符
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_pid As String
    ''' <summary>
    ''' mod制作者
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_creator As String
    ''' <summary>
    ''' mod作用描述
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_describe As String
    ''' <summary>
    ''' mod封面图片下载地址
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_download_photo As String

End Class

''' <summary>
''' nmo-模组列表所用的类
''' </summary>
Public Class ui_depend_form_nmo_form_mod_ph_list
    ''' <summary>
    ''' 模组名称---文件名
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_title As String
    ''' <summary>
    ''' 模组图片
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_image As ImageBrush

    ''' <summary>
    ''' 模组名字
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_name As String
    ''' <summary>
    ''' 模组标识符
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_pid As String
    ''' <summary>
    ''' 模组制作者
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_creator As String
    ''' <summary>
    ''' 模组作用描述
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_describe As String
    ''' <summary>
    ''' 模组封面图片下载地址
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_download_photo As String

End Class


''' <summary>
''' 设置-本地排行榜备份列表所用的类
''' </summary>
Public Class ui_depend_form_setting_form_local_hero_backups_list
    ''' <summary>
    ''' 本地排行榜备份名称---文件名
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_title As String

End Class


''' <summary>
''' 联网对战-加入服务器的人员列表所用的类
''' </summary>
Public Class ui_depend_form_web_form_people_list
    ''' <summary>
    ''' 加入人的名称代号
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_name As String
    ''' <summary>
    ''' 加入人的名称代号
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_ip As String
    ''' <summary>
    ''' 加入人的名称代号
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_port As String
    ''' <summary>
    ''' 加入人的名称代号
    ''' </summary>
    ''' <returns></returns>
    Public Property pro_state As String

End Class



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
