using UnityEngine;
using System;

public class SceneGame:PPPUIBase
{
	public UISceneBase sceneUI;

	public SceneGame ()
	{
	}
	public override void Init ()
	{
		base.Init ();
		int ManualWidth = 1280;   //首先记录下你想要的屏幕分辨率的宽
		int ManualHeight = 720;   //记录下你想要的屏幕分辨率的高        //普通安卓的都是 1280*720的分辨率
		int manualHeight;
		//然后得到当前屏幕的高宽比 和 你自定义需求的高宽比。通过判断他们的大小，来不同赋值
		//*其中Convert.ToSingle（）和 Convert.ToFloat() 来取得一个int类型的单精度浮点数（C#中没有 Convert.ToFloat() ）；
		if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(ManualHeight) / ManualWidth)
		{
			//如果屏幕的高宽比大于自定义的高宽比 。则通过公式  ManualWidth * manualHeight = Screen.width * Screen.height；
			//来求得适应的  manualHeight ，用它待求出 实际高度与理想高度的比率 scale
			manualHeight = Mathf.RoundToInt(Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
		}
		else
		{   //否则 直接给manualHeight 自定义的 ManualHeight的值，那么相机的fieldOfView就会原封不动
			manualHeight = ManualHeight;
		}
		var traCamera = getChildByName ("Camera");
		if (traCamera!=null) {
			Camera camera = traCamera.GetComponentInChildren<Camera>();
			var scale = manualHeight *1.0f/ ManualHeight;
			camera.fieldOfView *= scale;//Camera.fieldOfView 视野:  这是垂直视野：水平FOV取决于视口的宽高比，当相机是正交时fieldofView被忽略
			//把实际高度与理想高度的比率 scale乘加给Camera.fieldOfView。
			//这样就能达到，屏幕自动调节分辨率的效果
		}
	}
	void Start()
	{
		//#if UNITY_EDITOR
		//SetParent();
		//#endif
		//this.transform.SetParent(root.transform);
		transform.localScale=Vector3.one;
	}
}


