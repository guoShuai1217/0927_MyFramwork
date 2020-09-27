/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Reflection;
public class DialogMgr : BaseMonoBehaviour
{
    private class DialogParameter
    {
        public string _strPath;
        public object[] _paras;


        public DialogParameter(string strPath, params object[] paras)
        {
            _strPath = strPath;
            _paras = paras;
        }
    }

    private List<UIDialogBase> cacheList = new List<UIDialogBase>();
    private List<UIDialogBase> openedList = new List<UIDialogBase>();

    private Queue<DialogParameter> queue = new Queue<DialogParameter>();
    private Queue<DialogParameter> tipsQueue = new Queue<DialogParameter>();

    /// <summary>
    /// 回调事件，用于在dialog界面打开时被阻挡的事件处理
    /// </summary>
    public Action<string, bool> OnDialogCallBack;

    private void Update()
    {
        try
        {
            if (queue.Count != 0 && !IsDialogActive)
            {
                DialogParameter parameter = queue.Dequeue();
                UIDialogBase activeDialog = LoadScene(parameter._strPath); // 去加载这个UI界面
                if (activeDialog != null)
                {
                    openedList.Add(activeDialog); // 加到openedList里
                    activeDialog.transform.SetAsLastSibling();
                    activeDialog.OnSceneActivated(parameter._paras); // 场景打开的时候，把参数传递进去
                }
                else
                {
                    PPP.pppShow();
                }
            }

            if (tipsQueue.Count != 0)
            {
                DialogParameter parameter = tipsQueue.Dequeue();
                UIDialogBase dialog = LoadScene(parameter._strPath);
                dialog.transform.SetAsLastSibling();
                dialog.OnSceneActivated(parameter._paras);
            }
        }
        catch (Exception ex)
        {
            PPP.pppShow(true, ex.ToString());
        }
    }

    public bool IsDialogActive
    {
        get
        {
            return (openedList.Count > 0);
        }
    }

    /// <summary>
    /// 这个UI界面是否是打开的
    /// </summary>
    /// <param name="strPath"></param>
    /// <returns></returns>
    public bool IsDialogSceneActive(string strPath)
    {
        for (int i = 0; i < openedList.Count; i++)
        {
            if (openedList[i].name == strPath && openedList[i].IsScreenActivated)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取这个UI界面
    /// </summary>
    /// <param name="strPath"></param>
    /// <returns></returns>
    public UIDialogBase getOpenDialogScene(string strPath)
    {
        for (int i = 0; i < openedList.Count; i++)
        {
            if (openedList[i].name == strPath)
            {
                return openedList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 加载UI界面
    /// </summary>
    /// <param name="strPrefabePath"></param>
    /// <returns></returns>
    public UIDialogBase LoadScene(string strPrefabePath)
    {
        UIDialogBase dialog = FindInCache(strPrefabePath);


        if (dialog == null)
        {
            GameObject newSceneGameObject = Resources.Load<GameObject>(strPrefabePath);

            if (newSceneGameObject != null)
            {
                newSceneGameObject = UIUtils.AddChild(this.gameObject, newSceneGameObject);
                if (newSceneGameObject != null)
                {
                    dialog = newSceneGameObject.GetComponent<UIDialogBase>();
                    if (dialog == null)
                    {
                        newSceneGameObject.name = newSceneGameObject.name.Replace("(Clone)", "");
                        dialog = PPPUIBase.addScript(newSceneGameObject, newSceneGameObject.name) as UIDialogBase;
                    }

                    if (dialog != null)
                    {
                        dialog.name = strPrefabePath;
                        dialog.InitializeScene();
                    }

                }
                else
                {
                    PPP.pppShow(true, "UISystem::LoadDialog() Failed to add new scene to parent UISystem with name: " + strPrefabePath);
                }
            }
            else
            {
                PPP.pppShow(true, "UISystem::LoadDialog() Failed to load new scene with name: " + strPrefabePath);
            }
        }

        return dialog;
    }

    /// <summary>
    /// 从缓存里找UI界面
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    UIDialogBase FindInCache(string key)
    {
        //foreach (UIDialogBase dialog in cacheList)
        //{
        //    if (dialog.name == key)
        //    {
        //        cacheList.Remove(dialog);
        //        return dialog;
        //    }
        //}

        // guoShuai 修改
        for (int i = 0; i < cacheList.Count; i++)
        {
            if (cacheList[i].name == key)
            {
                UIDialogBase tmpDialog = cacheList[i];
                cacheList.Remove(cacheList[i]);
                return tmpDialog;
            }
        }

        return null;
    }

    /// <summary>
    /// 加载UI界面
    /// </summary>
    /// <param name="strPath"></param>
    /// <param name="paras"></param>
    public void PushDialog(string strPath, params object[] paras)
    {
        if (strPath == null || strPath.Length == 0)
        {
            PPP.pppShow();
            return;
        }
        DialogParameter parameter = new DialogParameter(strPath, paras);
        queue.Enqueue(parameter);
    }

    /// <summary>
    /// 立刻加载UI界面
    /// </summary>
    /// <param name="strPath"></param>
    /// <param name="paras"></param>
    /// <returns></returns>
    public UIDialogBase PushDialogImmediately(string strPath, params object[] paras)
    {
        DialogParameter parameter = new DialogParameter(strPath, paras);
        UIDialogBase activeDialog = LoadScene(parameter._strPath);
        if (activeDialog == null)
        {
            PPP.pppShow();
            return null;
        }
        openedList.Add(activeDialog);
        activeDialog.transform.SetAsLastSibling();
        activeDialog.OnSceneActivated(parameter._paras);
        return activeDialog;
    }

    public void PushTips(string dialog, params object[] paras)
    {
        DialogParameter parameter = new DialogParameter(dialog, paras);
        tipsQueue.Enqueue(parameter);
    }

    public void Cache(UIDialogBase dialog, bool isOk)
    {
        if (null != dialog)
        {
            openedList.Remove(dialog);
            if (!cacheList.Contains(dialog))
            {
                cacheList.Add(dialog);
            }
            if (null != OnDialogCallBack)
            {
                OnDialogCallBack(dialog.name, isOk);
            }
        }
    }

    public void OnBackPressed()
    {
        if (openedList.Count > 0)
        {
            openedList[openedList.Count - 1].OnBackPressed();
        }
    }
    public bool isDialogQueueContain(string strDialogPath)
    {
        bool ret = false;
        foreach (var item in Game.DialogMgr.queue)
        {
            if (item._strPath.Equals(strDialogPath))
            {
                ret = true;
                break;
            }
        }
        return ret;
    }
}
