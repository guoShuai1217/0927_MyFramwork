using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;



public class UIMgr : MonoBehaviour
{
    void Awake()
    {
        Camera uica = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        Canvas ca = this.transform.GetComponent<Canvas>();
        if (ca && uica)
        {
            ca.worldCamera = uica;
        }
    }
    /* List of scenes that have been loaded and are ready to be opened */
    protected List<UISceneBase> LoadedScenes = null;

    /* List of opened scenes */
    protected List<UISceneBase> OpenScenes;

    /* The current active scene (has the focus) */
    [HideInInspector]
    public UISceneBase ActiveScene = null;

    public string FrontEndPage;

    #region UI common
    /* Function used to initialize the UI system */
    public virtual void InitializeUISystem()
    {
        LoadedScenes = new List<UISceneBase>();
        OpenScenes = new List<UISceneBase>();
    }

    /* Function called when the UI system is destroyed */
    public virtual void DestroyUISystem()
    {
    }

    protected virtual void OnBackPressed()
    {
        if (Game.DialogMgr.IsDialogActive)
        {
            Game.DialogMgr.OnBackPressed();
        }
        else if (ActiveScene != null && ActiveScene.gameObject.activeInHierarchy == true)
        {

            ActiveScene.OnBackPressed();
        }
    }

