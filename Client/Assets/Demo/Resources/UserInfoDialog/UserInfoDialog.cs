/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using GameProtocol;
using GameProtocol.model.login;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoDialog : UIDialogBase
{

    public InputField input_ReName;
    public InputField input_ReSex;
    public InputField input_Birthday;
    public InputField input_Phone;
    public InputField input_Mail;

    public Text txt_Info;

    public Image btn_Head;

    private bool isReNameSuccess = false; //重命名昵称是否成功(如果昵称重复了,就不能提交修改用户信息请求)

    public static System.Action callback;

    public override void OnSceneActivated(params object[] sceneData)
    {
        base.OnSceneActivated(sceneData);


        showText();
    }

    void showText()
    {
        string str = "账号 : " + Game.CurUserModel.account + "\n";
        str += "昵称 : " + Game.CurUserModel.nickname + "\n";
        str += "性别 : " + Game.CurUserModel.sex + "\n";
        str += "生日 : " + Game.CurUserModel.birthday + "\n";
        str += "手机号 : " + Game.CurUserModel.phone + "\n";
        str += "邮箱 : " + Game.CurUserModel.mail + "\n";

        txt_Info.text = str;
        btn_Head.sprite = Game.curHead;

    }

    public override void Init()
    {
        base.Init();

        UserHandler.updateInfoCallback = updateCallback;
        UserHandler.updateImageCallback = UpdateImageCallback;
        UserHandler.reNameCallback = ReNameCallback; // 重命名回调       

        input_ReName.onEndEdit.AddListener((str) =>
        {
            if (string.IsNullOrEmpty(str)) return;
            Debug.Log("发送修改昵称请求");
            Game.SocketMgr.Send(TypeProtocol.USER, UserProtocol.RENAME_CREQ, str);
        });
    }

    #region 回调事件

  
    // 上传图片的回调
    private void UpdateImageCallback(int obj)
    {
        if (obj == -1)
        {
            Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "上传失败", Color.red);
            return;
        }

       // 上传图片成功了,  给MainPage和UserInfoPage的头像 更换图片
        btn_Head.sprite = tmpSprite;
        Game.curHead = tmpSprite;

        // MainPage 里的头像也需要修改
        callback();

        Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "上传成功", Color.green);
      
    }

    // 修改昵称的回调
    void ReNameCallback(int nick)
    {
        if (nick != 0)
        {
            Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "修改昵称失败,昵称已存在", Color.red);
        }
        else
        {           
            Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "修改昵称成功", Color.green);
            isReNameSuccess = true;
        }
    }

    // 修改信息的回调
    private void updateCallback(int obj)
    {

        if (obj == 0)
        {
            Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "修改成功", Color.green);
            showText();
            callback();
        }
        else
            Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "修改失败", Color.red);

    }

    #endregion


    #region 按钮点击事件


    public override void OnClick(string str)
    {
        base.OnClick(str);
        switch (str)
        {
            case "btn_Head":
                OnClickHead();
                break;

            case "btn_Sure":
                OnClickSure();
                break;

            case "btn_Back":
                OnBackPressed();
                break;
            default:
                PPP.pppShow();
                break;
        }
    }

    // 点击确定
    private void OnClickSure()
    {
        if (!isReNameSuccess)
        {
            Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "请修改昵称", Color.red);
            return;
        }

        if (string.IsNullOrEmpty(input_ReName.text) && string.IsNullOrEmpty(input_ReSex.text) && string.IsNullOrEmpty(input_Birthday.text) &&
            string.IsNullOrEmpty(input_Phone.text) && string.IsNullOrEmpty(input_Mail.text))
        {
            Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "没有做任何修改", Color.red);
            return;
        }


        if (!string.IsNullOrEmpty(input_ReName.text))
        {
            Game.CurUserModel.nickname = input_ReName.text;
        }

        if (!string.IsNullOrEmpty(input_ReSex.text))
        {
            if (input_ReSex.text != "男" && input_ReSex.text != "女")
            {
                Game.DialogMgr.PushDialog("性别不合法");
                return;
            }
            int result = input_ReSex.text == "男" ? 0 : 1;
            Game.CurUserModel.sex = result;
        }

        if (!string.IsNullOrEmpty(input_Birthday.text))
        {
            Game.CurUserModel.birthday = input_Birthday.text;
        }

        if (!string.IsNullOrEmpty(input_Phone.text))
        {
            Game.CurUserModel.phone = input_Phone.text;
        }

        if (!string.IsNullOrEmpty(input_Mail.text))
        {
            Game.CurUserModel.mail = input_Mail.text;
        }

        // 修改个人信息
        Game.SocketMgr.Send(TypeProtocol.USER, UserProtocol.UPDATEINFO_CREQ, Game.CurUserModel);

    }



    private Sprite tmpSprite = null;
    
    // 点击头像
    private void OnClickHead()
    {

        LoadTexUtil.LoadSingleTexture((spr, arr) =>
        {

            tmpSprite = spr;
            
            ImageModel im = new ImageModel("head.jpg", arr);
            Game.SocketMgr.Send(TypeProtocol.USER, UserProtocol.UPDATEIMG_CREQ, im);
        
        });

    }

    #endregion


    public override void OnSceneDeactivated()
    {
        base.OnSceneDeactivated();

        input_ReName.text = "";
        input_ReSex.text = "";
        input_Phone.text = "";
        input_Birthday.text = "";
        input_Mail.text = "";

        txt_Info.text = "";
        tmpSprite = null;
        isReNameSuccess = false;
    }

}   