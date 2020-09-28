/*
 *		Description: UI预制体的Item的基类
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

public class ItemBase : MonoBehaviour
{
    // 点击item的事件
    public System.Action<ItemBase> clickAction;

    // 删除item的事件
    public System.Action<ItemBase> deleteAction;


    /// <summary>
    /// 初始化组件
    /// (要求每个子类都要执行这个方法,在这个方法里把数据清空 exp:txt_Content.text = string.Empty)
    /// </summary>
    public virtual void Init()
    {
        clickAction = null;
        deleteAction = null;

        Show();
    }

    /// <summary>
    /// 组件赋值
    /// </summary>
    /// <param name="data"></param>
    public virtual void SetContent(params object[] data)
    {

    }

    // 隐藏
    public virtual void Hide()
    {    
        gameObject.SetActive(false);
    }
 
    // 显示
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}

 
 