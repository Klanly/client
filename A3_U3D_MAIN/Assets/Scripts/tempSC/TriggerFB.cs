using System;
using MuGame;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("COMM_COMPONENT/TriggerFB")]
class TriggerFB : MonoBehaviour
{
    public int triggerTimes = 1;
    void Start()
    {
        uint levelID = MapModel.getInstance().curLevelId;
        switch (levelID)
        {
            case 108:
                WdsyOpenDoor cp = gameObject.AddComponent<WdsyOpenDoor>();
                cp.triggerTimes = triggerTimes;
                Destroy(this); break;
            case 109:
                OpenDoor109 od = gameObject.AddComponent<OpenDoor109>();
                od.triggerTimes = triggerTimes;
                Destroy(this); break;
            case 110:
                OpenDoor110 od1 = gameObject.AddComponent<OpenDoor110>();
                od1.triggerTimes = triggerTimes;
                Destroy(this); break;
            case 111:
                OpenDoor111 od2 = gameObject.AddComponent<OpenDoor111>();
                od2.triggerTimes = triggerTimes;
                Destroy(this); break;
            default:
                break;
        }
    }

}

