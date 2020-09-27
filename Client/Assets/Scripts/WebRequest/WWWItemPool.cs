/*
 *		Description: 存储 WWWItemBase 的对象池，不用频繁的new
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.02.03
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class WWWItemPool
{

    private static List<WWWItemBase> poolList;

    static WWWItemPool()
    {
        poolList = new List<WWWItemBase>();
    }

    /// <summary>
    /// 入池子
    /// </summary>
    /// <param name="item"></param>
    public static void AddWWWItem(WWWItemBase item)
    {
        poolList.Add(item);
    }

    /// <summary>
    /// 从池子里获取（只从池子里获取，不从池子里移除）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetWWWItem<T>() where T : WWWItemBase
    {
        if (poolList.Count == 0)
        {
            Debug.Log("pool is Empty");
            return null;
        }

        for (int i = 0; i < poolList.Count; i++)
        {
            T item = poolList[i] as T;
            if (item != null && item.IsFinish) // 池子里有，并且已经使用完成了（可以进行下一次复用了）
            {
                //Debug.Log("池子里有，不用 new 了");
                item.Init();
                return item;
            }
        }

        //Debug.Log("池子里没有，new 一个吧");
        return null;

    }

}

