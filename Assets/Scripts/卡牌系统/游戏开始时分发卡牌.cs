using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 游戏开始时分发卡牌 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        牌库手牌墓地管理.实例.加入牌库(0);
        牌库手牌墓地管理.实例.加入牌库(1);
        牌库手牌墓地管理.实例.加入牌库(2);
        牌库手牌墓地管理.实例.加入牌库(3);
        牌库手牌墓地管理.实例.加入牌库(4);
        牌库手牌墓地管理.实例.加入牌库(5);
        牌库手牌墓地管理.实例.加入牌库(6);
        牌库手牌墓地管理.实例.加入牌库(7);
        牌库手牌墓地管理.实例.加入牌库(8);
    }
}
