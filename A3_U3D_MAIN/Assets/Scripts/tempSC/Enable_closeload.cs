using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;

[AddComponentMenu("COMM_COMPONENT/CLOSELOAD")]
class Enable_closeload : MonoBehaviour
{
    void Start()
    {
        TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
        th.type = 17;
 
        Destroy(this);
    }

}

