using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 保持贴图朝向不变 : MonoBehaviour
{
    Quaternion lastParentRotation;
    // Start is called before the first frame update
    void Start()
    {
        lastParentRotation = transform.parent.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.rotation) *
            lastParentRotation *
            transform.localRotation;
        lastParentRotation = transform.parent.rotation;
    }
}
