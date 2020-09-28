/*
 *		Description: 输入框界面
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

public class InputDialog : UIDialogBase
{
    // 输入框
    public InputField input;

    // 输入框左侧的文字
    public Text txt;

    // 回调
    private System.Action<string> confirmAction;

    //public Text title;
    public Text txt_Confirm;
    public Text txt_Cancel;


    public override void OnSceneActivated(params object[] sceneData)
    {
        base.OnSceneActivated(sceneData);

        if(sceneData.Length == 1) //只传了一个 回调
        {
            confirmAction = (System.Action<string>)sceneData[0];
        }
        else if(sceneData.Length == 4)// 输入框左侧的文字,确定,取消,回调
        {
            txt.text = sceneData[0].ToString();
            confirmAction = (System.Action<string>)sceneData[1];
            txt_Confirm.text = sceneData[2].ToString();
            txt_Cancel.text = sceneData[3].ToString();       
        }

    }


    public override void OnClick(string str)
    {
        base.OnClick(str);
        switch (str)
        {
            case "btn_Cancel":              
                OnBackPressed();
                break;
            case "btn_Close":
                OnBackPressed();
                break;
               
            case "btn_Confirm":
                if (confirmAction != null) confirmAction(input.text);
                OnBackPressed();
                break;
            default:
                break;
        }
    }



    public override void OnSceneDeactivated()
    {
        base.OnSceneDeactivated();

        input.text = "";
        txt.text = "请输入";
        txt_Confirm.text = "确定";
        txt_Cancel.text = "取消";
        confirmAction = null;
    }

    #region 调用方式

    //Action<string> callBack = (str) =>
    //{
    //    if (string.IsNullOrEmpty(str))
    //    {
    //        print("没有输入任何内容");
    //    }
    //    else
    //    {
    //        print("你输入了 : " + str);
    //    }

    //};
    ////后面三个参数可以不填,会给默认值
    //Game.DialogMgr.PushDialog(UIDialog.InputDialog, "输入新名称", callBack, "确定", "取消");


    #endregion

}