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

public static class ServerUtil
{

    // http://47.105.137.129/111/6.jpg
    // http://192.168.2.144:8088/7.jpg


    /// <summary>
    /// 获取服务器IP地址
    /// </summary>
    /// <returns></returns>
    public static string GetServerURL()
    {
        if (GameConfig.PPP_DEBUG == 0)
            return "47.105.137.129";

            return "127.0.0.1";
    }


    /// <summary>
    /// 获取账号网址(获取账号下某个文件夹里的文件用)
    /// </summary>
    /// <returns></returns>
    public static string GetAccountURL()
    {
        //if (GameConfig.PPP_DEBUG == 0)
        //    return "http://47.105.137.129:8088/";
        //else
        //    return "http://192.168.2.144:8088/";

        return "http://" + GetServerURL() + ":8088/";

    }


    /// <summary>
    /// 获取ab包网址(资源热更用)
    /// </summary>
    /// <returns></returns>
    public static string GetAssetBundleURL()
    {

        //if (GameConfig.PPP_DEBUG == 0)
        //    return "http://47.105.137.129:8089/";
        //else
        //    return "http://192.168.2.144:8089/";

        return "http://" + GetServerURL() + ":8089/";
    }




}