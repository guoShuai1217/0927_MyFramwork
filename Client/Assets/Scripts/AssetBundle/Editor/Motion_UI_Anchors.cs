//------------------------------------------------------------------------------
// Copyright 2017 KeMingJuJiang. All rights reserved.
// 
// Author: Xu GuangLiang
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// 自动绑定UI的锚点
/// </summary>
public class Motion_UI_Anchors : Editor
{
	[MenuItem("guoShuai/自动对齐锚点")]
	public static void SetAnchorsEditor()
	{
		Transform[] transforms = Selection.transforms;
		foreach (Transform item in transforms)
		{
			SetAnchors(item.gameObject.GetComponent<RectTransform>());
		}
	}
	static void SetAnchors(RectTransform itemRect)
	{
		Vector2 pivot = new Vector2(0.5f, 0.5f);
		if (itemRect.pivot == pivot)
		{

			#region 把锚点放在父物体的中心点上
			Vector3 localPos = itemRect.localPosition;
			Rect s_rect = itemRect.rect;
			itemRect.anchorMax = new Vector2(0.5f, 0.5f);
			itemRect.anchorMin = new Vector2(0.5f, 0.5f);
			itemRect.sizeDelta = new Vector2(s_rect.width, s_rect.height);//设置大小不变
			itemRect.localPosition = localPos;
			#endregion

			#region 获取父物体的信息
			RectTransform p_rectTransform = itemRect.parent.GetComponent<RectTransform>();
			Rect p_rect = p_rectTransform.rect;
			#endregion

			#region 设置anchorMin和anchorMax
			//左边距离父物体左边的距离
			float s_l_distance = p_rect.width / 2 + (itemRect.localPosition.x - itemRect.sizeDelta.x / 2);
			float s_r_distance = p_rect.width / 2 + (itemRect.localPosition.x + itemRect.sizeDelta.x / 2);
			float s_u_distance = p_rect.height / 2 + (itemRect.localPosition.y - itemRect.sizeDelta.y / 2);
			float s_d_distance = p_rect.height / 2 + (itemRect.localPosition.y + itemRect.sizeDelta.y / 2);

			float minX = s_l_distance / p_rect.width;
			float maxX = s_r_distance / p_rect.width;
			float minY = s_u_distance / p_rect.height;
			float maxY = s_d_distance / p_rect.height;

			itemRect.anchorMin = new Vector2(minX, minY);
			itemRect.anchorMax = new Vector2(maxX, maxY);
			itemRect.sizeDelta = Vector2.zero;
			itemRect.anchoredPosition = Vector3.zero;
			#endregion
		}
		foreach (Transform item in itemRect.transform)
		{
			SetAnchors(item.gameObject.GetComponent<RectTransform>());
		}
	}
}
