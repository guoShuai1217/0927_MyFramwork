/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConfig  
{

    // 0 服务器 , 1 本地
    public const int PPP_DEBUG = 1;

    



}

public class UIPage
{
    public const string LoginPage = "LoginPage/LoginPage";

    public const string MainPage = "MainPage/MainPage2";

}

public class UIDialog
{
    public const string SingleBtnDialog = "PromptDialog/SingleBtnDialog";   //单按钮提示框
    public const string DoubleBtnDialog = "PromptDialog/DoubleBtnDialog";   //双按钮提示框
    public const string TripleBtnDialog = "PromptDialog/TripleBtnDialog";   //三按钮提示框
    public const string FadedDialog = "PromptDialog/FadedDialog";           //渐隐提示框
    public const string InputDialog = "PromptDialog/InputDialog";           //输入框界面






    // ----------------------------------具体项目弹窗-------------------------------------------

    public const string CreateRoomDialog = "CreateRoomDialog/CreateRoomDialog";
    public const string JoinRoomDialog = "JoinRoomDialog/JoinRoomDialog";
    public const string UserInfoDialog = "UserInfoDialog/UserInfoDialog";
}