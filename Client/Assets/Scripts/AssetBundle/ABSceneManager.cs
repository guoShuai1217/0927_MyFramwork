/*
 *		Description: 所有场景的管理类   
 *
 *		CreatedBy:  guoShuai
 *
 *		DataTime:  2019.05.24
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ABSceneManager
{

	private ABRelationManager abManager;

	/// <summary>
	/// 文件夹名 -> AB包的名字 (scene2--scene2.u3d)
	/// </summary>
	private Dictionary<string, string> allAsset;


	public ABSceneManager(string sceneName)
	{
        allAsset = new Dictionary<string, string>();

        abManager = new ABRelationManager(sceneName);
	}

	/// <summary>
	/// 读取配置文件
	/// </summary>
	/// <param name="sceneName">场景名</param>
	public void ReadConfig(string sceneName)  
	{		
		string outPath = PathUtil.GetAssetBundlePath() + "/" + sceneName + "Record.txt"; //StreamingAssets/Windows/Scene1Record.txt

		ReadConfiger(outPath);
	}

    // 读取配置文件
	private void ReadConfiger(string outPath)
	{
		using (FileStream fs = new FileStream(outPath,FileMode.Open,FileAccess.Read))
		{
			using (StreamReader sr = new StreamReader(fs))
			{
				int count = int.Parse(sr.ReadLine());

				for (int i = 0; i < count; i++)
				{
					string tmpStr =  sr.ReadLine();
					string[] sceneName = tmpStr.Split(new string[] { "--" }, System.StringSplitOptions.None);
					allAsset.Add(sceneName[0], sceneName[1]);
				}
				 
			}
		}
	}


	/// <summary>
	/// 获取包名
	/// </summary>
	public string GetBundleName(string fileName)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return null;
		}
		return allAsset[fileName];
	}

	/// <summary>
	/// 加载AB包
	/// </summary> 
	public void LoadAB(string fileName,LoadProgress lp,LoadAssetBundleCallBack callBack)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return;
		}

		string bundleName = allAsset[fileName];
		abManager.LoadAB(bundleName, lp, callBack);
	}


	#region 下层提供的功能

	/// <summary>
	/// 是否加载过这个AB包
	/// </summary>
	/// <param name="fileName">文件夹名</param>
	/// <returns></returns>
	public bool IsLoadedAssetBundle(string fileName)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return false;
		}
		return abManager.IsLoadedAssetBundle(allAsset[fileName]);
	}

	/// <summary>
	/// 这个AB包是否加载完成
	/// </summary>
	public bool IsLoadingFinished(string fileName)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return false;
		}
		return abManager.IsLoadingFinished(allAsset[fileName]);
	}

	/// <summary>
	/// 协程加载AB包
	/// </summary>
	public IEnumerator LoadAssetBundle(string bundleName)
	{
		yield return abManager.LoadAssetBundle(bundleName); 
	}

	/// <summary>
	/// 加载单个资源
	/// </summary>	 
	public Object LoadAsset(string fileName,string assetName)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return null;
		}
		return abManager.LoadAsset(allAsset[fileName], assetName);
	}

	/// <summary>
	/// 加载多个资源
	/// </summary>
	public Object[] LoadAssetWithSubAssets(string fileName, string assetName)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return null;
		}
		return abManager.LoadAssetWithSubAssets(allAsset[fileName], assetName);
	}

	/// <summary>
	/// 释放这个AB包里的单个资源
	/// </summary>
	public void UnloadAsset(string fileName,string assetName)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return ;
		}

		abManager.UnloadAsset(allAsset[fileName], assetName);
	}

	/// <summary>
	/// 释放这个AB包里的所有资源
	/// </summary>
	public void UnloadAsset(string fileName)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return;
		}

		abManager.UnloadAsset(allAsset[fileName]);
	}

	/// <summary>
	/// 释放所有资源(所有AB包里的所有资源)
	/// </summary>
	public void UnloadAsset()
	{
		abManager.UnloadAsset();
	}


	/// <summary>
	/// 释放单个AB包
	/// </summary>
	public void Dispose(string fileName)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return;
		}

		abManager.Dispose(allAsset[fileName]);
	}

	/// <summary>
	/// 释放所有AB包
	/// </summary>
	public void Dispose()
	{
		abManager.Dispose();

		allAsset.Clear();
	}

	/// <summary>
	/// 释放所有的AB包和资源
	/// </summary>
	public void ReleaseAllBundleAndAssets()
	{
		abManager.ReleaseAllBundleAndAssets();

		allAsset.Clear();
	}

	/// <summary>
	/// 获取这个AB包下的所有资源的名字
	/// </summary>
	public string[] GetAllAssetNames(string fileName)
	{
		if (!allAsset.ContainsKey(fileName))
		{
			Debug.LogError("The allAsset Not Contains Key :  " + fileName);
			return null;
		}
		return abManager.GetAllAssetNames(allAsset[fileName]);
	}


	#endregion



}