    /* Makes the scene passed in the active/focused scene */
    protected void SetActiveScene(UISceneBase newActiveScene, params object[] sceneData)
    {
        // 解决商城中技能跳转到商城财宝，再点返回时界面空白问题
        //if (newActiveScene != ActiveScene)
        {
            // Notify deactivation on any possible previous active screen
            if (ActiveScene != null/* && ActiveScene.IsScreenActivated == true*/)
            {
                ActiveScene.OnSceneDeactivated(newActiveScene.HideOldScenes);
                if (ActiveScene.scene3D != null)
                {
                    ActiveScene.scene3D.gameObject.SetActive(false);
                }
            }
            // Set the new active scene


            ActiveScene = newActiveScene;

            if (ActiveScene.scene3D != null)
            {
                ActiveScene.scene3D.gameObject.SetActive(true);
            }
            else
            {
                RectTransform rect = ActiveScene.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.SetAsLastSibling();
                }
            }

            // Notify the new active scene that it is the active scene
            ActiveScene.OnSceneActivated(sceneData);
        }
    }

    /** Checks if the given scene is the active scene */
    public virtual bool IsSceneActive(string inPage)
    {
        if (ActiveScene != null)
        {
            if (ActiveScene.Page == inPage)
            {
                return true;
            }
        }

        return false;
    }

    /* Pushes a new scene to the top of the open stack */
    public virtual UISceneBase PushScene(string page, params object[] sceneData)
    {
        UISceneBase openedScene = OpenScene(page, sceneData);

        // Make the newly opened scene active
        if (openedScene != null &&
            (ActiveScene == null ||
                openedScene.Page != ActiveScene.Page))
        {
            /**
             * TEMPORARY HACKERY FOR DISPLAYING CHARACTER INVENTORY IN STORE UI. DO NOT USE ANYWHERE ELSE.
             * TODO: REMOVE WITH THE CHARACTER UI REFACTOR.
             * */
            //if (sceneData != null)
            //{
            //    bool HideOldScenes = DictionaryHelper.ReadBool(sceneData, "HideOldScenes", false);
            //    openedScene.HideOldScenes = HideOldScenes;
            //}
            /** END TEMPORARY HACKERY */

            SetActiveScene(openedScene, sceneData);
        }

        return openedScene;
    }

    /**
     * Closes the last scene in the stack and activates the next one
     * NOTE: You don't know the screen that PopScene will take you to so use newActiveScreenData
     * with LOTS of caution.
     * */
    public virtual void PopScene(params object[] sceneData)
    {
        //bool BackPopPreScenes = true;
        // Close the last scene
        if (OpenScenes.Count > 0)
        {
            UISceneBase poppedScene = OpenScenes[OpenScenes.Count - 1];
            if (poppedScene != null)
            {
                CloseScene(poppedScene);
            }

        }

        // Activate the next scene
        if (/*BackPopPreScenes && */OpenScenes.Count > 0)
        {
            UISceneBase nextScene = OpenScenes[OpenScenes.Count - 1];
            if (nextScene != null)
            {
                SetActiveScene(nextScene, sceneData);
            }
            else
            {
                ActiveScene = null;
            }
        }
        else
        {
            ActiveScene = null;
        }
    }

    /* Opens a UI scene. */
    public UISceneBase OpenScene(string page, params object[] sceneData)
    {
        // First iterate through the open scenes and check to see whether this scene is already open/active
        for (int idx = 0; idx < OpenScenes.Count; idx++)
        {
            // If the scene is open
            if (OpenScenes[idx] != null &&
                OpenScenes[idx].Page == page)
            {
                UISceneBase scene = OpenScenes[idx];
                OpenScenes.RemoveAt(idx);
                OpenScenes.Add(scene);
                return scene;
            }
        }

        // Load the scene (LoadScene will just return the already loaded scene if it was already loaded)
        UISceneBase openScene = LoadScene(page);

        // Add the open scene to the open list and make it active
        if (openScene != null)
        {
            OpenScenes.Add(openScene);
            openScene.OnSceneOpened(sceneData);
        }

        return openScene;
    }

    /* Closes a UI scene */
    public void CloseScene(UISceneBase closeScene, bool activateNextScene = true)
    {
        // Remove the scene from the open list
        if (OpenScenes.Remove(closeScene) == true)
        {
            // Notify deactivation on the active screen if it's still active
            if (ActiveScene != null && ActiveScene.IsScreenActivated == true)
            {
                ActiveScene.OnSceneDeactivated(true);
            }

            closeScene.OnSceneClosed();
            if (OpenScenes.Count > 0)
            {
                var node = OpenScenes[OpenScenes.Count - 1];
                if (node != null && node.scene3D == null)
                {
                    this.gameObject.SetActive(true);
                }
            }
        }
    }

    public void CloseAll()
    {
        for (int idx = 0; idx < OpenScenes.Count; idx++)
        {
            CloseScene(OpenScenes[idx], false);
        }
    }

    /* Load the scene and return the instanced scene. Now by name! */
    public UISceneBase LoadScene(string page, bool hideScene = false)
    {
        // Find the scene in the loaded list
        for (int idx = 0; idx < LoadedScenes.Count; idx++)
        {
            // NEW: find by name instead of enum
            if (LoadedScenes[idx] != null &&
                LoadedScenes[idx].Page == page)
            {
                return LoadedScenes[idx];
            }
        }

        //if (page == UIPage.Null)
        //{
        //    PPP.pppShow();
        //    return null;
        //}
        string strPrefabeName = page;
        GameObject newSceneGameObject = Resources.Load<GameObject>(strPrefabeName);

        if (newSceneGameObject == null)
        {
            PPP.pppShow();
            return null;
        }
        GameObject go = GameObject.Instantiate(newSceneGameObject) as GameObject;
        if (go == null)
        {
            PPP.pppShow();
            return null;
        }
        SceneGame objSceneGame = go.GetComponent<SceneGame>();

        UISceneBase objSceneUI = go.GetComponent<UISceneBase>();

        if (objSceneGame == null && objSceneUI == null)
        {
            go.name = go.name.Replace("(Clone)", "");
            var objTemp = PPPUIBase.addScript(go, go.name);
            if (objTemp != null)
            {
                objSceneGame = objTemp as SceneGame;
                if (objSceneGame == null)
                {
                    objSceneUI = objTemp as UISceneBase;
                }
            }
        }
        if (objSceneGame != null)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
            Transform t = go.transform;
            t.SetParent(Game.Instance.transform);
            go.layer = Game.Instance.gameObject.layer;

            if (objSceneGame.sceneUI == null)
            {
                var arrChild = objSceneGame.GetComponentsInChildren<UISceneBase>();
                if (arrChild == null || arrChild.Length == 0)
                {
                    PPP.pppShow();
                    return null;
                }
                objSceneGame.sceneUI = arrChild[0];
                objSceneGame.sceneUI.scene3D = objSceneGame;
            }
            objSceneGame.sceneUI.Page = page;

            LoadedScenes.Add(objSceneGame.sceneUI);
            // Allow the scene to initialize itself
            objSceneGame.sceneUI.InitializeScene();

            if (hideScene == true)
            {
                objSceneGame.sceneUI.HideScene();
            }
            return objSceneGame.sceneUI;
        }
        else if (objSceneUI != null)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
            Transform t = go.transform;
            t.SetParent(this.gameObject.transform);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = this.gameObject.layer;

            objSceneUI.Page = page;

            LoadedScenes.Add(objSceneUI);
            // Allow the scene to initialize itself
            objSceneUI.InitializeScene();

            if (hideScene == true)
            {
                objSceneUI.HideScene();
            }
            return objSceneUI;
        }
        else
        {
            PPP.pppShow();
        }
        return null;
    }

    // 通过枚举加载获得路径
    public string GetEnumDes(System.Enum en)
    {
        System.Type type = en.GetType();
        MemberInfo[] memInfo = type.GetMember(en.ToString());

        if (memInfo != null && memInfo.Length > 0)
        {
            object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            if (attrs != null && attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
        }

        return en.ToString();
    }

    /* Unload the scene and clear it from the various lists */
    protected void UnloadScene(UISceneBase unloadScene)
    {
        // Find the scene in the loaded list
        for (int idx = 0; idx < LoadedScenes.Count; idx++)
        {
            if (LoadedScenes[idx] != null &&
                LoadedScenes[idx] == unloadScene)
            {
                // Close it, just in case
                CloseScene(unloadScene);
                // Remove it from the loaded list
                LoadedScenes.Remove(unloadScene);
                // Call the cleanup
                unloadScene.DestroyScene();
                // Delete the scene
                if (unloadScene.scene3D == null)
                {
                    GameObject.Destroy(unloadScene.gameObject);
                }
                else
                {
                    GameObject.Destroy(unloadScene.scene3D.gameObject);
                }
                return;
            }
        }
    }

    public virtual void ResetToHomeScreen()
    {
        while (OpenScenes.Count > 0)
        {
            UISceneBase poppedScene = OpenScenes[OpenScenes.Count - 1];
            if (poppedScene != null)
            {
                CloseScene(poppedScene);
            }
        }

        PushScene(FrontEndPage);
    }
    #endregion

}
