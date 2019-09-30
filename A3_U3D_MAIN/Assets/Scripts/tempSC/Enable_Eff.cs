using System;
using MuGame;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[AddComponentMenu("COMM_COMPONENT/显示特效")]
public class Enable_Eff : MonoBehaviour
{
    public GameObject item;
    public float sec = 0;
    void Start()
    {
        if (item == null)
            return;

        if (sec <= 0)
            return;

        TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
        th.type = 10;
        th.paramGo = new List<GameObject> { item };
        th.paramFloat = new List<float> { sec };
        Destroy(this);
    }
}