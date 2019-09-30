using System;
using MuGame;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("COMM_COMPONENT/Trigger/Userborn")]
public class Trigger_UserBorn : MonoBehaviour
{
    public bool run = false;

    void Start()
    {
        GameEventTrigger cp = gameObject.AddComponent<GameEventTrigger>();
        cp.type = 2;

        if (run)
            cp.onTriggerHanlde();

        //   cp.paramInts = new List<int> { monId, killingNum };
        Destroy(this);
    }


}
