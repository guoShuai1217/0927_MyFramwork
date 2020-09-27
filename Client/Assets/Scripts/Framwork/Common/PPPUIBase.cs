/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Reflection;

public class PPPUIBase : MonoBehaviour
{
    public PPPUIBase()
    {
    }
    public Dictionary<string, ArrayList> _dic;
    void initDic()
    {
        if (_dic == null)
        {
            _dic = new Dictionary<string, ArrayList>();
            var arr = GetComponentsInChildren<Transform>(true);
            foreach (var item in arr)
            {
                ArrayList list = null;
                _dic.TryGetValue(item.name, out list);
                if (list == null)
                {
                    list = new ArrayList();
                }
                list.Add(item);
                _dic[item.name] = list;
            }
        }
    }
    void initProperty()
    {
        initDic();

        var arrBtn = GetComponentsInChildren<Button>(true);
        if (arrBtn != null)
        {
            foreach (var item in arrBtn)
            {
                var btnTemp = item;
                item.onClick.AddListener(() =>
                {
                    this.OnClick(btnTemp);
                });
            }
        }

        var type = this.GetType();
        var arrField = type.GetFields();
        foreach (var item in arrField)
        {
            var child = getChildByName(item.Name);
            //if (PPPGameConfig.PPP_DEBUG == 1 && PPPGameConfig.haveClientRule(PPPGameConfig.CLIENT_RULE.CHECK_NAME_DEFINE))
            //{
                checkName(child, item);
            //}
            if (child != null)
            {
                var typeItem = item.FieldType;
                if (item.FieldType == typeof(GameObject))
                {
                    item.SetValue(this, child.gameObject);
                }
                else if (item.FieldType == typeof(Transform))
                {
                    item.SetValue(this, child);
                }
                else if (isUIClass(typeItem))
                {
                    try
                    {
                        var value = child.GetComponent(typeItem);
                        if (value != null)
                        {
                            item.SetValue(this, value);
                        }
                        else
                        {
                            if (isKindOfPPPUIBase(typeItem))
                            {
                                var obj = addScript(child.gameObject, typeItem.FullName);
                                if (obj != null)
                                {
                                    item.SetValue(this, obj);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        PPP.pppShow();
                        continue;
                    }
                }
                else if (typeItem.BaseType == typeof(Array))
                {
                    var strTypeOne = typeItem.FullName.Replace("[]", "");

                    var typeOne = PPP.GetType(strTypeOne);

                    var list = getChildsByName(item.Name);
                    if (list == null || typeOne == null)
                    {
                        continue;
                    }
                    if (isUIClass(typeOne))
                    {
                        try
                        {
                            var arr = Array.CreateInstance(typeOne, list.Count);
                            for (int i = 0; i < list.Count; i++)
                            {
                                var childTemp = list[i] as Transform;
                                var value = childTemp.GetComponent(typeOne);
                                if (value == null)
                                {
                                    if (isKindOfPPPUIBase(typeOne))
                                    {
                                        value = addScript(childTemp.gameObject, typeOne.FullName);
                                    }
                                }
                                arr.SetValue(value, i);
                                if (value == null)
                                {
                                    PPP.pppShow();
                                }
                            }
                            item.SetValue(this, arr);
                        }
                        catch (Exception)
                        {
                            PPP.pppShow();
                            continue;
                        }
                    }
                }

                else
                {
                    continue;
                }
            }
        }
        var time2 = DateTime.Now;
    }
    bool isUIClass(Type typeItem)
    {
        var p = typeItem.GetProperty("transform");
        if (p == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public T getChildByName<T>(string name)
    {
        var traTemp = getChildByName(name);
        if (traTemp == null)
        {
            return default(T);
        }
        if (isUIClass(typeof(T)) == false)
        {
            PPP.pppShow();
            return default(T);
        }
        if (typeof(T) == typeof(GameObject) || typeof(T) == typeof(Transform))
        {
            PPP.pppShow(true, "该方法不能获取 GameObject  Transform");
        }
        return traTemp.gameObject.GetComponent<T>();
    }
    public Transform getChildByName(string name)
    {
        initDic();
        ArrayList list = null;
        _dic.TryGetValue(name, out list);
        Transform child = null;
        if (list != null)
        {
            child = list[0] as Transform;
        }
        return child;
    }
    public ArrayList getChildsByName(string name)
    {
        initDic();
        ArrayList list = null;
        _dic.TryGetValue(name, out list);
        return list;
    }

    public static PPPUIBase addScript(GameObject obj, string strClassName)
    {
        var type = Type.GetType(strClassName);
        PPPUIBase ret = null;
        if (type != null)
        {
            ret = obj.AddComponent(type) as PPPUIBase;
            if (ret != null)
            {
                ret.initProperty();
                ret.Init();
            }
        }
        return ret;
    }
    public virtual void SetValues(params object[] sceneData)
    {

    }


    public virtual void OnClick(string str)
    {

    }

    public virtual void OnClick(Button btn)
    {
        ClickOnece(btn);
        OnClick(btn.name);
    }
   
    /// <summary>
    /// 按钮3s才能再点一次
    /// </summary>
    /// <param name="btn"></param>
    void ClickOnece(Button btn)
    {
       // Debug.Log(btn.name + "   不能点击了");
        btn.interactable = false;
        Game.Delay(1.5f, () => 
        {
            btn.interactable = true;
            //Debug.LogWarning(btn.name + "   可以点击了");
        });
    }

    public static bool isKindOfPPPUIBase(Type type)
    {
        return typeof(PPPUIBase).IsAssignableFrom(type);
    }

    public virtual void Init()
    {



    }
    public Component findChild(string strPath, Type type)
    {
        var child = transform.Find(strPath);
        if (child == null)
        {
            PPP.pppShow();
            return null;
        }
        if (isUIClass(type) == false)
        {
            PPP.pppShow();
            return null;
        }
        return child.gameObject.GetComponent(type);
    }
    public T findChild<T>(string strPath)
    {
        var child = transform.Find(strPath);
        if (child == null)
        {
            return default(T);
        }
        if (isUIClass(typeof(T)) == false)
        {
            PPP.pppShow();
            return default(T);
        }
        return child.gameObject.GetComponent<T>();
    }

    public static T newGameObject<T>(Transform parent, string strName, Vector3 pos, Vector2 size)
    {
        var obj = new GameObject(strName, new Type[1] { typeof(T) });
        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
        obj.transform.localPosition = pos;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        var ret = obj.GetComponent<T>();
        if (size != Vector2.zero)
        {
            var rect = obj.GetComponent<RectTransform>();
            if (rect == null)
            {
                rect = obj.AddComponent<RectTransform>();
            }
            rect.sizeDelta = size;
        }
        if (typeof(T).Equals(typeof(Text)))
        {
            var txt = ret as Text;
           // txt.font = Game.Instance.txtVersion.font;
            txt.fontSize = 20;
            txt.alignment = TextAnchor.MiddleCenter;
        }
        return ret;
    }

    public static GameObject newGameObject(Transform parent, string strName, Vector3 pos, Vector2 size)
    {
        var ret = new GameObject(strName);
        if (parent != null)
        {
            ret.transform.SetParent(parent);
        }
        ret.transform.localPosition = pos;
        ret.transform.localRotation = Quaternion.identity;
        ret.transform.localScale = Vector3.one;

        if (size != Vector2.zero)
        {
            var rect = ret.GetComponent<RectTransform>();
            if (rect == null)
            {
                rect = ret.AddComponent<RectTransform>();
            }
            rect.sizeDelta = size;
        }
        return ret;
    }

    public static ScrollRect newScrollView(Transform parent, string strName, Vector3 pos, Vector2 size, Vector2 sizeContent, Vector2 spacingContent, GridLayoutGroup.Axis axis, TextAnchor childAlignment, GridLayoutGroup.Constraint constraint, int constraintCount)
    {
        var scrollRect = newGameObject<ScrollRect>(parent, strName, pos, size);
        var viewport = newGameObject<Mask>(scrollRect.transform, "Viewport", Vector3.zero, size);
        var content = newGameObject<GridLayoutGroup>(viewport.transform, "Content", Vector3.zero, size);
        content.cellSize = sizeContent;
        content.spacing = spacingContent;
        content.startAxis = axis;
        content.childAlignment = childAlignment;
        content.constraint = constraint;
        content.constraintCount = constraintCount;
        scrollRect.content = content.GetComponent<RectTransform>();
        return scrollRect;
    }

    static CanvasScaler getScaler(Transform traRoot)
    {
        CanvasScaler ret = null;
        var parent = traRoot;
        while (parent != null)
        {
            ret = parent.GetComponent<CanvasScaler>();
            if (ret != null)
            {
                break;
            }
            else
            {
                parent = parent.parent;
            }
        }
        return ret;
    }
    public static void autoLayout2D(Transform traRoot)
    {
        if (traRoot == null)
        {
            PPP.pppShow();
            return;
        }
        int ManualWidth = 1280;   //首先记录下你想要的屏幕分辨率的宽
        int ManualHeight = 720;   //记录下你想要的屏幕分辨率的高        //普通安卓的都是 1280*720的分辨率
        var scaleOld = traRoot.localScale;
        var canvasScaler = getScaler(traRoot);
        if (canvasScaler == null)
        {
            PPP.pppShow();
            return;
        }
        if (canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.Expand)
        {
            //然后得到当前屏幕的高宽比 和 你自定义需求的高宽比。通过判断他们的大小，来不同赋值
            //*其中Convert.ToSingle（）和 Convert.ToFloat() 来取得一个int类型的单精度浮点数（C#中没有 Convert.ToFloat() ）；
            if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(ManualHeight) / ManualWidth)
            {
                //如果屏幕的高宽比大于自定义的高宽比 。则通过公式  ManualWidth * manualHeight = Screen.width * Screen.height；
                //来求得适应的  manualHeight ，用它待求出 实际高度与理想高度的比率 scale
                int manualHeight = Mathf.RoundToInt(Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
                var scaleY = manualHeight * 1.0f / ManualHeight;
                traRoot.transform.localScale = new Vector3(scaleOld.x, scaleOld.y * scaleY, 1f);
            }
            else if (Convert.ToSingle(Screen.height) / Screen.width < Convert.ToSingle(ManualHeight) / ManualWidth)
            {
                int manualWidth = Mathf.RoundToInt(Convert.ToSingle(ManualHeight) / Screen.height * Screen.width);
                var scaleX = manualWidth * 1.0f / ManualWidth;
                traRoot.transform.localScale = new Vector3(scaleOld.x * scaleX, scaleOld.y, 1f);
            }
        }
        else if (canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight && canvasScaler.matchWidthOrHeight == 0)
        {
            //然后得到当前屏幕的高宽比 和 你自定义需求的高宽比。通过判断他们的大小，来不同赋值
            //*其中Convert.ToSingle（）和 Convert.ToFloat() 来取得一个int类型的单精度浮点数（C#中没有 Convert.ToFloat() ）；
            if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(ManualHeight) / ManualWidth)
            {
                //如果屏幕的高宽比大于自定义的高宽比 。则通过公式  ManualWidth * manualHeight = Screen.width * Screen.height；
                //来求得适应的  manualHeight ，用它待求出 实际高度与理想高度的比率 scale
                int manualHeight = Mathf.RoundToInt(Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
                var scaleY = manualHeight * 1.0f / ManualHeight;
                traRoot.transform.localScale = new Vector3(scaleOld.x, scaleOld.y * scaleY, 1f);
            }
            else if (Convert.ToSingle(Screen.height) / Screen.width < Convert.ToSingle(ManualHeight) / ManualWidth)
            {
                int manualWidth = Mathf.RoundToInt(Convert.ToSingle(ManualHeight) / Screen.height * Screen.width);
                var scaleX = manualWidth * 1.0f / ManualWidth;
                traRoot.transform.localScale = new Vector3(scaleOld.x, scaleOld.y / scaleX, 1f);
            }
        }
        else
        {
            PPP.pppShow();
        }
    }

    public static void addButtonClickEffect(Button button)
    {
        if (button.transition == Selectable.Transition.ColorTint || button.transition == Selectable.Transition.None)
        {
            GameObject btn = button.gameObject;
            EventTriggerListener.Get(btn).onDown = (bt) =>
            {
                bt.transform.DOKill();
                bt.transform.DOScale(((Vector3)PPP.getPropertyOriginal(bt.transform, "localScale")) * 0.9f, 0.1f);
            };

            EventTriggerListener.Get(btn).onUp = (bt) =>
            {
                bt.transform.DOKill();
                bt.transform.DOScale((Vector3)PPP.getPropertyOriginal(bt.transform, "localScale"), 0.1f);
            };
        }
    }

    void checkName(Transform tra, System.Reflection.FieldInfo item)
    {
        if (tra == null)
        {
            return;
        }
        var strName = tra.name;
        var type = item.FieldType;
        if (type == typeof(Button))
        {
            if (strName.StartsWith("btn") == false)
            {
                PPP.pppShow(true, this.name + ":" + strName + " 命名不规范,Button 以 btn 开头");
            }
        }
        else if (type == typeof(Image))
        {
            if (strName.StartsWith("img") == false)
            {
                PPP.pppShow(true, this.name + ":" + strName + " 命名不规范,Image 以 img 开头");
            }
        }
        else if (type == typeof(Text))
        {
            if (strName.StartsWith("txt") == false)
            {
                PPP.pppShow(true, this.name + ":" + strName + " 命名不规范,Text 以 txt 开头");
            }
        }
        else if (type == typeof(Toggle))
        {
            if (strName.StartsWith("tog") == false)
            {
                PPP.pppShow(true, this.name + ":" + strName + " 命名不规范,Toggle 以 tog 开头");
            }
        }
        else if (type == typeof(Transform))
        {
            if (strName.StartsWith("tra") == false)
            {
                PPP.pppShow(true, this.name + ":" + strName + " 命名不规范,Transform 以 tra 开头");
            }
        }
        else if (type == typeof(GameObject))
        {
            if (strName.StartsWith("obj") == false)
            {
                PPP.pppShow(true, this.name + ":" + strName + " 命名不规范,GameObject 以 obj 开头");
            }
        }
        else if (type.BaseType == typeof(Array))
        {
            if (strName.StartsWith("arr") == false)
            {
                PPP.pppShow(true, this.name + ":" + strName + " 命名不规范,数组 以 arr 开头");
            }
        }
    }

    public static void setActive(MonoBehaviour m, bool active)
    {
        if (m != null)
        {
            m.gameObject.SetActive(active);
        }
    }
}

 

