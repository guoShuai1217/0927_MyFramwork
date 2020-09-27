//using System.Collections.Generic;
//using UnityEngine;


//public delegate void NativeRecCallBack(NativeNode node);

///// <summary>
///// 链表类
///// </summary>
//public class NativeNode
//{
//	public string sceneName;

//	/// <summary>
//	/// 文件夹名
//	/// </summary>
//	public string fileName;

//	public string assetName;

//	public ushort backMsgId; // 资源加载完了之后,发送的消息ID

//	public bool isSingle; // 是否是单个资源 

//	public NativeRecCallBack callBack;

//	public NativeNode next;

//	public NativeNode(bool isSingle, string sceneName, string bundleName,
//		string assetName, ushort backMsgId, NativeRecCallBack callBack, NativeNode next)
//	{
//		this.isSingle = isSingle;
//		this.sceneName = sceneName;
//		this.fileName = bundleName;
//		this.assetName = assetName;
//		this.backMsgId = backMsgId;

//		this.callBack = callBack;
//		this.next = next;
//	}

//	/// <summary>
//	/// 释放
//	/// </summary>
//	public void Dispose()
//	{
//		this.isSingle = false;
//		this.sceneName = string.Empty;
//		this.fileName = string.Empty;
//		this.assetName = string.Empty;
//		this.backMsgId = 0;

//		this.callBack = null;
//		this.next = null;
//	}


//}

//public class NativeManager
//{
//	public Dictionary<string, NativeNode> nodeDic;

//	public NativeManager()
//	{
//		nodeDic = new Dictionary<string, NativeNode>();
//	}

//	/// <summary>
//	/// 添加 加载AB包的请求
//	/// </summary>
//	public void AddBundle(string bundleName, NativeNode node)
//	{
//		if (!nodeDic.ContainsKey(bundleName))
//		{
//			nodeDic.Add(bundleName, node);
//			return;
//		}

//		NativeNode tmpNode = nodeDic[bundleName];

//		while (tmpNode.next != null)
//		{
//			tmpNode = tmpNode.next;
//		}

//		tmpNode.next = node;
//	}

//	/// <summary>
//	/// 加载完成了,消息向上层传递也完成了之后 ,释放链表
//	/// </summary>
//	public void Dispose(string bundleName)
//	{
//		if (!nodeDic.ContainsKey(bundleName))
//		{
//			Debug.Log("The nodeDic Not Contains Key :  " + bundleName);
//			return;
//		}

//		NativeNode tmpNode = nodeDic[bundleName];

//		while (tmpNode.next != null) // 从前往后,挨个释放
//		{
//			NativeNode currentNode = tmpNode;
//			tmpNode = tmpNode.next;
//			currentNode.Dispose();
//		}

//		tmpNode.Dispose();  // 释放最后一个 

//		nodeDic.Remove(bundleName);
//	}


//	public void CallBackRes(string bundleName)
//	{
//		if (!nodeDic.ContainsKey(bundleName))
//		{
//			Debug.Log("The nodeDic Not Contains Key :  " + bundleName);
//			return;
//		}

//		NativeNode tmpNode = nodeDic[bundleName];

//		do
//		{
//			tmpNode.callBack(tmpNode); // 把自己回调回去
//			tmpNode = tmpNode.next;
//		}
//		while (tmpNode != null);

//	}

//}