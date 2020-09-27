using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
#if PPP_TEST
#else
using DG.Tweening;
#endif

[ExecuteInEditMode]
public class UISceneBase : PPPUIBase
{
    /* Whether or not to show the scene below this one in the stack */
    //是否要在堆栈下面显示这个场景
    [HideInInspector]
    public bool HideOldScenes = true;
    [HideInInspector]
    public bool BackPopPreScenes = false;

    public string Page;

    public bool IsScreenActivated = false;

    public SceneGame scene3D;
    void Start()
    {
        //#if UNITY_EDITOR
        //SetParent();
        //#endif
        //this.transform.SetParent(root.transform);
        RectTransform rect = this.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            rect.localScale = Vector3.one;
        }
    }

    void SetParent()
    {
        UIMgr root = GameObject.FindObjectOfType<UIMgr>();
        if (null != root)
        {
            Transform parent = this.transform.parent;
            if (parent != root.transform)
            {
                this.transform.SetParent(root.transform);
                RectTransform rect = this.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.offsetMax = Vector2.zero;
                    rect.offsetMin = Vector2.zero;
                    rect.localScale = Vector3.one;
                }
            }
        }
    }
    /* Initialization function that is called immediately after this scene is created */
    //在此场景创建后立即调用的初始化函数
    public virtual void InitializeScene()
    {
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
        }
        Button[] allBtns = GetComponentsInChildren<Button>();
        for (int i = 0; i < allBtns.Length; i++)
        {
            Button button = allBtns[i];
            addButtonClickEffect(button);
        }
    }

    /* Function called when this scene is to be destroyed (allows for deallocation of memory or whatnot) */
    //当这个场景被销毁时函数被调用(允许存储内存或其他东西)
    public virtual void DestroyScene()
    {
    }

    /* Function called every time this scene is opened */
    //每当这个场景被打开时函数就会被调用
    public virtual void OnSceneOpened(params object[] sceneData)
    {
        ShowScene();
    }

    /* Function called every time this scene is closed */
    //每当这个场景被关闭时函数就会被调用
    public virtual void OnSceneClosed()
    {
        HideScene();
    }

    /* Function called every time this scene becomes the active scene */
    //每当这个场景变成活动场景时函数就会被调用
    public virtual void OnSceneActivated(params object[] sceneData)
    {
        IsScreenActivated = true;

        ShowScene();

        //		if(EnterAnimation != null)
        //		{
        //			EnterAnimation.Play();
        //		}
    }

    /* Function called every time this scene becomes deactivated (no longer the active scene) */
    //每当这个场景被禁用时函数就会被调用(不再是活动场景)
    public virtual void OnSceneDeactivated(bool hideScene)
    {
        IsScreenActivated = false;

        if (hideScene == true)
        {
            HideScene();
        }
    }

    /* Function that hides this scene */
    //隐藏这个场景的函数
    public virtual void HideScene()
    {
        if (scene3D != null)
        {
            scene3D.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    /* Function that unhides this scene */
    //将这个场景隐藏起来的函数
    public virtual void ShowScene()
    {
        gameObject.SetActive(true);

        //RectTransform rect = GetComponent<RectTransform>();
        //rect.SetAsFirstSibling();
    }

    public virtual void OnBackPressed()
    {
#if PPP_TEST
#else
       // Game.SoundManager.PlayClick();
        Game.UIMgr.PopScene();
#endif
    }

    public virtual void SetAllMemberValue()
    {

    }
}
