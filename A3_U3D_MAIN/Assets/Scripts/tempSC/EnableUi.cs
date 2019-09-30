

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/ui控制")]
public class EnableUi : MonoBehaviour
{
    public bool floatUI = true;
    void Start()
    {
        

        TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
        th.type = 14;
        th.paramBool = floatUI;
        Destroy(this);
    }
}
