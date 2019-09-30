

using System;
using MuGame;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;




[AddComponentMenu("COMM_COMPONENT/Trigger/delay")]
public class Trigger_delay : MonoBehaviour
{
    public bool run = false;
    public float sec;

    void Start()
    {
        GameEventTrigger cp = gameObject.AddComponent<GameEventTrigger>();
        cp.type = 3;
        cp.paramFloat = new List<float> { sec};
        if (run)
            cp.onTriggerHanlde();
        Destroy(this);
    }


}

