/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using GameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameProtocol.model.login;

public class LoginPage : UISceneBase
{

    public InputField input_Account;

    public InputField input_Password;

    public override void OnClick(string str)
    {
        base.OnClick(str);

        AccountModel model = new AccountModel(input_Account.text, input_Password.text);
        switch (str)
        {
            case "btn_Login":
               
                Debug.Log("Click btn_Login");

                Game.SocketMgr.Send(TypeProtocol.ACCOUNT, AccountProtocol.LOGIN_CREQ, model);

                // Game.UIMgr.PushScene(UIPage.MainPage);
                break;

            case "btn_Register":
                Debug.Log("btn_Register");
               
                Game.SocketMgr.Send(TypeProtocol.ACCOUNT, AccountProtocol.REGISTER_CREQ, model);

                break;
            default:
                break;
        }
    }

  
   
}