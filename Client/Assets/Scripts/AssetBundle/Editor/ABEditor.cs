/*
 *		 Description:  
 *		 操作步骤如下 : 
 *		 1. MarkSign 给需要打成AB包的资源做标记;
 *		 2. BuildeAssetBundle 将带有标记的资源打成AB包;
 *		 3. 拷贝Lua脚本 把Lua脚本和AB包资源放在一起,便于从服务端下载;
 *		 4. 生成MD5文件 便于每次启动游戏的时候 检测是否更新了资源或代码
 *			 
 *		 CreatedBy:  guoShuai
 *
 *		 DataTime:  2019.05
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEditor.Experimental.Build.AssetBundle;

public class ABEditor
{

    #region 打包

    [MenuItem("AssetBundle/BuildAssetBundle/PC")]
    public static void BuildAssetBundlePC()
    {
        string outPath = PathUtil.GetAssetBundlePath(); // StreamingAssets/Windows

        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);
    }

    [MenuItem("AssetBundle/BuildAssetBundle/Android")]
    public static void BuildAssetBundleAndroid()
    {
        string outPath = PathUtil.GetAssetBundlePath(); // StreamingAssets/Windows

        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.Android);
    }

    [MenuItem("AssetBundle/BuildAssetBundle/IPhone")]
    public static void BuildAssetBundleIOS()
    {
        string outPath = PathUtil.GetAssetBundlePath(); // StreamingAssets/Windows

        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.iOS);
    }

    [MenuItem("AssetBundle/BuildAssetBundle/Web")]
    public static void BuildAssetBundleWeb()
    {
        string outPath = PathUtil.GetAssetBundlePath(); // StreamingAssets/Windows

        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.WebGL);
    }


    #endregion

    #region 删除

    [MenuItem("AssetBundle/DeleteAssetBundle")]
    public static void DeleteAssetBundle()
    {
        string outPath = PathUtil.GetAssetBundlePath();

        Directory.Delete(outPath, true);
        File.Delete(outPath + ".meta");
        AssetDatabase.Refresh();
    }
    #endregion

    #region 自动做标记

    [MenuItem("AssetBundle/MarkSign")]
    public static void MarkSign()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames(); // 删除没有用到的tag名

        string outPath = Application.dataPath + "/" + "Art";

        DirectoryInfo directoryInfo = new DirectoryInfo(outPath);
        FileSystemInfo[] dirInfo = directoryInfo.GetFileSystemInfos(); // 所有的一级文件系统(Art文件夹下面有 Scene1,Scene2,Scene3文件夹)

        foreach (FileSystemInfo item in dirInfo)
        {
            if (item is DirectoryInfo)// item as DirectoryInfo != null
            {
                string scenePath = outPath + "/" + item.Name; //  D:/FramWork/Assets/Art/Scene1
                SceneOverView(scenePath);
            }
        }

        string copyPath = PathUtil.GetAssetBundlePath();

        CopyRecoder(outPath, copyPath);

        AssetDatabase.Refresh();

    }

    /// <summary>
    /// 拷贝文件
    /// </summary>
    /// <param name="sourcePath">文件原来的路径</param>
    /// <param name="disPath">你想拷贝到的路径</param>
    private static void CopyRecoder(string sourcePath, string disPath, bool isLua = false)
    {
        DirectoryInfo dir = new DirectoryInfo(sourcePath);
        if (!dir.Exists)
        {
            Debug.Log("Path is Not Exists ");
            return;
        }

        if (!Directory.Exists(disPath))
            Directory.CreateDirectory(disPath);

        FileSystemInfo[] fileArr = dir.GetFileSystemInfos();

        string fileExtension = "";
        if (isLua)
        {
            fileExtension = ".lua";
        }
        else
        {
            fileExtension = ".txt";
        }

        for (int i = 0; i < fileArr.Length; i++)
        {
            FileInfo file = fileArr[i] as FileInfo;

            if (file != null && file.Extension == fileExtension)
            {
                string soureFile = sourcePath + "/" + file.Name;
                string disFile = disPath + "/" + file.Name;

                File.Copy(soureFile, disFile, true);
            }
         
        }
    }

    /// <summary>
    /// 遍历 Art/Scene1下的所有的文件系统
    /// </summary>
    private static void SceneOverView(string scenePath)
    {

        Dictionary<string, string> recordDic = new Dictionary<string, string>();
        ChangeHead(scenePath, recordDic);
        // 记录对应关系
        string txtFile = "Record.txt";
        string outPath = scenePath + txtFile;
        using (FileStream fs = new FileStream(outPath, FileMode.OpenOrCreate))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {

                sw.WriteLine(recordDic.Count);
                foreach (var item in recordDic)
                {
                    sw.WriteLine(item.Key + "--" + item.Value);
                }
            }
        }
    }

    /// <summary>
    /// 截取相对路径 
    /// // D:/FreamWork/Assets/Art/Scene1/Load --> Scene1/Load
    /// </summary>
    private static void ChangeHead(string outPath, Dictionary<string, string> recordDic)  // outPath = D:/FreamWork/Assets/Art/Scene1
    {


        int tmpCount = outPath.IndexOf("Assets"); // 得到了 D:/FreamWork/ 的长度
        int tmpLength = outPath.Length; // 总长度

        string subPath = outPath.Substring(tmpCount, tmpLength - tmpCount); // 得到了 Assets/Art/Scene1 

        DirectoryInfo dir = new DirectoryInfo(outPath);

        if (dir != null)
        {
            ListFiles(dir, subPath, recordDic);
        }
        else
        {
            Debug.Log("This Path is Not Exists");
        }

    }

    /// <summary>
    /// 遍历场景中的每一个功能文件夹
    /// </summary>	 
    private static void ListFiles(FileSystemInfo fileSystemInfo, string subPath, Dictionary<string, string> recordDic)
    {
        if (!fileSystemInfo.Exists)
        {
            Debug.Log(fileSystemInfo.Name + "   is Not Exists");
            return;
        }

        if (fileSystemInfo.Extension == ".meta")
            return;

        DirectoryInfo dir = fileSystemInfo as DirectoryInfo;

        FileSystemInfo[] info = dir.GetFileSystemInfos();

        foreach (FileSystemInfo item in info)
        {
            FileInfo fileInfo = item as FileInfo;
            if (fileInfo != null) // 是文件 , 就改变tag
            {
                ChangeMark(fileInfo, subPath, recordDic);
            }
            else // 不是文件,就继续遍历(递归)
            {
                ListFiles(item, subPath, recordDic);
            }
        }

    }

    /// <summary>
    /// 改变物体的tag
    /// </summary>
    private static void ChangeMark(FileInfo fileInfo, string subPath, Dictionary<string, string> recordDic)
    {
        if (fileInfo.Extension == ".meta")
            return;

        string markName = GetBundlePath(fileInfo, subPath);


        SetMark(fileInfo, markName, recordDic);

    }

    public static void SetMark(FileInfo file, string markName, Dictionary<string, string> recordDic)
    {

        //markName = markName.ToLower();

        string fullPath = file.FullName;

        // 获取 Assets之后的路径 

        int assetCount = fullPath.IndexOf("Assets");
        string assetPath = fullPath.Substring(assetCount, fullPath.Length - assetCount);

        // 改变物体的tag值
        AssetImporter ai = AssetImporter.GetAtPath(assetPath);

        ai.assetBundleName = markName.ToLower();

        if (file.Extension == ".unity")
        {
            ai.assetBundleVariant = "u3d";
        }
        else
        {
            ai.assetBundleVariant = "ld";
        }

        string subName = "";

        if (markName.Contains("/"))
        {
            subName = markName.Split('/')[1];

        }
        else
        {
            subName = markName;
        }

        string subPath = ai.assetBundleName.ToLower() + "." + ai.assetBundleVariant;

        if (!recordDic.ContainsKey(subName))
            recordDic.Add(subName, subPath);

    }

    /// <summary>
    /// 获取标记名
    /// </summary>	 
    private static string GetBundlePath(FileInfo fileInfo, string subPath) //subPath = Assets/Art\Scene1
    {
        // D:\guoShuai\Projects\FramWork\Assets\Art\Scene1\Load\testthree\Capsule.prefab
        string tmpPath = fileInfo.FullName;

        tmpPath = GetUnityPath(tmpPath);

        int assetCount = tmpPath.IndexOf(subPath);
        assetCount += subPath.Length + 1;

        int nameCount = tmpPath.LastIndexOf(fileInfo.Name); // Capsule.prefab

        int tmpLength = nameCount - assetCount;

        int tmpCount = subPath.LastIndexOf('/');

        string sceneName = subPath.Substring(tmpCount + 1, subPath.Length - tmpCount - 1);

        if (tmpLength > 0)
        {
            string subString = tmpPath.Substring(assetCount, tmpPath.Length - assetCount);
            string subName = subString.Split('/')[0];
            return sceneName + "/" + subName;
        }

        return sceneName;

    }



    /// <summary>
    /// 把windows路径(反斜杠) 转换成 Unity的路径(正斜杠)
    /// </summary>
    private static string GetUnityPath(string windowsPath)
    {
        string unityPath = windowsPath.Replace('\\', '/');
        return unityPath;
    }
    #endregion

    #region 拷贝Lua脚本

    [MenuItem("AssetBundle/拷贝Lua脚本")]
    public static void CopyLuaScript()
    {
        string sourPath = Application.dataPath + "/Scripts/Lua/XLuaLogic";
        string desPath = PathUtil.GetAssetBundlePath() + "/XLuaLogic";
        FileUtil.CopyFileOrDirectory(sourPath, desPath);
    }

    #endregion

    #region 生成MD5文件
    [MenuItem("AssetBundle/CreateMD5File(生成MD5文件)")]
    private static void CreateFile()
    {

        // outPath = E:/Shuai/AssetBundle/Assets/StreamingAssets/Windows
        string outPath = PathUtil.GetAssetBundlePath();
        string filePath = outPath + "/" + "version.txt";
        if (File.Exists(filePath))
            File.Delete(filePath);

        List<string> fileList = new List<string>();
        // file.txt文件,让它生成在Assets/StreamingAssets/windows 下
        DirectoryInfo directoryInfo = new DirectoryInfo(outPath);
        // 调用ListFiles()方法 ,遍历文件夹下的文件 -> 存到 fileList 里
        ListFiles(directoryInfo, ref fileList);

        FileStream fs = new FileStream(filePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);

        for (int i = 0; i < fileList.Count; i++)
        {
            string extension = Path.GetExtension(fileList[i]);
            if (extension.EndsWith(".meta"))
                continue;

            // 生成文件 MD5 值
            string md5 = GetFileMD5(fileList[i]);

            // E:/ Shuai / AssetBundle / Assets / StreamingAssets / Windows / Scene1 / Player1
            string value = fileList[i].Replace(outPath + "/", string.Empty);
            // value = Scene1 / Player1 , 前面的都不要了
            sw.WriteLine(value + "|" + md5);

        }

        sw.Close();
        fs.Close();

        AssetDatabase.Refresh();

    }

    /// <summary>
    /// 遍历文件夹下所有的文件
    /// </summary>
    /// <param name="fileSystemInfo"></param>
    /// <param name="list"></param>
    private static void ListFiles(FileSystemInfo fileSystemInfo, ref List<string> list)
    {
        if (fileSystemInfo.Extension == ".meta")
            return;

        DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
        FileSystemInfo[] fileSystemInfoArr = directoryInfo.GetFileSystemInfos();

        foreach (FileSystemInfo item in fileSystemInfoArr)
        {
            FileInfo fileInfoItem = item as FileInfo;

            // fileInfoItem != null 就是文件,就把该文件加到list里
            if (fileInfoItem != null)
            {
                list.Add(fileInfoItem.FullName.Replace("\\", "/"));
            }
            else // fileInfoItem == null 就是文件夹, 递归调用自己,再从该文件夹里遍历所有文件
            {
                ListFiles(item, ref list);
            }

        }

    }

    /// <summary>
    /// 获取文件 MD5 值
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static string GetFileMD5(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open);

        // 引入命名空间   using System.Security.Cryptography;
        MD5 md5 = new MD5CryptoServiceProvider();

        byte[] bt = md5.ComputeHash(fs);
        fs.Close();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bt.Length; i++)
        {
            sb.Append(bt[i].ToString("x2"));
        }

        return sb.ToString();
    }

    #endregion

    //#region MyMark

    //[MenuItem("AssetBundle/我的标记")]
    //private static void SignObject()
    //{
    //	string outPath = Application.dataPath + "/" + "Art";

    //	DirectoryInfo dirInfo = new DirectoryInfo(outPath);
    //	DirectoryInfo[] dirArr = dirInfo.GetDirectories(); // 获取Art文件夹下的所有子文件夹

    //	foreach (DirectoryInfo item in dirArr)
    //	{
    //		string subPath = outPath + "/" + item.Name;

    //		DirectoryInfo sceneDir = new DirectoryInfo(subPath);

    //		if(sceneDir == null)
    //		{
    //			Debug.LogWarning("This Path is Not Exists :" + subPath);
    //			return;
    //		}

    //		Dictionary<string, string> recordDic = new Dictionary<string, string>();

    //		string sceneName = item.Name;

    //		ProcessDirectory(sceneName, sceneDir, recordDic);

    //		OnWriteRecord(sceneName,recordDic);
    //	}

    //	AssetDatabase.Refresh();
    //}

    //private static void OnWriteRecord(string sceneName, Dictionary<string, string> recordDic)
    //{
    //	string filePath = PathUtil.GetAssetBundle();
    //	if (!Directory.Exists(filePath))
    //		Directory.CreateDirectory(filePath);

    //	filePath = filePath + "/" + sceneName + "Record.txt"; ;
    //	using (FileStream fs = new FileStream(filePath,FileMode.OpenOrCreate,FileAccess.Write))
    //	{
    //		using (StreamWriter sw = new StreamWriter(fs))
    //		{
    //			sw.WriteLine(recordDic.Count);
    //			foreach (var item in recordDic)
    //			{
    //				sw.WriteLine(item.Key + "---" + item.Value);
    //			}
    //		}
    //	}
    //}

    ///// <summary>
    ///// 遍历scene文件夹下的所有文件系统
    ///// </summary>
    //private static void ProcessDirectory(string sceneName,FileSystemInfo sceneDir,Dictionary<string,string> recordDic)
    //{

    //	DirectoryInfo dir =  sceneDir as DirectoryInfo;

    //	FileSystemInfo[] fileArr = dir.GetFileSystemInfos();

    //	foreach (FileSystemInfo item in fileArr)
    //	{
    //		if (item.Extension == ".meta") continue;

    //		FileInfo file = item as FileInfo;

    //		if(file == null)
    //		{
    //			ProcessDirectory(sceneName, item, recordDic); // 注意这里第二个参数是item,而不是file,file是空
    //		}
    //		else
    //		{
    //			if (file.Extension == ".meta") // 这个判断是不需要的,255行已经判断了
    //				continue;

    //			ChangeFileTag(file, sceneName, recordDic);
    //		}
    //	}


    //}

    //private static void ChangeFileTag(FileInfo file,string sceneName,Dictionary<string,string> recordDic)
    //{
    //	if (!file.Exists || file.Extension == ".meta") return;

    //	string bundleName = getBundleName(file,sceneName); // Load/test1

    //	// AssetInmporter 只能获取 Assets 目录之后的路径

    //	int assetIndex = file.FullName.IndexOf("Assets");

    //	string assetsPath = file.FullName.Substring(assetIndex);

    //	AssetImporter ai = AssetImporter.GetAtPath(assetsPath);
    //	ai.assetBundleName = bundleName.ToLower(); //load/test1

    //	if(file.Extension == ".unity")
    //	{
    //		ai.assetBundleVariant = "u3d"; // 场景后缀为 u3d
    //	}
    //	else
    //	{
    //		ai.assetBundleVariant = "assetbundle"; // 资源后缀为 assetbundle
    //	}


    //	string folderName = "";

    //	if (bundleName.Contains("/"))
    //		folderName = bundleName.Split('/')[1]; // test1
    //	else
    //		folderName = bundleName; // Scene1

    //	string bundlePath = ai.assetBundleName + "." + ai.assetBundleVariant;

    //	if (!recordDic.ContainsKey(folderName))
    //		recordDic.Add(folderName, bundlePath);

    //}

    //private static string getBundleName(FileInfo file,string sceneName)
    //{
    //	string windowsPath = file.FullName; // D:\Framwork\Assets\Art\Scene1\Load\test1

    //	string unityPath = windowsPath.Replace('\\', '/');

    //	// 获取scene之后的路径 

    //	int sceneIndex = unityPath.IndexOf(sceneName) + sceneName.Length;

    //	string bundlePath = unityPath.Substring(sceneIndex + 1); // Load/test1

    //	if (bundlePath.Contains("/"))
    //	{
    //		return sceneName + "/" + bundlePath.Split('/')[0]; // Scene1/Load
    //	}
    //	else
    //	{
    //		return sceneName; // Scene1
    //	}


    //}

    //#endregion


}
