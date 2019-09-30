using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

[AddComponentMenu("COMM_COMPONENT/ITEM_HIDDEN")]
class TempSc_HiddenItem : MonoBehaviour
{
    public bool useAni = false;
    public float hideSec = 5;

    void Start()
    {
        HiddenItem th = gameObject.AddComponent<HiddenItem>();
        th.useAni = useAni;
        th.hideSec = hideSec;
        Destroy(this);
    }
}

