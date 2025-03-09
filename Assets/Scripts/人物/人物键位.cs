using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 人物键位 : MonoBehaviour
{
    public Dictionary<string,KeyCode> _人物键位 = new Dictionary<string, KeyCode>()
    {
        {"上",KeyCode.W},
        {"下",KeyCode.S},
        {"左",KeyCode.A},
        {"右",KeyCode.D},
    };
}
