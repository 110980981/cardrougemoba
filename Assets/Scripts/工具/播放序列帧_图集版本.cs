using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class 播放序列帧_图集版本 : MonoBehaviour
{
    public void 播放序列帧(SpriteAtlas 序列帧,int 起始帧=0,int 结束帧=-1,string 前缀=null,Action 播放完成回调=null)
    {
        if(结束帧==-1)结束帧=序列帧.spriteCount-1;
        StartCoroutine(播放序列帧携程(序列帧,起始帧,结束帧,前缀,播放完成回调));
    }
    IEnumerator 播放序列帧携程(SpriteAtlas 序列帧,int 起始帧,int 结束帧,string 前缀=null,Action 播放完成回调=null)
    {
        for(int i=起始帧;i<=结束帧;i++)
        {
            GetComponent<SpriteRenderer>().sprite = 序列帧.GetSprite(前缀+i);
            yield return new WaitForSeconds(0.016f);
        }
        播放完成回调?.Invoke();
    }
}
