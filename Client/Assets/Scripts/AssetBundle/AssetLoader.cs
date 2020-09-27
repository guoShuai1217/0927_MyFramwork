/*
 *		Description:   加载资源的脚本,为上层代码提供功能的类
 *
 *		CreatedBy:  guoShuai
 *
 *		DataTime:  2019.05.21
 */
using UnityEngine;
public class AssetLoader:System.IDisposable
{

	private AssetBundle abRes;

	public AssetLoader(AssetBundle ab)
	{
		this.abRes = ab;
	}

	/// <summary>
	/// 加载单个资源(索引器)
	/// </summary> 
	public UnityEngine.Object this[string assetName]
	{
		get
		{
			if (abRes == null || !abRes.Contains(assetName))
			{
				Debug.LogError("The abRes Not Contains this assetName :" + assetName);
				return null;
			}

			return abRes.LoadAsset(assetName);
		}
	}

	/// <summary>
	/// 加载单个资源
	/// </summary>	 
	public Object LoadAsset(string assetName)
	{
		if (abRes == null || !abRes.Contains(assetName))
		{
			Debug.LogError("The abRes is Not Exists :" + assetName);
			return null;
		}

		return abRes.LoadAsset(assetName);
	}

	/// <summary>
	/// 加载多个资源
	/// </summary>
	public Object[] LoadAssetWithSubAssets(string assetName)
	{
		if (abRes == null || !abRes.Contains(assetName))
		{
			Debug.LogError("The abRes is Not Exists :" + assetName);
			return null;
		}

		return abRes.LoadAssetWithSubAssets(assetName);
	}


	/// <summary>
	/// 卸载单个资源
	/// </summary>
	public void UnLoadRes(Object obj)
	{
		Resources.UnloadAsset(obj);
	}


	/// <summary>
	/// 释放AssetBundle包
	/// </summary>
	public void Dispose()
	{
		if (abRes == null) return;

		abRes.Unload(false);
		abRes = null;
	}


	/// <summary>
	/// 获取所有的包名(测试用)
	/// </summary>
	/// <returns></returns>
	public string[] GetAllAssetNames()
	{
		return abRes.GetAllAssetNames();
	}


 

}
