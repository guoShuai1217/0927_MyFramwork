/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PPP
{

    public static Dictionary<int, Dictionary<string, object>> dicProperty = new Dictionary<int, Dictionary<string, object>>();//<GameObject hashcode,<

    public static void pppShow(bool err = true, object obj = null)
    {
        if (obj == null)
        {
            obj = "";
        }
        var st = new System.Diagnostics.StackTrace(1, true);
        string strMsg = st.GetFrame(0).GetFileName() + ":" + st.GetFrame(0).GetFileLineNumber() + ":" + obj;
        if (err)
        {
            strMsg = "pppErr:" + strMsg;
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
            Debug.LogWarning(strMsg);
#else
			//string userid=Game.Instance.playerId.ToString();
			//string srtMsg = st.GetFrame(0).GetFileName() + ":" + st.GetFrame(0).GetFileLineNumber() + ":" + obj;
			//DebugLogHttp.Instance.GetErrorData(userid, srtMsg);
			Debug.LogWarning (strMsg);
#endif
        }
        else
        {
            strMsg = "ppp:" + strMsg;
            Debug.LogFormat("{0}", strMsg);
        }

    }



    public static Type GetType(string TypeName)
    {
        var type = Type.GetType(TypeName);
        if (type != null)
        {
            return type;
        }
        var assemblyName = TypeName.Substring(0, TypeName.LastIndexOf('.'));
        var assembly = Assembly.Load(assemblyName);
        //var assembly = Assembly.LoadWithPartialName( assemblyName );
        if (assembly == null)
        {
            return null;
        }
        type = assembly.GetType(TypeName);
        //		if (type==null) {
        //			if (TypeName=="UnityEngine.UI.Text") {
        //				return typeof(UnityEngine.UI.Text);
        //			}
        //			else if (TypeName=="UnityEngine.UI.Button") {
        //				return typeof(UnityEngine.UI.Button);
        //			}
        //			else if (TypeName=="UnityEngine.UI.Toggle") {
        //				return typeof(UnityEngine.UI.Toggle);
        //			}
        //			else if (TypeName=="UnityEngine.UI.Image") {
        //				return typeof(UnityEngine.UI.Image);
        //			}
        //		}
        return type;
    }




    public static void setProperty(object obj, string strPropertyName, object value)
    {
        var hashCodeObj = obj.GetHashCode();
        if (dicProperty.ContainsKey(hashCodeObj) == false)
        {
            dicProperty.Add(hashCodeObj, new Dictionary<string, object>());
        }

        var type = obj.GetType();
        var propertyInfo = type.GetProperty(strPropertyName);
        if (propertyInfo != null)
        {
            if (dicProperty[hashCodeObj].ContainsKey(strPropertyName) == false)
            {
                dicProperty[hashCodeObj].Add(strPropertyName, propertyInfo.GetValue(obj, null));
            }
            propertyInfo.SetValue(obj, value, null);
        }
        else
        {
            PPP.pppShow();
        }
    }

    public static object getPropertyOriginal(object obj, string strPropertyName)
    {
        var hashCodeObj = obj.GetHashCode();
        if (dicProperty.ContainsKey(obj.GetHashCode()) == false)
        {
            dicProperty.Add(hashCodeObj, new Dictionary<string, object>());
        }
        var type = obj.GetType();
        var propertyInfo = type.GetProperty(strPropertyName);
        object ret = null;
        if (propertyInfo != null)
        {
            if (dicProperty[hashCodeObj].ContainsKey(strPropertyName))
            {
                ret = dicProperty[hashCodeObj][strPropertyName];
            }
            else
            {
                dicProperty[hashCodeObj].Add(strPropertyName, propertyInfo.GetValue(obj, null));
                ret = propertyInfo.GetValue(obj, null);
            }
        }
        else
        {
            PPP.pppShow();
        }
        return ret;
    }
}