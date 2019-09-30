using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/HANDLE_SHAKE_CAMERA")]
class TempSc_ShakeCamera : MonoBehaviour
{
    public float second = 1;
    public int count = 5;
    public float strength=3;

    void Start()
    {
        TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
        th.type = 9;
        th.paramFloat = new List<float> { second, strength };
        th.paramInts = new List<int> { count };
        Destroy(this);
    }

}

