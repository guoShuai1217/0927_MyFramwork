/*
 *		Description: Post请求
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

public class PostWWWItem : WWWItemBase
{

    private byte[] postData;

    //public PostWWWItem(string url,byte[] postData)
    //{
    //    this.url = url;
    //    this.postData = postData;
    //}

    
    public void SetValue(string url,byte[] postData)
    {
        this.url = url;
        this.postData = postData;
    }

    public override void Init()
    {
        base.Init();
        // this.url = string.Empty;
        this.postData = null;
    }

    public override IEnumerator DownLoad()
    {
        Debug.Log("Request URL : " + url);

        if (beginDownload != null)
            beginDownload();

        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/json");

        WWW www = new WWW(url,postData,header);
        float timeNow = Time.time;
        while (!www.isDone)
        {
            if (downloadProgress != null)
                downloadProgress(www.progress);
            if (Time.time - timeNow > TimeOut)
            {
                Debug.LogError("请求超时.. " + url);
                if (downloadError != null)
                {
                    downloadError("请求超时");
                    yield break;
                }
            }

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
            if (downloadError != null)
            {
                downloadError(www.error);
            }
        }

        // 不管请求成功还是失败，这次请求结束了，
        // 池子里可以再次使用这个类了；
        this.isFinish = true;
    }
}

