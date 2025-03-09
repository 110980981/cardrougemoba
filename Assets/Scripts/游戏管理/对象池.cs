using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class 对象池 : MonoBehaviour
{
    public static 对象池 _实例;
    public static 对象池 实例
    {
        get
        {
            if (_实例 == null)
            {
                _实例 = FindFirstObjectByType<对象池>();
                if (_实例 == null)
                {
                    GameObject go = new GameObject("对象池");
                    _实例 = go.AddComponent<对象池>();
                }
                DontDestroyOnLoad(_实例);
            }
            return _实例;
        }
    }
    private Dictionary<string, Stack<GameObject>> 技能 = new Dictionary<string, Stack<GameObject>>();
    public GameObject 获取(string 技能类型,string 二级目录,GameObject 父级=null)
    {
        if (技能.ContainsKey(技能类型)==false)技能.Add(技能类型,new Stack<GameObject>());
        if (技能[技能类型].Count == 0)
        {
            if(父级==null)父级 = this.gameObject;
            GameObject 技能实例 = Instantiate(Resources.Load<GameObject>("Prefabs/"+二级目录+"/" + 技能类型.ToString()),父级.transform);
            return 技能实例;
        }
        else
        {
            GameObject 技能实例 = 技能[技能类型].Pop();
            技能实例.SetActive(true);
            return 技能实例;
        }
    }
    public void 回收(GameObject 技能实例,string 技能类型)
    {
        技能实例.SetActive(false);
        if(技能.ContainsKey(技能类型)==false)技能.Add(技能类型,new Stack<GameObject>());
        技能[技能类型].Push(技能实例);
    }
}