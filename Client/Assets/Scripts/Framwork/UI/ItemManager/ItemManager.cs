/*
 *		Description: 管理类
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

public class ItemManager  
{
    private static ItemManager instance;
    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemManager();
            }
            return instance;
        }
    }

    private ItemManager()
    {
        pool = new ItemPool();
    }

    private ItemPool pool;

    /// <summary>
    /// 入池子
    /// </summary>
    /// <param name="item"></param>
    public void Enqueue(ItemBase item)
    {
        pool.Enqueue(item);
    }

    /// <summary>
    /// 入池子
    /// </summary>
    /// <param name="item"></param>
    public void Enqueue(params ItemBase[] item)
    {
        pool.Enqueue(item);
    }

    /// <summary>
    /// 出池子
    /// 约定 UI 的Item预制体都放到 Resources/ItemPrefabs文件夹下
    /// 约定 UI 的Item预制体和脚本名字保持一致
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Dequeue<T>() where T : ItemBase
    {
        T tmp = pool.Dequeue<T>();

        if (tmp == null)
        {
            T oo = Resources.Load<T>("ItemPrefabs/" + typeof(T).Name);
            tmp = GameObject.Instantiate(oo);
            tmp.name = tmp.name.Split('(')[0];
            pool.AddPool(tmp);
        }

        return tmp;
    }


    /// <summary>
    /// 清空父物体
    /// </summary>
    /// <param name="content"></param>
    public void ClearParent(Transform content)
    {
        ItemBase[] itemArr = content.GetComponentsInChildren<ItemBase>();
        for (int i = 0; i < itemArr.Length; i++)
        {
            Enqueue(itemArr[i]);
        }

        //for (int i = 0; i < content.childCount; i++)
        //{
        //    ItemBase tmp = content.GetChild(i).GetComponent<ItemBase>();

        //    if (tmp != null && tmp.gameObject.activeSelf)
        //        Enqueue(tmp);
        //}
    }
 




}