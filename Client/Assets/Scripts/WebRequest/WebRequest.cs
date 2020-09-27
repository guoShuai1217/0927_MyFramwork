/*
 *		Description: 程序对外的入口，用户只需要知道这个类就可以了
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.02.03
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[DisallowMultipleComponent]
public class WebRequest :MonoBehaviour
{

  
    // 存储请求的数据队列
    private Queue<WWWItemBase> que = new Queue<WWWItemBase>();

    // 队列里的数据是否都请求完成了？
    private bool isFinish = true;

 
 
    /// <summary>
    /// 把需要请求的数据 加入到队列里
    /// </summary>
    /// <param name="wwwItem"></param>
    public void AddTask(WWWItemBase wwwItem)
    {
        que.Enqueue(wwwItem);
       
        if(que.Count == 1 && isFinish)
        {
            isFinish = false;
            StartCoroutine(DownLoad());
        }
    }

    private IEnumerator DownLoad()
    {
        while (que.Count > 0)
        {
            WWWItemBase item = que.Dequeue();
            yield return item.DownLoad();
        }

        isFinish = true;
    }
}

