using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientNetFrame;
using GameProtocol;
using GameProtocol.model.login;
using UnityEngine;

public partial class UserHandler : IHandler
{


    #region 委托事件

    public static System.Action<int> updateImageCallback; // 上传图片回调

    public static System.Action<ImageModel> loadImageCallback; // 加载图片回调

    public static System.Action<int> reNameCallback; // 重命名回调

    public static System.Action<int> updateInfoCallback; // 修改个人信息回调

    #endregion




    public void MessageReceive(SocketModel model)
    {

        switch (model.command)
        {
            case UserProtocol.GETINFO_SRES: // 获取用户信息
                UserModel getinfoUserModel = model.GetMessage<UserModel>();
                getinfoRes(getinfoUserModel);
                break;

            case UserProtocol.RENAME_SRES: //重命名
                int nick = model.GetMessage<int>();
                reNickNameRes(nick);

                break;
            case UserProtocol.UPDATEIMG_SRES:

                int re = model.GetMessage<int>();
                if (updateImageCallback != null)
                    updateImageCallback(re);

                break;

            case UserProtocol.LAODIMG_SRES:

                ImageModel im = model.GetMessage<ImageModel>();

                if (loadImageCallback != null)
                    loadImageCallback(im);
                break;

            case UserProtocol.UPDATEINFO_SRES:
                int ii = model.GetMessage<int>();
                if (updateInfoCallback != null)
                    updateInfoCallback(ii);

                break;
        }

    }



    private void getinfoRes(UserModel getinfoUserModel)
    {
        
        if(getinfoUserModel == null)
        {
            Debug.LogError("不存在用户");
            return;
        }

        // 赋值用户信息
        Game.CurUserModel = getinfoUserModel;

        // 打开主界面
        Game.UIMgr.PushScene(UIPage.MainPage);
    }


    // 修改昵称 回复
    private void reNickNameRes(int nick)
    {
        if (reNameCallback != null)
            reNameCallback(nick);
    }




}
 