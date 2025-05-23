using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 用于找全局变量的脚本 : MonoBehaviour
{
    private static 用于找全局变量的脚本 _实例;
    public static 用于找全局变量的脚本 实例
    {
        get
        {
            if (_实例 == null)
            {
                _实例 = GameObject.FindFirstObjectByType<用于找全局变量的脚本>();
                if (_实例 == null)
                {
                    _实例 = new GameObject("用于找全局变量的脚本").AddComponent<用于找全局变量的脚本>();
                    DontDestroyOnLoad(_实例);
                }
            }
            return _实例;
        }
    }
    private GameObject _玩家;
    public GameObject 玩家
    {
        get
        {
            if (_玩家 == null)
            {
                GameObject[] 玩家 = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < 玩家.Length; i++)
                {
                    if (玩家[i].GetComponent<人物控制器>()!=null)
                        if (玩家[i].GetComponent<人物控制器>().是否为该客户端操作的角色)
                            _玩家 = 玩家[i];
                }
            }
            return _玩家;
        }
    }
}
