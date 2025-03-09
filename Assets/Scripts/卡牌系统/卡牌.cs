using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Data;

public class 卡牌 : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    string 卡牌名,文本描述;
    public Vector3 初始大小,初始位置;
    public GameObject 卡名;
    public GameObject 卡面;
    GameObject 玩家;
    bool 是否跟随鼠标,是否进入,正在弃牌;
    public void Init(int id)
    {
        玩家 = GameObject.FindWithTag("Player");
        初始大小 = transform.localScale;
        transform.position = Vector2.one * 3000;
        初始位置 = transform.position;
        卡牌名 = 根据id获取卡牌信息.实例.获取卡牌信息(id)[0];
        文本描述 = 根据id获取卡牌信息.实例.获取卡牌信息(id)[1];
        卡名.GetComponent<TextMeshProUGUI>().text = 卡牌名;
    }
    void 跟随鼠标()
    {
        if(正在弃牌)return;
        Vector3 鼠标位置 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        鼠标位置.z = 0;
        transform.position = 鼠标位置+new Vector3(0,1,0);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(正在弃牌)return;
        if(!是否进入)
        {
            是否进入 = true;
            卡牌文本框.Instance.显示文本(文本描述);
            transform.DOScale(1.5f*初始大小, 0.2f);
            transform.DOMove(初始位置 + new Vector3(0, 1f, 0), 0.2f);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(正在弃牌)return;
        if(是否进入&&(!是否跟随鼠标))
        {
            是否进入 = false;
            transform.DOScale(初始大小, 0.2f);
            transform.DOMove(初始位置, 0.2f);
            卡牌文本框.Instance.隐藏文本();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(正在弃牌)return;
        if(!是否跟随鼠标)
        {
            是否跟随鼠标 = true;
            卡牌文本框.Instance.隐藏文本();
            MonoMgr.GetInstance().AddUpdateListener(跟随鼠标);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(初始大小, 0.2f);
        if(transform.position.y-初始位置.y>3f)
        {
            正在弃牌 = true;
            玩家.GetComponent<人物技能>().释放技能(卡牌名);
            卡面.transform.GetComponent<SpriteRenderer>().DOFade(0, 0.2f).OnComplete(() => {
                牌库手牌墓地管理.实例.弃牌(this.gameObject);
                卡面.transform.GetComponent<SpriteRenderer>().DOFade(1, 0.2f);
                正在弃牌 = false;
            });
        }
        else
        {
            transform.DOMove(初始位置, 0.2f);
        }
        if(是否跟随鼠标)
        {
            是否跟随鼠标 = false;
            是否进入 = false;
            MonoMgr.GetInstance().RemoveUpdateListener(跟随鼠标);
        }
    }
}
