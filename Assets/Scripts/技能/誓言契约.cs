using UnityEngine;

public class 誓言契约 : MonoBehaviour
{
    void Start()//暂时实现方法
    {
        GameObject[] 敌人 = GameObject.FindGameObjectsWithTag("敌人");
        敌人[0].transform.tag = "Player";
        敌人[0].transform.gameObject.layer = 10;
        对象池.实例.回收(gameObject,"誓言契约");
    }
}