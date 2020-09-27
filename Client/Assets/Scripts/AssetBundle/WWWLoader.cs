/*
 *		Description:  加载单个ab包,
 *
 *		CreatedBy:  guoShuai
 *
 *		DataTime:  2019.05
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 加载进度的委托
/// </summary>
/// <param name="bundleName"></param>
/// <param name="progress"></param>
public delegate void LoadProgress(string bundleName,float progress); // 加载进度

/// <summary>
/// 加载完成的委托
/// </summary>
/// <param name="bundleName"></param>
public delegate void LoadCompeleted(string bundleName); // 加载完成

public class WWWLoader  
{
	 
	private string bundleName; // 包名

	private string bundlePath; // 包的路径
 
	private WWW www; // www加载对象

	private LoadProgress lp; // 加载进度的委托

    private LoadCompeleted lc; // 加载完成的委托

	private AssetLoader abRes; // 下层资源加载类的对象


	public WWWLoader( string bundleName, LoadProgress lp,LoadCompeleted lc)
	{
		this.bundleName = bundleName;
		this.bundlePath = PathUtil.GetWWWPath() + "/" + bundleName;		 
		www = null;
		this.lp = lp;
		this.lc = lc;		
		abRes = null;
	}
	
	 

	/// <summary>
	/// 协程加载 AssetBundle 包
	/// </summary>	 
	public IEnumerator LoadAssetBundle()
	{
		www = new WWW(bundlePath);
		while (!www.isDone)
		{ 
			if (lp != null) lp(bundleName, www.progress);
			yield return www.progress;
 
		}

		if(www.progress >= 1.0) // 加载完成
		{		
			abRes = new AssetLoader(www.assetBundle); // 这里注意顺序 , 先把 www.assetBundle 赋值给 abRes, 再回调上去

			if (lp != null) lp(bundleName, www.progress);
			if (lc != null) lc(bundleName);

		}
		else
		{
			Debug.LogError("Load Bundle Error  :" + bundleName);
		}

		www = null;
	}




	#region 为上层提供的接口

	/// <summary>
	/// 加载单个资源
	/// </summary>
	public UnityEngine.Object GetResources(string assetName)
	{
		if (abRes == null)
		{
			Debug.LogError("The abRes is Null");
			return null;
		}	
		return abRes[assetName];
	}

	/// <summary>
	/// 加载单个资源
	/// </summary>
	public Object LoadAsset(string assetName)
	{
		if (abRes == null)
		{
			Debug.LogError("The abRes is Null");
			return null;
		}
		return abRes.LoadAsset(assetName);
	}


	/// <summary>
	/// 加载多个资源
	/// </summary>
	public Object[] LoadAssetWithSubAssets(string assetName)
	{
		if (abRes == null)
		{
			Debug.LogError("The abRes is Null");
			return null;
		}
		return abRes.LoadAssetWithSubAssets(assetName);
	}

	/// <summary>
	/// 获取所有的资源名称(测试用)
	/// </summary>
	public string[] GetAllAssetNames()
	{
		if (abRes == null)
		{
			Debug.LogError("The abRes is Null");
			return null;
		}
		return abRes.GetAllAssetNames();
	}


	#endregion

	/// <summary>
	/// 卸载资源
	/// </summary>
	public void UnLoadAssetRes(UnityEngine.Object obj)
	{
		if (abRes == null)
		{
			Debug.LogError("The abRes is Null");
			return ;
		}
		abRes.UnLoadRes(obj);
	}


	/// <summary>
	///  卸载AssetBundle包
	/// </summary>
	public void Dispose()
	{
		if (abRes == null)
		{
			Debug.LogError("The abRes is Null");
			return;
		}

		abRes.Dispose();
		abRes = null;
	}

}
