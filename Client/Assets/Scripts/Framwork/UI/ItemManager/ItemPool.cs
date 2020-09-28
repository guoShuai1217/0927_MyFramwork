/*
 *		Description: 管理UI Item的对象池
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPool  
{
    private List<ItemBase> poolList;

    public ItemPool()
    {
        poolList = new List<ItemBase>();
    }


    /// <summary>
    /// 用于第一次加到池子里(实例化的时候用)
    /// </summary>
    /// <param name="item"></param>
    public void AddPool(ItemBase item)
    {
        poolList.Add(item);
        Debug.LogWarning("池子数量为 : " + poolList.Count);
    }

    /// <summary>
    /// 放回池子
    /// </summary>
    /// <param name="item"></param>
    public void Enqueue(ItemBase item)
    {
        item.Hide();     
    }

    /// <summary>
    /// 放回池子
    /// </summary>
    /// <param name="item"></param>
    public void Enqueue(params ItemBase[] item)
    {
        for (int i = 0; i < item.Length; i++)
        {
            Enqueue(item[i]);
        }
    }


    /// <summary>
    /// 从池子拿出
    /// </summary>
    /// <param name="item"></param>
    public T Dequeue<T>() where T : ItemBase
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            T tmp = poolList[i] as T;
            if (tmp != null && !tmp.gameObject.activeSelf)
            {
                tmp.Init();
                return tmp;
            }
        }

        return null;
    }
}