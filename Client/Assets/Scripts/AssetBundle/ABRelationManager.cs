/*
 *		Description:   对一个场景下的 所有AB包的管理
 *
 *		CreatedBy:  guoShuai
 *
 *		DataTime:  2019.05.22
 */
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 开始加载AB包的委托（初始化完成后，回调到ILoaderManager开启协程,开始加载AB包）
/// </summary>
/// <param name="sceneName"></param>
/// <param name="bundleName"></param>
public delegate void  LoadAssetBundleCallBack(string sceneName, string bundleName);

public class ABRelationManager
{

	// 每一个AB包 -> 每一个AB包的依赖关系
	private Dictionary<string, AssetBundleRelation> relationDic = new Dictionary<string, AssetBundleRelation>();

	// 每一个AB包 -> 每一个AB包里面加载出来的资源
	private Dictionary<string, AssetResObj> resObj = new Dictionary<string, AssetResObj>(); // 空间换时间,不用每次都去底层加载


	private string sceneName; 

	public ABRelationManager(string sceneName)
	{
		this.sceneName = sceneName;
	}


	/// <summary>
	/// 是否已经加载过  这个AB包
	/// </summary>
	public bool IsLoadedAssetBundle(string bundleName)
	{
        return relationDic.ContainsKey(bundleName);
    }


    /// <summary>
    /// 加载AB包（供上层提供的API）
    /// </summary>
    /// <param name="bundleName">包名</param>
    /// <param name="lp">加载进度回调</param>
    /// <param name="labcb">开启协程的回调</param>
    public void LoadAB(string bundleName,LoadProgress lp,LoadAssetBundleCallBack labcb)
	{

		if (!relationDic.ContainsKey(bundleName))
		{
			AssetBundleRelation abRelation = new AssetBundleRelation(bundleName, lp);
			 
			relationDic.Add(bundleName, abRelation);

			if (labcb != null)
				labcb(sceneName, bundleName); // 回调给上层,上层收到这个回调,就开始启用协程加载这个AB包
		}
		else
		{
			Debug.Log("The LoadHelper have Contains the bundleName  :" + bundleName);
		}

	}


    /// <summary>
    /// 协程加载 AssetBundle 
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public IEnumerator LoadAssetBundle(string bundleName)
	{
		while (!LoadManifest.Instance.IsLoadFinished()) //必须先加载 AssetBundleMainfest 文件
        {
			yield return null;
		}

		AssetBundleRelation relation = relationDic[bundleName];

		string[] dependences = LoadManifest.Instance.GetAllDependencies(bundleName); // 获取所有的依赖关系

		relation.SetDependenceBundle(dependences); // 获取依赖关系之后,抓紧记录下来

		for (int i = 0; i < dependences.Length; i++)
		{
			// 先去获取这个AB包的所有的依赖关系,之后再加载这个AB包

			yield return LoadAssetBundleDependences(dependences[i], bundleName, relation.GetProgress);
		}

		// 开启协程,开始加载 
		yield return relation.LoadAssetBundle();

	}


	/// <summary>
	/// 加载目标AB包 依赖的包 (b被a依赖,要加载a必须先加载b)
	/// </summary>
	/// <param name="bundleName">目标AB包所依赖的包的名字 b</param>
	/// <param name="referName">目标AB包 a</param>
	/// <param name="lp">加载进度</param>
	/// <returns></returns>
	private IEnumerator LoadAssetBundleDependences(string bundleName,string referName,LoadProgress lp) 
	{

		if (!relationDic.ContainsKey(bundleName))
		{
			AssetBundleRelation abRelation = new AssetBundleRelation(bundleName, lp);
			 
			if (referName != null)
			{
				abRelation.AddReferBundle(referName);
			}

			relationDic.Add(bundleName, abRelation);

			yield return LoadAssetBundle(bundleName);
		}
		else // 已经在加载队列里了 ， 只需要记录一下被依赖关系就行
		{
			if(referName != null)
			{
				AssetBundleRelation abRelation = relationDic[bundleName];
				abRelation.AddReferBundle(referName);
			}

		}


	}



	#region 释放缓存的物体

