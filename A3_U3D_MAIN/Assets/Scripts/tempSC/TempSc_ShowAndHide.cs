using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/HANDLE_SHOW_ABD_HIDE")]
class TempSc_ShowAndHide:MonoBehaviour
{
    public bool useAni = false;
    public float hideSec = 5;
    public GameObject target;

    void Start()
    {
        if (target != null)
        {
            target.SetActive(false);

            TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
            th.type = 8;
            th.paramGo = new List<GameObject> { target };

            HiddenItem hide = target.AddComponent<HiddenItem>();
            hide.useAni = useAni;
            hide.hideSec = hideSec;
        }

       
        Destroy(this);
    }
}

