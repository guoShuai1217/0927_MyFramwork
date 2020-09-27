/*
 *		Description: 存储下载信息的基类
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.02.03
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 开始下载
public delegate void BeginDownLoad();
// 下载进度
public delegate void DownLoadProgress(float progress);
// 下载完成
public delegate void DownLoadFinish(WWW www);

public delegate void DownLoadText(string text);

public delegate void DownLoadData(byte[] data);

// 下载出错
public delegate void DownLoadError(string reason);

public abstract class WWWItemBase  
{
    protected string url;

    //public WWWItemBase(string url)
    //{
    //    this.url = url;
    //}

    protected bool isFinish = false;
    /// <summary>
    /// 是否使用完成（true：可以再从池子里拿出来复用）
    /// </summary>
    public bool IsFinish
    {
        get { return isFinish; }
    }

    public BeginDownLoad beginDownload;
    public DownLoadProgress downloadProgress;
    public DownLoadFinish downloadFinish;
    public DownLoadText downloadText;
    public DownLoadData downloadData;
    public DownLoadError downloadError;

    /// <summary>
    /// 请求数据
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator DownLoad();

    /// <summary>
    /// 初始化数据
    /// </summary>
    public virtual void Init()
    {
        this.beginDownload = null;
        this.downloadProgress = null;
        this.downloadFinish = null;
        this.downloadText = null;
        this.downloadData = null;
        this.downloadError = null;

        this.isFinish = false;
        this.url = string.Empty;
    }

}