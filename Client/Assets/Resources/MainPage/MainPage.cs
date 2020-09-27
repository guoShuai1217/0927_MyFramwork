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
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public partial class MainPage : UISceneBase
{

    public Text txt_Info;

    public InputField input_Rename; // 改昵称

    public Image img_Head;

    public Text txt_Nickname;

  
    public override void Init()
    {
        base.Init();

     
        //
        //UserHandler.loadImageCallback = LoadImageCallback; // 加载图片回调

        UserInfoDialog.callback = () => { img_Head.sprite = Game.curHead; GetRoleInfo(); }; // UserInfoDialog界面修改头像后,这里也要更新一下

    }

    private void LoadImageCallback(ImageModel obj)
    {
        if (obj == null) return;

        Directory.CreateDirectory(Application.streamingAssetsPath + "/ServerImage/");

        File.WriteAllBytes(Application.streamingAssetsPath + "/ServerImage/" + obj.imgName, obj.imgArr);
    }

 

    public override void OnSceneActivated(params object[] sceneData)
    {
        base.OnSceneActivated(sceneData);

        LoadHeadAndNickName(); // 加载头像
        GetRoleInfo(); // 用户信息赋值
    }

    public override void OnClick(string str)
    {
        base.OnClick(str);
        switch (str)
        {

            case "btn_Head": // 点击头像按钮

                Game.DialogMgr.PushDialog(UIDialog.UserInfoDialog);
                break;

            case "btn_JoinRoom":
                Game.DialogMgr.PushDialog(UIDialog.JoinRoomDialog);
                break;
            case "btn_CreateRoom":
                Game.DialogMgr.PushDialog(UIDialog.CreateRoomDialog);
                break;
           
        }
    }


 

    // 获取用户信息
    void GetRoleInfo()
    {
        string sex;
        if (Game.CurUserModel.sex == 2) sex = "";
        else sex = Game.CurUserModel.sex == 0 ? "男" : "女";
        
        txt_Info.text = "Id :  " + Game.CurUserModel.id + "\n" +
                       "nickname :  " + Game.CurUserModel.nickname + "\n" +
                       "account :  " + Game.CurUserModel.account + "\n" +
                       "sex :  " + sex;
        img_Head.sprite = Game.curHead;
    }

   
    /// <summary>
    /// 加载头像和昵称  
    /// </summary>
    public void LoadHeadAndNickName()
    {
        txt_Nickname.text = Game.CurUserModel.nickname;

        StartCoroutine(LoadHead());

    }

    // 加载头像
    IEnumerator LoadHead()
    {
        string url = ServerUtil.GetAccountURL() + Game.CurUserModel.account + "/Head/head.jpg";
        WWW www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Texture2D tx = www.texture;
            Sprite spr = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero);
            img_Head.sprite = spr;
            Game.curHead = spr;
        }
        else
        {
            Debug.Log("加载头像出错");
        }


    }

}