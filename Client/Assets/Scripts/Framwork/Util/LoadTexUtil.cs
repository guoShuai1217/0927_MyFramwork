/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020-04-29
 *
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public static class LoadTexUtil  
{

    #region 加载单张图片按钮

    public static bool isLoadAllTextureFirst = true;//是否已经添加过路径记忆
    public static string historyPath = "";//记忆的路径

    /// <summary>
    /// 加载外部图片
    /// </summary>
    /// <param name="directoryName">填写GameConfigs里面的静态路径(例如pingmianPath)</param>
    public static void LoadSingleTexture(System.Action<Sprite,byte[]> callback)
    {
        OpenFileName openfilename = new OpenFileName();
        openfilename.structSize = Marshal.SizeOf(openfilename);
        openfilename.filter = "图片文件(*.jpg*.png)\0*.jpg;*.png";
        openfilename.file = new string(new char[256]);
        openfilename.maxFile = openfilename.file.Length;
        openfilename.fileTitle = new string(new char[64]);
        openfilename.maxFileTitle = openfilename.fileTitle.Length;
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        //默认路径
        openfilename.initialDir = path;
        if (isLoadAllTextureFirst)
        {
            openfilename.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//外部文件打开的路径
        }
        else
        {
            openfilename.initialDir = historyPath.Replace('/', '\\');
        }
        //添加路径记忆
        //ofn.initialDir = "D:\\MyProject\\UnityOpenCV\\Assets\\StreamingAssets";
        openfilename.title = "Open Project";

        openfilename.defExt = "JPG";//显示文件的类型
                                    //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        openfilename.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (DllOpenFileDialog.GetSaveFileName(openfilename))
        {
            //确定了图片的选择
            Game.Instance.StartCoroutine(LoadSingleImage(openfilename.file, callback));

            string[] newPath = openfilename.file.Split('\\');
            for (int i = 0; i < newPath.Length - 1; i++)
            {
                historyPath += (newPath[i] + "\\");
            }

            isLoadAllTextureFirst = false;
        }
        else
        {
            //点击的取消按键
        }
    }

    /// <summary>
    /// 从外部加载单张图片
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static IEnumerator LoadSingleImage(string path,System.Action<Sprite,byte[]> callback) //加载单张图片
    {
        // string _name = path.Substring(path.LastIndexOf("\\") + 1); //获取包含扩展名的文件名

        WWW www = new WWW("file:///" + path);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            //获取Texture
            Texture2D tx = www.texture;
            byte[] tmp = tx.EncodeToJPG();
            Sprite spr = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero);

            callback(spr, tmp);
        }

    }

    

    #endregion




}