/*
 *		Description: Get请求
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.02.03
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GetWWWItem : WWWItemBase
{

    //public GetWWWItem(string url):base(url)
    //{
    //    this.url = url;
    //}

    /// <summary>
    /// 设置参数
    /// </summary>
    /// <param name="url"></param>
    public void SetValue(string url)
    {
        this.url = url;
    }

     
    //public override void Init()
    //{
    //    base.Init();
    //    // this.url = string.Empty;
    //}

   
    public override IEnumerator DownLoad()
    {
        Debug.Log("Request URL : " + url);

        if (beginDownload != null)
            beginDownload();

        WWW www = new WWW(url);
        while (!www.isDone)
        {
            if (downloadProgress != null)
                downloadProgress(www.progress);

            yield return www.progress;
        }

        if (downloadProgress != null)
            downloadProgress(www.progress);

        if (string.IsNullOrEmpty(www.error))
        {
            if (downloadFinish != null)
                downloadFinish(www);

            if (downloadText != null)
                downloadText(www.text);

            if (downloadData != null)
                downloadData(www.bytes);
        }
        else
        {
            if(downloadError != null)
            {
                downloadError(www.error);
            }
        }

        // 不管请求成功还是失败，这次请求结束了，
        // 池子里可以再次使用这个类了；
        this.isFinish = true;
    }
}
 
