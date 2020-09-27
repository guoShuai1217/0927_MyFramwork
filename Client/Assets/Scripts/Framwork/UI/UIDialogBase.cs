using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


[ExecuteInEditMode]
public class UIDialogBase : PPPUIBase
{
    public bool IsScreenActivated = false;

    // protected GameObject BlackMask;
    //protected GameObject BoxCollider;
    //private Action<bool> callback;
#if PPP_TEST
#else
    public UtilBase EnterAnimatioin;
    public UtilBase ExitAnimatioin;
#endif

    /* Initialization function that is called immediately after this scene is created */
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
    /* Function called every time this scene becomes the active scene */
    public virtual void OnSceneActivated(params object[] sceneData)
    {
        //Action<bool> callback = null, 
        //this.callback = callback;
        IsScreenActivated = true;
#if PPP_TEST
#else
        UIUtils.SetActive(this.gameObject, true);
#endif
        //PlayEnterAnimation();

    }
    public void EnterAnim()
    {
        if (transform.Find("Root"))
        {
            transform.Find("Root").DOKill();
            transform.Find("Root").DOScale(0, 0.1f).From().OnComplete(() => transform.Find("Root").DOPunchScale(Vector3.one / 10, 0.1f, 10, 0.1f));
        }
    }

    /* Function called every time this scene becomes deactivated (no longer the active scene) */
    public virtual void OnSceneDeactivated()
    {
        IsScreenActivated = false;
#if PPP_TEST
#else
        UIUtils.SetActive(this.gameObject, false);
#endif
    }

    public virtual void OnBackPressed()
    {
        PlayExitAnimation();
    }

    public virtual void SetAllMemberValue()
    {

    }

    public virtual void PlayEnterAnimation()
    {
#if PPP_TEST
#else
        if (null != EnterAnimatioin)
        {
            EnterAnimatioin.Play();
        }
#endif
    }

    public virtual void PlayExitAnimation(bool isOk = false)
    {
#if PPP_TEST
		OnExitFinish(isOk);
#else
        if (null != ExitAnimatioin)
        {
            ExitAnimatioin.Play();
        }
        else
        {
            OnExitFinish(isOk);
        }
#endif
    }

    public void OnExitFinish(bool isOk)
    {
        OnSceneDeactivated();
#if PPP_TEST
#else
        Game.DialogMgr.Cache(this, isOk);
#endif
    }
}