	/// <summary>
	/// 释放这个AB包里的单个资源
	/// </summary>
	public void UnloadAsset(string bundleName,string assetName)
	{
		if (resObj.ContainsKey(bundleName))
		{
			AssetResObj tmpAsset = resObj[bundleName];
			tmpAsset.UnloadAsset(assetName);
		}
	
	}


	/// <summary>
	/// 释放这个AB包里的所有资源
	/// </summary>
	/// <param name="bundleName"></param>
	public void UnloadAsset(string bundleName)
	{
		if (resObj.ContainsKey(bundleName))
		{
			AssetResObj tmpAsset = resObj[bundleName];
			tmpAsset.UnloadAsset();
		}

		Resources.UnloadUnusedAssets();
	}

	/// <summary>
	/// 释放所有资源(所有AB包里的所有资源)
	/// </summary>
	public void UnloadAsset()
	{
		List<string> bundleList = new List<string>();
		bundleList.AddRange(resObj.Keys);

		for (int i = 0; i < bundleList.Count; i++)
		{
			UnloadAsset(bundleList[i]);
		}

		resObj.Clear();
	}

	/// <summary>
	/// 释放单个AB包
	/// </summary>
	/// <param name="bundleName"></param>
	public void Dispose(string bundleName)
	{
		if (!relationDic.ContainsKey(bundleName))
		{
			Debug.LogError("LoadHelper Not Contains Key :  " + bundleName);
			return;
		}

		AssetBundleRelation abRelation = relationDic[bundleName];

		List<string> depenList = abRelation.GetDependenceBundle(); // 获取这个需要释放的AB包(a) 的所有依赖的包(b,c,d)

		for (int i = 0; i < depenList.Count; i++)
		{
			if (relationDic.ContainsKey(depenList[i]))
			{
				AssetBundleRelation depenABRlation = relationDic[depenList[i]]; // 获取b的依赖关系类
				if (depenABRlation.RemoveReference(bundleName))  // 移除 b 和 a 的被依赖关系 , 如果b只有a一个依赖的话,就把b也删掉
				{
					Dispose(depenABRlation.GetBundleName);
				}
			}
		}

		if(abRelation.GetReferBundleList().Count <= 0)
		{
			abRelation.Dispose();
			relationDic.Remove(bundleName);
		}
	}

	/// <summary>
	/// 释放所有AB包
	/// </summary>
	public void Dispose()
	{
		List<string> keyList = new List<string>();
		keyList.AddRange(relationDic.Keys);
		for (int i = 0; i < relationDic.Count; i++)
		{
			AssetBundleRelation abRela = relationDic[keyList[i]];
			abRela.Dispose(); // 释放所有AB包,就不用关心依赖关系了,都干掉
		}

		relationDic.Clear();
	}

	/// <summary>
	/// 释放所有AB包和资源
	/// </summary>
	public void ReleaseAllBundleAndAssets()
	{
		UnloadAsset(); // 释放所有资源

		Dispose(); // 释放所有AB包
	}

	


	#endregion



	#region 下层提供的API

	/// <summary>
	/// 这个AB包是否已经加载完成
	/// </summary>
	public bool IsLoadingFinished(string bundleName)
	{
		if (!relationDic.ContainsKey(bundleName))
		{
			Debug.LogError("The loadHelper Not ContainsKey :" + bundleName);
			return false;
		}

		AssetBundleRelation abRela = relationDic[bundleName];
		return abRela.IsBundleLoadFinished;
	}


	/// <summary>
	/// 获取这个AB包下的单个资源
	/// </summary>
	public UnityEngine.Object LoadAsset(string bundleName,string assetName) // scene1/test.prefab
	{
		// 先看缓存的有没有
		if (resObj.ContainsKey(bundleName))
		{
			AssetResObj tmpRes =  resObj[bundleName];
			List<Object> objList =  tmpRes.GetResObj(assetName);
			if (objList != null)
				return objList[0];
		}

		// 缓存的没有,或者缓存里没有我们需要的Object, 就去底层加载一次
		if (!relationDic.ContainsKey(bundleName))
		{
			Debug.LogError("The loadHelper Not ContainsKey :" + bundleName);
			return null;
		}

		AssetBundleRelation abRela = relationDic[bundleName];
		Object obj =  abRela.LoadAsset(assetName);

		AssetObj tmpObj = new AssetObj(obj);

		if (resObj.ContainsKey(bundleName)) // 如果缓存里有这个bundle包,获取value,加到value里去;
		{
			AssetResObj tmpResObj = resObj[bundleName];
			tmpResObj.AddResObj(assetName, tmpObj);
		}
		else // 缓存里没有这个 bundle包,直接加进去
		{
			AssetResObj assetRes = new AssetResObj(assetName,tmpObj);
			resObj.Add(bundleName, assetRes);
		}

		return obj;
	}


