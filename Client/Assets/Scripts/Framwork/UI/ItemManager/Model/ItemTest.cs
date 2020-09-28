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

public class ItemTest : ItemBase
{

    public Text txt_Title;
    public Text txt_Content;
    public Text txt_Time;
    public Button btn_Click;
    public Button btn_Delete;

    private void Awake()
    {
        btn_Click.onClick.AddListener(() =>
        {

            if (clickAction != null)
                clickAction(this);

        });

        btn_Delete.onClick.AddListener(() =>
        {

            if (deleteAction != null)
                deleteAction(this);

        });
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public override void Init()
    {
        base.Init();
        txt_Title.text = "";
        txt_Content.text = string.Empty;
        txt_Time.text = string.Empty;
    }

    /// <summary>
    /// 赋值
    /// </summary>
    /// <param name="data"></param>
    public override void SetContent(params object[] data)
    {
        base.SetContent(data);

        this.txt_Title.text = data[0].ToString();
        this.txt_Content.text = data[1].ToString(); ;
        this.txt_Time.text = data[2].ToString(); ;
    }

    //public void SetContent(string title,string content,string time)
    //{
    //    this.txt_Title.text = title;
    //    this.txt_Content.text = content;
    //    this.txt_Time.text = time;
    //}


}