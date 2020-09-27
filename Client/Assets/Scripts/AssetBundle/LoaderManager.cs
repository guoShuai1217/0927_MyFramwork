/*
 *		Description: 外界调用的接口,外界加载AB包只需要知道这一个类就可以了   
 *
 *		CreatedBy:  guoShuai
 *
 *		DataTime:  2019.05.24
 */
using System.Collections.Generic;
using UnityEngine;

public class LoaderManager : MonoBehaviour
{

	//  本类的 方法里传递的 bundleName 并不是真正的包名 , 是IABSceneManager下的字典allAsset的key值,value值才是真正的包名

	public static LoaderManager Instance;

	// 场景名 -> 场景下的所有AB包的管理的映射
	private Dictionary<string, ABSceneManager> loadManager = new Dictionary<string, ABSceneManager>();

 
	private void Awake()
	{
		Instance = this;

		// 1. 先加载 AssetBundleManifest 文件 
	    // StartCoroutine(LoadManifest.Instance.LoadAssetBundleManifest());		
	}

	// 2. 读取配置文件
	private void ReadConfiger(string sceneName)
	{	 
		if (!loadManager.ContainsKey(sceneName))
		{
			ABSceneManager sceneManager = new ABSceneManager(sceneName);
			sceneManager.ReadConfig(sceneName);
			loadManager.Add(sceneName, sceneManager);
		}	 
	}

	// 3.加载AssetBundle包
	/// <summary>
	/// 加载AssetBundle包
	/// </summary>
	public void LoadAssetBundle(string sceneName,string fileName,LoadProgress lp)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.Log("loadManager Not Contains Key :  " + sceneName + ", ReadConfiger first");
			ReadConfiger(sceneName); // 读取配置文件,然后存到 loadManager 里
		}

		ABSceneManager abScene = loadManager[sceneName];
		abScene.LoadAB(fileName, lp, LoadCompeleteCallBack);

	}

	// 加载AB包的回调
	private void LoadCompeleteCallBack(string sceneName,string bundleName)
	{
		if (!loadManager.ContainsKey(sceneName)) // 这里 字典里是一定要有的,不然就是哪里出错了
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName + ", That Not Allowed");
			return;
		}

		ABSceneManager tmpManager = loadManager[sceneName];

		StartCoroutine(tmpManager.LoadAssetBundle(bundleName)); // 开启协程,加载AB包
	}


	/// <summary>
	/// 是否加载过这个AB包
	/// </summary>
	public bool IsLoadedAssetBundle(string sceneName,string fileName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.Log("没有加载过这个场景 : " + sceneName);
			return false;
		}

		ABSceneManager abScene = loadManager[sceneName];
		return abScene.IsLoadedAssetBundle(fileName);
	}

	/// <summary>
	/// 这个AB包是否加载完成
	/// </summary>
	public bool IsLoadingFinished(string sceneName, string fileName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return false;
		}

		ABSceneManager abScene = loadManager[sceneName];
		return abScene.IsLoadingFinished(fileName);
	}


	/// <summary>
	/// 获取包名
	/// </summary>
	/// <param name="sceneName"></param>
	/// <param name="fileName"></param>
	/// <returns></returns>
	public string GetBundleName(string sceneName,string fileName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return null;
		}

		ABSceneManager tmpSceneManager = loadManager[sceneName];
		return tmpSceneManager.GetBundleName(fileName);
	}



    #region 下层提供的API


    #region 加载资源

   
    /// <summary>
    /// 加载单个资源
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="fileName">场景下的文件夹名</param>
    /// <param name="assetName">资源名</param>
    /// <returns></returns>
    public Object LoadAsset(string sceneName,string fileName,string assetName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return null;
		}

		ABSceneManager tmpManager = loadManager[sceneName];
		return tmpManager.LoadAsset(fileName, assetName);
	}



	/// <summary>
	/// 加载多个资源
	/// </summary>
	/// <param name="sceneName">场景名</param>
	/// <param name="fileName">场景下的文件夹名</param>
	/// <param name="assetName">资源名</param>
	/// <returns></returns>
	public Object[] LoadAssetWithSubAssets(string sceneName, string fileName, string assetName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return null;
		}

		ABSceneManager tmpManager = loadManager[sceneName];
		return tmpManager.LoadAssetWithSubAssets(fileName, assetName);

	}

    #endregion


    #region 卸载资源

    /// <summary>
    /// 释放这个AB包里的单个资源
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="fileName">场景下的文件夹名</param>
    /// <param name="assetName">资源名</param>
    public void UnloadAsset(string sceneName,string fileName,string assetName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return;
		}

		ABSceneManager tmpManager = loadManager[sceneName];
		tmpManager.UnloadAsset(fileName, assetName);
	}


	/// <summary>
	/// 释放这个AB包里的所有资源
	/// </summary>
	/// <param name="sceneName">场景名</param>
	/// <param name="fileName">场景下的文件夹名</param>
	/// <param name="assetName">资源名</param>
	public void UnloadAsset(string sceneName, string fileName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return;
		}

		ABSceneManager tmpManager = loadManager[sceneName];
		tmpManager.UnloadAsset(fileName);
	}


    /// <summary>
	/// 释放所有资源(当前场景名下的 所有AB包里的所有资源)
	/// </summary>
	/// <param name="sceneName">场景名</param>
	public void UnloadAsset(string sceneName)
    {
        if (!loadManager.ContainsKey(sceneName))
        {
            Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
            return;
        }

        ABSceneManager tmpManager = loadManager[sceneName];
        tmpManager.UnloadAsset();
    }


    #endregion


    #region 释放bundle包

   
    /// <summary>
    /// 释放单个AB包
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="fileName">场景下的文件夹名</param>
    public void Dispose(string sceneName,string fileName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return;
		}

		ABSceneManager tmpManager = loadManager[sceneName];
		tmpManager.Dispose(fileName);
	}



	/// <summary>
	/// 释放所有的AB包
	/// </summary>
	/// <param name="sceneName">场景名</param>
	public void Dispose(string sceneName )
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return;
		}

		ABSceneManager tmpManager = loadManager[sceneName];
		tmpManager.Dispose();
		System.GC.Collect();
	}



	/// <summary>
	/// 释放所有AB包和资源
	/// </summary>
	/// <param name="sceneName">场景名</param>
	public void ReleaseAllBundleAndAssets(string sceneName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return;
		}

		ABSceneManager tmpManager = loadManager[sceneName];
		tmpManager.ReleaseAllBundleAndAssets();
		System.GC.Collect();
	}

    #endregion


    /// <summary>
    /// 获取这个AB包下的所有资源名
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="bundleName">场景下的文件夹名</param>
    public void GetAllAssetNames(string sceneName, string bundleName)
	{
		if (!loadManager.ContainsKey(sceneName))
		{
			Debug.LogError("The LoadManager Not Contains Key :  " + sceneName);
			return;
		}

		ABSceneManager tmpManager = loadManager[sceneName];
		tmpManager.GetAllAssetNames(bundleName);
	}




	#endregion



	private void OnDestroy()
	{
		loadManager.Clear();
		System.GC.Collect();
	}





}