	/// <summary>
	/// 获取这个AB包下的多个资源
	/// </summary>
	public Object[] LoadAssetWithSubAssets(string bundleName,string assetName)
	{
		if (resObj.ContainsKey(bundleName))
		{
			AssetResObj tmpRes = resObj[bundleName];
			List<Object> objList =  tmpRes.GetResObj(assetName);
			if (objList != null)
				return objList.ToArray();
		}

		if (!relationDic.ContainsKey(bundleName))
		{
			Debug.LogError("The loadHelper Not ContainsKey :" + bundleName);
			return null;
		}

		AssetBundleRelation abRela = relationDic[bundleName];
		Object[] tmpObjs =  abRela.LoadAssetWithSubAssets(assetName);

		AssetObj assetObj = new AssetObj(tmpObjs);

		if (resObj.ContainsKey(bundleName))
		{
			AssetResObj tmpAsset = resObj[bundleName];
			tmpAsset.AddResObj(assetName, assetObj);		 
		}
		else
		{
			AssetResObj assetRes = new AssetResObj(assetName,assetObj);
			resObj.Add(bundleName, assetRes);
		}


		return tmpObjs;
	}

	/// <summary>
	/// 获取这个AB包下的所有资源名字
	/// </summary>
	/// <param name="bundleName"></param>
	/// <returns></returns>
	public string[] GetAllAssetNames(string bundleName)
	{
		if (!relationDic.ContainsKey(bundleName))
		{
			Debug.LogError("The loadHelper Not ContainsKey :" + bundleName);
			return null;
		}

		AssetBundleRelation abRela = relationDic[bundleName];
		return abRela.GetAllAssetNames();
	}


 


	#endregion



}

#region 缓存AB包加载出来的资源的两个类


/// <summary>
/// 单个Object资源
/// </summary>
public class AssetObj
{
	public List<Object> objList;

	public AssetObj(params Object[] obj)
	{
		objList = new List<Object>();

		objList.AddRange(obj);
	}

	/// <summary>
	/// 释放资源
	/// </summary>
	public void UnloadAsset()
	{
		for (int i = 0; i < objList.Count; i++)
		{
			Resources.UnloadAsset(objList[i]);
		}
	}
}

 
/// <summary>
/// 一个AB包里的资源(多个 Object)
/// </summary>
public class AssetResObj
{
	// 资源名 --> 资源
	private Dictionary<string, AssetObj> assetDic;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="assetName">资源名</param>
	/// <param name="obj">资源名对应的Obejct</param>
	public AssetResObj(string assetName,AssetObj obj)
	{
		assetDic = new Dictionary<string, AssetObj>();

		assetDic.Add(assetName, obj);
	}

	/// <summary>
	/// 添加 资源名 --> 资源名对应的Object
	/// </summary>
	public void AddResObj(string assetName,AssetObj obj)
	{
		assetDic.Add(assetName, obj);
	}

	/// <summary>
	/// 获取资源名 -> 对应的所有资源
	/// </summary>
	public List<Object> GetResObj(string assetName)
	{
		if (assetDic.ContainsKey(assetName))
			return assetDic[assetName].objList;

		Debug.Log("The assetDic Not Contains Key  :" + assetName);
		return null;
	}

	/// <summary>
	/// 释放单个资源
	/// </summary>
	public void UnloadAsset(string assetName)
	{
		if (assetDic.ContainsKey(assetName))
		{
			assetDic[assetName].UnloadAsset();
		}
		else
		{
			Debug.LogError("The Release name is Not Exist  :" + assetName);
		}
	}

	/// <summary>
	/// 释放所有资源
	/// </summary>
	public void UnloadAsset()
	{
		List<string> keyList = new List<string>();
		keyList.AddRange(assetDic.Keys);

		for (int i = 0; i < keyList.Count; i++)
		{
			UnloadAsset(keyList[i]);
		}
	 
	}
}

#endregion