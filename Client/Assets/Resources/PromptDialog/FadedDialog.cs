/*
 * 		Description: 渐变提示框
 *
 *  	CreatedBy:  guoShuai
 *
 *  	DataTime: 2019.04.11
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadedDialog : UIDialogBase
{

    public Text txt;
    public Text txt_Warning; // 内容
    public CanvasGroup cg;

    [SerializeField]
    [Range(0, 3)]
    private float showTime = 1f;

    private float timer = 0f;

    public override void Init()
    {
        base.Init();
        cg = txt_Warning.transform.GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }

    /// <summary>
    /// 第一个参数是 提示  , 第二个参数是 颜色
    /// </summary>
    /// <param name="sceneData"></param>
    public override void OnSceneActivated(params object[] sceneData)
    {
        base.OnSceneActivated(sceneData);       
        
        promptMessage(sceneData[0].ToString(), (Color)sceneData[1]);
    }

    /// <summary>
    /// 提示消息
    /// </summary>
    private void promptMessage(string text, Color color)
    {
        txt.text = text;
        txt_Warning.text = text;
        txt_Warning.color = color;
        //txt_Warning.color = Color.white;
        //txt_Warning.GetComponent<Text>().fontSize = 13;
        cg.alpha = 0;
        timer = 0;
        //做动画显示
        StartCoroutine(promptAnim());
    }

    /// <summary>
    /// 用来显示动画
    /// </summary>
    /// <returns></returns>
    IEnumerator promptAnim()
    {
        while (cg.alpha < 1f)
        {
            cg.alpha += Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }
        while (timer < showTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }

        OnBackPressed();
    }

    public override void OnSceneDeactivated()
    {
        base.OnSceneDeactivated();

        StopCoroutine(promptAnim());
    }


}
