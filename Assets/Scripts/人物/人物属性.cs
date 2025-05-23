using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 人物属性 : MonoBehaviour
{
    public int 血量上限=3;
    public int _血量;
    public int 血量
    {
        get { return _血量; }
        set
        {
            _血量 = Mathf.Min(value,血量上限);
            血条管理.实例.血量个数 = _血量;
        }
    }
    public int 固定格挡伤害;
    private float _伤害 = 1;
    public float 伤害
    {
        get
        {
            if (临时伤害加成 > 0.000001)
            {
                float 最终伤害 = _伤害 * (1 + 临时伤害加成) + 固定伤害加成;
                临时伤害加成 = 0;
                return 最终伤害;
            }
            return _伤害;
        }
    }
    public float 临时伤害加成;
    public float 固定伤害加成;
    public float 移动速度 = 5;
    private float _冲撞伤害;
    public float 冲撞伤害
    {
        get
        {
            float 最终伤害 = _冲撞伤害 * (1 + 临时伤害加成);
            临时伤害加成 = 0;
            return 最终伤害;
        }
        set
        {
            _冲撞伤害 = value;
        }
    }
    public bool 是否无敌 = false;
    public int 无敌时间=3;
}