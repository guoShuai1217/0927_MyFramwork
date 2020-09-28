/*
 *		Description:   加载 AssetBundle Mainfest 文件的脚本
 *
 *		CreatedBy:  guoShuai
 *
 *		DataTime:  2019.05.22
 */
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class LoadManifest  
{
	#region 单例

	private static LoadManifest instance;

	public static LoadManifest Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new LoadManifest();
			}
			return instance;
		}
	}
 
	#endregion


	private AssetBundleManifest maniFest;

	private string manifestPath;

	private bool isLoadFinished = false;

	private AssetBundle assetBundle;


	// 是否加载完成
	public bool IsLoadFinished()
	{
		return isLoadFinished;
	}


	public LoadManifest()
	{
		isLoadFinished = false;
		manifestPath = PathUtil.GetWWWPath() + "/" + PathUtil.GetPlatformName(); //注意这里的路径是: Windows/Windows 
		assetBundle = null;
		maniFest = null;
	}

	/// <summary>
	/// 加载 AssetBundleManifest 文件
	/// </summary>
	/// <returns></returns>
	public IEnumerator LoadAssetBundleManifest(Action callback = null)
	{
		WWW www = new WWW(manifestPath);
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError(www.error);
		}
		else
		{
			if(www.progress >= 1.0f)
			{
				assetBundle = www.assetBundle;
				
				maniFest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

				isLoadFinished = true;
				Debug.Log("加载AssetBundleManifest文件完成");
			}
		}		
	}

	/// <summary>
	/// 获取所有的依赖关系
	/// </summary>
	public string[] GetAllDependencies(string bundleName)
	{
		return maniFest.GetAllDependencies(bundleName);
	}


	/// <summary>
	/// 释放AB包和资源
	/// </summary>
	public void UnLoadManifest()
	{
		assetBundle.Unload(true);
	}


}

