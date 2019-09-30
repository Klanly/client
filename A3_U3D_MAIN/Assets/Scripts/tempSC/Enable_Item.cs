using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/HANDLE_ENABLE_ITEM")]
public class Enable_Item : MonoBehaviour
{
    public List<GameObject> item;

    void Start()
    {
        if (item == null)
            return;

        TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
        th.type = 2;
        th.paramGo = item;
        Destroy(this);
    }
}


