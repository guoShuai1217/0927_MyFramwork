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
using UnityEngine;
using UnityEngine.UI;

public class JustForTest : MonoBehaviour
{
    public Transform content;

    public List<TestData> dataList = new List<TestData>();
    private void Start()
    {
        dataList.Add(new TestData("标题1", "内容1", "时间1"));
        dataList.Add(new TestData("标题2", "内容2", "时间2"));
        dataList.Add(new TestData("标题3", "内容3", "时间3"));
        dataList.Add(new TestData("标题4", "内容4", "时间4"));
        dataList.Add(new TestData("标题5", "内容5", "时间5"));
        dataList.Add(new TestData("标题6", "内容6", "时间6"));
        dataList.Add(new TestData("标题7", "内容7", "时间7"));
        dataList.Add(new TestData("标题8", "内容8", "时间8"));
        dataList.Add(new TestData("标题9", "内容9", "时间9"));
        dataList.Add(new TestData("标题10", "内容10", "时间10"));

        ShowItem(dataList);
    }

    private void ShowItem(List<TestData> dataList)
    {
        ItemManager.Instance.ClearParent(content);

        foreach (TestData item in dataList)
        {
            ItemTest tmp = ItemManager.Instance.Dequeue<ItemTest>();
            tmp.transform.SetParent(content);
            tmp.transform.SetAsLastSibling();
            tmp.clickAction = ClickItem;
            tmp.deleteAction = DeleteItem;
            tmp.SetContent(item.title,item.content,item.time);

        }
    }

    private void ClickItem(ItemBase obj)
    {
        ItemTest test = obj as ItemTest;
        Debug.Log("点击了 : " + test.txt_Title.text);
    }

    private void DeleteItem(ItemBase obj)
    {
        ItemTest test = obj as ItemTest;
        Debug.Log("删除了 : " + test.txt_Title.text);
        ItemManager.Instance.Enqueue(test);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ItemManager.Instance.ClearParent(content);

            ItemTest tmp = ItemManager.Instance.Dequeue<ItemTest>();
            tmp.transform.SetParent(content);
            tmp.transform.SetAsLastSibling();
            tmp.clickAction = ClickItem;
            tmp.deleteAction = DeleteItem;
            tmp.SetContent("111", "111", "111");

            ItemTest2 tmp1 = ItemManager.Instance.Dequeue<ItemTest2>();
            tmp1.transform.SetParent(content);
            tmp1.transform.SetAsLastSibling();
            tmp1.SetContent("哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈");
        }
    }
}

public class TestData
{
    public string title;
    public string content;
    public string time;
    public TestData(string title,string content,string time)
    {
        this.title = title;
        this.content = content;
        this.time = time;
    }
}