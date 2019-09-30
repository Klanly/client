using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;

[AddComponentMenu("COMM_COMPONENT/BREAK_BRIDGE")]
class BreakBridge : MonoBehaviour
{
    public GameObject obj;
    void Start()
    {
        if (obj !=null)
        {
          
            TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
            th.type = 5;
            th.paramGo = new List<GameObject> { obj};

        }


        Destroy(this);
    }

}

