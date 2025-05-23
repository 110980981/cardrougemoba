using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;



public class 人物受击 : MonoBehaviour,受击接口
{
    public class 受击前事件参数类 : EventArgs
    {
        public Vector2 击退方向;
        public int 伤害;
    }
    public 受击前事件参数类 受击前事件参数 = new 受击前事件参数类();
    public event EventHandler<受击前事件参数类> 受击前事件;
    public void 受击(Vector2 击退方向, object 伤害)
    {
        {
            受击前事件参数.击退方向 = 击退方向;
            受击前事件参数.伤害 = (int)伤害;
        }
        if (受击前事件 != null) 受击前事件(this, 受击前事件参数);
        {
            击退方向 = 受击前事件参数.击退方向;
            伤害 = 受击前事件参数.伤害;
        }
        transform.DOMove(transform.position + (Vector3)击退方向, 0.2f);
        GetComponent<人物属性>().血量 -= Mathf.Max(0, (int)伤害 - GetComponent<人物属性>().固定格挡伤害);
    }
}
