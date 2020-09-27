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

public partial class MainPage2 : MainPage 
{

    public override void OnClick(string str)
    {
        base.OnClick(str);
        switch (str)
        {
            //case "btn_Custom":
                
            //    Debug.Log("发送修改昵称请求");
            //    Game.SocketMgr.Send(TypeProtocol.USER, UserProtocol.RENAME_CREQ, input_Rename.text);
            //    break;
            case "btn_Back":
                OnClickExit();
                break;

            //case "btn_UpdateImg":
            //    string path = Application.streamingAssetsPath + "/6.jpg";
            //    byte[] arr = File.ReadAllBytes(path);
            //    ImageModel im = new ImageModel("6.jpg",arr);
            //    ImageInfo tmp = new ImageInfo();
            //    tmp.list.Add(im);
 
            //    Game.SocketMgr.Send(TypeProtocol.USER, UserProtocol.UPDATEIMG_CREQ, tmp);

            //    break;

            default:
                break;
        }
    }

    /// <summary>
    /// 退出当前用户
    /// </summary>
    private void OnClickExit()
    {
        Game.SocketMgr.Send(TypeProtocol.ACCOUNT, AccountProtocol.OFFLINE_CREQ, null);

        Game.CurUserModel = null;
        Game.curHead = null;

        OnBackPressed();
    }
}