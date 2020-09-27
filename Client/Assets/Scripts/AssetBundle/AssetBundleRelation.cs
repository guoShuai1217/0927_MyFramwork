/*
 *		Description:  管理一个AB包的依赖关系和被依赖关系
 *
 *		CreatedBy:  guoShuai
 *
 *		DataTime:  2019.05.22
 */

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AssetBundleRelation
{
  
	private WWWLoader assetLoader; // 下层www加载bundle包的类 的对象


	private string bundleName;
	public string GetBundleName // 获取包名
	{
		get{ return bundleName; }
	}

	private LoadProgress lp;
	public LoadProgress GetProgress // 获取加载进度
	{
        get { return lp; }
	}

    private bool isloadFinished; // 是否加载完成
    public bool IsBundleLoadFinished
    {
        get { return isloadFinished; }
    }


    public AssetBundleRelation(string bundleName, LoadProgress lp)
	{
        isloadFinished = false;
        this.bundleName = bundleName;
        this.lp = lp;
        assetLoader = new WWWLoader(bundleName, lp, LoadFinished);

        dependenceBundleList = new List<string>();
		referBundleList = new List<string>();
	}

 

	/// <summary>
	/// 加载这个AB包完成的回调
	/// </summary>
	/// <param name="bundleName"></param>
	public void LoadFinished(string bundleName)
	{
        Debug.Log(bundleName + " --> 加载完成");
		isloadFinished = true;
	}


	#region 依赖关系


	/// <summary>
	/// 依赖关系列表
	/// </summary>
	private List<string> dependenceBundleList;

	/// <summary>
	/// 添加这个AB包依赖关系
	/// </summary>
	public void SetDependenceBundle(string[] bundleName)
	{
		if (bundleName.Length > 0)
			dependenceBundleList.AddRange(bundleName);
	}

	/// <summary>
	/// 获取这个AB包所有的依赖关系
	/// </summary>
	/// <returns></returns>
	public List<string> GetDependenceBundle()
	{
		return dependenceBundleList;
	}

	/// <summary>
	/// 移除这个AB包的依赖关系
	/// </summary>
	/// <param name="bundleName"></param>
	public void RemoveDependenceBundle(string bundleName)
	{
		dependenceBundleList.Remove(bundleName);
	}

	#endregion


	#region 被依赖关系

	/// <summary>
	/// 被依赖关系列表
	/// </summary>
	private List<string> referBundleList;

	/// <summary>
	/// 添加这个AB包被依赖关系
	/// </summary>
	/// <param name="bundleName"></param>
	public void AddReferBundle(string bundleName)
	{
		referBundleList.Add(bundleName);
	}

	/// <summary>
	/// 获取这个AB包的所有被依赖关系
	/// </summary>
	/// <returns></returns>
	public List<string> GetReferBundleList()
	{
		return referBundleList;
	}

	/// <summary>
	/// 移除这个AB包的被依赖关系
	/// </summary>
	/// <param name="bundleName"></param>
	/// <returns>是否释放了该AB包</returns>
	public bool RemoveReference(string bundleName)
	{
		referBundleList.Remove(bundleName);

		if(referBundleList.Count <= 0)
		{
			//Dispose();
			return true;
		}

		return false;
	}

	#endregion



	#region 下层提供的API


	/// <summary>
	///  协程加载 AssetBundle 包
	/// </summary>
	public IEnumerator LoadAssetBundle()
	{
		yield return assetLoader.LoadAssetBundle();
	}

	/// <summary>
	/// 获取单个资源
	/// </summary>
	public Object LoadAsset(string assetName)
	{
		if (assetLoader == null)
		{
			Debug.LogError("assetLoader is Null");
			return null;
		}
		return assetLoader.LoadAsset(assetName);
	}

	/// <summary>
	/// 获取多个资源
	/// </summary>
	public Object[] LoadAssetWithSubAssets(string assetName)
	{
		if (assetLoader == null)
		{
			Debug.LogError("assetLoader is Null");
			return null;
		}
		return assetLoader.LoadAssetWithSubAssets(assetName);
	}

	/// <summary>
	/// 释放AB包
	/// </summary>
	public void Dispose()
	{
		if (assetLoader == null)
		{
			Debug.LogError("assetLoader is Null");
			return ;
		}
		assetLoader.Dispose();
	}

	/// <summary>
	/// 获取所有的资源名称(测试用)
	/// </summary>
	public string[] GetAllAssetNames()
	{
		if(assetLoader == null)
		{
			Debug.LogError("assetLoader is Null");
			return null;
		}
		return assetLoader.GetAllAssetNames();
	}

	#endregion

}

