using System;
using MuGame;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("COMM_COMPONENT/TriggerFB")]
 class Trigger_jdzc : MonoBehaviour {
    // Use this for initialization
    void Start () {
        uint levelID = MapModel.getInstance().curLevelId;
        switch (levelID)
        {
            case 9000:
                change_judian cj = gameObject.AddComponent<change_judian>();
                jdzc_zhandian jz = gameObject.AddComponent<jdzc_zhandian>();
                Destroy(this);
                break;
            case 8000:
                city_War cw = gameObject.AddComponent<city_War>();
                Destroy(this);
                break;
        }
    }
	
}
