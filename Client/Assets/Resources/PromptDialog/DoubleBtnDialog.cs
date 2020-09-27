/*
 * 		Description: 双选按钮弹窗
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

public class DoubleBtnDialog : UIDialogBase
{

    public Text txt_Title; // 标题
    public Text txt_Content; // 内容
    public Button btn_Cancel; // 取消按钮
    public Button btn_Sure; // 确定按钮
    public Text txt_Cancel; // 取消
    public Text txt_Sure; // 确定

    public Action<bool> callBack;


    /// <summary>
    /// content + callBack + title(提示) + btnSure(确定) + btnCancel(取消)
    /// </summary>
    /// <param name="sceneData"></param>
    void SetContent(object[] sceneData)
    {
        if (sceneData.Length == 2)
        {
            txt_Title.text = "提示";
            txt_Sure.text = "确定";
            txt_Cancel.text = "取消";
        }
        else if (sceneData.Length == 5)
        {
            txt_Title.text = sceneData[2].ToString();
            txt_Sure.text = sceneData[3].ToString();
            txt_Cancel.text = sceneData[4].ToString();
        }

        txt_Content.text = sceneData[0].ToString();
        this.callBack = (Action<bool>)sceneData[1];
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
            case "btn_Cancel":
                if (callBack != null)
                    callBack(false);
                OnBackPressed();
                break;
            case "btn_Sure":
                if (callBack != null)
                    callBack(true);
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

        txt_Title.text = "提示";
        txt_Sure.text = "确定";
        txt_Cancel.text = "取消";
    }


    #region 使用方式

    //Action<bool> callBack = (isOk) =>
    //{
    //    if (isOk)
    //    {
    //        print("好的,你点击的确定");
    //    }
    //    else
    //    {
    //        print("你点击了取消,GOOD CHOICE");
    //    }

    //};
    // // 后面三个参数可以不填,会给默认值
    //UIMgr.Instance.PushDialog(UIDialog.DoubleBtnDialog,"确定退出系统吗?退出系统将不再打开",callBack,"警告","再玩一会" , "退出了");

    #endregion
}
