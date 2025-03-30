using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GifImporter;
using System;

public class 播放一次帧动画 : MonoBehaviour
{
    public void 播放序列帧(Gif gif,int 起始帧=0,int 结束帧=-1,Action 播放完成回调=null,float 播放速度=1)
    {
        if(结束帧==-1)结束帧=gif.Frames.Count-1;
        StartCoroutine(播放序列帧携程(gif,起始帧,结束帧,播放完成回调,播放速度));
    }
    IEnumerator 播放序列帧携程(Gif gif,int 起始帧,int 结束帧,Action 播放完成回调=null,float 播放速度=1)
    {
        int index = 起始帧;
        while (index <= 结束帧)
        {
            var frame = gif.Frames[index];
            transform.GetComponent<SpriteRenderer>().sprite = frame.Sprite;
            index++;
            yield return new WaitForSeconds(frame.DelayInMs / 1000f / 播放速度);
        }
        播放完成回调?.Invoke();
    }
}
