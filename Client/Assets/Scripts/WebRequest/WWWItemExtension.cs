/*
 *		Description: WWWUtil的扩展类，这里用来添加扩展方法的
 *		
 *		根据需要的回调，选择不同的调用方法；
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.02.03
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class WWWItemExtension
{

    #region GetRequest

    #region 返回结果为 string

    public static void GetStringRequest(this WebRequest wwwUtil, string url, DownLoadText text, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }
    
        item.SetValue(url);
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    public static void GetStringRequest(this WebRequest wwwUtil, string url, BeginDownLoad begin, DownLoadText text, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.beginDownload = begin;
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    public static void GetStringRequest(this WebRequest wwwUtil, string url, DownLoadProgress progress, DownLoadText text, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.downloadProgress = progress;
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    public static void GetStringRequest(this WebRequest wwwUtil, string url,BeginDownLoad begin, DownLoadProgress progress, DownLoadText text, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.beginDownload = begin;
        item.downloadProgress = progress;
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    #endregion

    #region 返回结果为 byte[]

    public static void GetByteRequest(this WebRequest wwwUtil, string url, DownLoadData data, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    public static void GetByteRequest(this WebRequest wwwUtil, string url, BeginDownLoad begin, DownLoadData data, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.beginDownload = begin;
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    public static void GetByteRequest(this WebRequest wwwUtil, string url, DownLoadProgress progress, DownLoadData data, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.downloadProgress = progress;
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    public static void GetByteRequest(this WebRequest wwwUtil, string url, BeginDownLoad begin, DownLoadProgress progress, DownLoadData data, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.beginDownload = begin;
        item.downloadProgress = progress;
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    #endregion

    #region 返回结果为WWW

    public static void GetWWWRequest(this WebRequest wwwUtil, string url, DownLoadFinish finish, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    public static void GetWWWRequest(this WebRequest wwwUtil, string url, BeginDownLoad begin, DownLoadFinish finish, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.beginDownload = begin;
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void GetWWWRequest(this WebRequest wwwUtil, string url, DownLoadProgress progress, DownLoadFinish finish, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.downloadProgress = progress;
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void GetWWWRequest(this WebRequest wwwUtil, string url, BeginDownLoad begin, DownLoadProgress progress, DownLoadFinish finish, DownLoadError error = null)
    {
        GetWWWItem item = WWWItemPool.GetWWWItem<GetWWWItem>();
        if (item == null)
        {
            item = new GetWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url);
        item.beginDownload = begin;
        item.downloadProgress = progress;
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    #endregion

    #endregion



    #region PostRequest

    #region 返回结果为 string

    public static void PostStringRequest(this WebRequest wwwUtil, string url, byte[] postData, DownLoadText text, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    public static void PostStringRequest(this WebRequest wwwUtil, string url, byte[] postData,BeginDownLoad begin, DownLoadText text, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.beginDownload = begin;
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void PostStringRequest(this WebRequest wwwUtil, string url, byte[] postData, DownLoadProgress progress, DownLoadText text, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.downloadProgress = progress;
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void PostStringRequest(this WebRequest wwwUtil, string url, byte[] postData, BeginDownLoad begin, DownLoadProgress progress, DownLoadText text, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.beginDownload = begin;
        item.downloadProgress = progress;
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    #endregion

    #region 返回结果为 byte[]

    public static void PostByteRequest(this WebRequest wwwUtil, string url, byte[] postData, DownLoadData data, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void PostByteRequest(this WebRequest wwwUtil, string url, byte[] postData, BeginDownLoad begin, DownLoadData data, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.beginDownload = begin;
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    public static void PostByteRequest(this WebRequest wwwUtil, string url, byte[] postData,DownLoadProgress progress,  DownLoadData data, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);       
        item.downloadProgress = progress;
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void PostByteRequest(this WebRequest wwwUtil, string url, byte[] postData,BeginDownLoad begin, DownLoadProgress progress, DownLoadData data, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.beginDownload = begin;
        item.downloadProgress = progress;
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    #endregion

    #region 返回结果为 WWW

    public static void PostWWWRequest(this WebRequest wwwUtil, string url, byte[] postData, DownLoadFinish finish, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void PostWWWRequest(this WebRequest wwwUtil, string url, byte[] postData, BeginDownLoad begin, DownLoadFinish finish, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.beginDownload = begin;
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void PostWWWRequest(this WebRequest wwwUtil, string url, byte[] postData, DownLoadProgress progress, DownLoadFinish finish, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.downloadProgress = progress;
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void PostWWWRequest(this WebRequest wwwUtil, string url, byte[] postData, BeginDownLoad begin, DownLoadProgress progress, DownLoadFinish finish, DownLoadError error = null)
    {
        PostWWWItem item = WWWItemPool.GetWWWItem<PostWWWItem>();
        if (item == null)
        {
            item = new PostWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, postData);
        item.beginDownload = begin;
        item.downloadProgress = progress;
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    #endregion

    #endregion



    #region WWWFormRequest

    #region 返回结果为 string

    public static void WWWFormStringRequest(this WebRequest wwwUtil, string url, WWWForm form, DownLoadText text, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void WWWFormStringRequest(this WebRequest wwwUtil, string url, WWWForm form,BeginDownLoad begin, DownLoadText text, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.beginDownload = begin;
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void WWWFormStringRequest(this WebRequest wwwUtil, string url, WWWForm form, DownLoadProgress progress, DownLoadText text, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.downloadProgress = progress;
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void WWWFormStringRequest(this WebRequest wwwUtil, string url, WWWForm form,BeginDownLoad begin, DownLoadProgress progress,  DownLoadText text, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.beginDownload = begin;
        item.downloadProgress = progress;
        item.downloadText = text;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    #endregion

    #region 返回结果为 byte[]

    public static void WWWFormByteRequest(this WebRequest wwwUtil, string url, WWWForm form, DownLoadData data, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void WWWFormByteRequest(this WebRequest wwwUtil, string url, WWWForm form, BeginDownLoad begin, DownLoadData data, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.beginDownload = begin;
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void WWWFormByteRequest(this WebRequest wwwUtil, string url, WWWForm form, DownLoadProgress progress, DownLoadData data, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.downloadProgress = progress;
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void WWWFormByteRequest(this WebRequest wwwUtil, string url, WWWForm form, BeginDownLoad begin, DownLoadProgress progress, DownLoadData data, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.beginDownload = begin;
        item.downloadProgress = progress;
        item.downloadData = data;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }

    #endregion

    #region 返回结果为 WWW

    public static void WWWFormWWWRequest(this WebRequest wwwUtil, string url, WWWForm form, DownLoadFinish finish, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void WWWFormWWWRequest(this WebRequest wwwUtil, string url, WWWForm form, BeginDownLoad begin, DownLoadFinish finish, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.beginDownload = begin;
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void WWWFormWWWRequest(this WebRequest wwwUtil, string url, WWWForm form, DownLoadProgress progress, DownLoadFinish finish, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.downloadProgress = progress;
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    public static void WWWFormWWWRequest(this WebRequest wwwUtil, string url, WWWForm form, BeginDownLoad begin, DownLoadProgress progress, DownLoadFinish finish, DownLoadError error = null)
    {
        FormWWWItem item = WWWItemPool.GetWWWItem<FormWWWItem>();
        if (item == null)
        {
            item = new FormWWWItem();
            WWWItemPool.AddWWWItem(item);
        }

        item.SetValue(url, form);
        item.beginDownload = begin;
        item.downloadProgress = progress;
        item.downloadFinish = finish;
        item.downloadError = error;

        wwwUtil.AddTask(item);
    }


    #endregion


    #endregion
}

