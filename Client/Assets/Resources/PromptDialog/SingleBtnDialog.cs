/*
 * 		Description: 单个按钮的提示界面
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

public class SingleBtnDialog : UIDialogBase
{

    public Text txt_Title; // 标题
    public Text txt_Content; // 内容  
    public Button btn_Sure; // 确定按钮  
    public Text txt_Sure; // 确定

    public Action<bool> callBack;


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
            case "btn_Sure":
                if (callBack != null)
                    callBack(true);
                OnBackPressed();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="sceneData"></param>
    void SetContent(object[] sceneData)
    {      
        int length = sceneData.Length;

        txt_Content.text = sceneData[0].ToString();
        callBack = length > 1 ? (Action<bool>)sceneData[1] : null;

        if (length < 4)
        {
            txt_Title.text = "提示";
            txt_Sure.text = "确定";
        }
        else
        {
            txt_Title.text = sceneData[2].ToString();
            txt_Sure.text = sceneData[3].ToString();
        }

    }

    public override void OnSceneDeactivated()
    {
        base.OnSceneDeactivated();

        txt_Title.text = "提示";
        txt_Sure.text = "确定";
        txt_Content.text = "";
        callBack = null;
    }

    #region 使用方式

    //Action<bool> callBack = (ok) =>
    //{
    //    if (ok)
    //    {
    //        print("你点击了确定");
    //    }

    //};
    // 后面三个参数可以不填,会给默认值
    //Game.DialogMgr.PushDialog(UIDialog.SingleBtnDialog, "开发中,敬请期待.",callBack,"提示","确定");

    #endregion
}
