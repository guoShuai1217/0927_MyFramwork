///*
// *		Description:  把AssetBundle工具和框架结合起来,这个类用于处理加载 AB 包 
// *		
// *				相当于对 ILoaderManager 进一步封装
// *
// *		CreatedBy:  guoShuai
// *
// *		DataTime:  2019.05.25
// */

//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//public class NativeResLoader : AssetsBase
//{

//	private AssetBackMsg msgBack;
//	public AssetBackMsg MsgBack
//	{
//		get
//		{
//			if(msgBack == null)
//			{
//				msgBack = new AssetBackMsg();
//			}

//			return msgBack;
//		}
//	}


//	private NativeManager callBack = null; 
//	public NativeManager CallBack
//	{
//		get
//		{
//			if(callBack == null)
//			{
//				callBack = new NativeManager();
//			}
//			return callBack;
//		}
//	}

//	private void Awake()
//	{
//		msgArr = new ushort[]
//		{
//			 (ushort)AssetsEvent.UnLoadAsset,

//			 (ushort)AssetsEvent.UnLoadBundleAsset,

//			 (ushort)AssetsEvent.UnLoadAllBundleAsset,

//			 (ushort)AssetsEvent.DisposeBundle,

//			 (ushort)AssetsEvent.DisposeAllBundle,

//			 (ushort)AssetsEvent.ReleaseAll,

//			 (ushort)AssetsEvent.LoadAsset,

//		};

//		Regist(this, msgArr); // 注册消息

//	}


//	public override void Execute(MessageBase msg)
//	{
//		base.Execute(msg);
 
//        switch (msg.msgId)
//		{
			
//			case (ushort)AssetsEvent.UnLoadAsset: // 释放一个bundle包里的单个资源
//				{
//					AssetMsg tmpMsg1 = (AssetMsg)msg;
//					LoaderManager.Instance.UnloadAsset(tmpMsg1.sceneName, tmpMsg1.fileName, tmpMsg1.assetName);
//					break;
//				}

//			case (ushort)AssetsEvent.UnLoadBundleAsset: // 释放一个bundle包里的全部资源
//				{
//					AssetMsg tmpMsg2 = (AssetMsg)msg;
//					LoaderManager.Instance.UnloadAsset(tmpMsg2.sceneName, tmpMsg2.fileName);
//					break;
//				}

	
//			case (ushort)AssetsEvent.UnLoadAllBundleAsset: // 释放所有bundle包里的资源
//				{
//					AssetMsg tmpMsg3 = (AssetMsg)msg;

//					LoaderManager.Instance.UnloadAsset(tmpMsg3.sceneName);

//					break;
//				}


//			case (ushort)AssetsEvent.DisposeBundle: // 释放单个bundle包
//				{
//					AssetMsg tmpMsg4 = (AssetMsg)msg;

//					LoaderManager.Instance.Dispose(tmpMsg4.sceneName, tmpMsg4.fileName);
//					break;

//				}

//			case (ushort)AssetsEvent.DisposeAllBundle: // 释放所有bundle包
//				{
//					AssetMsg tmpMsg5 = (AssetMsg)msg;

//					LoaderManager.Instance.Dispose(tmpMsg5.sceneName);
//					break;
//				}

//			case (ushort)AssetsEvent.ReleaseAll: // 释放所有bundle包 和 bundle包里的资源
//				{
//					AssetMsg tmpMsg6 = (AssetMsg)msg;

//					LoaderManager.Instance.ReleaseAllBundleAndAssets(tmpMsg6.sceneName);
//					break;
//				}


//			case (ushort)AssetsEvent.LoadAsset: // 加载资源
//				{
//					AssetMsg tmpMsg7 = (AssetMsg)msg;

//					LoadAssetBundle(tmpMsg7.sceneName, tmpMsg7.fileName, tmpMsg7.assetName, tmpMsg7.isSingle, tmpMsg7.backMsgId);

//					break;
//				}
		 
//		}


//	}

//	/// <summary>
//	/// 加载请求的资源
//	/// </summary>
//	/// <param name="sceneName">场景名</param>
//	/// <param name="fileName">文件夹名</param>
//	/// <param name="assetName">资源名</param>
//	/// <param name="isSingle">是否是单个资源</param>
//	/// <param name="backId">返回消息</param>
//	public void LoadAssetBundle(string sceneName,string fileName,string assetName,bool isSingle,ushort backId)
//	{
//		if (!LoaderManager.Instance.IsLoadedAssetBundle(sceneName, fileName)) //没有加载过,就去加载
//		{
//			LoaderManager.Instance.LoadAssetBundle(sceneName, fileName, LoadProgressCallBack);

//			//  bundleFullName = "scene1/test.ld" --> bundleName = "test"
//			string bundleName = LoaderManager.Instance.GetBundleName(sceneName, fileName); // 这才是真正的 bundleName

//			if (bundleName != null)
//			{
//				NativeNode tmpNode = new NativeNode(isSingle, sceneName, fileName, assetName, backId, SendToBackMsg, null);

//				CallBack.AddBundle(bundleName, tmpNode);

//				Debug.Log("GetResource bundleName == " + bundleName);
//			}
//			else
//				Debug.LogError("Not Contains This bundle :  " + bundleName);
//		}
//		else if (LoaderManager.Instance.IsLoadingFinished(sceneName,fileName)) // 加载过,而且已经加载完成了
//		{
//			if (isSingle)
//			{
//				Object tmpObj = LoaderManager.Instance.LoadAsset(sceneName, fileName, assetName);
//				this.MsgBack.Changer(backId, tmpObj);
//				Dispatch(MsgBack);	// 加载完了所需的资源之后,把资源封装到AssetBackMsg类里发送出去
//									//(谁需要这个资源谁就去注册这个backId就可以收到)
//			}
//            else
//            {
//                Object[] tmpObj = LoaderManager.Instance.LoadAssetWithSubAssets(sceneName, fileName, assetName);
//                this.MsgBack.Changer(backId, tmpObj);
//                Dispatch(MsgBack);   
//            }

//        }
//		else // 加载过,但是还没有加载完
//		{
//			string bundleName = LoaderManager.Instance.GetBundleName(sceneName, fileName);

//			if (bundleName != null)
//			{
//				NativeNode tmpNode = new NativeNode(isSingle, sceneName, fileName, assetName, backId, SendToBackMsg, null);

//				CallBack.AddBundle(bundleName, tmpNode);

//			}
//			else
//				Debug.LogError("Not Contains This bundle :  " + bundleName);
//		}

//	}

//	// 加载进度回调
//	public void LoadProgressCallBack(string bundleName,float progress)
//	{
//		Debug.Log("Loading " + bundleName + " ;   Progress :  " + progress);
//		if(progress >= 1.0f)
//		{
//			CallBack.CallBackRes(bundleName); // 加载完了包,就去加载所需的资源,然后发送出去
//			CallBack.Dispose(bundleName);
//		}
//	}

//	/// <summary>
//	/// NativeNode 里的 callback 的回调
//	/// </summary>
//	public void SendToBackMsg(NativeNode node)
//	{
//		if (node.isSingle)
//		{
//			Object obj = LoaderManager.Instance.LoadAsset(node.sceneName, node.fileName, node.assetName);

//			MsgBack.Changer(node.backMsgId, obj);

//			Dispatch(MsgBack); // 把消息发送出去
//		}
//		else
//		{
//			Object[] objArr = LoaderManager.Instance.LoadAssetWithSubAssets(node.sceneName, node.fileName, node.assetName);
//			MsgBack.Changer(node.backMsgId, objArr);
//			Dispatch(MsgBack);
//		}
//	}

//}



