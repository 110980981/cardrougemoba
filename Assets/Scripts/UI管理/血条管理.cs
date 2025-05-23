using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 血条管理 : MonoBehaviour
{
    private static 血条管理 _实例;
    public static 血条管理 实例
    {
        get
        {
            if(_实例==null)
            {
                _实例=FindFirstObjectByType<血条管理>();
                if(_实例==null)
                {
                    GameObject 新的血条管理 = new GameObject("血条管理");
                    _实例=新的血条管理.AddComponent<血条管理>();
                    DontDestroyOnLoad(新的血条管理);
                }
            }
            return _实例;
        }
    }
    public GameObject[] 爱心;
    int 激活的爱心个数;
    private int _血量个数;
    public int 血量个数
    {
        get { return _血量个数; }
        set
        {
            _血量个数 = value;
            while(激活的爱心个数!=_血量个数)
            {
                if(激活的爱心个数<_血量个数)
                {
                    激活的爱心个数++;
                    爱心[激活的爱心个数-1].SetActive(true);
                }
                else
                {
                    激活的爱心个数--;
                    爱心[激活的爱心个数].SetActive(false);
                }
            }
        }
    }
    public void 初始化血量(int 新的血量)
    {
        GameObject 爱心图标 = 爱心[0];
        爱心=new GameObject[新的血量];
        爱心[0]=爱心图标;
        for(int i=1;i<新的血量;i++)
        {
            爱心[i]=Instantiate(爱心图标,transform);
            爱心[i].transform.localPosition=爱心[0].transform.localPosition+new Vector3(80*i,0,0);
        }
        血量个数=新的血量;
    }
}
