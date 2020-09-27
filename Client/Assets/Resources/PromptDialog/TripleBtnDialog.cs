/*
 * 		Description: 三个按钮的提示界面
 *
 *  	CreatedBy:  guoShuai
 *
 *  	DataTime: 2019.04.11
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripleBtnDialog :  UIDialogBase
{

    public Text txt_Title; // 提示
    public Text txt_Content; // 内容
    public Button btn_Yes; // 是按钮
    public Button btn_No; // 否按钮
    public Button btn_Cancel; // 取消按钮

    public Text txt_Yes; // 是
    public Text txt_No; // 否
    public Text txt_Cancel; // 取消

    public Action<ushort> callBack; // 0是,1否,2取消


    /// <summary>
    /// content + callBack + title(提示) + btnYes(是) + btnNo(否)+ btnCancel(取消)
    /// </summary>
    /// <param name="sceneData"></param>
    void SetContent(object[] sceneData)
    {
        int length = sceneData.Length;

        txt_Content.text = sceneData[0].ToString();
        callBack = length > 1 ? (Action<ushort>)sceneData[1] : null;

        if(length == 2)
        {
            txt_Title.text = "提示";
            txt_Yes.text = "是";
            txt_No.text = "否";
            txt_Cancel.text = "取消";
        }
        else if(length == 6)
        {
            txt_Title.text = sceneData[2].ToString(); 
            txt_Yes.text = sceneData[3].ToString(); ;
            txt_No.text = sceneData[4].ToString(); ;
            txt_Cancel.text = sceneData[5].ToString(); ;
        }
    }

    public override void OnSceneActivated(params object[] sceneData)
    {
        base.OnSceneActivated(sceneData);

        SetContent(sceneData);
    }

    public override void OnClick(string str)
    {
        base.OnClick(str);

        switch (str)
        {
            case "btn_Yes":
                if (callBack != null)
                    callBack(0);
                OnBackPressed();
                break;
            case "btn_No":
                if (callBack != null)
                    callBack(1);
                OnBackPressed();
                break;
            case "btn_Cancel":
                if (callBack != null)
                    callBack(2);
                OnBackPressed();
                break;
            default:
                break;
        }

    }

    public override void OnSceneDeactivated()
    {
        base.OnSceneDeactivated();
 
        txt_Content.text = "";
        callBack = null;

        txt_Title.text = "提示";
        txt_Yes.text = "是";
        txt_No.text = "否";
        txt_Cancel.text = "取消";
    }

    #region 使用方式

    //Action<ushort> callBack = (ok) =>
    //{
    //    switch (ok)
    //    {
    //        case 0:
    //            print("you click Yes");
    //            break;
    //        case 1:
    //            print("you click No");
    //            break;
    //        case 2:
    //            print("you click Cancel");
    //            break;
    //        default:
    //            break;
    //    }

    //};

    // // 后面四个参数可以不填,会给默认值
    //UIMgr.Instance.PushDialog(UIDialog.TripleBtnDialog, "确定退出当前程序吗?", callBack, "郑重警告", "是吗", "否吗", "取消吧");

    #endregion
}
