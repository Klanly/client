using System;
using MuGame;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;




[AddComponentMenu("COMM_COMPONENT/Trigger/MonsterDead")]
public class Trigger_MonsterDead : MonoBehaviour
{
    public int monId;
    public int killingNum;

    void Start()
    {
        GameEventTrigger cp = gameObject.AddComponent<GameEventTrigger>();
        cp.type = 1;
        cp.paramInts = new List<int> { monId, killingNum };
        Destroy(this);
    }


}

