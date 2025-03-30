using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Data;
using Unity.VisualScripting;

public class 卡牌 : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    string 卡牌名,文本描述;
    public Vector3 初始大小,初始位置;
    public GameObject 卡面;
    GameObject 玩家;
    bool 是否跟随鼠标,正在弃牌;
    public void Init(int id)
    {
        玩家 = GameObject.FindWithTag("Player");
        初始大小 = 卡面.transform.localScale;
        transform.position = Vector2.one * 3000;
        初始位置 = 卡面.transform.position;
        卡面.GetComponent<SpriteRenderer>().sortingOrder = 0; // 恢复默认值
        卡面.transform.localScale = 初始大小;
        卡面.transform.position = 初始位置;
        是否跟随鼠标 = false;
        正在弃牌 = false;
        卡牌名 = 根据id获取卡牌信息.实例.获取卡牌信息(id)[0];
        文本描述 = 卡牌名 + "\n" + 根据id获取卡牌信息.实例.获取卡牌信息(id)[1];
        if(根据id获取卡牌信息.实例.获取卡牌图片(id)!=null)
            卡面.GetComponent<SpriteRenderer>().sprite = 根据id获取卡牌信息.实例.获取卡牌图片(id);
    }
    public void Init_加入手牌的时候()
    {
        初始大小 = 卡面.transform.localScale;
        transform.position = Vector2.one * 3000;
        初始位置 = 卡面.transform.position;
        卡面.GetComponent<SpriteRenderer>().sortingOrder = 0; // 恢复默认值
        是否跟随鼠标 = false;
        正在弃牌 = false;
        卡面.transform.localScale = 初始大小;
        卡面.transform.position = 初始位置;
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
        卡牌文本框.Instance.显示文本(文本描述);
        卡面.transform.DOScale(1.5f*初始大小, 0.2f);
        卡面.transform.DOMove(初始位置 + new Vector3(0, 1f, 0), 0.2f);
        卡面.GetComponent<SpriteRenderer>().sortingOrder = 10; // 设置为较高的值
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(正在弃牌)return;
        if(!是否跟随鼠标)
        {
            卡面.transform.DOScale(初始大小, 0.2f);
            卡面.transform.DOMove(初始位置, 0.2f);
            卡牌文本框.Instance.隐藏文本();
            卡面.GetComponent<SpriteRenderer>().sortingOrder = 0; // 恢复默认值
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
        卡面.transform.DOScale(初始大小, 0.2f);
        GameObject 距离最近的牌 = 寻找距离最近的牌();
        if(卡面.transform.position.y-初始位置.y>3f)
        {
            正在弃牌 = true;
            玩家.GetComponent<人物技能>().释放技能(卡牌名);
            卡面.transform.GetComponent<SpriteRenderer>().DOFade(0, 0.2f).OnComplete(() => {
                卡面.transform.localScale = 初始大小;
                卡面.transform.localPosition = Vector3.zero;
                transform.position = Vector2.one * 3000;
                卡面.GetComponent<SpriteRenderer>().sortingOrder = 0; // 恢复默认值
                卡面.transform.GetComponent<SpriteRenderer>().DOFade(1, 0.05f).OnComplete(() => {
                    牌库手牌墓地管理.实例.弃牌(this.gameObject);
                    正在弃牌 = false;
                });
            });
        }
        else if(距离最近的牌!=this.gameObject)
        {
            string 组合技 = 玩家.GetComponent<人物技能>().获取组合技(距离最近的牌.GetComponent<卡牌>().卡牌名,卡牌名);
            if(组合技!="没有这个组合技")
            {
                正在弃牌 = true;
                距离最近的牌.GetComponent<卡牌>().正在弃牌 = true;
                玩家.GetComponent<人物技能>().释放技能(组合技);
                卡面.transform.GetComponent<SpriteRenderer>().DOFade(0, 0.2f).OnComplete(() => {
                    卡面.transform.localScale = 初始大小;
                    卡面.transform.localPosition = Vector3.zero;
                    距离最近的牌.transform.GetComponent<卡牌>().卡面.transform.localScale = 距离最近的牌.GetComponent<卡牌>().初始大小;
                    距离最近的牌.transform.GetComponent<卡牌>().卡面.transform.localPosition = Vector3.zero;
                    transform.position = Vector2.one * 3000;
                    距离最近的牌.transform.position = Vector2.one * 3000;
                    卡面.GetComponent<SpriteRenderer>().sortingOrder = 0; // 恢复默认值
                    卡面.transform.GetComponent<SpriteRenderer>().DOFade(1, 0.05f).OnComplete(() => {
                        牌库手牌墓地管理.实例.连续弃牌(new List<GameObject>{this.gameObject,距离最近的牌});
                        卡牌文本框.Instance.隐藏文本();
                        正在弃牌 = false;
                        距离最近的牌.GetComponent<卡牌>().正在弃牌 = false;
                    });
                });
            }
            else
            {
                卡面.transform.DOMove(初始位置, 0.2f);
                牌库手牌墓地管理.实例.展示手牌();
            }
        }
        else
        {
            卡面.transform.DOMove(初始位置, 0.2f);
            牌库手牌墓地管理.实例.展示手牌();
        }
        if(是否跟随鼠标)
        {
            是否跟随鼠标 = false;
            MonoMgr.GetInstance().RemoveUpdateListener(跟随鼠标);
        }
    }
    public GameObject 寻找距离最近的牌()
    {
        GameObject 距离最近的牌 = transform.gameObject;
        float 最近距离 = Vector3.Distance(transform.position, 初始位置);
        for(int i = 0; i < 牌库手牌墓地管理.实例.手牌.Count; i++)
        {
            if(牌库手牌墓地管理.实例.手牌[i]==this.gameObject)continue;
            float 距离 = Vector3.Distance(牌库手牌墓地管理.实例.手牌[i].transform.GetComponent<卡牌>().初始位置,transform.position);
            if(距离 < 最近距离)
            {
                距离最近的牌 = 牌库手牌墓地管理.实例.手牌[i];
                最近距离 = 距离;
            }
        }
        return 距离最近的牌;
    }
}