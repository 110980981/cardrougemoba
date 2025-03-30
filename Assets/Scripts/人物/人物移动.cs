using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GifImporter;

public class 人物移动 : MonoBehaviour
{
    public Rigidbody2D 刚体;
    public void 移动(Vector2 移动方向,float 移动加速度)
    {
        float 最大移动速度 = 移动加速度*2;
        刚体.velocity += 移动方向.normalized * 移动加速度;
        刚体.drag = 移动加速度*2;
        if(刚体.velocity.magnitude > 最大移动速度)刚体.velocity = 刚体.velocity.normalized * 最大移动速度;
    }
}
