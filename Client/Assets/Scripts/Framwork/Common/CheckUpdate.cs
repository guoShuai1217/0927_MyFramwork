/*
 *		Description: 检查资源更新的类
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CheckUpdate
{
    /// <summary>
    /// 检查更新完成后 回调给上层
    /// </summary>
    private Action updateFinish = null;

    public CheckUpdate(Action callback)
    {
        updateFinish = callback;
    }

    // 需要更新的资源的列表
    private List<string> NeedDownLoadList = new List<string>();

    // 本地version.txt文件的存放路径路径
    private string localVersionFilePath = "";

    // 本地资源名称的集合
    private Dictionary<string, string> localDic = new Dictionary<string, string>();
    // 服务端资源名称的集合
    private Dictionary<string, string> serverDic = new Dictionary<string, string>();

    // 记录下载完成的数量
    private int alreadDownLoadCount = 0;

    // 1.下载服务端的version.txt
    public void DownLoadVersionFile()
    {
        
        // http://47.105.137.129:8089/Windows
        string versionUrl = ServerUtil.GetAssetBundleURL() + PathUtil.GetPlatformName() + "/" + "version.txt";
        Game.Web.GetStringRequest(versionUrl, DownLoadVersionFileCallback, downLoadErrorCallback);
    }



    // 下载完成version.txt的回调
    private void DownLoadVersionFileCallback(string content)
    {
        localDic.Clear();
        serverDic.Clear();
        NeedDownLoadList.Clear();

        // 服务端所有数据
        serverDic = getDic(content);

        // 获取本地version.txt的数据
        localVersionFilePath = PathUtil.GetAssetBundlePath() + "/version.txt";

        // 如果本地不存在版本文件(是第一次进入游戏),就把服务器资源全部下载下来
        if (!File.Exists(localVersionFilePath))
        {
            foreach (var item in serverDic.Keys)
            {
                NeedDownLoadList.Add(item);
            }
        }
        else // 本地存在版本文件,读取本地版本文件数据
        {
            string localContent = File.ReadAllText(localVersionFilePath);
            localDic = getDic(localContent);

            // 服务端的版本文件数量 一定是大于等于 本地版本文件的数量 , 所以遍历服务端的数据集合
            foreach (var item in serverDic)
            {
                // 如果本地不包含该资源 或者 本地包含的资源和服务端的不一样 , 都需要加到下载列表里更新
                if (!localDic.ContainsKey(item.Key) || localDic[item.Key] != item.Value)
                {
                    NeedDownLoadList.Add(item.Key);
                }
            }

        }


        DownLoadFile();

    }

    /// <summary>
    /// 开始下载更新的文件
    /// </summary>
    private void DownLoadFile()
    {
        if(NeedDownLoadList.Count == 0)
        {
            Debug.Log("没有资源需要更新");
            if (updateFinish != null)
            {
                updateFinish();
            }
            return;
        }

        for (int i = 0; i < NeedDownLoadList.Count; i++)
        {
            int index = i;
            string localPath = PathUtil.GetAssetBundlePath() + "/" + NeedDownLoadList[index]; //本地资源的路径(可能有,可能没有)

            string dir = Path.GetDirectoryName(localPath);          
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir); //创建文件夹

            if (File.Exists(localPath)) File.Delete(localPath); //如果本地有资源,就删除资源(因为服务端更新了)

            string url = ServerUtil.GetAssetBundleURL() + PathUtil.GetPlatformName() + "/"+ NeedDownLoadList[index]; //服务端资源的地址

            Game.Web.GetWWWRequest(url, (www) =>
            {
                // 把服务端文件下载到本地
                File.WriteAllBytes(localPath, www.bytes);

                // 修改本地version.txt文件
                ModifyLocalVersionFile(NeedDownLoadList[index]);

            },

            (error) =>
            {
                Debug.LogError(url);
                Debug.LogError("更新资源出错:" + error);            
            });

        }
    }

    /// <summary>
    /// 修改本地字典的数据,写入txt文本
    /// </summary>
    /// <param name="key"></param>
    private void ModifyLocalVersionFile(string key)
    {

        // 不管本地字典里有没有,走这句话都可以更新了... 不用上面的判断
        localDic[key] = serverDic[key];
        alreadDownLoadCount++;

        // TODO 这里还可以做进度条展示 

        if (alreadDownLoadCount == NeedDownLoadList.Count) // 如果都下载完了,就保存到本地版本文件里
        {
            saveVersionFile();
        }
    }

    /// <summary>
    /// 保存本地version.txt
    /// </summary>
    private void saveVersionFile()
    {
        if (File.Exists(localVersionFilePath))
            File.Delete(localVersionFilePath);

        StringBuilder sb = new StringBuilder();
        foreach (var item in localDic)
        {
            sb.AppendLine(string.Format("{0}|{1}", item.Key, item.Value));
        }

        File.WriteAllText(localVersionFilePath, sb.ToString());

        Debug.Log("更新资源完成");
        if (updateFinish != null)
        {
            updateFinish();
        }
    }

    /// <summary>
    /// 读取version.txt,写入字典里
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    Dictionary<string, string> getDic(string content)
    {
        Dictionary<string, string> tmpDic = new Dictionary<string, string>();

        string[] conArr = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        for (int i = 0; i < conArr.Length; i++)
        {
            if (string.IsNullOrEmpty(conArr[i])) continue; // 检测是否有空行

            string[] tmp = conArr[i].Split('|');
            if (tmp.Length != 2) continue;

            tmpDic[tmp[0]] = tmp[1];
        }

        return tmpDic;
    }



    private void downLoadErrorCallback(string reason)
    {
        Debug.LogError("下载出错 " + reason);
    }
}