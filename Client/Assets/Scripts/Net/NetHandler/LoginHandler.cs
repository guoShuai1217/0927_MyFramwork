using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientNetFrame;
using GameProtocol;
using UnityEngine;

public class LoginHandler : IHandler
{

   

    public void MessageReceive(SocketModel model)
    {

        switch (model.command)
        {
            case AccountProtocol.REGISTER_SRES:
                int registResult = model.GetMessage<int>();
                registRes(registResult);

                break;

            case AccountProtocol.LOGIN_SRES:
                int accResult = model.GetMessage<int>();
                loginRes(accResult);

                break;

            case AccountProtocol.OFFLINE_SRES:


                break;

       
            default:
                break;
        }


    }

  
    // 服务器注册回复
    private void registRes(int result)
    {
        if (result == 0)
            Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "注册成功", Color.green);
        else
            Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "注册失败,账号已存在", Color.red);
    }

    // 服务器登陆回复
    private void loginRes(int Status)
    {
        
        switch (Status)
        {
            case 0:
                Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "登陆成功", Color.green);
                Debug.Log("登录成功");

                Game.SocketMgr.Send(TypeProtocol.USER, UserProtocol.GETINFO_CREQ, null);
                break;
            case -1:
                Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "请求错误", Color.red);
                Debug.Log("请求错误");
                break;
            case -2:
                Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "账号密码不合法", Color.red);
                Debug.Log("账号密码不合法");
                break;
            case -3:
                Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "没有此账号", Color.red);
                Debug.Log("没有此账号");
                break;
            case -4:
                Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "密码错误", Color.red);
                Debug.Log("密码错误");
                break;
            case -5:
                Game.DialogMgr.PushDialogImmediately(UIDialog.FadedDialog, "账号已登录", Color.red);
                Debug.Log("账号已登录");
                break;
        }
    }







}

