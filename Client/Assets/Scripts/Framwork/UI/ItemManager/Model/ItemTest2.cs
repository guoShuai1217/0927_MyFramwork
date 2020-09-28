/*
 *		Description: 
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

public class ItemTest2 : ItemBase
{

    public Text txt_Content;


    /// <summary>
    /// 初始化数据
    /// </summary>
    public override void Init()
    {
        base.Init();
        txt_Content.text = string.Empty;
    }

    /// <summary>
    /// 赋值
    /// </summary>
    /// <param name="data"></param>
    public override void SetContent(params object[] data)
    {
        base.SetContent(data);
        txt_Content.text = data[0].ToString();
    }

}